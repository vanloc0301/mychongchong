namespace SkyMap.Net.SqlOM
{
    using System;

    public class GroupByTerm
    {
        private string field;
        private FromTerm table;

        public GroupByTerm(string field) : this(field, null)
        {
        }

        public GroupByTerm(string field, FromTerm table)
        {
            this.field = field;
            this.table = table;
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

        internal string TableAlias
        {
            get
            {
                return ((this.table == null) ? null : this.table.RefName);
            }
        }
    }
}

