namespace SkyMap.Net.SqlOM.Render
{
    using SkyMap.Net.SqlOM;
    using System;
    using System.Text;

    public class MySqlRenderer : SqlOmRenderer
    {
        public MySqlRenderer() : base('`', '`')
        {
        }

        protected override void IfNull(StringBuilder builder, SqlExpression expr)
        {
            builder.Append("ifnull(");
            this.Expression(builder, expr.SubExpr1);
            builder.Append(", ");
            this.Expression(builder, expr.SubExpr2);
            builder.Append(")");
        }

        public override string RenderPage(int pageIndex, int pageSize, int totalRowCount, SelectQuery query)
        {
            return this.RenderSelect(query, false, pageIndex * pageSize, pageSize);
        }

        public override string RenderRowCount(SelectQuery query)
        {
            string str = this.RenderSelect(query);
            SelectQuery query2 = new SelectQuery();
            SelectColumn column = new SelectColumn("*", null, "cnt", SqlAggregationFunction.Count);
            query2.Columns.Add(column);
            query2.FromClause.BaseTable = FromTerm.SubQuery(str, "t");
            return this.RenderSelect(query2);
        }

        public override string RenderSelect(SelectQuery query)
        {
            return this.RenderSelect(query, false, 0, query.Top);
        }

        private string RenderSelect(SelectQuery query, bool forRowCount, int offset, int limitRows)
        {
            query.Validate();
            StringBuilder builder = new StringBuilder();
            this.Select(builder, query.Distinct);
            if (forRowCount)
            {
                this.SelectColumn(builder, new SelectColumn("*", null, "cnt", SqlAggregationFunction.Count));
            }
            else
            {
                this.SelectColumns(builder, query.Columns);
            }
            this.FromClause(builder, query.FromClause, query.TableSpace);
            this.Where(builder, query.WherePhrase);
            this.WhereClause(builder, query.WherePhrase);
            this.GroupBy(builder, query.GroupByTerms);
            this.GroupByTerms(builder, query.GroupByTerms);
            if (query.GroupByWithCube)
            {
                throw new InvalidQueryException("MySql does not support WITH CUBE modifier.");
            }
            if (query.GroupByWithRollup)
            {
                builder.Append(" with rollup");
            }
            this.Having(builder, query.HavingPhrase);
            this.WhereClause(builder, query.HavingPhrase);
            this.OrderBy(builder, query.OrderByTerms);
            this.OrderByTerms(builder, query.OrderByTerms);
            if (limitRows > -1)
            {
                builder.AppendFormat(" limit {0}, {1}", offset, limitRows);
            }
            return builder.ToString();
        }
    }
}

