namespace SkyMap.Net.SqlOM
{
    using System;

    public class SqlExpression
    {
        private SkyMap.Net.SqlOM.CaseClause caseClause = new SkyMap.Net.SqlOM.CaseClause();
        private SqlAggregationFunction func = SqlAggregationFunction.None;
        private SqlExpression subExpr1;
        private SqlExpression subExpr2;
        private FromTerm table = null;
        private SqlExpressionType type;
        private object val;

        private SqlExpression()
        {
        }

        public static SqlExpression Case(SkyMap.Net.SqlOM.CaseClause caseClause)
        {
            SqlExpression expression = new SqlExpression();
            expression.type = SqlExpressionType.Case;
            expression.caseClause = caseClause;
            return expression;
        }

        public static SqlExpression Constant(SqlConstant val)
        {
            SqlExpression expression = new SqlExpression();
            expression.val = val;
            expression.type = SqlExpressionType.Constant;
            return expression;
        }

        public static SqlExpression Constant(SqlDataType dataType, object val)
        {
            SqlExpression expression = new SqlExpression();
            expression.val = new SqlConstant(dataType, val);
            expression.type = SqlExpressionType.Constant;
            return expression;
        }

        public static SqlExpression Date(DateTime val)
        {
            return Constant(SqlConstant.Date(val));
        }

        public static SqlExpression Field(string fieldName)
        {
            return Field(fieldName, null);
        }

        public static SqlExpression Field(string fieldName, FromTerm table)
        {
            SqlExpression expression = new SqlExpression();
            expression.val = fieldName;
            expression.table = table;
            expression.type = SqlExpressionType.Field;
            return expression;
        }

        public static SqlExpression Function(SqlAggregationFunction func, SqlExpression param)
        {
            SqlExpression expression = new SqlExpression();
            expression.type = SqlExpressionType.Function;
            expression.subExpr1 = param;
            expression.func = func;
            return expression;
        }

        public static SqlExpression IfNull(SqlExpression test, SqlExpression val)
        {
            SqlExpression expression = new SqlExpression();
            expression.type = SqlExpressionType.IfNull;
            expression.subExpr1 = test;
            expression.subExpr2 = val;
            return expression;
        }

        public static SqlExpression Null()
        {
            SqlExpression expression = new SqlExpression();
            expression.type = SqlExpressionType.Null;
            return expression;
        }

        public static SqlExpression Number(double val)
        {
            return Constant(SqlConstant.Number(val));
        }

        public static SqlExpression Number(int val)
        {
            return Constant(SqlConstant.Number(val));
        }

        public static SqlExpression Parameter(string paramName)
        {
            SqlExpression expression = new SqlExpression();
            expression.val = paramName;
            expression.type = SqlExpressionType.Parameter;
            return expression;
        }

        internal static SqlExpression PseudoField(string fieldName)
        {
            SqlExpression expression = new SqlExpression();
            expression.val = fieldName;
            expression.type = SqlExpressionType.PseudoField;
            return expression;
        }

        public static SqlExpression Raw(string sql)
        {
            SqlExpression expression = new SqlExpression();
            expression.val = sql;
            expression.type = SqlExpressionType.Raw;
            return expression;
        }

        public static SqlExpression String(string val)
        {
            return Constant(SqlConstant.String(val));
        }

        public static SqlExpression SubQuery(SelectQuery query)
        {
            SqlExpression expression = new SqlExpression();
            expression.val = query;
            expression.type = SqlExpressionType.SubQueryObject;
            return expression;
        }

        public static SqlExpression SubQuery(string queryText)
        {
            SqlExpression expression = new SqlExpression();
            expression.val = queryText;
            expression.type = SqlExpressionType.SubQueryText;
            return expression;
        }

        internal SqlAggregationFunction AggFunction
        {
            get
            {
                return this.func;
            }
        }

        internal SkyMap.Net.SqlOM.CaseClause CaseClause
        {
            get
            {
                return this.caseClause;
            }
        }

        internal SqlExpression SubExpr1
        {
            get
            {
                return this.subExpr1;
            }
        }

        internal SqlExpression SubExpr2
        {
            get
            {
                return this.subExpr2;
            }
        }

        internal string TableAlias
        {
            get
            {
                return ((this.table == null) ? null : this.table.RefName);
            }
        }

        internal SqlExpressionType Type
        {
            get
            {
                return this.type;
            }
        }

        internal object Value
        {
            get
            {
                return this.val;
            }
        }
    }
}

