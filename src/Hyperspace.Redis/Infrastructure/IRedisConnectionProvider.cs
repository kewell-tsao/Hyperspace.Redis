using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperspace.Redis.Infrastructure
{
    public interface IRedisConnectionProvider
    {
        IRedisConnection Connect(IRedisContextOptions options);

        IRedisDatabase ConnectAndSelect(IRedisContextOptions options);
    }
}
