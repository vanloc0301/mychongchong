namespace SkyMap.Net.Core
{
    using SkyMap.Net.Configuration;
    using System;

    public class FtpSetting
    {
        public string Host;
        public string Password;
        public string Path;
        public string User;

        public static FtpSetting Get(string ftpcfgfile)
        {
            FtpSetting setting = new FtpSetting();
            string cfgCollectionName = "ftpsettting";
            setting.User = NiniConfigHelper.GetValueByKeyFromXML(ftpcfgfile, cfgCollectionName, "user", "LDM");
            setting.Password = NiniConfigHelper.GetValueByKeyFromXML(ftpcfgfile, cfgCollectionName, "password", "123");
            setting.Host = NiniConfigHelper.GetValueByKeyFromXML(ftpcfgfile, cfgCollectionName, "host", "192.168.119.222");
            setting.Path = NiniConfigHelper.GetValueByKeyFromXML(ftpcfgfile, cfgCollectionName, "path", "CameraProject");
            return setting;
        }
    }
}

