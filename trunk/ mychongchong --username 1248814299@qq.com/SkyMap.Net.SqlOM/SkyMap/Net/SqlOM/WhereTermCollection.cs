namespace SkyMap.Net.SqlOM
{
    using System;
    using System.Collections;
    using System.Reflection;

    [Serializable]
    public class WhereTermCollection : CollectionBase
    {
        public WhereTermCollection()
        {
        }

        public WhereTermCollection(WhereTermCollection val)
        {
            this.AddRange(val);
        }

        public WhereTermCollection(WhereTerm[] val)
        {
            this.AddRange(val);
        }

        public int Add(WhereTerm val)
        {
            return base.List.Add(val);
        }

        public void AddRange(WhereTerm[] val)
        {
            for (int i = 0; i < val.Length; i++)
            {
                this.Add(val[i]);
            }
        }

        public void AddRange(WhereTermCollection val)
        {
            for (int i = 0; i < val.Count; i++)
            {
                this.Add(val[i]);
            }
        }

        public bool Contains(WhereTerm val)
        {
            return base.List.Contains(val);
        }

        public void CopyTo(WhereTerm[] array, int index)
        {
            base.List.CopyTo(array, index);
        }

        public WhereClauseEnumerator GetEnumerator()
        {
            return new WhereClauseEnumerator(this);
        }

        public int IndexOf(WhereTerm val)
        {
            return base.List.IndexOf(val);
        }

        public void Insert(int index, WhereTerm val)
        {
            base.List.Insert(index, val);
        }

        public void Remove(WhereTerm val)
        {
            base.List.Remove(val);
        }

        public WhereTerm this[int index]
        {
            get
            {
                return (WhereTerm) base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }

        public class WhereClauseEnumerator : IEnumerator
        {
            private IEnumerator baseEnumerator;
            private IEnumerable temp;

            public WhereClauseEnumerator(WhereTermCollection mappings)
            {
                this.temp = mappings;
                this.baseEnumerator = this.temp.GetEnumerator();
            }

            public bool MoveNext()
            {
                return this.baseEnumerator.MoveNext();
            }

            public void Reset()
            {
                this.baseEnumerator.Reset();
            }

            public WhereTerm Current
            {
                get
                {
                    return (WhereTerm) this.baseEnumerator.Current;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return this.baseEnumerator.Current;
                }
            }
        }
    }
}

