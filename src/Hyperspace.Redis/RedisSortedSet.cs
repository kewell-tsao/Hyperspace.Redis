using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Hyperspace.Redis
{
    [RedisEntryType(RedisEntryType.SortedSet)]
    public class RedisSortedSet : RedisEntry
    {
        public RedisSortedSet(RedisContext context, RedisKey key) : base(context, key, RedisEntryType.SortedSet)
        {
        }

    }

    public class RedisSortedSet<T> : RedisSortedSet
    {
        public RedisSortedSet(RedisContext context, RedisKey key) : base(context, key)
        {
        }
    }
}
