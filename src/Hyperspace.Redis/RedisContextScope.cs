using JetBrains.Annotations;
using StackExchange.Redis;
using System;

namespace Hyperspace.Redis
{
    public abstract class RedisContextScope : IDisposable
    {
        [ThreadStatic]
        private static RedisContextScope _current;

        protected internal RedisContextScope([NotNull] RedisContext context)
        {
            Check.NotNull(context, nameof(context));

            Context = context;
        }

        protected internal RedisContext Context { get; }

        internal static RedisContextScope Current
        {
            get { return _current; }
        }

        internal abstract IDatabaseAsync AsyncFunc { get; }

        protected class ScopeDaemon : IDisposable
        {
            private readonly RedisContextScope _scope;

            public ScopeDaemon([NotNull] RedisContextScope scope)
            {
                Check.NotNull(scope, nameof(scope));
                if (_current != null)
                    throw new InvalidOperationException("");
                _scope = scope;
                _current = scope;
            }

            public void Dispose()
            {
                if (_current == null)
                    throw new InvalidOperationException("");
                if (_current != _scope)
                    throw new InvalidOperationException("");
                _current = null;
            }

        }

        #region IDisposable

        private bool _disposed;

        ~RedisContextScope()
        {
            Dispose(false);
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
            {

            }
            _disposed = true;
        }

        #endregion

    }
}
