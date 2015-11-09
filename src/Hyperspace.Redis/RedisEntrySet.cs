using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Hyperspace.Redis
{
    public class RedisEntrySet<TEntry, TIdentifier> : RedisEntry where TEntry : RedisEntry
    {
        public RedisEntrySet(RedisEntry parent, RedisKey key, RedisEntryType entryType) : base(parent, key, entryType)
        {
        }

        public RedisEntrySet(RedisContext context, RedisKey key, RedisEntryType entryType) : base(context, key, entryType)
        {
        }

        public TEntry this[TIdentifier identifier] => GetSubEntry(this, identifier);

        protected TEntry GetSubEntry(RedisEntry parent, TIdentifier identifier)
        {
            throw new NotImplementedException();
        }

    }

    public class RedisEntrySet<TEntry> : RedisEntrySet<TEntry, string> where TEntry : RedisEntry
    {
        public RedisEntrySet(RedisEntry parent, RedisKey key, RedisEntryType entryType) : base(parent, key, entryType)
        {
        }

        public RedisEntrySet(RedisContext context, RedisKey key, RedisEntryType entryType) : base(context, key, entryType)
        {
        }

    }
}
