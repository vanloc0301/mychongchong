namespace SkyMap.Net.SqlOM
{
    using System;
    using System.Collections;

    public class SqlUnion
    {
        private ArrayList items = new ArrayList(5);

        public void Add(SelectQuery query)
        {
            this.Add(query, DistinctModifier.Distinct);
        }

        public void Add(SelectQuery query, DistinctModifier repeatingAction)
        {
            this.items.Add(new SqlUnionItem(query, repeatingAction));
        }

        internal IList Items
        {
            get
            {
                return this.items;
            }
        }
    }
}

