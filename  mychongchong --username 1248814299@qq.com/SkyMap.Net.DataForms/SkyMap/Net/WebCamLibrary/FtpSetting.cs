namespace SkyMap.Net.WebCamLibrary
{
    using SkyMap.Net.Configuration;
    using SkyMap.Net.Core;
    using System;
    using System.IO;

    public class FtpSetting
    {
        public string Host;
        public string Password;
        public string Path;
        public string User;

        public static FtpSetting Get(string ftpvideocfgfile)
        {
            FtpSetting setting = new FtpSetting();
            string cfgCollectionName = "ftpsettting";
            setting.User = NiniConfigHelper.GetValueByKeyFromXML(ftpvideocfgfile, cfgCollectionName, "user", "LDM");
            setting.Password = NiniConfigHelper.GetValueByKeyFromXML(ftpvideocfgfile, cfgCollectionName, "password", "123");
            setting.Host = NiniConfigHelper.GetValueByKeyFromXML(ftpvideocfgfile, cfgCollectionName, "host", "192.168.119.222");
            setting.Path = NiniConfigHelper.GetValueByKeyFromXML(ftpvideocfgfile, cfgCollectionName, "path", "CameraProject");
            return setting;
        }

        public static FtpSetting CameraInstance
        {
            get
            {
                return Get(System.IO.Path.Combine(PropertyService.ConfigDirectory, "ftpCamera.config"));
            }
        }
    }
}

