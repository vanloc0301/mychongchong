namespace SkyMap.Net.DAO
{
    using System;

    public class DAOCache : AbstractDAOCache
    {
        public override void Put()
        {
            base.PutTypeToCache<SMDataSource>(null);
            base.PutTypeToCache<TaskNode>(null);
            base.PutTypeToCache<DataWordType>(null);
        }
    }
}

