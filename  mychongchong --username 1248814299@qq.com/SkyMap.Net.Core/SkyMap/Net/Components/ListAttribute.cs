namespace SkyMap.Net.Components
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    [AttributeUsage(AttributeTargets.Property)]
    public class ListAttribute : Attribute
    {
        private MethodInfo mi;
        private string name;
        private object[] parameters;
        private Type type;

        public ListAttribute()
        {
        }

        public ListAttribute(Type type, string name, object[] parameters)
        {
            this.type = type;
            this.name = name;
            this.parameters = parameters;
        }

        public virtual IEnumerable DataSource
        {
            get
            {
                if (this.mi == null)
                {
                    List<Type> list = new List<Type>();
                    foreach (object obj2 in this.parameters)
                    {
                        list.Add(obj2.GetType());
                    }
                    if (list.Count == 0)
                    {
                        this.mi = this.type.GetMethod(this.name);
                    }
                    else
                    {
                        this.mi = this.type.GetMethod(this.name, list.ToArray());
                    }
                }
                return (IList) this.mi.Invoke(null, this.parameters);
            }
        }
    }
}

