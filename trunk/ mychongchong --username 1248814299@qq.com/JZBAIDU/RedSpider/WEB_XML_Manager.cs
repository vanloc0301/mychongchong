namespace RedSpider
{
    using Crawler;
    using RedSpider.db_class;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Drawing;
    using System.Windows.Forms;

    public class WEB_XML_Manager : Form
    {
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private IContainer components = null;
        private CrawlerForm fm2;
        private System.Windows.Forms.GroupBox groupBox1;
        private bool isadd = true;
        private Label label1;
        private Label label2;
        private ListView listView1;
        private TextBox Sourcename;
        private TextBox Sourceurl;

        public WEB_XML_Manager(CrawlerForm fm)
        {
            this.fm2 = fm;
            this.InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.Sourceurl.Text == "")
            {
                MessageBox.Show("请 输 入 源 网 址 ！并以 http:// 开头 !", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else if (this.Sourcename.Text == "")
            {
                MessageBox.Show("请 输 入 源 数 据 名 称 !", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                try
                {
                    if (this.isadd)
                    {
                        new WEB_XML_Source().ADD_XML_Url(this.Sourcename.Text, this.Sourceurl.Text);
                        this.getinfo();
                    }
                    else
                    {
                        new WEB_XML_Source().update_XML_Url(this.Sourcename.Text, this.Sourceurl.Text);
                        this.getinfo();
                        this.isadd = true;
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("请单击选择您需要修改的行，然后修改！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                ListViewItem item = this.listView1.SelectedItems[this.listView1.SelectedItems.Count - 1];
                this.Sourcename.Text = item.SubItems[0].Text;
                this.Sourceurl.Text = item.SubItems[1].Text;
                this.isadd = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string str = this.getselectid();
            if (str == null)
            {
                MessageBox.Show("请单击选择您需要删除的栏目源地址！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                try
                {
                    OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
                    OleDbCommand command = new OleDbCommand("DELETE * FROM WEB_XML_Source where Source_url='" + str + "'", connection);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    this.getinfo();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("请单击选择您需要设置的行！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                try
                {
                    ListViewItem item = this.listView1.SelectedItems[this.listView1.SelectedItems.Count - 1];
                    new WEB_XML_Source().XML_Url_Default(item.SubItems[1].Text.ToString());
                    this.getinfo();
                    this.fm2.WebNode();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                }
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

        private void getinfo()
        {
            OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
            string cmdText = "select * from WEB_XML_Source";
            OleDbCommand command = new OleDbCommand(cmdText, connection);
            connection.Open();
            OleDbDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
            try
            {
                this.listView1.Items.Clear();
                while (reader.Read())
                {
                    string[] items = new string[] { reader.GetString(1), reader.GetString(2), reader.GetString(3) };
                    this.listView1.Items.Add(new ListViewItem(items));
                }
                reader.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private string getselectid()
        {
            if (this.listView1.SelectedItems.Count == 0)
            {
                return null;
            }
            ListViewItem item = this.listView1.SelectedItems[this.listView1.SelectedItems.Count - 1];
            return item.SubItems[1].Text;
        }

        private void InitializeComponent()
        {
            this.groupBox1 = new GroupBox();
            this.button4 = new Button();
            this.button3 = new Button();
            this.button2 = new Button();
            this.button1 = new Button();
            this.listView1 = new ListView();
            this.Sourceurl = new TextBox();
            this.label2 = new Label();
            this.Sourcename = new TextBox();
            this.label1 = new Label();
            this.columnHeader1 = new ColumnHeader();
            this.columnHeader2 = new ColumnHeader();
            this.columnHeader3 = new ColumnHeader();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.listView1);
            this.groupBox1.Controls.Add(this.Sourceurl);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.Sourcename);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x284, 0xc6);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "WEB栏目同步数据源XML设定管理";
            this.button4.Location = new Point(0x1fa, 0x97);
            this.button4.Name = "button4";
            this.button4.Size = new Size(0x79, 0x17);
            this.button4.TabIndex = 8;
            this.button4.Text = "将源设为系统默认";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new EventHandler(this.button4_Click);
            this.button3.Location = new Point(0x1fa, 0x6f);
            this.button3.Name = "button3";
            this.button3.Size = new Size(0x79, 0x17);
            this.button3.TabIndex = 7;
            this.button3.Text = "删除栏目数据源地址";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new EventHandler(this.button3_Click);
            this.button2.Location = new Point(0x1fa, 70);
            this.button2.Name = "button2";
            this.button2.Size = new Size(0x79, 0x17);
            this.button2.TabIndex = 6;
            this.button2.Text = "修改栏目数据源地址";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new EventHandler(this.button2_Click);
            this.button1.Location = new Point(0x24d, 0x1a);
            this.button1.Name = "button1";
            this.button1.Size = new Size(0x31, 0x17);
            this.button1.TabIndex = 5;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new EventHandler(this.button1_Click);
            this.listView1.Columns.AddRange(new ColumnHeader[] { this.columnHeader1, this.columnHeader2, this.columnHeader3 });
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new Point(0x13, 70);
            this.listView1.Name = "listView1";
            this.listView1.Size = new Size(0x1dd, 0x70);
            this.listView1.TabIndex = 4;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = View.Details;
            this.Sourceurl.Location = new Point(0x139, 0x1c);
            this.Sourceurl.Name = "Sourceurl";
            this.Sourceurl.Size = new Size(260, 0x15);
            this.Sourceurl.TabIndex = 3;
            this.Sourceurl.Text = "http://";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(230, 0x1f);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x4d, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "数据源地址：";
            this.Sourcename.Location = new Point(90, 0x1c);
            this.Sourcename.Name = "Sourcename";
            this.Sourcename.Size = new Size(0x7d, 0x15);
            this.Sourcename.TabIndex = 1;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x11, 0x1f);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x4d, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "数据源名称：";
            this.columnHeader1.Text = "数据源名称";
            this.columnHeader1.Width = 0x61;
            this.columnHeader2.Text = "数据源地址";
            this.columnHeader2.Width = 0x148;
            this.columnHeader3.Text = "默认";
            this.columnHeader3.Width = 0x26;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x29d, 0xe2);
            base.Controls.Add(this.groupBox1);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.Name = "WEB_XML_Manager";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "WEB_XML_Manager";
            base.Load += new EventHandler(this.WEB_XML_Manager_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            base.ResumeLayout(false);
        }

        private void WEB_XML_Manager_Load(object sender, EventArgs e)
        {
            this.getinfo();
        }
    }
}

