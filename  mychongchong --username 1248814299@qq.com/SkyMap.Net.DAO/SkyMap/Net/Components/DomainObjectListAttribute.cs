namespace SkyMap.Net.Components
{
    using SkyMap.Net.DAO;
    using System;
    using System.Collections;

    [AttributeUsage(AttributeTargets.Property)]
    public class DomainObjectListAttribute : ListAttribute
    {
        public string CacheKey;
        public string[] Names;
        public string[] OrderBys;
        public Type type;
        public string[] Values;

        public override IEnumerable DataSource
        {
            get
            {
                return QueryHelper.List(this.type, this.CacheKey, this.Names, this.Values, this.OrderBys);
            }
        }
    }
}

