using System;
using System.Collections;
using System.Collections.Generic;

namespace Hyperspace.Redis.Metadata.Internal
{
    public class MetadataElementCollection<T> : MetadataElement, ICollection<T>, IReadOnlyCollection<T> where T : MetadataElement
    {
        private readonly int? _limit;
        private readonly List<T> _list;

        public MetadataElementCollection()
        {
            _list = new List<T>();
        }

        public MetadataElementCollection(int limit)
        {
            _limit = limit;
            _list = new List<T>(limit);
        }

        public bool IsFull
        {
            get { return _limit != null && _list.Count >= _limit; }
        }

        protected void VerifyFull()
        {
            if (IsFull)
                throw new InvalidOperationException("");
        }

        #region ICollection

        public int Count
        {
            get { return _list.Count; }
        }

        public bool IsReadOnly
        {
            get { return IsFrozen || IsFull; }
        }

        public void Add(T item)
        {
            VerifyChange();
            VerifyFull();
            _list.Add(item);
        }

        public void Clear()
        {
            VerifyChange();
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

        public bool Remove(T item)
        {
            VerifyChange();
            return _list.Remove(item);
        }

        #endregion

        #region IEnumerable

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion

        protected override void FreezeCore()
        {
            foreach (var item in _list)
            {
                item.Freeze();
            }
        }
    }
}
