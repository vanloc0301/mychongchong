namespace SkyMap.Net.SqlOM
{
    using System;

    public class FromClause : ICloneable
    {
        private FromTerm baseTable = null;
        private JoinCollection joins = new JoinCollection();

        internal FromClause()
        {
        }

        public FromClause Clone()
        {
            FromClause clause = new FromClause();
            clause.joins = new JoinCollection(this.joins);
            clause.baseTable = this.baseTable;
            return clause;
        }

        public void Join(JoinType type, FromTerm leftTable, FromTerm rightTable)
        {
            this.Join(type, leftTable, rightTable, new JoinCondition[0]);
        }

        public void Join(JoinType type, FromTerm leftTable, FromTerm rightTable, JoinCondition cond)
        {
            this.Join(type, leftTable, rightTable, new JoinCondition[] { cond });
        }

        public void Join(JoinType type, FromTerm leftTable, FromTerm rightTable, JoinCondition[] conditions)
        {
            WhereClause clause = new WhereClause(WhereClauseRelationship.And);
            foreach (JoinCondition condition in conditions)
            {
                clause.Terms.Add(WhereTerm.CreateCompare(SqlExpression.Field(condition.LeftField, leftTable), SqlExpression.Field(condition.RightField, rightTable), CompareOperator.Equal));
            }
            this.Join(type, leftTable, rightTable, clause);
        }

        public void Join(JoinType type, FromTerm leftTable, FromTerm rightTable, WhereClause conditions)
        {
            if (conditions.IsEmpty && (type != JoinType.Cross))
            {
                throw new InvalidQueryException("A join must have at least one condition.");
            }
            this.joins.Add(new SkyMap.Net.SqlOM.Join(leftTable, rightTable, conditions, type));
        }

        public void Join(JoinType type, FromTerm leftTable, FromTerm rightTable, JoinCondition cond1, JoinCondition cond2)
        {
            this.Join(type, leftTable, rightTable, new JoinCondition[] { cond1, cond2 });
        }

        public void Join(JoinType type, FromTerm leftTable, FromTerm rightTable, string leftField, string rightField)
        {
            this.Join(type, leftTable, rightTable, new JoinCondition(leftField, rightField));
        }

        public void Join(JoinType type, FromTerm leftTable, FromTerm rightTable, JoinCondition cond1, JoinCondition cond2, JoinCondition cond3)
        {
            this.Join(type, leftTable, rightTable, new JoinCondition[] { cond1, cond2, cond3 });
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public bool TermExists(string alias)
        {
            if ((this.joins.Count == 0) && (this.baseTable != null))
            {
                return (string.Compare(this.baseTable.RefName, alias) == 0);
            }
            foreach (SkyMap.Net.SqlOM.Join join in this.joins)
            {
                if (string.Compare(join.RightTable.RefName, alias) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public FromTerm BaseTable
        {
            get
            {
                return this.baseTable;
            }
            set
            {
                this.baseTable = value;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return ((this.baseTable == null) && (this.joins.Count == 0));
            }
        }

        internal JoinCollection Joins
        {
            get
            {
                return this.joins;
            }
        }
    }
}

