namespace SkyMap.Net.DAO
{
    using SkyMap.Net.Core;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.Common;
    using System.Drawing.Design;

    [Serializable]
    public class SMDataSource : DomainObject, ISaveAs
    {
        private string connectionString;
        private string dsType = "System.Data.SqlClient";

        public DbConnection CreateConnection()
        {
            DbConnection connection = DbProviderFactories.GetFactory(this.dsType).CreateConnection();
            connection.ConnectionString = this.DecryptConnectionString;
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("将创建连接:{0}", new object[] { connection.ConnectionString });
            }
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            return connection;
        }

        public void SaveAs(UnitOfWork unitOfWork)
        {
            unitOfWork.RegisterNew(this);
        }

        [Browsable(false)]
        public string ConnectionString
        {
            get
            {
                return this.connectionString;
            }
            set
            {
                this.connectionString = value;
            }
        }

        [DisplayName("连接字符串"), Editor("SkyMap.Net.Gui.Components.DbConnectionEditor,SkyMap.Net.Windows", typeof(UITypeEditor))]
        public string ConnectionStringForEdit
        {
            get
            {
                return this.DecryptConnectionString;
            }
            set
            {
                if (value != "*****")
                {
                    this.connectionString = new CryptoHelper().Encrypt(value);
                }
            }
        }

        [Browsable(false)]
        public string DecryptConnectionString
        {
            get
            {
                CryptoHelper helper = new CryptoHelper();
                try
                {
                    return helper.Decrypt(this.connectionString);
                }
                catch
                {
                    return this.connectionString;
                }
            }
        }

        [Editor("SkyMap.Net.Gui.Components.DbProviderEditor,SkyMap.Net.Windows", typeof(UITypeEditor)), DisplayName("数据库类型")]
        public string DSType
        {
            get
            {
                return this.dsType;
            }
            set
            {
                this.dsType = value;
            }
        }
    }
}

