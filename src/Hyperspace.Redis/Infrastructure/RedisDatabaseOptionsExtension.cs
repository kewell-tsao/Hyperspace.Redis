using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Hyperspace.Redis.Infrastructure
{
    public class RedisDatabaseOptionsExtension : IRedisContextOptionsExtension
    {
        private int _databaseIndex;

        public RedisDatabaseOptionsExtension()
        {
        }

        public RedisDatabaseOptionsExtension([NotNull] RedisDatabaseOptionsExtension copyFrom)
        {
            Check.NotNull(copyFrom, nameof(copyFrom));

            _databaseIndex = copyFrom._databaseIndex;
        }

        public void ApplyServices(RedisServicesBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));
        }

        public virtual int DatabaseIndex
        {
            get { return _databaseIndex; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value));
                _databaseIndex = value;
            }
        }

    }
}
