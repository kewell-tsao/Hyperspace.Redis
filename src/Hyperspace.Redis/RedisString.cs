using Hyperspace.Redis.Infrastructure;
using Hyperspace.Redis.Metadata;
using Microsoft.Extensions.DependencyInjection;
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
        public RedisString(RedisKey key, RedisEntryMetadata metadata, RedisContext context, RedisEntry parent) : base(key, metadata, context, parent)
        {
        }

        #region Append

        public long Append(RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.StringAppend(Key, value, flags);
        }

        public Task<long> AppendAsync(RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.StringAppendAsync(Key, value, flags);
        }

        #endregion

        #region Get & Set

        public RedisValue Get(CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.StringGet(Key, flags);
        }

        public Task<RedisValue> GetAsync(CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.StringGetAsync(Key, flags);
        }

        public bool Set(RedisValue value, TimeSpan? expiry = default(TimeSpan?), When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.StringSet(Key, value, expiry, when, flags);
        }

        public Task<bool> SetAsync(RedisValue value, TimeSpan? expiry = default(TimeSpan?), When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.StringSetAsync(Key, value, expiry, when, flags);
        }

        public RedisValue GetSet(RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.StringGetSet(Key, value, flags);
        }

        public Task<RedisValue> GetSetAsync(RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.StringGetSetAsync(Key, value, flags);
        }

        #endregion

        #region Get & Set Bit

        public bool GetBit(long offset, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.StringGetBit(Key, offset, flags);
        }

        public Task<bool> GetBitAsync(long offset, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.StringGetBitAsync(Key, offset, flags);
        }

        public bool SetBit(long offset, bool bit, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.StringSetBit(Key, offset, bit, flags);
        }

        public Task<bool> SetBitAsync(long offset, bool bit, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.StringSetBitAsync(Key, offset, bit, flags);
        }

        #endregion

        #region Get & Set Range

        public RedisValue GetRange(long start, long end, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.StringGetRange(Key, start, end, flags);
        }

        public Task<RedisValue> GetRangeAsync(long start, long end, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.StringGetRangeAsync(Key, start, end, flags);
        }

        public RedisValue SetRange(long offset, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.StringSetRange(Key, offset, value, flags);
        }

        public Task<RedisValue> SetRangeAsync(long offset, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.StringSetRangeAsync(Key, offset, value, flags);
        }

        #endregion

        #region Length

        public long Length(CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.StringLength(Key, flags);
        }

        public Task<long> LengthAsync(CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.StringLengthAsync(Key, flags);
        }

        #endregion

        #region BitCount

        public long BitCount(long start = 0, long end = -1, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.StringBitCount(Key, start, end, flags);
        }

        public Task<long> BitCountAsync(long start = 0, long end = -1, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.StringBitCountAsync(Key, start, end, flags);
        }

        #endregion

        #region BitOperation

        public long BitOperation(Bitwise operation, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.StringBitOperation(operation, Key, keys, flags);
        }

        public Task<long> BitOperationAsync(Bitwise operation, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.StringBitOperationAsync(operation, Key, keys, flags);
        }

        public long BitOperation(Bitwise operation, RedisKey first, RedisKey second = default(RedisKey), CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.StringBitOperation(operation, Key, first, second, flags);
        }

        public Task<long> BitOperationAsync(Bitwise operation, RedisKey first, RedisKey second = default(RedisKey), CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.StringBitOperationAsync(operation, Key, first, second, flags);
        }

        public long BitPosition(bool bit, long start = 0, long end = -1, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.StringBitPosition(Key, bit, start, end, flags);
        }

        public Task<long> BitPositionAsync(bool bit, long start = 0, long end = -1, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.StringBitPositionAsync(Key, bit, start, end, flags);
        }

        #endregion

        #region Increment & Decrement

        public double Increment(double value, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.StringIncrement(Key, value, flags);
        }

        public Task<double> IncrementAsync(double value, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.StringIncrementAsync(Key, value, flags);
        }

        public long Increment(long value = 1, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.StringIncrement(Key, value, flags);
        }

        public Task<long> IncrementAsync(long value = 1, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.StringIncrementAsync(Key, value, flags);
        }

        public double Decrement(double value, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.StringDecrement(Key, value, flags);
        }

        public Task<double> DecrementAsync(double value, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.StringDecrementAsync(Key, value, flags);
        }

        public long Decrement(long value = 1, CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.StringDecrement(Key, value, flags);
        }

        public Task<long> DecrementAsync(long value = 1, CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.StringDecrementAsync(Key, value, flags);
        }

        #endregion

        #region GetWithExpiry

        public RedisValueWithExpiry GetWithExpiry(CommandFlags flags = CommandFlags.None)
        {
            return RedisSync.StringGetWithExpiry(Key, flags);
        }

        public Task<RedisValueWithExpiry> GetWithExpiryAsync(CommandFlags flags = CommandFlags.None)
        {
            return RedisAsync.StringGetWithExpiryAsync(Key, flags);
        }

        #endregion

    }

    public class RedisText : RedisString
    {
        public RedisText(RedisKey key, RedisEntryMetadata metadata, RedisContext context, RedisEntry parent) : base(key, metadata, context, parent)
        {
        }

    }

    public class RedisGuid : RedisString
    {
        public RedisGuid(RedisKey key, RedisEntryMetadata metadata, RedisContext context, RedisEntry parent) : base(key, metadata, context, parent)
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
        public RedisJson(RedisKey key, RedisEntryMetadata metadata, RedisContext context, RedisEntry parent) : base(key, metadata, context, parent)
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
        public RedisNumber(RedisKey key, RedisEntryMetadata metadata, RedisContext context, RedisEntry parent) : base(key, metadata, context, parent)
        {
        }

        public long Increment(uint amount = 1)
        {
            return RedisSync.StringIncrement(Key, amount);
        }

        public Task<long> IncrementAsync(uint amount = 1)
        {
            return RedisAsync.StringIncrementAsync(Key, amount);
        }

        public long Decrement(uint amount = 1)
        {
            return RedisSync.StringDecrement(Key, amount);
        }

        public Task<long> DecrementAsync(uint amount = 1)
        {
            return RedisAsync.StringDecrementAsync(Key, amount);
        }

        public long? Reset(uint count = 0)
        {
            var oldValue = RedisSync.StringGetSet(Key, count);
            return oldValue.IsInteger ? (long?)oldValue : null;
        }

        public Task<long?> ResetAsync(uint count = 0)
        {
            return RedisAsync.StringGetSetAsync(Key, count).ContinueWith(t => t.Result.IsInteger ? (long?)t.Result : null);
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
        public RedisObject(RedisKey key, RedisEntryMetadata metadata, RedisContext context, RedisEntry parent) : base(key, metadata, context, parent)
        {
            Converter = new Lazy<IRedisValueConverter>(() => Context.ServiceProvider.GetRequiredService<IRedisValueConverter>());
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
