namespace SkyMap.Net.DAO
{
    using NHibernate;
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO.Search;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;

    [Serializable]
    public class HibernateImpl : MarshalByRefObject, IDBSession
    {
        private void Close(ISession session)
        {
            session.Close();
        }

        public int Delete(string daoCfgName, string query)
        {
            int num2;
            ISession session = this.GetSession(daoCfgName);
            try
            {
                int num = session.Delete(query);
                session.Flush();
                num2 = num;
            }
            finally
            {
                this.Close(session);
            }
            return num2;
        }

        public object ExecuteScalar(string daoCfgName, string queryName)
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("Excute Query:{0}", new object[] { queryName });
            }
            return this.ExecuteScalar(daoCfgName, queryName, null);
        }

        public object ExecuteScalar(string daoCfgName, string queryName, object[] vals)
        {
            object obj2;
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("Excute Query:{0}", new object[] { queryName });
            }
            ISession session = this.GetSession(daoCfgName);
            try
            {
                string str = "[sql]";
                string oldValue = "?";
                string customDefineNameQuerySQL = HibernateSessionFactory.GetCustomDefineNameQuerySQL(daoCfgName, queryName);
                if (customDefineNameQuerySQL.IndexOf(str) <= -1)
                {
                    return null;
                }
                StringBuilder builder = new StringBuilder(customDefineNameQuerySQL);
                builder.Replace(str, "");
                if ((vals != null) && (vals.Length > 0))
                {
                    for (int i = 0; i < vals.Length; i++)
                    {
                        string newValue = vals[i].ToString();
                        builder.Replace(oldValue, newValue, builder.ToString().IndexOf(oldValue), 1);
                    }
                }
                obj2 = this.ExecuteSqlScalar(daoCfgName, builder.ToString());
            }
            finally
            {
                this.Close(session);
            }
            return obj2;
        }

        private DataTable ExecuteSql(ISession session, string cacheKey, string sql)
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("将执行SQL:{0}", new object[] { sql });
            }
            IDbCommand command = session.Connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = sql;
            if (session.Transaction != null)
            {
                session.Transaction.Enlist(command);
            }
            using (IDataReader reader = command.ExecuteReader())
            {
                DataTable table = new DataTable();
                table.Load(reader);
                if (!string.IsNullOrEmpty(cacheKey))
                {
                    DAOCacheService.Put(cacheKey, table);
                }
                return table;
            }
        }

        public DataTable ExecuteSql(string daoCfgName, string cacheKey, string sql)
        {
            if (this.GetFromCache(cacheKey))
            {
                return (DataTable) DAOCacheService.Get(cacheKey);
            }
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("Excute Query:{0}", new object[] { sql });
            }
            using (ISession session = this.GetSession(daoCfgName))
            {
                return this.ExecuteSql(session, cacheKey, sql);
            }
        }

        public int ExecuteSqlNonQuery(string daoCfgName, string sql)
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("Excute NonQuery:{0}", new object[] { sql });
            }
            using (ISession session = this.GetSession(daoCfgName))
            {
                return session.CreateSQLQuery(sql).ExecuteUpdate();
            }
        }

        public DataSet ExecuteSqls(string daoCfgName, string cacheKey, string[] sqls, string[] names)
        {
            if (this.GetFromCache(cacheKey))
            {
                return (DataSet) DAOCacheService.Get(cacheKey);
            }
            if (sqls.Length != names.Length)
            {
                throw new ArgumentException("SQL语句数量与表名数量不一致!");
            }
            using (ISession session = this.GetSession(daoCfgName))
            {
                DataSet set = new DataSet();
                for (int i = 0; i < sqls.Length; i++)
                {
                    DataTable table = this.ExecuteSql(session, string.Empty, sqls[i]);
                    table.TableName = names[i];
                    set.Tables.Add(table);
                }
                set.RemotingFormat = SerializationFormat.Binary;
                if (!string.IsNullOrEmpty(cacheKey))
                {
                    DAOCacheService.Put(cacheKey, set);
                }
                return set;
            }
        }

        public object ExecuteSqlScalar(string daoCfgName, string sql)
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("Excute Sql:{0}", new object[] { sql });
            }
            using (ISession session = this.GetSession(daoCfgName))
            {
                return session.CreateSQLQuery(sql).UniqueResult();
            }
        }

        public IDbConnection GetConnection(string nameSpace)
        {
            return HibernateSessionFactory.GetConnection(SupportClass.GetDAOConfigFile(nameSpace));
        }

        public IDbConnection GetConnection(Type type)
        {
            return HibernateSessionFactory.GetConnection(SupportClass.GetDAOConfigFile(type.Namespace));
        }

        public string GetConnectionString(string daoCfgName)
        {
            string connectionString;
            ISession session = this.GetSession(daoCfgName);
            try
            {
                connectionString = session.Connection.ConnectionString;
            }
            finally
            {
                this.Close(session);
            }
            return connectionString;
        }

        public Type GetConnectionType(string daoCfgName)
        {
            Type type;
            ISession session = this.GetSession(daoCfgName);
            try
            {
                type = session.Connection.GetType();
            }
            finally
            {
                this.Close(session);
            }
            return type;
        }

        private bool GetFromCache(string cacheKey)
        {
            return (!string.IsNullOrEmpty(cacheKey) && DAOCacheService.Contains(cacheKey));
        }

        private IQuery GetQuery(ISession session, string queryName, object[] vals)
        {
            IQuery namedQuery = session.GetNamedQuery(queryName);
            if (vals != null)
            {
                for (int i = 0; i < vals.Length; i++)
                {
                    namedQuery.SetParameter(i, vals[i]);
                }
            }
            return namedQuery;
        }

        private IQuery GetQuery(ISession session, string queryName, string[] names, object[] vals)
        {
            IQuery namedQuery = session.GetNamedQuery(queryName);
            if (names.Length != vals.Length)
            {
                throw new ApplicationException("Query parameters have errors:paras number and vals number is not equator");
            }
            int index = 0;
            foreach (string str in names)
            {
                if (vals[index] is IList)
                {
                    namedQuery.SetParameterList(str, vals[index] as ICollection);
                }
                else
                {
                    namedQuery.SetParameter(str, vals[index]);
                }
                index++;
            }
            return namedQuery;
        }

        private ISession GetSession(string daoCfgName)
        {
            ISession session = HibernateSessionFactory.GetSession(daoCfgName);
            if (!session.IsConnected)
            {
                session.Reconnect();
            }
            return session;
        }

        private IList<T> ListAndPutToCache<T>(string cacheKey, IQuery q, Action<IList<T>> action)
        {
            IList<T> list = q.List<T>();
            if (action != null)
            {
                action(list);
            }
            if (!string.IsNullOrEmpty(cacheKey))
            {
                DAOCacheService.Put(cacheKey, list);
            }
            return list;
        }

        public T Load<T>(string daoCfgName, string id) where T: DomainObject
        {
            ISession session = this.GetSession(daoCfgName);
            try
            {
                return session.Load<T>(id);
            }
            catch (ObjectNotFoundException)
            {
            }
            finally
            {
                this.Close(session);
            }
            return default(T);
        }

        public T Load<T>(string daoCfgName, string cacheKey, string id) where T: DomainObject
        {
            return this.Load<T>(daoCfgName, cacheKey, id, null);
        }

        public T Load<T>(string daoCfgName, string cacheKey, string id, Action<T> action) where T: DomainObject
        {
            if (this.GetFromCache(cacheKey))
            {
                return (T) DAOCacheService.Get(cacheKey);
            }
            ISession session = this.GetSession(daoCfgName);
            try
            {
                T local = session.Load<T>(id);
                if (action != null)
                {
                    action(local);
                }
                if (!(string.IsNullOrEmpty(cacheKey) || (local == null)))
                {
                    DAOCacheService.Put(cacheKey, local);
                }
                return local;
            }
            catch (ObjectNotFoundException)
            {
            }
            finally
            {
                this.Close(session);
            }
            return default(T);
        }

        public void Put(string daoCfgName, IEnumerable addObjs, IEnumerable updateObjs, IEnumerable removeObjs, bool isTransaction)
        {
            LoggingService.Info("开始批量保存删改数据...");
            ISession session = this.GetSession(daoCfgName);
            ITransaction transaction = null;
            if (isTransaction)
            {
                transaction = session.BeginTransaction();
            }
            try
            {
                foreach (object obj2 in addObjs)
                {
                    session.Save(obj2);
                }
                foreach (object obj2 in updateObjs)
                {
                    session.Update(obj2);
                }
                foreach (object obj2 in removeObjs)
                {
                    session.Delete(obj2);
                }
                if (transaction != null)
                {
                    transaction.Commit();
                }
                else
                {
                    session.Flush();
                }
                LoggingService.Info("结束批量保存删改数据...");
            }
            catch (Exception exception)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                LoggingService.Error(exception);
                throw exception;
            }
            finally
            {
                this.Close(session);
            }
        }

        public ArrayList Query<T, U, V, W>(string daoCfgName, string cacheKey, params KeyValuePair<string, object[]>[] querys)
        {
            ArrayList list2;
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("将同时执行多个查询...", new object[0]);
            }
            if (this.GetFromCache(cacheKey))
            {
                return (ArrayList) DAOCacheService.Get(cacheKey);
            }
            ISession session = this.GetSession(daoCfgName);
            try
            {
                ArrayList list = new ArrayList();
                for (int i = 0; i < querys.Length; i++)
                {
                    IQuery query = this.GetQuery(session, querys[i].Key, querys[i].Value);
                    switch (i)
                    {
                        case 0:
                            list.Add(query.List<T>());
                            break;

                        case 1:
                            list.Add(query.List<U>());
                            break;

                        case 2:
                            list.Add(query.List<V>());
                            break;

                        case 3:
                            list.Add(query.List<W>());
                            break;

                        default:
                            list.Add(query.List());
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(cacheKey))
                {
                    DAOCacheService.Put(cacheKey, list);
                }
                list2 = list;
            }
            finally
            {
                this.Close(session);
            }
            return list2;
        }

        public IList<T> Query<T>(string daoCfgName, string cacheKey, ISearch search)
        {
            IList<T> list;
            if (this.GetFromCache(cacheKey))
            {
                return (IList<T>) DAOCacheService.Get(cacheKey);
            }
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("Excute Query:{0}", new object[] { search.ToStatementString() });
            }
            ISession session = this.GetSession(daoCfgName);
            try
            {
                IQuery q = session.CreateQuery(search.ToStatementString());
                list = this.ListAndPutToCache<T>(cacheKey, q, null);
            }
            finally
            {
                this.Close(session);
            }
            return list;
        }

        public IList Query(string daoCfgName, string cacheKey, ISearch search)
        {
            IList list2;
            if (this.GetFromCache(cacheKey))
            {
                return (IList) DAOCacheService.Get(cacheKey);
            }
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("Excute Query:{0}", new object[] { search.ToStatementString() });
            }
            ISession session = this.GetSession(daoCfgName);
            try
            {
                IList list = session.CreateQuery(search.ToStatementString()).List();
                if (!string.IsNullOrEmpty(cacheKey))
                {
                    DAOCacheService.Put(cacheKey, list);
                }
                list2 = list;
            }
            finally
            {
                this.Close(session);
            }
            return list2;
        }

        public IList<T> Query<T>(string daoCfgName, string cacheKey, string queryName)
        {
            IList<T> list;
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("Excute Query:{0}", new object[] { queryName });
            }
            if (this.GetFromCache(cacheKey))
            {
                return (IList<T>) DAOCacheService.Get(cacheKey);
            }
            ISession session = this.GetSession(daoCfgName);
            try
            {
                IQuery namedQuery = session.GetNamedQuery(queryName);
                list = this.ListAndPutToCache<T>(cacheKey, namedQuery, null);
            }
            finally
            {
                this.Close(session);
            }
            return list;
        }

        public IList Query(string daoCfgName, string multiKey, string[] cacheKeys, params KeyValuePair<ISearch, Type>[] searchs)
        {
            IList list2;
            ISession session = this.GetSession(daoCfgName);
            try
            {
                if (!this.GetFromCache(multiKey))
                {
                    int num;
                    IMultiQuery query = session.CreateMultiQuery();
                    if (cacheKeys.Length != searchs.Length)
                    {
                        throw new ApplicationException("缓存键值数量与查询数量不相等");
                    }
                    for (num = 0; num < searchs.Length; num++)
                    {
                        string queryString = searchs[num].Key.ToStatementString();
                        if (LoggingService.IsInfoEnabled)
                        {
                            LoggingService.InfoFormatted("Excute Query:{0}", new object[] { queryString });
                        }
                        query.Add(searchs[num].Value, session.CreateQuery(queryString));
                    }
                    IList list = query.List();
                    DAOCacheService.Put(multiKey, list);
                    for (num = 0; num < cacheKeys.Length; num++)
                    {
                        if (!string.IsNullOrEmpty(cacheKeys[num]))
                        {
                            DAOCacheService.Put(searchs[num].Value, list[num]);
                        }
                    }
                    return list;
                }
                list2 = DAOCacheService.Get(multiKey) as IList;
            }
            finally
            {
                this.Close(session);
            }
            return list2;
        }

        public IList<T> Query<T>(string daoCfgName, string cacheKey, string queryName, object[] vals)
        {
            return this.Query<T>(daoCfgName, cacheKey, queryName, vals, null);
        }

        public IList<T> Query<T>(string daoCfgName, string cacheKey, ISearch search, int pageNumber, int pageSize)
        {
            IList<T> list;
            if (this.GetFromCache(cacheKey))
            {
                return (IList<T>) DAOCacheService.Get(cacheKey);
            }
            ISession session = this.GetSession(daoCfgName);
            try
            {
                IQuery q = session.CreateQuery(search.ToStatementString());
                q.SetFirstResult((pageNumber - 1) * pageSize);
                q.SetMaxResults(pageNumber * pageSize);
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.InfoFormatted("Excute Query:{0}", new object[] { q.QueryString });
                }
                list = this.ListAndPutToCache<T>(cacheKey, q, null);
            }
            finally
            {
                this.Close(session);
            }
            return list;
        }

        public IList<T> Query<T>(string daoCfgName, string cacheKey, string queryName, object[] vals, Action<IList<T>> action)
        {
            IList<T> list;
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("Excute Query:{0}", new object[] { queryName });
            }
            if (this.GetFromCache(cacheKey))
            {
                return (IList<T>) DAOCacheService.Get(cacheKey);
            }
            ISession session = this.GetSession(daoCfgName);
            try
            {
                IQuery q = this.GetQuery(session, queryName, vals);
                list = this.ListAndPutToCache<T>(cacheKey, q, action);
            }
            finally
            {
                this.Close(session);
            }
            return list;
        }

        public IList<T> Query<T>(string daoCfgName, string cacheKey, string queryName, string[] names, object[] vals)
        {
            return this.Query<T>(daoCfgName, cacheKey, queryName, names, vals, null);
        }

        public IList<T> Query<T>(string daoCfgName, string cacheKey, string queryName, object[] vals, int pageNumber, int pageSize)
        {
            IList<T> list;
            if (this.GetFromCache(cacheKey))
            {
                return (IList<T>) DAOCacheService.Get(cacheKey);
            }
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("Excute Query:{0},查询第{1}页", new object[] { queryName, pageNumber });
            }
            ISession session = this.GetSession(daoCfgName);
            try
            {
                IQuery q = this.GetQuery(session, queryName, vals);
                q.SetFirstResult((pageNumber - 1) * pageSize);
                q.SetMaxResults(pageNumber * pageSize);
                list = this.ListAndPutToCache<T>(cacheKey, q, null);
            }
            finally
            {
                this.Close(session);
            }
            return list;
        }

        public IList<T> Query<T>(string daoCfgName, string cacheKey, string queryName, string[] names, object[] vals, Action<IList<T>> action)
        {
            IList<T> list;
            if (this.GetFromCache(cacheKey))
            {
                return (IList<T>) DAOCacheService.Get(cacheKey);
            }
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("Excute Query:{0}", new object[] { queryName });
            }
            ISession session = this.GetSession(daoCfgName);
            try
            {
                IQuery q = this.GetQuery(session, queryName, names, vals);
                list = this.ListAndPutToCache<T>(cacheKey, q, action);
            }
            finally
            {
                this.Close(session);
            }
            return list;
        }

        public IList<T> Query<T>(string daoCfgName, string cacheKey, string queryName, string[] names, object[] vals, int pageNumber, int pageSize)
        {
            IList<T> list;
            if (this.GetFromCache(cacheKey))
            {
                return (IList<T>) DAOCacheService.Get(cacheKey);
            }
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("Excute Query:{0}", new object[] { queryName });
            }
            ISession session = this.GetSession(daoCfgName);
            try
            {
                IQuery q = this.GetQuery(session, queryName, names, vals);
                q.SetFirstResult((pageNumber - 1) * pageSize);
                q.SetMaxResults(pageNumber * pageSize);
                list = this.ListAndPutToCache<T>(cacheKey, q, null);
            }
            finally
            {
                this.Close(session);
            }
            return list;
        }

        public T Refresh<T>(string daoCfgName, T obj) where T: DomainObject
        {
            T local;
            ISession session = this.GetSession(daoCfgName);
            try
            {
                session.Refresh(obj);
                local = obj;
            }
            finally
            {
                this.Close(session);
            }
            return local;
        }

        public DataTable SQLQuery(string daoCfgName, string cacheKey, string queryName)
        {
            return this.SQLQuery(daoCfgName, cacheKey, queryName, null);
        }

        public DataTable SQLQuery(string daoCfgName, string cacheKey, string queryName, object[] vals)
        {
            if (this.GetFromCache(cacheKey))
            {
                return (DataTable) DAOCacheService.Get(cacheKey);
            }
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("Excute SQLQuery:{0}", new object[] { queryName });
            }
            string str = "[sql]";
            string customDefineNameQuerySQL = HibernateSessionFactory.GetCustomDefineNameQuerySQL(daoCfgName, queryName);
            if (customDefineNameQuerySQL.IndexOf(str) <= -1)
            {
                return null;
            }
            StringBuilder builder = new StringBuilder(customDefineNameQuerySQL);
            builder.Replace(str, "");
            if ((vals != null) && (vals.Length > 0))
            {
                string str3 = "?";
                if ((vals != null) && (vals.Length > 0))
                {
                    for (int i = 0; i < vals.Length; i++)
                    {
                        int index = builder.ToString().IndexOf(str3);
                        if (index > -1)
                        {
                            if (LoggingService.IsDebugEnabled)
                            {
                                LoggingService.DebugFormatted("将在位置{0}替换?为：{1}", new object[] { index, vals[i] });
                            }
                            string newValue = vals[i].ToString();
                            builder.Replace(str3, newValue, index, 1);
                        }
                    }
                }
            }
            return this.ExecuteSql(daoCfgName, cacheKey, builder.ToString());
        }

        public void UpdateVersion()
        {
            ISession session = this.GetSession(SupportClass.GetDAOConfigFile("SkyMap.Net.Core"));
            try
            {
                string str = string.Format("UPDATE DAOCACHE_VERSION SET VERSION=VERSION+1,LAST_DATE='{0}';\r\nINSERT INTO DAOCACHE_VERSION SELECT 1 AS VERSION,'{0}' AS LAST_DATE where not exists(select 1 from daocache_version); \r\nSELECT VERSION FROM DAOCACHE_VERSION", DateTime.Now);
                using (IDbCommand command = session.Connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = str;
                    if (session.Transaction != null)
                    {
                        session.Transaction.Enlist(command);
                    }
                    DAOCacheService.ReloadCaches(Convert.ToInt32(command.ExecuteScalar()));
                }
            }
            catch (Exception exception)
            {
                LoggingService.Error("更新缓存版本时发生错误", exception);
                throw exception;
            }
            finally
            {
                session.Close();
            }
        }

        public int DAOCacheVersion
        {
            get
            {
                if (DAOCacheService.preLoadCaches && CacheService.Contains("DAOCACHE_VERSION"))
                {
                    return (int) CacheService.Get("DAOCACHE_VERSION");
                }
                string sql = "SELECT VERSION FROM DAOCACHE_VERSION";
                int num = -1;
                try
                {
                    num = Convert.ToInt32(this.ExecuteSqlScalar(SupportClass.GetDAOConfigFile("SkyMap.Net.Core"), sql));
                }
                catch (Exception exception)
                {
                    LoggingService.Error(exception);
                }
                return num;
            }
        }
    }
}

