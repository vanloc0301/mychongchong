namespace SkyMap.Net.DAO.Search
{
    using System;
    using System.Text;

    [Serializable]
    internal class HQLSearch : ISearch
    {
        private string fromClause = "";
        private string groupByClause = "";
        private string joinFrom = "";
        private string joinWhere = "";
        private string orderByClause = "";
        private string selectClause = "";
        private string sSql = "";
        private string whereClause = "";

        internal HQLSearch()
        {
        }

        public virtual void Clear()
        {
            this.selectClause = "";
            this.fromClause = "";
            this.groupByClause = "";
            this.joinFrom = "";
            this.joinWhere = "";
            this.orderByClause = "";
            this.sSql = "";
            this.whereClause = "";
        }

        public virtual void SetInnerJoins(string joinForm, string joinWhere)
        {
            this.joinFrom = joinForm;
            this.joinWhere = joinWhere;
        }

        public string ToStatementString()
        {
            if (this.sSql.Trim().Length > 0)
            {
                return this.sSql;
            }
            StringBuilder builder = new StringBuilder(((((this.selectClause.Length + this.fromClause.Length) + this.joinFrom.Length) + this.whereClause.Length) + this.joinWhere.Length) + 20);
            if (this.selectClause.Trim().Length > 0)
            {
                builder.Append("select ").Append(this.selectClause);
            }
            builder.Append(" from ").Append(this.fromClause).Append(this.joinFrom).Append(this.joinWhere);
            if (this.whereClause.Trim().Length > 0)
            {
                builder.Append(" where ").Append(this.whereClause);
            }
            if (this.orderByClause.Trim().Length > 0)
            {
                builder.Append(" order by ").Append(this.orderByClause);
            }
            if (this.groupByClause.Trim().Length > 0)
            {
                builder.Append(" group by ").Append(this.groupByClause);
            }
            Console.WriteLine(builder.ToString());
            return builder.ToString();
        }

        public virtual string From
        {
            set
            {
                this.fromClause = value;
            }
        }

        public virtual string GroupBy
        {
            set
            {
                this.groupByClause = value;
            }
        }

        public virtual string OrderBy
        {
            set
            {
                this.orderByClause = value;
            }
        }

        public virtual string Select
        {
            set
            {
                this.selectClause = value;
            }
        }

        public virtual string Sql
        {
            set
            {
                this.sSql = value;
            }
        }

        public virtual string Where
        {
            set
            {
                this.whereClause = value;
            }
        }
    }
}

