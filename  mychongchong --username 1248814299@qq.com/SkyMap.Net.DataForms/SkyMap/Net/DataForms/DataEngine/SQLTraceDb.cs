namespace SkyMap.Net.DataForms.DataEngine
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DataForms;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;

    public class SQLTraceDb : AbstractTraceDb
    {
        private DataTable _RecDelTable;
        private DataTable _traceTable;

        private void AddRecDelRow(string tablename, string keyValue, byte[] deleted)
        {
            DataRow row = this._RecDelTable.NewRow();
            row[TraceUtil.FLD_TIMESTAMP] = this.traceTime.ToString("u", DateTimeFormatInfo.InvariantInfo);
            row[TraceUtil.FLD_TABLE] = tablename;
            row[TraceUtil.FLD_KEY] = keyValue;
            row[TraceUtil.FLD_DELETED] = deleted;
            row[TraceUtil.FLD_STAFFFID] = base.OpStaffId;
            row[TraceUtil.FLD_STAFFNAME] = base.OpStaffName;
            this._RecDelTable.Rows.Add(row);
        }

        private DataTable GetTable(string TableName, DBConnection dbcn)
        {
            DataTable dataTable = new DataTable(TableName);
            string strSelect = "select * from " + TableName + " where 1<>1";
            DbDataAdapter adapter = dbcn.GetDataAdapter(strSelect, true, false, false);
            adapter.Fill(dataTable);
            dataTable.ExtendedProperties.Add("da", adapter);
            return dataTable;
        }

        protected override DataTable QueryTrace(string table, string field, string keyValue)
        {
            DataTable table3;
            StringBuilder builder = new StringBuilder(100);
            builder.Append("select * from ").Append(TraceUtil.TraceTableName);
            builder.Append(" \r where ").Append(TraceUtil.FLD_TABLE).Append("='").Append(table);
            builder.Append("' \r and ").Append(TraceUtil.FLD_FIELD).Append("='").Append(field);
            builder.Append("' \r and ").Append(TraceUtil.FLD_KEY).Append("='").Append(keyValue);
            builder.Append("' \r order by ").Append(TraceUtil.FLD_TIMESTAMP);
            DBConnection connection = new DBConnection(base.SMDataSource);
            connection.Open();
            try
            {
                DataTable dt = new DataTable();
                connection.ExecuteSql(dt, builder.ToString());
                table3 = dt;
            }
            finally
            {
                connection.Close();
            }
            return table3;
        }

        public override void SaveTraceToDb()
        {
            LoggingService.Info("开始保存痕迹记录...");
            DBConnection connection = new DBConnection(base.SMDataSource);
            connection.Open();
            try
            {
                if (LoggingService.IsDebugEnabled)
                {
                    LoggingService.DebugFormatted("共有修改或添加的痕迹记录：{0}条，删除记录：{1}条", new object[] { this.TraceTable.Rows.Count, (this._RecDelTable == null) ? 0 : this._RecDelTable.Rows.Count });
                }
                DbTransaction transaction = connection.BeginTrasaction() as DbTransaction;
                DbDataAdapter adapter = this.TraceTable.ExtendedProperties["da"] as DbDataAdapter;
                adapter.InsertCommand.Connection = connection.Connection as DbConnection;
                adapter.InsertCommand.Transaction = transaction;
                adapter.Update(this.TraceTable);
                if (TraceUtil.CanTraceLevel(TraceLevel.Deleted))
                {
                    DbDataAdapter adapter2 = this._RecDelTable.ExtendedProperties["da"] as DbDataAdapter;
                    adapter2.InsertCommand.Connection = connection.Connection as DbConnection;
                    adapter2.InsertCommand.Transaction = transaction;
                    adapter2.Update(this._RecDelTable);
                }
                connection.CommitTransaction();
            }
            catch (DbException exception)
            {
                connection.RollBackTransaction();
                string dataSetName = "TraceDb" + DateTimeHelper.GetNow().ToString();
                DataSet set = new DataSet(dataSetName);
                set.Tables.Add(this.TraceTable);
                set.Tables.Add(this._RecDelTable);
                set.WriteXml(Environment.CurrentDirectory + @"\" + dataSetName + ".xml");
                throw new DataFormException("数据修改日志记录保存不成功能（数据已成功能保存）！", exception);
            }
            finally
            {
                connection.Close();
                this.TraceTable.Rows.Clear();
                if (this._RecDelTable != null)
                {
                    this._RecDelTable.Rows.Clear();
                }
            }
        }

        protected override void TraceDeleted(DataTable DeletedTable, string TableName, string PrimaryKeyName)
        {
            DataTable table = DeletedTable.Clone();
            DataSet o = new DataSet(TableName);
            o.Tables.Add(table);
            XmlSerializer serializer = new XmlSerializer(typeof(DataSet));
            foreach (DataRow row in DeletedTable.Rows)
            {
                string keyValue = row[PrimaryKeyName, DataRowVersion.Original].ToString();
                base.AddTraceRow(DbOpType.Deleted, TableName, keyValue);
                table.ImportRow(row);
                MemoryStream stream = new MemoryStream();
                serializer.Serialize((Stream) stream, o);
                int length = (int) stream.Length;
                byte[] buffer = new byte[length];
                stream.Read(buffer, 0, length);
                stream.Close();
                this.AddRecDelRow(TableName, keyValue, buffer);
                table.Rows.Clear();
            }
        }

        protected override DataTable TraceTable
        {
            get
            {
                if (this._traceTable == null)
                {
                    DBConnection dbcn = new DBConnection(base.SMDataSource);
                    dbcn.Open();
                    try
                    {
                        this._traceTable = this.GetTable(TraceUtil.TraceTableName, dbcn);
                        if (TraceUtil.CanTraceLevel(TraceLevel.Deleted))
                        {
                            this._RecDelTable = this.GetTable(TraceUtil.RecDelTableName, dbcn);
                        }
                    }
                    finally
                    {
                        dbcn.Close();
                    }
                }
                return this._traceTable;
            }
        }
    }
}

