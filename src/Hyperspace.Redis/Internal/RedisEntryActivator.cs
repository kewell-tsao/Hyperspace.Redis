using JetBrains.Annotations;
using Hyperspace.Redis.Metadata;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Hyperspace.Redis.Internal
{
    public class RedisEntryActivator
    {
        private readonly RedisModelMetadata _metadata;
        private readonly Dictionary<RedisEntryMetadata, Dictionary<string, EntryBuilder>> _factories;

        public RedisEntryActivator([NotNull] RedisModelMetadata metadata)
        {
            Check.NotNull(metadata, nameof(metadata));

            _metadata = metadata;
        }

        public TEntry CreateInstance<TEntry>([NotNull] RedisContext context, [NotNull] string name) where TEntry : RedisEntry
        {
            Check.NotNull(context, nameof(context));
            Check.NotEmpty(name, nameof(name));

            var entryMetadata = _metadata.Children.SingleOrDefault(m => m.Name == name && m.ClrType == typeof(TEntry));

            return null;
        }

        public TEntry CreateInstance<TEntry>([NotNull] RedisEntry parent, [NotNull] string name) where TEntry : RedisEntry
        {
            Check.NotNull(parent, nameof(parent));
            Check.NotNull(parent.Context, nameof(parent), nameof(parent.Context));
            Check.NotEmpty(name, nameof(name));

            EntryBuilder factoriy;
            Dictionary<string, EntryBuilder> factories;
            if (!_factories.TryGetValue(parent.Metadata, out factories))
                throw new InvalidOperationException();
            if (!factories.TryGetValue(name, out factoriy))
                throw new InvalidOperationException();

            return null;
        }

        public TEntry CreateInstance<TEntry, TIdentifier>([NotNull] RedisEntrySet<TEntry, TIdentifier> parent, [NotNull] TIdentifier identifier) where TEntry : RedisEntry
        {
            Check.NotNull(parent, nameof(parent));
            Check.NotNull(parent.Context, nameof(parent), nameof(parent.Context));
            Check.NotNull(identifier, nameof(identifier));


            return null;
        }

        private Func<RedisKey, RedisEntryMetadata, RedisContext, RedisEntry, TEntry> CreateConstructor<TEntry>() where TEntry : RedisEntry
        {
            var p0 = Expression.Parameter(typeof(RedisKey), "key");
            var p1 = Expression.Parameter(typeof(RedisEntryMetadata), "metadata");
            var p2 = Expression.Parameter(typeof(RedisContext), "context");
            var p3 = Expression.Parameter(typeof(RedisEntry), "parent");
            var entryType = typeof(TEntry);
            var entryConstructorInfo = entryType.GetConstructor(new[] { p0.Type, p1.Type, p2.Type, p3.Type });
            if (entryConstructorInfo == null)
                throw new InvalidOperationException();
            var entryConstructor = Expression.Lambda<Func<RedisKey, RedisEntryMetadata, RedisContext, RedisEntry, TEntry>>(
                Expression.New(entryConstructorInfo, p0, p1, p2, p3), p0, p1, p2, p3).Compile();
            return entryConstructor;
        }

    }

    public abstract class EntryBuilder
    {
    }

    public class RooEntryBuilder<TEntry> : EntryBuilder where TEntry : RedisEntry
    {
        public TEntry Create([NotNull] RedisContext context)
        {
            return null;
        }
    }

    public class SubEntryBuilder<TEntry> : EntryBuilder where TEntry : RedisEntry
    {
        public TEntry Create([NotNull] RedisEntry parent)
        {
            return null;
        }
    }

    public class IteEntryBuilder<TEntry, TIdentifier> : EntryBuilder where TEntry : RedisEntry
    {
        public TEntry Create([NotNull] RedisEntrySet<TEntry, TIdentifier> parent, [NotNull] TIdentifier identifier)
        {
            return null;
        }
    }

}
