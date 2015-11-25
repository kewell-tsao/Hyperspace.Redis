using JetBrains.Annotations;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hyperspace.Redis
{
    public abstract class RedisTransaction : RedisContextScope
    {
        protected internal RedisTransaction([NotNull] RedisContext context) : base(context)
        {
            Transaction = Context.Database.Database.CreateTransaction();
        }

        protected internal RedisTransaction([NotNull] RedisContext context, [NotNull] ITransaction transaction) : base(context)
        {
            Check.NotNull(transaction, nameof(transaction));

            Transaction = transaction;
        }

        protected internal ITransaction Transaction { get; }

        internal override IDatabaseAsync AsyncFunc
        {
            get { return Transaction; }
        }

        public RedisTransaction When([NotNull] Action<RedisCondition> condition)
        {
            Check.NotNull(condition, nameof(condition));

            condition.Invoke(new RedisCondition(Context, Transaction));
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

        public RedisTransaction<TContext> When([NotNull] Action<RedisCondition<TContext>> condition)
        {
            Check.NotNull(condition, nameof(condition));

            condition.Invoke(new RedisCondition<TContext>(Context, Transaction));
            return this;
        }

        public RedisTransaction<TContext> Done([NotNull] Action<TContext> commands)
        {
            Check.NotNull(commands, nameof(commands));
            using (new ScopeDaemon(this))
            {
                commands(Context);
            }
            return this;
        }

        public RedisTransaction<TContext, TResults> Done<TResults>([NotNull] Func<TContext, TResults> commands)
        {
            Check.NotNull(commands, nameof(commands));

            if (ResultsType != null)
                throw new InvalidOperationException();
            ResultsType = typeof(TResults);
            TResults results;
            using (new ScopeDaemon(this))
            {
                results = commands(Context);
            }
            return new RedisTransaction<TContext, TResults>(this, results);
        }

        public RedisTransaction<TContext, IEnumerable<TResult>> Done<TResult>([NotNull] Func<TContext, IEnumerable<TResult>> commands)
        {
            Check.NotNull(commands, nameof(commands));

            if (ResultsType != null)
                throw new InvalidOperationException();
            ResultsType = typeof(IEnumerable<TResult>);
            IEnumerable<TResult> results;
            using (new ScopeDaemon(this))
            {
                results = commands(Context);
            }
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

}
