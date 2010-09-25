namespace SkyMap.Net.DAO
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO.Search;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;

    public static class QueryHelper
    {

        private sealed class c__DisplayClass1<T> where T : DomainObject
        {
            // Fields
            public T obj;

            // Methods
            public void Rb__0()
            {
                DBSessionFactory.Instance.Refresh<T>(SupportClass.GetDAOConfigFile(typeof(T).Namespace), this.obj);
            }
        }

        private sealed class c__DisplayClass11<T> where T : DomainObject
        {
            // Fields
            public Action<T> action;
            public string cacheKey;
            public string id;

            // Methods
            public T Gb__10()
            {
                return DBSessionFactory.Instance.Load<T>(SupportClass.GetDAOConfigFile(typeof(T).Namespace), this.cacheKey, this.id, this.action);
            }
        }
        private sealed class c__DisplayClass14<T>
        {
            // Fields
            public string cacheKey;
        }
        private sealed class c__DisplayClass16<T>
        {
            // Fields
            public QueryHelper.c__DisplayClass14<T> cs__a8locals15;
            public ISearch s;
            public Type type;

            // Methods
            public IList<T> Lb__13()
            {
                return DBSessionFactory.Instance.Query<T>(SupportClass.GetDAOConfigFile(this.type.Namespace), this.cs__a8locals15.cacheKey, this.s);
            }
        }

        private sealed class c__DisplayClass19
        {
            // Fields
            public string cacheKey;
            public Type type;
        }

        private sealed class c__DisplayClass1b
        {
            // Fields
            public QueryHelper.c__DisplayClass19 cs__a8locals1a;
            public ISearch s;

            // Methods
            public IList Lb__18()
            {
                return DBSessionFactory.Instance.Query(SupportClass.GetDAOConfigFile(this.cs__a8locals1a.type.Namespace), this.cs__a8locals1a.cacheKey, this.s);
            }
        }


        private sealed class c__DisplayClass1e<T>
        {
            // Fields
            public string cacheKey;
        }
        private sealed class c__DisplayClass20<T>
        {
            // Fields
            public QueryHelper.c__DisplayClass1e<T> cs__a8locals1f;
            public ISearch s;

            // Methods
            public IList<T> Lb__1d()
            {
                return DBSessionFactory.Instance.Query<T>(SupportClass.GetDAOConfigFile(typeof(T).Namespace), this.cs__a8locals1f.cacheKey, this.s);
            }
        }
        private sealed class c__DisplayClass23
        {
            // Fields
            public string cacheKey;
            public string nameSpace;
        }
        private sealed class c__DisplayClass25
        {
            // Fields
            public QueryHelper.c__DisplayClass23 cs__a8locals24;
            public ISearch s;

            // Methods
            public IList Lb__22()
            {
                return DBSessionFactory.Instance.Query(SupportClass.GetDAOConfigFile(this.cs__a8locals24.nameSpace), this.cs__a8locals24.cacheKey, this.s);
            }
        }

        private sealed class c__DisplayClass28<T>
        {
            // Fields
            public string cacheKey;
            public string nameSpace;
            public string queryName;

            // Methods
            public IList<T> Lb__27()
            {
                return DBSessionFactory.Instance.Query<T>(SupportClass.GetDAOConfigFile(this.nameSpace), this.cacheKey, this.queryName);
            }
        }

        private sealed class c__DisplayClass2b<T>
        {
            // Fields
            public Action<IList<T>> action;
            public string cacheKey;
            public string nameSpace;
            public string queryName;
            public object[] vals;

            // Methods
            public IList<T> Lb__2a()
            {
                return DBSessionFactory.Instance.Query<T>(SupportClass.GetDAOConfigFile(this.nameSpace), this.cacheKey, this.queryName, this.vals, this.action);
            }
        }
        private sealed class c__DisplayClass2e<T>
        {
            // Fields
            public string cacheKey;
            public string[] names;
            public string nameSpace;
            public string queryName;
            public object[] vals;

            // Methods
            public IList<T> Lb__2d()
            {
                return DBSessionFactory.Instance.Query<T>(SupportClass.GetDAOConfigFile(this.nameSpace), this.cacheKey, this.queryName, this.names, this.vals);
            }
        }
        private sealed class c__DisplayClass31<T>
        {
            // Fields
            public Action<IList<T>> action;
            public string cacheKey;
            public string[] names;
            public string nameSpace;
            public string queryName;
            public object[] vals;

            // Methods
            public IList<T> Lb__30()
            {
                return DBSessionFactory.Instance.Query<T>(SupportClass.GetDAOConfigFile(this.nameSpace), this.cacheKey, this.queryName, this.names, this.vals, this.action);
            }
        }
        private sealed class c__DisplayClass34<T>
        {
            // Fields
            public string cacheKey;
            public int pageNumber;
            public int pageSize;
        }

        private sealed class c__DisplayClass36<T>
        {
            // Fields
            public QueryHelper.c__DisplayClass34<T> cs__a8locals35;
            public ISearch s;
            public Type type;

            // Methods
            public IList<T> Lb__33()
            {
                return DBSessionFactory.Instance.Query<T>(SupportClass.GetDAOConfigFile(this.type.Namespace), this.cs__a8locals35.cacheKey, this.s, this.cs__a8locals35.pageNumber, this.cs__a8locals35.pageSize);
            }
        }

        private sealed class c__DisplayClass39<T>
        {
            // Fields
            public string cacheKey;
        }

        private sealed class c__DisplayClass3b<T>
        {
            // Fields
            public QueryHelper.c__DisplayClass39<T> cs__a8locals3a;
            public ISearch s;
            public Type type;

            // Methods
            public IList<T> Lb__38()
            {
                return DBSessionFactory.Instance.Query<T>(SupportClass.GetDAOConfigFile(this.type.Namespace), this.cs__a8locals3a.cacheKey, this.s);
            }
        }

        private sealed class c__DisplayClass3e<T>
        {
            // Fields
            public string cacheKey;
            public string nameSpace;
            public int pageNumber;
            public int pageSize;
            public string queryName;

            // Methods
            public IList<T> Lb__3d()
            {
                return DBSessionFactory.Instance.Query<T>(SupportClass.GetDAOConfigFile(this.nameSpace), this.cacheKey, this.queryName, null, this.pageNumber, this.pageSize);
            }
        }

        private sealed class c__DisplayClass4<T, U, V, W>
        {
            // Fields
            public string cacheKey;
            public string nameSpace;
            public KeyValuePair<string, object[]>[] querys;

            // Methods
            public ArrayList Lb__3()
            {
                return DBSessionFactory.Instance.Query<T, U, V, W>(SupportClass.GetDAOConfigFile(this.nameSpace), this.cacheKey, this.querys);
            }
        }

        private sealed class c__DisplayClass41<T>
        {
            // Fields
            public string cacheKey;
            public string nameSpace;
            public int pageNumber;
            public int pageSize;
            public string queryName;
            public object[] vals;

            // Methods
            public IList<T> Lb__40()
            {
                return DBSessionFactory.Instance.Query<T>(SupportClass.GetDAOConfigFile(this.nameSpace), this.cacheKey, this.queryName, this.vals, this.pageNumber, this.pageSize);
            }
        }

        private sealed class c__DisplayClass44<T>
        {
            // Fields
            public string cacheKey;
            public string[] names;
            public string nameSpace;
            public int pageNumber;
            public int pageSize;
            public string queryName;
            public object[] vals;

            // Methods
            public IList<T> Lb__43()
            {
                return DBSessionFactory.Instance.Query<T>(SupportClass.GetDAOConfigFile(this.nameSpace), this.cacheKey, this.queryName, this.names, this.vals, this.pageNumber, this.pageSize);
            }
        }
        private sealed class c__DisplayClass47
        {
            // Fields
            public string nameSpace;
            public string queryName;

            // Methods
            public object Eb__46()
            {
                return DBSessionFactory.Instance.ExecuteScalar(SupportClass.GetDAOConfigFile(this.nameSpace), this.queryName);
            }
        }

        private sealed class c__DisplayClass4a
        {
            // Fields
            public string nameSpace;
            public string queryName;
            public object[] vals;

            // Methods
            public object Eb__49()
            {
                return DBSessionFactory.Instance.ExecuteScalar(SupportClass.GetDAOConfigFile(this.nameSpace), this.queryName, this.vals);
            }
        }

        private sealed class c__DisplayClass4d
        {
            // Fields
            public string nameSpace;
            public string sql;

            // Methods
            public object Eb__4c()
            {
                return DBSessionFactory.Instance.ExecuteSqlScalar(SupportClass.GetDAOConfigFile(this.nameSpace), this.sql);
            }
        }

        private sealed class c__DisplayClass50
        {
            // Fields
            public string cacheKey;
            public string nameSpace;
            public string sql;

            // Methods
            public DataTable Eb__4f()
            {
                return DBSessionFactory.Instance.ExecuteSql(SupportClass.GetDAOConfigFile(this.nameSpace), this.cacheKey, this.sql);
            }
        }

        private sealed class c__DisplayClass53
        {
            // Fields
            public string cacheKey;
            public string[] names;
            public string nameSpace;
            public string[] sqls;

            // Methods
            public DataSet Eb__52()
            {
                return DBSessionFactory.Instance.ExecuteSqls(SupportClass.GetDAOConfigFile(this.nameSpace), this.cacheKey, this.sqls, this.names);
            }
        }

        private sealed class c__DisplayClass56
        {
            // Fields
            public string cacheKey;
            public string nameSpace;
            public string queryName;
            public object[] vals;

            // Methods
            public DataTable Eb__55()
            {
                return DBSessionFactory.Instance.SQLQuery(SupportClass.GetDAOConfigFile(this.nameSpace), this.cacheKey, this.queryName, this.vals);
            }
        }

        private sealed class c__DisplayClass59
        {
            // Fields
            public string cacheKey;
            public string nameSpace;
            public string queryName;

            // Methods
            public DataTable Eb__58()
            {
                return DBSessionFactory.Instance.SQLQuery(SupportClass.GetDAOConfigFile(this.nameSpace), this.cacheKey, this.queryName);
            }
        }

        private sealed class c__DisplayClass5c
        {
            // Fields
            public string nameSapce;
            public string sql;

            // Methods
            public int Eb__5b()
            {
                return DBSessionFactory.Instance.ExecuteSqlNonQuery(SupportClass.GetDAOConfigFile(this.nameSapce), this.sql);
            }
        }

        private sealed class c__DisplayClass8
        {
            // Fields
            public string multiKey;
            public string nameSpace;
            public QueryHelper.MutliSearch searchs;

            // Methods
            public IList Lb__6()
            {
                return DBSessionFactory.Instance.Query(SupportClass.GetDAOConfigFile(this.nameSpace), this.multiKey, this.searchs.CacheKeys.ToArray(), this.searchs.ToArray());
            }
        }

        private sealed class c__DisplayClassb<T> where T : DomainObject
        {
            // Fields
            public string id;

            // Methods
            public T Gb__a()
            {
                return DBSessionFactory.Instance.Load<T>(SupportClass.GetDAOConfigFile(typeof(T).Namespace), this.id);
            }
        }

        private sealed class c__DisplayClasse<T> where T : DomainObject
        {
            // Fields
            public string cacheKey;
            public string id;

            // Methods
            public T Gb__d()
            {
                return DBSessionFactory.Instance.Load<T>(SupportClass.GetDAOConfigFile(typeof(T).Namespace), this.cacheKey, this.id);
            }
        }

        public static MutliSearch CreateMutliSearch()
        {
            return new MutliSearch();
        }

        public static int Delete(string nameSpace, string query)
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("将根据查询语句:{0}删除多个对象", new object[] { query });
            }
            return DBSessionFactory.Instance.Delete(SupportClass.GetDAOConfigFile(nameSpace), query);
        }

        public static object ExecuteScalar(string nameSpace, string queryName)
        {
            return RedoIfError.Execute<object>(delegate
            {
                return DBSessionFactory.Instance.ExecuteScalar(SupportClass.GetDAOConfigFile(nameSpace), queryName);
            }, 2);
        }

        public static object ExecuteScalar(string nameSpace, string queryName, object[] vals)
        {
            return RedoIfError.Execute<object>(delegate
            {
                return DBSessionFactory.Instance.ExecuteScalar(SupportClass.GetDAOConfigFile(nameSpace), queryName, vals);
            }, 2);
        }

        public static DataTable ExecuteSql(string nameSpace, string cacheKey, string sql)
        {
            if (GetFromCache(cacheKey))
            {
                return (DataTable)DAOCacheService.Get(cacheKey);
            }
            return PutToCachesAndReturn<DataTable>(cacheKey, RedoIfError.Execute<DataTable>(delegate
            {
                return DBSessionFactory.Instance.ExecuteSql(SupportClass.GetDAOConfigFile(nameSpace), cacheKey, sql);
            }, 2));
        }

        public static int ExecuteSqlNonQuery(string nameSapce, string sql)
        {
            return RedoIfError.Execute<int>(delegate
            {
                return DBSessionFactory.Instance.ExecuteSqlNonQuery(SupportClass.GetDAOConfigFile(nameSapce), sql);
            }, 2);
        }

        public static DataTable ExecuteSqlQuery(string nameSpace, string cacheKey, string queryName)
        {
            if (GetFromCache(cacheKey))
            {
                return (DataTable)DAOCacheService.Get(cacheKey);
            }
            return PutToCachesAndReturn<DataTable>(cacheKey, RedoIfError.Execute<DataTable>(delegate
            {
                return DBSessionFactory.Instance.SQLQuery(SupportClass.GetDAOConfigFile(nameSpace), cacheKey, queryName);
            }, 2));
        }

        public static DataTable ExecuteSqlQuery(string nameSpace, string cacheKey, string queryName, object[] vals)
        {
            LoggingService.DebugFormatted("使用的命名空间是:{0}", new object[] { cacheKey });
            if (GetFromCache(cacheKey))
            {
                return (DataTable)DAOCacheService.Get(cacheKey);
            }
            return PutToCachesAndReturn<DataTable>(cacheKey, RedoIfError.Execute<DataTable>(delegate
            {
                return DBSessionFactory.Instance.SQLQuery(SupportClass.GetDAOConfigFile(nameSpace), cacheKey, queryName, vals);
            }, 2));
        }

        public static DataSet ExecuteSqls(string nameSpace, string cacheKey, string[] sqls, string[] names)
        {
            if (GetFromCache(cacheKey))
            {
                return (DataSet)DAOCacheService.Get(cacheKey);
            }
            return PutToCachesAndReturn<DataSet>(cacheKey, RedoIfError.Execute<DataSet>(delegate
            {
                return DBSessionFactory.Instance.ExecuteSqls(SupportClass.GetDAOConfigFile(nameSpace), cacheKey, sqls, names);
            }, 2));
        }

        public static object ExecuteSqlScalar(string nameSpace, string sql)
        {
            return RedoIfError.Execute<object>(delegate
            {
                return DBSessionFactory.Instance.ExecuteSqlScalar(SupportClass.GetDAOConfigFile(nameSpace), sql);
            }, 2);
        }

        public static IList<T> Find<T>(string groupPropertyName, string groupPropertyValue, Predicate<T> predicate) where T : DomainObject
        {
            IList<T> list = null;
            string key = DAOCacheService.CreateGroupCacheKey<T>(groupPropertyName, groupPropertyValue);
            if (!DAOCacheService.Contains(key))
            {
                IList<T> list2;
                string str2 = "ALL_" + typeof(T).Name;
                if (!DAOCacheService.Contains(str2))
                {
                    list2 = List<T>(str2);
                }
                else
                {
                    list2 = (IList<T>)DAOCacheService.Get(str2);
                }
                list = new List<T>(list2).FindAll(predicate);
                DAOCacheService.Put(key, list);
                return list;
            }
            return (DAOCacheService.Get(key) as IList<T>);
        }

        public static T Get<T>(string id) where T : DomainObject
        {
            return RedoIfError.Execute<T>(delegate
            {
                return DBSessionFactory.Instance.Load<T>(SupportClass.GetDAOConfigFile(typeof(T).Namespace), id);
            }, 2);
        }

        public static T Get<T>(string cacheKey, string id) where T : DomainObject
        {
            if (GetFromCache(cacheKey))
            {
                return (T)DAOCacheService.Get(cacheKey);
            }
            return PutToCachesAndReturn<T>(cacheKey, RedoIfError.Execute<T>(delegate
            {
                return DBSessionFactory.Instance.Load<T>(SupportClass.GetDAOConfigFile(typeof(T).Namespace), cacheKey, id);
            }, 2));
        }

        public static T Get<T>(T result, string id) where T : DomainObject
        {
            if (!((result != null) || string.IsNullOrEmpty(id)))
            {
                result = Get<T>(typeof(T).Name + "_" + id, id);
            }
            return result;
        }

        public static T Get<T>(string cacheKey, string id, Action<T> action) where T : DomainObject
        {
            if (GetFromCache(cacheKey))
            {
                return (T)DAOCacheService.Get(cacheKey);
            }
            return PutToCachesAndReturn<T>(cacheKey, RedoIfError.Execute<T>(delegate
            {
                return DBSessionFactory.Instance.Load<T>(SupportClass.GetDAOConfigFile(typeof(T).Namespace), cacheKey, id, action);
            }, 2));
        }

        private static bool GetFromCache(string cacheKey)
        {
            return (!string.IsNullOrEmpty(cacheKey) && DAOCacheService.Contains(cacheKey));
        }

        public static IList<T> List<T>(string cacheKey)
        {
            return List<T>(cacheKey, (string[])null, (string[])null, (string[])null);
        }

        public static IList<T> List<T>(string cacheKey, string[] orderBys)
        {
            return List<T>(cacheKey, null, null, orderBys);
        }

        public static IList<T> List<T>(string cacheKey, string sql)
        {
            c__DisplayClass1e<T> classe = new c__DisplayClass1e<T>();
            if (GetFromCache(cacheKey))
            {
                return (IList<T>)DAOCacheService.Get(cacheKey);
            }
            c__DisplayClass1e<T> cs__a8locals1f = classe;
            ISearch s = SearchFactory.GetSearch(sql);
            return PutToCachesAndReturn<IList<T>>(cacheKey, RedoIfError.Execute<IList<T>>(delegate
            {
                return DBSessionFactory.Instance.Query<T>(SupportClass.GetDAOConfigFile(typeof(T).Namespace), cs__a8locals1f.cacheKey, s);
            }, 2));
        }

        public static IList<T> List<T>(string cacheKey, int pageNumber, int pageSize)
        {
            return List<T>(cacheKey, (string[])null, (string[])null, (string[])null, pageNumber, pageSize);
        }

        public static IList List(string nameSpace, string multiKey, MutliSearch searchs)
        {
            RedoAction<IList> action = null;
            if (!GetFromCache(multiKey))
            {
                if (action == null)
                {
                    action = delegate
                    {
                        return DBSessionFactory.Instance.Query(SupportClass.GetDAOConfigFile(nameSpace), multiKey, searchs.CacheKeys.ToArray(), searchs.ToArray());
                    };
                }
                IList list = RedoIfError.Execute<IList>(action, 2);
                for (int i = 0; i < list.Count; i++)
                {
                    PutToCachesAndReturn<object>(searchs.CacheKeys[i], list[i]);
                }
                return PutToCachesAndReturn<IList>(multiKey, list);
            }
            return (DAOCacheService.Get(multiKey) as IList);
        }

        public static IList<T> List<T>(string nameSpace, string cacheKey, string queryName)
        {
            if (GetFromCache(cacheKey))
            {
                return (IList<T>)DAOCacheService.Get(cacheKey);
            }
            return PutToCachesAndReturn<IList<T>>(cacheKey, RedoIfError.Execute<IList<T>>(delegate
            {
                return DBSessionFactory.Instance.Query<T>(SupportClass.GetDAOConfigFile(nameSpace), cacheKey, queryName);
            }, 2));
        }

        public static IList List(string nameSpace, string cacheKey, string sql)
        {
            c__DisplayClass23 class3 = new c__DisplayClass23();
            if (GetFromCache(cacheKey))
            {
                return (IList)DAOCacheService.Get(cacheKey);
            }
            c__DisplayClass23 cs__a8locals24 = class3;
            ISearch s = SearchFactory.GetSearch(sql);
            return PutToCachesAndReturn<IList>(cacheKey, RedoIfError.Execute<IList>(delegate
            {
                return DBSessionFactory.Instance.Query(SupportClass.GetDAOConfigFile(cs__a8locals24.nameSpace), cs__a8locals24.cacheKey, s);
            }, 2));
        }

        public static ArrayList List<T, U, V, W>(string nameSpace, string cacheKey, params KeyValuePair<string, object[]>[] querys)
        {
            if (GetFromCache(cacheKey))
            {
                return (DAOCacheService.Get(cacheKey) as ArrayList);
            }
            return PutToCachesAndReturn<ArrayList>(cacheKey, RedoIfError.Execute<ArrayList>(delegate
            {
                return DBSessionFactory.Instance.Query<T, U, V, W>(SupportClass.GetDAOConfigFile(nameSpace), cacheKey, querys);
            }, 2));
        }

        public static IList<T> List<T>(string cacheKey, string[] names, string[] vals)
        {
            return List<T>(cacheKey, names, vals, null);
        }

        public static IList<T> List<T>(string cacheKey, string[] orderBys, int pageNumber, int pageSize)
        {
            return List<T>(cacheKey, (string[])null, (string[])null, orderBys, pageNumber, pageSize);
        }

        public static IList<T> List<T>(string cacheKey, string[] names, string[] vals, string[] orderBys)
        {
            return List<T>(cacheKey, names, vals, orderBys, (string[])null);
        }

        public static IList<T> List<T>(string nameSpace, string cacheKey, string queryName, object[] vals)
        {
            if (GetFromCache(cacheKey))
            {
                return (IList<T>)DAOCacheService.Get(cacheKey);
            }
            return PutToCachesAndReturn<IList<T>>(cacheKey, DBSessionFactory.Instance.Query<T>(SupportClass.GetDAOConfigFile(nameSpace), cacheKey, queryName, vals));
        }

        public static IList<T> List<T>(string nameSpace, string cacheKey, string queryName, int pageNumber, int pageSize)
        {
            if (GetFromCache(cacheKey))
            {
                return (IList<T>)DAOCacheService.Get(cacheKey);
            }
            return PutToCachesAndReturn<IList<T>>(cacheKey, RedoIfError.Execute<IList<T>>(delegate
            {
                return DBSessionFactory.Instance.Query<T>(SupportClass.GetDAOConfigFile(nameSpace), cacheKey, queryName, null, pageNumber, pageSize);
            }, 2));
        }

        public static IList<T> List<T>(string cacheKey, string[] names, string[] vals, int pageNumber, int pageSize)
        {
            return List<T>(cacheKey, names, vals, null, pageNumber, pageSize);
        }

        public static IList<T> List<T>(string nameSpace, string cacheKey, string queryName, object[] vals, Action<IList<T>> action)
        {
            if (GetFromCache(cacheKey))
            {
                return (IList<T>)DAOCacheService.Get(cacheKey);
            }
            return PutToCachesAndReturn<IList<T>>(cacheKey, RedoIfError.Execute<IList<T>>(delegate
            {
                return DBSessionFactory.Instance.Query<T>(SupportClass.GetDAOConfigFile(nameSpace), cacheKey, queryName, vals, action);
            }, 2));
        }

        public static IList<T> List<T>(string nameSpace, string cacheKey, string queryName, string[] names, object[] vals)
        {
            if (GetFromCache(cacheKey))
            {
                return (IList<T>)DAOCacheService.Get(cacheKey);
            }
            return PutToCachesAndReturn<IList<T>>(cacheKey, RedoIfError.Execute<IList<T>>(delegate
            {
                return DBSessionFactory.Instance.Query<T>(SupportClass.GetDAOConfigFile(nameSpace), cacheKey, queryName, names, vals);
            }, 2));
        }

        public static IList<T> List<T>(string cacheKey, string[] names, string[] vals, string[] orderBys, string[] fetchJoins)
        {
            c__DisplayClass14<T> class3 = new c__DisplayClass14<T>();
            if (GetFromCache(cacheKey))
            {
                return (IList<T>)DAOCacheService.Get(cacheKey);
            }
            c__DisplayClass14<T> cs__a8locals15 = class3;
            Type type = typeof(T);
            ISearch s = SearchFactory.GetSearch(type, names, vals, orderBys, fetchJoins);
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("将执行'{0}'获取'{1}'类型的集合...", new object[] { s.ToStatementString(), type.FullName });
            }
            return PutToCachesAndReturn<IList<T>>(cacheKey, RedoIfError.Execute<IList<T>>(delegate
            {
                return DBSessionFactory.Instance.Query<T>(SupportClass.GetDAOConfigFile(type.Namespace), cs__a8locals15.cacheKey, s);
            }, 2));
        }

        public static IList List(Type type, string cacheKey, string[] names, string[] vals, string[] orderBys)
        {
            c__DisplayClass19 class2 = new c__DisplayClass19();
            if (GetFromCache(cacheKey))
            {
                return (IList)DAOCacheService.Get(cacheKey);
            }
            c__DisplayClass19 cs__a8locals1a = class2;
            ISearch s = SearchFactory.GetSearch(type, names, vals, orderBys, null);
            return PutToCachesAndReturn<IList>(cacheKey, RedoIfError.Execute<IList>(delegate
            {
                return DBSessionFactory.Instance.Query(SupportClass.GetDAOConfigFile(type.Namespace), cs__a8locals1a.cacheKey, s);
            }, 2));
        }

        public static IList<T> List<T>(string nameSpace, string cacheKey, string queryName, object[] vals, int pageNumber, int pageSize)
        {
            if (GetFromCache(cacheKey))
            {
                return (IList<T>)DAOCacheService.Get(cacheKey);
            }
            return PutToCachesAndReturn<IList<T>>(cacheKey, RedoIfError.Execute<IList<T>>(delegate
            {
                return DBSessionFactory.Instance.Query<T>(SupportClass.GetDAOConfigFile(nameSpace), cacheKey, queryName, vals, pageNumber, pageSize);
            }, 2));
        }

        public static IList<T> List<T>(string nameSpace, string cacheKey, string queryName, string[] names, object[] vals, Action<IList<T>> action)
        {
            if (GetFromCache(cacheKey))
            {
                return (IList<T>)DAOCacheService.Get(cacheKey);
            }
            return PutToCachesAndReturn<IList<T>>(cacheKey, RedoIfError.Execute<IList<T>>(delegate
            {
                return DBSessionFactory.Instance.Query<T>(SupportClass.GetDAOConfigFile(nameSpace), cacheKey, queryName, names, vals, action);
            }, 2));
        }

        public static IList<T> List<T>(string cacheKey, string[] names, string[] vals, string[] orderBys, int pageNumber, int pageSize)
        {
            c__DisplayClass34<T> class3 = new c__DisplayClass34<T>();
            if (GetFromCache(cacheKey))
            {
                return (IList<T>)DAOCacheService.Get(cacheKey);
            }
            c__DisplayClass34<T> cs__a8locals35 = class3;
            Type type = typeof(T);
            ISearch s = SearchFactory.GetSearch(type, names, vals, orderBys, null);
            return PutToCachesAndReturn<IList<T>>(cacheKey, RedoIfError.Execute<IList<T>>(delegate
            {
                return DBSessionFactory.Instance.Query<T>(SupportClass.GetDAOConfigFile(type.Namespace), cs__a8locals35.cacheKey, s, cs__a8locals35.pageNumber, cs__a8locals35.pageSize);
            }, 2));
        }

        public static IList<T> List<T>(string nameSpace, string cacheKey, string queryName, string[] names, object[] vals, int pageNumber, int pageSize)
        {
            if (GetFromCache(cacheKey))
            {
                return (IList<T>)DAOCacheService.Get(cacheKey);
            }
            return PutToCachesAndReturn<IList<T>>(cacheKey, RedoIfError.Execute<IList<T>>(delegate
            {
                return DBSessionFactory.Instance.Query<T>(SupportClass.GetDAOConfigFile(nameSpace), cacheKey, queryName, names, vals, pageNumber, pageSize);
            }, 2));
        }

        public static IList<T> List<T>(string cacheKey, string select, string from, string joinFrom, string joinWhere, string where, string orderBy, string groupBy)
        {
            c__DisplayClass39<T> class2 = new c__DisplayClass39<T>();
            if (GetFromCache(cacheKey))
            {
                return (IList<T>)DAOCacheService.Get(cacheKey);
            }
            c__DisplayClass39<T> cs__a8locals3a = class2;
            Type type = typeof(T);
            ISearch s = SearchFactory.GetSearch(select, from, joinFrom, joinWhere, where, orderBy, groupBy);
            return PutToCachesAndReturn<IList<T>>(cacheKey, RedoIfError.Execute<IList<T>>(delegate
            {
                return DBSessionFactory.Instance.Query<T>(SupportClass.GetDAOConfigFile(type.Namespace), cs__a8locals3a.cacheKey, s);
            }, 2));
        }

        private static T PutToCachesAndReturn<T>(string key, T obj)
        {
            if ((!string.IsNullOrEmpty(key) && !DAOCacheService.Contains(key)) && (obj != null))
            {
                DAOCacheService.Put(key, obj);
            }
            return obj;
        }

        public static void Refresh<T>(T obj) where T : DomainObject
        {
            RedoIfError.Execute(delegate
            {
                DBSessionFactory.Instance.Refresh<T>(SupportClass.GetDAOConfigFile(typeof(T).Namespace), obj);
            }, 2);
        }

        public class MutliSearch : List<KeyValuePair<ISearch, Type>>
        {
            private List<string> cacheKeys = new List<string>();

            public QueryHelper.MutliSearch Add(Type type, string cacheKey, string[] names, string[] vals, string[] orderBys, string[] fetchJoins)
            {
                Add(new KeyValuePair<ISearch, Type>(SearchFactory.GetSearch(type, names, vals, orderBys, fetchJoins), type));
                this.cacheKeys.Add(cacheKey);
                return this;
            }

            public List<string> CacheKeys
            {
                get
                {
                    return this.cacheKeys;
                }
            }
        }
    }
}

