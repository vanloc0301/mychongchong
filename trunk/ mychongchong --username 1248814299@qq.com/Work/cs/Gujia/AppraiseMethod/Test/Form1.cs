using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Taramon.Exceller;
using AppraiseMethod.Excel;
using System.IO;
using System.Reflection;
using System.Collections;
using Utility;
using NHibernate.Cfg;
using NHibernate;
using Gujia.Method;

namespace Test
{
    public partial class Form1 : Form
    {
        static DateTime beforeTime;  //Excel启动之前时间
        static DateTime afterTime;   
        ILand gy;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ISession isession = NHibernateSession.GetCurrentSession();
            工业区片 ss = isession.Get<Gujia.Method.工业区片>(4420000004002);
            string tmp = "";
            //try
            //{
            //    beforeTime = DateTime.Now;
            //    LandInput li;
            //    for (int i = 1; i < 2; i++)
            //    {

            //        ExcelMapper mapper = new ExcelMapper();

            //        // LandGy gy = new LandGy();
            //       // gy = new LandZz();
            //        li = new LandInput();
            //        li.Qph = "4420001112003";
            //        string excelFileName = Path.Combine(Directory.GetCurrentDirectory(), @"D:\Hi\cs\AppraiseMethod\AppraiseMethod\房地产土地估价.xls");
            //        // mapper.Write(li, @"c:\aaa.xls",excelFileName);
            //        mapper.Write(li, excelFileName, true);
            //        if (li.Qph_From == li.Qph)
            //        {
            //            if (li.Type == "工业")
            //            {
            //                gy = new LandGy();
            //            }
            //            else if (li.Type == "商住")
            //            {
            //                gy = new LandSz();
            //            }
            //            else if (li.Type == "住宅")
            //            {
            //                gy = new LandZz();
            //            }
            //            else 
            //            {

            //            }
            //            if (gy != null)
            //            {
            //                mapper.Read(gy, excelFileName);
            //            }

            //        }
            //        //mapper.Read(li, excelFileName);
            //        //mapper.Read(gy, excelFileName);

            //        //double sum = 0.0;
            //        //int n = 0;
            //        //try
            //        //{

            //        //    //foreach (string number in gy.Qph)
            //        //    //{
            //        //    //  //  sum += Convert.ToDouble(number);
            //        //    //    n++;
            //        //    //}
            //            InitGrid();
            //        //}
            //        //catch (Exception ex)
            //        //{
            //        //    throw new Exception(ex.Message.ToString());
            //        //}


            //    }
            //}
            //finally
            //{
            //    //afterTime = DateTime.Now;
            //    //KillExcel.KillExcelProcess(beforeTime, afterTime);
            //    //KillExcel.KillExcelProcess();
            //}
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void InitGrid()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataColumn dc;
            DataRow dr;

            //---------------------添加字段----------------

            //建立字段1
            dc = new DataColumn();
            dc.DataType = System.Type.GetType("System.Int32");
            dc.ColumnName = "id";
            dc.AutoIncrement = true;//标识
            dc.AutoIncrementSeed = 1;//标识种子
            dc.AutoIncrementStep = 1;//标识递增量
            dt.Columns.Add(dc);
            //建立字段2
            dc = new DataColumn();
            dc.DataType = System.Type.GetType("System.String");
            dc.ColumnName = "修正";
            dc.AllowDBNull = true;
            dt.Columns.Add(dc);
            //建立字段3
            dc = new DataColumn();
            dc.DataType = System.Type.GetType("System.String");
            dc.ColumnName = "具体条件";
            dc.AllowDBNull = true;
            dt.Columns.Add(dc);
            //建立字段4
            dc = new DataColumn();
            dc.DataType = System.Type.GetType("System.String");
            dc.ColumnName = "修正选择";
            dc.AllowDBNull = true;
            dt.Columns.Add(dc);
            //建立字段5
            dc = new DataColumn();
            dc.DataType = System.Type.GetType("System.String");
            dc.ColumnName = "修正系数";
            dc.AllowDBNull = true;
            dt.Columns.Add(dc);
            //设置主键
            DataColumn[] newdc = new DataColumn[1];//可设置多个字段为主键
            newdc[0] = dt.Columns["id"];
            dt.PrimaryKey = newdc;

            //---------------------添加记录----------------
            if (gy == null) return;
            for (int i = 0; i < gy.Range_Xz.Count; i++)
            {
                //记录1
                dr = dt.NewRow();

                dr["修正"] = gy.Range_Xz[i].ToString();

                object tmp;
                tmp = (object)gy.GetType().InvokeMember(string.Format("Range_Jttj_{0}", i + 1), BindingFlags.GetProperty, null, gy, null);
                ArrayList al = (ArrayList)tmp;
                dr["具体条件"] = al[0].ToString();

                tmp = (object)gy.GetType().InvokeMember("Range_Xzsz", BindingFlags.GetProperty, null, gy, null);

                al = (ArrayList)tmp;
                dr["修正选择"] = al[0].ToString();

                tmp = (object)gy.GetType().InvokeMember(string.Format("Range_Xzxs_{0}", i + 1), BindingFlags.GetProperty, null, gy, null);
                al = (ArrayList)tmp;
                dr["修正系数"] = al[0].ToString();
                dt.Rows.Add(dr);
            }

            //----把表添加到DataSet并绑定到DataList----
            ds.Tables.Add(dt);
            ds.Tables[0].TableName = "HomePage";
            this.gridControlGy.DataSource = ds.Tables[0].DefaultView;
        }

        private void repositoryItemComboBox具体条件_EditValueChanged(object sender, EventArgs e)
        {
            int i = (sender as DevExpress.XtraEditors.ComboBoxEdit).SelectedIndex;
            int id = (int)(this.gridView1 as DevExpress.XtraGrid.Views.Grid.GridView).GetFocusedDataRow()["id"];
            DataRow dr = (this.gridView1 as DevExpress.XtraGrid.Views.Grid.GridView).GetFocusedDataRow();
            object tmp = (object)gy.GetType().InvokeMember("Range_Xzsz", BindingFlags.GetProperty, null, gy, null);
            ArrayList al = (ArrayList)tmp;
            dr["修正选择"] = al[i].ToString();

            tmp = (object)gy.GetType().InvokeMember(string.Format("Range_Xzxs_{0}", id), BindingFlags.GetProperty, null, gy, null);
            al = (ArrayList)tmp;
            dr["修正系数"] = al[i].ToString();
        }

        private void repositoryItemComboBox具体条件_Click(object sender, EventArgs e)
        {


        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gy != null)
            {
                int id = (int)(sender as DevExpress.XtraGrid.Views.Grid.GridView).GetFocusedDataRow()["id"];
                this.repositoryItemComboBox具体条件.Items.Clear();
                object tmp;
                tmp = (object)gy.GetType().InvokeMember(string.Format("Range_Jttj_{0}", id), BindingFlags.GetProperty, null, gy, null);
                ArrayList al = (ArrayList)tmp;
                foreach (string str in al)
                {
                    this.repositoryItemComboBox具体条件.Items.Add(str);
                }
            }
        }
    }
}
