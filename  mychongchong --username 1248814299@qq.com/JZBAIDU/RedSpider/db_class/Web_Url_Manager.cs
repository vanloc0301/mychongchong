namespace RedSpider.db_class
{
    using Spider_Global_variables;
    using System;
    using System.Data.OleDb;

    internal class Web_Url_Manager
    {
        public void ADD_Exclude(string pc_tt)
        {
            OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
            OleDbCommand command = new OleDbCommand("insert into RedSpider_Exclude_replacement (url_id,lable_name,pc_or_tt,pc_tt) values ('" + Global.URL_BIANHAO.ToString() + "','" + Global.LABLE_NAME.ToString() + "','排除','" + pc_tt.ToString() + "')", connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            OleDbConnection connection2 = new OleDbConnection(CONN_ACCESS.ConnString);
            OleDbCommand command2 = new OleDbCommand("update RedSpider_Label SET [E_xclusion]='是'  where url_id='" + Global.URL_BIANHAO.ToString() + "' and Label_name='" + Global.LABLE_NAME.ToString() + "'", connection2);
            connection2.Open();
            command2.ExecuteNonQuery();
            connection2.Close();
        }

        public void ADD_Replacement(string ttq, string tth)
        {
            OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
            OleDbCommand command = new OleDbCommand("insert into RedSpider_Exclude_replacement (url_id,lable_name,pc_or_tt,pc_tt,tt_h) values ('" + Global.URL_BIANHAO.ToString() + "','" + Global.LABLE_NAME.ToString() + "','替换','" + ttq.ToString() + "','" + tth.ToString() + "')", connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            OleDbConnection connection2 = new OleDbConnection(CONN_ACCESS.ConnString);
            OleDbCommand command2 = new OleDbCommand("update RedSpider_Label SET [R_Parts]='是'  where url_id='" + Global.URL_BIANHAO.ToString() + "' and Label_name='" + Global.LABLE_NAME.ToString() + "'", connection2);
            connection2.Open();
            command2.ExecuteNonQuery();
            connection2.Close();
        }

        public void ADD_Url(string column_name, string web_name, string web_url, string bxbh, string bdbh, string bm, string ccbox)
        {
            OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
            OleDbCommand command = new OleDbCommand("insert into Web_URL (column_name,parentid,web_url,bxbh,bdbh,bm,class) values ('" + web_name + "','" + Global.COLUMN_ID + "','" + web_url + "','" + bxbh + "','" + bdbh + "','" + bm + "','" + ccbox + "')", connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void DeL_Exclude()
        {
            OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
            OleDbCommand command = new OleDbCommand("delete * FROM RedSpider_Exclude_replacement  where url_id='" + Global.URL_BIANHAO.ToString() + "' and lable_name='" + Global.LABLE_NAME.ToString() + "' and pc_or_tt='排除' and pc_tt='" + Global.PAICU.ToString() + "'", connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            OleDbConnection connection2 = new OleDbConnection(CONN_ACCESS.ConnString);
            OleDbCommand command2 = new OleDbCommand("SELECT count(*) FROM RedSpider_Exclude_replacement where url_id='" + Global.URL_BIANHAO.ToString() + "' and lable_name='" + Global.LABLE_NAME.ToString() + "' and pc_or_tt='排除'", connection2);
            connection2.Open();
            string str3 = command2.ExecuteScalar().ToString();
            connection2.Close();
            if (str3 == "0")
            {
                OleDbConnection connection3 = new OleDbConnection(CONN_ACCESS.ConnString);
                OleDbCommand command3 = new OleDbCommand("update RedSpider_Label SET [E_xclusion]='否'  where url_id='" + Global.URL_BIANHAO.ToString() + "' and Label_name='" + Global.LABLE_NAME.ToString() + "'", connection3);
                connection3.Open();
                command3.ExecuteNonQuery();
                connection3.Close();
            }
        }

        public void DeL_lable()
        {
            OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
            OleDbCommand command = new OleDbCommand("delete * FROM RedSpider_Label  where url_id='" + Global.URL_BIANHAO.ToString() + "' and Label_name='" + Global.LABLE_NAME.ToString() + "'", connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void DeL_Replacement()
        {
            OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
            OleDbCommand command = new OleDbCommand("delete * FROM RedSpider_Exclude_replacement  where url_id='" + Global.URL_BIANHAO.ToString() + "' and lable_name='" + Global.LABLE_NAME.ToString() + "' and pc_or_tt='替换' and pc_tt='" + Global.PAICU.ToString() + "'", connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            OleDbConnection connection2 = new OleDbConnection(CONN_ACCESS.ConnString);
            OleDbCommand command2 = new OleDbCommand("SELECT count(*) FROM RedSpider_Exclude_replacement where url_id='" + Global.URL_BIANHAO.ToString() + "' and lable_name='" + Global.LABLE_NAME.ToString() + "' and pc_or_tt='替换'", connection2);
            connection2.Open();
            string str3 = command2.ExecuteScalar().ToString();
            connection2.Close();
            if (str3 == "0")
            {
                OleDbConnection connection3 = new OleDbConnection(CONN_ACCESS.ConnString);
                OleDbCommand command3 = new OleDbCommand("update RedSpider_Label SET [R_Parts]='否'  where url_id='" + Global.URL_BIANHAO.ToString() + "' and Label_name='" + Global.LABLE_NAME.ToString() + "'", connection3);
                connection3.Open();
                command3.ExecuteNonQuery();
                connection3.Close();
            }
        }

        public void DEL_Url()
        {
            long num = long.Parse(Global.URLID.ToString());
            OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
            OleDbCommand command = new OleDbCommand("delete * FROM Web_URL where id=" + num, connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            OleDbConnection connection2 = new OleDbConnection(CONN_ACCESS.ConnString);
            OleDbCommand command2 = new OleDbCommand("delete * FROM RedSpider_Label where url_id= '" + Global.URLID.ToString() + "'", connection2);
            connection2.Open();
            command2.ExecuteNonQuery();
            connection2.Close();
            OleDbConnection connection3 = new OleDbConnection(CONN_ACCESS.ConnString);
            OleDbCommand command3 = new OleDbCommand("delete * FROM RedSpider_Exclude_replacement where url_id= '" + Global.URLID.ToString() + "'", connection3);
            connection3.Open();
            command3.ExecuteNonQuery();
            connection3.Close();
        }

        public void UP_Exclude(string pc_tt)
        {
            OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
            OleDbCommand command = new OleDbCommand("update RedSpider_Exclude_replacement  SET [pc_tt]='" + pc_tt + "' where url_id='" + Global.URL_BIANHAO.ToString() + "' and lable_name='" + Global.LABLE_NAME.ToString() + "' and pc_or_tt='排除' and pc_tt='" + Global.PAICU.ToString() + "'", connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void UP_Replacement(string ttq, string tth)
        {
            OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
            OleDbCommand command = new OleDbCommand("update RedSpider_Exclude_replacement  SET [pc_tt]='" + ttq.ToString() + "',[tt_h]='" + tth.ToString() + "' where url_id='" + Global.URL_BIANHAO.ToString() + "' and lable_name='" + Global.LABLE_NAME.ToString() + "' and pc_or_tt='替换' and pc_tt='" + Global.PAICU.ToString() + "'", connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void update_lable(string lable_name, string B_egin, string E_nd)
        {
            OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
            OleDbCommand command = new OleDbCommand("update RedSpider_Label SET [B_egin]='" + B_egin + "',[E_nd]='" + E_nd + "',[zzyes]='否' where Label_name='" + lable_name.ToString() + "' and url_id='" + Global.URL_BIANHAO.ToString() + "'", connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void update_lable_zz(string lable_name, string B_egin)
        {
            OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
            OleDbCommand command = new OleDbCommand("update RedSpider_Label SET [B_egin]='" + B_egin + "',[zzyes]='是' where Label_name='" + lable_name.ToString() + "' and url_id='" + Global.URL_BIANHAO.ToString() + "'", connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void insert_lable(string lable_name, string B_egin, string E_nd)
        {
            OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
            OleDbCommand command = new OleDbCommand("insert into RedSpider_Label([url_id],[B_egin],[E_nd],[zzyes],[Label_name])  values ('" + Global.URLID + "','" + B_egin + "','" + E_nd + "','否','" + lable_name + "')", connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void insert_lable_zz(string lable_name, string B_egin)
        {
            OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
            OleDbCommand command = new OleDbCommand("insert into RedSpider_Label([url_id],[B_egin],[zzyes],[Label_name]) values ('" + Global.URLID + "','" + B_egin + "','是','" + lable_name + "')", connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void UPDATE_Url(string column_name, string web_name, string web_url, string bxbh, string bdbh, string bm, string ccbox)
        {
            long num = long.Parse(Global.URLID.ToString());
            OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
            OleDbCommand command = new OleDbCommand(string.Concat(new object[] { "update Web_URL SET [column_name]='", web_name, "',[web_url]='", web_url, "',[bxbh]='", bxbh, "',[bdbh]='", bdbh, "',[bm]='", bm, "',[class]='", ccbox, "' where id=", num }), connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}

