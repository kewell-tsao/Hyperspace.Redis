using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using System.Reflection;
using Hyperspace.Redis.Internal;

namespace Hyperspace.Redis.Metadata
{
    public class MetadataElement
    {
        public bool IsFrozen { get; private set; }
    }

    public class ModelMetadata : MetadataElement
    {
        public string Name { get; set; }
        public string Prefix { get; set; }
        public Type ClrType { get; set; }

        public ICollection<RedisEntryMetadata> Children { get; } = new List<RedisEntryMetadata>();

        internal RedisEntryActivator Activator { get; set; }
    }

    public class RedisEntryMetadata : MetadataElement
    {
        public RedisEntryMetadata() : this(false)
        {
        }

        public RedisEntryMetadata(bool isEntrySet)
        {
            IsEntrySet = isEntrySet;
            if (IsEntrySet)
                Children = new LimitedCollection<RedisEntryMetadata>(1);
            else
                Children = new List<RedisEntryMetadata>();
        }

        public bool IsEntrySet { get; }

        public ModelMetadata Model { get; set; }

        public string Name { get; set; }
        public string ShortName { get; set; }
        public RedisEntryType EntryType { get; set; }
        public Type ClrType { get; set; }
        public Type IdentifierClrType { get; set; }
        public Type IdentifierConverterType { get; set; }

        public RedisEntryMetadata Parent { get; set; }
        public ICollection<RedisEntryMetadata> Children { get; }

        internal RedisEntryActivator Activator { get; set; }
    }

}
