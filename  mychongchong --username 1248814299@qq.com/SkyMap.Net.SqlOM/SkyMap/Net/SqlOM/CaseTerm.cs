namespace SkyMap.Net.SqlOM
{
    using System;

    public class CaseTerm
    {
        private WhereClause cond;
        private SqlExpression val;

        public CaseTerm(WhereClause condition, SqlExpression val)
        {
            this.cond = condition;
            this.val = val;
        }

        internal WhereClause Condition
        {
            get
            {
                return this.cond;
            }
        }

        internal SqlExpression Value
        {
            get
            {
                return this.val;
            }
        }
    }
}

