using Hyperspace.Redis.Infrastructure;
using Hyperspace.Redis.Internal;
using Hyperspace.Redis.Properties;
using Hyperspace.Redis.Storage;
using JetBrains.Annotations;
using Microsoft.Framework.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace Hyperspace.Redis
{
    public class RedisContext : IServiceProvider
    {
        private RedisDatabase _database;
        private static readonly ConcurrentDictionary<Type, Type> OptionsTypes = new ConcurrentDictionary<Type, Type>();

        protected RedisContext()
        {
            var serviceProvider = RedisContextActivator.ServiceProvider;
            Initialize(serviceProvider, GetOptions(serviceProvider));
        }

        public RedisContext([NotNull] IServiceProvider serviceProvider)
        {
            Check.NotNull(serviceProvider, nameof(serviceProvider));

            Initialize(serviceProvider, GetOptions(serviceProvider));
        }

        public RedisContext([NotNull] RedisContextOptions options)
        {
            Check.NotNull(options, nameof(options));

            Initialize(RedisContextActivator.ServiceProvider, options);
        }

        public RedisContext([NotNull] IServiceProvider serviceProvider, [NotNull] RedisContextOptions options)
        {
            Check.NotNull(serviceProvider, nameof(serviceProvider));
            Check.NotNull(options, nameof(options));

            Initialize(serviceProvider, options);
        }

        public IDatabase Database => _database.Database;

        private RedisContextOptions GetOptions(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                return new RedisContextOptions<RedisContext>();

            var genericOptions = OptionsTypes.GetOrAdd(GetType(), t => typeof(RedisContextOptions<>).MakeGenericType(t));
            var options = (RedisContextOptions)serviceProvider.GetService(genericOptions) ?? serviceProvider.GetService<RedisContextOptions>();

            if (options != null && options.GetType() != genericOptions)
                throw new InvalidOperationException(Strings.NonGenericOptions);

            return options ?? new RedisContextOptions<RedisContext>();
        }

        private void Initialize(IServiceProvider serviceProvider, RedisContextOptions options)
        {
            var connectionProvider = serviceProvider.GetRequiredService<IRedisConnectionProvider>();
            _database = (RedisDatabase)connectionProvider.ConnectAndSelect(options);
        }

        internal TEntry GetSubEntry<TEntry>(RedisEntry parent, string name) where TEntry : RedisEntry
        {
            Check.NotNull(parent, nameof(parent));
            Check.NotEmpty(name, nameof(name));

            throw new NotImplementedException();
        }

        internal TEntry GetSubEntry<TEntry, TIdentifier>(RedisEntry parent, TIdentifier identifier) where TEntry : RedisEntry
        {
            Check.NotNull(parent, nameof(parent));
            Check.NotNull(identifier, nameof(identifier));

            throw new NotImplementedException();
        }

        protected TEntry GetSubEntry<TEntry>([CallerMemberName] string name = null) where TEntry : RedisEntry
        {
            Check.NotEmpty(name, nameof(name));

            throw new NotImplementedException();
        }

        protected RedisEntrySet<TEntry> GetSubEntrySet<TEntry>([CallerMemberName] string name = null) where TEntry : RedisEntry
        {
            Check.NotEmpty(name, nameof(name));

            throw new NotImplementedException();
        }

        protected RedisEntrySet<TEntry, TIdentifier> GetSubEntrySet<TEntry, TIdentifier>([CallerMemberName] string name = null) where TEntry : RedisEntry
        {
            Check.NotEmpty(name, nameof(name));

            throw new NotImplementedException();
        }

        #region IServiceProvider

        object IServiceProvider.GetService(Type serviceType)
        {
            return serviceType;
        }

        #endregion

    }
}
