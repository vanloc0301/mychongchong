namespace RedSpider.db_class
{
    using System;
    using System.Configuration;
    using System.Data.OleDb;

    internal class Module_Manager
    {
        public void ADD_Module(string Post_Url, string Post_News, string P_Success, string P_Failure, string Module_Name, string Module_Var)
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["RedSpider_Module"];
            OleDbConnection connection = new OleDbConnection(settings.ConnectionString);
            OleDbCommand command = new OleDbCommand("insert into News_Module (Post_Url,Post_News,P_Success,P_Failure,Module_Name,Module_Var) values ('" + Post_Url + "','" + Post_News + "','" + P_Success + "','" + P_Failure + "','" + Module_Name + "','" + Module_Var + "')", connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void DEFAULT_Module(string id)
        {
            long num = long.Parse(id);
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["RedSpider_Module"];
            OleDbConnection connection = new OleDbConnection(settings.ConnectionString);
            string cmdText = "update  News_Module set [Module_Default]='否'";
            OleDbCommand command = new OleDbCommand(cmdText, connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            ConnectionStringSettings settings2 = ConfigurationManager.ConnectionStrings["RedSpider_Module"];
            OleDbConnection connection2 = new OleDbConnection(settings2.ConnectionString);
            OleDbCommand command2 = new OleDbCommand("update  News_Module set [Module_Default]='是' where id=" + num, connection2);
            connection2.Open();
            command2.ExecuteNonQuery();
            connection2.Close();
        }

        public void DEL_Module(string id)
        {
            long num = long.Parse(id);
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["RedSpider_Module"];
            OleDbConnection connection = new OleDbConnection(settings.ConnectionString);
            OleDbCommand command = new OleDbCommand("DELETE * FROM News_Module where id=" + num, connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void UP_Module(string Post_Url, string Post_News, string P_Success, string P_Failure, string Module_Name, string Module_Var, string id)
        {
            long num = long.Parse(id);
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["RedSpider_Module"];
            OleDbConnection connection = new OleDbConnection(settings.ConnectionString);
            OleDbCommand command = new OleDbCommand(string.Concat(new object[] { "update News_Module SET [Post_Url]='", Post_Url, "',[Post_News]='", Post_News, "',[P_Success]='", P_Success, "',[P_Failure]='", P_Failure, "',[Module_Name]='", Module_Name, "',[Module_Var]='", Module_Var, "' where id=", num }), connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}

