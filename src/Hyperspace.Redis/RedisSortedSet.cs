using StackExchange.Redis;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hyperspace.Redis.Metadata;

namespace Hyperspace.Redis
{
    [RedisEntryType(RedisEntryType.SortedSet)]
    public class RedisSortedSet : RedisEntry
    {
        public RedisSortedSet(RedisKey key, RedisEntryMetadata metadata, RedisContext context, RedisEntry parent) : base(key, metadata, context, parent)
        {
        }

        #region Add

        public bool Add(RedisValue member, double score, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.SortedSetAdd(Key, member, score, flags);
        }

        public Task<bool> AddAsync(RedisValue member, double score, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.SortedSetAddAsync(Key, member, score, flags);
        }

        public long Add(SortedSetEntry[] values, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.SortedSetAdd(Key, values, flags);
        }

        public Task<long> AddAsync(SortedSetEntry[] values, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.SortedSetAddAsync(Key, values, flags);
        }

        #endregion

        #region Combine And Store

        public long CombineAndStore(SetOperation operation, RedisKey first, RedisKey second,
            Aggregate aggregate = Aggregate.Sum, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.SortedSetCombineAndStore(operation, Key, first, second, aggregate, flags);
        }

        public Task<long> CombineAndStoreAsync(SetOperation operation, RedisKey destination, RedisKey first,
            RedisKey second, Aggregate aggregate = Aggregate.Sum, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.SortedSetCombineAndStoreAsync(operation, Key, first, second, aggregate, flags);
        }

        public long CombineAndStore(SetOperation operation, RedisKey destination, RedisKey[] keys,
            double[] weights = null, Aggregate aggregate = Aggregate.Sum, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.SortedSetCombineAndStore(operation, Key, keys, weights, aggregate, flags);
        }

        public Task<long> CombineAndStoreAsync(SetOperation operation, RedisKey destination, RedisKey[] keys,
            double[] weights = null, Aggregate aggregate = Aggregate.Sum, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.SortedSetCombineAndStoreAsync(operation, Key, keys, weights, aggregate, flags);
        }

        #endregion

        #region Increment & Decrement

        public double Increment(RedisValue member, double value, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.SortedSetIncrement(Key, member, value, flags);
        }

        public Task<double> IncrementAsync(RedisValue member, double value, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.SortedSetIncrementAsync(Key, member, value, flags);
        }

        public double Decrement(RedisValue member, double value, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.SortedSetDecrement(Key, member, value, flags);
        }

        public Task<double> DecrementAsync(RedisValue member, double value, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.SortedSetDecrementAsync(Key, member, value, flags);
        }

        #endregion

        #region Length

        public long Length(double min = double.NegativeInfinity, double max = double.PositiveInfinity, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.SortedSetLength(Key, min, max, exclude, flags);
        }

        public Task<long> LengthAsync(double min = double.NegativeInfinity, double max = double.PositiveInfinity, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.SortedSetLengthAsync(Key, min, max, exclude, flags);
        }

        public long LengthByValue(RedisValue min, RedisValue max, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.SortedSetLengthByValue(Key, min, max, exclude, flags);
        }

        public Task<long> LengthByValueAsync(RedisValue min, RedisValue max, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.SortedSetLengthByValueAsync(Key, min, max, exclude, flags);
        }

        #endregion

        #region Range By

        public RedisValue[] RangeByRank(long start = 0, long stop = -1, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.SortedSetRangeByRank(Key, start, stop, order, flags);
        }

        public Task<RedisValue[]> RangeByRankAsync(long start = 0, long stop = -1, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.SortedSetRangeByRankAsync(Key, start, stop, order, flags);
        }

        public RedisValue[] RangeByScore(double start = double.NegativeInfinity, double stop = double.PositiveInfinity, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.SortedSetRangeByScore(Key, start, stop, exclude, order, skip, take, flags);
        }

        public Task<RedisValue[]> RangeByScoreAsync(double start = double.NegativeInfinity, double stop = double.PositiveInfinity, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.SortedSetRangeByScoreAsync(Key, start, stop, exclude, order, skip, take, flags);
        }

        public SortedSetEntry[] RangeByRankWithScores(long start = 0, long stop = -1, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.SortedSetRangeByRankWithScores(Key, start, stop, order, flags);
        }

        public Task<SortedSetEntry[]> RangeByRankWithScoresAsync(long start = 0, long stop = -1, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.SortedSetRangeByRankWithScoresAsync(Key, start, stop, order, flags);
        }

        public RedisValue[] RangeByValue(RedisValue min = default(RedisValue), RedisValue max = default(RedisValue), Exclude exclude = Exclude.None, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.SortedSetRangeByValue(Key, min, max, exclude, skip, take, flags);
        }

        public Task<RedisValue[]> RangeByValueAsync(RedisValue min = default(RedisValue), RedisValue max = default(RedisValue), Exclude exclude = Exclude.None, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.SortedSetRangeByValueAsync(Key, min, max, exclude, skip, take, flags);
        }

        public SortedSetEntry[] RangeByScoreWithScores(double start = double.NegativeInfinity, double stop = double.PositiveInfinity, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.SortedSetRangeByScoreWithScores(Key, start, stop, exclude, order, skip, take, flags);
        }

        public Task<SortedSetEntry[]> RangeByScoreWithScoresAsync(double start = double.NegativeInfinity, double stop = double.PositiveInfinity, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.SortedSetRangeByScoreWithScoresAsync(Key, start, stop, exclude, order, skip, take, flags);
        }

        #endregion

        #region Rank

        public long? Rank(RedisValue member, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.SortedSetRank(Key, member, order, flags);
        }

        public Task<long?> RankAsync(RedisValue member, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.SortedSetRankAsync(Key, member, order, flags);
        }

        #endregion

        #region Remove

        public bool Remove(RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.SortedSetRemove(Key, member, flags);
        }

        public Task<bool> RemoveAsync(RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.SortedSetRemoveAsync(Key, member, flags);
        }

        public long Remove(RedisValue[] members, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.SortedSetRemove(Key, members, flags);
        }

        public Task<long> RemoveAsync(RedisValue[] members, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.SortedSetRemoveAsync(Key, members, flags);
        }

        #endregion

        #region Remove Range By

        public long RemoveRangeByRank(long start, long stop, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.SortedSetRemoveRangeByRank(Key, start, stop, flags);
        }

        public Task<long> RemoveRangeByRankAsync(long start, long stop, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.SortedSetRemoveRangeByRankAsync(Key, start, stop, flags);
        }

        public long RemoveRangeByScore(double start, double stop, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.SortedSetRemoveRangeByScore(Key, start, stop, exclude, flags);
        }

        public Task<long> RemoveRangeByScoreAsync(double start, double stop, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.SortedSetRemoveRangeByScoreAsync(Key, start, stop, exclude, flags);
        }

        public long RemoveRangeByValue(RedisValue min, RedisValue max, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.SortedSetRemoveRangeByValue(Key, min, max, exclude, flags);
        }

        public Task<long> RemoveRangeByValueAsync(RedisValue min, RedisValue max, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.SortedSetRemoveRangeByValueAsync(Key, min, max, exclude, flags);
        }

        #endregion

        #region Scan

        public IEnumerable<SortedSetEntry> Scan(RedisValue pattern, int pageSize, CommandFlags flags)
        {
            return RedisSync.SortedSetScan(Key, pattern, pageSize, flags);
        }

        public IEnumerable<SortedSetEntry> Scan(RedisValue pattern = default(RedisValue), int pageSize = 10, long cursor = 0, int pageOffset = 0, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.SortedSetScan(Key, pattern, pageSize, cursor, pageOffset, flags);
        }

        #endregion

        #region Score

        public double? Score(RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.SortedSetScore(Key, member, flags);
        }

        public Task<double?> ScoreAsync(RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.SortedSetScoreAsync(Key, member, flags);
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
