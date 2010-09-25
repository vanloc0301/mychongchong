namespace SkyMap.Net.DAO
{
    using SkyMap.Net.Core;
    using System;

    public class AutoLoadDAOCacheCommand : AbstractCommand
    {
        public override void Run()
        {
            DAOCacheService.LoadCaches();
        }
    }
}

