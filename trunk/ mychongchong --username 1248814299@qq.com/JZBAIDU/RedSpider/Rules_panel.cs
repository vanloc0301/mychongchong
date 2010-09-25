namespace RedSpider
{
    using Crawler;
    using Red_Spider_GetInside;
    using RedSpider.db_class;
    using Spider_Global_variables;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Data.OleDb;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using System.Xml;

    public class Rules_panel : Form
    {
        public string _bdbh;
        public string _bm;
        public string _bxbh;
        private TextBox bdbh;
        private TextBox bm;
        private Button button1;
        private Button button10;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private Button button6;
        private Button button7;
        private Button button8;
        private Button button9;
        private TextBox bxbh;
        private string[] c;
        private CheckBox checkBox1;
        private CheckBox checkBox2;
        private TextBox column_name;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader6;
        private ColumnHeader columnHeader8;
        private ColumnHeader columnHeader9;
        private ComboBox comboBox1;
        private ComboBox comboBox2;
        private IContainer components = null;
        private ContextMenuStrip contextMenuStrip1;
        private ContextMenuStrip contextMenuStrip2;
        private DataSet ds = new DataSet();
        private ToolStripMenuItem dToolStripMenuItem;
        private CrawlerForm fm2;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private GroupBox groupBox5;
        private GroupBox groupBox6;
        private GroupBox groupBox7;
        private GroupBox groupBox8;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.ImageList imageList5;
        private Label label1;
        private Label label10;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private ListView listView1;
        private ListView listView3;
        private OpenFileDialog openFileDialog1;
        private Panel panel1;
        public static int ss;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;
        private TextBox textBoxRequest;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripSeparator toolStripMenuItem3;
        private ToolStripSeparator toolStripMenuItem4;
        private TreeView treeView1;
        private TreeView treeView2;
        private TextBox webname;
        private TextBox weburi;
        private ToolStripMenuItem 删除ToolStripMenuItem;
        private ToolStripMenuItem 删除ToolStripMenuItem1;
        private ToolStripMenuItem 修改ToolStripMenuItem;

        public Rules_panel(CrawlerForm fm)
        {
            this.fm2 = fm;
            this.InitializeComponent();
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

        public void AddTree1(int ParentID, TreeNode pNode)
        {
            foreach (DataRowView view2 in new DataView(this.ds.Tables[0]) { RowFilter = "[PARENTID] = " + ParentID })
            {
                TreeNode node;
                if (pNode == null)
                {
                    node = this.treeView1.Nodes.Add(view2["column_name"].ToString());
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
            if (this.webname.Text == "")
            {
                MessageBox.Show("请 填 写 站 点 名 称 ！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else if (this.weburi.Text == "")
            {
                MessageBox.Show("请 填 写 列 表 网 址 ！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else if (Global.UPDATEURL)
            {
                new Web_Url_Manager().UPDATE_Url(this.column_name.Text, this.webname.Text, this.weburi.Text, this.bxbh.Text, this.bdbh.Text, this.bm.Text, this.checkBox1.Checked.ToString());
                this.getinfo();
                Global.UPDATEURL = false;
                this.redspider_column();
                if (this.checkBox1.Checked)
                {
                    this.listView3.Enabled = false;
                }
                else
                {
                    this.listView3.Enabled = true;
                }
                this.fm2.redspider_column();
            }
            else
            {
                new Web_Url_Manager().ADD_Url(this.column_name.Text, this.webname.Text, this.weburi.Text, this.bxbh.Text, this.bdbh.Text, this.bm.Text, this.checkBox1.Checked.ToString());
                this.redspider_column();
                this.getinfo();
                OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
                string cmdText = "SELECT * FROM Web_URL order by id desc";
                OleDbCommand selectCommand = new OleDbCommand(cmdText, connection);
                OleDbDataAdapter adapter = new OleDbDataAdapter(selectCommand);
                DataSet dataSet = new DataSet();
                dataSet.Clear();
                adapter.Fill(dataSet, "Web_URL");
                Global.URLID = dataSet.Tables["Web_URL"].Rows[0]["id"].ToString();
                Global.URL_ADD_OR_UPDATE = true;
                Global.UPDATEURL = false;
                this.Default_Rules(Global.URLID.ToString());
                this.tabControl1.SelectedTab = this.tabControl1.TabPages[1];
                if (this.checkBox1.Checked)
                {
                    this.listView3.Enabled = false;
                }
                else
                {
                    this.listView3.Enabled = true;
                }
                this.fm2.redspider_column();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (this.label9.Text == "选导入栏目")
            {
                MessageBox.Show("请选择网址要导入的分类！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else if (this.textBox2.Text == "")
            {
                MessageBox.Show("请导入或填写网址！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                try
                {
                    string[] strArray = this.textBox2.Text.Split(new char[] { '\n' });
                    int num = strArray.Length - 1;
                    ss = 0;
                    while (ss < num)
                    {
                        int num2 = 1;
                        this.c = strArray[ss].Split(new char[] { ' ' });
                        for (int i = 0; i < this.c.Length; i++)
                        {
                            Console.WriteLine(this.c[i]);
                        }
                        Console.Read();
                        num2 += ss;
                        Application.DoEvents();
                        this.xh(this.c[0], this.c[1]);
                        if (num2 == num)
                        {
                            MessageBox.Show("本次导入任务已顺利完成！");
                        }
                        ss++;
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                }
                this.redspider_column();
                this.redspider_column1();
                this.fm2.redspider_column();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.ql();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Global.URL_BIANHAO = "13";
            this.Default_R();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            new WEB_Published().ShowDialog();
        }

        private void button3_Click_2(object sender, EventArgs e)
        {
            new WEB_Published().ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text == "")
            {
                MessageBox.Show("请　填　写　测　试　网　址　！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                string html = spider_Getinside.GetHtml(this.textBox1.Text, this.comboBox2.SelectedItem.ToString());
                try
                {
                    OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
                    OleDbCommand command = new OleDbCommand("SELECT * FROM RedSpider_Label where url_id='" + Global.URLID.ToString() + "'", connection);
                    connection.Open();
                    OleDbDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                    string input = "";
                    string htmlcode = "";
                    while (reader.Read())
                    {
                        if (reader["zzyes"].ToString() == "是")
                        {
                            htmlcode = "";
                            string[] strArray = reader["B_egin"].ToString().Split(new char[] { ' ' });
                            for (int i = 0; i < strArray.Length; i++)
                            {
                                string zenze = spider_Getinside.Getzenze(html, strArray[i]);
                                htmlcode = htmlcode + zenze;
                            }
                        }
                        else
                        {
                            htmlcode = spider_Getinside.search(html, reader["B_egin"].ToString(), reader["E_nd"].ToString());
                        }
                        if (reader["E_xclusion"].ToString() == "是")
                        {
                            OleDbConnection connection2 = new OleDbConnection(CONN_ACCESS.ConnString);
                            OleDbCommand command2 = new OleDbCommand("SELECT * FROM RedSpider_Exclude_replacement where url_id='" + Global.URL_BIANHAO.ToString() + "' and lable_name='" + reader["Label_name"].ToString() + "' and pc_or_tt='排除'", connection2);
                            connection2.Open();
                            OleDbDataReader reader2 = command2.ExecuteReader(CommandBehavior.CloseConnection);
                            while (reader2.Read())
                            {
                                htmlcode = spider_Getinside.Getpaichi(htmlcode, reader2["pc_tt"].ToString());
                            }
                        }
                        if (reader["R_Parts"].ToString() == "是")
                        {
                            OleDbConnection connection3 = new OleDbConnection(CONN_ACCESS.ConnString);
                            OleDbCommand command3 = new OleDbCommand("SELECT * FROM RedSpider_Exclude_replacement where url_id='" + Global.URL_BIANHAO.ToString() + "' and lable_name='" + reader["Label_name"].ToString() + "' and pc_or_tt='替换'", connection3);
                            connection3.Open();
                            OleDbDataReader reader3 = command3.ExecuteReader(CommandBehavior.CloseConnection);
                            while (reader3.Read())
                            {
                                htmlcode = spider_Getinside.Gettihuan(htmlcode, reader3["pc_tt"].ToString(), reader3["tt_h"].ToString());
                            }
                        }
                        input = input + "【" + reader["Label_name"].ToString() + "】：" + htmlcode + "\r\n";
                    }
                    if (this.checkBox2.Checked)
                    {
                        input = Regex.Replace(Regex.Replace(Regex.Replace(input, "<[^>]*>", ""), " ", ""), "&nbsp;", "");
                    }
                    this.textBoxRequest.Multiline = true;
                    this.textBoxRequest.Text = input;
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Global.LABLE_NAME = "";
            Global.URL_BIANHAO = "";
            new biaoqian(this).ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (this.listView3.SelectedItems.Count == 0)
            {
                MessageBox.Show("请单击选择您需要删除的行，然后删除！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                ListViewItem item = this.listView3.SelectedItems[this.listView3.SelectedItems.Count - 1];
                Global.LABLE_NAME = item.SubItems[1].Text;
                Global.URL_BIANHAO = item.SubItems[0].Text;
                new Web_Url_Manager().DeL_lable();
                this.Default_R();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Default_Rules("100".ToString());
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (this.listView3.SelectedItems.Count == 0)
            {
                MessageBox.Show("请单击选择您需要修改的行，然后修改！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else if (this.listView3.SelectedItems.Count > 0)
            {
                ListViewItem item = this.listView3.SelectedItems[this.listView3.SelectedItems.Count - 1];
                Global.LABLE_NAME = item.SubItems[1].Text;
                Global.URL_BIANHAO = item.SubItems[0].Text;
                new biaoqian(this).ShowDialog();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.textBox3.Text = "";
            this.textBox2.Text = "";
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBox3.Text = this.openFileDialog1.FileName;
                StreamReader reader = new StreamReader(this.openFileDialog1.FileName, Encoding.Default);
                string str = string.Empty;
                while ((str = reader.ReadLine()) != null)
                {
                    this.textBox2.Text = this.textBox2.Text + str + "\r\n";
                }
                this.label10.Text = "共:" + ((this.textBox2.Text.Split(new char[] { '\n' }).Length - 1)).ToString() + "个URL";
            }
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            this.bm.Text = this.comboBox1.SelectedItem.ToString();
            this.bm.Enabled = false;
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
        }

        public void Default_R()
        {
            try
            {
                OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
                OleDbCommand command = new OleDbCommand("select * from RedSpider_Label where url_id='" + Global.URLID.ToString() + "'", connection);
                connection.Open();
                OleDbDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                this.listView3.Items.Clear();
                while (reader.Read())
                {
                    string[] items = new string[] { reader["url_id"].ToString(), reader["Label_name"].ToString(), reader["B_egin"].ToString() };
                    this.listView3.Items.Add(new ListViewItem(items));
                }
                reader.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void Default_Rules(string url_id)
        {
            try
            {
                string str = "标題";
                string str2 = "<title>";
                string str3 = "</title>";
                string str4 = "否";
                string str5 = "否";
                string str6 = "否";
                string str7 = "否";
                string str8 = "";
                for (int i = 1; i < 9; i++)
                {
                    switch (i)
                    {
                        case 2:
                            str = "导读";
                            str2 = "内容TOP500";
                            str3 = "";
                            str4 = "否";
                            str5 = "否";
                            str6 = "否";
                            str7 = "否";
                            str8 = "";
                            break;

                        case 3:
                            str = "内容";
                            str2 = "<body>";
                            str3 = "</body>";
                            str4 = "否";
                            str5 = "否";
                            str6 = "否";
                            str7 = "否";
                            str8 = "";
                            break;

                        case 4:
                            str = "作者";
                            str2 = "作者 编辑 发布人 发稿";
                            str3 = "";
                            str4 = "否";
                            str5 = "否";
                            str6 = "否";
                            str7 = "是";
                            str8 = "";
                            break;

                        case 5:
                            str = "图片";
                            str2 = "图片";
                            str3 = "";
                            str4 = "否";
                            str5 = "否";
                            str6 = "否";
                            str7 = "是";
                            str8 = "";
                            break;

                        case 6:
                            str = "时间";
                            str2 = "时间";
                            str3 = "";
                            str4 = "否";
                            str5 = "否";
                            str6 = "否";
                            str7 = "是";
                            str8 = "";
                            break;

                        case 7:
                            str = "自定义1";
                            str2 = "自定义1";
                            str3 = "";
                            str4 = "否";
                            str5 = "否";
                            str6 = "否";
                            str7 = "是";
                            str8 = "";
                            break;

                        case 8:
                            str = "自定义2";
                            str2 = "自定义2";
                            str3 = "";
                            str4 = "否";
                            str5 = "否";
                            str6 = "否";
                            str7 = "是";
                            str8 = "";
                            break;
                    }
                    OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
                    OleDbCommand command = new OleDbCommand("insert into RedSpider_Label(url_id,Label_name,B_egin,E_nd,F_ixed,E_xclusion,R_Parts,zzyes,zz) values ('" + url_id.ToString() + "','" + str.ToString() + "','" + str2.ToString() + "','" + str3.ToString() + "','" + str4.ToString() + "','" + str5.ToString() + "','" + str6.ToString() + "','" + str7.ToString() + "','" + str8.ToString() + "')", connection);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                OleDbConnection connection2 = new OleDbConnection(CONN_ACCESS.ConnString);
                OleDbCommand command2 = new OleDbCommand("select * from RedSpider_Label where url_id='" + url_id.ToString() + "'", connection2);
                connection2.Open();
                OleDbDataReader reader = command2.ExecuteReader(CommandBehavior.CloseConnection);
                this.listView3.Items.Clear();
                while (reader.Read())
                {
                    string[] items = new string[] { reader.GetString(1), reader.GetString(2), reader.GetString(3) };
                    this.listView3.Items.Add(new ListViewItem(items));
                }
                reader.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void LoadBq(string url_id)
        {
            Global.URLID = url_id;
            OleDbConnection connection2 = new OleDbConnection(CONN_ACCESS.ConnString);
            OleDbCommand command2 = new OleDbCommand("select * from RedSpider_Label where url_id='" + url_id.ToString() + "'", connection2);
            connection2.Open();
            OleDbDataReader reader = command2.ExecuteReader(CommandBehavior.CloseConnection);
            this.listView3.Items.Clear();
            while (reader.Read())
            {
                string[] items = new string[] { reader.GetString(1), reader.GetString(2), reader.GetString(3) };
                this.listView3.Items.Add(new ListViewItem(items));
            }
            reader.Close();
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void dToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.listView3.SelectedItems.Count == 0)
            {
                MessageBox.Show("没有任何项,或需单击选择您需要修改的行，然后修改！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else if (this.listView3.SelectedItems.Count > 0)
            {
                ListViewItem item = this.listView3.SelectedItems[this.listView3.SelectedItems.Count - 1];
                Global.LABLE_NAME = item.SubItems[1].Text;
                Global.URL_BIANHAO = item.SubItems[0].Text;
                new biaoqian(this).ShowDialog();
            }
        }

        private void getinfo()
        {
            try
            {
                long num = long.Parse(Global.COLUMN_ID);
                OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
                OleDbCommand command = new OleDbCommand("select * from Web_URL where id=" + num, connection);
                connection.Open();
                OleDbDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                this.listView1.Items.Clear();
                while (reader.Read())
                {
                    string[] items = new string[] { reader.GetInt32(0).ToString(), reader.GetString(2), reader.GetString(6) };
                    this.listView1.Items.Add(new ListViewItem(items));
                    LoadBq(reader.GetInt32(0).ToString());
                }
                reader.Close();
            }
            catch (Exception)
            {
            }
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Rules_panel));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.treeView2 = new System.Windows.Forms.TreeView();
            this.imageList5 = new System.Windows.Forms.ImageList(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.修改ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.webname = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.column_name = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.weburi = new System.Windows.Forms.TextBox();
            this.bm = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.bxbh = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.bdbh = new System.Windows.Forms.TextBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBoxRequest = new System.Windows.Forms.TextBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.button4 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.listView3 = new System.Windows.Forms.ListView();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader9 = new System.Windows.Forms.ColumnHeader();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.dToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.删除ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.button8 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.button10 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.button9 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.ImageList = this.imageList2;
            this.tabControl1.Location = new System.Drawing.Point(9, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(740, 386);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.ImageIndex = 3;
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(732, 359);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "网址规则";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.treeView2);
            this.groupBox2.Location = new System.Drawing.Point(7, 7);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(157, 348);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "分类列表";
            // 
            // treeView2
            // 
            this.treeView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView2.ImageIndex = 1;
            this.treeView2.ImageList = this.imageList5;
            this.treeView2.Location = new System.Drawing.Point(5, 20);
            this.treeView2.Name = "treeView2";
            this.treeView2.SelectedImageIndex = 0;
            this.treeView2.Size = new System.Drawing.Size(146, 322);
            this.treeView2.TabIndex = 1;
            this.treeView2.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView2_AfterSelect);
            // 
            // imageList5
            // 
            this.imageList5.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList5.ImageStream")));
            this.imageList5.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList5.Images.SetKeyName(0, "READ ME.ico");
            this.imageList5.Images.SetKeyName(1, "BMWDicons.ico");
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.listView1);
            this.groupBox1.Controls.Add(this.webname);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.column_name);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.weburi);
            this.groupBox1.Controls.Add(this.bm);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.bxbh);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.bdbh);
            this.groupBox1.Location = new System.Drawing.Point(170, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(556, 346);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "列表网址规则";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(150, 301);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 25;
            this.button3.Text = "发布设置";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click_2);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(422, 143);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(96, 16);
            this.checkBox1.TabIndex = 24;
            this.checkBox1.Text = "智能网页分析";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(6, 18);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(544, 105);
            this.listView1.TabIndex = 23;
            this.listView1.Tag = "点击鼠标右鍵试试？";
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "编号";
            this.columnHeader1.Width = 0;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "分类名称";
            this.columnHeader2.Width = 135;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "入口URL";
            this.columnHeader3.Width = 396;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.修改ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.删除ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(113, 60);
            // 
            // 修改ToolStripMenuItem
            // 
            this.修改ToolStripMenuItem.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.修改ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("修改ToolStripMenuItem.Image")));
            this.修改ToolStripMenuItem.Name = "修改ToolStripMenuItem";
            this.修改ToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.修改ToolStripMenuItem.Text = " 修　改";
            this.修改ToolStripMenuItem.Click += new System.EventHandler(this.修改ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(109, 6);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(109, 6);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.删除ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("删除ToolStripMenuItem.Image")));
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.删除ToolStripMenuItem.Text = " 删　除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // webname
            // 
            this.webname.Location = new System.Drawing.Point(251, 139);
            this.webname.Name = "webname";
            this.webname.Size = new System.Drawing.Size(152, 21);
            this.webname.TabIndex = 22;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(186, 143);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 21;
            this.label6.Text = "站点名称：";
            // 
            // column_name
            // 
            this.column_name.Enabled = false;
            this.column_name.Location = new System.Drawing.Point(90, 139);
            this.column_name.Name = "column_name";
            this.column_name.Size = new System.Drawing.Size(90, 21);
            this.column_name.TabIndex = 20;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 142);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 19;
            this.label5.Text = "所属分类：";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(331, 301);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 18;
            this.button2.Text = "重　填";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 182);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "入口URL：";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(436, 301);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 17;
            this.button1.Text = "下一步";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // weburi
            // 
            this.weburi.Location = new System.Drawing.Point(90, 179);
            this.weburi.Name = "weburi";
            this.weburi.Size = new System.Drawing.Size(450, 21);
            this.weburi.TabIndex = 0;
            // 
            // bm
            // 
            this.bm.Enabled = false;
            this.bm.Location = new System.Drawing.Point(150, 264);
            this.bm.Name = "bm";
            this.bm.Size = new System.Drawing.Size(123, 21);
            this.bm.TabIndex = 16;
            this.bm.Text = "GB2312";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 226);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "目标网址中必须包含：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 267);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 12);
            this.label4.TabIndex = 15;
            this.label4.Text = "目标网址编码(重要)：";
            // 
            // bxbh
            // 
            this.bxbh.Location = new System.Drawing.Point(150, 223);
            this.bxbh.Name = "bxbh";
            this.bxbh.Size = new System.Drawing.Size(123, 21);
            this.bxbh.TabIndex = 4;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "GB2312",
            "UTF-8",
            "GBK",
            "Unicode",
            "big5 ",
            "hz-gb-2312",
            "iso-8859-1",
            "windows-1257",
            "shift_jis"});
            this.comboBox1.Location = new System.Drawing.Point(285, 265);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(108, 20);
            this.comboBox1.TabIndex = 14;
            this.comboBox1.SelectedValueChanged += new System.EventHandler(this.comboBox1_SelectedValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(280, 226);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "目标网址中不得包含：";
            // 
            // bdbh
            // 
            this.bdbh.Location = new System.Drawing.Point(410, 222);
            this.bdbh.Name = "bdbh";
            this.bdbh.Size = new System.Drawing.Size(130, 21);
            this.bdbh.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.ImageIndex = 1;
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(732, 359);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "内容提取";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox5);
            this.groupBox3.Controls.Add(this.groupBox4);
            this.groupBox3.Location = new System.Drawing.Point(8, 10);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(716, 343);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "目标页分析";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.checkBox2);
            this.groupBox5.Controls.Add(this.panel1);
            this.groupBox5.Controls.Add(this.comboBox2);
            this.groupBox5.Controls.Add(this.button4);
            this.groupBox5.Controls.Add(this.textBox1);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Location = new System.Drawing.Point(265, 18);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(445, 316);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "测试分析器";
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Location = new System.Drawing.Point(268, 21);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(60, 16);
            this.checkBox2.TabIndex = 18;
            this.checkBox2.Text = "滤Html";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBoxRequest);
            this.panel1.Location = new System.Drawing.Point(7, 53);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(430, 257);
            this.panel1.TabIndex = 17;
            // 
            // textBoxRequest
            // 
            this.textBoxRequest.BackColor = System.Drawing.Color.WhiteSmoke;
            this.textBoxRequest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxRequest.Location = new System.Drawing.Point(0, 0);
            this.textBoxRequest.Multiline = true;
            this.textBoxRequest.Name = "textBoxRequest";
            this.textBoxRequest.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxRequest.Size = new System.Drawing.Size(430, 257);
            this.textBoxRequest.TabIndex = 6;
            this.textBoxRequest.WordWrap = false;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "GB2312",
            "UTF-8",
            "GBK",
            "Unicode",
            "big5 ",
            "hz-gb-2312",
            "iso-8859-1",
            "windows-1257",
            "shift_jis"});
            this.comboBox2.Location = new System.Drawing.Point(328, 18);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(63, 20);
            this.comboBox2.TabIndex = 15;
            this.comboBox2.Text = "GB2312";
            this.comboBox2.SelectedValueChanged += new System.EventHandler(this.comboBox2_SelectedValueChanged);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(394, 18);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(45, 23);
            this.button4.TabIndex = 2;
            this.button4.Text = "测试";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(66, 18);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(198, 21);
            this.textBox1.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 21);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "目标URL:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.listView3);
            this.groupBox4.Controls.Add(this.button8);
            this.groupBox4.Controls.Add(this.button7);
            this.groupBox4.Controls.Add(this.button6);
            this.groupBox4.Controls.Add(this.button5);
            this.groupBox4.Location = new System.Drawing.Point(7, 18);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(251, 316);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "规则";
            // 
            // listView3
            // 
            this.listView3.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6,
            this.columnHeader8,
            this.columnHeader9});
            this.listView3.ContextMenuStrip = this.contextMenuStrip2;
            this.listView3.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listView3.FullRowSelect = true;
            this.listView3.GridLines = true;
            this.listView3.Location = new System.Drawing.Point(6, 21);
            this.listView3.Name = "listView3";
            this.listView3.Size = new System.Drawing.Size(156, 167);
            this.listView3.TabIndex = 6;
            this.listView3.UseCompatibleStateImageBehavior = false;
            this.listView3.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "id";
            this.columnHeader6.Width = 7;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "标签";
            this.columnHeader8.Width = 68;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "标记";
            this.columnHeader9.Width = 70;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dToolStripMenuItem,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.删除ToolStripMenuItem1});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(107, 60);
            // 
            // dToolStripMenuItem
            // 
            this.dToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("dToolStripMenuItem.Image")));
            this.dToolStripMenuItem.Name = "dToolStripMenuItem";
            this.dToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.dToolStripMenuItem.Text = "修　改";
            this.dToolStripMenuItem.Click += new System.EventHandler(this.dToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(103, 6);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(103, 6);
            // 
            // 删除ToolStripMenuItem1
            // 
            this.删除ToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("删除ToolStripMenuItem1.Image")));
            this.删除ToolStripMenuItem1.Name = "删除ToolStripMenuItem1";
            this.删除ToolStripMenuItem1.Size = new System.Drawing.Size(106, 22);
            this.删除ToolStripMenuItem1.Text = "删　除";
            this.删除ToolStripMenuItem1.Click += new System.EventHandler(this.删除ToolStripMenuItem1_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(168, 53);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(75, 23);
            this.button8.TabIndex = 5;
            this.button8.Text = "修改标签";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button7
            // 
            this.button7.Enabled = false;
            this.button7.Location = new System.Drawing.Point(168, 129);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 23);
            this.button7.TabIndex = 4;
            this.button7.Text = "默认标签";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(168, 100);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 3;
            this.button6.Text = "删除标签";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(168, 21);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 2;
            this.button5.Text = "增加标签";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox8);
            this.tabPage3.Controls.Add(this.groupBox7);
            this.tabPage3.Controls.Add(this.groupBox6);
            this.tabPage3.Location = new System.Drawing.Point(4, 23);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(732, 359);
            this.tabPage3.TabIndex = 3;
            this.tabPage3.Text = "网址批量管理";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.textBox2);
            this.groupBox8.Location = new System.Drawing.Point(167, 56);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(562, 295);
            this.groupBox8.TabIndex = 7;
            this.groupBox8.TabStop = false;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.textBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox2.Location = new System.Drawing.Point(3, 17);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox2.Size = new System.Drawing.Size(556, 275);
            this.textBox2.TabIndex = 7;
            this.textBox2.WordWrap = false;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.label10);
            this.groupBox7.Controls.Add(this.button10);
            this.groupBox7.Controls.Add(this.label9);
            this.groupBox7.Controls.Add(this.button9);
            this.groupBox7.Controls.Add(this.label8);
            this.groupBox7.Controls.Add(this.textBox3);
            this.groupBox7.Location = new System.Drawing.Point(167, 4);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(562, 48);
            this.groupBox7.TabIndex = 6;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "管理";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(372, 27);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(59, 12);
            this.label10.TabIndex = 6;
            this.label10.Text = "共:0个URL";
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(466, 20);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(75, 23);
            this.button10.TabIndex = 5;
            this.button10.Text = "确定导入";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(18, 21);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(93, 16);
            this.label9.TabIndex = 4;
            this.label9.Text = "选导入栏目";
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(309, 19);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(47, 23);
            this.button9.TabIndex = 3;
            this.button9.Text = "浏览";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(117, 25);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 2;
            this.label8.Text = "文件：";
            // 
            // textBox3
            // 
            this.textBox3.Enabled = false;
            this.textBox3.Location = new System.Drawing.Point(164, 19);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(139, 21);
            this.textBox3.TabIndex = 1;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.treeView1);
            this.groupBox6.Location = new System.Drawing.Point(3, 3);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(157, 348);
            this.groupBox6.TabIndex = 5;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "分类列表";
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.ImageIndex = 1;
            this.treeView1.ImageList = this.imageList5;
            this.treeView1.Location = new System.Drawing.Point(5, 20);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(146, 322);
            this.treeView1.TabIndex = 1;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "Applications InternetExplorer.ico");
            this.imageList2.Images.SetKeyName(1, "SETTINGS.ICO");
            this.imageList2.Images.SetKeyName(2, "downloads.ico");
            this.imageList2.Images.SetKeyName(3, "BROWSER.ICO");
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Spider-Man3.ico");
            this.imageList1.Images.SetKeyName(1, "Crab2.ico");
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Rules_panel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(761, 410);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Rules_panel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "规则面板";
            this.Load += new System.EventHandler(this.Rules_panel_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.button1.Enabled = false;
            if (this.listView1.SelectedItems.Count > 0)
            {
                ListViewItem item = this.listView1.SelectedItems[this.listView1.SelectedItems.Count - 1];
                long num = long.Parse(item.SubItems[0].Text);
                Global.URLID = item.SubItems[0].Text;
                OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
                OleDbCommand selectCommand = new OleDbCommand("SELECT * FROM Web_URL where id=" + num, connection);
                OleDbDataAdapter adapter = new OleDbDataAdapter(selectCommand);
                DataSet dataSet = new DataSet();
                dataSet.Clear();
                adapter.Fill(dataSet, "Web_URL");
                this.webname.Text = dataSet.Tables["Web_URL"].Rows[0]["column_name"].ToString();
                this.weburi.Text = dataSet.Tables["Web_URL"].Rows[0]["web_url"].ToString();
                this.bxbh.Text = dataSet.Tables["Web_URL"].Rows[0]["bxbh"].ToString();
                this.bdbh.Text = dataSet.Tables["Web_URL"].Rows[0]["bdbh"].ToString();
                this.bm.Text = dataSet.Tables["Web_URL"].Rows[0]["bm"].ToString();
                if (dataSet.Tables["Web_URL"].Rows[0]["class"].ToString() == "True")
                {
                    this.checkBox1.Checked = true;
                    this.listView3.Enabled = true;
                }
                else
                {
                    this.checkBox1.Checked = false;
                    this.listView3.Enabled = true;
                }
                long num2 = long.Parse(dataSet.Tables["Web_URL"].Rows[0]["parentid"].ToString());
                OleDbConnection connection2 = new OleDbConnection(CONN_ACCESS.ConnString);
                OleDbCommand command2 = new OleDbCommand("SELECT * FROM Web_URL where id=" + num2, connection2);
                OleDbDataAdapter adapter2 = new OleDbDataAdapter(command2);
                DataSet set2 = new DataSet();
                set2.Clear();
                adapter2.Fill(set2, "Web_URL");
                this.column_name.Text = set2.Tables["Web_URL"].Rows[0]["column_name"].ToString();

               
            }
        }

        private void populateTreeControl(XmlNode document, TreeNodeCollection nodes)
        {
            foreach (XmlNode node in document.ChildNodes)
            {
                string str = (node.Value != null) ? node.Value : (((node.Attributes != null) && (node.Attributes.Count > 0)) ? node.Attributes[0].Value : node.Name);
                string str2 = str;
                string[] strArray = str2.Split(new char[] { ',' });
                for (int i = 0; i < strArray.Length; i++)
                {
                    Console.WriteLine(strArray[i]);
                }
                Console.Read();
                TreeNode node2 = new TreeNode(strArray[0]) {
                    Tag = str2
                };
                nodes.Add(node2);
                this.populateTreeControl(node, node2.Nodes);
            }
        }

        private void ql()
        {
            this.webname.Text = "";
            this.weburi.Text = "";
            this.bxbh.Text = "";
            this.bdbh.Text = "";
            this.bm.Text = "GB2312";
            this.button1.Text = "下一步";
        }

        public void redspider_column()
        {
            OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
            try
            {
                this.ds.Clear();
                this.treeView2.Nodes.Clear();
                connection.Open();
                new OleDbDataAdapter(new OleDbCommand { Connection = connection, CommandText = "select id,column_name,parentid from Web_URL order by id asc", CommandType = CommandType.Text }).Fill(this.ds);
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

        public void redspider_column1()
        {
            OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
            try
            {
                this.ds.Clear();
                this.treeView1.Nodes.Clear();
                connection.Open();
                new OleDbDataAdapter(new OleDbCommand { Connection = connection, CommandText = "select id,column_name,parentid from Web_URL order by id asc", CommandType = CommandType.Text }).Fill(this.ds);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
            finally
            {
                connection.Close();
            }
            this.AddTree1(0, null);
        }

        private void Rules_panel_Load(object sender, EventArgs e)
        {
            Global.UPDATEURL = false;
            this.column_name.Text = Global.COLUMN_NAME;
            this.redspider_column();
            this.redspider_column1();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Global.COLUMN_NAME = this.treeView1.SelectedNode.Text;
            Global.COLUMN_ID = this.treeView1.SelectedNode.Tag.ToString();
            this.label9.Text = Global.COLUMN_NAME;
        }

        private void treeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.button1.Enabled = true;
            this.listView3.Items.Clear();
            Global.COLUMN_NAME = this.treeView2.SelectedNode.Text;
            Global.COLUMN_ID = this.treeView2.SelectedNode.Tag.ToString();
            this.column_name.Text = Global.COLUMN_NAME;
            this.getinfo();
            this.ql();
        }

        public void WebNode()
        {
            try
            {
                OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
                string cmdText = "SELECT * FROM WEB_XML_Source where [Default]='是'";
                OleDbCommand selectCommand = new OleDbCommand(cmdText, connection);
                OleDbDataAdapter adapter = new OleDbDataAdapter(selectCommand);
                DataSet dataSet = new DataSet();
                dataSet.Clear();
                adapter.Fill(dataSet, "WEB_XML_Source");
                Global.webxmlid = dataSet.Tables["WEB_XML_Source"].Rows[0]["id"].ToString();
                XmlDocument document = new XmlDataDocument();
                document.Load(dataSet.Tables["WEB_XML_Source"].Rows[0]["Source_url"].ToString());
                this.treeView2.Nodes.Clear();
                this.populateTreeControl(document.DocumentElement, this.treeView2.Nodes);
            }
            catch (Exception)
            {
                MessageBox.Show("同步栏目源XML不能正确解析,请重新修改或设置!", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void xh(string column_name, string webname)
        {
            new Web_Url_Manager().ADD_Url(this.label9.Text, column_name, webname, this.bxbh.Text, this.bdbh.Text, this.bm.Text, this.checkBox1.Checked.ToString());
            OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
            string cmdText = "SELECT * FROM Web_URL order by id desc";
            OleDbCommand selectCommand = new OleDbCommand(cmdText, connection);
            OleDbDataAdapter adapter = new OleDbDataAdapter(selectCommand);
            DataSet dataSet = new DataSet();
            dataSet.Clear();
            adapter.Fill(dataSet, "Web_URL");
            Global.URLID = dataSet.Tables["Web_URL"].Rows[0]["id"].ToString();
            Global.URL_ADD_OR_UPDATE = true;
            Global.UPDATEURL = false;
            this.Default_Rules(Global.URLID.ToString());
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("请单击选择您需要删除的行，然后删除！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                ListViewItem item = this.listView1.SelectedItems[this.listView1.SelectedItems.Count - 1];
                Global.URLID = item.SubItems[0].Text;
                new Web_Url_Manager().DEL_Url();
                this.redspider_column();
                this.fm2.redspider_column();
            }
        }

        private void 删除ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.listView3.SelectedItems.Count == 0)
            {
                MessageBox.Show("请单击选择您需要删除的行，然后删除！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                ListViewItem item = this.listView3.SelectedItems[this.listView3.SelectedItems.Count - 1];
                Global.LABLE_NAME = item.SubItems[1].Text;
                Global.URL_BIANHAO = item.SubItems[0].Text;
                new Web_Url_Manager().DeL_lable();
                this.Default_R();
            }
        }

        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("没有任何项,或需单击选择您需要修改的行，然后修改！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                this.button1.Enabled = true;
                Global.UPDATEURL = true;
                ListViewItem item = this.listView1.SelectedItems[this.listView1.SelectedItems.Count - 1];
                Global.URLID = item.SubItems[0].Text;
                this.Default_R();
                this.button1.Text = "修 改";
            }
        }

        public static int SS
        {
            get
            {
                return ss;
            }
            set
            {
                ss = value;
            }
        }
    }
}

