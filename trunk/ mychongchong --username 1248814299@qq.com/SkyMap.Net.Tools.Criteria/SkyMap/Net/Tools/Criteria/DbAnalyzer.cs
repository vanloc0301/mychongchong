namespace SkyMap.Net.Tools.Criteria
{
    using System;
    using System.Data;
    using System.Data.OleDb;

    public class DbAnalyzer
    {
        private OleDbConnection _connection;
        private DataSet _ds;
        private string _rel;

        private DbAnalyzer()
        {
            this._rel = "tbl_col";
        }

        public DbAnalyzer(string connectString)
        {
            this._rel = "tbl_col";
            if (connectString.Length == 0)
            {
                throw new ApplicationException("No database connection string specified!");
            }
            this._connection = new OleDbConnection(connectString);
            this._connection.Open();
        }

        public void Dispose()
        {
            if (this._connection != null)
            {
                this._connection.Close();
            }
        }

        public DataRow[] GetColumns(DataRow table)
        {
            return table.GetChildRows(this._rel);
        }

        public DataSet GetDBSchemas()
        {
            DataSet set = new DataSet();
            object[] restrictions = new object[4];
            restrictions[3] = "TABLE";
            DataTable oleDbSchemaTable = this._connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, restrictions);
            set.Merge(oleDbSchemaTable);
            restrictions = new object[4];
            restrictions[3] = "VIEW";
            DataTable table = this._connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, restrictions);
            set.Merge(table);
            foreach (DataRow row in set.Tables[0].Rows)
            {
                restrictions = new object[3];
                restrictions[2] = row["TABLE_NAME"];
                DataTable table3 = this._connection.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, restrictions);
                set.Merge(table3, true, MissingSchemaAction.Add);
            }
            set.Relations.Add(this._rel, set.Tables["Tables"].Columns["TABLE_NAME"], set.Tables["Columns"].Columns["TABLE_NAME"]);
            return set;
        }

        public DataRow[] GetTables(string TableType)
        {
            if (this._ds == null)
            {
                this._ds = this.GetDBSchemas();
            }
            DataTable table = this._ds.Tables["Tables"];
            return table.Select("TABLE_TYPE='" + TableType + "'");
        }
    }
}

