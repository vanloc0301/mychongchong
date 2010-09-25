namespace SkyMap.Net.Windows.Test
{
    using SkyMap.Net.DAO;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Util;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.Common;
    using System.Drawing;
    using System.Windows.Forms;

    public class ReportVistorFormTest : Form, IReportVistor
    {
        private Button button1;
        private ComboBox comboBox1;
        private IContainer components;
        private DataSet ds;
        private Label label1;
        private byte[] reportData;

        public ReportVistorFormTest()
        {
            this.InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (((this.ds.Tables.Count > 0) && (this.ds.Tables[0].Columns.Count > 0)) && (this.ds.Tables[0].Rows.Count > 0))
            {
                this.ds.Tables[0].Rows[0][0] = this.comboBox1.SelectedText;
            }
            PrintHelper.PrintOrShowRDLC("Test", true, this.reportData, this.ds, new PrintEventHandler(this.PrintEvent), null, null);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void Execute(byte[] reportData, DataSet ds)
        {
            this.reportData = reportData;
            this.ds = ds;
            base.ShowDialog();
        }

        private void InitializeComponent()
        {
            this.comboBox1 = new ComboBox();
            this.label1 = new Label();
            this.button1 = new Button();
            base.SuspendLayout();
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] { "111111", "222222", "333333", "444444" });
            this.comboBox1.Location = new Point(0x72, 0x16);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new Size(0xbb, 20);
            this.comboBox1.TabIndex = 0;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x18, 0x19);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x4d, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "选择打印编号";
            this.button1.Location = new Point(0xce, 80);
            this.button1.Name = "button1";
            this.button1.Size = new Size(0x5f, 0x22);
            this.button1.TabIndex = 2;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new EventHandler(this.button1_Click);
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(350, 160);
            base.Controls.Add(this.button1);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.comboBox1);
            base.Name = "ReportVistorFormTest";
            this.Text = "ReportVistorFormTest";
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void PrintEvent(object sender, CancelEventArgs e)
        {
            SMDataSource source = QueryHelper.Get<SMDataSource>("SYSTEM_MAIN");
            if (source != null)
            {
                using (DbConnection connection = source.CreateConnection())
                {
                    DbCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "select 1";
                    if (command.ExecuteScalar().ToString() == "1")
                    {
                        MessageBox.Show("测试成功");
                    }
                }
            }
        }
    }
}

