namespace RedSpider.db_class
{
    using System;
    using System.Data.OleDb;

    internal class Mission_Time
    {
        public void M_timeup(string m_time, string jk)
        {
            OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
            OleDbCommand command = new OleDbCommand("update Mission_Time SET [Mission_Time]='" + m_time + "',[Monitoring]='" + jk + "'", connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void up_index_file(string file_lj)
        {
            OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
            OleDbCommand command = new OleDbCommand("update Mission_Time SET [index_file]='" + file_lj + "'", connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}

