namespace SkyMap.Net.SqlOM
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class SqlConstantCollection : CollectionBase
    {
        public SqlConstantCollection()
        {
        }

        public SqlConstantCollection(int capacity)
        {
            base.InnerList.Capacity = capacity;
        }

        public SqlConstantCollection(SqlConstant[] items)
        {
            this.AddRange(items);
        }

        public SqlConstantCollection(SqlConstantCollection items)
        {
            this.AddRange(items);
        }

        public virtual void Add(SqlConstant value)
        {
            base.List.Add(value);
        }

        public void Add(object val)
        {
            if (val != null)
            {
                SqlConstant constant;
                if (val is string)
                {
                    constant = SqlConstant.String((string) val);
                }
                else if (val is int)
                {
                    constant = SqlConstant.Number((int) val);
                }
                else if (val is DateTime)
                {
                    constant = SqlConstant.Date((DateTime) val);
                }
                else if (val is double)
                {
                    constant = SqlConstant.Number((double) val);
                }
                else if (val is float)
                {
                    constant = SqlConstant.Number((double) val);
                }
                else if (val is decimal)
                {
                    constant = SqlConstant.Number((decimal) val);
                }
                else
                {
                    constant = SqlConstant.String(val.ToString());
                }
                this.Add(constant);
            }
        }

        public virtual void AddRange(SqlConstant[] items)
        {
            foreach (SqlConstant constant in items)
            {
                base.List.Add(constant);
            }
        }

        public virtual void AddRange(SqlConstantCollection items)
        {
            foreach (SqlConstant constant in items)
            {
                base.List.Add(constant);
            }
        }

        public virtual bool Contains(SqlConstant value)
        {
            return base.List.Contains(value);
        }

        public static SqlConstantCollection FromList(IList values)
        {
            SqlConstantCollection constants = new SqlConstantCollection(values.Count);
            foreach (object obj2 in values)
            {
                constants.Add(obj2);
            }
            return constants;
        }

        public virtual Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        public virtual int IndexOf(SqlConstant value)
        {
            return base.List.IndexOf(value);
        }

        public virtual void Insert(int index, SqlConstant value)
        {
            base.List.Insert(index, value);
        }

        public virtual void Remove(SqlConstant value)
        {
            base.List.Remove(value);
        }

        public virtual SqlConstant this[int index]
        {
            get
            {
                return (SqlConstant) base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }

        public class Enumerator : IEnumerator
        {
            private IEnumerator wrapped;

            public Enumerator(SqlConstantCollection collection)
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

            public SqlConstant Current
            {
                get
                {
                    return (SqlConstant) this.wrapped.Current;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return (SqlConstant) this.wrapped.Current;
                }
            }
        }
    }
}

