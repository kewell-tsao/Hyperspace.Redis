using Hyperspace.Redis.Metadata;
using JetBrains.Annotations;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Hyperspace.Redis.Internal
{
    public class RedisEntryActivator
    {
        private readonly RedisModelMetadata _metadata;
        private readonly Dictionary<string, EntryBuilder> _entryBuilders;

        public RedisEntryActivator([NotNull] RedisModelMetadata metadata)
        {
            Check.NotNull(metadata, nameof(metadata));

            _metadata = metadata;

            var stack = new Stack<RedisEntryMetadata>(metadata.Children);

            while (stack.Count > 0)
            {
                var entryMetadata = stack.Pop();
                foreach (var item in entryMetadata.Children ?? Enumerable.Empty<RedisEntryMetadata>())
                {
                    stack.Push(item);
                }
                EntryBuilder entryBuilder;
                if (entryMetadata.Parent?.IsEntrySet ?? false)
                    entryBuilder = (EntryBuilder)Activator.CreateInstance(typeof(EntryBuilder<,>).MakeGenericType(entryMetadata.ClrType, entryMetadata.IdentifierClrType));
                else
                    entryBuilder = (EntryBuilder)Activator.CreateInstance(typeof(EntryBuilder<>).MakeGenericType(entryMetadata.ClrType));

                _entryBuilders.Add(entryMetadata.GetIdentifier(), entryBuilder);
            }
        }

        public TEntry CreateInstance<TEntry>([NotNull] RedisContext context, [NotNull] string name) where TEntry : RedisEntry
        {
            Check.NotNull(context, nameof(context));
            Check.NotEmpty(name, nameof(name));

            var metadataKey = string.IsNullOrEmpty(_metadata.Prefix) ? name : $"{_metadata.Prefix}:{name}";
            EntryBuilder entryBuilder;
            if (!_entryBuilders.TryGetValue(metadataKey, out entryBuilder))
                throw new InvalidOperationException();
            var entryBuilder_t = entryBuilder as EntryBuilder<TEntry>;
            if (entryBuilder_t == null)
                throw new InvalidOperationException();

            return entryBuilder_t.Create(context);
        }

        public TEntry CreateInstance<TEntry>([NotNull] RedisEntry parent, [NotNull] string name) where TEntry : RedisEntry
        {
            Check.NotNull(parent, nameof(parent));
            Check.NotNull(parent.Context, nameof(parent), nameof(parent.Context));
            Check.NotEmpty(name, nameof(name));

            var metadataKey = $"{parent.Metadata.GetIdentifier()}:{name}";
            EntryBuilder entryBuilder;
            if (!_entryBuilders.TryGetValue(metadataKey, out entryBuilder))
                throw new InvalidOperationException();
            var entryBuilder_t = entryBuilder as EntryBuilder<TEntry>;
            if (entryBuilder_t == null)
                throw new InvalidOperationException();

            return entryBuilder_t.Create(parent);
        }

        public TEntry CreateInstance<TEntry, TIdentifier>([NotNull] RedisEntrySet<TEntry, TIdentifier> parent, [NotNull] TIdentifier identifier) where TEntry : RedisEntry
        {
            Check.NotNull(parent, nameof(parent));
            Check.NotNull(parent.Context, nameof(parent), nameof(parent.Context));
            Check.NotNull(identifier, nameof(identifier));

            var metadataKey = parent.Metadata.GetIdentifier();
            EntryBuilder entryBuilder;
            if (!_entryBuilders.TryGetValue(metadataKey, out entryBuilder))
                throw new InvalidOperationException();
            var entryBuilder_t = entryBuilder as EntryBuilder<TEntry, TIdentifier>;
            if (entryBuilder_t == null)
                throw new InvalidOperationException();

            return entryBuilder_t.Create(parent, identifier);
        }

    }

    public abstract class EntryBuilder
    {
        protected EntryBuilder(RedisEntryMetadata metadata)
        {
            Check.NotNull(metadata, nameof(metadata));

            if (!metadata.IsFrozen)
                throw new ArgumentException("", nameof(metadata));

            Metadata = metadata;
        }

        public RedisEntryMetadata Metadata { get; }

        protected static Func<RedisKey, RedisEntryMetadata, RedisContext, RedisEntry, TEntry> CreateConstructor<TEntry>() where TEntry : RedisEntry
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

    public class EntryBuilder<TEntry> : EntryBuilder where TEntry : RedisEntry
    {
        private readonly Func<RedisKey, RedisEntryMetadata, RedisContext, RedisEntry, TEntry> _constructor;

        public EntryBuilder(RedisEntryMetadata metadata) : base(metadata)
        {
            Check.NotNull(metadata, nameof(metadata));

            _constructor = CreateConstructor<TEntry>();
        }

        public TEntry Create([NotNull] RedisEntry parent)
        {
            Check.NotNull(parent, nameof(parent));

            if (Metadata.Parent == null)
                throw new InvalidOperationException("");

            var context = parent.Context;
            var name = string.IsNullOrEmpty(Metadata.Alias) ? Metadata.Name : Metadata.Alias;
            var key = parent.Key.Append($":{name}");

            return _constructor(key, Metadata, context, parent);
        }

        public TEntry Create([NotNull] RedisContext context)
        {
            Check.NotNull(context, nameof(context));

            if (Metadata.Parent != null)
                throw new InvalidOperationException("");

            var prefix = Metadata.Model.Prefix;
            var name = string.IsNullOrEmpty(Metadata.Alias) ? Metadata.Name : Metadata.Alias;
            var key = string.IsNullOrEmpty(prefix) ? $"{prefix}:{name}" : name;
            return _constructor(key, Metadata, context, null);
        }

    }

    public class EntryBuilder<TEntry, TIdentifier> : EntryBuilder where TEntry : RedisEntry
    {
        private readonly Func<RedisKey, RedisEntryMetadata, RedisContext, RedisEntry, TEntry> _constructor;
        private readonly IIdentifierConverter<TIdentifier> _identifierConverter;

        public EntryBuilder(RedisEntryMetadata metadata) : base(metadata)
        {
            if (!metadata.IsEntrySet)
                throw new ArgumentException("", nameof(metadata));

            _constructor = CreateConstructor<TEntry>();
            _identifierConverter = (IIdentifierConverter<TIdentifier>)Activator.CreateInstance(metadata.IdentifierConverterType);
        }

        public TEntry Create([NotNull] RedisEntrySet<TEntry, TIdentifier> parent, [NotNull] TIdentifier identifier)
        {
            Check.NotNull(parent, nameof(parent));
            Check.NotNull(identifier, nameof(identifier));

            if (parent.Metadata != Metadata)
                throw new ArgumentException("", nameof(parent));

            var context = parent.Context;
            var identifierStr = identifier.ToString();
            var key = parent.Key.Append($":{_identifierConverter.ToString(identifier)}");

            return _constructor(key, Metadata, context, parent);
        }

    }

    public interface IIdentifierConverter<TIdentifier>
    {
        string ToString(TIdentifier identifier);
        TIdentifier ToIdentifier(string idstr);
    }

}
