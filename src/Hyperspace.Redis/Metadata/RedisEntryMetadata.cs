using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using System.Reflection;

namespace Hyperspace.Redis.Metadata
{
    public class RedisModel
    {
        public ICollection<RedisEntryMetadata> Children { get; set; }
    }

    public class RedisEntryMetadata
    {
        private readonly List<RedisEntryMetadata> _children;

        public RedisEntryMetadata([NotNull] string name, [NotNull] Type clrType, RedisEntryType entryType, [NotNull] RedisModel model)
        {
            Check.NotEmpty(name, nameof(name));
            Check.NotNull(model, nameof(model));
            Check.NotNull(clrType, nameof(clrType));
            if (clrType.GetTypeInfo().IsClass)
                throw new ArgumentException(nameof(clrType));

            Name = name;
            ClrType = clrType;
            Model = model;
            EntryType = entryType;

            Parent = null;
            _children = new List<RedisEntryMetadata>();
        }

        public RedisEntryMetadata([NotNull] string name, [NotNull] Type clrType, RedisEntryType entryType, [NotNull] RedisEntryMetadata parent)
            : this(name, clrType, entryType, Check.NotNull(parent, nameof(parent)).Model)
        {
            Parent = parent;
            Parent._children.Add(this);
        }

        public string Name { get; }
        public Type ClrType { get; }
        public RedisModel Model { get; }
        public RedisEntryType EntryType { get; }

        public RedisEntryMetadata Parent { get; }

        public IReadOnlyCollection<RedisEntryMetadata> Children
        {
            get { return _children; }
        }

    }
}
