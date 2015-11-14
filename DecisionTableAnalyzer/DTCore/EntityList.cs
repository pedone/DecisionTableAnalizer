using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace DTCore
{

    public interface IEntityList : IList, IEnumerable
    {
    }

    public class EntityList<T> : IList<T>, ICollection<T>, IEntityList, IEnumerable<T>
        where T : Entity
    {

        private List<T> _InternalList;

        public EntityList()
        {
            _InternalList = new List<T>();
        }

        public EntityList<T> Copy()
        {
            return new EntityList<T>
            {
                _InternalList = _InternalList.ToList()
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _InternalList.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _InternalList.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return _InternalList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            _InternalList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _InternalList.RemoveAt(index);
        }

        public T this[int index]
        {
            get
            {
                return _InternalList[index];
            }
            set { _InternalList[index] = value; }
        }

        public void Add(T item)
        {
            _InternalList.Add(item);
        }

        public void Clear()
        {
            _InternalList.Clear();
        }

        public bool Contains(T item)
        {
            return _InternalList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _InternalList.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get
            {
                return _InternalList.Count;
            }
        }

        public bool Remove(T item)
        {
            return _InternalList.Remove(item);
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public int Add(object value)
        {
            Add((T)value);
            return IndexOf((T)value);
        }

        public bool Contains(object value)
        {
            return Contains((T)value);
        }

        public int IndexOf(object value)
        {
            return IndexOf((T)value);
        }

        public void Insert(int index, object value)
        {
            Insert(index, (T)value);
        }

        public bool IsFixedSize
        {
            get { return false; }
        }

        public void Remove(object value)
        {
            Remove((T)value);
        }

        object IList.this[int index]
        {
            get { return this[index]; }
            set { this[index] = (T)value; }
        }

        public void CopyTo(Array array, int index)
        {
            CopyTo(array.Cast<T>().ToArray(), index);
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get { return this; }
        }
    }
}
