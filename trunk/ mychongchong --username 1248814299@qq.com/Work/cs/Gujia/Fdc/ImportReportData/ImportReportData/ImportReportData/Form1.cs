using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
namespace ImportReportData
{
    public partial class Form1 : Form
    {
        private const string m_strInsert = @"insert into DF_TEMPLETPRINT(TEMPLETPRINT_ID,TEMPLETPRINT_NAME,TEMPLETPRINT_TYPE,TEMPLETPRINT_DATA,NEED_ASK,PRINT_PREVIEW,TEMPLETPRINT_DESCRIPTION,REPLICATION_VERSION) 
            values(newid(),@TEMPLETPRINT_NAME,@TEMPLETPRINT_TYPE,@TEMPLETPRINT_DATA,0,0,@TEMPLETPRINT_DESCRIPTION,10) ";
        private const string m_strUpdate = @"update DF_TEMPLETPRINT set TEMPLETPRINT_DATA=@TEMPLETPRINT_DATA where TEMPLETPRINT_NAME=@TEMPLETPRINT_NAME and TEMPLETPRINT_DESCRIPTION = @TEMPLETPRINT_DESCRIPTION and REPLICATION_VERSION = 10";
        private const string m_strSelect = @"select TEMPLETPRINT_ID,TEMPLETPRINT_NAME,TEMPLETPRINT_TYPE,TEMPLETPRINT_DATA,NEED_ASK,PRINT_PREVIEW,TEMPLETPRINT_DESCRIPTION,REPLICATION_VERSION from DF_TEMPLETPRINT where TEMPLETPRINT_NAME=@TEMPLETPRINT_NAME and TEMPLETPRINT_TYPE = @TEMPLETPRINT_TYPE and  TEMPLETPRINT_DESCRIPTION = @TEMPLETPRINT_DESCRIPTION";
        public Form1()
        {
            InitializeComponent();
        }

        private void m_btnImport_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要导入报表吗?", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            DirectoryInfo o = new DirectoryInfo(System.Configuration.ConfigurationSettings.AppSettings["directory"].ToString());
            FileInfo[] fileInfos = o.GetFiles();
            string strDESCRIPTION = "";
            string strTEMPLETPRINT_NAME = "";
            string strTEMPLETPRINT_TYPE = "";
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["connection"].ToString()))
            {
                con.Open();
                SqlCommand sqlCom = new SqlCommand(m_strInsert, con);
                SqlParameter[] parms = { new SqlParameter("@TEMPLETPRINT_NAME", SqlDbType.VarChar), new SqlParameter("@TEMPLETPRINT_TYPE", SqlDbType.VarChar), new SqlParameter("@TEMPLETPRINT_DATA", SqlDbType.Binary), new SqlParameter("@TEMPLETPRINT_DESCRIPTION", SqlDbType.VarChar) };
                foreach (FileInfo fileInfo in fileInfos)
                {
                    String[] strs = fileInfo.Name.Split(new char[] { '=' });
                    byte[] bytPic = new byte[fileInfo.Length];
                    strTEMPLETPRINT_NAME = strs[0];
                    strTEMPLETPRINT_TYPE = strs[strs.Length - 1];
                    strDESCRIPTION = "";

                    for (int i = 1; i < strs.Length - 1; i++)
                    {
                        strDESCRIPTION += change(strs[i]);
                    }

                    fileInfo.OpenRead().Read(bytPic, 0, (int)fileInfo.Length);
                    parms[0].Value = strTEMPLETPRINT_NAME;
                    parms[1].Value = strTEMPLETPRINT_TYPE;
                    parms[2].Value = bytPic;
                    parms[3].Value = strDESCRIPTION;
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add(parms[0]);
                    sqlCom.Parameters.Add(parms[1]);
                    sqlCom.Parameters.Add(parms[2]);
                    sqlCom.Parameters.Add(parms[3]);
                    sqlCom.ExecuteNonQuery();
                    //if (== -1)
                    //{
                    //    sqlCom.CommandText = m_strInsert;
                    //    sqlCom.ExecuteNonQuery();
                    //}
                }
            }
            MessageBox.Show("导入完毕");
        }


        private void m_btnUpdate_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要更新报表吗?", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            DirectoryInfo o = new DirectoryInfo(System.Configuration.ConfigurationSettings.AppSettings["directory"].ToString());
            FileInfo[] fileInfos = o.GetFiles();
            string strDESCRIPTION = "";
            string strTEMPLETPRINT_NAME = "";
            string strTEMPLETPRINT_TYPE = "";
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["connection"].ToString()))
            {
                con.Open();
                SqlCommand sqlCom = new SqlCommand(m_strUpdate, con);
                SqlParameter[] parms = { new SqlParameter("@TEMPLETPRINT_NAME", SqlDbType.VarChar), new SqlParameter("@TEMPLETPRINT_TYPE", SqlDbType.VarChar), new SqlParameter("@TEMPLETPRINT_DATA", SqlDbType.Binary), new SqlParameter("@TEMPLETPRINT_DESCRIPTION", SqlDbType.VarChar) };
                foreach (FileInfo fileInfo in fileInfos)
                {
                    String[] strs = fileInfo.Name.Split(new char[] { '=' });
                    byte[] bytPic = new byte[fileInfo.Length];
                    strTEMPLETPRINT_NAME = strs[0];
                    strTEMPLETPRINT_TYPE = strs[strs.Length - 1];
                    strDESCRIPTION = "";

                    for (int i = 1; i < strs.Length - 1; i++)
                    {
                        strDESCRIPTION += change(strs[i]);
                    }

                    fileInfo.OpenRead().Read(bytPic, 0, (int)fileInfo.Length);
                    parms[0].Value = strTEMPLETPRINT_NAME;
                    parms[1].Value = strTEMPLETPRINT_TYPE;
                    parms[2].Value = bytPic;
                    parms[3].Value = strDESCRIPTION;
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add(parms[0]);
                    sqlCom.Parameters.Add(parms[1]);
                    sqlCom.Parameters.Add(parms[2]);
                    sqlCom.Parameters.Add(parms[3]);
                    sqlCom.ExecuteNonQuery();
                    //if (== -1)
                    //{
                    //    sqlCom.CommandText = m_strInsert;
                    //    sqlCom.ExecuteNonQuery();
                    //}
                }
            }
            MessageBox.Show("更新完毕");
        }

        private void bShow_Click(object sender, EventArgs e)
        {
            string strDESCRIPTION = "";
            string strTEMPLETPRINT_NAME = "";
            string strTEMPLETPRINT_TYPE = "";

            string tmpstr = txtFileName.Text;
            String[] strs = tmpstr.Split(new char[] { '=' });
            strTEMPLETPRINT_NAME = strs[0];
            strTEMPLETPRINT_TYPE = strs[strs.Length - 1];
            strDESCRIPTION = "";

            for (int i = 1; i < strs.Length - 1; i++)
            {
                strDESCRIPTION += change(strs[i]);
            }
            txtReturn0.Text = strTEMPLETPRINT_NAME;
            txtReturn1.Text = strDESCRIPTION;
            txtReturn2.Text = strTEMPLETPRINT_TYPE;

        }

        private void btnBak_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要备份报表吗?", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }
            DirectoryInfo o = new DirectoryInfo(System.Configuration.ConfigurationSettings.AppSettings["directory"].ToString());
            FileInfo[] fileInfos = o.GetFiles();
            string strTEMPLETPRINT_NAME = "";
            string strTEMPLETPRINT_TYPE = "";
            string strDESCRIPTION = "";
            string fileName = "";
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["connection"].ToString()))
            {
                con.Open();
                SqlCommand sqlCom = new SqlCommand(m_strSelect, con);
                SqlParameter[] parms = { new SqlParameter("@TEMPLETPRINT_NAME", SqlDbType.VarChar), new SqlParameter("@TEMPLETPRINT_TYPE", SqlDbType.VarChar), new SqlParameter("@TEMPLETPRINT_DESCRIPTION", SqlDbType.VarChar) };
                foreach (FileInfo fileInfo in fileInfos)
                {
                    String[] strs = fileInfo.Name.Split(new char[] { '=' });
                    byte[] bytPic = new byte[fileInfo.Length];
                    strTEMPLETPRINT_NAME = strs[0];
                    strTEMPLETPRINT_TYPE = strs[strs.Length - 1];
                    strDESCRIPTION = "";
                    for (int i = 1; i < strs.Length - 1; i++)
                    {
                        strDESCRIPTION += change(strs[i]);
                    }

                    fileInfo.OpenRead().Read(bytPic, 0, (int)fileInfo.Length);
                    parms[0].Value = strTEMPLETPRINT_NAME;
                    parms[1].Value = strTEMPLETPRINT_TYPE;
                    parms[2].Value = strDESCRIPTION;
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add(parms[0]);
                    sqlCom.Parameters.Add(parms[1]);
                    sqlCom.Parameters.Add(parms[2]);

                    SqlDataReader rd = sqlCom.ExecuteReader();
                    byte[] File = null;
                    if (rd.HasRows)
                    {
                        rd.Read();
                        File = (byte[])rd["TEMPLETPRINT_DATA"];
                        //-------------------
                        if (Directory.Exists(System.Configuration.ConfigurationSettings.AppSettings["backup"].ToString()) == false)//如果不存在就创建file文件夹
                        {
                            Directory.CreateDirectory(System.Configuration.ConfigurationSettings.AppSettings["backup"].ToString());
                        }
                        else
                        {
                            //Directory.Delete(System.Configuration.ConfigurationSettings.AppSettings["backup"].ToString(), true);//删除文件夹以及文件夹中的子目录，文件 
                        }
                        //-----------------
                        fileName = System.Configuration.ConfigurationSettings.AppSettings["backup"].ToString() + @"\\" + @rd["TEMPLETPRINT_NAME"].ToString() + "=" + @rd["TEMPLETPRINT_DESCRIPTION"].ToString() + "=" + @rd["TEMPLETPRINT_TYPE"].ToString();
                        fileName = fileName.Replace("*", "心");
                        FileStream fs = new FileStream(fileName, FileMode.Create);
                        BinaryWriter bw = new BinaryWriter(fs);
                        bw.Write(File, 0, File.Length);
                        bw.Close();
                        fs.Close();

                    }
                    else
                    {
                        MessageBox.Show(fileInfo.Name.ToString() + "模板文件不存在，请用后台管理工具添加");
                    }
                    rd.Close();

                }
                con.Close();
                MessageBox.Show("备份完成!");
            }

        }




        private string change(string p_str)
        {
            switch (p_str)
            {
                case "拍卖":
                    return "P";
                case "挂牌":
                    return "G";
                case "出让":
                    return "0";
                case "转让":
                    return "9";
                case "工业地":
                    return "Y";
                case "监管":
                    return "Y";
                case "委托":
                    return "Y";
                case "非监管":
                    return "N";
                case "非委托":
                    return "N";
                case "不判断":
                    return "*";
                case "非工业地":
                    return "N";
                default:
                    return p_str;
            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要还原报表吗?", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }
            DirectoryInfo o = new DirectoryInfo(System.Configuration.ConfigurationSettings.AppSettings["backup"].ToString());
            FileInfo[] fileInfos = o.GetFiles();
            string strTEMPLETPRINT_NAME = "";
            string strTEMPLETPRINT_TYPE = "";
            string strDESCRIPTION = "";
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["connection"].ToString()))
            {
                con.Open();
                SqlCommand sqlCom = new SqlCommand(m_strSelect, con);
                SqlParameter[] parms = { new SqlParameter("@TEMPLETPRINT_NAME", SqlDbType.VarChar), new SqlParameter("@TEMPLETPRINT_TYPE", SqlDbType.VarChar), new SqlParameter("@TEMPLETPRINT_DESCRIPTION", SqlDbType.VarChar) };
                foreach (FileInfo fileInfo in fileInfos)
                {
                    String[] strs = fileInfo.Name.Split(new char[] { '=' });
                    byte[] bytPic = new byte[fileInfo.Length];
                    strTEMPLETPRINT_NAME = strs[0];
                    strTEMPLETPRINT_TYPE = strs[strs.Length - 1];
                    strDESCRIPTION = "";
                    for (int i = 1; i < strs.Length - 1; i++)
                    {
                        strDESCRIPTION += strs[i];
                    }
                    strDESCRIPTION = strDESCRIPTION.Replace("心", "*");
                    fileInfo.OpenRead().Read(bytPic, 0, (int)fileInfo.Length);
                    parms[0].Value = strTEMPLETPRINT_NAME;
                    parms[1].Value = strTEMPLETPRINT_TYPE;
                    parms[2].Value = strDESCRIPTION;
                    sqlCom.Parameters.Clear();
                    sqlCom.Parameters.Add(parms[0]);
                    sqlCom.Parameters.Add(parms[1]);
                    sqlCom.Parameters.Add(parms[2]);

                    SqlDataReader rd = sqlCom.ExecuteReader();
                    if (rd.HasRows)
                    {
                        rd.Close();
                        SqlCommand sqlCom1 = new SqlCommand(m_strUpdate, con);
                        SqlParameter[] parms1 = { new SqlParameter("@TEMPLETPRINT_NAME", SqlDbType.VarChar), new SqlParameter("@TEMPLETPRINT_TYPE", SqlDbType.VarChar), new SqlParameter("@TEMPLETPRINT_DATA", SqlDbType.Binary), new SqlParameter("@TEMPLETPRINT_DESCRIPTION", SqlDbType.VarChar) };
                        parms1[0].Value = strTEMPLETPRINT_NAME;
                        parms1[1].Value = strTEMPLETPRINT_TYPE;
                        parms1[2].Value = bytPic;
                        parms1[3].Value = strDESCRIPTION;
                        sqlCom1.Parameters.Clear();
                        sqlCom1.Parameters.Add(parms1[0]);
                        sqlCom1.Parameters.Add(parms1[1]);
                        sqlCom1.Parameters.Add(parms1[2]);
                        sqlCom1.Parameters.Add(parms1[3]);
                        if (sqlCom1.ExecuteNonQuery() == -1)
                        {
                            MessageBox.Show(fileInfo.Name.ToString() + "更新失败!");
                        }
                    }
                    else
                    {
                        MessageBox.Show(fileInfo.Name.ToString() + "模板文件不存在，请用后台管理工具添加");
                    }


                }
                MessageBox.Show("还原完成!");
                con.Close();
            }
        }



    }
}
