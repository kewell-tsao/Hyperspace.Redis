using JetBrains.Annotations;
using StackExchange.Redis;

namespace Hyperspace.Redis
{
    public class RedisCondition
    {
        private readonly RedisContext _context;
        private readonly ITransaction _transaction;

        protected internal RedisCondition([NotNull] RedisContext context, [NotNull] ITransaction transaction)
        {
            Check.NotNull(context, nameof(context));
            Check.NotNull(transaction, nameof(transaction));

            _context = context;
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

    public class RedisCondition<TContext> : RedisCondition where TContext : RedisContext
    {
        protected internal RedisCondition(TContext context, ITransaction transaction) : base(context, transaction)
        {
        }
    }

}
