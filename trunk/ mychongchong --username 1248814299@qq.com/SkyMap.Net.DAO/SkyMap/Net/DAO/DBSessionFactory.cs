namespace SkyMap.Net.DAO
{
    using SkyMap.Net.Core;
    using System;

    public static class DBSessionFactory
    {
        public static IDBSession GetInstanceByUrl(string url)
        {
            HibernateImpl impl = null;
            try
            {
                if (LoggingService.IsDebugEnabled)
                {
                    LoggingService.Debug("将获取HibernateImpl对象实例，该对象无状态，可以使用WELLKNOW方式激活");
                }
                impl = RemoteUtil.CreateClientWellKnowObject<HibernateImpl>(url);
            }
            catch (Exception exception)
            {
                LoggingService.Error("Cannot create HibernateImpl : ", exception);
                throw exception;
            }
            return impl;
        }

        public static IDBSession Instance
        {
            get
            {
                HibernateImpl impl = null;
                try
                {
                    if (LoggingService.IsDebugEnabled)
                    {
                        LoggingService.Debug("将获取HibernateImpl对象实例，该对象无状态，可以使用WELLKNOW方式激活");
                    }
                    impl = RemoteUtil.CreateClientWellKnowObject<HibernateImpl>(string.Empty);
                }
                catch (Exception exception)
                {
                    LoggingService.Error("Cannot create HibernateImpl : ", exception);
                    throw exception;
                }
                return impl;
            }
        }
    }
}

