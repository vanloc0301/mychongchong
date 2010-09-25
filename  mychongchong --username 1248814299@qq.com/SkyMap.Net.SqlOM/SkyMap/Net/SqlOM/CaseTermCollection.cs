namespace SkyMap.Net.SqlOM
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class CaseTermCollection : CollectionBase
    {
        public CaseTermCollection()
        {
        }

        public CaseTermCollection(CaseTerm[] items)
        {
            this.AddRange(items);
        }

        public CaseTermCollection(CaseTermCollection items)
        {
            this.AddRange(items);
        }

        public virtual void Add(CaseTerm value)
        {
            base.List.Add(value);
        }

        public virtual void AddRange(CaseTermCollection items)
        {
            foreach (CaseTerm term in items)
            {
                base.List.Add(term);
            }
        }

        public virtual void AddRange(CaseTerm[] items)
        {
            foreach (CaseTerm term in items)
            {
                base.List.Add(term);
            }
        }

        public virtual bool Contains(CaseTerm value)
        {
            return base.List.Contains(value);
        }

        public virtual Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        public virtual int IndexOf(CaseTerm value)
        {
            return base.List.IndexOf(value);
        }

        public virtual void Insert(int index, CaseTerm value)
        {
            base.List.Insert(index, value);
        }

        public virtual void Remove(CaseTerm value)
        {
            base.List.Remove(value);
        }

        public virtual CaseTerm this[int index]
        {
            get
            {
                return (CaseTerm) base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }

        public class Enumerator : IEnumerator
        {
            private IEnumerator wrapped;

            public Enumerator(CaseTermCollection collection)
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

            public CaseTerm Current
            {
                get
                {
                    return (CaseTerm) this.wrapped.Current;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return (CaseTerm) this.wrapped.Current;
                }
            }
        }
    }
}

