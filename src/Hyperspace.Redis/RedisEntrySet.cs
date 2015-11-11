using StackExchange.Redis;

namespace Hyperspace.Redis
{
    public sealed class RedisEntrySet<TEntry, TIdentifier> : RedisEntry where TEntry : RedisEntry
    {
        public RedisEntrySet(RedisEntry parent, RedisKey key, RedisEntryType entryType) : base(parent, key, entryType)
        {
        }

        public RedisEntrySet(RedisContext context, RedisKey key, RedisEntryType entryType) : base(context, key, entryType)
        {
        }

        public TEntry this[TIdentifier identifier] => Context.GetSubEntry<TEntry, TIdentifier>(this, identifier);

    }
}
