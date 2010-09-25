namespace SkyMap.Net.DAO
{
    using SkyMap.Net.DAO.Search;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;

    public interface IDBSession
    {
        int Delete(string daoCfgName, string query);
        object ExecuteScalar(string daoCfgName, string queryName);
        object ExecuteScalar(string daoCfgName, string queryName, object[] vals);
        DataTable ExecuteSql(string daoCfgName, string cacheKey, string sql);
        int ExecuteSqlNonQuery(string daoCfgName, string sql);
        DataSet ExecuteSqls(string daoCfgName, string cacheKey, string[] sqls, string[] names);
        object ExecuteSqlScalar(string daoCfgName, string sql);
        IDbConnection GetConnection(string nameSpace);
        IDbConnection GetConnection(Type type);
        string GetConnectionString(string daoCfgName);
        Type GetConnectionType(string daoCfgName);
        T Load<T>(string daoCfgName, string id) where T: DomainObject;
        T Load<T>(string daoCfgName, string cacheKey, string id) where T: DomainObject;
        T Load<T>(string daoCfgName, string cacheKey, string id, Action<T> action) where T: DomainObject;
        void Put(string daoCfgName, IEnumerable addObjs, IEnumerable updateObjs, IEnumerable removeObjs, bool isTransaction);
        ArrayList Query<T, U, V, W>(string daoCfgName, string cacheKey, params KeyValuePair<string, object[]>[] querys);
        IList<T> Query<T>(string daoCfgName, string cacheKey, ISearch search);
        IList Query(string daoCfgName, string cacheKey, ISearch search);
        IList<T> Query<T>(string daoCfgName, string cacheKey, string queryName);
        IList<T> Query<T>(string daoCfgName, string cacheKey, string queryName, object[] vals);
        IList Query(string daoCfgName, string multiKey, string[] cacheKeys, params KeyValuePair<ISearch, Type>[] searchs);
        IList<T> Query<T>(string daoCfgName, string cacheKey, ISearch search, int pageNumber, int pageSize);
        IList<T> Query<T>(string daoCfgName, string cacheKey, string queryName, object[] vals, Action<IList<T>> action);
        IList<T> Query<T>(string daoCfgName, string cacheKey, string queryName, string[] names, object[] vals);
        IList<T> Query<T>(string daoCfgName, string cacheKey, string queryName, object[] vals, int pageNumber, int pageSize);
        IList<T> Query<T>(string daoCfgName, string cacheKey, string queryName, string[] names, object[] vals, Action<IList<T>> action);
        IList<T> Query<T>(string daoCfgName, string cacheKey, string queryName, string[] names, object[] vals, int pageNumber, int pageSize);
        T Refresh<T>(string daoCfgName, T obj) where T: DomainObject;
        DataTable SQLQuery(string daoCfgName, string cacheKey, string queryName);
        DataTable SQLQuery(string daoCfgName, string cacheKey, string queryName, object[] vals);
        void UpdateVersion();

        int DAOCacheVersion { get; }
    }
}

