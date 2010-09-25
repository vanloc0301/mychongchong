namespace SkyMap.Net.SqlOM
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class OrderByTermCollection : CollectionBase
    {
        public OrderByTermCollection()
        {
        }

        public OrderByTermCollection(OrderByTerm[] items)
        {
            this.AddRange(items);
        }

        public OrderByTermCollection(OrderByTermCollection items)
        {
            this.AddRange(items);
        }

        public virtual void Add(OrderByTerm value)
        {
            base.List.Add(value);
        }

        public virtual void AddRange(OrderByTermCollection items)
        {
            foreach (OrderByTerm term in items)
            {
                base.List.Add(term);
            }
        }

        public virtual void AddRange(OrderByTerm[] items)
        {
            foreach (OrderByTerm term in items)
            {
                base.List.Add(term);
            }
        }

        public virtual bool Contains(OrderByTerm value)
        {
            return base.List.Contains(value);
        }

        public virtual Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        public virtual int IndexOf(OrderByTerm value)
        {
            return base.List.IndexOf(value);
        }

        public virtual void Insert(int index, OrderByTerm value)
        {
            base.List.Insert(index, value);
        }

        public virtual void Remove(OrderByTerm value)
        {
            base.List.Remove(value);
        }

        public virtual OrderByTerm this[int index]
        {
            get
            {
                return (OrderByTerm) base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }

        public class Enumerator : IEnumerator
        {
            private IEnumerator wrapped;

            public Enumerator(OrderByTermCollection collection)
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

            public OrderByTerm Current
            {
                get
                {
                    return (OrderByTerm) this.wrapped.Current;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return (OrderByTerm) this.wrapped.Current;
                }
            }
        }
    }
}

