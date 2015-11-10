using Hyperspace.Redis.Infrastructure;
using Microsoft.Framework.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Hyperspace.Redis
{
    [RedisEntryType(RedisEntryType.String)]
    public class RedisString : RedisEntry
    {
        public RedisString(RedisContext context, RedisKey key) : base(context, key, RedisEntryType.String)
        {
        }

        #region Append

        public long Append(RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.StringAppend(Key, value, flags);
        }

        public Task<long> AppendAsync(RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.StringAppendAsync(Key, value, flags);
        }

        #endregion

        #region Get & Set

        public RedisValue Get(CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.StringGet(Key, flags);
        }

        public Task<RedisValue> GetAsync(CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.StringGetAsync(Key, flags);
        }

        public bool Set(RedisValue value, TimeSpan? expiry = default(TimeSpan?), When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.StringSet(Key, value, expiry, when, flags);
        }

        public Task<bool> SetAsync(RedisValue value, TimeSpan? expiry = default(TimeSpan?), When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.StringSetAsync(Key, value, expiry, when, flags);
        }

        public RedisValue GetSet(RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.StringGetSet(Key, value, flags);
        }

        public Task<RedisValue> GetSetAsync(RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.StringGetSetAsync(Key, value, flags);
        }

        #endregion

        #region Get & Set Bit

        public bool GetBit(long offset, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.StringGetBit(Key, offset, flags);
        }

        public Task<bool> GetBitAsync(long offset, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.StringGetBitAsync(Key, offset, flags);
        }

        public bool SetBit(long offset, bool bit, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.StringSetBit(Key, offset, bit, flags);
        }

        public Task<bool> SetBitAsync(long offset, bool bit, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.StringSetBitAsync(Key, offset, bit, flags);
        }

        #endregion

        #region Get & Set Range

        public RedisValue GetRange(long start, long end, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.StringGetRange(Key, start, end, flags);
        }

        public Task<RedisValue> GetRangeAsync(long start, long end, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.StringGetRangeAsync(Key, start, end, flags);
        }

        public RedisValue SetRange(long offset, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.StringSetRange(Key, offset, value, flags);
        }

        public Task<RedisValue> SetRangeAsync(long offset, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.StringSetRangeAsync(Key, offset, value, flags);
        }

        #endregion

        #region Length

        public long Length(CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.StringLength(Key, flags);
        }

        public Task<long> LengthAsync(CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.StringLengthAsync(Key, flags);
        }

        #endregion

        #region BitCount

        public long BitCount(long start = 0, long end = -1, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.StringBitCount(Key, start, end, flags);
        }

        public Task<long> BitCountAsync(long start = 0, long end = -1, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.StringBitCountAsync(Key, start, end, flags);
        }

        #endregion

        #region BitOperation

        public long BitOperation(Bitwise operation, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.StringBitOperation(operation, Key, keys, flags);
        }

        public Task<long> BitOperationAsync(Bitwise operation, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.StringBitOperationAsync(operation, Key, keys, flags);
        }

        public long BitOperation(Bitwise operation, RedisKey first, RedisKey second = default(RedisKey), CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.StringBitOperation(operation, Key, first, second, flags);
        }

        public Task<long> BitOperationAsync(Bitwise operation, RedisKey first, RedisKey second = default(RedisKey), CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.StringBitOperationAsync(operation, Key, first, second, flags);
        }

        public long BitPosition(bool bit, long start = 0, long end = -1, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.StringBitPosition(Key, bit, start, end, flags);
        }

        public Task<long> BitPositionAsync(bool bit, long start = 0, long end = -1, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.StringBitPositionAsync(Key, bit, start, end, flags);
        }

        #endregion

        #region Increment & Decrement

        public double Increment(double value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.StringIncrement(Key, value, flags);
        }

        public Task<double> IncrementAsync(double value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.StringIncrementAsync(Key, value, flags);
        }

        public long Increment(long value = 1, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.StringIncrement(Key, value, flags);
        }

        public Task<long> IncrementAsync(long value = 1, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.StringIncrementAsync(Key, value, flags);
        }

        public double Decrement(double value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.StringDecrement(Key, value, flags);
        }

        public Task<double> DecrementAsync(double value, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.StringDecrementAsync(Key, value, flags);
        }

        public long Decrement(long value = 1, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.StringDecrement(Key, value, flags);
        }

        public Task<long> DecrementAsync(long value = 1, CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.StringDecrementAsync(Key, value, flags);
        }

        #endregion

        #region GetWithExpiry

        public RedisValueWithExpiry GetWithExpiry(CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.StringGetWithExpiry(Key, flags);
        }

        public Task<RedisValueWithExpiry> GetWithExpiryAsync(CommandFlags flags = CommandFlags.None)
        {
            return Context.Database.StringGetWithExpiryAsync(Key, flags);
        }

        #endregion

    }

    public class RedisText : RedisString
    {
        public RedisText(RedisContext context, RedisKey key) : base(context, key)
        {
        }

    }

    public class RedisGuid : RedisString
    {
        public RedisGuid(RedisContext context, RedisKey key) : base(context, key)
        {
        }

        public Guid? Get()
        {
            var value = base.Get();
            if (value.IsNullOrEmpty)
                return null;

            Guid result;
            if (!TryDeserialize(base.Get(), out result))
                throw new InvalidOperationException();
            return result;
        }

        public async Task<Guid?> GetAsync()
        {
            var value = await base.GetAsync();
            if (value.IsNullOrEmpty)
                return null;

            Guid result;
            if (!TryDeserialize(base.Get(), out result))
                throw new InvalidOperationException();
            return result;
        }

        public bool Set(Guid? value)
        {
            return Set(value.HasValue ? Serialize(value.Value) : RedisValue.Null);
        }

        public Task<bool> SetAsync(Guid? value)
        {
            return SetAsync(value.HasValue ? Serialize(value.Value) : RedisValue.Null);
        }

        public bool IsBinarySerialization
        {
            get;
            set;
        }

        private RedisValue Serialize(Guid value)
        {
            if (IsBinarySerialization)
                return value.ToByteArray();
            return value.ToString();
        }

        private bool TryDeserialize(RedisValue value, out Guid result)
        {
            if (IsBinarySerialization)
            {
                byte[] binary = value;
                if (binary.Length != 16)
                {
                    result = default(Guid);
                    return false;
                }
                result = new Guid(binary);
                return true;
            }
            return Guid.TryParse(value, out result);
        }

    }

    public class RedisJson : RedisString
    {
        public RedisJson(RedisContext context, RedisKey key) : base(context, key)
        {
        }

        public JObject Get()
        {
            return JObject.Parse(base.Get());
        }

        public async Task<JObject> GetAsync()
        {
            return JObject.Parse(await base.GetAsync());
        }

        public bool Set(JObject value)
        {
            if (value == null)
                return Set(RedisValue.Null);
            using (var memoryStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(memoryStream))
            using (var jsonTextWriter = new JsonTextWriter(streamWriter))
            {
                value.WriteTo(jsonTextWriter);
                return Set(memoryStream.ToArray());
            }
        }

        public Task<bool> SetAsync(JObject value)
        {
            if (value == null)
                return SetAsync(RedisValue.Null);
            using (var memoryStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(memoryStream))
            using (var jsonTextWriter = new JsonTextWriter(streamWriter))
            {
                value.WriteTo(jsonTextWriter);
                return SetAsync(memoryStream.ToArray());
            }
        }

    }

    public class RedisNumber : RedisString
    {
        public RedisNumber(RedisContext context, RedisKey key) : base(context, key)
        {
        }

        public long Increment(uint amount = 1)
        {
            return Context.Database.StringIncrement(Key, amount);
        }

        public Task<long> IncrementAsync(uint amount = 1)
        {
            return Context.Database.StringIncrementAsync(Key, amount);
        }

        public long Decrement(uint amount = 1)
        {
            return Context.Database.StringDecrement(Key, amount);
        }

        public Task<long> DecrementAsync(uint amount = 1)
        {
            return Context.Database.StringDecrementAsync(Key, amount);
        }

        public long? Reset(uint count = 0)
        {
            var oldValue = Context.Database.StringGetSet(Key, count);
            return oldValue.IsInteger ? (long?)oldValue : null;
        }

        public Task<long?> ResetAsync(uint count = 0)
        {
            return Context.Database.StringGetSetAsync(Key, count).ContinueWith(t => t.Result.IsInteger ? (long?)t.Result : null);
        }

        public static long operator +(RedisNumber counter, uint amount)
        {
            return counter.Increment(amount);
        }

        public static long operator -(RedisNumber counter, uint amount)
        {
            return counter.Decrement(amount);
        }

    }

    public class RedisObject<T> : RedisString
    {
        public RedisObject(RedisContext context, RedisKey key) : base(context, key)
        {
            Converter = new Lazy<IRedisValueConverter>(() => Context.GetRequiredService<IRedisValueConverter>());
        }

        protected readonly Lazy<IRedisValueConverter> Converter;

        public T Get()
        {
            return Converter.Value.Deserialize<T>(base.Get());
        }

        public async Task<T> GetAsync()
        {
            return Converter.Value.Deserialize<T>(await base.GetAsync());
        }

        public bool Set(T value)
        {
            return Set(Converter.Value.Serialize(value));
        }

        public Task<bool> SetAsync(T value)
        {
            return SetAsync(Converter.Value.Serialize(value));
        }

    }

}
