namespace SkyMap.Net.DAO
{
    using Nini.Config;
    using SkyMap.Net.Core;
    using System;
    using System.Collections.Generic;
    using System.IO;

    internal sealed class SupportClass
    {
        private static XmlConfigSource config;
        private static List<string> daoCacheTypes;
        private const string daoCfgFile = "DAOConfigName";
        private static Dictionary<string, string> daoConfigFiles = new Dictionary<string, string>(1);
        internal static string DAOConfigsPath = PropertyService.ConfigDirectory;
        private const string daoDefaultFile = "Default";
        private const string source = "DAO.config";

        static SupportClass()
        {
            string path = Path.Combine(DAOConfigsPath, "DAO.config");
            LoggingService.DebugFormatted("查找DAO配置文件：{0}", new object[] { path });
            if (File.Exists(path))
            {
                config = new XmlConfigSource(path);
            }
            else
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
                LoggingService.DebugFormatted("查找DAO配置文件：{0}", new object[] { path });
                if (File.Exists(path))
                {
                    config = new XmlConfigSource(path);
                }
            }
            if (config == null)
            {
                LoggingService.Warn("没有找到DAO配置文件...");
            }
        }

        internal static string GetDAOConfigFile(string nameSpace)
        {
            string str = "Default";
            if (!string.IsNullOrEmpty(nameSpace))
            {
                lock (daoConfigFiles)
                {
                    if (!daoConfigFiles.ContainsKey(nameSpace))
                    {
                        if (config != null)
                        {
                            foreach (string str2 in config.Configs["DAOConfigName"].GetKeys())
                            {
                                if (LoggingService.IsDebugEnabled)
                                {
                                    LoggingService.DebugFormatted("Key:'{0}';Namespace:'{1}'", new object[] { str2, nameSpace });
                                }
                                if ((str2 == nameSpace) || nameSpace.StartsWith(str2 + "."))
                                {
                                    str = config.Configs["DAOConfigName"].GetString(str2);
                                    break;
                                }
                            }
                        }
                        daoConfigFiles.Add(nameSpace, str);
                    }
                    else
                    {
                        str = daoConfigFiles[nameSpace];
                    }
                }
            }
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("将获取命名空间'{0}'配置文件为'{1}.cfg'", new object[] { nameSpace, str });
            }
            return str;
        }

        internal static List<string> DAOCacheTypes
        {
            get
            {
                if (daoCacheTypes == null)
                {
                    daoCacheTypes = new List<string>();
                    if ((config != null) && (config.Configs["DAOCacheTypes"] != null))
                    {
                        foreach (string str in config.Configs["DAOCacheTypes"].GetKeys())
                        {
                            daoCacheTypes.Add(config.Configs["DAOCacheTypes"].GetString(str));
                        }
                    }
                }
                return daoCacheTypes;
            }
        }
    }
}

