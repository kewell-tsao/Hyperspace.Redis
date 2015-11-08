using JetBrains.Annotations;

namespace Hyperspace.Redis.Infrastructure
{
    public interface IRedisContextOptionsExtension
    {
        void ApplyServices([NotNull] RedisServicesBuilder builder);
    }
}
