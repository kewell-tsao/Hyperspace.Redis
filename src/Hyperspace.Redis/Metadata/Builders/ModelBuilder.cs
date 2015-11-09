using System;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;
using System.Reflection;

namespace Hyperspace.Redis.Metadata.Builders
{
    public class ModelBuilder
    {
        internal static Type GetPropertyType<T, TEntry>(Expression<Func<T, TEntry>> propertyExpression)
            where TEntry : RedisEntry
        {
            if (propertyExpression == null)
                throw new ArgumentNullException(nameof(propertyExpression));

            var body = propertyExpression.Body as MemberExpression;
            if (body == null)
                throw new ArgumentException("Invalid argument", nameof(propertyExpression));

            var property = body.Member as PropertyInfo;
            if (property == null)
                throw new ArgumentException("Argument is not a property", nameof(propertyExpression));

            return property.PropertyType;
        }

        internal static string GetPropertyName<T, TEntry>(Expression<Func<T, TEntry>> propertyExpression)
            where TEntry : RedisEntry
        {
            if (propertyExpression == null)
                throw new ArgumentNullException(nameof(propertyExpression));

            var body = propertyExpression.Body as MemberExpression;
            if (body == null)
                throw new ArgumentException("Invalid argument", nameof(propertyExpression));

            var property = body.Member as PropertyInfo;
            if (property == null)
                throw new ArgumentException("Argument is not a property", nameof(propertyExpression));

            return property.Name;
        }

        internal static RedisEntryType GetEntryType<TEntry>()
            where TEntry : RedisEntry
        {
            return GetEntryType(typeof(TEntry));
        }

        internal static RedisEntryType GetEntryType(Type entryType)
        {
            var typeInfo = entryType.GetTypeInfo();
            if (!typeInfo.IsSubclassOf(typeof(RedisEntry)))
                throw new ArgumentException("This type is't subclass of RedisEntry.", nameof(entryType));
            var types = typeInfo.GetCustomAttributes(true).OfType<RedisEntryTypeAttribute>().Select(a => a.Type).ToArray();
            if (types.Length == 0)
                return RedisEntryType.String;
            if (types.Distinct().Count() > 1)
                throw new ArgumentException("This type defines a number of different RedisEntryType.", nameof(entryType));
            return types.First();
        }

    }

    public class ModelBuilder<TContext> : ModelBuilder where TContext : RedisContext
    {
        private readonly ModelMetadata _metadata;

        public ModelBuilder()
        {
            _metadata = new ModelMetadata
            {
                ClrType = typeof(TContext),
                Name = typeof(TContext).GetTypeInfo().Name
            };
        }

        public EntryBuilder<TEntry> Entry<TEntry>([NotNull] Expression<Func<TContext, TEntry>> property)
            where TEntry : RedisEntry
        {
            Check.NotNull(property, nameof(property));

            var metadata = new EntryMetadata
            {
                Model = _metadata,
                Name = GetPropertyName(property),
                ClrType = GetPropertyType(property)
            };
            metadata.EntryType = GetEntryType(metadata.ClrType);
            return new EntryBuilder<TEntry>(metadata);
        }

        public ModelBuilder<TContext> Entry<TEntry>([NotNull] Expression<Func<TContext, TEntry>> property, [NotNull] Action<EntryBuilder<TEntry>> buildAction)
            where TEntry : RedisEntry
        {
            Check.NotNull(property, nameof(property));
            Check.NotNull(buildAction, nameof(buildAction));

            buildAction(Entry(property));

            return this;
        }

        public EntrySetBuilder<TEntry, TIdentifier> EntrySet<TEntry, TIdentifier>([NotNull] Expression<Func<TContext, RedisEntrySet<TEntry, TIdentifier>>> property)
            where TEntry : RedisEntry
        {
            Check.NotNull(property, nameof(property));

            var metadata = new EntrySetMetadata
            {
                Model = _metadata,
                Name = GetPropertyName(property),
                ClrType = GetPropertyType(property)
            };
            metadata.EntryType = GetEntryType(metadata.ClrType);
            return new EntrySetBuilder<TEntry, TIdentifier>(metadata);
        }

        public ModelBuilder<TContext> EntrySet<TEntry, TIdentifier>(
            [NotNull] Expression<Func<TContext, RedisEntrySet<TEntry, TIdentifier>>> property,
            [NotNull] Action<EntrySetBuilder<TEntry, TIdentifier>> buildAction)
            where TEntry : RedisEntry
        {
            Check.NotNull(property, nameof(property));
            Check.NotNull(buildAction, nameof(buildAction));

            buildAction(EntrySet(property));

            return this;
        }

    }

    public class EntryBuilder<TEntry> where TEntry : RedisEntry
    {
        private readonly EntryMetadata _metadata;

        public EntryBuilder([NotNull] EntryMetadata metadata)
        {
            Check.NotNull(metadata, nameof(metadata));

            _metadata = metadata;
        }

        public void ShortName(string shortName)
        {
            _metadata.ShortName = shortName;
        }

        public EntryBuilder<TSubEntry> SubEntry<TSubEntry>([NotNull] Expression<Func<TEntry, TSubEntry>> property)
            where TSubEntry : RedisEntry
        {
            Check.NotNull(property, nameof(property));

            var metadata = new EntrySetMetadata
            {
                Model = _metadata.Model,
                Parent = _metadata,
                Name = ModelBuilder.GetPropertyName(property),
                ClrType = ModelBuilder.GetPropertyType(property),
            };
            metadata.EntryType = ModelBuilder.GetEntryType(metadata.ClrType);
            return new EntryBuilder<TSubEntry>(metadata);
        }

        public EntryBuilder<TEntry> SubEntry<TSubEntry>([NotNull] Expression<Func<TEntry, TSubEntry>> property, [NotNull] Action<EntryBuilder<TSubEntry>> buildAction)
            where TSubEntry : RedisEntry
        {
            Check.NotNull(property, nameof(property));
            Check.NotNull(buildAction, nameof(buildAction));

            buildAction(SubEntry(property));

            return this;
        }

    }

    public class EntrySetBuilder<TEntry, TIdentifier> where TEntry : RedisEntry
    {
        private readonly EntrySetMetadata _metadata;

        public EntrySetBuilder(EntrySetMetadata metadata)
        {
            Check.NotNull(metadata, nameof(metadata));

            _metadata = metadata;
        }

        public EntrySetBuilder<TEntry, TIdentifier> ShortName(string shortName)
        {
            return this;
        }

        public EntrySetBuilder<TEntry, TIdentifier> EntryType(RedisEntryType type)
        {
            _metadata.EntryType = type;
            return this;
        }

        public EntrySetItemBuilder<TEntry, TIdentifier> EntrySetItem()
        {
            var metadata = new EntryMetadata
            {
                Parent = _metadata,
                Model = _metadata.Model,
                Name = typeof(TEntry).GetTypeInfo().Name,
                ClrType = typeof(TEntry),
            };
            metadata.EntryType = ModelBuilder.GetEntryType(metadata.ClrType);
            return new EntrySetItemBuilder<TEntry, TIdentifier>(metadata);
        }

        public EntrySetBuilder<TEntry, TIdentifier> EntrySetItem([NotNull] Action<EntrySetItemBuilder<TEntry, TIdentifier>> buildAction)
        {
            Check.NotNull(buildAction, nameof(buildAction));

            buildAction(EntrySetItem());

            return this;
        }

    }

    public class EntrySetItemBuilder<TEntry, TIdentifier> where TEntry : RedisEntry
    {
        private readonly EntryMetadata _metadata;

        public EntrySetItemBuilder(EntryMetadata metadata)
        {
            Check.NotNull(metadata, nameof(metadata));

            _metadata = metadata;
        }

        public EntryBuilder<TSubEntry> SubEntry<TSubEntry>([NotNull]Expression<Func<TEntry, TSubEntry>> property)
            where TSubEntry : RedisEntry
        {
            Check.NotNull(property, nameof(property));

            var metadata = new EntryMetadata
            {
                Parent = _metadata,
                Model = _metadata.Model,
                Name = ModelBuilder.GetPropertyName(property),
                ClrType = ModelBuilder.GetPropertyType(property),
            };
            metadata.EntryType = ModelBuilder.GetEntryType(metadata.ClrType);
            return new EntryBuilder<TSubEntry>(metadata);
        }

        public EntrySetItemBuilder<TEntry, TIdentifier> SubEntry<TSubEntry>([NotNull] Expression<Func<TEntry, TSubEntry>> property, [NotNull] Action<EntryBuilder<TSubEntry>> buildAction)
            where TSubEntry : RedisEntry
        {
            Check.NotNull(property, nameof(property));
            Check.NotNull(buildAction, nameof(buildAction));

            buildAction(SubEntry(property));

            return this;
        }

        public EntrySetItemBuilder<TEntry, TIdentifier> Identifier([NotNull]Expression<Func<TEntry, TIdentifier>> property)
        {
            return this;
        }

    }

}
