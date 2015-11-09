using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Hyperspace.Redis
{
    [RedisEntryType(RedisEntryType.Set)]
    public class RedisSet : RedisEntry
    {
        public RedisSet(RedisContext context, RedisKey key) : base(context, key, RedisEntryType.Set)
        {
        }

    }

    public class RedisSet<T> : RedisSet
    {
        public RedisSet(RedisContext context, RedisKey key) : base(context, key)
        {
        }
    }
}
