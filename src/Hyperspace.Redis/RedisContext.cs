using Hyperspace.Redis.Infrastructure;
using Hyperspace.Redis.Internal;
using Hyperspace.Redis.Properties;
using Hyperspace.Redis.Storage;
using JetBrains.Annotations;
using Microsoft.Framework.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Hyperspace.Redis.Metadata;

namespace Hyperspace.Redis
{
    public class RedisEntryActivator
    {
        public TEntry CreateInstance<TEntry>(RedisContext context, string name) where TEntry : RedisEntry
        {
            return null;
        }

        public TEntry CreateInstance<TEntry>(RedisEntry parent, string name) where TEntry : RedisEntry
        {
            return null;
        }

        public TEntry CreateInstance<TEntry, TIdentifier>(RedisEntrySet<TEntry, TIdentifier> parent, TIdentifier identifier) where TEntry : RedisEntry
        {
            return null;
        }
    }

    public class RedisContext : IServiceProvider
    {
        private ModelMetadata _model;
        private RedisDatabase _database;
        private Dictionary<string, RedisEntry> _entryCache;
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
            _entryCache = new Dictionary<string, RedisEntry>(_model.Children.Count);
        }

        protected TEntry GetEntry<TEntry>([CallerMemberName] string name = null) where TEntry : RedisEntry
        {
            Check.NotEmpty(name, nameof(name));
            RedisEntry entry;
            if (_entryCache.TryGetValue(name, out entry))
            {
                var result = entry as TEntry;
                if (result == null)
                    throw new InvalidOperationException();
                return result;
            }
            else
            {
                var result = _model.Activator.CreateInstance<TEntry>(this, name);
                if (result == null)
                    throw new InvalidOperationException();
                _entryCache.Add(name, result);
                return result;
            }
        }

        protected RedisEntrySet<TEntry, TIdentifier> GetEntry<TEntry, TIdentifier>([CallerMemberName] string name = null) where TEntry : RedisEntry
        {
            Check.NotEmpty(name, nameof(name));

            RedisEntry entry;
            if (_entryCache.TryGetValue(name, out entry))
            {
                var result = entry as RedisEntrySet<TEntry, TIdentifier>;
                if (result == null)
                    throw new InvalidOperationException();
                return result;
            }
            else
            {
                var result = _model.Activator.CreateInstance<RedisEntrySet<TEntry, TIdentifier>>(this, name);
                if (result == null)
                    throw new InvalidOperationException();
                _entryCache.Add(name, result);
                return result;
            }
        }

        #region IServiceProvider

        object IServiceProvider.GetService(Type serviceType)
        {
            return serviceType;
        }

        #endregion

    }
}
