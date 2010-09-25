namespace SkyMap.Net.Security
{
    using SkyMap.Net.OGM;
    using System;
    using System.Collections.Generic;

    public interface IQueryAuthResource
    {
        bool IsCurrentPricipalAuthResource(CAuthType authType, string resourceId);
        IList<T> QueryCurrentPricipalAuthResources<T>(CAuthType authType);
    }
}

