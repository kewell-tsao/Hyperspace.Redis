using System;
using System.Collections;
using System.Collections.Generic;

namespace Hyperspace.Redis.Internal
{
    public class LimitedCollection<T> : ICollection<T>
    {
        private readonly int _limit;
        private readonly List<T> _list;

        public LimitedCollection(int limit)
        {
            _list = new List<T>(_limit = limit);
        }

        public int Count => _list.Count;

        public bool IsReadOnly => _list.Count >= _limit;

        public void Add(T item)
        {
            if (IsReadOnly)
                throw new InvalidOperationException();
            _list.Add(item);
        }

        public bool Remove(T item)
        {
            return _list.Remove(item);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(T item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

    }
}
