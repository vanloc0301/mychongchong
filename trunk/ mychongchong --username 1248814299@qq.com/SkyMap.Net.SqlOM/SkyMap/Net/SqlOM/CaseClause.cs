namespace SkyMap.Net.SqlOM
{
    using System;

    public class CaseClause
    {
        private SqlExpression elseVal = null;
        private CaseTermCollection terms = new CaseTermCollection();

        public SqlExpression ElseValue
        {
            get
            {
                return this.elseVal;
            }
            set
            {
                this.elseVal = value;
            }
        }

        public CaseTermCollection Terms
        {
            get
            {
                return this.terms;
            }
        }
    }
}

