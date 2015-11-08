using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Hyperspace.Redis
{
    public class RedisHash : RedisEntry
    {
        public RedisHash(RedisContext context, RedisKey key) : base(context, key, RedisEntryType.Hash)
        {
        }
    }
}
