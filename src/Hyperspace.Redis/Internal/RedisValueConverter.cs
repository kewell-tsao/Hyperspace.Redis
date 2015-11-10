using Hyperspace.Redis.Infrastructure;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;

namespace Hyperspace.Redis.Internal
{
    public class RedisValueConverter : IRedisValueConverter
    {
        public RedisValue Serialize<T>(T value)
        {
            if (value == null)
                return RedisValue.Null;
            return JsonConvert.SerializeObject(value);
        }

        public RedisValue Serialize(object value, Type type)
        {
            Check.NotNull(type, nameof(type));

            if (value == null)
                return RedisValue.Null;
            return JsonConvert.SerializeObject(value, type, JsonConvert.DefaultSettings());
        }

        public bool TrySerialize<T>(T value, out RedisValue result)
        {
            if (value == null)
            {
                result = RedisValue.Null;
                return true;
            }
            try
            {
                result = JsonConvert.SerializeObject(value);
                return true;
            }
            catch (Exception)
            {
                result = RedisValue.Null;
                return false;
            }
        }

        public bool TrySerialize(object value, Type type, out RedisValue result)
        {
            if (value == null)
            {
                result = RedisValue.Null;
                return true;
            }
            try
            {
                result = JsonConvert.SerializeObject(value, type, JsonConvert.DefaultSettings());
                return true;
            }
            catch (Exception)
            {
                result = RedisValue.Null;
                return false;
            }
        }

        public T Deserialize<T>(RedisValue value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public object Deserialize(RedisValue value, Type type)
        {
            Check.NotNull(type, nameof(type));

            return JsonConvert.DeserializeObject(value, type);
        }

        public bool TryDeserialize<T>(RedisValue value, out T result)
        {
            try
            {
                result = JsonConvert.DeserializeObject<T>(value);
                return true;
            }
            catch
            {
                result = default(T);
                return false;
            }
        }

        public bool TryDeserialize(RedisValue value, Type type, out object result)
        {
            Check.NotNull(type, nameof(type));

            try
            {
                result = JsonConvert.DeserializeObject(value, type);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

    }
}
