using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SkyMap.Net.DAO;

namespace ZBPM
{
    public partial class ZBPMReportJQYW : SkyMap.Net.DataForms.AbstractDataForm
    {
        private string strZq = "";
        private string strYwlx = "";
        private string strJyfs = "";
        private string strTdyt = "";
        private string strEndDate = "";
        private string strEnd = " 1=1";
        private string strsql = "";

        public ZBPMReportJQYW()
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
       现场竞价时间,成交价,竞得人,土地使用证编号集合,图纸编号,出让金, 耕地占用税,业务费,成交后状态,当前工作状态,合同编号 FROM VW_tdzbpm_b where  业务状态= '公告中'  and not( (公告起始日期 > '{0}')  or  (公告结束日期 < '{1}') )   and  ", m_dateEnd.DateTime.ToString("yyyy-MM-dd 23:59:59"), m_dateStart.DateTime.ToString("yyyy-MM-dd 00:00:00"));
            GetSql();
            DataTable dtb;
            strSelect = strSelect + strsql + " order by 现场竞价时间1 asc";
            dtb = QueryHelper.ExecuteSql("Default",string.Empty, strSelect);           
            dtb.TableName = "Report";
            DataSet dts = new DataSet();
            dts.Tables.Add(dtb);
            Microsoft.Reporting.WinForms.ReportParameter parameter = new Microsoft.Reporting.WinForms.ReportParameter("startTime", m_dateStart.DateTime.ToShortDateString());
            Microsoft.Reporting.WinForms.ReportParameter parameter1 = new Microsoft.Reporting.WinForms.ReportParameter("endTime", m_dateEnd.DateTime.ToShortDateString());
            List<Microsoft.Reporting.WinForms.ReportParameter> ps = new List<Microsoft.Reporting.WinForms.ReportParameter>();
            ps.Add(parameter);
            ps.Add(parameter1);

            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource = new Microsoft.Reporting.WinForms.ReportDataSource("VW_tdzbpm_b", dtb);
            reportViewer1.Reset();
            reportViewer1.LocalReport.DataSources.Clear();

            // reportViewer1.LocalReport.ReportPath = "ReorptJQYW.rdlc";
            reportViewer1.LocalReport.LoadReportDefinition(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("ZBPM.ReportJQYW.rdlc"));
            reportViewer1.LocalReport.SetParameters(ps);
            reportViewer1.LocalReport.DataSources.Add(reportDataSource);
            reportViewer1.RefreshReport();
        }

        private void ZBPMReportJQYW_Load(object sender, EventArgs e)
        {
            m_dateStart.DateTime = DateTime.Today.AddDays(-30);
            m_dateEnd.DateTime = DateTime.Today;

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

        public void GetSql()
        {
            

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
                    strZq = " sszq in " + "(" + strZq + ") and ";
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
                    strTdyt = " 原土地用途 in " + "(" + strTdyt + ") and ";
                }
            }

            if (Chk_End.Checked)
            {
                strEndDate = string.Format("  公告结束日期 between '{0}' and '{1}' and ", m_dateStart.DateTime.ToString("yyyy-MM-dd 00:00:00"),m_dateEnd.DateTime.ToString("yyyy-MM-dd 23:59:59"));


            }
            else
            {
                strEndDate = "";
            }

            strsql = strZq + strYwlx + strJyfs + strTdyt +strEndDate+ strEnd;
            //foreach (DevExpress.XtraEditors.Controls.CheckedListBoxItem ss in CLB_zq.Items)
            //{
            //    //if (ss.CheckState.ToString() == CheckState.Checked.ToString())
            //        textEdit1.Text = textEdit1.Text + "+" + ss.Value.ToString();
            //}
          
        }
    }
}
