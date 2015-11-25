using System;

namespace Hyperspace.Redis
{
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
