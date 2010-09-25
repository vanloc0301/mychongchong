namespace RedSpider
{
    using RedSpider.db_class;
    using Spider_Global_variables;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class Replacement : Form
    {
        private Button button1;
        private Button button2;
        private IContainer components = null;
        private biaoqian fm2;
        private GroupBox groupBox1;
        private Label label1;
        private Label label2;
        private TextBox tth;
        private TextBox ttq;

        public Replacement(biaoqian fm)
        {
            this.fm2 = fm;
            this.InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.ttq.Text = "";
            this.tth.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.ttq.Text == "")
            {
                MessageBox.Show("请　填　替　换　前　内　容 ！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else if (this.tth.Text == "")
            {
                MessageBox.Show("请　填　替　换　后　内　容 ！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                string ttq = this.ttq.Text.Replace("'", "''");
                string tth = this.tth.Text.Replace("'", "''");
                if (Global.PAICU == "")
                {
                    new Web_Url_Manager().ADD_Replacement(ttq, tth);
                }
                else
                {
                    new Web_Url_Manager().UP_Replacement(ttq, tth);
                }
                this.fm2.Replacement_R();
                base.Close();
            }
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
            this.groupBox1 = new GroupBox();
            this.ttq = new TextBox();
            this.tth = new TextBox();
            this.label1 = new Label();
            this.label2 = new Label();
            this.button1 = new Button();
            this.button2 = new Button();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tth);
            this.groupBox1.Controls.Add(this.ttq);
            this.groupBox1.Location = new Point(13, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x15a, 0xa5);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "替换项";
            this.ttq.Location = new Point(12, 0x24);
            this.ttq.Multiline = true;
            this.ttq.Name = "ttq";
            this.ttq.Size = new Size(0x9d, 0x5c);
            this.ttq.TabIndex = 0;
            this.tth.Location = new Point(0xb7, 0x24);
            this.tth.Multiline = true;
            this.tth.Name = "tth";
            this.tth.Size = new Size(150, 0x5c);
            this.tth.TabIndex = 1;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(13, 15);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x35, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "替换前：";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0xba, 15);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x35, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "替换后：";
            this.button1.Location = new Point(0xa4, 0x88);
            this.button1.Name = "button1";
            this.button1.Size = new Size(0x4b, 0x17);
            this.button1.TabIndex = 4;
            this.button1.Text = "重　填";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new EventHandler(this.button1_Click);
            this.button2.Location = new Point(0x105, 0x88);
            this.button2.Name = "button2";
            this.button2.Size = new Size(0x4b, 0x17);
            this.button2.TabIndex = 5;
            this.button2.Text = "保　存";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new EventHandler(this.button2_Click);
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x173, 180);
            base.Controls.Add(this.groupBox1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.Name = "Replacement";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Replacement";
            base.Load += new EventHandler(this.Replacement_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            base.ResumeLayout(false);
        }

        private void R_eplacement()
        {
            if (Global.PAICU == "")
            {
                this.ttq.Text = "";
                this.tth.Text = "";
            }
            else
            {
                this.ttq.Text = Global.PAICU.ToString();
                this.tth.Text = Global.TTH.ToString();
            }
        }

        private void Replacement_Load(object sender, EventArgs e)
        {
            this.R_eplacement();
        }
    }
}

