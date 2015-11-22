using JetBrains.Annotations;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hyperspace.Redis
{
    public abstract class RedisBatch : IDisposable
    {
        protected internal RedisBatch(RedisContext context)
        {
            Check.NotNull(context, nameof(context));

            Context = context;
            Batch = context.Database.CreateBatch();
        }

        protected internal RedisBatch(RedisContext context, IBatch batch)
        {
            Check.NotNull(context, nameof(context));
            Check.NotNull(batch, nameof(batch));

            Context = context;
            Batch = batch;
        }

        protected internal RedisContext Context { get; }
        protected internal IBatch Batch { get; }

        public void Execute()
        {
            Batch.Execute();
        }

        #region IDisposable

        void IDisposable.Dispose()
        {
        }

        #endregion

    }

    public class RedisBatch<TContext> : RedisBatch where TContext : RedisContext
    {
        protected internal RedisBatch(TContext context) : base(context)
        {
        }

        protected internal RedisBatch(TContext context, IBatch batch) : base(context, batch)
        {
        }

        protected internal new TContext Context
        {
            get { return (TContext)base.Context; }
        }

        public RedisBatch<TContext> Done([NotNull] Action<TContext> commands)
        {
            Check.NotNull(commands, nameof(commands));

            return this;
        }

        public RedisBatch<TContext, TResults> Done<TResults>([NotNull] Func<TContext, TResults> commands)
        {
            Check.NotNull(commands, nameof(commands));

            var results = commands(Context);

            return new RedisBatch<TContext, TResults>(this, results);
        }

        public RedisBatch<TContext, IEnumerable<TResult>> Done<TResult>([NotNull] Func<TContext, IEnumerable<TResult>> commands)
        {
            Check.NotNull(commands, nameof(commands));

            var results = commands(Context);

            return new RedisBatch<TContext, IEnumerable<TResult>>(this, results);
        }

    }

    public class RedisBatch<TContext, TResults> : RedisBatch<TContext> where TContext : RedisContext
    {
        protected internal RedisBatch(RedisBatch<TContext> batch, TResults results) : base(batch.Context, batch.Batch)
        {
            Results = results;
        }

        protected TResults Results { get; }

        public new TResults Execute()
        {
            Batch.Execute();
            return Results;
        }

        public void Execute(Action<TResults> executed)
        {
            Batch.Execute();
            executed?.Invoke(Results);
        }

    }

    public abstract class RedisTransaction : IDisposable
    {
        protected internal RedisTransaction([NotNull] RedisContext context)
        {
            Check.NotNull(context, nameof(context));

            Context = context;
            Transaction = context.Database.CreateTransaction();
        }

        protected internal RedisTransaction([NotNull] RedisContext context, [NotNull] ITransaction transaction)
        {
            Check.NotNull(context, nameof(context));
            Check.NotNull(transaction, nameof(transaction));

            Context = context;
            Transaction = transaction;
        }

        protected internal RedisContext Context { get; }

        protected internal ITransaction Transaction { get; }

        public RedisTransaction When([NotNull] Action<RedisCondition> condition)
        {
            Check.NotNull(condition, nameof(condition));

            condition.Invoke(new RedisCondition(Transaction));
            return this;
        }

        public bool Execute()
        {
            return Transaction.Execute();
        }

        public Task<bool> ExecuteAsync()
        {
            return Transaction.ExecuteAsync();
        }

        #region IDisposable

        void IDisposable.Dispose()
        {
        }

        #endregion

    }

    public class RedisTransaction<TContext> : RedisTransaction where TContext : RedisContext
    {
        protected internal RedisTransaction([NotNull] TContext context) : base(context)
        {
        }

        protected internal RedisTransaction([NotNull] TContext context, [NotNull] ITransaction transaction) : base(context, transaction)
        {
        }

        protected internal new TContext Context
        {
            get { return (TContext)base.Context; }
        }

        protected internal Type ResultsType { get; set; }

        public new RedisTransaction<TContext> When([NotNull] Action<RedisCondition> condition)
        {
            Check.NotNull(condition, nameof(condition));

            condition.Invoke(new RedisCondition(Transaction));
            return this;
        }

        public RedisTransaction<TContext> Done([NotNull] Action<TContext> commands)
        {
            Check.NotNull(commands, nameof(commands));

            return this;
        }

        public RedisTransaction<TContext, TResults> Done<TResults>([NotNull] Func<TContext, TResults> commands)
        {
            Check.NotNull(commands, nameof(commands));

            if (ResultsType != null)
                throw new InvalidOperationException();
            ResultsType = typeof(TResults);

            var results = commands(Context);

            return new RedisTransaction<TContext, TResults>(this, results);
        }

        public RedisTransaction<TContext, IEnumerable<TResult>> Done<TResult>([NotNull] Func<TContext, IEnumerable<TResult>> commands)
        {
            Check.NotNull(commands, nameof(commands));

            if (ResultsType != null)
                throw new InvalidOperationException();
            ResultsType = typeof(IEnumerable<TResult>);

            var results = commands(Context);

            return new RedisTransaction<TContext, IEnumerable<TResult>>(this, results);
        }

    }

    public class RedisTransaction<TContext, TResults> : RedisTransaction<TContext> where TContext : RedisContext
    {
        protected internal RedisTransaction([NotNull] RedisTransaction<TContext> @base, TResults results) : base(Check.NotNull(@base, nameof(@base)).Context, @base.Transaction)
        {
            Base = @base;
            Results = results;
            ResultsType = typeof(TResults);
        }

        protected TResults Results { get; }

        protected RedisTransaction<TContext> Base { get; }

        public new RedisTransaction<TContext, TResults> Done([NotNull] Action<TContext> commands)
        {
            return (RedisTransaction<TContext, TResults>)base.Done(commands);
        }

        public new TResults Execute()
        {
            return Transaction.Execute() ? Results : default(TResults);
        }

        public new async Task<TResults> ExecuteAsync()
        {
            return await Transaction.ExecuteAsync() ? Results : default(TResults);
        }

        public bool Execute(Action<TResults> committed)
        {
            if (!Transaction.Execute())
                return false;
            committed?.Invoke(Results);
            return true;
        }

        public async Task<bool> ExecuteAsync(Action<TResults> committed)
        {
            if (!await Transaction.ExecuteAsync())
                return false;
            committed?.Invoke(Results);
            return true;
        }

    }

    public class RedisCondition
    {
        private readonly ITransaction _transaction;

        protected internal RedisCondition(ITransaction transaction)
        {
            _transaction = transaction;
        }

        public bool HashEqual(RedisKey key, RedisValue hashField, RedisValue value)
        {
            return _transaction.AddCondition(Condition.HashEqual(key, hashField, value)).WasSatisfied;
        }

        public bool HashExists(RedisKey key, RedisValue hashField)
        {
            return _transaction.AddCondition(Condition.HashExists(key, hashField)).WasSatisfied;
        }

        public bool HashNotEqual(RedisKey key, RedisValue hashField, RedisValue value)
        {
            return _transaction.AddCondition(Condition.HashNotEqual(key, hashField, value)).WasSatisfied;
        }

        public bool HashNotExists(RedisKey key, RedisValue hashField)
        {
            return _transaction.AddCondition(Condition.HashNotExists(key, hashField)).WasSatisfied;
        }

        public bool KeyExists(RedisKey key)
        {
            return _transaction.AddCondition(Condition.KeyExists(key)).WasSatisfied;
        }

        public bool KeyNotExists(RedisKey key)
        {
            return _transaction.AddCondition(Condition.KeyNotExists(key)).WasSatisfied;
        }

        public bool StringEqual(RedisKey key, RedisValue value)
        {
            return _transaction.AddCondition(Condition.StringEqual(key, value)).WasSatisfied;
        }

        public bool StringNotEqual(RedisKey key, RedisValue value)
        {
            return _transaction.AddCondition(Condition.StringNotEqual(key, value)).WasSatisfied;
        }

    }

    public static class RedisContextExtensions
    {
        public static RedisBatch<TContext> BeginBatch<TContext>(this TContext context) where TContext : RedisContext
        {
            return context.BeginBatch<TContext>();
        }

        public static RedisTransaction<TContext> BeginTransaction<TContext>(this TContext context) where TContext : RedisContext
        {
            return context.BeginTransaction<TContext>();
        }

    }

}
