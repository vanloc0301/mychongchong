namespace SkyMap.Net.SqlOM
{
    using System;
    using System.Collections;
    using System.Reflection;

    [Serializable]
    internal class JoinCollection : CollectionBase
    {
        internal JoinCollection()
        {
        }

        internal JoinCollection(JoinCollection val)
        {
            this.AddRange(val);
        }

        internal JoinCollection(Join[] val)
        {
            this.AddRange(val);
        }

        internal int Add(Join val)
        {
            return base.List.Add(val);
        }

        internal void AddRange(Join[] val)
        {
            for (int i = 0; i < val.Length; i++)
            {
                this.Add(val[i]);
            }
        }

        internal void AddRange(JoinCollection val)
        {
            for (int i = 0; i < val.Count; i++)
            {
                this.Add(val[i]);
            }
        }

        public bool Contains(Join val)
        {
            return base.List.Contains(val);
        }

        public void CopyTo(Join[] array, int index)
        {
            base.List.CopyTo(array, index);
        }

        public JoinEnumerator GetEnumerator()
        {
            return new JoinEnumerator(this);
        }

        public int IndexOf(Join val)
        {
            return base.List.IndexOf(val);
        }

        internal void Insert(int index, Join val)
        {
            base.List.Insert(index, val);
        }

        internal void Remove(Join val)
        {
            base.List.Remove(val);
        }

        internal Join this[int index]
        {
            get
            {
                return (Join) base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }

        public class JoinEnumerator : IEnumerator
        {
            private IEnumerator baseEnumerator;
            private IEnumerable temp;

            public JoinEnumerator(JoinCollection mappings)
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

            public Join Current
            {
                get
                {
                    return (Join) this.baseEnumerator.Current;
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

