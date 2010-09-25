namespace SkyMap.Net.SqlOM
{
    using System;

    public class InsertQuery
    {
        private string tableName;
        private UpdateTermCollection terms;

        public InsertQuery() : this(null)
        {
        }

        public InsertQuery(string tableName)
        {
            this.terms = new UpdateTermCollection();
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
    }
}

