namespace SkyMap.Net.DAO.Search
{
    using System;

    public sealed class SearchFactory
    {
        private SearchFactory()
        {
        }

        public static ISearch GetSearch()
        {
            return new HQLSearch();
        }

        public static ISearch GetSearch(string sql)
        {
            ISearch search = GetSearch();
            search.Sql = sql;
            return search;
        }

        public static ISearch GetSearch(Type type, string[] names, string[] vals, string[] orderBys, string[] fetchJoins)
        {
            int num;
            ISearch search = GetSearch();
            search.From = type.Name + " t1 ";
            if ((fetchJoins != null) && (fetchJoins.Length > 0))
            {
                string joinForm = string.Empty;
                for (num = 0; num < fetchJoins.Length; num++)
                {
                    joinForm = joinForm + string.Format(" left join fetch {0}.{1} t{2}", "t1", fetchJoins[num], num + 2);
                }
                search.SetInnerJoins(joinForm, string.Empty);
            }
            if ((names != null) && (names.Length > 0))
            {
                if (vals == null)
                {
                    throw new ArgumentException();
                }
                if (names.Length != vals.Length)
                {
                    throw new ArgumentException("输入的参数与参数值的数量不相等");
                }
                string str2 = string.Empty;
                for (num = 0; num < names.Length; num++)
                {
                    if (num > 0)
                    {
                        str2 = str2 + " and ";
                    }
                    string str4 = str2;
                    str2 = str4 + "t1." + names[num] + "='" + vals[num] + "' ";
                }
                search.Where = str2;
            }
            if ((orderBys != null) && (orderBys.Length > 0))
            {
                string str3 = string.Empty;
                for (num = 0; num < orderBys.Length; num++)
                {
                    if (num > 0)
                    {
                        str3 = str3 + ",";
                    }
                    str3 = str3 + "t1." + orderBys[num];
                }
                search.OrderBy = str3;
            }
            return search;
        }

        public static ISearch GetSearch(string select, string from, string joinFrom, string joinWhere, string where, string orderBy, string groupBy)
        {
            ISearch search = GetSearch();
            search.Select = select;
            search.From = from;
            search.SetInnerJoins(joinFrom, joinWhere);
            search.Where = where;
            search.OrderBy = orderBy;
            search.GroupBy = groupBy;
            return search;
        }
    }
}

