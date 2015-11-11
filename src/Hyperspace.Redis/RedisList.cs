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
            Context.Database.ListSetByIndex(Key, start, stop, flags);
        }

        public Task TrimAsync(long start, long stop, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListTrimAsync(Key, start, stop, flags);
        }

        #endregion

        #region Range

        public RedisValue[] Range(long start = 0, long stop = -1, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListRange(Key, start, stop, flags);
        }

        public Task<RedisValue[]> RangeAsync(long start = 0, long stop = -1, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListRangeAsync(Key, start, stop, flags);
        }

        #endregion

        #region Insert

        public long InsertAfter(RedisValue pivot, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListInsertAfter(Key, pivot, value, flags);
        }

        public Task<long> InsertAfterAsync(RedisValue pivot, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListInsertAfterAsync(Key, pivot, value, flags);
        }

        public long InsertBefore(RedisValue pivot, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListInsertBefore(Key, pivot, value, flags);
        }

        public Task<long> InsertBeforeAsync(RedisValue pivot, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListInsertBeforeAsync(Key, pivot, value, flags);
        }

        #endregion

        #region Remove

        public long Remove(RedisValue value, long count = 0, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListRemove(Key, value, count, flags);
        }

        public Task<long> RemoveAsync(RedisValue value, long count = 0, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListRemoveAsync(Key, value, count, flags);
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

        public RedisValue GetByIndex(long index, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListGetByIndex(Key, index, flags);
        }

        public Task<RedisValue> GetByIndexAsync(long index, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListGetByIndexAsync(Key, index, flags);
        }

        public void SetByIndex(long index, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            Context.Database.ListSetByIndex(Key, index, value, flags);
        }

        public Task SetByIndexAsync(long index, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListSetByIndexAsync(Key, index, value, flags);
        }

        #endregion

        #region Left Push & Pop

        public long LeftPush(RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListLeftPush(Key, values, flags);
        }

        public Task<long> LeftPushAsync(RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListLeftPushAsync(Key, values, flags);
        }

        public long LeftPush(RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListLeftPush(Key, value, when, flags);
        }

        public Task<long> LeftPushAsync(RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListLeftPushAsync(Key, value, when, flags);
        }

        public RedisValue LeftPop(CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListLeftPop(Key, flags);
        }

        public Task<RedisValue> LeftPopAsync(CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListLeftPopAsync(Key, flags);
        }

        #endregion

        #region Right Push & Pop

        public long RightPush(RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListRightPush(Key, values, flags);
        }

        public Task<long> RightPushAsync(RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListRightPushAsync(Key, values, flags);
        }

        public long RightPush(RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListRightPush(Key, value, when, flags);
        }

        public Task<long> RightPushAsync(RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListRightPushAsync(Key, value, when, flags);
        }

        public RedisValue RightPop(CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListRightPop(Key, flags);
        }

        public Task<RedisValue> RightPopAsync(CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListRightPopAsync(Key, flags);
        }

        #endregion

        #region Right Pop Left Push

        public RedisValue RightPopLeftPush(RedisKey destination, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListRightPopLeftPush(Key, destination, flags);
        }

        public Task<RedisValue> RightPopLeftPushAsync(RedisKey destination, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListRightPopLeftPushAsync(Key, destination, flags);
        }

        #endregion

        #region Sort

        public RedisValue[] Sort(long skip = 0, long take = -1, Order order = Order.Ascending,
            SortType sortType = SortType.Numeric, RedisValue by = default(RedisValue), RedisValue[] get = null,
            CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.Sort(Key, skip, take, order, sortType, by, get, flags);
        }

        public Task<RedisValue[]> SortAsync(long skip = 0, long take = -1, Order order = Order.Ascending,
            SortType sortType = SortType.Numeric, RedisValue by = default(RedisValue), RedisValue[] get = null,
            CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortAsync(Key, skip, take, order, sortType, by, get, flags);
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
