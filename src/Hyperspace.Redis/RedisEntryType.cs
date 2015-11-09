using System;

namespace Hyperspace.Redis
{
    public enum RedisEntryType
    {
        String,
        List,
        Hash,
        Set,
        SortedSet
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class RedisEntryTypeAttribute : Attribute
    {
        public RedisEntryTypeAttribute(RedisEntryType type)
        {
            Type = type;
        }

        public RedisEntryType Type { get; }
    }

}
