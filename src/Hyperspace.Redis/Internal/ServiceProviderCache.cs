using Hyperspace.Redis.Infrastructure;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Hyperspace.Redis.Internal
{
    public class ServiceProviderCache
    {
        private readonly ConcurrentDictionary<long, IServiceProvider> _configurations = new ConcurrentDictionary<long, IServiceProvider>();

        public static ServiceProviderCache Instance { get; } = new ServiceProviderCache();

        public virtual IServiceProvider GetOrAdd([NotNull] IRedisContextOptions options)
        {
            unchecked
            {
                var key = options.Extensions.Aggregate(0, (t, e) => (t * 397) ^ e.GetType().GetHashCode());

                return _configurations.GetOrAdd(
                    key,
                    k =>
                    {
                        var services = new ServiceCollection();
                        var builder = services.AddRedis();

                        foreach (var extension in options.Extensions)
                            extension.ApplyServices(builder);

                        return services.BuildServiceProvider();
                    });
            }
        }
    }
}
