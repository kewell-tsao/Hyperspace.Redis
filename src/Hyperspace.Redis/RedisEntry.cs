using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Hyperspace.Redis
{
    public abstract class RedisEntry
    {
        protected internal RedisEntry(RedisContext context, RedisKey key, RedisEntryType entryType)
        {
            Context = context;
            Key = key;
            EntryType = entryType;
        }

        public RedisKey Key { get; }

        public RedisContext Context { get; }

        public RedisEntryType EntryType { get; }

    }

    public enum RedisEntryType
    {
        String,
        List,
        Hash,
        Set,
        SortedSet
    }
}
