namespace SkyMap.Net.SqlOM
{
    using System;

    internal class SqlUnionItem
    {
        public SelectQuery Query;
        public DistinctModifier RepeatingAction;

        public SqlUnionItem(SelectQuery query, DistinctModifier repeatingAction)
        {
            this.Query = query;
            this.RepeatingAction = repeatingAction;
        }
    }
}

