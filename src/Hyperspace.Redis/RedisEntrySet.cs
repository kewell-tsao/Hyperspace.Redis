using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Hyperspace.Redis.Metadata;
using StackExchange.Redis;

namespace Hyperspace.Redis
{
    public sealed class RedisEntrySet<TEntry, TIdentifier> : RedisEntry where TEntry : RedisEntry
    {
        public RedisEntrySet(RedisKey key, RedisEntryMetadata metadata, RedisContext context, RedisEntry parent) : base(key, metadata, context, parent)
        {
        }

        private Dictionary<TIdentifier, TEntry> _cache;

        public TEntry this[[NotNull] TIdentifier identifier]
        {
            get
            {
                Check.NotNull(identifier, nameof(identifier));

                TEntry result;
                if (_cache != null && _cache.TryGetValue(identifier, out result))
                {
                    if (result == null)
                        throw new InvalidOperationException();
                    return result;
                }
                result = Context.Services.Activator.CreateInstance(this, identifier);
                if (result == null)
                    throw new InvalidOperationException();
                if (_cache == null)
                    _cache = new Dictionary<TIdentifier, TEntry>();
                _cache.Add(identifier, result);
                return result;
            }
        }

    }
}
