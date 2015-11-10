using System;
using System.Collections.Generic;

namespace Hyperspace.Redis
{
    public struct RedisSortedSetEntry<T> : IEquatable<RedisSortedSetEntry<T>>, IComparable, IComparable<RedisSortedSetEntry<T>>
    {
        public RedisSortedSetEntry(T element, double score)
        {
            Element = element;
            Score = score;
        }

        public T Element { get; }

        public double Score { get; }

        public int CompareTo(object other)
        {
            if (!(other is RedisSortedSetEntry<T>))
                return -1;
            return CompareTo((RedisSortedSetEntry<T>)other);
        }

        public int CompareTo(RedisSortedSetEntry<T> other)
        {
            return Score.CompareTo(other.Score);
        }

        public override bool Equals(object other)
        {
            return other is RedisSortedSetEntry<T> && Equals((RedisSortedSetEntry<T>)other);
        }

        public bool Equals(RedisSortedSetEntry<T> other)
        {
            return Score.Equals(other.Score) && EqualityComparer<T>.Default.Equals(Element, other.Element);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Element?.GetHashCode() ?? 0) ^ Score.GetHashCode();
            }
        }

        public static bool operator ==(RedisSortedSetEntry<T> x, RedisSortedSetEntry<T> y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(RedisSortedSetEntry<T> x, RedisSortedSetEntry<T> y)
        {
            return !x.Equals(y);
        }

        public static implicit operator KeyValuePair<T, double>(RedisSortedSetEntry<T> value)
        {
            return new KeyValuePair<T, double>(value.Element, value.Score);
        }

        public static implicit operator RedisSortedSetEntry<T>(KeyValuePair<T, double> value)
        {
            return new RedisSortedSetEntry<T>(value.Key, value.Value);
        }

    }
}
