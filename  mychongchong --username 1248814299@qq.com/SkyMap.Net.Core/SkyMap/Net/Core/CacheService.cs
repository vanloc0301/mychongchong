namespace SkyMap.Net.Core
{
    using Bamboo.Prevalence;
    using Bamboo.Prevalence.Implementation;
    using System;
    using System.IO;
    using System.Reflection;

    public static class CacheService
    {
        private static PrevalenceEngine _engine;
        private static CacheSystem _system;
        private static FileStream fs;

        static CacheService()
        {
            Configure();
            AppDomain.CurrentDomain.ProcessExit += delegate (object sender, EventArgs e) {
                TakeSnapshot();
            };
        }

        public static void Clear()
        {
            try
            {
                _system.Clear();
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
            }
        }

        private static void Configure()
        {
            string path = Path.Combine(FileUtility.ApplicationRootPath, "DataCache");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (!string.IsNullOrEmpty(PropertyService.ConfigDirectory))
            {
                string str2 = PropertyService.Get<string>("DefaultServer", string.Empty);
                if (!string.IsNullOrEmpty(str2))
                {
                    path = Path.Combine(path, str2);
                }
                else
                {
                    path = Path.Combine(path, "default");
                }
            }
            CreateCacheSystem(TryGetaCacheDir(path, 0));
        }

        public static bool Contains(object key)
        {
            try
            {
                return _system.Contains(key);
            }
            catch (Exception exception)
            {
                LoggingService.WarnFormatted("获取或查询KEY值为:{0},的缓存对象时发生错误{1},将重建缓存系统...\r{2}", new object[] { key, exception.Message, exception.StackTrace });
                Clear();
                return false;
            }
        }

        private static void CreateCacheSystem(string dataDir)
        {
            try
            {
                InternalCreateCacheSystem(dataDir);
            }
            catch (Exception exception)
            {
                LoggingService.WarnFormatted("创建缓存系统时发生错误:{0},将重建缓存系统...\r{1}", new object[] { exception.Message, exception.StackTrace });
                try
                {
                    Directory.Delete(dataDir, true);
                    InternalCreateCacheSystem(dataDir);
                }
                catch (Exception exception2)
                {
                    LoggingService.WarnFormatted("重建缓存系统时发生错误:{0},将使用主程名称新建一缓存目录...\r{1}", new object[] { exception2.Message, exception2.StackTrace });
                    try
                    {
                        Assembly entryAssembly = Assembly.GetEntryAssembly();
                        if (entryAssembly != null)
                        {
                            InternalCreateCacheSystem(dataDir + entryAssembly.GetName().Name);
                        }
                        else
                        {
                            LoggingService.Debug("不存在主入口程序，使用时间临时新建缓存目录");
                            InternalCreateCacheSystem(dataDir + DateTime.Now.ToString("yyyyMMddHHmm"));
                        }
                    }
                    catch (Exception exception3)
                    {
                        LoggingService.WarnFormatted("重建缓存系统时发生错误:{0},使用时间临时新建缓存目录\r{1}", new object[] { exception2.Message, exception2.StackTrace });
                        InternalCreateCacheSystem(dataDir + DateTime.Now.ToString("yyyyMMddHHmm"));
                        LoggingService.Error(exception3);
                    }
                }
            }
        }

        public static object Get(object key)
        {
            if (key == null)
            {
                return null;
            }
            return _system.Get(key);
        }

        private static void InternalCreateCacheSystem(string dataDir)
        {
            if (!Directory.Exists(dataDir))
            {
                LoggingService.InfoFormatted("建立缓存目录{0}", new object[] { dataDir });
                Directory.CreateDirectory(dataDir);
            }
            _engine = PrevalenceActivator.CreateTransparentEngine(typeof(CacheSystem), dataDir);
            _system = _engine.PrevalentSystem as CacheSystem;
        }

        public static void Put(object key, object value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key", "null key not allowed");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value", "null value not allowed");
            }
            _system.Add(key, value);
        }

        public static void Remove(object key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            _system.Remove(key);
        }

        public static void TakeSnapshot()
        {
            if (_engine != null)
            {
                try
                {
                    int num2;
                    _engine.TakeSnapshot();
                    FileInfo[] files = _engine.PrevalenceBase.GetFiles("*.*");
                    Array.Sort(files, FileNameComparer.Default);
                    int num = -1;
                    for (num2 = files.Length - 1; num2 > -1; num2--)
                    {
                        if (0 == string.Compare(files[num2].Extension, ".snapshot", true))
                        {
                            num = num2;
                            break;
                        }
                    }
                    for (num2 = files.Length - 1; num2 > -1; num2--)
                    {
                        if (num2 != num)
                        {
                            files[num2].Delete();
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private static string TryGetaCacheDir(string dataDir, int i)
        {
            string str = dataDir + ((i > 0) ? i.ToString() : string.Empty);
            try
            {
                fs = new FileStream(str + ".cache", FileMode.Create, FileAccess.ReadWrite, FileShare.None);
                return str;
            }
            catch (Exception exception)
            {
                LoggingService.Error("缓存目录可能已在使用中：" + str, exception);
                int num = i + 1;
                return TryGetaCacheDir(dataDir, num);
            }
        }

        public static void Unload()
        {
            _engine.HandsOffOutputLog();
            _engine.PrevalenceBase.Delete(true);
        }
    }
}

