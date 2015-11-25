using Hyperspace.Redis.Infrastructure;
using Hyperspace.Redis.Metadata;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperspace.Redis.Internal
{
    public class RedisContextServices
    {
        public RedisContextServices Initialize([NotNull] IServiceProvider scopedServiceProvider, [NotNull] IRedisContextOptions contextOptions, [NotNull] RedisContext context)
        {
            ServiceProvider = scopedServiceProvider;
            ContextOptions = contextOptions;

            return this;
        }

        public RedisContext Context { get; private set; }
        public IRedisContextOptions ContextOptions { get; private set; }
        public ModelMetadata Model { get; private set; }
        public IServiceProvider ServiceProvider { get; private set; }

    }
}
