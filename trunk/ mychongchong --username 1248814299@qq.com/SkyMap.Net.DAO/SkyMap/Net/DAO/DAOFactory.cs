namespace SkyMap.Net.DAO
{
    using SkyMap.Net.Core;
    using System;

    public static class DAOFactory
    {
        internal static IDA0 GetInstance(string configFileName)
        {
            HibernateUtil util = null;
            try
            {
                if (LoggingService.IsDebugEnabled)
                {
                    LoggingService.DebugFormatted("将获得配置文件为'{0}.cfg.xml'DAO对象", new object[] { configFileName });
                }
                util = new HibernateUtil(configFileName);
            }
            catch (Exception exception)
            {
                LoggingService.Error("Cannot create dao : " + configFileName + "\r\n", exception);
                throw exception;
            }
            return util;
        }

        public static IDA0 GetInstanceByNameSpace(string nameSpace)
        {
            return GetInstance(SupportClass.GetDAOConfigFile(nameSpace));
        }
    }
}

