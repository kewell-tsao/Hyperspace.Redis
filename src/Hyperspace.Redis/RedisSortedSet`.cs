using Hyperspace.Redis.Infrastructure;
using Microsoft.Framework.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperspace.Redis
{
    [RedisEntryType(RedisEntryType.SortedSet)]
    public class RedisSortedSet<T> : RedisEntry
    {
        public RedisSortedSet(RedisContext context, RedisKey key) : base(context, key, RedisEntryType.SortedSet)
        {
            Converter = new Lazy<IRedisValueConverter>(() => Context.GetRequiredService<IRedisValueConverter>());
        }

        protected readonly Lazy<IRedisValueConverter> Converter;

        #region Add

        public bool Add(T member, double score, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetAdd(Key, Converter.Value.Serialize(member), score, flags);
        }

        public Task<bool> AddAsync(T member, double score, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetAddAsync(Key, Converter.Value.Serialize(member), score, flags);
        }

        public long Add(IEnumerable<RedisSortedSetEntry<T>> values, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetAdd(Key, values.Select(e => new SortedSetEntry(Converter.Value.Serialize(e.Element), e.Score)).ToArray(), flags);
        }

        public Task<long> AddAsync(IEnumerable<RedisSortedSetEntry<T>> values, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetAddAsync(Key, values.Select(e => new SortedSetEntry(Converter.Value.Serialize(e.Element), e.Score)).ToArray(), flags);
        }

        #endregion

        #region Combine And Store

        public long CombineAndStore(SetOperation operation, RedisKey first, RedisKey second, Aggregate aggregate = Aggregate.Sum, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetCombineAndStore(operation, Key, first, second, aggregate, flags);
        }

        public Task<long> CombineAndStoreAsync(SetOperation operation, RedisKey destination, RedisKey first, RedisKey second, Aggregate aggregate = Aggregate.Sum, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetCombineAndStoreAsync(operation, Key, first, second, aggregate, flags);
        }

        public long CombineAndStore(SetOperation operation, RedisKey destination, RedisKey[] keys, double[] weights = null, Aggregate aggregate = Aggregate.Sum, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetCombineAndStore(operation, Key, keys, weights, aggregate, flags);
        }

        public Task<long> CombineAndStoreAsync(SetOperation operation, RedisKey destination, RedisKey[] keys, double[] weights = null, Aggregate aggregate = Aggregate.Sum, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetCombineAndStoreAsync(operation, Key, keys, weights, aggregate, flags);
        }

        #endregion

        #region Increment & Decrement

        public double Increment(T member, double value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetIncrement(Key, Converter.Value.Serialize(member), value, flags);
        }

        public Task<double> IncrementAsync(T member, double value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetIncrementAsync(Key, Converter.Value.Serialize(member), value, flags);
        }

        public double Decrement(T member, double value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetDecrement(Key, Converter.Value.Serialize(member), value, flags);
        }

        public Task<double> DecrementAsync(T member, double value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetDecrementAsync(Key, Converter.Value.Serialize(member), value, flags);
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

        public long LengthByValue(T min, T max, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetLengthByValue(Key, Converter.Value.Serialize(min), Converter.Value.Serialize(max), exclude, flags);
        }

        public Task<long> LengthByValueAsync(T min, T max, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetLengthByValueAsync(Key, Converter.Value.Serialize(min), Converter.Value.Serialize(max), exclude, flags);
        }

        #endregion

        #region Range By

        public IEnumerable<T> RangeByRank(long start = 0, long stop = -1, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetRangeByRank(Key, start, stop, order, flags).Select(Converter.Value.Deserialize<T>);
        }

        public async Task<IEnumerable<T>> RangeByRankAsync(long start = 0, long stop = -1, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            return (await Context.Database.SortedSetRangeByRankAsync(Key, start, stop, order, flags)).Select(Converter.Value.Deserialize<T>);
        }

        public IEnumerable<T> RangeByScore(double start = double.NegativeInfinity, double stop = double.PositiveInfinity, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetRangeByScore(Key, start, stop, exclude, order, skip, take, flags).Select(Converter.Value.Deserialize<T>);
        }

        public async Task<IEnumerable<T>> RangeByScoreAsync(double start = double.NegativeInfinity, double stop = double.PositiveInfinity, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            return (await Context.Database.SortedSetRangeByScoreAsync(Key, start, stop, exclude, order, skip, take, flags)).Select(Converter.Value.Deserialize<T>);
        }

        public IEnumerable<RedisSortedSetEntry<T>> RangeByRankWithScores(long start = 0, long stop = -1, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetRangeByRankWithScores(Key, start, stop, order, flags).Select(e => new RedisSortedSetEntry<T>(Converter.Value.Deserialize<T>(e.Element), e.Score));
        }

        public async Task<IEnumerable<RedisSortedSetEntry<T>>> RangeByRankWithScoresAsync(long start = 0, long stop = -1, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            return (await Context.Database.SortedSetRangeByRankWithScoresAsync(Key, start, stop, order, flags)).Select(e => new RedisSortedSetEntry<T>(Converter.Value.Deserialize<T>(e.Element), e.Score));
        }

        public IEnumerable<T> RangeByValue(T min, T max, Exclude exclude = Exclude.None, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetRangeByValue(Key, Converter.Value.Serialize(min), Converter.Value.Serialize(max), exclude, skip, take, flags).Select(Converter.Value.Deserialize<T>);
        }

        public async Task<IEnumerable<T>> RangeByValueAsync(T min, T max, Exclude exclude = Exclude.None, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            return (await Context.Database.SortedSetRangeByValueAsync(Key, Converter.Value.Serialize(min), Converter.Value.Serialize(max), exclude, skip, take, flags)).Select(Converter.Value.Deserialize<T>);
        }

        public IEnumerable<RedisSortedSetEntry<T>> RangeByScoreWithScores(double start = double.NegativeInfinity, double stop = double.PositiveInfinity, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetRangeByScoreWithScores(Key, start, stop, exclude, order, skip, take, flags).Select(e => new RedisSortedSetEntry<T>(Converter.Value.Deserialize<T>(e.Element), e.Score));
        }

        public async Task<IEnumerable<RedisSortedSetEntry<T>>> RangeByScoreWithScoresAsync(double start = double.NegativeInfinity, double stop = double.PositiveInfinity, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            return (await Context.Database.SortedSetRangeByScoreWithScoresAsync(Key, start, stop, exclude, order, skip, take, flags)).Select(e => new RedisSortedSetEntry<T>(Converter.Value.Deserialize<T>(e.Element), e.Score));
        }

        #endregion

        #region Rank

        public long? Rank(T member, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetRank(Key, Converter.Value.Serialize(member), order, flags);
        }

        public Task<long?> RankAsync(T member, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetRankAsync(Key, Converter.Value.Serialize(member), order, flags);
        }

        #endregion

        #region Remove

        public bool Remove(T member, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetRemove(Key, Converter.Value.Serialize(member), flags);
        }

        public Task<bool> RemoveAsync(T member, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetRemoveAsync(Key, Converter.Value.Serialize(member), flags);
        }

        public long Remove(IEnumerable<T> members, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetRemove(Key, members.Select(Converter.Value.Serialize).ToArray(), flags);
        }

        public Task<long> RemoveAsync(IEnumerable<T> members, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetRemoveAsync(Key, members.Select(Converter.Value.Serialize).ToArray(), flags);
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

        public long RemoveRangeByValue(T min, T max, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetRemoveRangeByValue(Key, Converter.Value.Serialize(min), Converter.Value.Serialize(max), exclude, flags);
        }

        public Task<long> RemoveRangeByValueAsync(T min, T max, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetRemoveRangeByValueAsync(Key, Converter.Value.Serialize(min), Converter.Value.Serialize(max), exclude, flags);
        }

        #endregion

        #region Scan

        public IEnumerable<RedisSortedSetEntry<T>> Scan(RedisValue pattern, int pageSize, CommandFlags flags)
        {
            return Context.Database.SortedSetScan(Key, pattern, pageSize, flags).Select(e => new RedisSortedSetEntry<T>(Converter.Value.Deserialize<T>(e.Element), e.Score));
        }

        public IEnumerable<RedisSortedSetEntry<T>> Scan(RedisValue pattern = default(RedisValue), int pageSize = 10, long cursor = 0, int pageOffset = 0, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetScan(Key, pattern, pageSize, cursor, pageOffset, flags).Select(e => new RedisSortedSetEntry<T>(Converter.Value.Deserialize<T>(e.Element), e.Score));
        }

        #endregion

        #region Score

        public double? Score(T member, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetScore(Key, Converter.Value.Serialize(member), flags);
        }

        public Task<double?> ScoreAsync(T member, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SortedSetScoreAsync(Key, Converter.Value.Serialize(member), flags);
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
