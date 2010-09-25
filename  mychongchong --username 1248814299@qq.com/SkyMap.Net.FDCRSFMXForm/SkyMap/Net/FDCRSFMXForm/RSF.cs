namespace SkyMap.Net.FDCRSFMXForm
{
    using Microsoft.Reporting.WinForms;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Security;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;

    public class RSF : Form
    {
        private BindingSource bindingSource1;
        private IContainer components = null;
        private ReportViewer reportViewer1;

        public RSF()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ReportDataSource item = new ReportDataSource();
            this.reportViewer1 = new ReportViewer();
            this.bindingSource1 = new BindingSource(this.components);
            ((ISupportInitialize) this.bindingSource1).BeginInit();
            base.SuspendLayout();
            this.reportViewer1.Dock = DockStyle.Fill;
            item.Name = "ZSZQESFDataSet_YW_ZQRSF";
            item.Value = null;
            this.reportViewer1.LocalReport.DataSources.Add(item);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "SkyMap.Net.FDCRSFMXForm.RSFMX.rdlc";
            this.reportViewer1.Location = new Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new Size(0x2e6, 0x23d);
            this.reportViewer1.TabIndex = 2;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(0xe0, 0xe0, 0xe0);
            base.ClientSize = new Size(0x2e6, 0x23d);
            base.Controls.Add(this.reportViewer1);
            base.MaximizeBox = false;
            base.Name = "RSF";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "镇区二手房统计报表";
            base.Load += new EventHandler(this.RSF_Load);
            ((ISupportInitialize) this.bindingSource1).EndInit();
            base.ResumeLayout(false);
        }

        private void RSF_Load(object sender, EventArgs e)
        {
            DataTable table = QueryHelper.ExecuteSql("FDC", string.Empty, "SELECT PROJECT_ID,Srf,  Zrf,Zq, FdcZl, Tdyt, Fwyt,Zrlx, Zrfs,Ksjg, Jzmj, Bcrmj FROM dbo.YW_ZQRSF " + Class1.aa);
            ReportDataSource item = new ReportDataSource();
            DataSet set = new DataSet();
            table.TableName = "FDC";
            set.Tables.Add(table);
            this.bindingSource1.DataSource = set;
            this.bindingSource1.DataMember = "FDC";
            item.Name = "ZSZQESFDataSet_YW_ZQRSF";
            item.Value = this.bindingSource1;
            this.reportViewer1.LocalReport.DataSources.Add(item);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "SkyMap.Net.FDCRSFMXForm.RSFMX.rdlc";
            ReportParameter parameter = new ReportParameter("CDate", DateTime.Today.ToString("yyyy年MM月dd日"));
            ReportParameter parameter2 = new ReportParameter("CName", SecurityUtil.GetSmIdentity().UserName);
            ReportParameter parameter3 = new ReportParameter("Count", table.Rows.Count.ToString());
            List<ReportParameter> parameters = new List<ReportParameter>();
            parameters.Add(parameter);
            parameters.Add(parameter2);
            parameters.Add(parameter3);
            this.reportViewer1.LocalReport.SetParameters(parameters);
            this.reportViewer1.RefreshReport();
        }
    }
}

