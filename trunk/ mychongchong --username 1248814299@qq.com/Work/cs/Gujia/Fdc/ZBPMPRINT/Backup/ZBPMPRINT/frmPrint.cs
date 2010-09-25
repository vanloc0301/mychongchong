using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Reporting.WinForms;
using System.Drawing.Printing;
using System.Drawing.Imaging;
namespace ZBPMPRINT
{
    public partial class frmPrint : Form
    {
        public frmPrint()
        {
            InitializeComponent();
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                for (int i =tabPage1.Controls.Count ; i > 0;i--)
                {
                    tabPage1.Controls.RemoveAt(i-1);
                }
                this.reportViewer.Reset();
                //this.reportViewer.LocalReport.DataSources.Clear();
                if ((sender as ToolStripComboBox).SelectedItem.ToString() == "电汇凭证")
                {
                    tabPage1.Text = "电汇凭证";
                    TelegraphicMoneyOrdertelegraph telegraphicMoneyOrdertelegraph = new TelegraphicMoneyOrdertelegraph();
                    
                    tabPage1.Controls.Add(telegraphicMoneyOrdertelegraph);
                }
                else if ((sender as ToolStripComboBox).SelectedItem.ToString() == "中山市财政局专用缴款书")
                {
                    tabPage1.Text = "中山市财政局专用缴款书";
                    Financial financial = new Financial();
                    tabPage1.Controls.Add(financial);
                }
                else if ((sender as ToolStripComboBox).SelectedItem.ToString() == "中国建设银行支票")
                {
                    tabPage1.Text = "中国建设银行支票";
                    Check check = new Check();
                    tabPage1.Controls.Add(check);
                }
                else if ((sender as ToolStripComboBox).SelectedItem.ToString() == "广东省行政事业单位往来结算票据")
                {
                    tabPage1.Text = "广东省行政事业单位往来结算票据";
                    Balance balance = new Balance();
                    tabPage1.Controls.Add(balance);
                }
                else if ((sender as ToolStripComboBox).SelectedItem.ToString() == "广东省其他非税收入通用票据")
                {
                    tabPage1.Text = "广东省其他非税收入通用票据";
                    Income income = new Income();
                    tabPage1.Controls.Add(income);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripComboBox1_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            toolStripComboBox1.SelectedItem = "广东省其他非税收入通用票据";
            tabPage1.Text = "广东省其他非税收入通用票据";
            Income income = new Income();
            tabPage1.Controls.Add(income);
            this.reportViewer.LocalReport.LoadReportDefinition(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("ZBPMPRINT.Income.rdlc"));
            this.reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("ZBPMPRINT_clsIncome", new clsIncome[] { ((Income)tabPage1.Controls.Find("income", true)[0]).ClsObject }));
            this.reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("ZBPMPRINT_clsItem", ((Income)tabPage1.Controls.Find("income", true)[0]).ClsObject.Items));
            this.reportViewer.RefreshReport();
            
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.reportViewer.Reset();
            if (toolStripComboBox1.SelectedItem.ToString() == "电汇凭证")
            {

                //this.reportViewer.LocalReport.
                this.reportViewer.LocalReport.LoadReportDefinition(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("ZBPMPRINT.TelegraphicMoneyOrdertelegraph.rdlc"));
                this.reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("ZBPMPRINT_clsTelegraphicMoneyOrdertelegraph", new clsTelegraphicMoneyOrdertelegraph[] { ((TelegraphicMoneyOrdertelegraph)tabPage1.Controls.Find("telegraphicMoneyOrdertelegraph", true)[0]).ClsObject }));
               
                this.reportViewer.RefreshReport();
            }
            else if (toolStripComboBox1.SelectedItem.ToString() == "中山市财政局专用缴款书")
            {
             
               this.reportViewer.LocalReport.LoadReportDefinition(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("ZBPMPRINT.Financial.rdlc"));
               this.reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("ZBPMPRINT_clsFinancial", new clsFinancial[] { ((Financial)tabPage1.Controls.Find("financial", true)[0]).ClsObject }));
               this.reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("ZBPMPRINT_clsItem", ((Financial)tabPage1.Controls.Find("financial", true)[0]).ClsObject.Items ));
           
               this.reportViewer.RefreshReport();
            }
            else if (toolStripComboBox1.SelectedItem.ToString() == "中国建设银行支票")
            {
                this.reportViewer.LocalReport.LoadReportDefinition(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("ZBPMPRINT.Check.rdlc"));
                this.reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("ZBPMPRINT_clsCheck", new clsCheck[] { ((Check)tabPage1.Controls.Find("check", true)[0]).ClsObject }));
                this.reportViewer.RefreshReport();
            }
            else if (toolStripComboBox1.SelectedItem.ToString() == "广东省行政事业单位往来结算票据")
            {
                this.reportViewer.LocalReport.LoadReportDefinition(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("ZBPMPRINT.Balance.rdlc"));
                this.reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("ZBPMPRINT_clsBalance", new clsBalance[] { ((Balance)tabPage1.Controls.Find("balance", true)[0]).ClsObject }));
                this.reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("ZBPMPRINT_clsItem", ((Balance)tabPage1.Controls.Find("balance", true)[0]).ClsObject.Items ));
                this.reportViewer.RefreshReport();
            }
            else if (toolStripComboBox1.SelectedItem.ToString() == "广东省其他非税收入通用票据")
            {
                this.reportViewer.LocalReport.LoadReportDefinition(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("ZBPMPRINT.Income.rdlc"));
                this.reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("ZBPMPRINT_clsIncome", new clsIncome[] { ((Income)tabPage1.Controls.Find("income", true)[0]).ClsObject }));
                this.reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("ZBPMPRINT_clsItem", ((Income)tabPage1.Controls.Find("income", true)[0]).ClsObject.Items));
                this.reportViewer.RefreshReport();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            LocalReport report = new LocalReport();
            if (toolStripComboBox1.SelectedItem.ToString() == "电汇凭证")
            {
                report.ReportPath = "TelegraphicMoneyOrdertelegraph.rdlc";
                //report.LoadReportDefinition(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("ZBPMPRINT.TelegraphicMoneyOrdertelegraph.rdlc"));
                report.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("ZBPMPRINT_clsTelegraphicMoneyOrdertelegraph", new clsTelegraphicMoneyOrdertelegraph[] { ((TelegraphicMoneyOrdertelegraph)tabPage1.Controls.Find("telegraphicMoneyOrdertelegraph", true)[0]).ClsObject }));

            }
            else if (toolStripComboBox1.SelectedItem.ToString() == "中山市财政局专用缴款书")
            {
                report.ReportPath = "Financial.rdlc";
               // report.LoadReportDefinition(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("ZBPMPRINT.Financial.rdlc"));
                report.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("ZBPMPRINT_clsFinancial", new clsFinancial[] { ((Financial)tabPage1.Controls.Find("financial", true)[0]).ClsObject }));
                report.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("ZBPMPRINT_clsItem", ((Financial)tabPage1.Controls.Find("financial", true)[0]).ClsObject.Items));

            }
            else if (toolStripComboBox1.SelectedItem.ToString() == "中国建设银行支票")
            {
                report.ReportPath = "Check.rdlc";
               // report.LoadReportDefinition(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("ZBPMPRINT.Check.rdlc"));
                report.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("ZBPMPRINT_clsCheck", new clsCheck[] { ((Check)tabPage1.Controls.Find("check", true)[0]).ClsObject }));
            }
            else if (toolStripComboBox1.SelectedItem.ToString() == "广东省行政事业单位往来结算票据")
            {
                report.ReportPath = "Balance.rdlc";
               // report.LoadReportDefinition(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("ZBPMPRINT.Balance.rdlc"));
                report.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("ZBPMPRINT_clsBalance", new clsBalance[] { ((Balance)tabPage1.Controls.Find("balance", true)[0]).ClsObject }));
                report.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("ZBPMPRINT_clsItem", ((Balance)tabPage1.Controls.Find("balance", true)[0]).ClsObject.Items));
            }
            else if (toolStripComboBox1.SelectedItem.ToString() == "广东省其他非税收入通用票据")
            {
                report.ReportPath = "Income.rdlc";
                //report.LoadReportDefinition(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("ZBPMPRINT.Income.rdlc"));
                report.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("ZBPMPRINT_clsIncome", new clsIncome[] { ((Income)tabPage1.Controls.Find("income", true)[0]).ClsObject }));
                report.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("ZBPMPRINT_clsItem", ((Income)tabPage1.Controls.Find("income", true)[0]).ClsObject.Items));
        
            }
           
            using (clsPrint clsObj = new clsPrint())
            {
                clsObj.Run(report);
            }
        }
    }
}