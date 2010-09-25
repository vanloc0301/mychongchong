namespace SkyMap.Net.SqlOM.Render
{
    using SkyMap.Net.SqlOM;
    using System;
    using System.Text;

    public abstract class SqlOmRenderer : ISqlOmRenderer
    {
        private string dateFormat = "yyyy-MM-dd";
        private string dateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        private char identifierClosingQuote;
        private char identifierOpeningQuote;

        public SqlOmRenderer(char identifierOpeningQuote, char identifierClosingQuote)
        {
            this.identifierOpeningQuote = identifierOpeningQuote;
            this.identifierClosingQuote = identifierClosingQuote;
        }

        private void ApplyOrderBy(OrderByTermCollection terms, SelectQuery orderQuery, bool forward, FromTerm table)
        {
            foreach (SkyMap.Net.SqlOM.OrderByTerm term in terms)
            {
                OrderByDirection dir = term.Direction;
                if (!(forward || (dir != OrderByDirection.Ascending)))
                {
                    dir = OrderByDirection.Descending;
                }
                else if (!(forward || (dir != OrderByDirection.Descending)))
                {
                    dir = OrderByDirection.Ascending;
                }
                orderQuery.OrderByTerms.Add(new SkyMap.Net.SqlOM.OrderByTerm(this.FormatSortFieldName(term.Field.ToString()), table, dir));
            }
        }

        protected virtual void BitwiseAnd(StringBuilder builder, WhereTerm term)
        {
            builder.Append("(");
            this.Expression(builder, term.Expr1);
            builder.Append(" & ");
            this.Expression(builder, term.Expr2);
            builder.Append(") > 0");
        }

        protected virtual void CaseClause(StringBuilder builder, SkyMap.Net.SqlOM.CaseClause clause)
        {
            builder.Append(" case ");
            foreach (SkyMap.Net.SqlOM.CaseTerm term in clause.Terms)
            {
                this.CaseTerm(builder, term);
            }
            if (clause.ElseValue != null)
            {
                builder.Append(" else ");
                this.Expression(builder, clause.ElseValue);
            }
            builder.Append(" end ");
        }

        protected virtual void CaseTerm(StringBuilder builder, SkyMap.Net.SqlOM.CaseTerm term)
        {
            builder.Append(" when ");
            this.WhereClause(builder, term.Condition);
            builder.Append(" then ");
            this.Expression(builder, term.Value);
        }

        protected virtual void Coma(StringBuilder builder)
        {
            builder.Append(", ");
        }

        protected virtual void Constant(StringBuilder builder, SqlConstant expr)
        {
            switch (expr.Type)
            {
                case SqlDataType.Number:
                    builder.Append(expr.Value.ToString());
                    break;

                case SqlDataType.String:
                    builder.AppendFormat("'{0}'", (expr.Value == null) ? "" : expr.Value.ToString());
                    break;

                case SqlDataType.Date:
                {
                    DateTime time = (DateTime) expr.Value;
                    string format = ((((time.Hour == 0) && (time.Minute == 0)) && (time.Second == 0)) && (time.Millisecond == 0)) ? this.dateFormat : this.dateTimeFormat;
                    builder.AppendFormat("'{0}'", time.ToString(format));
                    break;
                }
            }
        }

        protected virtual void ConstantList(StringBuilder builder, SqlConstantCollection values)
        {
            for (int i = 0; i < values.Count; i++)
            {
                SqlConstant expr = values[i];
                this.Constant(builder, expr);
                if (i != (values.Count - 1))
                {
                    this.Coma(builder);
                }
            }
        }

        protected virtual void Delete(StringBuilder builder, string tableName)
        {
            builder.Append("delete from ");
            this.Identifier(builder, tableName);
            builder.Append(" ");
        }

        public virtual string DeleteStatement(DeleteQuery query)
        {
            query.Validate();
            StringBuilder builder = new StringBuilder();
            this.Delete(builder, query.TableName);
            this.Where(builder, query.WhereClause);
            this.WhereClause(builder, query.WhereClause);
            return builder.ToString();
        }

        protected virtual void Expression(StringBuilder builder, SqlExpression expr)
        {
            SqlExpressionType type = expr.Type;
            if (type == SqlExpressionType.Field)
            {
                this.QualifiedIdentifier(builder, expr.TableAlias, expr.Value.ToString());
            }
            else if (type == SqlExpressionType.Function)
            {
                this.Function(builder, expr.AggFunction, expr.SubExpr1);
            }
            else if (type == SqlExpressionType.Constant)
            {
                this.Constant(builder, (SqlConstant) expr.Value);
            }
            else if (type == SqlExpressionType.SubQueryText)
            {
                builder.AppendFormat("({0})", (string) expr.Value);
            }
            else if (type == SqlExpressionType.SubQueryObject)
            {
                builder.AppendFormat("({0})", this.RenderSelect((SelectQuery) expr.Value));
            }
            else if (type == SqlExpressionType.PseudoField)
            {
                builder.AppendFormat("{0}", (string) expr.Value);
            }
            else if (type == SqlExpressionType.Parameter)
            {
                builder.AppendFormat("{0}", (string) expr.Value);
            }
            else if (type == SqlExpressionType.Raw)
            {
                builder.AppendFormat("{0}", (string) expr.Value);
            }
            else if (type == SqlExpressionType.Case)
            {
                this.CaseClause(builder, expr.CaseClause);
            }
            else if (type == SqlExpressionType.IfNull)
            {
                this.IfNull(builder, expr);
            }
            else
            {
                if (type != SqlExpressionType.Null)
                {
                    throw new InvalidQueryException("Unkown expression type: " + type.ToString());
                }
                builder.Append("null");
            }
        }

        private string FormatSortFieldName(string fieldName)
        {
            return ("sort_" + fieldName);
        }

        protected virtual void From(StringBuilder builder)
        {
            builder.Append(" from ");
        }

        protected virtual void FromClause(StringBuilder builder, SkyMap.Net.SqlOM.FromClause fromClause, string tableSpace)
        {
            this.From(builder);
            this.RenderFromTerm(builder, fromClause.BaseTable, tableSpace);
            foreach (Join join in fromClause.Joins)
            {
                builder.AppendFormat(" {0} join ", join.Type.ToString().ToLower());
                this.RenderFromTerm(builder, join.RightTable, tableSpace);
                if (join.Type != JoinType.Cross)
                {
                    builder.AppendFormat(" on ", new object[0]);
                    this.WhereClause(builder, join.Conditions);
                }
            }
        }

        protected virtual void Function(StringBuilder builder, SqlAggregationFunction func, SqlExpression param)
        {
            builder.AppendFormat("{0}(", func.ToString());
            this.Expression(builder, param);
            builder.AppendFormat(")", new object[0]);
        }

        protected virtual void GroupBy(StringBuilder builder, GroupByTermCollection groupByTerms)
        {
            if (groupByTerms.Count > 0)
            {
                builder.Append(" group by ");
            }
        }

        protected virtual void GroupByTerm(StringBuilder builder, SkyMap.Net.SqlOM.GroupByTerm term)
        {
            this.QualifiedIdentifier(builder, term.TableAlias, term.Field);
        }

        protected virtual void GroupByTerms(StringBuilder builder, GroupByTermCollection groupByTerms)
        {
            foreach (SkyMap.Net.SqlOM.GroupByTerm term in groupByTerms)
            {
                if (term != groupByTerms[0])
                {
                    builder.Append(", ");
                }
                this.GroupByTerm(builder, term);
            }
        }

        protected virtual void Having(StringBuilder builder, SkyMap.Net.SqlOM.WhereClause group)
        {
            if (!group.IsEmpty)
            {
                builder.AppendFormat(" having ", new object[0]);
            }
        }

        protected virtual void Identifier(StringBuilder builder, string name)
        {
            if (name == "*")
            {
                builder.Append(name);
            }
            else
            {
                if (this.UpperCaseIdentifiers)
                {
                    name = name.ToUpper();
                }
                builder.AppendFormat("{0}{1}{2}", this.identifierOpeningQuote, name, this.identifierClosingQuote);
            }
        }

        protected abstract void IfNull(StringBuilder builder, SqlExpression expr);
        protected virtual void Insert(StringBuilder builder, string tableName)
        {
            builder.Append("insert into ");
            this.Identifier(builder, tableName);
            builder.Append(" ");
        }

        protected virtual void InsertColumn(StringBuilder builder, SkyMap.Net.SqlOM.UpdateTerm term)
        {
            this.Identifier(builder, term.FieldName);
        }

        protected virtual void InsertColumns(StringBuilder builder, UpdateTermCollection terms)
        {
            foreach (SkyMap.Net.SqlOM.UpdateTerm term in terms)
            {
                if (terms[0] != term)
                {
                    this.Coma(builder);
                }
                this.InsertColumn(builder, term);
            }
        }

        public virtual string InsertStatement(InsertQuery query)
        {
            query.Validate();
            StringBuilder builder = new StringBuilder();
            this.Insert(builder, query.TableName);
            builder.Append("(");
            this.InsertColumns(builder, query.Terms);
            builder.Append(") values (");
            this.InsertValues(builder, query.Terms);
            builder.Append(")");
            return builder.ToString();
        }

        protected virtual void InsertValue(StringBuilder builder, SkyMap.Net.SqlOM.UpdateTerm term)
        {
            this.Expression(builder, term.Value);
        }

        protected virtual void InsertValues(StringBuilder builder, UpdateTermCollection terms)
        {
            foreach (SkyMap.Net.SqlOM.UpdateTerm term in terms)
            {
                if (terms[0] != term)
                {
                    this.Coma(builder);
                }
                this.InsertValue(builder, term);
            }
        }

        protected virtual void Operator(StringBuilder builder, CompareOperator op)
        {
            if (op == CompareOperator.Equal)
            {
                builder.Append("=");
            }
            else if (op == CompareOperator.NotEqual)
            {
                builder.Append("<>");
            }
            else if (op == CompareOperator.Greater)
            {
                builder.Append(">");
            }
            else if (op == CompareOperator.Less)
            {
                builder.Append("<");
            }
            else if (op == CompareOperator.LessOrEqual)
            {
                builder.Append("<=");
            }
            else if (op == CompareOperator.GreaterOrEqual)
            {
                builder.Append(">=");
            }
            else
            {
                if (op != CompareOperator.Like)
                {
                    throw new InvalidQueryException("Unkown operator: " + op.ToString());
                }
                builder.Append("like");
            }
        }

        protected virtual void OrderBy(StringBuilder builder, OrderByTermCollection orderByTerms)
        {
            if (orderByTerms.Count > 0)
            {
                builder.Append(" order by ");
            }
        }

        protected virtual void OrderByTerm(StringBuilder builder, SkyMap.Net.SqlOM.OrderByTerm term)
        {
            string str = (term.Direction == OrderByDirection.Descending) ? "desc" : "asc";
            this.QualifiedIdentifier(builder, term.TableAlias, term.Field);
            builder.AppendFormat(" {0}", str);
        }

        protected virtual void OrderByTerms(StringBuilder builder, OrderByTermCollection orderByTerms)
        {
            for (int i = 0; i < orderByTerms.Count; i++)
            {
                SkyMap.Net.SqlOM.OrderByTerm term = orderByTerms[i];
                if (i > 0)
                {
                    builder.Append(", ");
                }
                this.OrderByTerm(builder, term);
            }
        }

        protected virtual void QualifiedIdentifier(StringBuilder builder, string qnamespace, string name)
        {
            if (qnamespace != null)
            {
                this.Identifier(builder, qnamespace);
                builder.Append(".");
            }
            this.Identifier(builder, name);
        }

        protected virtual void RelationshipOperator(StringBuilder builder, WhereClauseRelationship relationship)
        {
            builder.AppendFormat(" {0} ", relationship.ToString().ToLower());
        }

        public virtual string RenderDelete(DeleteQuery query)
        {
            return this.DeleteStatement(query);
        }

        protected virtual void RenderFromTerm(StringBuilder builder, FromTerm table, string tableSpace)
        {
            if (table.Type == FromTermType.Table)
            {
                if (table.Ns1 != null)
                {
                    this.TableNamespace(builder, table.Ns1);
                }
                if (table.Ns2 != null)
                {
                    this.TableNamespace(builder, table.Ns2);
                }
                if (((table.Ns1 == null) && (table.Ns2 == null)) && (tableSpace != null))
                {
                    this.TableNamespace(builder, tableSpace);
                }
                this.Identifier(builder, (string) table.Expression);
            }
            else if (table.Type == FromTermType.SubQuery)
            {
                builder.AppendFormat("( {0} )", table.Expression);
            }
            else
            {
                if (table.Type != FromTermType.SubQueryObj)
                {
                    throw new InvalidQueryException("Unknown FromExpressionType: " + table.Type.ToString());
                }
                builder.AppendFormat("( {0} )", this.RenderSelect((SelectQuery) table.Expression));
            }
            if (table.Alias != null)
            {
                builder.AppendFormat(" ", new object[0]);
                this.Identifier(builder, table.Alias);
            }
        }

        public virtual string RenderInsert(InsertQuery query)
        {
            return this.InsertStatement(query);
        }

        public virtual string RenderPage(int pageIndex, int pageSize, int totalRowCount, SelectQuery query)
        {
            if (query.OrderByTerms.Count == 0)
            {
                throw new InvalidQueryException("OrderBy must be specified for paging to work on SqlServer.");
            }
            int num = pageSize;
            if ((pageSize * (pageIndex + 1)) > totalRowCount)
            {
                num = totalRowCount - (pageSize * pageIndex);
            }
            if (num < 0)
            {
                num = 0;
            }
            SelectQuery query2 = query.Clone();
            query2.Top = (pageIndex + 1) * pageSize;
            foreach (SkyMap.Net.SqlOM.OrderByTerm term in query2.OrderByTerms)
            {
                query2.Columns.Add(new SkyMap.Net.SqlOM.SelectColumn(term.Field, term.Table, this.FormatSortFieldName(term.Field), SqlAggregationFunction.None));
            }
            string str = this.RenderSelect(query2);
            SelectQuery orderQuery = new SelectQuery();
            orderQuery.Columns.Add(new SkyMap.Net.SqlOM.SelectColumn("*"));
            orderQuery.Top = num;
            orderQuery.FromClause.BaseTable = FromTerm.SubQuery(str, "r");
            this.ApplyOrderBy(query2.OrderByTerms, orderQuery, false, orderQuery.FromClause.BaseTable);
            string str2 = this.RenderSelect(orderQuery);
            SelectQuery query4 = new SelectQuery();
            query4.Columns.AddRange(query.Columns);
            query4.FromClause.BaseTable = FromTerm.SubQuery(str2, "f");
            this.ApplyOrderBy(query2.OrderByTerms, query4, true, query4.FromClause.BaseTable);
            return this.RenderSelect(query4);
        }

        public abstract string RenderRowCount(SelectQuery query);
        public abstract string RenderSelect(SelectQuery query);
        public virtual string RenderUnion(SqlUnion union)
        {
            StringBuilder builder = new StringBuilder();
            foreach (SqlUnionItem item in union.Items)
            {
                if (item != union.Items[0])
                {
                    builder.AppendFormat(" union {0} ", (item.RepeatingAction == DistinctModifier.All) ? "all" : "");
                }
                builder.Append(this.RenderSelect(item.Query));
            }
            return builder.ToString();
        }

        public virtual string RenderUpdate(UpdateQuery query)
        {
            return this.UpdateStatement(query);
        }

        protected virtual void Select(StringBuilder builder, bool distinct)
        {
            builder.Append("select ");
            if (distinct)
            {
                builder.Append("distinct ");
            }
        }

        protected virtual void SelectColumn(StringBuilder builder, SkyMap.Net.SqlOM.SelectColumn col)
        {
            this.Expression(builder, col.Expression);
            if (col.ColumnAlias != null)
            {
                builder.Append(" ");
                this.Identifier(builder, col.ColumnAlias);
            }
        }

        protected virtual void SelectColumns(StringBuilder builder, SelectColumnCollection columns)
        {
            foreach (SkyMap.Net.SqlOM.SelectColumn column in columns)
            {
                if (column != columns[0])
                {
                    this.Coma(builder);
                }
                this.SelectColumn(builder, column);
            }
        }

        public virtual string SqlEncode(string val)
        {
            return val.Replace("'", "''");
        }

        protected virtual void TableNamespace(StringBuilder builder, string ns)
        {
            builder.AppendFormat("{0}.", ns);
        }

        protected virtual void Update(StringBuilder builder, string tableName)
        {
            builder.Append("update ");
            this.Identifier(builder, tableName);
            builder.Append(" set ");
        }

        public virtual string UpdateStatement(UpdateQuery query)
        {
            query.Validate();
            StringBuilder builder = new StringBuilder();
            this.Update(builder, query.TableName);
            this.UpdateTerms(builder, query.Terms);
            this.Where(builder, query.WhereClause);
            this.WhereClause(builder, query.WhereClause);
            return builder.ToString();
        }

        protected virtual void UpdateTerm(StringBuilder builder, SkyMap.Net.SqlOM.UpdateTerm term)
        {
            this.Identifier(builder, term.FieldName);
            builder.Append(" = ");
            this.Expression(builder, term.Value);
        }

        protected virtual void UpdateTerms(StringBuilder builder, UpdateTermCollection terms)
        {
            foreach (SkyMap.Net.SqlOM.UpdateTerm term in terms)
            {
                if (terms[0] != term)
                {
                    this.Coma(builder);
                }
                this.UpdateTerm(builder, term);
            }
        }

        protected virtual void Where(StringBuilder builder, SkyMap.Net.SqlOM.WhereClause group)
        {
            if (!group.IsEmpty)
            {
                builder.AppendFormat(" where ", new object[0]);
            }
        }

        protected virtual void WhereClause(StringBuilder builder, SkyMap.Net.SqlOM.WhereClause group)
        {
            if (!group.IsEmpty)
            {
                builder.AppendFormat("(", new object[0]);
                for (int i = 0; i < group.Terms.Count; i++)
                {
                    if (i > 0)
                    {
                        this.RelationshipOperator(builder, group.Relationship);
                    }
                    WhereTerm term = group.Terms[i];
                    this.WhereClause(builder, term);
                }
                bool flag = group.Terms.Count > 0;
                foreach (SkyMap.Net.SqlOM.WhereClause clause in group.SubClauses)
                {
                    if (!clause.IsEmpty)
                    {
                        if (flag)
                        {
                            this.RelationshipOperator(builder, group.Relationship);
                        }
                        this.WhereClause(builder, clause);
                        flag = true;
                    }
                }
                builder.AppendFormat(")", new object[0]);
            }
        }

        protected virtual void WhereClause(StringBuilder builder, WhereTerm term)
        {
            if ((term.Type == WhereTermType.Compare) && (term.Op == CompareOperator.BitwiseAnd))
            {
                this.BitwiseAnd(builder, term);
            }
            else if (term.Type == WhereTermType.Compare)
            {
                this.Expression(builder, term.Expr1);
                builder.Append(" ");
                this.Operator(builder, term.Op);
                builder.Append(" ");
                this.Expression(builder, term.Expr2);
            }
            else if ((((term.Type == WhereTermType.In) || (term.Type == WhereTermType.NotIn)) || (term.Type == WhereTermType.InSubQuery)) || (term.Type == WhereTermType.NotInSubQuery))
            {
                this.Expression(builder, term.Expr1);
                if ((term.Type == WhereTermType.NotIn) || (term.Type == WhereTermType.NotInSubQuery))
                {
                    builder.Append(" not");
                }
                builder.Append(" in (");
                if ((term.Type == WhereTermType.InSubQuery) || (term.Type == WhereTermType.NotInSubQuery))
                {
                    builder.Append(term.SubQuery);
                }
                else
                {
                    this.ConstantList(builder, term.Values);
                }
                builder.Append(")");
            }
            else if ((term.Type == WhereTermType.Exists) || (term.Type == WhereTermType.NotExists))
            {
                if (term.Type == WhereTermType.NotExists)
                {
                    builder.Append(" not");
                }
                builder.Append(" exists (");
                builder.Append(term.SubQuery);
                builder.Append(")");
            }
            else if (term.Type == WhereTermType.Between)
            {
                this.Expression(builder, term.Expr1);
                builder.AppendFormat(" between ", new object[0]);
                this.Expression(builder, term.Expr2);
                builder.AppendFormat(" and ", new object[0]);
                this.Expression(builder, term.Expr3);
            }
            else if ((term.Type == WhereTermType.IsNull) || (term.Type == WhereTermType.IsNotNull))
            {
                this.Expression(builder, term.Expr1);
                builder.Append(" is ");
                if (term.Type == WhereTermType.IsNotNull)
                {
                    builder.Append("not ");
                }
                builder.Append(" null ");
            }
        }

        public string DateFormat
        {
            get
            {
                return this.dateFormat;
            }
            set
            {
                this.dateFormat = value;
            }
        }

        public string DateTimeFormat
        {
            get
            {
                return this.dateTimeFormat;
            }
            set
            {
                this.dateTimeFormat = value;
            }
        }

        protected virtual bool UpperCaseIdentifiers
        {
            get
            {
                return false;
            }
        }
    }
}

