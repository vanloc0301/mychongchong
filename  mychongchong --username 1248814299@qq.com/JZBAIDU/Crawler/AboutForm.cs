namespace Crawler
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class AboutForm : Form
    {
        private Button IDOK;
        private Label label1;
        private Label label2;
        private Label label3;

        public AboutForm()
        {
            this.InitializeComponent();
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            base.Dispose(disposing);
        }

        private void IDOK_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void InitializeComponent()
        {
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.IDOK = new Button();
            base.SuspendLayout();
            this.label1.Location = new Point(0x1d, 0x1a);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x150, 0x11);
            this.label1.TabIndex = 0;
            this.label1.Text = "品名：军长搜索";
            this.label2.Location = new Point(0x1d, 0x2e);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x150, 0x11);
            this.label2.TabIndex = 0;
            this.label2.Text = "版本：var1.00 测试版";
            this.label3.Location = new Point(0x1d, 0x45);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x150, 0x11);
            this.label3.TabIndex = 0;
            this.label3.Text = "作者：Bluesky Copyright QQ:7234295 ";
            this.IDOK.Location = new Point(0x141, 20);
            this.IDOK.Name = "IDOK";
            this.IDOK.Size = new Size(90, 0x19);
            this.IDOK.TabIndex = 1;
            this.IDOK.Text = "OK";
            this.IDOK.Click += new EventHandler(this.IDOK_Click);
            this.AutoScaleBaseSize = new Size(6, 14);
            base.ClientSize = new Size(0x1b0, 0x60);
            base.Controls.Add(this.IDOK);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label3);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "AboutForm";
            base.Opacity = 0.95;
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "About";
            base.Load += new EventHandler(this.AboutForm_Load);
            base.ResumeLayout(false);
        }
    }
}

