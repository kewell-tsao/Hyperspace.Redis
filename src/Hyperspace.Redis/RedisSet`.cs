using Hyperspace.Redis.Infrastructure;
using Microsoft.Framework.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperspace.Redis
{
    [RedisEntryType(RedisEntryType.Set)]
    public class RedisSet<T> : RedisEntry
    {
        static RedisSet()
        {
            var type = typeof(T);
            if (type == typeof(RedisKey) || type == typeof(RedisValue))
                throw new InvalidOperationException();
        }

        public RedisSet(RedisContext context, RedisKey key) : base(context, key, RedisEntryType.Set)
        {
            Converter = new Lazy<IRedisValueConverter>(() => Context.GetRequiredService<IRedisValueConverter>());
        }

        protected readonly Lazy<IRedisValueConverter> Converter;

        #region Add

        public bool Add(T value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SetAdd(Key, Converter.Value.Serialize(value), flags);
        }

        public long Add(IEnumerable<T> values, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SetAdd(Key, values.Select(Converter.Value.Serialize).ToArray(), flags);
        }

        public Task<bool> AddAsync(T value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SetAddAsync(Key, Converter.Value.Serialize(value), flags);
        }

        public Task<long> AddAsync(IEnumerable<T> values, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SetAddAsync(Key, values.Select(Converter.Value.Serialize).ToArray(), flags);
        }

        #endregion

        #region Remove

        public bool Remove(T value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SetRemove(Key, Converter.Value.Serialize(value), flags);
        }

        public Task<bool> RemoveAsync(T value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SetRemoveAsync(Key, Converter.Value.Serialize(value), flags);
        }

        public long Remove(IEnumerable<T> values, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SetRemove(Key, values.Select(Converter.Value.Serialize).ToArray(), flags);
        }

        public Task<long> RemoveAsync(IEnumerable<T> values, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SetRemoveAsync(Key, values.Select(Converter.Value.Serialize).ToArray(), flags);
        }

        #endregion

        #region Combine & Store

        public IEnumerable<T> Combine(SetOperation operation, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SetCombine(operation, keys, flags).Select(Converter.Value.Deserialize<T>);
        }

        public async Task<IEnumerable<T>> CombineAsync(SetOperation operation, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            return (await Context.Database.SetCombineAsync(operation, keys, flags)).Select(Converter.Value.Deserialize<T>);
        }

        public IEnumerable<T> Combine(SetOperation operation, RedisKey first, RedisKey second, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SetCombine(operation, first, second, flags).Select(Converter.Value.Deserialize<T>);
        }

        public async Task<IEnumerable<T>> CombineAsync(SetOperation operation, RedisKey first, RedisKey second, CommandFlags flags = CommandFlags.None)
        {
            return (await Context.Database.SetCombineAsync(operation, first, second, flags)).Select(Converter.Value.Deserialize<T>);
        }

        public long CombineAndStore(SetOperation operation, RedisKey destination, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SetCombineAndStore(operation, destination, keys, flags);
        }

        public Task<long> CombineAndStoreAsync(SetOperation operation, RedisKey destination, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SetCombineAndStoreAsync(operation, destination, keys, flags);
        }

        public long CombineAndStore(SetOperation operation, RedisKey destination, RedisKey first, RedisKey second, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SetCombineAndStore(operation, destination, first, second, flags);
        }

        public Task<long> CombineAndStoreAsync(SetOperation operation, RedisKey destination, RedisKey first, RedisKey second, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SetCombineAndStoreAsync(operation, destination, first, second, flags);
        }

        #endregion

        #region Contains

        public bool Contains(T value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SetContains(Key, Converter.Value.Serialize(value), flags);
        }

        public Task<bool> ContainsAsync(T value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SetContainsAsync(Key, Converter.Value.Serialize(value), flags);
        }

        #endregion

        #region Length

        public long Length(CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SetLength(Key, flags);
        }

        public Task<long> LengthAsync(CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SetLengthAsync(Key, flags);
        }

        #endregion

        #region Members

        public IEnumerable<T> Members(CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SetMembers(Key, flags).Select(Converter.Value.Deserialize<T>);
        }

        public async Task<IEnumerable<T>> MembersAsync(CommandFlags flags = CommandFlags.None)
        {
            return (await Context.Database.SetMembersAsync(Key, flags)).Select(Converter.Value.Deserialize<T>);
        }

        #endregion

        #region Random Members

        public T RandomMember(CommandFlags flags = CommandFlags.None)
        {
            return Converter.Value.Deserialize<T>(Context.Database.SetRandomMember(Key, flags));
        }

        public async Task<T> RandomMemberAsync(CommandFlags flags = CommandFlags.None)
        {
            return Converter.Value.Deserialize<T>(await Context.Database.SetRandomMemberAsync(Key, flags));
        }

        public IEnumerable<T> RandomMembers(long count, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SetRandomMembers(Key, count, flags).Select(Converter.Value.Deserialize<T>);
        }

        public async Task<IEnumerable<T>> RandomMembersAsync(long count, CommandFlags flags = CommandFlags.None)
        {
            return (await Context.Database.SetRandomMembersAsync(Key, count, flags)).Select(Converter.Value.Deserialize<T>);
        }

        #endregion

        #region Move

        public bool Move(RedisKey destination, T value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SetMove(Key, destination, Converter.Value.Serialize(value), flags);
        }

        public Task<bool> MoveAsync(RedisKey destination, T value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SetMoveAsync(Key, destination, Converter.Value.Serialize(value), flags);
        }

        #endregion

        #region Scan

        public IEnumerable<T> Scan(RedisValue pattern, int pageSize, CommandFlags flags)
        {
            return Context.Database.SetScan(Key, pattern, pageSize, flags).Select(Converter.Value.Deserialize<T>);
        }

        public IEnumerable<T> Scan(RedisValue pattern = default(RedisValue), int pageSize = 10, long cursor = 0, int pageOffset = 0, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.SetScan(Key, pattern, pageSize, cursor, pageOffset, flags).Select(Converter.Value.Deserialize<T>);
        }

        #endregion

        #region Pop

        public T Pop(CommandFlags flags = CommandFlags.None)
        {
            return Converter.Value.Deserialize<T>(Context.Database.SetPop(Key, flags));
        }

        public async Task<T> PopAsync(CommandFlags flags = CommandFlags.None)
        {
            return Converter.Value.Deserialize<T>(await Context.Database.SetPopAsync(Key, flags));
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
