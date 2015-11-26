using Hyperspace.Redis.Internal;
using Hyperspace.Redis.Metadata.Internal;
using System;

namespace Hyperspace.Redis.Metadata
{
    public class RedisModelMetadata : MetadataElement
    {
        private string _name;
        private string _prefix;
        private Type _clrType;

        public RedisModelMetadata()
        {
            Children = new MetadataElementCollection<RedisEntryMetadata>();
        }

        public string Name
        {
            get { return _name; }
            set { SetValue(ref _name, value); }
        }

        public string Prefix
        {
            get { return _prefix; }
            set { SetValue(ref _prefix, value); }
        }

        public Type ClrType
        {
            get { return _clrType; }
            set { SetValue(ref _clrType, value); }
        }

        public MetadataElementCollection<RedisEntryMetadata> Children { get; }

        protected override void FreezeCore()
        {
            Children.Freeze();
        }

    }
}
