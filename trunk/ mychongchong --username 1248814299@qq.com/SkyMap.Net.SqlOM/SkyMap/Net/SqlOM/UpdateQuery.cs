namespace SkyMap.Net.SqlOM
{
    using System;

    public class UpdateQuery
    {
        private string tableName;
        private UpdateTermCollection terms;
        private SkyMap.Net.SqlOM.WhereClause whereClause;

        public UpdateQuery() : this(null)
        {
        }

        public UpdateQuery(string tableName)
        {
            this.terms = new UpdateTermCollection();
            this.whereClause = new SkyMap.Net.SqlOM.WhereClause(WhereClauseRelationship.And);
            this.tableName = tableName;
        }

        public void Validate()
        {
            if (this.tableName == null)
            {
                throw new InvalidQueryException("TableName is empty.");
            }
            if (this.terms.Count == 0)
            {
                throw new InvalidQueryException("Terms collection is empty.");
            }
        }

        public string TableName
        {
            get
            {
                return this.tableName;
            }
            set
            {
                this.tableName = value;
            }
        }

        public UpdateTermCollection Terms
        {
            get
            {
                return this.terms;
            }
        }

        public SkyMap.Net.SqlOM.WhereClause WhereClause
        {
            get
            {
                return this.whereClause;
            }
        }
    }
}

