using Hyperspace.Redis.Infrastructure;
using Hyperspace.Redis.Internal;
using Hyperspace.Redis.Properties;
using Hyperspace.Redis.Storage;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Hyperspace.Redis.Metadata;
using Hyperspace.Redis.Metadata.Builders;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Hyperspace.Redis
{
    public class RedisEntryActivator
    {
        private readonly ModelMetadata _metadata;

        public RedisEntryActivator(ModelMetadata metadata)
        {
            _metadata = metadata;
        }

        public TEntry CreateInstance<TEntry>(RedisContext context, string name) where TEntry : RedisEntry
        {
            var entryMetadata = _metadata.Children.SingleOrDefault(m => m.Name == name && m.ClrType == typeof(TEntry));

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

        private Func<RedisKey, RedisEntryMetadata, RedisContext, RedisEntry, TEntry> CreateConstructor<TEntry>() where TEntry : RedisEntry
        {
            var p0 = Expression.Parameter(typeof(RedisKey), "key");
            var p1 = Expression.Parameter(typeof(RedisEntryMetadata), "metadata");
            var p2 = Expression.Parameter(typeof(RedisContext), "context");
            var p3 = Expression.Parameter(typeof(RedisEntry), "parent");
            var entryType = typeof(TEntry);
            var entryConstructorInfo = entryType.GetConstructor(new[] { p0.Type, p1.Type, p2.Type, p3.Type });
            if (entryConstructorInfo == null)
                throw new InvalidOperationException();
            var entryConstructor = Expression.Lambda<Func<RedisKey, RedisEntryMetadata, RedisContext, RedisEntry, TEntry>>(
                Expression.New(entryConstructorInfo, p0, p1, p2, p3), p0, p1, p2, p3).Compile();
            return entryConstructor;
        }

    }

    public class RedisContext : IDisposable, IInfrastructure<IServiceProvider>
    {
        private bool _disposed;
        private bool _initializing;
        private ILogger _logger;
        private IServiceScope _serviceScope;
        //private LazyRef<IDbContextServices> _contextServices;

        private ModelMetadata _metadata;
        private RedisDatabase _database;
        private Dictionary<string, RedisEntry> _entryCache;
        private static readonly ConcurrentDictionary<Type, Type> OptionsTypes = new ConcurrentDictionary<Type, Type>();
        private static readonly ConcurrentDictionary<Type, ModelMetadata> Metadatas = new ConcurrentDictionary<Type, ModelMetadata>();

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

        internal RedisDatabase Database
        {
            get { return _database; }
        }

        private IServiceProvider ServiceProvider
        {
            get
            {
                if (_disposed)
                    throw new ObjectDisposedException(GetType().Name);
                return null;
                //return _contextServices.Value.ServiceProvider;
            }
        }

        internal T GetRequiredService<T>()
        {
            return ServiceProvider.GetRequiredService<T>();
        }

        private ModelMetadata GetMetadata()
        {
            var contextType = GetType();
            var metadata = Metadatas.GetOrAdd(contextType, t =>
            {
                var modelBuilder = (ModelBuilder)Activator.CreateInstance(typeof(ModelBuilder<>).MakeGenericType(contextType));
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
            var options = (RedisContextOptions)serviceProvider.GetService(genericOptions) ?? serviceProvider.GetService<RedisContextOptions>();

            if (options != null && options.GetType() != genericOptions)
                throw new InvalidOperationException(Strings.NonGenericOptions);

            return options ?? new RedisContextOptions<RedisContext>();
        }

        private void Initialize(IServiceProvider serviceProvider, RedisContextOptions options)
        {
            var connectionProvider = serviceProvider.GetRequiredService<IRedisConnectionProvider>();
            _database = (RedisDatabase)connectionProvider.ConnectAndSelect(options);
            _entryCache = new Dictionary<string, RedisEntry>(_metadata.Children.Count);
            _metadata = GetMetadata();
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
                var result = _metadata.Activator.CreateInstance<TEntry>(this, name);
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
                var result = _metadata.Activator.CreateInstance<RedisEntrySet<TEntry, TIdentifier>>(this, name);
                if (result == null)
                    throw new InvalidOperationException();
                _entryCache.Add(name, result);
                return result;
            }
        }

        protected internal virtual void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        public RedisBatch<T> BeginBatch<T>() where T : RedisContext
        {
            return new RedisBatch<T>((T)this);
        }

        public RedisTransaction<T> BeginTransaction<T>() where T : RedisContext
        {
            return new RedisTransaction<T>((T)this);
        }

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