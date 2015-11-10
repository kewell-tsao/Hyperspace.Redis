using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hyperspace.Redis.Infrastructure;
using Hyperspace.Redis.Internal;
using Hyperspace.Redis.Storage;
using JetBrains.Annotations;
using Microsoft.Framework.DependencyInjection.Extensions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Framework.DependencyInjection
{
    public static class RedisServiceCollectionExtensions
    {
        public static RedisServicesBuilder AddRedis([NotNull] this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAdd(new[]
            {
                ServiceDescriptor.Singleton<IRedisValueConverter, RedisValueConverter>(),
                ServiceDescriptor.Singleton<IRedisConnectionProvider, RedisConnectionProvider>(),
            });

            return new RedisServicesBuilder(serviceCollection);
        }
    }
}
