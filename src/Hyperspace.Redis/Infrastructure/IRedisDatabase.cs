using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hyperspace.Redis.Infrastructure
{
    public interface IRedisDatabase
    {
        IRedisConnection Connection { get; }
    }
}
