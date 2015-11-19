using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Hyperspace.Redis.Internal
{
    public static class RedisContextActivator
    {
        [ThreadStatic]
        private static IServiceProvider _serviceProvider;

        public static IServiceProvider ServiceProvider
        {
            get { return _serviceProvider; }
            [param: CanBeNull] set { _serviceProvider = value; }
        }

        public static TContext CreateInstance<TContext>([NotNull] IServiceProvider serviceProvider) where TContext : RedisContext
        {
            try
            {
                _serviceProvider = serviceProvider;
                return (TContext)ActivatorUtilities.CreateInstance(serviceProvider, typeof(TContext));
            }
            finally
            {
                _serviceProvider = null;
            }
        }

        public static TContext CreateInstance<TContext>([NotNull] IServiceProvider serviceProvider, [NotNull] params object[] parameters) where TContext : RedisContext
        {
            try
            {
                _serviceProvider = serviceProvider;
                return (TContext)ActivatorUtilities.CreateInstance(serviceProvider, typeof(TContext), parameters);
            }
            finally
            {
                _serviceProvider = null;
            }
        }

    }
}
