namespace SkyMap.Net.Security
{
    using SkyMap.Net.DAO;
    using SkyMap.Net.OGM;
    using System;

    public interface IAuthSetting
    {
        void PostEditor();

        CAuthType AuthType { get; set; }

        UnitOfWork CurrentUnitOfWork { get; set; }
    }
}

