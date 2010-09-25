namespace SkyMap.Net.Gui
{
    using Microsoft.Reporting.WinForms;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using SkyMap.Net.Core;

    public class ReportViewDialog : Form
    {
        private IContainer components = null;
        private bool enableMultiPrint = true;
        private bool printed = false;
        private ReportViewer reportViewer;
        private static string templetPath = Path.Combine(PropertyService.DataDirectory, "resources" + Path.DirectorySeparatorChar + "rdlc");

        public event PrintEventHandler Print;

        public ReportViewDialog()
        {
            this.InitializeComponent();
            base.Icon = MessageHelper.GetSystemIcon();
        }

        public void AddDataSource(DataSet dataset)
        {
            foreach (DataTable table in dataset.Tables)
            {
                this.AddDataSource(table.TableName, table);
            }
        }

        public void AddDataSource(string name, object datasource)
        {
            this.reportViewer.LocalReport.DataSources.Add(new ReportDataSource(name, datasource));
        }

        public void AddParameters(Dictionary<string, string> dict)
        {
            List<ReportParameter> parameters = new List<ReportParameter>();
            foreach (KeyValuePair<string, string> pair in dict)
            {
                parameters.Add(new ReportParameter(pair.Key, pair.Value));
            }
            this.reportViewer.LocalReport.SetParameters(parameters);
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
            this.reportViewer = new ReportViewer();
            base.SuspendLayout();
            this.reportViewer.Dock = DockStyle.Fill;
            this.reportViewer.Location = new Point(0, 0);
            this.reportViewer.Name = "reportViewer";
            this.reportViewer.Size = new Size(0x1ba, 0x161);
            this.reportViewer.TabIndex = 0;
            this.reportViewer.Print += new CancelEventHandler(this.reportViewer_Print);
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x1ba, 0x161);
            base.Controls.Add(this.reportViewer);
            base.Name = "ReportViewDialog";
            this.Text = "ReportViewDialog";
            base.Load += new EventHandler(this.ReportViewDialog_Load);
            base.ResumeLayout(false);
        }

        public void LoadReportDefinition(Stream stream)
        {
            this.reportViewer.LocalReport.LoadReportDefinition(stream);
        }

        public void LoadReportDefinition(string file)
        {
            this.reportViewer.LocalReport.ReportPath = Path.Combine(templetPath, file);
        }

        private void ReportViewDialog_Load(object sender, EventArgs e)
        {
            this.reportViewer.RefreshReport();
        }

        private void reportViewer_Print(object sender, CancelEventArgs e)
        {
            if (!(this.enableMultiPrint || !this.printed))
            {
                e.Cancel = true;
                MessageHelper.ShowInfo("该报表或票据不能打印多次!");
            }
            else
            {
                if (this.Print != null)
                {
                    PrintEventArgs args = new PrintEventArgs();
                    this.Print(sender, args);
                    e.Cancel = args.Cancel;
                }
                if (!e.Cancel)
                {
                    this.printed = true;
                }
            }
        }

        public bool EnableMultiPrint
        {
            get
            {
                return this.enableMultiPrint;
            }
            set
            {
                this.enableMultiPrint = value;
            }
        }

        public bool Printed
        {
            get
            {
                return this.printed;
            }
            set
            {
                this.printed = true;
            }
        }
    }
}

