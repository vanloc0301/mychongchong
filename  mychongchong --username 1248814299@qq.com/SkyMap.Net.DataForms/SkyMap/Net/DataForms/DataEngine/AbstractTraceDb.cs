namespace SkyMap.Net.DataForms.DataEngine
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.DataForms;
    using SkyMap.Net.Security;
    using SkyMap.Net.Security.Principal;
    using System;
    using System.Data;
    using System.Text;

    public abstract class AbstractTraceDb : ITraceDb
    {
        protected string OpStaffId;
        protected string OpStaffName;
        private SkyMap.Net.DAO.SMDataSource smDS;
        protected DateTime traceTime;

        protected AbstractTraceDb()
        {
        }

        private void AddExtendPropToTable(DataTable Datatable, string PrimaryKeyName)
        {
            DataView view = new DataView(Datatable) {
                Sort = PrimaryKeyName + " DESC"
            };
            Datatable.ExtendedProperties.Add("MAX", view[0][PrimaryKeyName].ToString());
        }

        protected DataRow AddTraceRow(DbOpType type, string tablename, string keyValue)
        {
            DataRow row = this.TraceTable.NewRow();
            row[TraceUtil.FLD_TIMESTAMP] = this.traceTime;
            row[TraceUtil.FLD_STAFFFID] = this.OpStaffId;
            row[TraceUtil.FLD_STAFFNAME] = this.OpStaffName;
            row[TraceUtil.FLD_TYPE] = type;
            row[TraceUtil.FLD_TABLE] = tablename;
            row[TraceUtil.FLD_KEY] = keyValue;
            this.TraceTable.Rows.Add(row);
            return row;
        }

        protected void AddTraceRow(DbOpType type, string tablename, string keyValue, string description)
        {
            this.AddTraceRow(type, tablename, keyValue)[TraceUtil.FLD_MEMO] = description;
        }

        protected DataRow AddTraceRow(DbOpType type, string columnName, string tablename, string keyValue, object oleValue, object newValue)
        {
            DataRow row = this.AddTraceRow(type, tablename, keyValue);
            row[TraceUtil.FLD_FIELD] = columnName;
            row[TraceUtil.FLD_OLD] = oleValue;
            row[TraceUtil.FLD_NEW] = newValue;
            return row;
        }

        protected void AddTraceRow(DbOpType type, string columnName, string tablename, string keyValue, object oleValue, object newValue, string description)
        {
            this.AddTraceRow(type, columnName, tablename, keyValue, oleValue, newValue)[TraceUtil.FLD_MEMO] = description;
        }

        public static ITraceDb Create(SkyMap.Net.DAO.SMDataSource smDS)
        {
            ITraceDb db;
            if (TraceUtil.TraceType != string.Empty)
            {
                Type type = Type.GetType(TraceUtil.TraceType);
                if (type == null)
                {
                    throw new DataFormException("不能加类型：" + TraceUtil.TraceType);
                }
                db = Activator.CreateInstance(type) as ITraceDb;
            }
            else
            {
                db = new SQLTraceDb();
            }
            db.SMDataSource = smDS;
            return db;
        }

        public string GetTraceHistory(string table, string field, string keyValue)
        {
            StringBuilder builder = new StringBuilder(100);
            DataTable table2 = this.QueryTrace(table, field, keyValue);
            foreach (DataRow row in table2.Rows)
            {
                builder.Append("\n\r");
                builder.Append("  '").Append(row[TraceUtil.FLD_OLD].ToString()).Append("'->'").Append(row[TraceUtil.FLD_NEW].ToString()).Append("'");
                DateTime time = (DateTime) row[TraceUtil.FLD_TIMESTAMP];
                builder.Append(" ： 被'").Append(row[TraceUtil.FLD_STAFFNAME].ToString()).Append("'修改于").Append(time.ToString("yyyy-MM-dd:HH-mm-ss"));
            }
            if (builder.Length > 0)
            {
                builder.Insert(0, "修改日志：");
                return builder.ToString();
            }
            return string.Empty;
        }

        protected abstract DataTable QueryTrace(string table, string field, string keyValue);
        public abstract void SaveTraceToDb();
        private void TraceAdded(DataTable AddedTable, string TableName, string PrimaryKeyName)
        {
            foreach (DataRow row in AddedTable.Rows)
            {
                string keyValue = row[PrimaryKeyName].ToString();
                this.AddTraceRow(DbOpType.Added, TableName, keyValue, "新增");
            }
        }

        protected virtual void TraceDeleted(DataTable DeletedTable, string TableName, string PrimaryKeyName)
        {
            foreach (DataRow row in DeletedTable.Rows)
            {
                string keyValue = row[PrimaryKeyName, DataRowVersion.Original].ToString();
                this.AddTraceRow(DbOpType.Deleted, TableName, keyValue, "删除");
            }
        }

        public void TraceDsAfterSave(DataSet ds)
        {
            foreach (DataTable table2 in ds.Tables)
            {
                if (table2.ExtendedProperties["MAX"] != null)
                {
                    string columnName = table2.PrimaryKey[0].ColumnName;
                    DataRow[] rowArray = table2.Select(columnName + ">" + Convert.ToInt32(table2.ExtendedProperties["MAX"]));
                    if (rowArray.GetLength(0) > 0)
                    {
                        DataTable addedTable = table2.Clone();
                        foreach (DataRow row in rowArray)
                        {
                            addedTable.ImportRow(row);
                        }
                        this.TraceAdded(addedTable, table2.TableName, columnName);
                    }
                    table2.ExtendedProperties.Remove("MAX");
                }
            }
            ds.ExtendedProperties.Clear();
        }

        public void TraceDsChange(DataSet ds)
        {
            this.traceTime = DateTimeHelper.GetNow();
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("进行痕迹记录的时间是：{0}", new object[] { this.traceTime });
            }
            SmIdentity smIdentity = SecurityUtil.GetSmIdentity();
            this.OpStaffId = smIdentity.UserId;
            this.OpStaffName = smIdentity.UserName;
            foreach (DataTable table in ds.Tables)
            {
                this.TraceTableChange(table);
            }
        }

        private void TraceModified(DataTable ModifiedTable, string TableName, string PrimaryKeyName)
        {
            int num = ModifiedTable.Columns.Count - 1;
            foreach (DataRow row in ModifiedTable.Rows)
            {
                for (int i = 0; i <= num; i++)
                {
                    string columnName = ModifiedTable.Columns[i].ColumnName;
                    if ((columnName != PrimaryKeyName) && (TraceUtil.NotNeedTraceFields.IndexOf(columnName) < 0))
                    {
                        object oleValue = row[i, DataRowVersion.Original];
                        object obj3 = row[i, DataRowVersion.Current];
                        if (!oleValue.Equals(obj3))
                        {
                            string keyValue = row[PrimaryKeyName].ToString();
                            this.AddTraceRow(DbOpType.Modified, columnName, TableName, keyValue, oleValue, obj3, "修改");
                        }
                    }
                }
            }
        }

        public void TraceOuterAdd(string table, string keyValue, string description)
        {
            this.traceTime = DateTimeHelper.GetNow();
            SmIdentity smIdentity = SecurityUtil.GetSmIdentity();
            this.OpStaffId = smIdentity.UserId;
            this.OpStaffName = smIdentity.UserName;
            this.AddTraceRow(DbOpType.Added, table, keyValue, description);
        }

        public void TraceOuterAdd(string table, string keyValue, string field, string fldValue, string description)
        {
        }

        public void TraceOuterRemove(string[] tables, string[] keyValues, string description)
        {
            this.traceTime = DateTimeHelper.GetNow();
            SmIdentity smIdentity = SecurityUtil.GetSmIdentity();
            this.OpStaffId = smIdentity.UserId;
            this.OpStaffName = smIdentity.UserName;
            for (int i = 0; i < tables.Length; i++)
            {
                this.AddTraceRow(DbOpType.Deleted, tables[i], keyValues[i], description);
            }
        }

        private void TraceTableChange(DataTable DataTable)
        {
            DataTable changes;
            string columnName = DataTable.PrimaryKey[0].ColumnName;
            string tableName = DataTable.TableName;
            if (TraceUtil.CanTraceLevel(TraceLevel.Added))
            {
                changes = DataTable.GetChanges(DataRowState.Added);
                if (changes != null)
                {
                    if (!DataTable.PrimaryKey[0].AutoIncrement)
                    {
                        this.TraceAdded(changes, tableName, columnName);
                    }
                    else
                    {
                        this.AddExtendPropToTable(DataTable, columnName);
                    }
                }
            }
            if (TraceUtil.CanTraceLevel(TraceLevel.Modified))
            {
                changes = DataTable.GetChanges(DataRowState.Modified);
                if (changes != null)
                {
                    this.TraceModified(changes, tableName, columnName);
                }
            }
            if (TraceUtil.CanTraceLevel(TraceLevel.Deleted))
            {
                changes = DataTable.GetChanges(DataRowState.Deleted);
                if (changes != null)
                {
                    this.TraceDeleted(changes, tableName, columnName);
                }
            }
        }

        public SkyMap.Net.DAO.SMDataSource SMDataSource
        {
            get
            {
                return this.smDS;
            }
            set
            {
                this.smDS = value;
            }
        }

        protected abstract DataTable TraceTable { get; }
    }
}

