namespace SkyMap.Net.DataForms.DataEngine
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using System;
    using System.Data;
    using System.Data.Common;

    public sealed class DBConnection
    {
        private DbConnection conn;
        private string connectionString;
        private DbProviderFactory dataFactory;
        private int OpenCount = 0;
        private DbTransaction tran;
        private int TranCount = 0;

        public DBConnection(SMDataSource smDs)
        {
            if (smDs == null)
            {
                throw new ArgumentNullException("smDs");
            }
            this.dataFactory = DbProviderFactories.GetFactory(smDs.DSType);
            this.connectionString = smDs.DecryptConnectionString;
        }

        public IDbTransaction BeginTrasaction()
        {
            if (this.conn == null)
            {
                throw new NoNullAllowedException("数据库连尚未创建，你不能开启事物");
            }
            if (this.tran == null)
            {
                this.tran = this.conn.BeginTransaction();
            }
            this.TranCount++;
            return this.tran;
        }

        public void Close()
        {
            if (this.OpenCount != 0)
            {
                this.OpenCount--;
                if (this.OpenCount == 0)
                {
                    this.Connection.Close();
                }
            }
        }

        public void CommitTransaction()
        {
            this.TranCount--;
            if ((this.TranCount == 0) && (this.tran != null))
            {
                this.tran.Commit();
                this.tran = null;
            }
        }

        public int ExecuteNonQuery(string sql)
        {
            int num;
            this.Open();
            try
            {
                IDbCommand command = this.Connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = sql;
                num = command.ExecuteNonQuery();
            }
            finally
            {
                this.Close();
            }
            return num;
        }

        public T ExecuteSql<T>(string sql)
        {
            T local;
            this.Open();
            try
            {
                IDbCommand command = this.Connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = sql;
                local = (T) command.ExecuteScalar();
            }
            finally
            {
                this.Close();
            }
            return local;
        }

        public void ExecuteSql(string sql)
        {
            this.ExecuteNonQuery(sql);
        }

        public void ExecuteSql(DataTable dt, string sql)
        {
            this.Open();
            try
            {
                IDbCommand command = this.Connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = sql;
                IDataReader reader = command.ExecuteReader();
                try
                {
                    dt.Load(reader);
                }
                finally
                {
                    reader.Close();
                }
            }
            finally
            {
                this.Close();
            }
        }

        public DbDataAdapter GetDataAdapter(string strSelect, bool getInsertCommand, bool getUpdateCommand, bool getDeleteCommand)
        {
            DbCommand command = this.conn.CreateCommand();
            command.CommandText = strSelect;
            command.Transaction = this.tran;
            DbDataAdapter adapter = this.dataFactory.CreateDataAdapter();
            adapter.SelectCommand = command;
            DbCommandBuilder builder = this.dataFactory.CreateCommandBuilder();
            builder.DataAdapter = adapter;
            if (getInsertCommand)
            {
                adapter.InsertCommand = builder.GetInsertCommand();
                adapter.InsertCommand.Connection = this.conn;
                adapter.InsertCommand.Transaction = this.tran;
            }
            if (getUpdateCommand)
            {
                adapter.UpdateCommand = builder.GetUpdateCommand();
                adapter.UpdateCommand.Connection = this.conn;
                adapter.UpdateCommand.Transaction = this.tran;
            }
            if (getDeleteCommand)
            {
                adapter.DeleteCommand = builder.GetDeleteCommand();
                adapter.DeleteCommand.Connection = this.conn;
                adapter.DeleteCommand.Transaction = this.tran;
            }
            return adapter;
        }

        public void Open()
        {
            if (this.Connection.State == ConnectionState.Closed)
            {
                this.Connection.Open();
            }
            this.OpenCount++;
        }

        public void RollBackTransaction()
        {
            this.TranCount--;
            if ((this.TranCount == 0) && (this.tran != null))
            {
                this.tran.Rollback();
                this.tran = null;
            }
        }

        public IDbConnection Connection
        {
            get
            {
                if (this.conn == null)
                {
                    this.conn = this.dataFactory.CreateConnection();
                    this.conn.ConnectionString = this.connectionString;
                    if (LoggingService.IsDebugEnabled)
                    {
                        LoggingService.DebugFormatted("数据库连接字符串是:{0}", new object[] { this.conn.ConnectionString });
                    }
                }
                return this.conn;
            }
        }

        public IDbTransaction Transaction
        {
            get
            {
                return this.tran;
            }
        }
    }
}

