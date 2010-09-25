namespace SkyMap.Net.SqlOM
{
    using System;

    internal class Join
    {
        private WhereClause conditions;
        private FromTerm leftTable;
        private FromTerm rightTable;
        private JoinType type;

        public Join(FromTerm leftTable, FromTerm rightTable, WhereClause conditions, JoinType type)
        {
            this.leftTable = leftTable;
            this.rightTable = rightTable;
            this.conditions = conditions;
            this.type = type;
        }

        public WhereClause Conditions
        {
            get
            {
                return this.conditions;
            }
        }

        public FromTerm LeftTable
        {
            get
            {
                return this.leftTable;
            }
        }

        public FromTerm RightTable
        {
            get
            {
                return this.rightTable;
            }
        }

        public JoinType Type
        {
            get
            {
                return this.type;
            }
        }
    }
}

