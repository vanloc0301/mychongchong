namespace RedSpider
{
    using RedSpider.db_class;
    using Spider_Global_variables;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class Exclude : Form
    {
        private Button button1;
        private Button button2;
        private IContainer components;
        private TextBox E_xclude;
        private biaoqian fm2;
        private GroupBox groupBox1;

        public Exclude()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public Exclude(biaoqian fm)
        {
            this.components = null;
            this.fm2 = fm;
            this.InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.E_xclude.Text == "")
            {
                MessageBox.Show("请　填　内　容 ！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                string str = this.E_xclude.Text.Replace("'", "''");
                if (Global.PAICU == "")
                {
                    new Web_Url_Manager().ADD_Exclude(str);
                }
                else
                {
                    new Web_Url_Manager().UP_Exclude(str);
                }
                this.fm2.Exclude_R();
                base.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.E_xclude.Text = "";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void Exclude_Load(object sender, EventArgs e)
        {
            this.exlc();
        }

        private void exlc()
        {
            if (Global.PAICU == "")
            {
                this.E_xclude.Text = "";
            }
            else
            {
                this.E_xclude.Text = Global.PAICU.ToString();
            }
        }

        private void InitializeComponent()
        {
            this.button1 = new Button();
            this.groupBox1 = new GroupBox();
            this.button2 = new Button();
            this.E_xclude = new TextBox();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.button1.Location = new Point(0xbc, 0x5c);
            this.button1.Name = "button1";
            this.button1.Size = new Size(0x4b, 0x17);
            this.button1.TabIndex = 0;
            this.button1.Text = "保　存";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new EventHandler(this.button1_Click);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.E_xclude);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new Point(11, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x11e, 120);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "新增排除项";
            this.button2.Location = new Point(0x5f, 0x5c);
            this.button2.Name = "button2";
            this.button2.Size = new Size(0x4b, 0x17);
            this.button2.TabIndex = 3;
            this.button2.Text = "重　填";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new EventHandler(this.button2_Click);
            this.E_xclude.Location = new Point(7, 20);
            this.E_xclude.Multiline = true;
            this.E_xclude.Name = "E_xclude";
            this.E_xclude.Size = new Size(0x110, 0x40);
            this.E_xclude.TabIndex = 2;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x133, 0x86);
            base.Controls.Add(this.groupBox1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "Exclude";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Exclude";
            base.Load += new EventHandler(this.Exclude_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            base.ResumeLayout(false);
        }
    }
}

