namespace SkyMap.Net.Criteria
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using System;

    public class DAOCache : AbstractDAOCache
    {
        public override void Put()
        {
            base.PutTypeToCache<TySearchx>(null);
            try
            {
                base.PutTypeToCache<TyQuery>(new string[] { "DisplayOrder" }, null);
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
            }
        }
    }
}

