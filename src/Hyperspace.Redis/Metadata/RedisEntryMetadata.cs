using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using System.Reflection;
using Hyperspace.Redis.Internal;

namespace Hyperspace.Redis.Metadata
{
    public class ModelMetadata
    {
        public string Name { get; set; }
        public string Prefix { get; set; }
        public Type ClrType { get; set; }

        public ICollection<EntryMetadata> Children { get; } = new List<EntryMetadata>();
    }

    public class EntryMetadata
    {
        public EntryMetadata() : this(false)
        {
        }

        public EntryMetadata(bool isEntrySet)
        {
            IsEntrySet = isEntrySet;
            if (IsEntrySet)
                Children = new LimitedCollection<EntryMetadata>(1);
            else
                Children = new List<EntryMetadata>();
        }

        public bool IsEntrySet { get; }

        public ModelMetadata Model { get; set; }

        public string Name { get; set; }
        public string ShortName { get; set; }
        public RedisEntryType EntryType { get; set; }
        public Type ClrType { get; set; }
        public Type IdentifierClrType { get; set; }
        public Type IdentifierConverterType { get; set; }

        public EntryMetadata Parent { get; set; }
        public ICollection<EntryMetadata> Children { get; }
    }

}
