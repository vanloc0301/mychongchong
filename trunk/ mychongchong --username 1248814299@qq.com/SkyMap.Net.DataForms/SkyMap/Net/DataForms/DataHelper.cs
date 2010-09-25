namespace SkyMap.Net.DataForms
{
    using log4net;
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.DataForms.DataEngine;
    using SkyMap.Net.Security;
    using SkyMap.Net.SqlOM;
    using SkyMap.Net.SqlOM.Render;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;

    public class DataHelper
    {
        private static ILog log = LogManager.GetLogger(typeof(DataHelper));

        private static void AddUpdateTerm(UpdateTermCollection terms, UpdateTerm ut)
        {
            if (terms.Contains(ut.FieldName))
            {
                terms[ut.FieldName] = ut;
            }
            else
            {
                terms.Add(ut);
            }
        }

        private static string CreateInsertTableSQL(string[] keys, string[] fields, IList<string[]> vals, ITraceDb trace, bool canTraceAdd, DAODataTable ddt)
        {
            string str = null;
            if ((keys != null) || (keys.Length > 0))
            {
                int length = keys.Length;
                int num2 = fields.Length;
                for (int i = 0; i < length; i++)
                {
                    InsertQuery query = new InsertQuery(ddt.Name);
                    foreach (DAODataColumn column in ddt.DataColumns)
                    {
                        object val = null;
                        if (column.IsNeedInitial)
                        {
                            val = GetInitialValue(column.InitialValue);
                            LoggingService.DebugFormatted("初始化字段：{0} 的值为：{1}", new object[] { column.Name, val });
                        }
                        else if (column.Name.ToLower() == "createdate")
                        {
                            val = DateTimeHelper.GetNow();
                        }
                        if (val != null)
                        {
                            try
                            {
                                query.Terms.Add(new UpdateTerm(column.Name, GetSqlExpression(column.DataType, val)));
                                if (canTraceAdd)
                                {
                                    trace.TraceOuterAdd(ddt.Name, keys[i], column.Name, val.ToString(), "系统新增字段值记录");
                                }
                            }
                            catch (Exception exception)
                            {
                                LoggingService.ErrorFormatted("在设置表：{0}字段：{1}初始化值：{2}时出错：{3}\r\n{4}", new object[] { ddt.Name, column.Name, val, exception.Message, exception.StackTrace });
                            }
                        }
                    }
                    string[] strArray = vals[i];
                    UpdateTerm ut = new UpdateTerm(ddt.Key, SqlExpression.String(keys[i]));
                    AddUpdateTerm(query.Terms, ut);
                    if (canTraceAdd)
                    {
                        trace.TraceOuterAdd(ddt.Name, keys[i], "系统自动初始化数据新增记录");
                    }
                    if (fields != null)
                    {
                        string str2 = string.Empty;
                        for (int j = 0; j < num2; j++)
                        {
                            str2 = fields[j];
                            if (ddt.Contains(str2))
                            {
                                ut = new UpdateTerm(str2, SqlExpression.String(strArray[j]));
                                AddUpdateTerm(query.Terms, ut);
                                if (canTraceAdd)
                                {
                                    trace.TraceOuterAdd(ddt.Name, keys[i], str2, strArray[j], "系统新增字段值记录");
                                }
                            }
                        }
                    }
                    if (query.Terms.Count > 0)
                    {
                        str = SqlOmRenderHelper.Instance.RenderInsert(query);
                    }
                }
                return str;
            }
            log.Error("没有找到初始化记录的关键字值,不能正确初始化");
            return str;
        }

        public static void Delete(IDictionary<DAODataSet, string> deletes)
        {
            Exception exception;
            try
            {
                string str = string.Empty;
                DAODataSet key = null;
                List<string> list = new List<string>(deletes.Count);
                List<string> list2 = new List<string>(deletes.Count);
                foreach (KeyValuePair<DAODataSet, string> pair in deletes)
                {
                    key = pair.Key;
                    foreach (DAODataTable table in key.DAODataTables)
                    {
                        str = str + "delete from " + table.Name + " where ";
                        if (table.Level)
                        {
                            str = str + table.RelKey;
                        }
                        else
                        {
                            str = str + table.Key;
                            list.Add(table.Name);
                            list2.Add(pair.Value.ToString().Replace("'", ""));
                        }
                        str = str + " in (" + pair.Value + ");\r";
                    }
                }
                if (str != string.Empty)
                {
                    DBConnection connection = new DBConnection(key.DataSource);
                    connection.Open();
                    IDbTransaction transaction = connection.BeginTrasaction();
                    try
                    {
                        IDbCommand command = connection.Connection.CreateCommand();
                        command.Transaction = transaction;
                        command.CommandText = str;
                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQuery();
                        connection.CommitTransaction();
                    }
                    catch (Exception exception1)
                    {
                        exception = exception1;
                        connection.RollBackTransaction();
                        throw exception;
                    }
                    finally
                    {
                        connection.Close();
                    }
                    try
                    {
                        ITraceDb traceDB = GetTraceDB(key.DataSource);
                        if (TraceUtil.CanTraceLevel(TraceLevel.Deleted) && (traceDB != null))
                        {
                            traceDB.TraceOuterRemove(list.ToArray(), list2.ToArray(), "经由系统(可能是前台工作流完全删除)操作删除进行的记录,注意它并没有对子表的删除进行记录.");
                            traceDB.SaveTraceToDb();
                        }
                    }
                    catch (Exception exception2)
                    {
                        exception = exception2;
                        log.Error("进行删除的痕迹记录出错:", exception);
                    }
                }
            }
            catch (Exception exception3)
            {
                exception = exception3;
                throw new DataFormException("delete data raise error!", exception);
            }
        }

        private static void Execute(DBConnection dbcn, string insert, IDbTransaction tran)
        {
            LoggingService.DebugFormatted("将执行SQL:{0}", new object[] { insert });
            IDbCommand command = dbcn.Connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = insert;
            if (tran != null)
            {
                command.Transaction = tran;
            }
            command.ExecuteNonQuery();
        }

        private static object GetInitialValue(string val)
        {
            if (val == "{SYS:STAFFID}")
            {
                return SecurityUtil.GetSmIdentity().UserId;
            }
            if (val == "{SYS:STAFFNAME}")
            {
                return SecurityUtil.GetSmIdentity().UserName;
            }
            if (val == "{SYS:DEPTID}")
            {
                return SecurityUtil.GetSmPrincipal().DeptIds[0];
            }
            if (val == "{SYS:DEPTNAME}")
            {
                return SecurityUtil.GetSmPrincipal().DeptNames[0];
            }
            if (val == "{SYS:NOW}")
            {
                return DateTimeHelper.GetNow();
            }
            return val;
        }

        private static SqlExpression GetSqlExpression(string type, object val)
        {
            type = type.ToLower();
            if (type.IndexOf("int") >= 0)
            {
                return SqlExpression.Number(Convert.ToInt32(val));
            }
            if (type.IndexOf("bool") >= 0)
            {
                if (val.Equals(false))
                {
                    return SqlExpression.Number(0);
                }
                return SqlExpression.Number(1);
            }
            if (type.IndexOf("date") >= 0)
            {
                if (val is DateTime)
                {
                    return SqlExpression.Date((DateTime) val);
                }
                return SqlExpression.Date(Convert.ToDateTime(val));
            }
            return SqlExpression.String(Convert.ToString(val));
        }

        public static ITraceDb GetTraceDB(SMDataSource smDS)
        {
            if (!TraceUtil.CanTraceLevel(TraceLevel.None))
            {
                return AbstractTraceDb.Create(smDS);
            }
            return null;
        }

        public static void Initial(SMDataSource smDS, DAODataTable ddt)
        {
            Initial(smDS, ddt, null, null, new string[0]);
        }

        public static void Initial(DAODataTable ddt, DataRow row)
        {
            foreach (DAODataColumn column in ddt.DataColumns)
            {
                if (column.IsNeedInitial)
                {
                    row[column.Name] = GetInitialValue(column.InitialValue);
                }
            }
        }

        public static void Initial(SMDataSource smDS, DAODataTable ddt, string key)
        {
            Initial(smDS, ddt, new string[] { key }, null, null);
        }

        public static void Initial(SMDataSource smDS, DAODataTable ddt, string[] keys)
        {
            Initial(smDS, ddt, keys, null, null);
        }

        public static void Initial(SMDataSource smDS, DAODataTable ddt, string[] keys, string[] fields, IList<string[]> vals)
        {
            ITraceDb traceDB = GetTraceDB(smDS);
            bool canTraceAdd = TraceUtil.CanTraceLevel(TraceLevel.Added) && (traceDB != null);
            string str = CreateInsertTableSQL(keys, fields, vals, traceDB, canTraceAdd, ddt);
            if (!string.IsNullOrEmpty(str))
            {
                Exception exception;
                DBConnection dbcn = new DBConnection(smDS);
                dbcn.Open();
                try
                {
                    Execute(dbcn, str, null);
                    if (canTraceAdd)
                    {
                        try
                        {
                            traceDB.SaveTraceToDb();
                        }
                        catch (Exception exception1)
                        {
                            exception = exception1;
                            log.Error("系统对新增数据进行痕记录时出错:", exception);
                        }
                    }
                }
                catch (Exception exception2)
                {
                    exception = exception2;
                    throw new DataFormException("Initial data raise error", exception);
                }
                finally
                {
                    dbcn.Close();
                }
            }
        }

        public static void Initial(SMDataSource smDS, DAODataTable ddt, string key, string[] fields, string[] vals)
        {
            List<string[]> list = new List<string[]>(1) {
                vals
            };
            Initial(smDS, ddt, new string[] { key }, fields, list);
        }

        public static void Initial(SMDataSource smDS, IList<DAODataTable> ddts, bool onlyTopLevel, string[] keys, string[] fields, IList<string[]> vals)
        {
            Exception exception;
            ITraceDb traceDB = GetTraceDB(smDS);
            bool canTraceAdd = TraceUtil.CanTraceLevel(TraceLevel.Added) && (traceDB != null);
            StringBuilder builder = new StringBuilder();
            foreach (DAODataTable table in ddts)
            {
                if (!onlyTopLevel || !table.Level)
                {
                    string str = CreateInsertTableSQL(keys, fields, vals, traceDB, canTraceAdd, table);
                    if (!string.IsNullOrEmpty(str))
                    {
                        builder.Append(str).Append("\r\n");
                    }
                }
            }
            if (builder.Length > 0)
            {
                DBConnection dbcn = new DBConnection(smDS);
                dbcn.Open();
                try
                {
                    Execute(dbcn, builder.ToString(), null);
                }
                catch (Exception exception1)
                {
                    exception = exception1;
                    throw new DataFormException("Initial data raise error", exception);
                }
                finally
                {
                    dbcn.Close();
                }
            }
            if (canTraceAdd)
            {
                try
                {
                    traceDB.SaveTraceToDb();
                }
                catch (Exception exception2)
                {
                    exception = exception2;
                    log.Error("系统对新增数据进行痕记录时出错:", exception);
                }
            }
        }
    }
}

