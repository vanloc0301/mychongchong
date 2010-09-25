namespace SkyMap.Net.SqlOM
{
    using System;

    public class DeleteQuery
    {
        private string tableName;
        private SkyMap.Net.SqlOM.WhereClause whereClause;

        public DeleteQuery() : this(null)
        {
        }

        public DeleteQuery(string tableName)
        {
            this.whereClause = new SkyMap.Net.SqlOM.WhereClause(WhereClauseRelationship.And);
            this.tableName = tableName;
        }

        public void Validate()
        {
            if (this.tableName == null)
            {
                throw new InvalidQueryException("TableName is empty.");
            }
            if (this.WhereClause.IsEmpty)
            {
                throw new InvalidQueryException("DeleteQuery has no where condition.");
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

        public SkyMap.Net.SqlOM.WhereClause WhereClause
        {
            get
            {
                return this.whereClause;
            }
        }
    }
}

