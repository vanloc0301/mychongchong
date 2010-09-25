namespace SkyMap.Net.SqlOM
{
    using System;

    public class WhereClause : ICloneable
    {
        private WhereClauseCollection clauses;
        private WhereClauseRelationship relationship;
        private WhereTermCollection whereTerms;

        internal WhereClause()
        {
            this.relationship = WhereClauseRelationship.And;
            this.whereTerms = new WhereTermCollection();
            this.clauses = new WhereClauseCollection();
        }

        public WhereClause(WhereClauseRelationship relationship)
        {
            this.relationship = WhereClauseRelationship.And;
            this.whereTerms = new WhereTermCollection();
            this.clauses = new WhereClauseCollection();
            this.relationship = relationship;
        }

        public WhereClause Clone()
        {
            WhereClause clause = new WhereClause();
            clause.relationship = this.relationship;
            clause.whereTerms = new WhereTermCollection(this.whereTerms);
            foreach (WhereClause clause2 in this.clauses)
            {
                clause.clauses.Add(clause2.Clone());
            }
            return clause;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public bool IsEmpty
        {
            get
            {
                foreach (WhereClause clause in this.clauses)
                {
                    if (!clause.IsEmpty)
                    {
                        return false;
                    }
                }
                return (this.whereTerms.Count == 0);
            }
        }

        public WhereClauseRelationship Relationship
        {
            get
            {
                return this.relationship;
            }
        }

        public WhereClauseCollection SubClauses
        {
            get
            {
                return this.clauses;
            }
        }

        public WhereTermCollection Terms
        {
            get
            {
                return this.whereTerms;
            }
        }
    }
}

