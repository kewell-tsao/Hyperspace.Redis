using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;
using Hyperspace.Redis.Infrastructure;

namespace Hyperspace.Redis.Storage
{
    public class RedisConnectionProvider : IRedisConnectionProvider
    {
        public RedisConnection Connect(IRedisContextOptions options)
        {
            Check.NotNull(options, nameof(options));

            var connectionOptions = options.FindExtension<RedisConnectionOptionsExtension>();

            var configuration = connectionOptions.Configuration;
            if (configuration == null && !string.IsNullOrEmpty(connectionOptions.ConfigurationString))
                configuration = ConfigurationOptions.Parse(connectionOptions.ConfigurationString);
            if (configuration == null)
                throw new InvalidOperationException();
            var connectionMultiplexer = ConnectionMultiplexer.Connect(configuration);
            return new RedisConnection(connectionMultiplexer);
        }

        public RedisDatabase ConnectAndSelect(IRedisContextOptions options)
        {
            Check.NotNull(options, nameof(options));

            var connect = Connect(options);
            var databaseOptions = options.FindExtension<RedisDatabaseOptionsExtension>();
            return connect.GetDatabase(databaseOptions.DatabaseIndex);
        }

        IRedisConnection IRedisConnectionProvider.Connect(IRedisContextOptions options)
        {
            return Connect(options);
        }

        IRedisDatabase IRedisConnectionProvider.ConnectAndSelect(IRedisContextOptions options)
        {
            return ConnectAndSelect(options);
        }

    }
}
