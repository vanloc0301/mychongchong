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
    public partial class ZBPMReportZBYW : SkyMap.Net.DataForms.AbstractDataForm
    {
        private string strZq = "";
        private string strYwlx = "";
        private string strJyfs = "";
        private string strTdyt = "";
        private string strEnd = " 1=1";
        private string strsql = "";

        public ZBPMReportZBYW()
        {
            InitializeComponent();
        }
        public override bool LoadMe()
        {
            return true;
        }
        private void btn_select_Click(object sender, EventArgs e)
        {
            try
            {
                string strSelect = string.Format(@"SELECT 镇区窗口收件, 镇区分局经办人审核, 分局局长初审, 市局经办人出审批表, 
      副主任审核, 主任审核, 局长, 市局经办人发件, 权属单位, 宗地地址, 业务编号, 
       用地面积, 交易底价, 当前服务器时间,  结案时间
FROM VW_ZBYW where  当前服务器时间 between  '{0}'  and '{1}'  and ", m_dateStart.DateTime.ToString("yyyy-MM-dd 00:00:00"), m_dateEnd.DateTime.ToString("yyyy-MM-dd 23:59:59"));
                DataTable dtb;
                GetSql();
                strSelect = strSelect + strsql + " order by 当前服务器时间 asc";
                dtb = QueryHelper.ExecuteSql("Default", string.Empty, strSelect);
                dtb.TableName = "Report";
                DataSet dts = new DataSet();
                dts.Tables.Add(dtb);
                Microsoft.Reporting.WinForms.ReportParameter parameter = new Microsoft.Reporting.WinForms.ReportParameter("startTime", m_dateStart.DateTime.ToShortDateString());
                Microsoft.Reporting.WinForms.ReportParameter parameter1 = new Microsoft.Reporting.WinForms.ReportParameter("endTime", m_dateEnd.DateTime.ToShortDateString());
                //Microsoft.Reporting.WinForms.ReportParameter parameter3 = new Microsoft.Reporting.WinForms.ReportParameter("title", Chk_是否成交.Checked ? "成交业务情况" : "流拍业务情况");
                List<Microsoft.Reporting.WinForms.ReportParameter> ps = new List<Microsoft.Reporting.WinForms.ReportParameter>();
                ps.Add(parameter);
                ps.Add(parameter1);
                //  ps.Add(parameter3);

                Microsoft.Reporting.WinForms.ReportDataSource reportDataSource = new Microsoft.Reporting.WinForms.ReportDataSource("VW_ZBYW", dtb);
                reportViewer1.Reset();
                reportViewer1.LocalReport.DataSources.Clear();

                // reportViewer1.LocalReport.ReportPath = "ReorptJQYW.rdlc";
                reportViewer1.LocalReport.LoadReportDefinition(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("ZBPM.ReportZBYW.rdlc"));
                reportViewer1.LocalReport.SetParameters(ps);
                reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.StackTrace);
            }
        }

        private void ZBPMReportZBYW_Load(object sender, EventArgs e)
        {
            m_dateStart.DateTime = DateTime.Today.AddDays(-30);
            m_dateEnd.DateTime = DateTime.Today;

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


        public void GetSql()
        {
           //textEdit1.Text = "";

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
                    strZq = " 镇区 in " + "(" + strZq + ") and ";
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
                    strYwlx = " 业务类型 in " + "(" + strYwlx + ") and ";
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
                    strJyfs = " 交易方式 in " + "(" + strJyfs + ") and";
                }
            }
           
            strsql = strZq + strYwlx + strJyfs +   strEnd;
            //foreach (DevExpress.XtraEditors.Controls.CheckedListBoxItem ss in CLB_zq.Items)
            //{
            //    //if (ss.CheckState.ToString() == CheckState.Checked.ToString())
            //        textEdit1.Text = textEdit1.Text + "+" + ss.Value.ToString();
            //}
         
        }
    }
}
