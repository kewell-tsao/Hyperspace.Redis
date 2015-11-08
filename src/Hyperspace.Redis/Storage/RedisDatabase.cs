using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;
using Hyperspace.Redis.Infrastructure;
using JetBrains.Annotations;

namespace Hyperspace.Redis.Storage
{
    public class RedisDatabase: IRedisDatabase
    {
        public RedisDatabase([NotNull] RedisConnection connection, [NotNull] IDatabase database)
        {
            Check.NotNull(connection, nameof(connection));
            Check.NotNull(database, nameof(database));

            Database = database;
            Connection = connection;
        }

        public IDatabase Database { get; }
        public RedisConnection Connection { get; }

        IRedisConnection IRedisDatabase.Connection
        {
            get { return Connection; }
        }

    }
}
