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
            return Context.Database.SortedSetAdd(Key, member, score, flags);
        }

        public Task<bool> AddAsync(RedisValue member, double score, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetAddAsync(Key, member, score, flags);
        }

        public long Add(SortedSetEntry[] values, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetAdd(Key, values, flags);
        }

        public Task<long> AddAsync(SortedSetEntry[] values, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetAddAsync(Key, values, flags);
        }

        #endregion

        #region Combine And Store

        public long CombineAndStore(SetOperation operation, RedisKey first, RedisKey second,
            Aggregate aggregate = Aggregate.Sum, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetCombineAndStore(operation, Key, first, second, aggregate, flags);
        }

        public Task<long> CombineAndStoreAsync(SetOperation operation, RedisKey destination, RedisKey first,
            RedisKey second, Aggregate aggregate = Aggregate.Sum, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetCombineAndStoreAsync(operation, Key, first, second, aggregate, flags);
        }

        public long CombineAndStore(SetOperation operation, RedisKey destination, RedisKey[] keys,
            double[] weights = null, Aggregate aggregate = Aggregate.Sum, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetCombineAndStore(operation, Key, keys, weights, aggregate, flags);
        }

        public Task<long> CombineAndStoreAsync(SetOperation operation, RedisKey destination, RedisKey[] keys,
            double[] weights = null, Aggregate aggregate = Aggregate.Sum, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetCombineAndStoreAsync(operation, Key, keys, weights, aggregate, flags);
        }

        #endregion

        #region Increment & Decrement

        public double Increment(RedisValue member, double value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetIncrement(Key, member, value, flags);
        }

        public Task<double> IncrementAsync(RedisValue member, double value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetIncrementAsync(Key, member, value, flags);
        }

        public double Decrement(RedisValue member, double value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetDecrement(Key, member, value, flags);
        }

        public Task<double> DecrementAsync(RedisValue member, double value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetDecrementAsync(Key, member, value, flags);
        }

        #endregion

        #region Length

        public long Length(double min = double.NegativeInfinity, double max = double.PositiveInfinity, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetLength(Key, min, max, exclude, flags);
        }

        public Task<long> LengthAsync(double min = double.NegativeInfinity, double max = double.PositiveInfinity, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetLengthAsync(Key, min, max, exclude, flags);
        }

        public long LengthByValue(RedisValue min, RedisValue max, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetLengthByValue(Key, min, max, exclude, flags);
        }

        public Task<long> LengthByValueAsync(RedisValue min, RedisValue max, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetLengthByValueAsync(Key, min, max, exclude, flags);
        }

        #endregion

        #region Range By

        public RedisValue[] RangeByRank(long start = 0, long stop = -1, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetRangeByRank(Key, start, stop, order, flags);
        }

        public Task<RedisValue[]> RangeByRankAsync(long start = 0, long stop = -1, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetRangeByRankAsync(Key, start, stop, order, flags);
        }

        public RedisValue[] RangeByScore(double start = double.NegativeInfinity, double stop = double.PositiveInfinity, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetRangeByScore(Key, start, stop, exclude, order, skip, take, flags);
        }

        public Task<RedisValue[]> RangeByScoreAsync(double start = double.NegativeInfinity, double stop = double.PositiveInfinity, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetRangeByScoreAsync(Key, start, stop, exclude, order, skip, take, flags);
        }

        public SortedSetEntry[] RangeByRankWithScores(long start = 0, long stop = -1, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetRangeByRankWithScores(Key, start, stop, order, flags);
        }

        public Task<SortedSetEntry[]> RangeByRankWithScoresAsync(long start = 0, long stop = -1, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetRangeByRankWithScoresAsync(Key, start, stop, order, flags);
        }

        public RedisValue[] RangeByValue(RedisValue min = default(RedisValue), RedisValue max = default(RedisValue), Exclude exclude = Exclude.None, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetRangeByValue(Key, min, max, exclude, skip, take, flags);
        }

        public Task<RedisValue[]> RangeByValueAsync(RedisValue min = default(RedisValue), RedisValue max = default(RedisValue), Exclude exclude = Exclude.None, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetRangeByValueAsync(Key, min, max, exclude, skip, take, flags);
        }

        public SortedSetEntry[] RangeByScoreWithScores(double start = double.NegativeInfinity, double stop = double.PositiveInfinity, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetRangeByScoreWithScores(Key, start, stop, exclude, order, skip, take, flags);
        }

        public Task<SortedSetEntry[]> RangeByScoreWithScoresAsync(double start = double.NegativeInfinity, double stop = double.PositiveInfinity, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetRangeByScoreWithScoresAsync(Key, start, stop, exclude, order, skip, take, flags);
        }

        #endregion

        #region Rank

        public long? Rank(RedisValue member, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetRank(Key, member, order, flags);
        }

        public Task<long?> RankAsync(RedisValue member, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetRankAsync(Key, member, order, flags);
        }

        #endregion

        #region Remove

        public bool Remove(RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetRemove(Key, member, flags);
        }

        public Task<bool> RemoveAsync(RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetRemoveAsync(Key, member, flags);
        }

        public long Remove(RedisValue[] members, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetRemove(Key, members, flags);
        }

        public Task<long> RemoveAsync(RedisValue[] members, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetRemoveAsync(Key, members, flags);
        }

        #endregion

        #region Remove Range By

        public long RemoveRangeByRank(long start, long stop, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetRemoveRangeByRank(Key, start, stop, flags);
        }

        public Task<long> RemoveRangeByRankAsync(long start, long stop, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetRemoveRangeByRankAsync(Key, start, stop, flags);
        }

        public long RemoveRangeByScore(double start, double stop, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetRemoveRangeByScore(Key, start, stop, exclude, flags);
        }

        public Task<long> RemoveRangeByScoreAsync(double start, double stop, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetRemoveRangeByScoreAsync(Key, start, stop, exclude, flags);
        }

        public long RemoveRangeByValue(RedisValue min, RedisValue max, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetRemoveRangeByValue(Key, min, max, exclude, flags);
        }

        public Task<long> RemoveRangeByValueAsync(RedisValue min, RedisValue max, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetRemoveRangeByValueAsync(Key, min, max, exclude, flags);
        }

        #endregion

        #region Scan

        public IEnumerable<SortedSetEntry> Scan(RedisValue pattern, int pageSize, CommandFlags flags)
        {
            return Context.Database.SortedSetScan(Key, pattern, pageSize, flags);
        }

        public IEnumerable<SortedSetEntry> Scan(RedisValue pattern = default(RedisValue), int pageSize = 10, long cursor = 0, int pageOffset = 0, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetScan(Key, pattern, pageSize, cursor, pageOffset, flags);
        }

        #endregion

        #region Score

        public double? Score(RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetScore(Key, member, flags);
        }

        public Task<double?> ScoreAsync(RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetScoreAsync(Key, member, flags);
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
