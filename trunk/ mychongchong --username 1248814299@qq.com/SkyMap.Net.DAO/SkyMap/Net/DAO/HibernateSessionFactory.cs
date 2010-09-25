namespace SkyMap.Net.DAO
{
    using NHibernate;
    using NHibernate.Cfg;
    using NHibernate.Engine;
    using NHibernate.Impl;
    using SkyMap.Net.Core;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Xml;

    internal class HibernateSessionFactory
    {
        private const string csuffix = ".cfg.cxml";
        private static Dictionary<string, string> CustomDefineNameQuerySQLs = new Dictionary<string, string>();
        private static Dictionary<string, ISessionFactory> sessionFactorys = new Dictionary<string, ISessionFactory>();
        private const string suffix = ".cfg.xml";

        private static Configuration DeSerializeBinaryConfiguration(string assemblyName)
        {
            try
            {
                string path = Path.Combine(SupportClass.DAOConfigsPath, assemblyName + ".bin");
                if (File.Exists(path))
                {
                    LoggingService.InfoFormatted("将从文件：{0} 获取序列化的ISessionFactory", new object[] { path });
                    using (FileStream stream = File.OpenRead(path))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        return (Configuration) formatter.Deserialize(stream);
                    }
                }
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
            }
            return null;
        }

        internal static IDbConnection GetConnection(string assemblyName)
        {
            return ((ISessionFactoryImplementor) GetSessionFactory(assemblyName)).ConnectionProvider.GetConnection();
        }

        internal static string GetCustomDefineNameQuerySQL(string assemblyName, string queryName)
        {
            string key = assemblyName + "-" + queryName;
            lock (CustomDefineNameQuerySQLs)
            {
                if (!CustomDefineNameQuerySQLs.ContainsKey(key))
                {
                    NamedQueryDefinition namedQuery = ((ISessionFactoryImplementor) GetSessionFactory(assemblyName)).GetNamedQuery(queryName);
                    if (namedQuery == null)
                    {
                        throw new ApplicationException(string.Format("没有找到名称为'{0}'的SQL查询...", queryName));
                    }
                    string queryString = namedQuery.QueryString;
                    CustomDefineNameQuerySQLs.Add(key, queryString);
                    return queryString;
                }
                return CustomDefineNameQuerySQLs[key];
            }
        }

        internal static ISession GetSession(string AssemblyName)
        {
            ISession session = GetSessionFactory(AssemblyName).OpenSession();
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("获取了配置文件为'{0}'.cfg.xml的NHibernateSession, ID:{1}", new object[] { AssemblyName, ((AbstractSessionImpl) session).SessionId });
            }
            return session;
        }

        private static ISessionFactory GetSessionFactory(string AssemblyName)
        {
            lock (sessionFactorys)
            {
                if (!sessionFactorys.ContainsKey(AssemblyName))
                {
                    Configuration cfg = DeSerializeBinaryConfiguration(AssemblyName);
                    if (cfg == null)
                    {
                        using (XmlTextReader reader = GetXmlTextReader(AssemblyName))
                        {
                            if (reader == null)
                            {
                                throw new ApplicationException("Config file is null");
                            }
                            cfg = new Configuration();
                            cfg.Configure(reader);
                            SerializeBinaryConfiguration(cfg, AssemblyName);
                        }
                    }
                    ISessionFactory factory = cfg.BuildSessionFactory();
                    sessionFactorys.Add(AssemblyName, factory);
                    return factory;
                }
                return sessionFactorys[AssemblyName];
            }
        }

        private static XmlTextReader GetXmlTextReader(string AssemblyName)
        {
            XmlTextReader xmlTextReader = GetXmlTextReader(SupportClass.DAOConfigsPath, AssemblyName);
            if (xmlTextReader == null)
            {
                xmlTextReader = GetXmlTextReader(AppDomain.CurrentDomain.BaseDirectory, AssemblyName);
            }
            if (xmlTextReader == null)
            {
                LoggingService.WarnFormatted("找不到{0}.cfg.xml的配置文件", new object[] { AssemblyName });
                throw new ArgumentException(string.Format("找不到{0}.cfg.xml的配置文件", AssemblyName));
            }
            return xmlTextReader;
        }

        private static XmlTextReader GetXmlTextReader(string path, string AssemblyName)
        {
            XmlTextReader reader = null;
            string str = Path.Combine(path, AssemblyName + ".cfg.cxml");
            if (File.Exists(str))
            {
                CryptoHelper helper = new CryptoHelper(CryptoTypes.encTypeDES);
                return helper.GetDecryptXmlReader(str);
            }
            str = Path.Combine(path, AssemblyName + ".cfg.xml");
            if (File.Exists(str))
            {
                reader = new XmlTextReader(str);
            }
            return reader;
        }

        private static void SerializeBinaryConfiguration(Configuration cfg, string assemblyName)
        {
            try
            {
                string path = Path.Combine(SupportClass.DAOConfigsPath, assemblyName + ".bin");
                LoggingService.InfoFormatted("将序列化Configuration到文件：{0}", new object[] { path });
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream stream = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    formatter.Serialize(stream, cfg);
                    stream.Flush();
                }
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
            }
        }
    }
}

