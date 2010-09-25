namespace SkyMap.Net.SqlOM
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class SelectColumnCollection : CollectionBase
    {
        public SelectColumnCollection()
        {
        }

        public SelectColumnCollection(SelectColumn[] items)
        {
            this.AddRange(items);
        }

        public SelectColumnCollection(SelectColumnCollection items)
        {
            this.AddRange(items);
        }

        public virtual void Add(SelectColumn value)
        {
            base.List.Add(value);
        }

        public virtual void AddRange(SelectColumnCollection items)
        {
            foreach (SelectColumn column in items)
            {
                base.List.Add(column);
            }
        }

        public virtual void AddRange(SelectColumn[] items)
        {
            foreach (SelectColumn column in items)
            {
                base.List.Add(column);
            }
        }

        public virtual bool Contains(SelectColumn value)
        {
            return base.List.Contains(value);
        }

        public virtual Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        public virtual int IndexOf(SelectColumn value)
        {
            return base.List.IndexOf(value);
        }

        public virtual void Insert(int index, SelectColumn value)
        {
            base.List.Insert(index, value);
        }

        public virtual void Remove(SelectColumn value)
        {
            base.List.Remove(value);
        }

        public virtual SelectColumn this[int index]
        {
            get
            {
                return (SelectColumn) base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }

        public class Enumerator : IEnumerator
        {
            private IEnumerator wrapped;

            public Enumerator(SelectColumnCollection collection)
            {
                this.wrapped = collection.GetEnumerator();
            }

            public bool MoveNext()
            {
                return this.wrapped.MoveNext();
            }

            public void Reset()
            {
                this.wrapped.Reset();
            }

            public SelectColumn Current
            {
                get
                {
                    return (SelectColumn) this.wrapped.Current;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return (SelectColumn) this.wrapped.Current;
                }
            }
        }
    }
}

