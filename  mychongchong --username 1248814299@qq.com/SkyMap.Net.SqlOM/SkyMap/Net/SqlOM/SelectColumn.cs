namespace SkyMap.Net.SqlOM
{
    using System;

    public class SelectColumn
    {
        private string alias;
        private SqlExpression expr;

        public SelectColumn(string columnName) : this(columnName, null)
        {
        }

        public SelectColumn(SqlExpression expr, string columnAlias)
        {
            this.expr = expr;
            this.alias = columnAlias;
        }

        public SelectColumn(string columnName, FromTerm table) : this(columnName, table, null)
        {
        }

        public SelectColumn(string columnName, FromTerm table, string columnAlias) : this(columnName, table, columnAlias, SqlAggregationFunction.None)
        {
        }

        public SelectColumn(string columnName, FromTerm table, string columnAlias, SqlAggregationFunction function)
        {
            if (function == SqlAggregationFunction.None)
            {
                this.expr = SqlExpression.Field(columnName, table);
            }
            else
            {
                this.expr = SqlExpression.Function(function, SqlExpression.Field(columnName, table));
            }
            this.alias = columnAlias;
        }

        public string ColumnAlias
        {
            get
            {
                return this.alias;
            }
        }

        internal SqlExpression Expression
        {
            get
            {
                return this.expr;
            }
        }
    }
}

