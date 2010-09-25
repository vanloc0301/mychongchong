namespace SkyMap.Net.Core
{
    using Nini.Config;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Xml;
    using System.Xml.XPath;

    public sealed class AutoUpdateHepler
    {
        private static string autoupdate;
        private static string liveup;
        private static string password;
        private static string path;
        private static string server;
        private static string softName;
        private static string user;
        private static string version;

        static AutoUpdateHepler()
        {
            string str = FileUtility.ApplicationRootPath + @"\update";
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.Info("读取升级配置:" + str);
            }
            XmlConfigSource source = null;
            if (System.IO.File.Exists(str + ".cxml"))
            {
                XmlTextReader decryptXmlReader = new CryptoHelper(CryptoTypes.encTypeDES).GetDecryptXmlReader(str + ".cxml");
                IXPathNavigable document = new XPathDocument(decryptXmlReader);
                source = new XmlConfigSource(document);
                decryptXmlReader.Close();
            }
            else if (System.IO.File.Exists(str + ".xml"))
            {
                source = new XmlConfigSource(str + ".xml");
            }
            if (source != null)
            {
                softName = source.Configs["FtpSetting"].GetString("SoftName", string.Empty);
                version = source.Configs["FtpSetting"].GetString("Version", string.Empty);
                server = source.Configs["FtpSetting"].GetString("Server", string.Empty);
                user = source.Configs["FtpSetting"].GetString("User", string.Empty);
                password = source.Configs["FtpSetting"].GetString("Password", string.Empty);
                path = source.Configs["FtpSetting"].GetString("Path", string.Empty);
                liveup = source.Configs["FtpSetting"].GetString("LiveUp", string.Empty);
                autoupdate = source.Configs["FtpSetting"].GetString("autoupdate", string.Empty);
            }
        }

        public static string GetLastVersion()
        {
            return version;
        }

        public static bool HasNewVesion()
        {
            try
            {
                if (!string.IsNullOrEmpty(server))
                {
                    WebRequest request = WebRequest.Create(string.Format("ftp://{0}/{1}/update.xml", server, softName));
                    request.Credentials = new NetworkCredential(user, password);
                    WebResponse response = request.GetResponse();
                    XmlDocument document = new XmlDocument();
                    document.Load(response.GetResponseStream());
                    XmlConfigSource source = new XmlConfigSource(document);
                    if (source.Configs["FtpSetting"].GetString("Version").CompareTo(version) > 0)
                    {
                        LoggingService.InfoFormatted("系统有新版本：{0}", new object[] { source.Configs["FtpSetting"].GetString("Version") });
                        return true;
                    }
                }
            }
            catch (Exception exception)
            {
                LoggingService.Error("查询是否有新版本时出错", exception);
            }
            return false;
        }

        public static bool TryUpdate()
        {
            if (softName == null)
            {
                return false;
            }
            return TryUpdate(softName, version, server, user, password, path);
        }

        public static bool TryUpdate(string minVersion)
        {
            if (softName == null)
            {
                return false;
            }
            return TryUpdate(softName, minVersion, server, user, password, path);
        }

        private static bool TryUpdate(string softName, string version, string server, string user, string password, string path)
        {
            string str;
            if (!string.IsNullOrEmpty(liveup))
            {
                str = FileUtility.ApplicationRootPath + @"\" + liveup;
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.Info("准备升级从:" + str);
                }
                if (System.IO.File.Exists(str))
                {
                    string arguments = softName + " " + version + " " + server + " " + user + " " + password + " " + path;
                    Process.Start(str, arguments);
                }
            }
            if (!string.IsNullOrEmpty(autoupdate))
            {
                str = FileUtility.ApplicationRootPath + @"\" + autoupdate;
                if (System.IO.File.Exists(str))
                {
                    LoggingService.InfoFormatted("准备升级从:{0}", new object[] { str });
                    if (version == "0.00.01")
                    {
                        Process.Start(str, "true");
                    }
                    else
                    {
                        Process.Start(str);
                    }
                }
            }
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.Debug("已尝试升级...");
            }
            return true;
        }
    }
}

