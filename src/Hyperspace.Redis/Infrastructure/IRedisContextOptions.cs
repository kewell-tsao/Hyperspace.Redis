using System.Collections.Generic;

namespace Hyperspace.Redis.Infrastructure
{
    public interface IRedisContextOptions
    {
        IEnumerable<IRedisContextOptionsExtension> Extensions { get; }

        TExtension FindExtension<TExtension>() where TExtension : class, IRedisContextOptionsExtension;
    }
}