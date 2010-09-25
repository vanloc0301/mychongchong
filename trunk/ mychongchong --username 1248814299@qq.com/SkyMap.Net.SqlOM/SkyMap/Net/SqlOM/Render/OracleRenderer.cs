namespace SkyMap.Net.SqlOM.Render
{
    using SkyMap.Net.SqlOM;
    using System;
    using System.Text;

    public class OracleRenderer : SqlOmRenderer
    {
        public OracleRenderer() : base('"', '"')
        {
            base.DateFormat = "dd-MMM-yy";
            base.DateTimeFormat = "dd-MMM-yy HH:mm:ss";
        }

        protected override void BitwiseAnd(StringBuilder builder, WhereTerm term)
        {
            builder.Append("BITAND(");
            this.Expression(builder, term.Expr1);
            builder.Append(", ");
            this.Expression(builder, term.Expr2);
            builder.Append(") > 0");
        }

        protected override void IfNull(StringBuilder builder, SqlExpression expr)
        {
            builder.Append("nvl(");
            this.Expression(builder, expr.SubExpr1);
            builder.Append(", ");
            this.Expression(builder, expr.SubExpr2);
            builder.Append(")");
        }

        public override string RenderRowCount(SelectQuery query)
        {
            string str = this.RenderSelect(query, -1);
            SelectQuery query2 = new SelectQuery();
            SelectColumn column = new SelectColumn("*", null, "cnt", SqlAggregationFunction.Count);
            query2.Columns.Add(column);
            query2.FromClause.BaseTable = FromTerm.SubQuery(str, "t");
            return this.RenderSelect(query2);
        }

        public override string RenderSelect(SelectQuery query)
        {
            if ((query.Top > -1) && (query.OrderByTerms.Count > 0))
            {
                string str = this.RenderSelect(query, -1);
                SelectQuery query2 = new SelectQuery();
                SelectColumn column = new SelectColumn("*");
                query2.Columns.Add(column);
                query2.FromClause.BaseTable = FromTerm.SubQuery(str, "t");
                return this.RenderSelect(query2, query.Top);
            }
            return this.RenderSelect(query, query.Top);
        }

        private string RenderSelect(SelectQuery query, int limitRows)
        {
            query.Validate();
            StringBuilder builder = new StringBuilder();
            this.Select(builder, query.Distinct);
            this.SelectColumns(builder, query.Columns);
            this.FromClause(builder, query.FromClause, query.TableSpace);
            WhereClause group = new WhereClause(WhereClauseRelationship.And);
            group.SubClauses.Add(query.WherePhrase);
            if (limitRows > -1)
            {
                group.Terms.Add(WhereTerm.CreateCompare(SqlExpression.PseudoField("rownum"), SqlExpression.Number(limitRows), CompareOperator.LessOrEqual));
            }
            this.Where(builder, group);
            this.WhereClause(builder, group);
            this.GroupBy(builder, query.GroupByTerms);
            if (query.GroupByWithCube)
            {
                builder.Append(" cube (");
            }
            else if (query.GroupByWithRollup)
            {
                builder.Append(" rollup (");
            }
            this.GroupByTerms(builder, query.GroupByTerms);
            if (query.GroupByWithCube || query.GroupByWithRollup)
            {
                builder.Append(" )");
            }
            this.Having(builder, query.HavingPhrase);
            this.WhereClause(builder, query.HavingPhrase);
            this.OrderBy(builder, query.OrderByTerms);
            this.OrderByTerms(builder, query.OrderByTerms);
            return builder.ToString();
        }

        protected override bool UpperCaseIdentifiers
        {
            get
            {
                return true;
            }
        }
    }
}

