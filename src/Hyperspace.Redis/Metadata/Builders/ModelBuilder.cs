using System;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;
using System.Reflection;

namespace Hyperspace.Redis.Metadata.Builders
{
    public abstract class ModelBuilder
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

        internal abstract RedisModelMetadata Complete();

        public ModelBuilder<TContext> As<TContext>() where TContext : RedisContext
        {
            return this as ModelBuilder<TContext>;
        }

    }

    public class ModelBuilder<TContext> : ModelBuilder where TContext : RedisContext
    {
        private readonly RedisModelMetadata _metadata;

        public ModelBuilder()
        {
            _metadata = new RedisModelMetadata
            {
                ClrType = typeof(TContext),
                Name = typeof(TContext).GetTypeInfo().Name
            };
        }

        public EntryBuilder<TEntry> Entry<TEntry>([NotNull] Expression<Func<TContext, TEntry>> property)
            where TEntry : RedisEntry
        {
            Check.NotNull(property, nameof(property));

            var metadata = new RedisEntryMetadata
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

            var metadata = new RedisEntryMetadata(true)
            {
                Model = _metadata,
                Name = GetPropertyName(property),
                ClrType = GetPropertyType(property)
            };
            metadata.Children.Add(new RedisEntryMetadata()
            {
                Model = _metadata,
                ClrType = typeof(TEntry),
                EntryType = GetEntryType(typeof(TEntry))
            });
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

        internal override RedisModelMetadata Complete()
        {
            return _metadata;
        }

    }

    public class EntryBuilder<TEntry> where TEntry : RedisEntry
    {
        private readonly RedisEntryMetadata _metadata;

        public EntryBuilder([NotNull] RedisEntryMetadata metadata)
        {
            Check.NotNull(metadata, nameof(metadata));

            _metadata = metadata;
        }

        public EntryBuilder<TEntry> MapTo([NotNull] string alias)
        {
            Check.NotEmpty(alias, nameof(alias));

            _metadata.Alias = alias;

            return this;
        }

        public EntryBuilder<TSubEntry> SubEntry<TSubEntry>([NotNull] Expression<Func<TEntry, TSubEntry>> property)
            where TSubEntry : RedisEntry
        {
            Check.NotNull(property, nameof(property));

            var metadata = new RedisEntryMetadata
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

        public EntrySetBuilder<TSubEntry, TIdentifier> SubEntrySet<TSubEntry, TIdentifier>([NotNull] Expression<Func<TEntry, RedisEntrySet<TSubEntry, TIdentifier>>> property)
            where TSubEntry : RedisEntry
        {
            Check.NotNull(property, nameof(property));

            var metadata = new RedisEntryMetadata(true)
            {
                Model = _metadata.Model,
                Name = ModelBuilder.GetPropertyName(property),
                ClrType = ModelBuilder.GetPropertyType(property)
            };
            return new EntrySetBuilder<TSubEntry, TIdentifier>(metadata);
        }

        public EntryBuilder<TEntry> SubEntrySet<TSubEntry, TIdentifier>([NotNull] Expression<Func<TEntry, RedisEntrySet<TSubEntry, TIdentifier>>> property, [NotNull] Action<EntrySetBuilder<TSubEntry, TIdentifier>> buildAction)
            where TSubEntry : RedisEntry
        {
            Check.NotNull(property, nameof(property));
            Check.NotNull(buildAction, nameof(buildAction));

            buildAction(SubEntrySet(property));

            return this;
        }

    }

    public class EntrySetBuilder<TEntry, TIdentifier> where TEntry : RedisEntry
    {
        private readonly RedisEntryMetadata _metadata;

        public EntrySetBuilder(RedisEntryMetadata metadata)
        {
            Check.NotNull(metadata, nameof(metadata));

            _metadata = metadata;
        }

        public EntrySetBuilder<TEntry, TIdentifier> MapTo([NotNull] string token)
        {
            Check.NotEmpty(token, nameof(token));

            _metadata.Alias = token;
            return this;
        }

        public EntrySetBuilder<TEntry, TIdentifier> EntryType(RedisEntryType type)
        {
            _metadata.EntryType = type;
            return this;
        }

        public EntrySetBuilder<TEntry, TIdentifier> Identifier([NotNull]Expression<Func<TEntry, TIdentifier>> property)
        {
            return this;
        }


        public EntryBuilder<TEntry> EntryItem()
        {
            var metadata = new RedisEntryMetadata
            {
                Parent = _metadata,
                Model = _metadata.Model,
                Name = typeof(TEntry).GetTypeInfo().Name,
                ClrType = typeof(TEntry),
            };
            metadata.EntryType = ModelBuilder.GetEntryType(metadata.ClrType);
            return new EntryBuilder<TEntry>(metadata);
        }

        public EntrySetBuilder<TEntry, TIdentifier> EntryItem([NotNull] Action<EntryBuilder<TEntry>> buildAction)
        {
            Check.NotNull(buildAction, nameof(buildAction));

            buildAction(EntryItem());

            return this;
        }

    }

}
