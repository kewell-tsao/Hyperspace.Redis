using System;
using System.Linq;
using StackExchange.Redis;
using Hyperspace.Redis.Infrastructure;
using JetBrains.Annotations;

namespace Hyperspace.Redis
{
    public class RedisContextOptionsBuilder: IRedisContextOptionsBuilder
    {
        public RedisContextOptionsBuilder() : this(new RedisContextOptions<RedisContext>())
        {
        }

        public RedisContextOptionsBuilder([NotNull] RedisContextOptions options)
        {
            Check.NotNull(options, nameof(options));
            Options = options;
        }

        public virtual RedisContextOptions Options { get; private set; }

        public virtual bool IsConfigured => Options.Extensions.Any();

        public virtual RedisContextOptionsBuilder UseConnection([NotNull]string configurationString)
        {
            Check.NotEmpty(configurationString, nameof(configurationString));

            var extension = GetOrCreateRedisConnectionOptionsExtension(this);
            extension.ConfigurationString = configurationString;
            ((IRedisContextOptionsBuilder)this).AddOrUpdateExtension(extension);

            return this;
        }

        public virtual RedisContextOptionsBuilder UseConnection([NotNull]ConfigurationOptions configuration)
        {
            Check.NotNull(configuration, nameof(configuration));

            var extension = GetOrCreateRedisConnectionOptionsExtension(this);
            extension.Configuration = configuration;
            ((IRedisContextOptionsBuilder)this).AddOrUpdateExtension(extension);

            return this;
        }

        public virtual RedisContextOptionsBuilder UseDatabase(int index)
        {
            var extension = GetOrCreateRedisDatabaseOptionsExtension(this);
            extension.DatabaseIndex = index;
            ((IRedisContextOptionsBuilder)this).AddOrUpdateExtension(extension);
            return this;
        }

        void IRedisContextOptionsBuilder.AddOrUpdateExtension<TExtension>(TExtension extension)
        {
            Check.NotNull(extension, nameof(extension));

            Options = Options.WithExtension(extension);
        }


        private static RedisDatabaseOptionsExtension GetOrCreateRedisDatabaseOptionsExtension(RedisContextOptionsBuilder optionsBuilder)
        {
            var existing = optionsBuilder.Options.FindExtension<RedisDatabaseOptionsExtension>();
            return existing != null
                ? new RedisDatabaseOptionsExtension(existing)
                : new RedisDatabaseOptionsExtension();
        }

        private static RedisConnectionOptionsExtension GetOrCreateRedisConnectionOptionsExtension(RedisContextOptionsBuilder optionsBuilder)
        {
            var existing = optionsBuilder.Options.FindExtension<RedisConnectionOptionsExtension>();
            return existing != null
                ? new RedisConnectionOptionsExtension(existing)
                : new RedisConnectionOptionsExtension();
        }

    }

    public class RedisContextOptionsBuilder<TContext> : RedisContextOptionsBuilder where TContext : RedisContext
    {
        public RedisContextOptionsBuilder() : this(new RedisContextOptions<TContext>())
		{
        }

        public RedisContextOptionsBuilder([NotNull] RedisContextOptions<TContext> options) : base(options)
		{
        }

        public new virtual RedisContextOptions<TContext> Options
        {
            get { return (RedisContextOptions<TContext>)base.Options; }
        }

        public new virtual RedisContextOptionsBuilder<TContext> UseConnection([NotNull]string configurationString)
        {
            return (RedisContextOptionsBuilder<TContext>)base.UseConnection(configurationString);
        }

        public new virtual RedisContextOptionsBuilder<TContext> UseConnection([NotNull]ConfigurationOptions configuration)
        {
            return (RedisContextOptionsBuilder<TContext>)base.UseConnection(configuration);
        }

        public new virtual RedisContextOptionsBuilder<TContext> UseDatabase(int index)
        {
            return (RedisContextOptionsBuilder<TContext>)base.UseDatabase(index);
        }
    }

}
