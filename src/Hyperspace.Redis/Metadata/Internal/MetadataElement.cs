using System;

namespace Hyperspace.Redis.Metadata.Internal
{
    public abstract class MetadataElement
    {
        public bool IsFrozen
        {
            get;
            private set;
        }

        public void Freeze()
        {
            if (IsFrozen)
                throw new InvalidOperationException("");
            FreezeCore();
            IsFrozen = true;
        }

        protected abstract void FreezeCore();

        protected void VerifyChange()
        {
            if (IsFrozen)
                throw new InvalidOperationException("");
        }

        protected bool SetValue<T>(ref T field, T value)
        {
            VerifyChange();
            if (ReferenceEquals(field, value))
                return false;
            field = value;
            return true;
        }

    }
}
