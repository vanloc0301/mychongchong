namespace SkyMap.Net.SqlOM
{
    using System;

    public class SelectQuery : ICloneable
    {
        private SelectColumnCollection columns = new SelectColumnCollection();
        private bool distinct = false;
        private SkyMap.Net.SqlOM.FromClause fromClause = new SkyMap.Net.SqlOM.FromClause();
        private GroupByTermCollection groupByTerms = new GroupByTermCollection();
        private bool groupByWithCube = false;
        private bool groupByWithRollup = false;
        private WhereClause havingPhrase = new WhereClause();
        private OrderByTermCollection orderByTerms = new OrderByTermCollection();
        private string tableSpace = null;
        private int top = -1;
        private WhereClause wherePhrase = new WhereClause();

        public SelectQuery Clone()
        {
            SelectQuery query = new SelectQuery();
            query.columns = new SelectColumnCollection(this.columns);
            query.orderByTerms = new OrderByTermCollection(this.orderByTerms);
            query.groupByTerms = new GroupByTermCollection(this.groupByTerms);
            query.wherePhrase = this.wherePhrase.Clone();
            query.fromClause = this.fromClause.Clone();
            query.top = this.top;
            query.groupByWithRollup = this.groupByWithRollup;
            query.groupByWithCube = this.groupByWithCube;
            query.distinct = this.distinct;
            query.tableSpace = this.tableSpace;
            return query;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public void Validate()
        {
            if (this.columns.Count == 0)
            {
                throw new InvalidQueryException("A select query must have at least one column");
            }
            if (this.fromClause.BaseTable == null)
            {
                throw new InvalidQueryException("A select query must have FromPhrase.BaseTable set");
            }
        }

        public SelectColumnCollection Columns
        {
            get
            {
                return this.columns;
            }
        }

        public bool Distinct
        {
            get
            {
                return this.distinct;
            }
            set
            {
                this.distinct = value;
            }
        }

        public SkyMap.Net.SqlOM.FromClause FromClause
        {
            get
            {
                return this.fromClause;
            }
        }

        public GroupByTermCollection GroupByTerms
        {
            get
            {
                return this.groupByTerms;
            }
        }

        public bool GroupByWithCube
        {
            get
            {
                return this.groupByWithCube;
            }
            set
            {
                this.groupByWithCube = value;
            }
        }

        public bool GroupByWithRollup
        {
            get
            {
                return this.groupByWithRollup;
            }
            set
            {
                this.groupByWithRollup = value;
            }
        }

        public WhereClause HavingPhrase
        {
            get
            {
                return this.havingPhrase;
            }
        }

        public OrderByTermCollection OrderByTerms
        {
            get
            {
                return this.orderByTerms;
            }
        }

        public string TableSpace
        {
            get
            {
                return this.tableSpace;
            }
            set
            {
                this.tableSpace = value;
            }
        }

        public int Top
        {
            get
            {
                return this.top;
            }
            set
            {
                this.top = value;
            }
        }

        public WhereClause WherePhrase
        {
            get
            {
                return this.wherePhrase;
            }
        }
    }
}

