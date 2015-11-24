using System.Collections.Generic;
using System.Threading.Tasks;
using Hyperspace.Redis.Metadata;
using StackExchange.Redis;

namespace Hyperspace.Redis
{
    [RedisEntryType(RedisEntryType.Set)]
    public class RedisSet : RedisEntry
    {
        public RedisSet(RedisKey key, RedisEntryMetadata metadata, RedisContext context, RedisEntry parent) : base(key, metadata, context, parent)
        {
        }

        #region Add

        public bool Add(RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.SetAdd(Key, value, flags);
        }

        public long Add(RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.SetAdd(Key, values, flags);
        }

        public Task<bool> AddAsync(RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.SetAddAsync(Key, value, flags);
        }

        public Task<long> AddAsync(RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.SetAddAsync(Key, values, flags);
        }

        #endregion

        #region Remove

        public bool Remove(RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.SetRemove(Key, value, flags);
        }

        public Task<bool> RemoveAsync(RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.SetRemoveAsync(Key, value, flags);
        }

        public long Remove(RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.SetRemove(Key, values, flags);
        }

        public Task<long> RemoveAsync(RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.SetRemoveAsync(Key, values, flags);
        }

        #endregion

        #region Combine & Store

        public RedisValue[] Combine(SetOperation operation, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.SetCombine(operation, keys, flags);
        }

        public Task<RedisValue[]> CombineAsync(SetOperation operation, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.SetCombineAsync(operation, keys, flags);
        }

        public RedisValue[] Combine(SetOperation operation, RedisKey first, RedisKey second, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.SetCombine(operation, first, second, flags);
        }

        public Task<RedisValue[]> CombineAsync(SetOperation operation, RedisKey first, RedisKey second, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.SetCombineAsync(operation, first, second, flags);
        }

        public long CombineAndStore(SetOperation operation, RedisKey destination, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.SetCombineAndStore(operation, destination, keys, flags);
        }

        public Task<long> CombineAndStoreAsync(SetOperation operation, RedisKey destination, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.SetCombineAndStoreAsync(operation, destination, keys, flags);
        }

        public long CombineAndStore(SetOperation operation, RedisKey destination, RedisKey first, RedisKey second, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.SetCombineAndStore(operation, destination, first, second, flags);
        }

        public Task<long> CombineAndStoreAsync(SetOperation operation, RedisKey destination, RedisKey first, RedisKey second, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.SetCombineAndStoreAsync(operation, destination, first, second, flags);
        }

        #endregion

        #region Contains

        public bool Contains(RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.SetContains(Key, value, flags);
        }

        public Task<bool> ContainsAsync(RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.SetContainsAsync(Key, value, flags);
        }

        #endregion

        #region Length

        public long Length(CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.SetLength(Key, flags);
        }

        public Task<long> LengthAsync(CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.SetLengthAsync(Key, flags);
        }

        #endregion

        #region Members

        public RedisValue[] Members(CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.SetMembers(Key, flags);
        }

        public Task<RedisValue[]> MembersAsync(CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.SetMembersAsync(Key, flags);
        }

        #endregion

        #region Random Members

        public RedisValue RandomMember(CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.SetRandomMember(Key, flags);
        }

        public Task<RedisValue> RandomMemberAsync(CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.SetRandomMemberAsync(Key, flags);
        }

        public RedisValue[] RandomMembers(long count, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.SetRandomMembers(Key, count, flags);
        }

        public Task<RedisValue[]> RandomMembersAsync(long count, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.SetRandomMembersAsync(Key, count, flags);
        }

        #endregion

        #region Move

        public bool Move(RedisKey destination, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.SetMove(Key, destination, value, flags);
        }

        public Task<bool> MoveAsync(RedisKey destination, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.SetMoveAsync(Key, destination, value, flags);
        }

        #endregion

        #region Scan

        public IEnumerable<RedisValue> Scan(RedisValue pattern, int pageSize, CommandFlags flags)
        {
            return RedisSync.SetScan(Key, pattern, pageSize, flags);
        }

        public IEnumerable<RedisValue> Scan(RedisValue pattern = default(RedisValue), int pageSize = 10, long cursor = 0, int pageOffset = 0, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.SetScan(Key, pattern, pageSize, cursor, pageOffset, flags);
        }

        #endregion

        #region Pop

        public RedisValue Pop(CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.SetPop(Key, flags);
        }

        public Task<RedisValue> PopAsync(CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.SetPopAsync(Key, flags);
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
