namespace RedSpider
{
    using Lucene.Net.Analysis.KTDictSeg;
    using Lucene.Net.Index;
    using Lucene.Net.Store;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class index_hb : Form
    {
        private Button button1;
        private Button button2;
        private Button button3;
        private IContainer components = null;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox textBox1;
        private TextBox textBox2;
        private FolderBrowserDialog webindex;

        public index_hb()
        {
            this.InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = "";
            if (this.webindex.ShowDialog() == DialogResult.OK)
            {
                this.textBox1.Text = this.webindex.SelectedPath.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = "";
            if (this.webindex.ShowDialog() == DialogResult.OK)
            {
                this.textBox2.Text = this.webindex.SelectedPath.ToString();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.textBox1.Text == "")
                {
                    MessageBox.Show("请选择并合项1");
                }
                else if (this.textBox2.Text == "")
                {
                    MessageBox.Show("请选择并合项2");
                }
                else
                {
                    this.hb();
                    MessageBox.Show("合并成功执行完毕!");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("合并发生错误");
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

        public void hb()
        {
            Directory d = FSDirectory.GetDirectory(this.textBox1.Text, false);
            Directory directory = FSDirectory.GetDirectory(this.textBox2.Text, false);
            IndexWriter writer = new IndexWriter(d, new KTDictSegAnalyzer(), false);
            writer.AddIndexes(new Directory[] { directory });
            writer.Close();
        }

        private void InitializeComponent()
        {
            this.button1 = new Button();
            this.textBox1 = new TextBox();
            this.label1 = new Label();
            this.label2 = new Label();
            this.textBox2 = new TextBox();
            this.button2 = new Button();
            this.button3 = new Button();
            this.label3 = new Label();
            this.webindex = new FolderBrowserDialog();
            base.SuspendLayout();
            this.button1.Location = new Point(0x16a, 9);
            this.button1.Name = "button1";
            this.button1.Size = new Size(40, 0x17);
            this.button1.TabIndex = 0;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new EventHandler(this.button1_Click);
            this.textBox1.Enabled = false;
            this.textBox1.Location = new Point(0x3b, 11);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(0x11d, 0x15);
            this.textBox1.TabIndex = 1;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(12, 0x10);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x29, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "项目1:";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(12, 0x33);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x29, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "项目2:";
            this.textBox2.Enabled = false;
            this.textBox2.Location = new Point(0x3b, 0x2a);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Size(0x11d, 0x15);
            this.textBox2.TabIndex = 4;
            this.button2.Location = new Point(0x16a, 0x2a);
            this.button2.Name = "button2";
            this.button2.Size = new Size(40, 0x17);
            this.button2.TabIndex = 5;
            this.button2.Text = "...";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new EventHandler(this.button2_Click);
            this.button3.Location = new Point(0x147, 0x56);
            this.button3.Name = "button3";
            this.button3.Size = new Size(0x4b, 0x17);
            this.button3.TabIndex = 6;
            this.button3.Text = "执行合并";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new EventHandler(this.button3_Click);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(2, 0x7a);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x1a3, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "提示:合并功能是将\"项目2\"合并到\"项目1\"中(如果项目文件很大,请耐心等待).";
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.AutoValidate = AutoValidate.Disable;
            base.ClientSize = new Size(0x19e, 0x8f);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.button3);
            base.Controls.Add(this.button2);
            base.Controls.Add(this.textBox2);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.textBox1);
            base.Controls.Add(this.button1);
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "index_hb";
            this.Text = "多引擎项目合并";
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}

