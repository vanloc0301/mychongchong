namespace SkyMap.Net.Configuration
{
    using Nini.Config;
    using SkyMap.Net.Core;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using System.Xml.XPath;

    public static class NiniConfigHelper
    {
        private static Dictionary<string, XmlConfigSource> configs = new Dictionary<string, XmlConfigSource>();

        private static XmlConfigSource GetConfig(string cfgFileName)
        {
            XmlConfigSource source = null;
            if (configs.ContainsKey(cfgFileName))
            {
                return configs[cfgFileName];
            }
            string filename = cfgFileName;
            int num = filename.LastIndexOf('.');
            if (filename.EndsWith(".config") && File.Exists(filename.Insert(num + 1, "c")))
            {
                filename = filename.Insert(num + 1, "c");
                LoggingService.DebugFormatted("读取的是加密的配置文件：{0}", new object[] { filename });
                CryptoHelper helper = new CryptoHelper(CryptoTypes.encTypeDES);
                using (XmlTextReader reader = helper.GetDecryptXmlReader(filename))
                {
                    XPathDocument document = new XPathDocument(reader);
                    source = new XmlConfigSource(document);
                    goto Label_00B8;
                }
            }
            if (!File.Exists(filename))
            {
                throw new ArgumentOutOfRangeException(cfgFileName + "不存在");
            }
            source = new XmlConfigSource(filename);
        Label_00B8:
            configs.Add(cfgFileName, source);
            return source;
        }

        public static string GetValueByKeyFromXML(string cfgFileName, string cfgCollectionName, string key, string defaultValue)
        {
            lock (configs)
            {
                return GetConfig(cfgFileName).Configs[cfgCollectionName].GetString(key, defaultValue);
            }
        }

        public static string GetValueByMatchKeyFromXML(string cfgFileName, string cfgCollectionName, string matchkey, string defaultValue)
        {
            if (!File.Exists(cfgFileName))
            {
                throw new ArgumentOutOfRangeException(cfgFileName + "不存在");
            }
            XmlConfigSource config = GetConfig(cfgFileName);
            foreach (string str in config.Configs[cfgCollectionName].GetKeys())
            {
                if (matchkey.StartsWith(str))
                {
                    return config.Configs[cfgCollectionName].GetString(str, defaultValue);
                }
            }
            return string.Empty;
        }
    }
}

