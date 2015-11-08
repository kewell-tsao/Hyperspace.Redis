using System;
using System.Collections.Generic;
using Hyperspace.Redis;
using Hyperspace.Redis.Internal;
using JetBrains.Annotations;
using Microsoft.Framework.DependencyInjection;

namespace Hyperspace.Redis.Infrastructure
{
    public class RedisServicesBuilder
    {
        private readonly IServiceCollection _serviceCollection;

        public RedisServicesBuilder([NotNull] IServiceCollection serviceCollection)
        {
            Check.NotNull(serviceCollection, nameof(serviceCollection));

            _serviceCollection = serviceCollection;
        }

        public RedisServicesBuilder AddRedisContext<TContext>([CanBeNull] Action<RedisContextOptionsBuilder> optionsAction = null) where TContext : RedisContext
        {
            _serviceCollection.AddSingleton(p => RedisOptionsFactory<TContext>(optionsAction));
            _serviceCollection.AddSingleton<RedisContextOptions>(p => p.GetRequiredService<RedisContextOptions<TContext>>());
            _serviceCollection.AddScoped(typeof(TContext), RedisContextActivator.CreateInstance<TContext>);
            return this;
        }

        private static RedisContextOptions<TContext> RedisOptionsFactory<TContext>([CanBeNull] Action<RedisContextOptionsBuilder> optionsAction)
            where TContext : RedisContext
        {
            var options = new RedisContextOptions<TContext>(new Dictionary<Type, IRedisContextOptionsExtension>());
            if (optionsAction != null)
            {
                var builder = new RedisContextOptionsBuilder<TContext>(options);
                optionsAction(builder);
                options = builder.Options;
            }
            return options;
        }

    }
}
