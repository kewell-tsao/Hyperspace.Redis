using Hyperspace.Redis.Infrastructure;
using Hyperspace.Redis.Internal;
using Hyperspace.Redis.Metadata;
using Hyperspace.Redis.Metadata.Builders;
using Hyperspace.Redis.Properties;
using Hyperspace.Redis.Storage;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Hyperspace.Redis
{
    public class RedisContext : IDisposable, IInfrastructure<IServiceProvider>
    {
        private bool _disposed;
        private bool _initializing;
        private ILogger _logger;
        private IServiceScope _serviceScope;
        private LazyRef<RedisContextServices> _contextServices;

        private RedisModelMetadata _metadata;
        private RedisDatabase _database;
        private Dictionary<string, RedisEntry> _entryCache;
        private static readonly ConcurrentDictionary<Type, Type> OptionsTypes = new ConcurrentDictionary<Type, Type>();
        private static readonly ConcurrentDictionary<Type, RedisModelMetadata> Metadatas = new ConcurrentDictionary<Type, RedisModelMetadata>();

        protected RedisContext()
        {
            var serviceProvider = RedisContextActivator.ServiceProvider;
            Initialize(serviceProvider, GetOptions(serviceProvider));
        }

        public RedisContext([NotNull] RedisContextOptions options)
        {
            Check.NotNull(options, nameof(options));

            Initialize(RedisContextActivator.ServiceProvider, options);
        }

        public RedisContext([NotNull] IServiceProvider serviceProvider)
        {
            Check.NotNull(serviceProvider, nameof(serviceProvider));

            Initialize(serviceProvider, GetOptions(serviceProvider));
        }

        public RedisContext([NotNull] IServiceProvider serviceProvider, [NotNull] RedisContextOptions options)
        {
            Check.NotNull(serviceProvider, nameof(serviceProvider));
            Check.NotNull(options, nameof(options));

            Initialize(serviceProvider, options);
        }

        #region Internal Properties

        internal RedisDatabase Database
        {
            get { return _database; }
        }

        internal IServiceProvider ServiceProvider
        {
            get
            {
                if (_disposed)
                    throw new ObjectDisposedException(GetType().Name);
                return _contextServices.Value.ServiceProvider;
            }
        }

        internal RedisContextServices Services => _contextServices.Value;

        #endregion

        #region Initialize Methods

        private void Initialize(IServiceProvider serviceProvider, RedisContextOptions options)
        {
            _contextServices = new LazyRef<RedisContextServices>(() => InitializeServices(serviceProvider, options));

            var connectionProvider = serviceProvider.GetRequiredService<IRedisConnectionProvider>();
            _database = (RedisDatabase)connectionProvider.ConnectAndSelect(options);
            _entryCache = new Dictionary<string, RedisEntry>(_metadata.Children.Count);
            _metadata = GetMetadata();
        }

        private RedisContextServices InitializeServices(IServiceProvider serviceProvider, RedisContextOptions options)
        {
            if (_initializing)
                throw new InvalidOperationException(Strings.RecursiveOnConfiguring);

            try
            {
                _initializing = true;

                var optionsBuilder = new RedisContextOptionsBuilder(options);

                OnConfiguring(optionsBuilder);

                serviceProvider = serviceProvider ?? ServiceProviderCache.Instance.GetOrAdd(optionsBuilder.Options);

                _logger = serviceProvider.GetRequiredService<ILogger<RedisContext>>();

                _serviceScope = serviceProvider
                    .GetRequiredService<IServiceScopeFactory>()
                    .CreateScope();

                var scopedServiceProvider = _serviceScope.ServiceProvider;

                return scopedServiceProvider.GetRequiredService<RedisContextServices>()
                    .Initialize(scopedServiceProvider, optionsBuilder.Options, this);
            }
            finally
            {
                _initializing = false;
            }
        }

        private RedisModelMetadata GetMetadata()
        {
            var contextType = GetType();
            var metadata = Metadatas.GetOrAdd(contextType, t =>
            {
                var modelBuilder =
                    (ModelBuilder)Activator.CreateInstance(typeof(ModelBuilder<>).MakeGenericType(contextType));
                OnModelCreating(modelBuilder);
                return modelBuilder.Complete();
            });
            return metadata;
        }

        private RedisContextOptions GetOptions(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                return new RedisContextOptions<RedisContext>();

            var genericOptions = OptionsTypes.GetOrAdd(GetType(), t => typeof(RedisContextOptions<>).MakeGenericType(t));
            var options = (RedisContextOptions)serviceProvider.GetService(genericOptions) ??
                          serviceProvider.GetService<RedisContextOptions>();

            if (options != null && options.GetType() != genericOptions)
                throw new InvalidOperationException(Strings.NonGenericOptions);

            return options ?? new RedisContextOptions<RedisContext>();
        }

        #endregion

        #region Get Entry Methods

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
                var result = Services.Activator.CreateInstance<TEntry>(this, name);
                if (result == null)
                    throw new InvalidOperationException();
                _entryCache.Add(name, result);
                return result;
            }
        }

        protected RedisEntrySet<TEntry, TIdentifier> GetEntry<TEntry, TIdentifier>([CallerMemberName] string name = null)
            where TEntry : RedisEntry
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
                var result = Services.Activator.CreateInstance<RedisEntrySet<TEntry, TIdentifier>>(this, name);
                if (result == null)
                    throw new InvalidOperationException();
                _entryCache.Add(name, result);
                return result;
            }
        }

        #endregion

        #region Configuring Methods

        protected internal virtual void OnConfiguring(RedisContextOptionsBuilder optionsBuilder)
        {
        }

        protected internal virtual void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        #endregion

        #region Public Methods

        public RedisBatch<T> BeginBatch<T>() where T : RedisContext
        {
            return new RedisBatch<T>((T)this);
        }

        public RedisTransaction<T> BeginTransaction<T>() where T : RedisContext
        {
            return new RedisTransaction<T>((T)this);
        }

        #endregion

        #region IDisposable

        public virtual void Dispose()
        {
            _disposed = true;
            _serviceScope?.Dispose();
        }

        #endregion

        #region IInfrastructure

        IServiceProvider IInfrastructure<IServiceProvider>.Instance => ServiceProvider;

        #endregion

    }
}