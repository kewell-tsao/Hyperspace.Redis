using System;
using JetBrains.Annotations;
using StackExchange.Redis;

namespace Hyperspace.Redis.Infrastructure
{
    public class RedisConnectionOptionsExtension : IRedisContextOptionsExtension
    {
        private string _configurationString;
        private ConfigurationOptions _configuration;

        public RedisConnectionOptionsExtension()
        {
        }

        public RedisConnectionOptionsExtension(string configurationString)
        {
            _configurationString = configurationString;
        }

        public RedisConnectionOptionsExtension([NotNull] RedisConnectionOptionsExtension copyFrom)
        {
            Check.NotNull(copyFrom, nameof(copyFrom));

            _configurationString = copyFrom._configurationString;
            _configuration = copyFrom._configuration;
        }

        public virtual string ConfigurationString
        {
            get { return _configurationString; }
            [param: NotNull]
            set
            {
                Check.NotEmpty(value, nameof(value));
                _configurationString = value;
            }
        }

        public virtual ConfigurationOptions Configuration
        {
            get { return _configuration; }
            [param: NotNull]
            set
            {
                Check.NotNull(value, nameof(value));
                _configuration = value;
            }
        }

        public virtual void ApplyServices(RedisServicesBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));
        }

    }
}
