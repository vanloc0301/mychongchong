using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SkyMap.Net.DAO;
using SkyMap.Net.Gui.Components;
using DevExpress.XtraEditors.Controls;
namespace ZBPM
{
    public partial class ZBPMReportCJYW : SkyMap.Net.DataForms.AbstractDataForm
    {
        private string strZq="";
        private string strYwlx="";
        private string strJyfs="";
        private string strTdyt = "";
        private string strCjhzt = "";
        private string strCjhzt1 = "";
        private string strEnd = " 1=1";
        private string strsql = "";

        public ZBPMReportCJYW()
        {
            InitializeComponent();
        }
        public override bool LoadMe()
        {
            return true;
        }
        private void btn_select_Click(object sender, EventArgs e)
        {
            string strSelect = string.Format(@"SELECT 业务编号, 土地位置坐落, 用地面积, 原土地用途, 交易底价, 保证金标, 权属单位, 宗地地址,业务类型,交易方式,备注,容积率允许值标,dbo.FormatDate(成交价日期) as 成交价日期,地价款,
       现场竞价时间,成交价,竞得人,土地使用证编号集合,图纸编号,出让金, 耕地占用税,业务费,成交后状态,当前工作状态,合同编号 FROM VW_tdzbpm_b where  业务状态= '完成竞价' and 是否成交 = {2} and 现场竞价时间1 between  '{0}'  and '{1}' and  ", m_dateStart.DateTime.ToString("yyyy-MM-dd 00:00:00"), m_dateEnd.DateTime.ToString("yyyy-MM-dd 23:59:59"), Chk_是否成交.Checked ? 1 : 0);
            DataTable dtb;
            GetSql();
            strSelect = strSelect + strsql + " order by 现场竞价时间1 asc";
            dtb = QueryHelper.ExecuteSql("Default",string.Empty, strSelect);
            dtb.TableName = "Report";
            DataSet dts = new DataSet();
            dts.Tables.Add(dtb);
            Microsoft.Reporting.WinForms.ReportParameter parameter = new Microsoft.Reporting.WinForms.ReportParameter("startTime", m_dateStart.DateTime.ToShortDateString());
            Microsoft.Reporting.WinForms.ReportParameter parameter1 = new Microsoft.Reporting.WinForms.ReportParameter("endTime", m_dateEnd.DateTime.ToShortDateString());
            Microsoft.Reporting.WinForms.ReportParameter parameter3 = new Microsoft.Reporting.WinForms.ReportParameter("title", Chk_是否成交.Checked ? "成交业务情况 " + strCjhzt1+"" : "流拍业务情况");
            List<Microsoft.Reporting.WinForms.ReportParameter> ps = new List<Microsoft.Reporting.WinForms.ReportParameter>();
            ps.Add(parameter);
            ps.Add(parameter1);
            ps.Add(parameter3);

            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource = new Microsoft.Reporting.WinForms.ReportDataSource("VW_tdzbpm_b", dtb);
            reportViewer1.Reset();
            reportViewer1.LocalReport.DataSources.Clear();

            // reportViewer1.LocalReport.ReportPath = "ReorptJQYW.rdlc";
            reportViewer1.LocalReport.LoadReportDefinition(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("ZBPM.ReportCJYW.rdlc"));
            reportViewer1.LocalReport.SetParameters(ps);
            reportViewer1.LocalReport.DataSources.Add(reportDataSource);
            reportViewer1.RefreshReport();
        }

        private void ZBPMReportCJYW_Load(object sender, EventArgs e)
        {
            m_dateStart.DateTime = DateTime.Today.AddDays(-30);
            m_dateEnd.DateTime = DateTime.Today;

            //
            //cmb_ZSTOWNSHIP.Properties.DisplayMember = "Name";
            //cmb_ZSTOWNSHIP.Properties.ValueMember = "Code";
            //cmb_ZSTOWNSHIP.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Code", 40, "代码"));
            //cmb_ZSTOWNSHIP.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Name", 80, "名称"));
            //cmb_ZSTOWNSHIP.Properties.DataSource = SkyMap.Net.DAO.DataWordService.FindDataWordsByTypeCode("ZSTOWNSHIP");
            //
            //if (cmb_ZSTOWNSHIP.Properties.DataSource == null)
            //{
            //    DataWordLookUpEditHelper.Init(cmb_ZSTOWNSHIP, "ZSTOWNSHIP", "Name", "Code");
            //}
            //镇区
            CLB_zq.DisplayMember = "Name";
            CLB_zq.ValueMember = "Code";
            CLB_zq.DataSource = SkyMap.Net.DAO.DataWordService.FindDataWordsByTypeCode("ZSTOWNSHIP");
            
            //业务类型
            CLB_ywlx.DisplayMember = "Name";
            CLB_ywlx.ValueMember = "Code";
            CLB_ywlx.DataSource = SkyMap.Net.DAO.DataWordService.FindDataWordsByTypeCode("type");
            
            //交易方式
            CLB_jyfs.DisplayMember = "Name";
            CLB_jyfs.ValueMember = "Code";
            CLB_jyfs.DataSource = SkyMap.Net.DAO.DataWordService.FindDataWordsByTypeCode("exchange");         
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            GetSql();
        }

        public void GetSql()
        {
            textEdit1.Text = "";

            if (Chk_zq.Checked)
            {
                strZq = "";
                int i = 1;
                if (CLB_zq.CheckedItems.Count > 0)
                {

                    foreach (SkyMap.Net.DAO.DataWord item in CLB_zq.CheckedItems)
                    {
                        if (i.ToString() == CLB_zq.CheckedItems.Count.ToString())
                        {
                            strZq = strZq + "'" + item.Code + "'";
                        }
                        else
                        {
                            strZq = strZq + "'" + item.Code + "'" + ",";
                        }
                        i = i + 1;
                    }
                    strZq = "sszq in " + "(" + strZq + ") and ";
                }
            }

            if (Chk_ywlx.Checked)
            {
                strYwlx = "";
                int i = 1;
                if (CLB_ywlx.CheckedItems.Count > 0)
                {
                    foreach (SkyMap.Net.DAO.DataWord item in CLB_ywlx.CheckedItems)
                    {
                        if (i.ToString() == CLB_ywlx.CheckedItems.Count.ToString())
                        {
                            strYwlx = strYwlx + "'" + item.Code + "'";
                        }
                        else
                        {
                            strYwlx = strYwlx + "'" + item.Code + "'" + ",";
                        }

                        i = i + 1;
                    }
                    strYwlx = " 业务类型1 in " + "(" + strYwlx + ") and ";
                }
            }

            if (Chk_jyfs.Checked)
            {
                strJyfs = "";
                int i = 1;
                if (CLB_jyfs.CheckedItems.Count > 0)
                {
                    foreach (SkyMap.Net.DAO.DataWord item in CLB_jyfs.CheckedItems)
                    {
                        if (i.ToString() == CLB_jyfs.CheckedItems.Count.ToString())
                        {
                            strJyfs = strJyfs + "'" + item.Code + "'";
                        }
                        else
                        {
                            strJyfs = strJyfs + "'" + item.Code + "'" + ",";
                        }

                        i = i + 1;
                    }
                    strJyfs = " 交易方式1 in " + "(" + strJyfs + ") and";
                }
            }
            if (chk土地用途.Checked)
            {
                strTdyt = "";
                int i = 1;
                if (CLB_yt.CheckedItems.Count > 0)
                {
                    foreach (object item in CLB_yt.CheckedItems)
                    {
                        if (i.ToString() == CLB_yt.CheckedItems.Count.ToString())
                        {
                            strTdyt = strTdyt + "'" + item.ToString() + "'";
                        }
                        else
                        {
                            strTdyt = strTdyt + "'" + item.ToString() + "'" + ",";
                        }
                        i = i + 1;
                    }
                    strTdyt = " 原土地用途 in " + "(" + strTdyt + ") and";
                }
            }
            if (Chk_是否成交.Checked)
            {
                strCjhzt = "";
                strCjhzt1 = "";
                int i = 1;
                if (cmb成交后状态.CheckedItems.Count > 0)
                {
                    foreach (object item in cmb成交后状态.CheckedItems)
                    {
                        if (i.ToString() == cmb成交后状态.CheckedItems.Count.ToString())
                        {
                            strCjhzt = strCjhzt + "'" + item.ToString() + "'";
                            strCjhzt1 = strCjhzt1  + item.ToString() ;
                        }
                        else
                        {
                            strCjhzt = strCjhzt + "'" + item.ToString() + "'" + ",";
                            strCjhzt1 = strCjhzt1 + item.ToString() + ",";
                        }
                        i = i + 1;
                    }
                    strCjhzt = " 成交后状态 in " + "(" + strCjhzt + ") and";
                    strCjhzt1 = " " + "(" + strCjhzt1 + ") ";
                }
            }
            else
            {
                strCjhzt = "";
                strCjhzt1 = "";
            }

            strsql = strZq + strYwlx + strJyfs + strTdyt + strCjhzt+strEnd;
            //foreach (DevExpress.XtraEditors.Controls.CheckedListBoxItem ss in CLB_zq.Items)
            //{
            //    //if (ss.CheckState.ToString() == CheckState.Checked.ToString())
            //        textEdit1.Text = textEdit1.Text + "+" + ss.Value.ToString();
            //}
            textEdit1.Text = strsql;
        }

        private void Chk_是否成交_CheckedChanged(object sender, EventArgs e)
        {
            if (((DevExpress.XtraEditors.CheckEdit)sender).Checked)
            {
                cmb成交后状态.Visible = true;
            }
            else
            {
                cmb成交后状态.Visible = false;
                cmb成交后状态.Text = "";
            }

        }


    }
}
