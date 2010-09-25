namespace RedSpider.db_class
{
    using Spider_Global_variables;
    using System;
    using System.Data.OleDb;

    internal class column_manager
    {
        public void add_column(string column_name)
        {
            OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
            OleDbCommand command = new OleDbCommand("insert into Web_URL (column_name,parentid,code) values ('" + column_name + "','0',' 0 ')", connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void DeL_column()
        {
            long num = long.Parse(Global.COLUMN_ID.ToString());
            OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
            OleDbCommand command = new OleDbCommand("delete * FROM Web_URL  where id=" + num, connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void up_column(string column_name)
        {
            long num = long.Parse(Global.COLUMN_ID.ToString());
            OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
            OleDbCommand command = new OleDbCommand(string.Concat(new object[] { "update Web_URL SET [column_name]='", column_name, "' where id=", num }), connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}

