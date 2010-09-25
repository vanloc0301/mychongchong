namespace SkyMap.Net.DAO
{
    using SkyMap.Net.Core;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;

    public static class DAOCacheService
    {
        private static BackgroundWorker bgWork;
        public static RunWorkerCompletedEventHandler CacheLoaded;
        internal const string DAOCACHE_VERSION = "DAOCACHE_VERSION";
        internal static bool preLoadCaches = false;
        private static Stopwatch stopWatch;

        static DAOCacheService()
        {
            Check();
        }

        private static void bgWork_DoWork(object sender, DoWorkEventArgs e)
        {
            LoggingService.InfoFormatted("开始异步载入数据库缓存...", new object[0]);
            List<string> dAOCacheTypes = SupportClass.DAOCacheTypes;
            foreach (string str in dAOCacheTypes)
            {
                Type type = Type.GetType(str, false);
                if (type != null)
                {
                    IDAOCache cache = (IDAOCache) Activator.CreateInstance(type);
                    if (cache != null)
                    {
                        cache.Put();
                    }
                }
            }
        }

        private static void bgWork_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            LoggingService.Info("后台获取缓存数据完成...");
            if (e.Error != null)
            {
                LoggingService.Error("后台获取缓存数据时出错...", e.Error);
            }
            else
            {
                CacheService.TakeSnapshot();
            }
            if (CacheLoaded != null)
            {
                CacheLoaded(sender, e);
            }
        }

        private static void Check()
        {
            if (stopWatch == null)
            {
                stopWatch = new Stopwatch();
                stopWatch.Start();
            }
            else
            {
                stopWatch.Stop();
                long elapsedMilliseconds = stopWatch.ElapsedMilliseconds;
                if (LoggingService.IsDebugEnabled)
                {
                    LoggingService.DebugFormatted("前后两次调用该函数时间相隔为:{0}ms", new object[] { elapsedMilliseconds });
                }
                if (elapsedMilliseconds < 0x2710L)
                {
                    stopWatch.Start();
                    return;
                }
                stopWatch.Reset();
                stopWatch.Start();
            }
            int num2 = -1;
            if (CacheService.Contains("DAOCACHE_VERSION"))
            {
                num2 = (int) CacheService.Get("DAOCACHE_VERSION");
            }
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("local_version:{0}", new object[] { num2 });
            }
            int dAOCacheVersion = DBSessionFactory.Instance.DAOCacheVersion;
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("new_version:{0}", new object[] { dAOCacheVersion });
            }
            if (dAOCacheVersion > num2)
            {
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.Info("因为服务器配置已经更新，所以将清空本地缓存的配置数据!");
                }
                CacheService.Clear();
                CacheService.Put("DAOCACHE_VERSION", dAOCacheVersion);
            }
            if (dAOCacheVersion == -1)
            {
                LoggingService.Error("不能获取新缓存版本号，请确认是否正确连接数据库");
            }
        }

        public static void Clear()
        {
            CacheService.Clear();
        }

        public static bool Contains(object key)
        {
            if (!CacheService.Contains(key))
            {
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.InfoFormatted("在缓存中没有找到'{0}'", new object[] { key });
                }
                return false;
            }
            if (!preLoadCaches)
            {
                Check();
            }
            return CacheService.Contains(key);
        }

        public static string CreateGroupCacheKey<T>(string groupPropertyName, string groupProperyValue) where T: DomainObject
        {
            return string.Format("{0}_groupcache_{1}_{2}", typeof(T).Name, groupPropertyName, groupProperyValue);
        }

        public static object Get(object key)
        {
            return CacheService.Get(key);
        }

        public static void LoadCaches()
        {
            preLoadCaches = true;
            bgWork = new BackgroundWorker();
            bgWork.WorkerSupportsCancellation = true;
            bgWork.WorkerReportsProgress = true;
            bgWork.DoWork += new DoWorkEventHandler(DAOCacheService.bgWork_DoWork);
            bgWork.RunWorkerCompleted += new RunWorkerCompletedEventHandler(DAOCacheService.bgWork_RunWorkerCompleted);
            bgWork.RunWorkerAsync();
        }

        public static void Put(object key, object value)
        {
            CacheService.Put(key, value);
        }

        public static void ReloadCaches(int version)
        {
            try
            {
                if (preLoadCaches && (bgWork != null))
                {
                    if (bgWork.IsBusy)
                    {
                        bgWork.CancelAsync();
                    }
                    Clear();
                    CacheService.Put("DAOCACHE_VERSION", version);
                    bgWork.RunWorkerAsync();
                }
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
            }
        }

        public static void Remove(object key)
        {
            CacheService.Remove(key);
        }

        public static void UpdateVersion()
        {
            DBSessionFactory.Instance.UpdateVersion();
        }
    }
}

