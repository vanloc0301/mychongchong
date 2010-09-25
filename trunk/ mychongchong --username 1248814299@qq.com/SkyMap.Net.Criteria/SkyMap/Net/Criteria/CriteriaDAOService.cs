namespace SkyMap.Net.Criteria
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    [Serializable]
    public class CriteriaDAOService : MarshalByRefObject
    {
        public DAOObjects Commit(DAOObjects daoObjects)
        {
            this.CurrentUnitOfWork.Commit(daoObjects);
            return daoObjects;
        }

        public SMDataSource CreateDataSource()
        {
            SMDataSource source = new SMDataSource();
            source.Id = StringHelper.GetNewGuid();
            source.Name = "新数据源";
            UnitOfWork currentUnitOfWork = this.CurrentUnitOfWork;
            currentUnitOfWork.RegisterNew(source);
            currentUnitOfWork.Commit();
            return source;
        }

        public List<TyFieldsSelect> CreateFieldsFromSchemaTable(DataTable schemaTable, TySearchx sx, string tableName)
        {
            List<TyFieldsSelect> list = new List<TyFieldsSelect>();
            UnitOfWork currentUnitOfWork = this.CurrentUnitOfWork;
            foreach (DataRow row in schemaTable.Rows)
            {
                TyFieldsSelect select = new TyFieldsSelect();
                select.Id = StringHelper.GetNewGuid();
                select.TableName = tableName;
                select.TySearchx = sx;
                select.Name = row["ColumnName"].ToString();
                select.Type = row["DataType"].ToString();
                select.DisplayIndex = list.Count * 10;
                select.DisplayName = select.Name;
                select.ConditionName = select.Name;
                currentUnitOfWork.RegisterNew(select);
                list.Add(select);
            }
            currentUnitOfWork.Commit();
            return list;
        }

        public TySearchtable CreateSearchtable(TySearchx sx)
        {
            TySearchtable searchtable = new TySearchtable();
            searchtable.Id = StringHelper.GetNewGuid();
            searchtable.TableName = "新查询表";
            searchtable.TySearchx = sx;
            searchtable.TableOrder = sx.TySearchtables.Count;
            UnitOfWork currentUnitOfWork = this.CurrentUnitOfWork;
            currentUnitOfWork.RegisterNew(searchtable);
            currentUnitOfWork.Commit();
            return searchtable;
        }

        public TySearchx CreateSearchX()
        {
            TySearchx searchx = new TySearchx();
            searchx.Id = StringHelper.GetNewGuid();
            searchx.Name = "新查询X";
            UnitOfWork currentUnitOfWork = this.CurrentUnitOfWork;
            currentUnitOfWork.RegisterNew(searchx);
            currentUnitOfWork.Commit();
            return searchx;
        }

        public DataSet Execute(string dsOID, string[] sqls, string[] tables)
        {
            SMDataSource source = this.FindDataSource(dsOID);
            if (source == null)
            {
                throw new NullReferenceException(string.Format("没有找到指定的'{0}'数据源", dsOID));
            }
            if (!string.IsNullOrEmpty(source.ConnectionString) && !string.IsNullOrEmpty(source.DSType))
            {
                DbConnection connection = DbProviderFactories.GetFactory(source.DSType).CreateConnection();
                connection.ConnectionString = source.DecryptConnectionString;
                if (LoggingService.IsDebugEnabled)
                {
                    LoggingService.DebugFormatted("数据库连接字符串是:{0}", new object[] { connection.ConnectionString });
                }
                connection.Open();
                try
                {
                    DbCommand command = connection.CreateCommand();
                    DataSet set = new DataSet();
                    for (int i = 0; i < sqls.Length; i++)
                    {
                        command.CommandText = sqls[i];
                        LoggingService.InfoFormatted("将执行:{0}", new object[] { command.CommandText });
                        DbDataReader reader = command.ExecuteReader();
                        try
                        {
                            DataTable table = new DataTable();
                            table.Load(reader);
                            table.TableName = tables[i];
                            set.Tables.Add(table);
                        }
                        finally
                        {
                            reader.Close();
                        }
                    }
                    return set;
                }
                finally
                {
                    connection.Close();
                }
            }
            throw new NullReferenceException(string.Format("连接字符串'{0}'或者数据提供者'{1}'为空!", source.ConnectionString, source.DSType));
        }

        public static List<SMDataSource> FindActiveDataSources()
        {
            IList<SMDataSource> allDataSource = GetAllDataSource();
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("获取了{0}个查询数据源", new object[] { allDataSource.Count });
            }
            List<SMDataSource> list2 = new List<SMDataSource>();
            foreach (SMDataSource source in allDataSource)
            {
                if (source.IsActive)
                {
                    list2.Add(source);
                }
            }
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("获取了{0}个有效查询数据源", new object[] { list2.Count });
            }
            return list2;
        }

        private SMDataSource FindDataSource(string oid)
        {
            foreach (SMDataSource source in GetAllDataSource())
            {
                if (LoggingService.IsDebugEnabled)
                {
                    LoggingService.DebugFormatted("要查询数据源是'{0}',当前数据源是'{1}',是否一致:{2}", new object[] { oid, source.Id, source.Id == oid });
                }
                if (source.Id == oid)
                {
                    return source;
                }
            }
            return null;
        }

        private static IList<SMDataSource> GetAllDataSource()
        {
            IList<SMDataSource> tyDataSources = null;
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.Debug("获取有效的查询数据源列表...");
            }
            string key = "ALL_SMDataSource";
            if (!DAOCacheService.Contains(key))
            {
                try
                {
                    tyDataSources = RemotingSingletonProvider<CriteriaDAOService>.Instance.TyDataSources;
                    if (LoggingService.IsDebugEnabled)
                    {
                        LoggingService.DebugFormatted("共获取{0}个查询数据源", new object[] { tyDataSources.Count });
                    }
                    DAOCacheService.Put(key, tyDataSources);
                }
                catch (Exception exception)
                {
                    LoggingService.Error(exception);
                }
                return tyDataSources;
            }
            return (IList<SMDataSource>) DAOCacheService.Get(key);
        }

        public DomainObject Refresh(DomainObject dao)
        {
            QueryHelper.Refresh<DomainObject>(dao);
            return dao;
        }

        private UnitOfWork CurrentUnitOfWork
        {
            get
            {
                return new UnitOfWork(base.GetType());
            }
        }

        public IList<SMDataSource> TyDataSources
        {
            get
            {
                return QueryHelper.List<SMDataSource>("ALL_SMDataSource");
            }
        }

        public IList<TySearchx> TySearchXs
        {
            get
            {
                return QueryHelper.List<TySearchx>("ALL_TySearchx");
            }
        }
    }
}

