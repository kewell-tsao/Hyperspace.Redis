using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Hyperspace.Redis.Metadata.Builders
{
    public abstract class ModelBuilder
    {
        protected internal ModelBuilder()
        {
        }
    }

    public class ModelBuilder<TContext> : ModelBuilder where TContext : RedisContext
    {
        public EntryBuilder<TEntry> Entry<TEntry>([NotNull]Expression<Func<TContext, TEntry>> property)
            where TEntry : RedisEntry
        {
            return new EntryBuilder<TEntry>();
        }

        public ModelBuilder<TContext> Entry<TEntry>([NotNull] Expression<Func<TContext, TEntry>> property, [NotNull] Action<EntryBuilder<TEntry>> buildAction)
            where TEntry : RedisEntry
        {
            return this;
        }

        public EntrySetBuilder<TEntry> EntrySet<TEntry>([NotNull] Expression<Func<TContext, RedisEntrySet<TEntry>>> property)
            where TEntry : RedisEntry
        {
            return new EntrySetBuilder<TEntry>();
        }

        public ModelBuilder<TContext> EntrySet<TEntry>([NotNull] Expression<Func<TContext, RedisEntrySet<TEntry>>> property, [NotNull] Action<EntrySetBuilder<TEntry>> buildAction)
            where TEntry : RedisEntry
        {
            return this;
        }

    }

    public abstract class EntryBuilder
    {
        protected internal EntryBuilder()
        {
        }
    }

    public class EntryBuilder<TEntry> : EntryBuilder where TEntry : RedisEntry
    {
        public void HasKey()
        {

        }

        public EntryBuilder<TSubEntry> SubEntry<TSubEntry>([NotNull]Expression<Func<TEntry, TSubEntry>> property)
            where TSubEntry : RedisEntry
        {
            return new EntryBuilder<TSubEntry>();
        }

        public EntryBuilder<TEntry> SubEntry<TSubEntry>([NotNull] Expression<Func<TEntry, TSubEntry>> property, [NotNull] Action<EntryBuilder<TSubEntry>> buildAction)
            where TSubEntry : RedisEntry
        {
            return this;
        }

    }

    public abstract class EntrySetBuilder: EntryBuilder
    {
        protected internal EntrySetBuilder()
        {
        }
    }

    public class EntrySetBuilder<TEntry> : EntrySetBuilder where TEntry : RedisEntry
    {
        public EntryBuilder<TEntry> ItemEntry()
        {
            return new EntryBuilder<TEntry>();
        }

        public EntrySetBuilder<TEntry> ItemEntry([NotNull] Action<EntryBuilder<TEntry>> buildAction)
        {
            return this;
        }
    }

}
