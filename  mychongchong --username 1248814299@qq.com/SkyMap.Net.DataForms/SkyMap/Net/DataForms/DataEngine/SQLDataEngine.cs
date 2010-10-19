namespace SkyMap.Net.DataForms.DataEngine
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.DataForms;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;
    using System.Threading;
    using System.Xml.Serialization;

    [Serializable, SoapType]
    public class SQLDataEngine : MarshalByRefObject, IDataEngine
    {
        private void BuildLowLevelSyncSQL(StringBuilder sb, int exitRows, DataTable dtDataSource, DAODataTable daoDataTable, string relKeyValue)
        {
            int count = dtDataSource.Rows.Count;
            if (count != 0)
            {
                int num3;
                DataRow row;
                int num2 = dtDataSource.Columns.Count;
                if (exitRows == 0)
                {
                    int num4;
                    string str = string.Format("insert {0}({1}", daoDataTable.Name, daoDataTable.RelKey);
                    string format = string.Format("values('{0}'", relKeyValue);
                    for (num3 = 0; num3 < num2; num3++)
                    {
                        str = str + "," + dtDataSource.Columns[num3].ColumnName;
                        format = format + ",'{" + num3.ToString() + "}'";
                    }
                    str = str + ")";
                    if (count > 1)
                    {
                        for (num3 = 0; num3 < count; num3++)
                        {
                            object[] itemArray = dtDataSource.Rows[num3].ItemArray;
                            num4 = 0;
                            while (num4 < itemArray.Length)
                            {
                                itemArray[num4] = itemArray[num4].ToString().Replace("'", "''");
                                num4++;
                            }
                            sb.Append(str).AppendFormat(format, itemArray).AppendFormat(");\n", new object[0]);
                        }
                    }
                    else
                    {
                        row = dtDataSource.Rows[0];
                        List<string[]> list = new List<string[]>();
                        for (num3 = 0; num3 < num2; num3++)
                        {
                            string[] strArray = row[num3].ToString().Split(new char[] { '\\' });
                            for (num4 = 0; num4 < strArray.Length; num4++)
                            {
                                if (num3 == 0)
                                {
                                    string[] item = new string[num2];
                                    list.Add(item);
                                }
                                if (list.Count > num4)
                                {
                                    //list[num4][num3] = (string[]) strArray[num4].Replace("'", "''");
                                    list[num4][num3] = strArray[num4].Replace("'", "''");
                                }
                            }
                        }
                        foreach (string[] strArray2 in list)
                        {
                            sb.AppendFormat(str, new object[0]).AppendFormat(format, (object[]) strArray2).AppendFormat(");\n", new object[0]);
                        }
                    }
                }
                else if (exitRows == 1)
                {
                    sb.AppendFormat("update {0} set ", daoDataTable.Name);
                    row = dtDataSource.Rows[0];
                    string str3 = string.Empty;
                    for (num3 = 0; num3 < num2; num3++)
                    {
                        if (num3 > 0)
                        {
                            sb.Append(",");
                        }
                        string columnName = dtDataSource.Columns[num3].ColumnName;
                        sb.AppendFormat("{0}='{1}'", columnName, row[num3].ToString().Replace("'", "''"));
                        str3 = str3 + string.Format(" and ({0} is null or {0}='')", columnName);
                    }
                    sb.AppendFormat("where {0}='{1}'{2};", daoDataTable.RelKey, relKeyValue, str3);
                }
            }
        }

        private DAODataForm GetDAODataForm(string dataFormID)
        {
            return QueryHelper.Get<DAODataForm>("DAODataForm_" + dataFormID, dataFormID);
        }

        private DataTable GetDataTable(DAODataTable DAODataTable, string keyValue, DBConnection dbcn)
        {
            DataTable dt = new DataTable(DAODataTable.Name);
            string selectString = this.GetSelectString(DAODataTable, keyValue);
            dbcn.ExecuteSql(dt, selectString);
            dt.PrimaryKey = new DataColumn[] { dt.Columns[DAODataTable.Key] };
            if (DAODataTable.Level)
            {
                dt.Columns[DAODataTable.Key].AutoIncrement = true;
                dt.Columns[DAODataTable.RelKey].DefaultValue = keyValue;
            }
            else
            {
                dt.Columns[DAODataTable.Key].DefaultValue = keyValue;
            }
            dt.ExtendedProperties.Add("selectsql", selectString);
            return dt;
        }

        public virtual DataSet GetDs(string dataFormID, string keyValue)
        {
            DataSet set2;
            DAODataForm dAODataForm = this.GetDAODataForm(dataFormID);
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("准备获取表单：'{0}',键值：'{1}'的业务表单数据!", new object[] { dAODataForm.Name, keyValue });
            }
            if (dAODataForm == null)
            {
                throw new DataFormException("没有找到数据表单配置信息!");
            }
            DataSet set = new DataSet(dAODataForm.Name);
            DBConnection dbcn = new DBConnection(dAODataForm.DataSource);
            dbcn.Open();
            try
            {
                this.SyncDataSouce(keyValue, dAODataForm, dbcn);
                foreach (DAODataTable table in dAODataForm.BindTables)
                {
                    if (!set.Tables.Contains(table.Name))
                    {
                        set.Tables.Add(this.GetDataTable(table, keyValue, dbcn));
                    }
                }
                set.AcceptChanges();
                set2 = set;
            }
            catch (ApplicationException exception)
            {
                throw exception;
            }
            finally
            {
                dbcn.Close();
            }
            return set2;
        }

        private string GetSelectString(DAODataTable table, string keyValue)
        {
            string str3;
            string str = "select * from " + table.Name;
            if (!table.Level)
            {
                str3 = str;
                return (str3 + " where " + table.Key + "='" + keyValue + "'");
            }
            str3 = str;
            return (str3 + " where " + table.RelKey + "='" + keyValue + "' order by " + table.Key);
        }

        private DataTable GetSyncDataTable(string p, string project_id)
        {
            int index = p.IndexOf(':');
            p = p.Replace("{PROJECT_ID}", project_id);
            if (index > 0)
            {
                return QueryHelper.ExecuteSql(p.Substring(0, index), string.Empty, p.Substring(index + 1));
            }
            return QueryHelper.ExecuteSql("Default", string.Empty, p);
        }

        private object GetSyncValue(string p, string project_id)
        {
            int index = p.IndexOf(':');
            p = p.Replace("{PROJECT_ID}", project_id);
            if (index > 0)
            {
                return QueryHelper.ExecuteSqlScalar(p.Substring(0, index), p.Substring(index + 1));
            }
            return QueryHelper.ExecuteScalar("Default", p);
        }

        private ITraceDb GetTraceDb(SMDataSource smDS)
        {
            return AbstractTraceDb.Create(smDS);
        }

        public string GetTraceHistory(string smDSOID, string table, string field, string keyValue)
        {
            if (TraceUtil.ShowTraceHistory)
            {
                SMDataSource smDS = QueryHelper.Get<SMDataSource>("SMDataSource_" + smDSOID, smDSOID);
                return this.GetTraceDb(smDS).GetTraceHistory(table, field, keyValue);
            }
            return string.Empty;
        }

        public DataSet RefreshDataset(SMDataSource smDS, DataSet ds)
        {
            DBConnection connection = new DBConnection(smDS);
            connection.Open();
            try
            {
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    dt.Rows.Clear();
                    string sql = (string) dt.ExtendedProperties["selectsql"];
                    connection.ExecuteSql(dt, sql);
                }
            }
            finally
            {
                connection.Close();
            }
            return ds;
        }

        public DataTable RefreshDataset(SMDataSource smDS, DataTable dt)
        {
            DBConnection connection = new DBConnection(smDS);
            connection.Open();
            try
            {
                DataTable table = dt;
                table.Rows.Clear();
                string sql = (string) table.ExtendedProperties["selectsql"];
                connection.ExecuteSql(table, sql);
            }
            finally
            {
                connection.Close();
            }
            return dt;
        }

        private void SaveAs(SMDataSource smDataSource, DataSet ds)
        {
            DBConnection connection = new DBConnection(smDataSource);
            using (smDataSource.CreateConnection())
            {
                foreach (DataTable table in ds.Tables)
                {
                    string strSelect = table.ExtendedProperties["selectsql"].ToString().ToLower();
                    string sql = "delete" + strSelect.Substring(strSelect.IndexOf(" from "));
                    if (sql.IndexOf(" where ") > 0)
                    {
                        LoggingService.DebugFormatted("将行删除语句:{0}", new object[] { sql });
                        int num = connection.ExecuteNonQuery(sql);
                        LoggingService.DebugFormatted("共删除了{0}行数据", new object[] { num });
                    }
                    DbDataAdapter adapter = connection.GetDataAdapter(strSelect, true, true, true);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    LoggingService.InfoFormatted("将异步另存{0}行数据...", new object[] { table.Rows.Count });
                    if (dataTable.Rows.Count == 0)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            DataRow row2 = dataTable.NewRow();
                            row2.ItemArray = row.ItemArray;
                            dataTable.Rows.Add(row2);
                        }
                        adapter.Update(dataTable);
                    }
                    else
                    {
                        LoggingService.WarnFormatted("另存数据前没有删除原来的{0}行数据,请检查", new object[] { dataTable.Rows.Count });
                    }
                }
                if (ds.ExtendedProperties.ContainsKey("CascadeSql"))
                {
                    string str3 = (string) ds.ExtendedProperties["CascadeSql"];
                    if (!string.IsNullOrEmpty(str3))
                    {
                        try
                        {
                            connection.ExecuteNonQuery(str3);
                        }
                        catch (Exception exception)
                        {
                            LoggingService.Error(exception, "执行级联更新:{0}时出错", new object[] { str3 });
                        }
                    }
                }
            }
        }

        private void SaveAsTos(IList<SMDataSource> smDataSources, DataSet ds)
        {
            WaitCallback callBack = delegate (object sender) {
                foreach (SMDataSource source in smDataSources)
                {
                    try
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            try
                            {
                                this.SaveAs(source, ds);
                                goto Label_00C4;
                            }
                            catch (Exception exception)
                            {
                                LoggingService.Error(string.Format("第{0}次尝试另存数据集{1}时发生错误:", j, ds.Tables[0].ExtendedProperties["selectsql"]), exception);
                            }
                        }
                    }
                    finally
                    {
                        LoggingService.InfoFormatted("异步另存{0}完成...", new object[] { ds.Tables[0].ExtendedProperties["selectsql"] });
                    }
                Label_00C4:;
                }
            };
            try
            {
                LoggingService.Info("开始异步执行另存...");
                ThreadPool.QueueUserWorkItem(callBack);
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
            }
        }

        public DataTable SaveData(SMDataSource smDS, DataTable dt)
        {
            DataTable table;
            try
            {
                this.SaveDataToSQlDB(smDS, ref dt);
                table = dt;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return table;
        }

        public void SaveData(SMDataSource smDS, DataSet dt)
        {
            this.SaveDataToSQlDB(smDS, ref dt);
        }

        public DataSet SaveData(string dataFormID, DataSet ds)
        {
            DataSet set;
            try
            {
                bool flag = !TraceUtil.CanTraceLevel(TraceLevel.None);
                ITraceDb traceDb = null;
                DAODataForm dAODataForm = this.GetDAODataForm(dataFormID);
                if ((dAODataForm.SaveAsTos != null) && (dAODataForm.SaveAsTos.Count > 0))
                {
                    this.SaveAsTos(dAODataForm.SaveAsTos, ds.Copy());
                }
                if (flag)
                {
                    traceDb = this.GetTraceDb(dAODataForm.DataSource);
                    traceDb.TraceDsChange(ds);
                }
                this.SaveDataToSQlDB(dAODataForm.DataSource, ref ds);
                if (flag)
                {
                    traceDb.TraceDsAfterSave(ds);
                    traceDb.SaveTraceToDb();
                }
                set = this.RefreshDataset(dAODataForm.DataSource, ds);
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
                throw new DataFormException("业务数据保存不成功", exception);
            }
            return set;
        }

        private void SaveDataToSQlDB(SMDataSource smDS, ref DataSet ds)
        {
            int count = ds.Tables.Count;
            DBConnection connection = new DBConnection(smDS);
            connection.Open();
            string str = null;
            if (ds.ExtendedProperties.ContainsKey("CascadeSql"))
            {
                str = (string) ds.ExtendedProperties["CascadeSql"];
            }
            try
            {
                IDbTransaction transaction = connection.BeginTrasaction();
                for (int i = 0; i < count; i++)
                {
                    DataTable dataTable = ds.Tables[i];
                    if (dataTable.GetChanges() != null)
                    {
                        string strSelect = (string) dataTable.ExtendedProperties["selectsql"];
                        DbDataAdapter adapter = connection.GetDataAdapter(strSelect, true, true, true);
                        try
                        {
                            adapter.Update(dataTable);
                        }
                        catch (DBConcurrencyException exception)
                        {
                            LoggingService.Error("数据可能被其他人修改了，不能保存...");
                            throw exception;
                        }
                    }
                }
                connection.CommitTransaction();
                try
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        if (LoggingService.IsDebugEnabled)
                        {
                            LoggingService.DebugFormatted("将执行级联SQL:{0}", new object[] { str });
                        }
                        connection.ExecuteSql(str);
                    }
                }
                catch (Exception exception2)
                {
                    LoggingService.ErrorFormatted("执行级联更新语句：{0}，发生错语：{1}\r\n{2}", new object[] { str, exception2.Message, exception2.StackTrace });
                }
            }
            catch (Exception exception3)
            {
                connection.RollBackTransaction();
                throw exception3;
            }
            finally
            {
                connection.Close();
            }
        }

        private void SaveDataToSQlDB(SMDataSource smDS, ref DataTable dt)
        {
            DBConnection connection = new DBConnection(smDS);
            connection.Open();
            try
            {
                IDbTransaction transaction = connection.BeginTrasaction();
                DataTable dataTable = dt;
                if (dataTable.GetChanges() != null)
                {
                    string strSelect = (string) dataTable.ExtendedProperties["selectsql"];
                    DbDataAdapter adapter = connection.GetDataAdapter(strSelect, true, true, true);
                    try
                    {
                        adapter.Update(dataTable);
                    }
                    catch (DBConcurrencyException exception)
                    {
                        throw new DataFormException(exception.Message, exception);
                    }
                }
                connection.CommitTransaction();
            }
            catch (Exception exception2)
            {
                connection.RollBackTransaction();
                throw exception2;
            }
            finally
            {
                connection.Close();
            }
            dt = this.RefreshDataset(smDS, dt);
        }

        private void SyncDataSouce(string keyValue, DAODataForm form, DBConnection dbcn)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                foreach (DataControl control in form.DataControls)
                {
                    if (!string.IsNullOrEmpty(control.ValueDataSource))
                    {
                        if (control.MapColumn != null)
                        {
                            object syncValue = this.GetSyncValue(control.ValueDataSource, keyValue);
                            LoggingService.DebugFormatted("将同步业务:{0} 字段:{1} 到值：{2}", new object[] { keyValue, control.MapColumn.Name, syncValue });
                            if (!((syncValue == null) || Convert.IsDBNull(syncValue)))
                            {
                                sb.AppendFormat("update {0} set {1}='{2}' where {3}='{4}' and ({1} is null or {1}!='{2}');\r\n", new object[] { control.MapTable.Name, control.MapColumn.Name, syncValue.ToString().Replace("'", "''"), control.MapTable.Key, keyValue });
                            }
                        }
                        else
                        {
                            int? nullable = dbcn.ExecuteSql<int?>(string.Format("select count(1) from {0} where {1}='{2}'", control.MapTable.Name, control.MapTable.RelKey, keyValue));
                            if (!(nullable.HasValue && (nullable.Value > 1)))
                            {
                                DataTable syncDataTable = this.GetSyncDataTable(control.ValueDataSource, keyValue);
                                this.BuildLowLevelSyncSQL(sb, nullable.Value, syncDataTable, control.MapTable, keyValue);
                            }
                        }
                    }
                }
                if (sb.Length > 0)
                {
                    LoggingService.DebugFormatted("将执行SQL语句：{0}", new object[] { sb.ToString() });
                    dbcn.ExecuteSql(sb.ToString());
                }
            }
            catch (Exception exception)
            {
                LoggingService.Error("执行同步数据时出错：", exception);
            }
        }
    }
}

