using Hyperspace.Redis.Infrastructure;
using Microsoft.Framework.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Hyperspace.Redis
{
    [RedisEntryType(RedisEntryType.Hash)]
    public class RedisHash : RedisEntry
    {
        public RedisHash(RedisContext context, RedisKey key) : base(context, key, RedisEntryType.Hash)
        {
            Converter = new Lazy<IRedisValueConverter>(() => Context.GetRequiredService<IRedisValueConverter>());
        }

        protected readonly Lazy<IRedisValueConverter> Converter;

        #region Increment & Decrement

        public long Increment(RedisValue hashField, long value = 1, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.HashIncrement(Key, hashField, value, flags);
        }

        public double Increment(RedisValue hashField, double value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.HashIncrement(Key, hashField, value, flags);
        }

        public Task<double> IncrementAsync(RedisValue hashField, double value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.HashIncrementAsync(Key, hashField, value, flags);
        }

        public Task<long> IncrementAsync(RedisValue hashField, long value = 1, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.HashIncrementAsync(Key, hashField, value, flags);
        }

        public long Decrement(RedisValue hashField, long value = 1, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.HashDecrement(Key, hashField, value, flags);
        }

        public double Decrement(RedisValue hashField, double value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.HashDecrement(Key, hashField, value, flags);
        }

        public Task<long> DecrementAsync(RedisValue hashField, long value = 1, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.HashDecrementAsync(Key, hashField, value, flags);
        }

        public Task<double> DecrementAsync(RedisValue hashField, double value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.HashDecrementAsync(Key, hashField, value, flags);
        }

        #endregion

        #region Delete

        public bool Delete(RedisValue hashField, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.HashDelete(Key, hashField, flags);
        }

        public long Delete(RedisValue[] hashFields, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.HashDelete(Key, hashFields, flags);
        }

        public Task<bool> HashDeleteAsync(RedisValue hashField, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.HashDeleteAsync(Key, hashField, flags);
        }

        public Task<long> HashDeleteAsync(RedisValue[] hashFields, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.HashDeleteAsync(Key, hashFields, flags);
        }

        #endregion

        #region Exists

        public bool Exists(RedisValue hashField, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.HashExists(Key, hashField, flags);
        }

        public Task<bool> ExistsAsync(RedisValue hashField, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.HashExistsAsync(Key, hashField, flags);
        }

        #endregion

        #region Get & Set

        public RedisValue Get(RedisValue hashField, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.HashGet(Key, hashField, flags);
        }

        public Task<RedisValue> GetAsync(RedisValue hashField, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.HashGetAsync(Key, hashField, flags);
        }

        public RedisValue[] Get(RedisValue[] hashFields, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.HashGet(Key, hashFields, flags);
        }

        public Task<RedisValue[]> GetAsync(RedisValue[] hashFields, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.HashGetAsync(Key, hashFields, flags);
        }

        public HashEntry[] GetAll(CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.HashGetAll(Key, flags);
        }

        public Task<HashEntry[]> GetAllAsync(CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.HashGetAllAsync(Key, flags);
        }

        public void Set(HashEntry[] hashFields, CommandFlags flags = CommandFlags.None)
        {
            Context.Database.HashSet(Key, hashFields, flags);
        }

        public bool Set(RedisValue hashField, RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.HashSet(Key, hashField, value, when, flags);
        }

        public Task SetAsync(HashEntry[] hashFields, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.HashSetAsync(Key, hashFields, flags);
        }

        public Task<bool> SetAsync(RedisValue hashField, RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.HashSetAsync(Key, hashField, value, when, flags);
        }

        #endregion

        #region Keys & Values

        public RedisValue[] Keys(CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.HashKeys(Key, flags);
        }

        public Task<RedisValue[]> KeysAsync(CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.HashKeysAsync(Key, flags);
        }

        public RedisValue[] Values(CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.HashValues(Key, flags);
        }

        public Task<RedisValue[]> ValuesAsync(CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.HashValuesAsync(Key, flags);
        }

        #endregion

        #region Scan

        public IEnumerable<HashEntry> HashScan(RedisValue pattern, int pageSize, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.HashScan(Key, pattern, pageSize, flags);
        }

        public IEnumerable<HashEntry> HashScan(RedisValue pattern = default(RedisValue), int pageSize = 10, long cursor = 0, int pageOffset = 0, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.HashScan(Key, pattern, pageSize, cursor, pageOffset, flags);
        }

        #endregion

        #region Length

        public long Length(CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.HashLength(Key, flags);
        }

        public Task<long> LengthAsync(CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.HashLengthAsync(Key, flags);
        }

        #endregion

        #region Get & Set Property

        protected T GetProperty<T>([CallerMemberName] string propertyName = null)
        {
            return Converter.Value.Deserialize<T>(Get(propertyName));
        }

        protected void SetProperty<T>(T value, [CallerMemberName] string propertyName = null)
        {
            Set(propertyName, Converter.Value.Serialize(value));
        }

        #endregion

    }
}
