using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;
using Hyperspace.Redis.Infrastructure;

namespace Hyperspace.Redis.Storage
{
    public class RedisConnection : IRedisConnection
    {
        public RedisConnection(ConnectionMultiplexer connection)
        {
            Connection = connection;
        }

        public ConnectionMultiplexer Connection { get; }

        public RedisDatabase GetDatabase(int index)
        {
            return new RedisDatabase(this, Connection.GetDatabase(index));
        }

        IRedisDatabase IRedisConnection.GetDatabase(int index)
        {
            return GetDatabase(index);
        }

    }
}
