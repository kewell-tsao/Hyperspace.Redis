using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using JetBrains.Annotations;
using StackExchange.Redis;

namespace Hyperspace.Redis
{
    public abstract class RedisEntry
    {
        protected internal RedisEntry([NotNull] RedisEntry parent, RedisKey key, RedisEntryType entryType)
        {
            Check.NotNull(parent, nameof(parent));

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

        protected TEntry GetSubEntry<TEntry>(RedisEntry parent, [CallerMemberName] string name = null) where TEntry : RedisEntry
        {
            throw new NotImplementedException();
        }

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

    }

}
