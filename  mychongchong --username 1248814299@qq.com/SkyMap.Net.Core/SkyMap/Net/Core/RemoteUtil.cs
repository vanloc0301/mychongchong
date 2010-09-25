namespace SkyMap.Net.Core
{
    using System;
    using System.Runtime.Remoting.Activation;

    public static class RemoteUtil
    {
        public static T CreateClientActivatedObject<T>(object[] args) where T: MarshalByRefObject
        {
            T local = (T) Activator.CreateInstance(typeof(T), args);
            if (local == null)
            {
                string message = string.Format("不能创建Client－Activated远程对象:{0}", new object[0]);
                LoggingService.Error(message);
                throw new CoreException(message);
            }
            return local;
        }

        public static T CreateClientWellKnowObject<T>(string url) where T: MarshalByRefObject, new()
        {
            if (!string.IsNullOrEmpty(url))
            {
                object[] objArray = new object[] { new UrlAttribute(url) };
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.InfoFormatted("将创建Client－Activated远程对象:{0}", new object[] { url });
                }
                object obj2 = Activator.GetObject(typeof(T), url);
                if (obj2 == null)
                {
                    string message = string.Format("不能创建Client－Activated远程对象:{0}", url);
                    LoggingService.Error(message);
                    throw new CoreException(message);
                }
                return (T) obj2;
            }
            return Activator.CreateInstance<T>();
        }
    }
}

