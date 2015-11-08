using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Hyperspace.Redis
{
    public class RedisList : RedisEntry
    {
        public RedisList(RedisContext context, RedisKey key) : base(context, key, RedisEntryType.List)
        {
        }

    }

    public class RedisList<T> : RedisList
    {
        public RedisList(RedisContext context, RedisKey key) : base(context, key)
        {
        }

    }
}
