namespace SkyMap.Net.SqlOM
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class UpdateTermCollection : CollectionBase
    {
        public UpdateTermCollection()
        {
        }

        public UpdateTermCollection(UpdateTerm[] items)
        {
            this.AddRange(items);
        }

        public UpdateTermCollection(UpdateTermCollection items)
        {
            this.AddRange(items);
        }

        public virtual void Add(UpdateTerm value)
        {
            base.List.Add(value);
        }

        public virtual void AddRange(UpdateTermCollection items)
        {
            foreach (UpdateTerm term in items)
            {
                base.List.Add(term);
            }
        }

        public virtual void AddRange(UpdateTerm[] items)
        {
            foreach (UpdateTerm term in items)
            {
                base.List.Add(term);
            }
        }

        public virtual bool Contains(UpdateTerm value)
        {
            return base.List.Contains(value);
        }

        public virtual bool Contains(string field)
        {
            field = field.ToLower();
            foreach (UpdateTerm term in base.List)
            {
                if (term.FieldName.ToLower() == field)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        public virtual int IndexOf(UpdateTerm value)
        {
            return base.List.IndexOf(value);
        }

        public virtual void Insert(int index, UpdateTerm value)
        {
            base.List.Insert(index, value);
        }

        public virtual void Remove(UpdateTerm value)
        {
            base.List.Remove(value);
        }

        public virtual UpdateTerm this[string field]
        {
            get
            {
                field = field.ToLower();
                foreach (UpdateTerm term in base.List)
                {
                    if (term.FieldName.ToLower() == field)
                    {
                        return term;
                    }
                }
                return null;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("UpdateTerm cannot be null");
                }
                for (int i = 0; i < base.List.Count; i++)
                {
                    UpdateTerm term = base.List[i] as UpdateTerm;
                    if (term.FieldName.ToLower() == value.FieldName.ToLower())
                    {
                        base.List[i] = value;
                        return;
                    }
                }
                throw new ArgumentException("没有找到字段");
            }
        }

        public virtual UpdateTerm this[int index]
        {
            get
            {
                return (UpdateTerm) base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }

        public class Enumerator : IEnumerator
        {
            private IEnumerator wrapped;

            public Enumerator(UpdateTermCollection collection)
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

            public UpdateTerm Current
            {
                get
                {
                    return (UpdateTerm) this.wrapped.Current;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return (UpdateTerm) this.wrapped.Current;
                }
            }
        }
    }
}

