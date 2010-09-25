namespace SkyMap.Net.SqlOM.Render
{
    using SkyMap.Net.SqlOM;
    using System;
    using System.Text;

    public class SqlServerRenderer : SqlOmRenderer
    {
        public SqlServerRenderer() : base('[', ']')
        {
        }

        protected override void IfNull(StringBuilder builder, SqlExpression expr)
        {
            builder.Append("isnull(");
            this.Expression(builder, expr.SubExpr1);
            builder.Append(", ");
            this.Expression(builder, expr.SubExpr2);
            builder.Append(")");
        }

        public override string RenderRowCount(SelectQuery query)
        {
            string str = this.RenderSelect(query, false);
            SelectQuery query2 = new SelectQuery();
            SelectColumn column = new SelectColumn("*", null, "cnt", SqlAggregationFunction.Count);
            query2.Columns.Add(column);
            query2.FromClause.BaseTable = FromTerm.SubQuery(str, "t");
            return this.RenderSelect(query2);
        }

        public override string RenderSelect(SelectQuery query)
        {
            return this.RenderSelect(query, true);
        }

        private string RenderSelect(SelectQuery query, bool renderOrderBy)
        {
            query.Validate();
            StringBuilder builder = new StringBuilder();
            this.Select(builder, query.Distinct);
            if (query.Top > -1)
            {
                builder.AppendFormat("top {0} ", query.Top);
            }
            this.SelectColumns(builder, query.Columns);
            this.FromClause(builder, query.FromClause, query.TableSpace);
            this.Where(builder, query.WherePhrase);
            this.WhereClause(builder, query.WherePhrase);
            this.GroupBy(builder, query.GroupByTerms);
            this.GroupByTerms(builder, query.GroupByTerms);
            if (query.GroupByWithCube)
            {
                builder.Append(" with cube");
            }
            else if (query.GroupByWithRollup)
            {
                builder.Append(" with rollup");
            }
            this.Having(builder, query.HavingPhrase);
            this.WhereClause(builder, query.HavingPhrase);
            if (renderOrderBy)
            {
                this.OrderBy(builder, query.OrderByTerms);
                this.OrderByTerms(builder, query.OrderByTerms);
            }
            return builder.ToString();
        }
    }
}

