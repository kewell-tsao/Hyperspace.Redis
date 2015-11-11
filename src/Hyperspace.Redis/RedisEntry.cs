using JetBrains.Annotations;
using StackExchange.Redis;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Hyperspace.Redis
{
    public abstract class RedisEntry : IEquatable<RedisEntry>
    {
        protected internal RedisEntry([NotNull] RedisEntry parent, RedisKey key, RedisEntryType entryType)
        {
            Check.NotNull(parent, nameof(parent));
            Check.NotNull(parent.Context, nameof(parent), nameof(parent.Context));

            Key = key;
            Parent = parent;
            Context = parent.Context;
            EntryType = entryType;
        }

        protected internal RedisEntry([NotNull] RedisContext context, RedisKey key, RedisEntryType entryType)
        {
            Check.NotNull(context, nameof(context));

            Key = key;
            Parent = null;
            Context = context;
            EntryType = entryType;
        }

        #region Properties

        /// <summary>
        /// Redis条目的键
        /// </summary>
        internal RedisKey Key { get; }

        /// <summary>
        /// Redis条目的父条目
        /// </summary>
        internal RedisEntry Parent { get; }

        /// <summary>
        /// Redis条目的上下文
        /// </summary>
        internal RedisContext Context { get; }

        /// <summary>
        /// Redis条目的类型
        /// </summary>
        internal RedisEntryType EntryType { get; }

        #endregion

        #region Identifier

        private object _identifier;

        protected internal TIdentifier GetIdentifier<TIdentifier>()
        {
            return (TIdentifier)_identifier;
        }

        internal void SetIdentifier<TIdentifier>(TIdentifier identifier)
        {
            _identifier = identifier;
        }

        #endregion

        #region Delete

        public bool Delete(CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.KeyDelete(Key, flags);
        }

        public Task<bool> DeleteAsync(CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.KeyDeleteAsync(Key, flags);
        }

        #endregion

        #region Exists

        public bool Exists(CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.KeyExists(Key, flags);
        }

        public Task<bool> ExistsAsync(CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.KeyExistsAsync(Key, flags);
        }

        #endregion

        #region Expire

        public bool Expire(DateTime? expiry, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.KeyExpire(Key, expiry, flags);
        }

        public bool Expire(TimeSpan? expiry, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.KeyExpire(Key, expiry, flags);
        }

        public Task<bool> ExpireAsync(DateTime? expiry, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.KeyExpireAsync(Key, expiry, flags);
        }

        public Task<bool> ExpireAsync(TimeSpan? expiry, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.KeyExpireAsync(Key, expiry, flags);
        }

        #endregion

        #region Persist

        public bool Persist(CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.KeyPersist(Key, flags);
        }

        public Task<bool> PersistAsync(CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.KeyPersistAsync(Key, flags);
        }

        #endregion

        #region TimeToLive

        public TimeSpan? TimeToLive(CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.KeyTimeToLive(Key, flags);
        }

        public Task<TimeSpan?> TimeToLiveAsync(CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.KeyTimeToLiveAsync(Key, flags);
        }

        #endregion

        #region Equals & GetHashCode & ToString

        public bool Equals(RedisEntry other)
        {
            return other != null && other.Key == Key;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as RedisEntry);
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }

        public override string ToString()
        {
            return Key.ToString();
        }

        #endregion

        #region Operators

        public static explicit operator RedisKey(RedisEntry entry)
        {
            return entry?.Key ?? default(RedisKey);
        }

        public static bool operator ==(RedisEntry x, RedisEntry y)
        {
            if (ReferenceEquals(x, y))
                return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                return false;
            return x.Key == y.Key;
        }

        public static bool operator !=(RedisEntry x, RedisEntry y)
        {
            return !(x == y);
        }

        #endregion

        #region Protected Methods

        protected TEntry GetSubEntry<TEntry>([CallerMemberName] string name = null) where TEntry : RedisEntry
        {
            return Context.GetSubEntry<TEntry>(this, name);
        }

        #endregion

    }
}
