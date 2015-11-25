using JetBrains.Annotations;
using StackExchange.Redis;
using System;
using System.Collections.Generic;

namespace Hyperspace.Redis
{
    public abstract class RedisBatch : RedisContextScope
    {
        protected internal RedisBatch(RedisContext context) : base(context)
        {
            Batch = context.Database.Database.CreateBatch();
        }

        protected internal RedisBatch(RedisContext context, IBatch batch) : base(context)
        {
            Check.NotNull(batch, nameof(batch));

            Batch = batch;
        }

        protected internal IBatch Batch { get; }

        internal override IDatabaseAsync AsyncFunc
        {
            get { return Batch; }
        }

        public void Execute()
        {
            Batch.Execute();
        }

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
            using (new ScopeDaemon(this))
            {
                commands(Context);
            }
            return this;
        }

        public RedisBatch<TContext, TResults> Done<TResults>([NotNull] Func<TContext, TResults> commands)
        {
            Check.NotNull(commands, nameof(commands));
            TResults results;
            using (new ScopeDaemon(this))
            {
                results = commands(Context);
            }
            return new RedisBatch<TContext, TResults>(this, results);

        }

        public RedisBatch<TContext, IEnumerable<TResult>> Done<TResult>([NotNull] Func<TContext, IEnumerable<TResult>> commands)
        {
            Check.NotNull(commands, nameof(commands));
            IEnumerable<TResult> results;
            using (new ScopeDaemon(this))
            {
                results = commands(Context);
            }
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

}
