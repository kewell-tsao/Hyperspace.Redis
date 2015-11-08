using JetBrains.Annotations;

namespace Hyperspace.Redis.Infrastructure
{
    public interface IRedisContextOptionsBuilder
    {
        void AddOrUpdateExtension<TExtension>([NotNull] TExtension extension) where TExtension : class, IRedisContextOptionsExtension;
    }
}
