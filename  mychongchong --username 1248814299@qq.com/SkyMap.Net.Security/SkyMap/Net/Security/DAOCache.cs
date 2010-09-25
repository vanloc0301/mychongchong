namespace SkyMap.Net.Security
{
    using SkyMap.Net.DAO;
    using System;
    using SkyMap.Net.OGM;

    public class DAOCache : AbstractDAOCache
    {
        public override void Put()
        {
            OGMService.InitOGMData();
            base.PutTypeToCache<CParticipant>(null);
            base.PutTypeToCache<CAuthType>(null);
        }
    }
}

