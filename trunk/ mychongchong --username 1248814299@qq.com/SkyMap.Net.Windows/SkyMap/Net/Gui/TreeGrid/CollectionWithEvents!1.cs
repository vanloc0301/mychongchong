namespace SkyMap.Net.Gui.TreeGrid
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public sealed class CollectionWithEvents<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
    {
        private List<T> list;

        public event EventHandler<CollectionItemEventArgs<T>> Added;

        public event EventHandler<CollectionItemEventArgs<T>> Removed;

        public CollectionWithEvents()
        {
            this.list = new List<T>();
        }

        public void Add(T item)
        {
            this.list.Add(item);
            this.OnAdded(item);
        }

        public void AddRange(IEnumerable<T> range)
        {
            foreach (T local in range)
            {
                this.Add(local);
            }
        }

        public void Clear()
        {
            List<T> list = this.list;
            this.list = new List<T>();
            foreach (T local in list)
            {
                this.OnRemoved(local);
            }
        }

        public bool Contains(T item)
        {
            return this.list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.list.CopyTo(array, arrayIndex);
        }

        [DebuggerStepThrough]
        public IEnumerator<T> GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return this.list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            this.list.Insert(index, item);
            this.OnAdded(item);
        }

        private void OnAdded(T item)
        {
            if (this.Added != null)
            {
                this.Added(this, new CollectionItemEventArgs<T>(item));
            }
        }

        private void OnRemoved(T item)
        {
            if (this.Removed != null)
            {
                this.Removed(this, new CollectionItemEventArgs<T>(item));
            }
        }

        public bool Remove(T item)
        {
            if (this.list.Remove(item))
            {
                this.OnRemoved(item);
                return true;
            }
            return false;
        }

        public void RemoveAt(int index)
        {
            T item = this.list[index];
            this.list.RemoveAt(index);
            this.OnRemoved(item);
        }

        [DebuggerStepThrough]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        public int Count
        {
            [DebuggerStepThrough]
            get
            {
                return this.list.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public T this[int index]
        {
            get
            {
                return this.list[index];
            }
            set
            {
                T objA = this.list[index];
                if (!object.Equals(objA, value))
                {
                    this.list[index] = value;
                    this.OnRemoved(objA);
                    this.OnAdded(value);
                }
            }
        }
    }
}

