using Hyperspace.Redis.Infrastructure;
using Microsoft.Framework.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hyperspace.Redis.Metadata;

namespace Hyperspace.Redis
{
    [RedisEntryType(RedisEntryType.List)]
    public class RedisList<T> : RedisEntry
    {
        static RedisList()
        {
            var type = typeof(T);
            if (type == typeof(RedisKey) || type == typeof(RedisValue))
                throw new InvalidOperationException();
        }

        public RedisList(RedisKey key, RedisEntryMetadata metadata, RedisContext context, RedisEntry parent) : base(key, metadata, context, parent)
        {
            Converter = new Lazy<IRedisValueConverter>(() => Context.GetRequiredService<IRedisValueConverter>());
        }

        protected readonly Lazy<IRedisValueConverter> Converter;

        #region Trim

        public void Trim(long start, long stop, CommandFlags flags = CommandFlags.None)
        {
            Context.Database.ListSetByIndex(Key, start, stop, flags);
        }

        public Task TrimAsync(long start, long stop, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListTrimAsync(Key, start, stop, flags);
        }

        #endregion

        #region Range

        public IEnumerable<T> Range(long start = 0, long stop = -1, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListRange(Key, start, stop, flags).Select(Converter.Value.Deserialize<T>);
        }

        public async Task<IEnumerable<T>> RangeAsync(long start = 0, long stop = -1, CommandFlags flags = CommandFlags.None)
        {
            return (await Context.Database.ListRangeAsync(Key, start, stop, flags)).Select(Converter.Value.Deserialize<T>);
        }

        #endregion

        #region Insert

        public long InsertAfter(T pivot, T value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListInsertAfter(Key, Converter.Value.Serialize(pivot), Converter.Value.Serialize(value), flags);
        }

        public Task<long> InsertAfterAsync(T pivot, T value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListInsertAfterAsync(Key, Converter.Value.Serialize(pivot), Converter.Value.Serialize(value), flags);
        }

        public long InsertBefore(T pivot, T value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListInsertBefore(Key, Converter.Value.Serialize(pivot), Converter.Value.Serialize(value), flags);
        }

        public Task<long> InsertBeforeAsync(T pivot, T value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListInsertBeforeAsync(Key, Converter.Value.Serialize(pivot), Converter.Value.Serialize(value), flags);
        }

        #endregion

        #region Remove

        public long Remove(T value, long count = 0, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListRemove(Key, Converter.Value.Serialize(value), count, flags);
        }

        public Task<long> RemoveAsync(T value, long count = 0, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListRemoveAsync(Key, Converter.Value.Serialize(value), count, flags);
        }

        #endregion

        #region Length

        public long Length(CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListLength(Key, flags);
        }

        public Task<long> LengthAsync(CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListLengthAsync(Key, flags);
        }

        #endregion

        #region Get & Set By Index

        public T GetByIndex(long index, CommandFlags flags = CommandFlags.None)
        {
            return Converter.Value.Deserialize<T>(Context.Database.ListGetByIndex(Key, index, flags));
        }

        public async Task<T> GetByIndexAsync(long index, CommandFlags flags = CommandFlags.None)
        {
            return Converter.Value.Deserialize<T>(await Context.Database.ListGetByIndexAsync(Key, index, flags));
        }

        public void SetByIndex(long index, T value, CommandFlags flags = CommandFlags.None)
        {
            Context.Database.ListSetByIndex(Key, index, Converter.Value.Serialize(value), flags);
        }

        public Task SetByIndexAsync(long index, T value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListSetByIndexAsync(Key, index, Converter.Value.Serialize(value), flags);
        }

        #endregion

        #region Left Push & Pop

        public long LeftPush(IEnumerable<T> values, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListLeftPush(Key, values.Select(Converter.Value.Serialize).ToArray(), flags);
        }

        public Task<long> LeftPushAsync(IEnumerable<T> values, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListLeftPushAsync(Key, values.Select(Converter.Value.Serialize).ToArray(), flags);
        }

        public long LeftPush(T value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListLeftPush(Key, Converter.Value.Serialize(value), when, flags);
        }

        public Task<long> LeftPushAsync(T value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListLeftPushAsync(Key, Converter.Value.Serialize(value), when, flags);
        }

        public T LeftPop(CommandFlags flags = CommandFlags.None)
        {
            return Converter.Value.Deserialize<T>(Context.Database.ListLeftPop(Key, flags));
        }

        public async Task<T> LeftPopAsync(CommandFlags flags = CommandFlags.None)
        {
            return Converter.Value.Deserialize<T>(await Context.Database.ListLeftPopAsync(Key, flags));
        }

        #endregion

        #region Right Push & Pop

        public long RightPush(IEnumerable<T> values, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListRightPush(Key, values.Select(Converter.Value.Serialize).ToArray(), flags);
        }

        public Task<long> RightPushAsync(IEnumerable<T> values, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListRightPushAsync(Key, values.Select(Converter.Value.Serialize).ToArray(), flags);
        }

        public long RightPush(T value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListRightPush(Key, Converter.Value.Serialize(value), when, flags);
        }

        public Task<long> RightPushAsync(T value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListRightPushAsync(Key, Converter.Value.Serialize(value), when, flags);
        }

        public T RightPop(CommandFlags flags = CommandFlags.None)
        {
            return Converter.Value.Deserialize<T>(Context.Database.ListRightPop(Key, flags));
        }

        public async Task<T> RightPopAsync(CommandFlags flags = CommandFlags.None)
        {
            return Converter.Value.Deserialize<T>(await Context.Database.ListRightPopAsync(Key, flags));
        }

        #endregion

        #region Right Pop Left Push

        public T RightPopLeftPush(RedisKey destination, CommandFlags flags = CommandFlags.None)
        {
            return Converter.Value.Deserialize<T>(Context.Database.ListRightPopLeftPush(Key, destination, flags));
        }

        public async Task<T> RightPopLeftPushAsync(RedisKey destination, CommandFlags flags = CommandFlags.None)
        {
            return Converter.Value.Deserialize<T>(await Context.Database.ListRightPopLeftPushAsync(Key, destination, flags));
        }

        #endregion

        #region Sort

        public IEnumerable<T> Sort(long skip = 0, long take = -1, Order order = Order.Ascending,
            SortType sortType = SortType.Numeric, RedisValue by = default(RedisValue), RedisValue[] get = null,
            CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.Sort(Key, skip, take, order, sortType, by, get, flags).Select(Converter.Value.Deserialize<T>);
        }

        public async Task<IEnumerable<T>> SortAsync(long skip = 0, long take = -1, Order order = Order.Ascending,
            SortType sortType = SortType.Numeric, RedisValue by = default(RedisValue), RedisValue[] get = null,
            CommandFlags flags = CommandFlags.None)
        {
            return (await Context.Database.SortAsync(Key, skip, take, order, sortType, by, get, flags)).Select(Converter.Value.Deserialize<T>);
        }

        public long SortAndStore(RedisKey destination, long skip = 0, long take = -1,
            Order order = Order.Ascending, SortType sortType = SortType.Numeric, RedisValue by = default(RedisValue),
            RedisValue[] get = null, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortAndStore(destination, Key, skip, take, order, sortType, by, get, flags);
        }

        public Task<long> SortAndStoreAsync(RedisKey destination, long skip = 0, long take = -1,
            Order order = Order.Ascending, SortType sortType = SortType.Numeric, RedisValue by = default(RedisValue),
            RedisValue[] get = null, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortAndStoreAsync(destination, Key, skip, take, order, sortType, by, get, flags);
        }

        #endregion

    }
}
