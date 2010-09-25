namespace SkyMap.Net.DAO
{
    using SkyMap.Net.Core;
    using System;
    using System.Collections.Generic;

    public abstract class AbstractDAOCache : IDAOCache
    {
        protected AbstractDAOCache()
        {
        }

        protected void AddOneByOneToCache<T>(IEnumerable<T> objs, Action<T> action) where T: DomainObject
        {
            string name = typeof(T).Name;
            foreach (T local in objs)
            {
                this.AddOneCache<T>(local);
                if (action != null)
                {
                    action(local);
                }
            }
        }

        protected void AddOneCache<T>(T obj) where T: DomainObject
        {
            DAOCacheService.Put(typeof(T).Name + "_" + obj.Id, obj);
        }

        public abstract void Put();
        protected void PutTypeAllObjectsToCache<T>(IList<T> objs) where T: DomainObject
        {
            try
            {
                LoggingService.InfoFormatted("将获取{0}所有对象实例到缓存", new object[] { typeof(T).FullName });
                string name = typeof(T).Name;
                if (!DAOCacheService.Contains("ALL_ONE_" + name))
                {
                    string key = "ALL_" + name;
                    this.AddOneByOneToCache<T>(objs, null);
                    DAOCacheService.Put(key, objs);
                    DAOCacheService.Put("ALL_ONE_" + name, true);
                }
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
            }
        }

        protected void PutTypeToCache<T>(Action<IEnumerable<T>> action) where T: DomainObject
        {
            try
            {
                LoggingService.InfoFormatted("将获取{0}所有对象实例到缓存", new object[] { typeof(T).FullName });
                string name = typeof(T).Name;
                if (!DAOCacheService.Contains("ALL_ONE_" + name))
                {
                    IList<T> objs = QueryHelper.List<T>("ALL_" + name);
                    this.AddOneByOneToCache<T>(objs, null);
                    if (action != null)
                    {
                        action(objs);
                    }
                    DAOCacheService.Put("ALL_ONE_" + name, true);
                }
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
            }
        }

        protected void PutTypeToCache<T>(string[] orderBys, Action<IEnumerable<T>> action) where T: DomainObject
        {
            try
            {
                LoggingService.InfoFormatted("将获取{0}所有对象实例到缓存", new object[] { typeof(T).FullName });
                string name = typeof(T).Name;
                if (!DAOCacheService.Contains("ALL_ONE_" + name))
                {
                    IList<T> objs = QueryHelper.List<T>("ALL_" + name, orderBys);
                    this.AddOneByOneToCache<T>(objs, null);
                    if (action != null)
                    {
                        action(objs);
                    }
                    DAOCacheService.Put("ALL_ONE_" + name, true);
                }
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
            }
        }
    }
}

