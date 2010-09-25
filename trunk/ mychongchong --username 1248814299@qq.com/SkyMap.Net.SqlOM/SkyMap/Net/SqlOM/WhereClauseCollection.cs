namespace SkyMap.Net.SqlOM
{
    using System;
    using System.Collections;
    using System.Reflection;

    [Serializable]
    public class WhereClauseCollection : CollectionBase
    {
        public WhereClauseCollection()
        {
        }

        public WhereClauseCollection(WhereClauseCollection val)
        {
            this.AddRange(val);
        }

        public WhereClauseCollection(WhereClause[] val)
        {
            this.AddRange(val);
        }

        public int Add(WhereClause val)
        {
            return base.List.Add(val);
        }

        public void AddRange(WhereClause[] val)
        {
            for (int i = 0; i < val.Length; i++)
            {
                this.Add(val[i]);
            }
        }

        public void AddRange(WhereClauseCollection val)
        {
            for (int i = 0; i < val.Count; i++)
            {
                this.Add(val[i]);
            }
        }

        public bool Contains(WhereClause val)
        {
            return base.List.Contains(val);
        }

        public void CopyTo(WhereClause[] array, int index)
        {
            base.List.CopyTo(array, index);
        }

        public WhereClauseGroupEnumerator GetEnumerator()
        {
            return new WhereClauseGroupEnumerator(this);
        }

        public int IndexOf(WhereClause val)
        {
            return base.List.IndexOf(val);
        }

        public void Insert(int index, WhereClause val)
        {
            base.List.Insert(index, val);
        }

        public void Remove(WhereClause val)
        {
            base.List.Remove(val);
        }

        public WhereClause this[int index]
        {
            get
            {
                return (WhereClause) base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }

        public class WhereClauseGroupEnumerator : IEnumerator
        {
            private IEnumerator baseEnumerator;
            private IEnumerable temp;

            public WhereClauseGroupEnumerator(WhereClauseCollection mappings)
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

            public WhereClause Current
            {
                get
                {
                    return (WhereClause) this.baseEnumerator.Current;
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

