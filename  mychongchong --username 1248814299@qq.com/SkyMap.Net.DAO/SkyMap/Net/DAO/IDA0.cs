namespace SkyMap.Net.DAO
{
    using SkyMap.Net.DAO.Search;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;

    public interface IDA0
    {
        void BeginTransaction();
        void Close();
        void CommitTransaction();
        int Delete(string query);
        void Execute();
        object ExecuteScalar(string queryName);
        object ExecuteScalar(string queryName, object[] vals);
        DataTable ExecuteSql(string sql);
        DataSet ExecuteSqls(string[] sqls, string[] names);
        object ExecuteSqlScalar(string sql);
        T Load<T>(string id) where T: class;
        object Load(Type clazz, string id);
        void Put(object obj, DAOType command);
        void Put(IEnumerable addObjs, IEnumerable updateObjs, IEnumerable removeObjs, bool isTransaction);
        IList<T> Query<T>(ISearch search);
        IList Query(ISearch search);
        IList<T> Query<T>(string queryName);
        IList Query(string queryName);
        IList<T> Query<T>(string queryName, object[] vals);
        IList Query(string queryName, object[] vals);
        IList<T> Query<T>(ISearch search, int pageNumber, int pageSize);
        IList Query(ISearch search, int pageNumber, int pageSize);
        IList<T> Query<T>(string queryName, string[] names, object[] vals);
        IList Query(string queryName, string[] names, object[] vals);
        IList<T> Query<T>(string queryName, object[] vals, int pageNumber, int pageSize);
        IList Query(string queryName, object[] vals, int pageNumber, int pageSize);
        IList<T> Query<T>(string queryName, string[] names, object[] vals, int pageNumber, int pageSize);
        IList Query(string queryName, string[] names, object[] vals, int pageNumber, int pageSize);
        object Refresh(object obj);
        void RollBackTransaction();
        DataTable SQLQuery(string queryName);
        DataTable SQLQuery(string queryName, object[] vals);

        IDbConnection Connection { get; }

        string ConnectionString { get; }

        Type ConnectionType { get; }
    }
}

