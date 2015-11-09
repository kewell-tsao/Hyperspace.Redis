using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using System.Reflection;

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
        public ModelMetadata Model { get; set; }

        public string Name { get; set; }
        public string ShortName { get; set; }
        public Type ClrType { get; set; }
        public RedisEntryType EntryType { get; set; }

        public bool IsEntrySetItem => Parent is EntrySetMetadata;

        public EntryMetadata Parent { get; set; }
        public ICollection<EntryMetadata> Children { get; } = new List<EntryMetadata>();

    }

    public class EntrySetMetadata : EntryMetadata
    {

    }

}
