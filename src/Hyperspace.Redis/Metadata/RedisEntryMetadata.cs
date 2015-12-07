using Hyperspace.Redis.Internal;
using Hyperspace.Redis.Metadata.Internal;
using System;
using System.Diagnostics;
using System.Text;

namespace Hyperspace.Redis.Metadata
{
    public class RedisEntryMetadata : MetadataElement
    {
        private RedisModelMetadata _model;
        private string _name;
        private string _alias;
        private RedisEntryType _entryType;
        private Type _clrType;
        private Type _identifierClrType;
        private Type _identifierConverterType;
        private RedisEntryMetadata _parent;

        public RedisEntryMetadata() : this(false)
        {
        }

        public RedisEntryMetadata(bool isEntrySet)
        {
            IsEntrySet = isEntrySet;
            Children = IsEntrySet ? new MetadataElementCollection<RedisEntryMetadata>(1) : new MetadataElementCollection<RedisEntryMetadata>();
        }

        public bool IsEntrySet { get; }

        public RedisModelMetadata Model
        {
            get { return _model; }
            set { SetValue(ref _model, value); }
        }

        public string Name
        {
            get { return _name; }
            set { SetValue(ref _name, value); }
        }

        public string Alias
        {
            get { return _alias; }
            set { SetValue(ref _alias, value); }
        }

        public RedisEntryType EntryType
        {
            get { return _entryType; }
            set { SetValue(ref _entryType, value); }
        }

        public Type ClrType
        {
            get { return _clrType; }
            set { SetValue(ref _clrType, value); }
        }

        public Type IdentifierClrType
        {
            get { return _identifierClrType; }
            set { SetValue(ref _identifierClrType, value); }
        }

        public Type IdentifierConverterType
        {
            get { return _identifierConverterType; }
            set { SetValue(ref _identifierConverterType, value); }
        }

        public RedisEntryMetadata Parent
        {
            get { return _parent; }
            set { SetValue(ref _parent, value); }
        }

        public MetadataElementCollection<RedisEntryMetadata> Children { get; }

        private string _identifierCache;

        public string GetIdentifier()
        {
            if (string.IsNullOrEmpty(_identifierCache))
            {
                const char separator = ':';
                var builder = new StringBuilder();
                var metadata = this;
                while (metadata != null)
                {
                    if (metadata.Parent?.IsEntrySet ?? false)
                    {
                        builder.Insert(0, "[*]");
                    }
                    else
                    {
                        builder.Insert(0, metadata.Name);
                    }
                    builder.Insert(0, separator);
                    metadata = metadata.Parent;
                }
                if (string.IsNullOrEmpty(Model?.Prefix))
                {
                    Debug.Assert(builder[0] == separator);
                    builder.Remove(0, 1);
                }
                else
                {
                    builder.Insert(0, Model.Prefix);
                }
                return builder.ToString();
            }
            return _identifierCache;
        }

        protected override void FreezeCore()
        {
            Children.Freeze();

            _identifierCache = GetIdentifier();
        }

    }
}
