namespace SkyMap.Net.SqlOM
{
    using System;

    public class WhereTerm : ICloneable
    {
        private SqlExpression expr1;
        private SqlExpression expr2;
        private SqlExpression expr3;
        private CompareOperator op;
        private string subQuery;
        private WhereTermType type;
        private SqlConstantCollection values;

        private WhereTerm()
        {
        }

        public WhereTerm Clone()
        {
            WhereTerm term = new WhereTerm();
            term.expr1 = this.expr1;
            term.expr2 = this.expr2;
            term.expr3 = this.expr3;
            term.op = this.op;
            term.type = this.type;
            term.subQuery = term.subQuery;
            term.values = new SqlConstantCollection(this.values);
            return term;
        }

        public static WhereTerm CreateBetween(SqlExpression expr, SqlExpression lowBound, SqlExpression highBound)
        {
            WhereTerm term = new WhereTerm();
            term.expr1 = expr;
            term.expr2 = lowBound;
            term.expr3 = highBound;
            term.type = WhereTermType.Between;
            return term;
        }

        public static WhereTerm CreateCompare(SqlExpression expr1, SqlExpression expr2, CompareOperator op)
        {
            WhereTerm term = new WhereTerm();
            term.expr1 = expr1;
            term.expr2 = expr2;
            term.op = op;
            term.type = WhereTermType.Compare;
            return term;
        }

        public static WhereTerm CreateExists(string sql)
        {
            WhereTerm term = new WhereTerm();
            term.subQuery = sql;
            term.type = WhereTermType.Exists;
            return term;
        }

        public static WhereTerm CreateIn(SqlExpression expr, SqlConstantCollection values)
        {
            WhereTerm term = new WhereTerm();
            term.expr1 = expr;
            term.values = values;
            term.type = WhereTermType.In;
            return term;
        }

        public static WhereTerm CreateIn(SqlExpression expr, string sql)
        {
            WhereTerm term = new WhereTerm();
            term.expr1 = expr;
            term.subQuery = sql;
            term.type = WhereTermType.InSubQuery;
            return term;
        }

        public static WhereTerm CreateIsNotNull(SqlExpression expr)
        {
            WhereTerm term = new WhereTerm();
            term.expr1 = expr;
            term.type = WhereTermType.IsNotNull;
            return term;
        }

        public static WhereTerm CreateIsNull(SqlExpression expr)
        {
            WhereTerm term = new WhereTerm();
            term.expr1 = expr;
            term.type = WhereTermType.IsNull;
            return term;
        }

        public static WhereTerm CreateNotExists(string sql)
        {
            WhereTerm term = new WhereTerm();
            term.subQuery = sql;
            term.type = WhereTermType.NotExists;
            return term;
        }

        public static WhereTerm CreateNotIn(SqlExpression expr, SqlConstantCollection values)
        {
            WhereTerm term = new WhereTerm();
            term.expr1 = expr;
            term.values = values;
            term.type = WhereTermType.NotIn;
            return term;
        }

        public static WhereTerm CreateNotIn(SqlExpression expr, string sql)
        {
            WhereTerm term = new WhereTerm();
            term.expr1 = expr;
            term.subQuery = sql;
            term.type = WhereTermType.NotInSubQuery;
            return term;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        internal SqlExpression Expr1
        {
            get
            {
                return this.expr1;
            }
        }

        internal SqlExpression Expr2
        {
            get
            {
                return this.expr2;
            }
        }

        internal SqlExpression Expr3
        {
            get
            {
                return this.expr3;
            }
        }

        internal CompareOperator Op
        {
            get
            {
                return this.op;
            }
        }

        internal string SubQuery
        {
            get
            {
                return this.subQuery;
            }
        }

        internal WhereTermType Type
        {
            get
            {
                return this.type;
            }
        }

        internal SqlConstantCollection Values
        {
            get
            {
                return this.values;
            }
        }
    }
}

