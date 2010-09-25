namespace SkyMap.Net.SqlOM
{
    using System;

    public class OrderByTerm
    {
        private OrderByDirection direction;
        private string field;
        private FromTerm table;

        public OrderByTerm(string field, OrderByDirection dir) : this(field, null, dir)
        {
        }

        public OrderByTerm(string field, FromTerm table, OrderByDirection dir)
        {
            this.field = field;
            this.table = table;
            this.direction = dir;
        }

        public OrderByDirection Direction
        {
            get
            {
                return this.direction;
            }
        }

        public string Field
        {
            get
            {
                return this.field;
            }
        }

        public FromTerm Table
        {
            get
            {
                return this.table;
            }
        }

        public string TableAlias
        {
            get
            {
                return ((this.table == null) ? null : this.table.RefName);
            }
        }
    }
}

