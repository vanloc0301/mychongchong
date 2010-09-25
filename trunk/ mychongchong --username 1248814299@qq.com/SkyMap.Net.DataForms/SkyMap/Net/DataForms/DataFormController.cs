namespace SkyMap.Net.DataForms
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Security;
    using SkyMap.Net.Security.Principal;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.IO;
    using System.Threading;

    public class DataFormController
    {
        private static string baseDir = null;
        private BackgroundWorker bgWorker;
        protected SkyMap.Net.DataForms.DAODataForm daoDataForm;
        protected Dictionary<string, object> dataFormParams = new Dictionary<string, object>(6);
        private DataSet ds;
        private const string fldCreateDate = "CreateDate";
        private const string fldUpdateDate = "UpdateDate";
        protected FormPermission formPermission;
        private object lockThis = new object();

        public void AddParams(string key, object val)
        {
            if (this.dataFormParams.ContainsKey(key))
            {
                this.dataFormParams[key] = val;
                if (key == "ProjectId")
                {
                    this.TryEnter();
                    this.ds = null;
                    this.bgWorker = null;
                    this.AsyncLoadDataSet();
                }
            }
            else
            {
                this.dataFormParams.Add(key, val);
            }
        }

        protected virtual void AfterSave()
        {
        }

        private void AsyncLoadDataSet()
        {
            if ((((this.bgWorker == null) && (this.ds == null)) && !string.IsNullOrEmpty(this.GetKeyValue())) && (this.daoDataForm != null))
            {
                this.bgWorker = new BackgroundWorker();
                this.bgWorker.WorkerReportsProgress = true;
                this.bgWorker.DoWork += new DoWorkEventHandler(this.bgWorker_DoWork);
                this.bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.bgWorker_RunWorkerCompleted);
                this.bgWorker.RunWorkerAsync();
            }
        }

        public void BeginEdit()
        {
            foreach (DataTable table in this.DataSource.Tables)
            {
                if ((table.Rows.Count > 0) && (table.Rows[0].RowState != DataRowState.Deleted))
                {
                    table.Rows[0].BeginEdit();
                }
            }
        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            LoggingService.Info("开始异步获取表单数据集...");
            lock (this.lockThis)
            {
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.InfoFormatted("正准备打开：{0}!", new object[] { this.GetKeyValue() });
                }
                this.ds = this.DataEngine.GetDs(this.daoDataForm.Id, this.GetKeyValue());
                if (this.ds == null)
                {
                    LoggingService.InfoFormatted("获取数据集错误:{0}", new object[] { this.GetKeyValue() });
                }
                foreach (DAODataTable table in this.daoDataForm.BindTables)
                {
                    if (LoggingService.IsInfoEnabled)
                    {
                        LoggingService.InfoFormatted("检查是否需要新建数据！", new object[0]);
                    }
                    if (!table.Level && this.DataSource.Tables.Contains(table.Name))
                    {
                        if (this.ds.Tables[table.Name].Rows.Count == 0)
                        {
                            DataRow row = this.ds.Tables[table.Name].NewRow();
                            if (row.Table.Columns.Contains("PROINST_ID") && this.dataFormParams.ContainsKey("ProinsId"))
                            {
                                row["PROINST_ID"] = this.dataFormParams["ProinsId"];
                            }
                            this.ds.Tables[table.Name].Rows.Add(row);
                            DataHelper.Initial(table, row);
                        }
                        else if (this.ds.Tables[table.Name].Rows[0][table.Key].ToString() != this.GetKeyValue())
                        {
                            throw new ApplicationException("获取的数据集有错误，需要重启应用服务器！");
                        }
                    }
                }
            }
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                LoggingService.Error(e.Error);
                MessageService.ShowError("异步获取表单数据集时发生错误:" + e.Error.Message);
            }
            if (LoggingService.IsDebugEnabled)
            {
                string path = FileUtility.Combine(new string[] { FileUtility.ApplicationRootPath, "Temp" });
                if (File.Exists(path))
                {
                    this.ds.WriteXml(FileUtility.Combine(new string[] { path, this.GetKeyValue() + ".xml" }));
                }
                else
                {
                    LoggingService.DebugFormatted("不存在应用程序临时目录,不会输出数据集的XML文件,如果需要,请建立目录{0}!", new object[] { path });
                }
            }
            LoggingService.Info("异步加载数集完成");
        }

        public virtual void BindData(IDataForm dataForm)
        {
            this.TryEnter();
            if (this.DataEngine == null)
            {
                throw new DataFormException("DataEngine cannot be null");
            }
            if (this.ds != null)
            {
                this.BeginEdit();
                foreach (DataControl control in this.daoDataForm.DataControls)
                {
                    if (!(((control.MapColumn == null) && (control.MapTable == null)) && string.IsNullOrEmpty(control.ValueList)))
                    {
                        string memberName = (control.MapColumn != null) ? control.MapColumn.Name : string.Empty;
                        dataForm.BindDataToControl(control.Name, (control.MapTable != null) ? this.ds.Tables[control.MapTable.Name] : null, memberName, control.ValueCollection);
                    }
                }
            }
            else
            {
                LoggingService.WarnFormatted("异步线程没有获取到数据集", new object[0]);
            }
        }

        public static DataFormController Create()
        {
            string dataControllerTypeName = SupportClass.GetDataControllerTypeName();
            if ((dataControllerTypeName != null) && (dataControllerTypeName.Length > 0))
            {
                Type type = Type.GetType(dataControllerTypeName);
                if (type != null)
                {
                    return (Activator.CreateInstance(type) as DataFormController);
                }
            }
            return new DataFormController();
        }

        public string GetFailSavedFile()
        {
            string path = FileUtility.Combine(new string[] { PropertyService.DataDirectory, "dataformunsaved" });
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return FileUtility.Combine(new string[] { path, this.GetKeyValue() + ".ds" });
        }

        protected virtual string GetKeyValue()
        {
            string str = string.Empty;
            if (this.dataFormParams.ContainsKey("ProjectId"))
            {
                str = (string) this.dataFormParams["ProjectId"];
            }
            if (string.IsNullOrEmpty(str))
            {
                throw new DataFormException("The key value cannot be null");
            }
            return str;
        }

        public object GetParamValue(string paramName, object defaultValue)
        {
            if (this.dataFormParams.ContainsKey(paramName))
            {
                return this.dataFormParams[paramName];
            }
            return defaultValue;
        }

        public string GetTraceHistory(string tableName, string fieldName, string keyValue)
        {
            return this.DataEngine.GetTraceHistory(this.daoDataForm.DataSource.Id, tableName, fieldName, keyValue);
        }

        public virtual void InitDataFormParams()
        {
            SmPrincipal smPrincipal = SecurityUtil.GetSmPrincipal();
            if (smPrincipal != null)
            {
                SmIdentity identity = smPrincipal.Identity as SmIdentity;
                if (smPrincipal.DeptIds.Length > 0)
                {
                    this.AddParams("DeptId", smPrincipal.DeptIds[0]);
                    this.AddParams("DeptName", smPrincipal.DeptNames[0]);
                }
                this.AddParams("StaffId", identity.UserId);
                this.AddParams("StaffName", identity.UserName);
            }
        }

        public virtual string ReplaceSqlParams(string sql)
        {
            string[] strArray = new string[] { "ActdefId", "ActinsId", "DeptId", "DeptName", "PageIndex", "ProinsId", "ProjectId", "StaffId", "StaffName" };
            string str = sql;
            foreach (string str2 in strArray)
            {
                string str3 = "{" + str2 + "}";
                if (sql.IndexOf(str3) >= 0)
                {
                    str = str.Replace(str3, this.GetParamValue(str2, string.Empty).ToString());
                }
            }
            return str;
        }

        public virtual void RestoreUnsavedData()
        {
            if (File.Exists(this.GetFailSavedFile()))
            {
                DataSet set = new DataSet();
                set.ReadXml(this.GetFailSavedFile());
                foreach (DataTable table in set.Tables)
                {
                    if (this.ds.Tables.Contains(table.TableName))
                    {
                        DataTable table2 = this.ds.Tables[table.TableName];
                        if (table2.PrimaryKey.Length <= 0)
                        {
                            throw new DataFormException(string.Format("表：{0}找不到主键，不能定位还行的数据行.", table.TableName));
                        }
                        string columnName = table2.PrimaryKey[0].ColumnName;
                        if (!table.Columns.Contains(columnName))
                        {
                            throw new DataFormException(string.Format("本地保存的数据表：{0}找不到主键{1}，不能定位还行的数据行.", table.TableName, columnName));
                        }
                        foreach (DataRow row in table.Rows)
                        {
                            DataRow[] rowArray = table2.Select(string.Format("{0}='{1}'", columnName, row[columnName]));
                            DataRow row2 = null;
                            if (rowArray.Length > 0)
                            {
                                row2 = rowArray[0];
                            }
                            else
                            {
                                row2 = this.ds.Tables[table.TableName].NewRow();
                            }
                            foreach (DataColumn column in table.Columns)
                            {
                                if (!(!table2.Columns.Contains(column.ColumnName) || table2.Columns[column.ColumnName].AutoIncrement))
                                {
                                    row2[column.ColumnName] = row[column.ColumnName];
                                }
                            }
                            if (rowArray.Length == 0)
                            {
                                this.ds.Tables[0].Rows.Add(row2);
                            }
                        }
                    }
                }
            }
        }

        public virtual bool Save()
        {
            bool flag;
            try
            {
                this.TryEnter();
                foreach (DataTable table in this.ds.Tables)
                {
                    if (table.Columns.Contains("CreateDate") && table.Columns.Contains("UpdateDate"))
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            if (Convert.IsDBNull(row["CreateDate"]))
                            {
                                row["CreateDate"] = DateTimeHelper.GetNow();
                            }
                            else
                            {
                                row["UpdateDate"] = DateTimeHelper.GetNow();
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(this.DAODataForm.CascadeSql))
                {
                    if (this.ds.ExtendedProperties.ContainsKey("CascadeSql"))
                    {
                        this.ds.ExtendedProperties.Remove("CascadeSql");
                    }
                    this.ds.ExtendedProperties.Add("CascadeSql", this.ReplaceSqlParams(this.DAODataForm.CascadeSql));
                }
                this.ds = this.DataEngine.SaveData(this.daoDataForm.Id, this.ds);
                this.ds.AcceptChanges();
                this.AfterSave();
                try
                {
                    if (File.Exists(this.GetFailSavedFile()))
                    {
                        File.Delete(this.GetFailSavedFile());
                    }
                }
                catch (Exception exception)
                {
                    LoggingService.Error(exception);
                }
                flag = true;
            }
            catch (Exception exception2)
            {
                this.ds.WriteXml(this.GetFailSavedFile());
                throw exception2;
            }
            return flag;
        }

        public virtual void SetFormPermission(FormPermission formPermission, SkyMap.Net.DataForms.DAODataForm ddf, bool saveEnable)
        {
            this.formPermission = formPermission;
            this.daoDataForm = ddf;
            this.AsyncLoadDataSet();
        }

        public virtual void SetPropertys()
        {
            this.InitDataFormParams();
        }

        private void TryEnter()
        {
            if (this.bgWorker.IsBusy)
            {
                Thread.Sleep(5);
            }
            Monitor.Enter(this.lockThis);
            Monitor.Exit(this.lockThis);
        }

        public static string BaseDir
        {
            get
            {
                if (baseDir == null)
                {
                    baseDir = SupportClass.GetDataFormBaseDir();
                }
                return baseDir;
            }
        }

        public SkyMap.Net.DataForms.DAODataForm DAODataForm
        {
            get
            {
                return this.daoDataForm;
            }
        }

        protected IDataEngine DataEngine
        {
            get
            {
                return DataEngineFactory.CreateInstance();
            }
        }

        public DataSet DataSource
        {
            get
            {
                this.TryEnter();
                if (this.ds == null)
                {
                    throw new CoreException("无法获取表单数据，这可能是网络连接问题，请关闭表单，稍后再试！");
                }
                return this.ds;
            }
        }

        public virtual bool IsChanged
        {
            get
            {
                bool flag;
                try
                {
                    this.TryEnter();
                    if (this.ds.GetChanges() != null)
                    {
                        if (this.ds.HasErrors)
                        {
                            throw new ApplicationException("你输入的数据中有错误!");
                        }
                        return true;
                    }
                    flag = false;
                }
                catch (Exception exception)
                {
                    throw exception;
                }
                return flag;
            }
        }

        public Dictionary<string, string> SqlParams
        {
            get
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                string[] strArray = new string[] { "ActdefId", "ActinsId", "DeptId", "DeptName", "PageIndex", "ProinsId", "ProjectId", "StaffId", "StaffName" };
                foreach (string str in strArray)
                {
                    string key = "{" + str + "}";
                    dictionary.Add(key, this.GetParamValue(str, string.Empty).ToString());
                }
                return dictionary;
            }
        }
    }
}

