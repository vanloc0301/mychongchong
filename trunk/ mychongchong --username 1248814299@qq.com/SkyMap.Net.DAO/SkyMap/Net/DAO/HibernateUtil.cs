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
    public class HibernateUtil : MarshalByRefObject, IDA0
    {
        private ISession _session = null;
        private string assemlbyName;
        private ITransaction tx;

        public HibernateUtil(string configFile)
        {
            this.assemlbyName = configFile;
        }

        public void BeginTransaction()
        {
            ISession internalSession = this.InternalSession;
            if (!internalSession.IsConnected)
            {
                internalSession.Reconnect();
            }
            this.tx = internalSession.BeginTransaction();
        }

        public void Close()
        {
            this.tx = null;
            if (this._session != null)
            {
                this._session.Close();
                this._session = null;
            }
        }

        public void CommitTransaction()
        {
            if (this.tx == null)
            {
                throw new NullReferenceException("从没有启动事务");
            }
            this.tx.Commit();
        }

        public int Delete(string query)
        {
            return this.InternalSession.Delete(query);
        }

        public void Execute()
        {
            this.InternalSession.Flush();
        }

        public object ExecuteScalar(string queryName)
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("Excute Query:{0}", new object[] { queryName });
            }
            return this.ExecuteScalar(queryName, null);
        }

        public object ExecuteScalar(string queryName, object[] vals)
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("Excute Query:{0}", new object[] { queryName });
            }
            string str = "[sql]";
            string oldValue = "?";
            string customDefineNameQuerySQL = HibernateSessionFactory.GetCustomDefineNameQuerySQL(this.assemlbyName, queryName);
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
            return this.ExecuteSqlScalar(builder.ToString());
        }

        public DataTable ExecuteSql(string sql)
        {
            DataTable table2;
            ISession internalSession = this.InternalSession;
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("Excute Query:{0}", new object[] { sql });
            }
            IDbCommand command = internalSession.Connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = sql;
            if (internalSession.Transaction != null)
            {
                internalSession.Transaction.Enlist(command);
            }
            IDataReader reader = command.ExecuteReader();
            try
            {
                DataTable table = new DataTable();
                table.Load(reader);
                table2 = table;
            }
            finally
            {
                reader.Close();
            }
            return table2;
        }

        public DataSet ExecuteSqls(string[] sqls, string[] names)
        {
            if (sqls.Length != names.Length)
            {
                throw new ArgumentException("SQL语句数量与表名数量不一致!");
            }
            DataSet set = new DataSet();
            for (int i = 0; i < sqls.Length; i++)
            {
                DataTable table = this.ExecuteSql(sqls[i]);
                table.TableName = names[i];
                set.Tables.Add(table);
            }
            set.RemotingFormat = SerializationFormat.Binary;
            return set;
        }

        public object ExecuteSqlScalar(string sql)
        {
            ISession internalSession = this.InternalSession;
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("Excute Sql:{0}", new object[] { sql });
            }
            IDbCommand command = internalSession.Connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = sql;
            if (internalSession.Transaction != null)
            {
                internalSession.Transaction.Enlist(command);
            }
            return command.ExecuteScalar();
        }

        private DataTable Fill(IDataReader oDataReader)
        {
            DataTable table = new DataTable();
            DataTable schemaTable = oDataReader.GetSchemaTable();
            int num = 0;
            while (num < schemaTable.Rows.Count)
            {
                table.Columns.Add((string) schemaTable.Rows[num]["ColumnName"], (Type) schemaTable.Rows[num]["DataType"]);
                num++;
            }
            object[] values = new object[num];
            while (oDataReader.Read())
            {
                DataRow row = table.NewRow();
                oDataReader.GetValues(values);
                row.ItemArray = values;
                table.Rows.Add(row);
            }
            schemaTable.Rows.Clear();
            return table;
        }

        private IQuery GetQuery(string queryName, object[] vals)
        {
            IQuery namedQuery = this.InternalSession.GetNamedQuery(queryName);
            if (vals != null)
            {
                for (int i = 0; i < vals.Length; i++)
                {
                    namedQuery.SetParameter(i, vals[i]);
                }
            }
            return namedQuery;
        }

        private IQuery GetQuery(string queryName, string[] names, object[] vals)
        {
            IQuery namedQuery = this.InternalSession.GetNamedQuery(queryName);
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

        private void HibernatePut(object obj, DAOType command)
        {
            ISession internalSession = this.InternalSession;
            switch (command)
            {
                case DAOType.SAVE:
                    internalSession.Save(obj);
                    break;

                case DAOType.UPDATE:
                    internalSession.Update(obj);
                    break;

                case DAOType.DELETE:
                    internalSession.Delete(obj);
                    break;

                case DAOType.SaveOrUpdate:
                    internalSession.SaveOrUpdate(obj);
                    break;
            }
        }

        public T Load<T>(string id) where T: class
        {
            T local = null;
            try
            {
                local = this.InternalSession.Load<T>(id);
            }
            catch (ObjectNotFoundException)
            {
            }
            return local;
        }

        public object Load(Type clazz, string id)
        {
            object obj3 = null;
            try
            {
                obj3 = this.InternalSession.Load(clazz, id);
            }
            catch (ObjectNotFoundException)
            {
            }
            return obj3;
        }

        public void Put(object obj, DAOType command)
        {
            this.HibernatePut(obj, command);
        }

        public void Put(IEnumerable addObjs, IEnumerable updateObjs, IEnumerable removeObjs, bool isTransaction)
        {
            LoggingService.Info("开始批量保存删改数据...");
            if (isTransaction)
            {
                this.BeginTransaction();
            }
            try
            {
                foreach (object obj2 in addObjs)
                {
                    this.HibernatePut(obj2, DAOType.SAVE);
                }
                foreach (object obj2 in updateObjs)
                {
                    this.HibernatePut(obj2, DAOType.UPDATE);
                }
                foreach (object obj2 in removeObjs)
                {
                    this.HibernatePut(obj2, DAOType.DELETE);
                }
                if (isTransaction)
                {
                    this.CommitTransaction();
                }
                LoggingService.Info("结束批量保存删改数据...");
            }
            catch (Exception exception)
            {
                this.RollBackTransaction();
                LoggingService.Error(exception);
                throw exception;
            }
        }

        public IList<T> Query<T>(ISearch search)
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("Excute Query:{0}", new object[] { search.ToStatementString() });
            }
            return this.InternalSession.CreateQuery(search.ToStatementString()).List<T>();
        }

        public IList Query(ISearch search)
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("Excute Query:{0}", new object[] { search.ToStatementString() });
            }
            return this.InternalSession.Find(search.ToStatementString());
        }

        public IList<T> Query<T>(string queryName)
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("Excute Query:{0}", new object[] { queryName });
            }
            return this.InternalSession.GetNamedQuery(queryName).List<T>();
        }

        public IList Query(string queryName)
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("Excute Query:{0}", new object[] { queryName });
            }
            return this.InternalSession.GetNamedQuery(queryName).List();
        }

        public IList<T> Query<T>(string queryName, object[] vals)
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("Excute Query:{0}", new object[] { queryName });
            }
            return this.GetQuery(queryName, vals).List<T>();
        }

        public IList Query(string queryName, object[] vals)
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("Excute Query:{0}", new object[] { queryName });
            }
            return this.GetQuery(queryName, vals).List();
        }

        public IList<T> Query<T>(ISearch search, int pageNumber, int pageSize)
        {
            IQuery query = this.InternalSession.CreateQuery(search.ToStatementString());
            query.SetFirstResult((pageNumber - 1) * pageSize);
            query.SetMaxResults(pageNumber * pageSize);
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("Excute Query:{0}", new object[] { query.QueryString });
            }
            return query.List<T>();
        }

        public IList Query(ISearch search, int pageNumber, int pageSize)
        {
            IQuery query = this.InternalSession.CreateQuery(search.ToStatementString());
            query.SetFirstResult((pageNumber - 1) * pageSize);
            query.SetMaxResults(pageNumber * pageSize);
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("Excute Query:{0}", new object[] { query.QueryString });
            }
            return query.List();
        }

        public IList<T> Query<T>(string queryName, string[] names, object[] vals)
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("Excute Query:{0}", new object[] { queryName });
            }
            return this.GetQuery(queryName, names, vals).List<T>();
        }

        public IList Query(string queryName, string[] names, object[] vals)
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("Excute Query:{0}", new object[] { queryName });
            }
            return this.GetQuery(queryName, names, vals).List();
        }

        public IList<T> Query<T>(string queryName, object[] vals, int pageNumber, int pageSize)
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("Excute Query:{0},查询第{1}页", new object[] { queryName, pageNumber });
            }
            IQuery query = this.GetQuery(queryName, vals);
            query.SetFirstResult((pageNumber - 1) * pageSize);
            query.SetMaxResults(pageNumber * pageSize);
            return query.List<T>();
        }

        public IList Query(string queryName, object[] vals, int pageNumber, int pageSize)
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("Excute Query:{0},查询第{1}页", new object[] { queryName, pageNumber });
            }
            IQuery query = this.GetQuery(queryName, vals);
            query.SetFirstResult((pageNumber - 1) * pageSize);
            query.SetMaxResults(pageNumber * pageSize);
            return query.List();
        }

        public IList<T> Query<T>(string queryName, string[] names, object[] vals, int pageNumber, int pageSize)
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("Excute Query:{0}", new object[] { queryName });
            }
            IQuery query = this.GetQuery(queryName, names, vals);
            query.SetFirstResult((pageNumber - 1) * pageSize);
            query.SetMaxResults(pageNumber * pageSize);
            return query.List<T>();
        }

        public IList Query(string queryName, string[] names, object[] vals, int pageNumber, int pageSize)
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("Excute Query:{0}", new object[] { queryName });
            }
            IQuery query = this.GetQuery(queryName, names, vals);
            query.SetFirstResult((pageNumber - 1) * pageSize);
            query.SetMaxResults(pageNumber * pageSize);
            return query.List();
        }

        public object Refresh(object obj)
        {
            this.InternalSession.Refresh(obj);
            return obj;
        }

        public void RollBackTransaction()
        {
            if (this.tx != null)
            {
                this.tx.Rollback();
            }
        }

        public DataTable SQLQuery(string queryName)
        {
            return this.SQLQuery(queryName, null);
        }

        public DataTable SQLQuery(string queryName, object[] vals)
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("Excute SQLQuery:{0}", new object[] { queryName });
            }
            string str = "[sql]";
            string oldValue = "?";
            string customDefineNameQuerySQL = HibernateSessionFactory.GetCustomDefineNameQuerySQL(this.assemlbyName, queryName);
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
            return this.ExecuteSql(builder.ToString());
        }

        public IDbConnection Connection
        {
            get
            {
                return HibernateSessionFactory.GetConnection(this.assemlbyName);
            }
        }

        public string ConnectionString
        {
            get
            {
                return this.InternalSession.Connection.ConnectionString;
            }
        }

        public Type ConnectionType
        {
            get
            {
                return this.InternalSession.Connection.GetType();
            }
        }

        private ISession InternalSession
        {
            get
            {
                if (this._session == null)
                {
                    this._session = HibernateSessionFactory.GetSession(this.assemlbyName);
                    if (!this._session.IsConnected)
                    {
                        this._session.Reconnect();
                    }
                }
                return this._session;
            }
        }
    }
}

