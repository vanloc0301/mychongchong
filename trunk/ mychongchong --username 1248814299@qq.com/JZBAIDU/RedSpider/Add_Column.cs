namespace RedSpider
{
    using Crawler;
    using RedSpider.db_class;
    using Spider_Global_variables;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Drawing;
    using System.Windows.Forms;

    public class Add_Column : Form
    {
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private TextBox column_name;
        private IContainer components = null;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private DataSet ds = new DataSet();
        private CrawlerForm fm2;
        private GroupBox groupBox1;
        private Label label1;
        private Label label2;
        private TextBox textBox1;
        private TreeView treeView2;
        private ToolStripMenuItem 删除ToolStripMenuItem;

        public Add_Column(CrawlerForm fm)
        {
            this.fm2 = fm;
            this.InitializeComponent();
        }

        private void Add_Column_Load(object sender, EventArgs e)
        {
            this.redspider_column();
        }

        public void AddTree(int ParentID, TreeNode pNode)
        {
            foreach (DataRowView view2 in new DataView(this.ds.Tables[0]) { RowFilter = "[PARENTID] = " + ParentID })
            {
                TreeNode node;
                if (pNode == null)
                {
                    node = this.treeView2.Nodes.Add(view2["column_name"].ToString());
                    this.AddTree(int.Parse(view2["ID"].ToString()), node);
                    node.Tag = view2["ID"].ToString();
                }
                else
                {
                    node = pNode.Nodes.Add(view2["column_name"].ToString());
                    this.AddTree(int.Parse(view2["ID"].ToString()), node);
                    node.Tag = view2["ID"].ToString();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.column_name.Text == "")
            {
                MessageBox.Show("请 输 入 频 道 分 类 名 ！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                new column_manager().add_column(this.column_name.Text.Trim());
                this.column_name.Text = "";
                MessageBox.Show("新 建 频 道 成 功 ！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                try
                {
                    this.fm2.redspider_column();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                }
            }
            this.redspider_column();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.column_name.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text == "")
            {
                MessageBox.Show("请 在　左　边　选　择　父　类　节　点　名 ！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else if (this.textBox1.Text == "")
            {
                MessageBox.Show("请 输 入 频 道 分 类 名 ！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                new column_manager().up_column(this.textBox1.Text.Trim());
                this.column_name.Text = "";
                MessageBox.Show("修 改 完　成 ！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                try
                {
                    this.fm2.redspider_column();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                }
                this.redspider_column();
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
            this.components = new Container();
            this.groupBox1 = new GroupBox();
            this.button4 = new Button();
            this.button3 = new Button();
            this.textBox1 = new TextBox();
            this.label2 = new Label();
            this.treeView2 = new TreeView();
            this.button2 = new Button();
            this.button1 = new Button();
            this.label1 = new Label();
            this.column_name = new TextBox();
            this.contextMenuStrip1 = new ContextMenuStrip(this.components);
            this.删除ToolStripMenuItem = new ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.treeView2);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.column_name);
            this.groupBox1.Location = new Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x1e6, 0x14e);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "分类管理";
            this.button4.Location = new Point(380, 0xa6);
            this.button4.Name = "button4";
            this.button4.Size = new Size(0x4b, 0x17);
            this.button4.TabIndex = 8;
            this.button4.Text = "修改";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new EventHandler(this.button4_Click);
            this.button3.Location = new Point(0x115, 0xa6);
            this.button3.Name = "button3";
            this.button3.Size = new Size(0x4b, 0x17);
            this.button3.TabIndex = 7;
            this.button3.Text = "重　填";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new EventHandler(this.button3_Click);
            this.textBox1.Location = new Point(0x115, 0x7c);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(0xb2, 0x15);
            this.textBox1.TabIndex = 6;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(220, 0x7f);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x1d, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "修改";
            this.treeView2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.treeView2.ContextMenuStrip = this.contextMenuStrip1;
            this.treeView2.Location = new Point(15, 20);
            this.treeView2.Name = "treeView2";
            this.treeView2.Size = new Size(0xa7, 0x134);
            this.treeView2.TabIndex = 4;
            this.treeView2.AfterSelect += new TreeViewEventHandler(this.treeView1_AfterSelect);
            this.button2.Location = new Point(0x115, 0x47);
            this.button2.Name = "button2";
            this.button2.Size = new Size(0x4b, 0x17);
            this.button2.TabIndex = 3;
            this.button2.Text = "重　填";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new EventHandler(this.button2_Click);
            this.button1.Location = new Point(380, 0x47);
            this.button1.Name = "button1";
            this.button1.Size = new Size(0x4b, 0x17);
            this.button1.TabIndex = 2;
            this.button1.Text = "创建";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new EventHandler(this.button1_Click);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0xda, 0x1f);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x1d, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "创建";
            this.column_name.Location = new Point(0x115, 0x1c);
            this.column_name.Name = "column_name";
            this.column_name.Size = new Size(0xb2, 0x15);
            this.column_name.TabIndex = 0;
            this.contextMenuStrip1.Items.AddRange(new ToolStripItem[] { this.删除ToolStripMenuItem });
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new Size(0x99, 0x30);
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new Size(0x98, 0x16);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new EventHandler(this.删除ToolStripMenuItem_Click);
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(510, 0x166);
            base.Controls.Add(this.groupBox1);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            base.MaximizeBox = false;
            base.Name = "Add_Column";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "分类管理";
            base.Load += new EventHandler(this.Add_Column_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        public void redspider_column()
        {
            OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
            try
            {
                this.ds.Clear();
                this.treeView2.Nodes.Clear();
                connection.Open();
                new OleDbDataAdapter(new OleDbCommand { Connection = connection, CommandText = "select id,column_name,parentid from Web_URL where parentid='0' order by id asc", CommandType = CommandType.Text }).Fill(this.ds);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
            finally
            {
                connection.Close();
            }
            this.AddTree(0, null);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Global.COLUMN_NAME = this.treeView2.SelectedNode.Text;
            Global.COLUMN_ID = this.treeView2.SelectedNode.Tag.ToString();
            this.textBox1.Text = this.treeView2.SelectedNode.Text;
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Global.COLUMN_ID.ToString() == "")
            {
                MessageBox.Show("请单击选择您需要删除的类名，然后删除！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else if (MessageBox.Show(this, "操作是不可恢复的,您将删除该分类和分类下的所有网址规则信息?", "核实", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                new column_manager().DeL_column();
                Global.URLID = Global.COLUMN_ID.ToString();
                new Web_Url_Manager().DEL_Url();
                this.redspider_column();
                this.fm2.redspider_column();
            }
        }
    }
}

