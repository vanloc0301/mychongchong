namespace SkyMap.Net.Security
{
    using SkyMap.Net.OGM;
    using SkyMap.Net.Security.Principal;
    using System;
    using System.Collections.Generic;

    public class DefaultQueryAuthResource : IQueryAuthResource
    {
        public bool IsCurrentPricipalAuthResource(CAuthType authType, string resourceId)
        {
            return this.QueryCurrentPricipalAuthResources<string>(authType).Contains(resourceId);
        }

        public IList<T> QueryCurrentPricipalAuthResources<T>(CAuthType authType)
        {
            SmPrincipal smPrincipal = SecurityUtil.GetSmPrincipal();
            List<string> list = new List<string>();
            List<string> list2 = new List<string>(smPrincipal.Participants);
            foreach (CUniversalAuth auth in authType.UniversalAuths)
            {
                if (list2.Contains(auth.ParticipantId))
                {
                    list.Add(auth.ResourceId);
                }
            }
            return (list as IList<T>);
        }
    }
}

