using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Hyperspace.Redis
{
    public class RedisEntrySet<TEntry, TIdentifier> : RedisEntry where TEntry : RedisEntry
    {
        private readonly Func<RedisContext, RedisKey, TEntry> _constructor;

        public RedisEntrySet(RedisContext context, RedisKey key, Func<RedisContext, RedisKey, TEntry> constructor) : base(context, key, RedisEntryType.Set)
        {
            _constructor = constructor;
        }

        public TEntry this[TIdentifier identifier]
        {
            get
            {
                return null;
            }
        }

    }

    public class RedisEntrySet<TEntry> : RedisEntrySet<TEntry, string> where TEntry : RedisEntry
    {
        public RedisEntrySet(RedisContext context, RedisKey key, Func<RedisContext, RedisKey, TEntry> constructor) : base(context, key, constructor)
        {
        }
    }
}
