namespace SkyMap.Net.SqlOM
{
    using System;

    public class FromTerm
    {
        private string alias;
        private object expr;
        private string ns1;
        private string ns2;
        private FromTermType type;

        private FromTerm()
        {
        }

        public static FromTerm SubQuery(SelectQuery query, string alias)
        {
            FromTerm term = new FromTerm();
            term.expr = query;
            term.alias = alias;
            term.type = FromTermType.SubQueryObj;
            return term;
        }

        public static FromTerm SubQuery(string query, string alias)
        {
            FromTerm term = new FromTerm();
            term.expr = query;
            term.alias = alias;
            term.type = FromTermType.SubQuery;
            return term;
        }

        public static FromTerm Table(string name)
        {
            return Table(name, null);
        }

        public static FromTerm Table(string tableName, string alias)
        {
            return Table(tableName, alias, null);
        }

        public static FromTerm Table(string tableName, string alias, string ns)
        {
            return Table(tableName, alias, ns, null);
        }

        public static FromTerm Table(string tableName, string alias, string ns1, string ns2)
        {
            FromTerm term = new FromTerm();
            term.expr = tableName;
            term.alias = alias;
            term.type = FromTermType.Table;
            term.ns1 = ns1;
            term.ns2 = ns2;
            return term;
        }

        public static FromTerm TermRef(string name)
        {
            return Table(null, name);
        }

        internal string Alias
        {
            get
            {
                return this.alias;
            }
        }

        internal object Expression
        {
            get
            {
                return this.expr;
            }
        }

        internal string Ns1
        {
            get
            {
                return this.ns1;
            }
        }

        internal string Ns2
        {
            get
            {
                return this.ns2;
            }
        }

        public string RefName
        {
            get
            {
                return (((this.alias == null) && (this.type == FromTermType.Table)) ? ((string) this.expr) : this.alias);
            }
        }

        internal FromTermType Type
        {
            get
            {
                return this.type;
            }
        }
    }
}

