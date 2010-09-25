namespace SkyMap.Net.SqlOM
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class GroupByTermCollection : CollectionBase
    {
        public GroupByTermCollection()
        {
        }

        public GroupByTermCollection(GroupByTerm[] items)
        {
            this.AddRange(items);
        }

        public GroupByTermCollection(GroupByTermCollection items)
        {
            this.AddRange(items);
        }

        public virtual void Add(GroupByTerm value)
        {
            base.List.Add(value);
        }

        public virtual void AddRange(GroupByTermCollection items)
        {
            foreach (GroupByTerm term in items)
            {
                base.List.Add(term);
            }
        }

        public virtual void AddRange(GroupByTerm[] items)
        {
            foreach (GroupByTerm term in items)
            {
                base.List.Add(term);
            }
        }

        public virtual bool Contains(GroupByTerm value)
        {
            return base.List.Contains(value);
        }

        public virtual Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        public virtual int IndexOf(GroupByTerm value)
        {
            return base.List.IndexOf(value);
        }

        public virtual void Insert(int index, GroupByTerm value)
        {
            base.List.Insert(index, value);
        }

        public virtual void Remove(GroupByTerm value)
        {
            base.List.Remove(value);
        }

        public virtual GroupByTerm this[int index]
        {
            get
            {
                return (GroupByTerm) base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }

        public class Enumerator : IEnumerator
        {
            private IEnumerator wrapped;

            public Enumerator(GroupByTermCollection collection)
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

            public GroupByTerm Current
            {
                get
                {
                    return (GroupByTerm) this.wrapped.Current;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return (GroupByTerm) this.wrapped.Current;
                }
            }
        }
    }
}

