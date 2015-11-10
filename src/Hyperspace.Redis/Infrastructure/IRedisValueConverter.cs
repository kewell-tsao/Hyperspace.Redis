using JetBrains.Annotations;
using StackExchange.Redis;
using System;

namespace Hyperspace.Redis.Infrastructure
{
    public interface IRedisValueConverter
    {
        RedisValue Serialize<T>(T value);
        RedisValue Serialize(object value, [NotNull] Type type);
        bool TrySerialize<T>(T value, out RedisValue result);
        bool TrySerialize(object value, [NotNull] Type type, out RedisValue result);

        T Deserialize<T>(RedisValue value);
        object Deserialize(RedisValue value, [NotNull] Type type);
        bool TryDeserialize<T>(RedisValue value, out T result);
        bool TryDeserialize(RedisValue value, [NotNull] Type type, out object result);
    }
}
