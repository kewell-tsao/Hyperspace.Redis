using StackExchange.Redis;
using System.Threading.Tasks;
using Hyperspace.Redis.Metadata;

namespace Hyperspace.Redis
{
    [RedisEntryType(RedisEntryType.List)]
    public class RedisList : RedisEntry
    {
        public RedisList(RedisKey key, RedisEntryMetadata metadata, RedisContext context, RedisEntry parent) : base(key, metadata, context, parent)
        {
        }

        #region Trim

        public void Trim(long start, long stop, CommandFlags flags = CommandFlags.None)
        {
            RedisSync.ListSetByIndex(Key, start, stop, flags);
        }

        public Task TrimAsync(long start, long stop, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.ListTrimAsync(Key, start, stop, flags);
        }

        #endregion

        #region Range

        public RedisValue[] Range(long start = 0, long stop = -1, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.ListRange(Key, start, stop, flags);
        }

        public Task<RedisValue[]> RangeAsync(long start = 0, long stop = -1, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.ListRangeAsync(Key, start, stop, flags);
        }

        #endregion

        #region Insert

        public long InsertAfter(RedisValue pivot, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.ListInsertAfter(Key, pivot, value, flags);
        }

        public Task<long> InsertAfterAsync(RedisValue pivot, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.ListInsertAfterAsync(Key, pivot, value, flags);
        }

        public long InsertBefore(RedisValue pivot, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.ListInsertBefore(Key, pivot, value, flags);
        }

        public Task<long> InsertBeforeAsync(RedisValue pivot, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.ListInsertBeforeAsync(Key, pivot, value, flags);
        }

        #endregion

        #region Remove

        public long Remove(RedisValue value, long count = 0, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.ListRemove(Key, value, count, flags);
        }

        public Task<long> RemoveAsync(RedisValue value, long count = 0, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.ListRemoveAsync(Key, value, count, flags);
        }

        #endregion

        #region Length

        public long Length(CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.ListLength(Key, flags);
        }

        public Task<long> LengthAsync(CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.ListLengthAsync(Key, flags);
        }

        #endregion

        #region Get & Set By Index

        public RedisValue GetByIndex(long index, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.ListGetByIndex(Key, index, flags);
        }

        public Task<RedisValue> GetByIndexAsync(long index, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.ListGetByIndexAsync(Key, index, flags);
        }

        public void SetByIndex(long index, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            RedisSync.ListSetByIndex(Key, index, value, flags);
        }

        public Task SetByIndexAsync(long index, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.ListSetByIndexAsync(Key, index, value, flags);
        }

        #endregion

        #region Left Push & Pop

        public long LeftPush(RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.ListLeftPush(Key, values, flags);
        }

        public Task<long> LeftPushAsync(RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.ListLeftPushAsync(Key, values, flags);
        }

        public long LeftPush(RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.ListLeftPush(Key, value, when, flags);
        }

        public Task<long> LeftPushAsync(RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.ListLeftPushAsync(Key, value, when, flags);
        }

        public RedisValue LeftPop(CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.ListLeftPop(Key, flags);
        }

        public Task<RedisValue> LeftPopAsync(CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.ListLeftPopAsync(Key, flags);
        }

        #endregion

        #region Right Push & Pop

        public long RightPush(RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.ListRightPush(Key, values, flags);
        }

        public Task<long> RightPushAsync(RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.ListRightPushAsync(Key, values, flags);
        }

        public long RightPush(RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.ListRightPush(Key, value, when, flags);
        }

        public Task<long> RightPushAsync(RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.ListRightPushAsync(Key, value, when, flags);
        }

        public RedisValue RightPop(CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.ListRightPop(Key, flags);
        }

        public Task<RedisValue> RightPopAsync(CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.ListRightPopAsync(Key, flags);
        }

        #endregion

        #region Right Pop Left Push

        public RedisValue RightPopLeftPush(RedisKey destination, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.ListRightPopLeftPush(Key, destination, flags);
        }

        public Task<RedisValue> RightPopLeftPushAsync(RedisKey destination, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.ListRightPopLeftPushAsync(Key, destination, flags);
        }

        #endregion

        #region Sort

        public RedisValue[] Sort(long skip = 0, long take = -1, Order order = Order.Ascending,
            SortType sortType = SortType.Numeric, RedisValue by = default(RedisValue), RedisValue[] get = null,
            CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.Sort(Key, skip, take, order, sortType, by, get, flags);
        }

        public Task<RedisValue[]> SortAsync(long skip = 0, long take = -1, Order order = Order.Ascending,
            SortType sortType = SortType.Numeric, RedisValue by = default(RedisValue), RedisValue[] get = null,
            CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.SortAsync(Key, skip, take, order, sortType, by, get, flags);
        }

        public long SortAndStore(RedisKey destination, long skip = 0, long take = -1,
            Order order = Order.Ascending, SortType sortType = SortType.Numeric, RedisValue by = default(RedisValue),
            RedisValue[] get = null, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.SortAndStore(destination, Key, skip, take, order, sortType, by, get, flags);
        }

        public Task<long> SortAndStoreAsync(RedisKey destination, long skip = 0, long take = -1,
            Order order = Order.Ascending, SortType sortType = SortType.Numeric, RedisValue by = default(RedisValue),
            RedisValue[] get = null, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.SortAndStoreAsync(destination, Key, skip, take, order, sortType, by, get, flags);
        }

        #endregion

    }
}
