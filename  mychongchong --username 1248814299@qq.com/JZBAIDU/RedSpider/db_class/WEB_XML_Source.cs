namespace RedSpider.db_class
{
    using System;
    using System.Data.OleDb;

    internal class WEB_XML_Source
    {
        public void ADD_XML_Url(string Source_name, string Source_url)
        {
            OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
            OleDbCommand command = new OleDbCommand("insert into WEB_XML_Source(Source_name,Source_url) values ('" + Source_name + "','" + Source_url + "')", connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void Default_Skin(string Skin_link)
        {
            string str = "否";
            OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
            OleDbCommand command = new OleDbCommand("update RedSpider_Skin SET [Skin_Default]='" + str + "'", connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            string str3 = "是";
            OleDbConnection connection2 = new OleDbConnection(CONN_ACCESS.ConnString);
            OleDbCommand command2 = new OleDbCommand("update RedSpider_Skin set [Skin_Default]='" + str3 + "' where [Skin_link]='" + Skin_link + "'", connection2);
            connection2.Open();
            command2.ExecuteNonQuery();
            connection2.Close();
        }

        public void update_XML_Url(string Source_name, string Source_url)
        {
            OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
            OleDbCommand command = new OleDbCommand("update WEB_XML_Source SET [Source_name]='" + Source_name + "',[Source_url]='" + Source_url + "'", connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void XML_Url_Default(string url)
        {
            string str = "否";
            OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
            OleDbCommand command = new OleDbCommand("update WEB_XML_Source SET [Default]='" + str + "'", connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            string str3 = "是";
            OleDbConnection connection2 = new OleDbConnection(CONN_ACCESS.ConnString);
            OleDbCommand command2 = new OleDbCommand("update WEB_XML_Source set [Default]='" + str3 + "' where [Source_url]='" + url + "'", connection2);
            connection2.Open();
            command2.ExecuteNonQuery();
            connection2.Close();
        }
    }
}

