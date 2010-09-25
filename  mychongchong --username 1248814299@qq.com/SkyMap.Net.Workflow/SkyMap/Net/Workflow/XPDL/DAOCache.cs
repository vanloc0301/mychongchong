namespace SkyMap.Net.Workflow.XPDL
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using SkyMap.Net.Workflow.XPDL.ExtendElement;

    public class DAOCache : AbstractDAOCache
    {
        public override void Put()
        {
            List<Prodef> all_prodefs = new List<Prodef>();
            Action<IEnumerable<Package>> action = delegate (IEnumerable<Package> pgs) {
                foreach (Package package in pgs)
                {
                    all_prodefs.AddRange(package.Prodefs.Values);
                }
            };
            base.PutTypeToCache<Package>(action);
            base.PutTypeAllObjectsToCache<Prodef>(all_prodefs);
            base.PutTypeToCache<WfTempletAppendixs>(null);
            if (!DAOCacheService.Contains("WF_HOLIDAYS"))
            {
                DataView holiDays = DateTimeHelper.HoliDays;
            }
            base.PutTypeToCache<SkyMap.Net.Workflow.XPDL.Condition>(null);
        }
    }
}

