using System;
using System.Collections.Generic;
using System.Linq;
using Hyperspace.Redis.Properties;
using JetBrains.Annotations;

namespace Hyperspace.Redis.Infrastructure
{
    public abstract class RedisContextOptions : IRedisContextOptions
    {
        private readonly IReadOnlyDictionary<Type, IRedisContextOptionsExtension> _extensions;

        protected RedisContextOptions([NotNull] IReadOnlyDictionary<Type, IRedisContextOptionsExtension> extensions)
        {
            Check.NotNull(extensions, nameof(extensions));

            _extensions = extensions;
        }

        public virtual IEnumerable<IRedisContextOptionsExtension> Extensions => _extensions.Values;

        public virtual TExtension GetExtension<TExtension>() where TExtension : class, IRedisContextOptionsExtension
        {
            var extension = FindExtension<TExtension>();
            if (extension == null)
                throw new InvalidOperationException(string.Format(Strings.OptionsExtensionNotFound, typeof(TExtension).Name));
            return extension;
        }

        public virtual TExtension FindExtension<TExtension>() where TExtension : class, IRedisContextOptionsExtension
        {
            IRedisContextOptionsExtension extension;
            return _extensions.TryGetValue(typeof(TExtension), out extension) ? (TExtension)extension : null;
        }

        public abstract RedisContextOptions WithExtension<TExtension>([NotNull] TExtension extension) where TExtension : class, IRedisContextOptionsExtension;

    }

    public class RedisContextOptions<TContext> : RedisContextOptions
        where TContext : RedisContext
    {
        public RedisContextOptions()
            : base(new Dictionary<Type, IRedisContextOptionsExtension>())
        {
        }

        public RedisContextOptions([NotNull] IReadOnlyDictionary<Type, IRedisContextOptionsExtension> extensions)
            : base(extensions)
        {
        }

        public override RedisContextOptions WithExtension<TExtension>(TExtension extension)
        {
            Check.NotNull(extension, nameof(extension));

            var extensions = Extensions.ToDictionary(p => p.GetType(), p => p);
            extensions[typeof(TExtension)] = extension;

            return new RedisContextOptions<TContext>(extensions);
        }

    }

}
