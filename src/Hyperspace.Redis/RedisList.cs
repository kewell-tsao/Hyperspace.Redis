using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperspace.Redis
{
    [RedisEntryType(RedisEntryType.List)]
    public class RedisList : RedisEntry
    {
        public RedisList(RedisContext context, RedisKey key) : base(context, key, RedisEntryType.List)
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

    }

    public class RedisList<T> : RedisList
    {
        static RedisList()
        {
            var type = typeof(T);
            if (type == typeof(RedisKey) ||
                type == typeof(RedisValue) ||
                type == typeof(bool) ||
                type == typeof(bool?) ||
                type == typeof(int) ||
                type == typeof(int?) ||
                type == typeof(long) ||
                type == typeof(long?) ||
                type == typeof(byte[]) ||
                type == typeof(string) ||
                type == typeof(double) ||
                type == typeof(double?))
                throw new InvalidOperationException();
        }

        public RedisList(RedisContext context, RedisKey key) : base(context, key)
        {
        }

        private static RedisValue SerializeObject(T value)
        {
            return JsonConvert.SerializeObject(value);
        }

        private static T DeserializeObject(RedisValue value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        #region Range

        public new IEnumerable<T> Range(long start = 0, long stop = -1, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListRange(Key, start, stop, flags).Select(DeserializeObject);
        }

        public new async Task<IEnumerable<T>> RangeAsync(long start = 0, long stop = -1, CommandFlags flags = CommandFlags.None)
        {
            return (await Context.Database.ListRangeAsync(Key, start, stop, flags)).Select(DeserializeObject);
        }

        #endregion

        #region Insert

        public long InsertAfter(T pivot, T value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListInsertAfter(Key, SerializeObject(pivot), SerializeObject(value), flags);
        }

        public Task<long> InsertAfterAsync(T pivot, T value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListInsertAfterAsync(Key, SerializeObject(pivot), SerializeObject(value), flags);
        }

        public long InsertBefore(T pivot, T value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListInsertBefore(Key, SerializeObject(pivot), SerializeObject(value), flags);
        }

        public Task<long> InsertBeforeAsync(T pivot, T value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListInsertBeforeAsync(Key, SerializeObject(pivot), SerializeObject(value), flags);
        }

        #endregion

        #region Remove

        public long Remove(T value, long count = 0, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListRemove(Key, SerializeObject(value), count, flags);
        }

        public Task<long> RemoveAsync(T value, long count = 0, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListRemoveAsync(Key, SerializeObject(value), count, flags);
        }

        #endregion

        #region Get & Set By Index

        public new T GetByIndex(long index, CommandFlags flags = CommandFlags.None)
        {
            return DeserializeObject(Context.Database.ListGetByIndex(Key, index, flags));
        }

        public new async Task<T> GetByIndexAsync(long index, CommandFlags flags = CommandFlags.None)
        {
            return DeserializeObject(await Context.Database.ListGetByIndexAsync(Key, index, flags));
        }

        public void SetByIndex(long index, T value, CommandFlags flags = CommandFlags.None)
        {
            Context.Database.ListSetByIndex(Key, index, SerializeObject(value), flags);
        }

        public Task SetByIndexAsync(long index, T value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListSetByIndexAsync(Key, index, SerializeObject(value), flags);
        }

        #endregion

        #region Left Push & Pop

        public long LeftPush(IEnumerable<T> values, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListLeftPush(Key, values.Select(SerializeObject).ToArray(), flags);
        }

        public Task<long> LeftPushAsync(IEnumerable<T> values, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListLeftPushAsync(Key, values.Select(SerializeObject).ToArray(), flags);
        }

        public long LeftPush(T value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListLeftPush(Key, SerializeObject(value), when, flags);
        }

        public Task<long> LeftPushAsync(T value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListLeftPushAsync(Key, SerializeObject(value), when, flags);
        }

        public new T LeftPop(CommandFlags flags = CommandFlags.None)
        {
            return DeserializeObject(Context.Database.ListLeftPop(Key, flags));
        }

        public new async Task<T> LeftPopAsync(CommandFlags flags = CommandFlags.None)
        {
            return DeserializeObject(await Context.Database.ListLeftPopAsync(Key, flags));
        }

        #endregion

        #region Right Push & Pop

        public long RightPush(IEnumerable<T> values, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListRightPush(Key, values.Select(SerializeObject).ToArray(), flags);
        }

        public Task<long> RightPushAsync(IEnumerable<T> values, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListRightPushAsync(Key, values.Select(SerializeObject).ToArray(), flags);
        }

        public long RightPush(T value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListRightPush(Key, SerializeObject(value), when, flags);
        }

        public Task<long> RightPushAsync(T value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.ListRightPushAsync(Key, SerializeObject(value), when, flags);
        }

        public new T RightPop(CommandFlags flags = CommandFlags.None)
        {
            return DeserializeObject(Context.Database.ListRightPop(Key, flags));
        }

        public new async Task<T> RightPopAsync(CommandFlags flags = CommandFlags.None)
        {
            return DeserializeObject(await Context.Database.ListRightPopAsync(Key, flags));
        }

        #endregion

        #region Right Pop Left Push

        public new T RightPopLeftPush(RedisKey destination, CommandFlags flags = CommandFlags.None)
        {
            return DeserializeObject(Context.Database.ListRightPopLeftPush(Key, destination, flags));
        }

        public new async Task<T> RightPopLeftPushAsync(RedisKey destination, CommandFlags flags = CommandFlags.None)
        {
            return DeserializeObject(await Context.Database.ListRightPopLeftPushAsync(Key, destination, flags));
        }

        #endregion

    }
}
