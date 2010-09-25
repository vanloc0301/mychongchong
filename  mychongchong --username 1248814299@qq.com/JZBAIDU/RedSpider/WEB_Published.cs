namespace RedSpider
{
    using RedSpider.db_class;
    using Spider_Global_variables;
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.Data;
    using System.Data.OleDb;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Web;
    using System.Windows.Forms;
    using System.Xml;

    public class WEB_Published : Form
    {
        private Button btnFirstPage;
        private Button btnLastPage;
        private Button btnNextPage;
        private Button btnPreviousPage;
        private Button button1;
        private Button button10;
        private Button button11;
        private Button button12;
        private Button button2;
        private Button button7;
        private Button button8;
        private Button button9;
        private Button buttonGog;
        private CheckBox checkBox1;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader7;
        private ColumnHeader Columnmk;
        private ComboBox comboBox1;
        private ComboBox comboBoxWebb;
        private IContainer components = null;
        private int currentPage;
        private DataGridView dataGrid1;
        private DataSet dss;
        private DataTable dtSource;
        private GroupBox groupBox1;
        private GroupBox groupBox4;
        private GroupBox groupBox5;
        private GroupBox groupBox6;
        private GroupBox groupBox7;
        private ColumnHeader id;
        private Label label16;
        private Label label17;
        private Label label18;
        private Label label19;
        private Label label20;
        private Label label21;
        private Label label22;
        private Label label23;
        private Label label24;
        private Label label25;
        private Label label26;
        private Label label27;
        private Label label28;
        private Label label29;
        private Label label30;
        private Label label31;
        private Label label32;
        private Label label33;
        private ListView listViewRequests;
        private int maxRec;
        private TextBox Module_Name;
        public string Module_Upid = "";
        private TextBox Module_Var;
        private TextBox P_Failure;
        private TextBox P_Success;
        private int PageCount;
        private int pageSize;
        private Panel panel1;
        private TextBox Post_New;
        private TextBox Post_Url;
        private ProgressBar progressBar2;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private RadioButton radioButton3;
        private RadioButton radioButton4;
        private int recNo;
        private ListView RED_Module;
        private System.Windows.Forms.TabControl tabControl4;
        private TabPage tabPage1;
        private TabPage tabPage15;
        private TabPage tabPage16;
        private TabPage tabPage17;
        private TextBox textBox13;
        private TextBox textBox14;
        private TextBox textBox15;
        private TextBox textBox16;
        private TextBox textBox17;
        private TextBox textBox18;
        private TextBox textBox19;
        private TextBox textBox20;
        private TextBox textBox21;
        private TreeView treeView1;
        private TextBox txtDisplayPageNo;
        private TextBox txtPageSize;
        private TextBox url_cookie;
        private string WEB_Post_News;
        private string WEB_Post_Url;
        private WebBrowser webBrowser1;
        private WebBrowser webBrowser2;

        public WEB_Published()
        {
            this.InitializeComponent();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            if (this.CheckFillButton())
            {
                if (this.currentPage == 1)
                {
                    MessageBox.Show("You are at the First Page!");
                }
                else
                {
                    this.currentPage = 1;
                    this.recNo = 0;
                    this.LoadPage();
                }
            }
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            if (this.CheckFillButton())
            {
                if (this.recNo == this.maxRec)
                {
                    MessageBox.Show("You are at the Last Page!");
                }
                else
                {
                    this.currentPage = this.PageCount;
                    this.recNo = this.pageSize * (this.currentPage - 1);
                    this.LoadPage();
                }
            }
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            if (this.CheckFillButton())
            {
                if (this.pageSize == 0)
                {
                    MessageBox.Show("Set the Page Size, and then click the Fill Grid button!");
                }
                else
                {
                    this.currentPage++;
                    if (this.currentPage > this.PageCount)
                    {
                        this.currentPage = this.PageCount;
                        if (this.recNo == this.maxRec)
                        {
                            MessageBox.Show("You are at the Last Page!");
                            return;
                        }
                    }
                    this.LoadPage();
                }
            }
        }

        private void btnPreviousPage_Click(object sender, EventArgs e)
        {
            if (this.CheckFillButton())
            {
                if (this.currentPage == this.PageCount)
                {
                    this.recNo = this.pageSize * (this.currentPage - 2);
                }
                this.currentPage--;
                if (this.currentPage < 1)
                {
                    MessageBox.Show("You are at the First Page!");
                    this.currentPage = 1;
                }
                else
                {
                    this.recNo = this.pageSize * (this.currentPage - 1);
                    this.LoadPage();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.post_fb("1");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (this.RED_Module.SelectedItems.Count == 0)
            {
                MessageBox.Show("请 单 击 选 择 您 需 要 修 改 的 行 ！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                ListViewItem item = this.RED_Module.SelectedItems[this.RED_Module.SelectedItems.Count - 1];
                long num = long.Parse(item.SubItems[0].Text);
                ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["RedSpider_Module"];
                OleDbConnection connection = new OleDbConnection(settings.ConnectionString);
                OleDbCommand selectCommand = new OleDbCommand("SELECT * FROM News_Module where id=" + num, connection);
                OleDbDataAdapter adapter = new OleDbDataAdapter(selectCommand);
                DataSet dataSet = new DataSet();
                dataSet.Clear();
                adapter.Fill(dataSet, "News_Module");
                this.Post_Url.Text = dataSet.Tables["News_Module"].Rows[0]["Post_Url"].ToString();
                this.Post_New.Text = dataSet.Tables["News_Module"].Rows[0]["Post_News"].ToString();
                this.P_Success.Text = dataSet.Tables["News_Module"].Rows[0]["P_Success"].ToString();
                this.P_Failure.Text = dataSet.Tables["News_Module"].Rows[0]["P_Failure"].ToString();
                this.Module_Name.Text = dataSet.Tables["News_Module"].Rows[0]["Module_Name"].ToString();
                this.Module_Var.Text = dataSet.Tables["News_Module"].Rows[0]["Module_Var"].ToString();
                this.Module_Upid = dataSet.Tables["News_Module"].Rows[0]["id"].ToString();
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (this.RED_Module.SelectedItems.Count == 0)
            {
                MessageBox.Show("请 单 击 选 择 您 需 要 删 除 的 行 ！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                ListViewItem item = this.RED_Module.SelectedItems[this.RED_Module.SelectedItems.Count - 1];
                new Module_Manager().DEL_Module(item.SubItems[0].Text);
                this.News_Module();
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            this.post_fb("0");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] strArray = this.comboBoxWebb.Text.Replace("http://", "").Split(new char[] { '/' });
            this.url_cookie.Text = strArray[0];
            this.textBox15.Text = this.webBrowser1.Document.Cookie;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (this.Post_Url.Text == "")
            {
                MessageBox.Show("POST URL 必 填 ！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else if (this.Post_New.Text == "")
            {
                MessageBox.Show("POST URL 必 填 ！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else if (this.Module_Name.Text == "")
            {
                MessageBox.Show("模 块 名 必 填 ！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                try
                {
                    string str = this.Post_Url.Text.Replace("'", "''");
                    string str2 = this.Post_New.Text.Replace("'", "''");
                    if (this.Module_Upid == "")
                    {
                        new Module_Manager().ADD_Module(str, str2, this.P_Success.Text, this.P_Failure.Text, this.Module_Name.Text, this.Module_Var.Text);
                        MessageBox.Show("模　块　添　加　成　功　！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.None);
                        this.News_Module();
                    }
                    else
                    {
                        new Module_Manager().UP_Module(str, str2, this.P_Success.Text, this.P_Failure.Text, this.Module_Name.Text, this.Module_Var.Text, this.Module_Upid.ToString());
                        MessageBox.Show("模　块　修　改　成　功　！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.None);
                        this.Module_Upid = "";
                        this.News_Module();
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (this.RED_Module.SelectedItems.Count == 0)
            {
                MessageBox.Show("请 单 击 选 择 您 需 要 设 置 的 行 ！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                ListViewItem item = this.RED_Module.SelectedItems[this.RED_Module.SelectedItems.Count - 1];
                new Module_Manager().DEFAULT_Module(item.SubItems[0].Text);
                this.News_Module();
            }
        }

        private void buttonGog_Click(object sender, EventArgs e)
        {
            string text = this.comboBoxWebb.Text;
            if (!text.StartsWith("http://"))
            {
                text = "http://" + text;
            }
            if (text.IndexOf("/", 8) == -1)
            {
                text = text + '/';
            }
            this.comboBoxWebb.Text = text;
            try
            {
                this.webBrowser1.Url = new Uri(text);
            }
            catch (Exception)
            {
                MessageBox.Show("无效的URL,主机域名无法解析！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private bool CheckFillButton()
        {
            if (this.pageSize == 0)
            {
                MessageBox.Show("没有数据，请先选择数据进行查找！");
                return false;
            }
            return true;
        }

        public void datapost(string url, string postData, string url_Domain, string bt, string P_Success, string cs)
        {
            string str = "";
            try
            {
                StreamReader reader;
                ASCIIEncoding encoding = new ASCIIEncoding();
                CookieContainer container = new CookieContainer();
                string[] strArray = this.textBox15.Text.Split(new char[] { ';' });
                foreach (string str3 in strArray)
                {
                    string[] strArray2 = str3.Split(new char[] { '=' });
                    Cookie cookie = new Cookie(strArray2[0].Trim().ToString(), strArray2[1].Trim().ToString()) {
                        Domain = url_Domain
                    };
                    container.Add(cookie);
                }
                byte[] bytes = encoding.GetBytes(postData);
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.CookieContainer = container;
                request.ContentLength = bytes.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                if (this.radioButton1.Checked)
                {
                    reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
                }
                else
                {
                    reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                }
                string input = reader.ReadToEnd();
                if (cs == "1")
                {
                    this.webBrowser2.DocumentText = input.ToString();
                    this.tabControl4.SelectedTab = this.tabControl4.TabPages[3];
                }
                if (Regex.IsMatch(input, P_Success))
                {
                    ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["RedSpider"];
                    OleDbConnection connection = new OleDbConnection(settings.ConnectionString);
                    OleDbCommand command = new OleDbCommand("update Spider_News SET [News_Published]='已发布' where News_Title='" + bt + "'", connection);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    str = "[恭喜发布成功]";
                    Monitor.Enter(this.listViewRequests);
                    try
                    {
                        this.listViewRequests.Items.Insert(0, bt.ToString()).SubItems.AddRange(new string[] { str.ToString() });
                    }
                    catch (Exception)
                    {
                    }
                    Monitor.Exit(this.listViewRequests);
                }
                else
                {
                    str = "抱歉发布失败,原因:请确保反回成功标识的正确性或先测试一条以便查找原因";
                    Monitor.Enter(this.listViewRequests);
                    try
                    {
                        this.listViewRequests.Items.Insert(0, bt.ToString()).SubItems.AddRange(new string[] { str.ToString() });
                    }
                    catch (Exception)
                    {
                    }
                    Monitor.Exit(this.listViewRequests);
                }
            }
            catch (Exception)
            {
            }
        }

        private void DisplayPageInfo()
        {
            this.txtDisplayPageNo.Text = "Page " + this.currentPage.ToString() + "/ " + this.PageCount.ToString();
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
            this.tabControl4 = new TabControl();
            this.tabPage15 = new TabPage();
            this.label16 = new Label();
            this.webBrowser1 = new WebBrowser();
            this.buttonGog = new Button();
            this.comboBoxWebb = new ComboBox();
            this.tabPage16 = new TabPage();
            this.groupBox1 = new GroupBox();
            this.treeView1 = new TreeView();
            this.groupBox7 = new GroupBox();
            this.txtPageSize = new TextBox();
            this.txtDisplayPageNo = new TextBox();
            this.btnLastPage = new Button();
            this.btnPreviousPage = new Button();
            this.btnNextPage = new Button();
            this.btnFirstPage = new Button();
            this.dataGrid1 = new DataGridView();
            this.groupBox6 = new GroupBox();
            this.panel1 = new Panel();
            this.listViewRequests = new ListView();
            this.columnHeader3 = new ColumnHeader();
            this.columnHeader1 = new ColumnHeader();
            this.button2 = new Button();
            this.url_cookie = new TextBox();
            this.progressBar2 = new ProgressBar();
            this.checkBox1 = new CheckBox();
            this.button1 = new Button();
            this.textBox19 = new TextBox();
            this.label27 = new Label();
            this.textBox16 = new TextBox();
            this.label28 = new Label();
            this.textBox17 = new TextBox();
            this.label29 = new Label();
            this.label30 = new Label();
            this.textBox18 = new TextBox();
            this.button12 = new Button();
            this.radioButton2 = new RadioButton();
            this.radioButton1 = new RadioButton();
            this.label26 = new Label();
            this.label25 = new Label();
            this.textBox15 = new TextBox();
            this.groupBox5 = new GroupBox();
            this.comboBox1 = new ComboBox();
            this.textBox21 = new TextBox();
            this.label33 = new Label();
            this.label32 = new Label();
            this.textBox20 = new TextBox();
            this.label31 = new Label();
            this.radioButton4 = new RadioButton();
            this.radioButton3 = new RadioButton();
            this.tabPage17 = new TabPage();
            this.groupBox4 = new GroupBox();
            this.textBox14 = new TextBox();
            this.label24 = new Label();
            this.button11 = new Button();
            this.button10 = new Button();
            this.button9 = new Button();
            this.textBox13 = new TextBox();
            this.RED_Module = new ListView();
            this.id = new ColumnHeader();
            this.Columnmk = new ColumnHeader();
            this.columnHeader7 = new ColumnHeader();
            this.button8 = new Button();
            this.button7 = new Button();
            this.Module_Var = new TextBox();
            this.label23 = new Label();
            this.Module_Name = new TextBox();
            this.label22 = new Label();
            this.Post_Url = new TextBox();
            this.label17 = new Label();
            this.P_Failure = new TextBox();
            this.P_Success = new TextBox();
            this.label18 = new Label();
            this.label19 = new Label();
            this.label20 = new Label();
            this.Post_New = new TextBox();
            this.label21 = new Label();
            this.tabPage1 = new TabPage();
            this.webBrowser2 = new WebBrowser();
            this.tabControl4.SuspendLayout();
            this.tabPage15.SuspendLayout();
            this.tabPage16.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox7.SuspendLayout();
            ((ISupportInitialize) this.dataGrid1).BeginInit();
            this.groupBox6.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tabPage17.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabPage1.SuspendLayout();
            base.SuspendLayout();
            this.tabControl4.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.tabControl4.Controls.Add(this.tabPage15);
            this.tabControl4.Controls.Add(this.tabPage16);
            this.tabControl4.Controls.Add(this.tabPage17);
            this.tabControl4.Controls.Add(this.tabPage1);
            this.tabControl4.Location = new Point(3, 7);
            this.tabControl4.Name = "tabControl4";
            this.tabControl4.SelectedIndex = 0;
            this.tabControl4.Size = new Size(0x391, 0x284);
            this.tabControl4.TabIndex = 1;
            this.tabPage15.Controls.Add(this.label16);
            this.tabPage15.Controls.Add(this.webBrowser1);
            this.tabPage15.Controls.Add(this.buttonGog);
            this.tabPage15.Controls.Add(this.comboBoxWebb);
            this.tabPage15.Location = new Point(4, 0x15);
            this.tabPage15.Name = "tabPage15";
            this.tabPage15.Padding = new Padding(3);
            this.tabPage15.Size = new Size(0x389, 0x26b);
            this.tabPage15.TabIndex = 0;
            this.tabPage15.Text = "登录";
            this.tabPage15.UseVisualStyleBackColor = true;
            this.label16.AutoSize = true;
            this.label16.Location = new Point(0x17, 12);
            this.label16.Name = "label16";
            this.label16.Size = new Size(0x29, 12);
            this.label16.TabIndex = 15;
            this.label16.Text = "地址：";
            this.webBrowser1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.webBrowser1.Location = new Point(6, 0x2c);
            this.webBrowser1.MinimumSize = new Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new Size(0x37d, 0x238);
            this.webBrowser1.TabIndex = 14;
            this.webBrowser1.Url = new Uri("http://www.baidu.com", UriKind.Absolute);
            this.buttonGog.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.buttonGog.ImageAlign = ContentAlignment.MiddleLeft;
            this.buttonGog.ImageIndex = 0;
            this.buttonGog.Location = new Point(0x30a, 6);
            this.buttonGog.Name = "buttonGog";
            this.buttonGog.Size = new Size(0x2e, 0x18);
            this.buttonGog.TabIndex = 13;
            this.buttonGog.Text = "转到";
            this.buttonGog.Click += new EventHandler(this.buttonGog_Click);
            this.comboBoxWebb.AllowDrop = true;
            this.comboBoxWebb.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.comboBoxWebb.BackColor = Color.WhiteSmoke;
            this.comboBoxWebb.ItemHeight = 12;
            this.comboBoxWebb.Location = new Point(70, 9);
            this.comboBoxWebb.MaxDropDownItems = 20;
            this.comboBoxWebb.Name = "comboBoxWebb";
            this.comboBoxWebb.Size = new Size(0x2be, 20);
            this.comboBoxWebb.TabIndex = 12;
            this.comboBoxWebb.Tag = "Settings";
            this.comboBoxWebb.Text = "http://";
            this.tabPage16.Controls.Add(this.groupBox1);
            this.tabPage16.Controls.Add(this.groupBox7);
            this.tabPage16.Controls.Add(this.groupBox6);
            this.tabPage16.Controls.Add(this.groupBox5);
            this.tabPage16.Location = new Point(4, 0x15);
            this.tabPage16.Name = "tabPage16";
            this.tabPage16.Padding = new Padding(3);
            this.tabPage16.Size = new Size(0x389, 0x26b);
            this.tabPage16.TabIndex = 1;
            this.tabPage16.Text = "整理发布";
            this.tabPage16.UseVisualStyleBackColor = true;
            this.groupBox1.Controls.Add(this.treeView1);
            this.groupBox1.Location = new Point(4, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0xc3, 290);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "栏目";
            this.treeView1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.treeView1.Location = new Point(9, 15);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new Size(180, 0x10d);
            this.treeView1.TabIndex = 1;
            this.treeView1.AfterSelect += new TreeViewEventHandler(this.treeView1_AfterSelect);
            this.groupBox7.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.groupBox7.Controls.Add(this.txtPageSize);
            this.groupBox7.Controls.Add(this.txtDisplayPageNo);
            this.groupBox7.Controls.Add(this.btnLastPage);
            this.groupBox7.Controls.Add(this.btnPreviousPage);
            this.groupBox7.Controls.Add(this.btnNextPage);
            this.groupBox7.Controls.Add(this.btnFirstPage);
            this.groupBox7.Controls.Add(this.dataGrid1);
            this.groupBox7.Location = new Point(0xd0, 0x12f);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new Size(0x2b6, 0x137);
            this.groupBox7.TabIndex = 2;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "数据";
            this.txtPageSize.Location = new Point(0x1ce, 0x113);
            this.txtPageSize.Name = "txtPageSize";
            this.txtPageSize.Size = new Size(0x36, 0x15);
            this.txtPageSize.TabIndex = 8;
            this.txtPageSize.Text = "200";
            this.txtDisplayPageNo.Location = new Point(190, 0x113);
            this.txtDisplayPageNo.Name = "txtDisplayPageNo";
            this.txtDisplayPageNo.Size = new Size(0x5b, 0x15);
            this.txtDisplayPageNo.TabIndex = 7;
            this.btnLastPage.Location = new Point(0x179, 0x111);
            this.btnLastPage.Name = "btnLastPage";
            this.btnLastPage.Size = new Size(0x38, 0x17);
            this.btnLastPage.TabIndex = 6;
            this.btnLastPage.Text = "尾页";
            this.btnLastPage.UseVisualStyleBackColor = true;
            this.btnLastPage.Click += new EventHandler(this.btnLastPage_Click);
            this.btnPreviousPage.Location = new Point(310, 0x113);
            this.btnPreviousPage.Name = "btnPreviousPage";
            this.btnPreviousPage.Size = new Size(0x38, 0x17);
            this.btnPreviousPage.TabIndex = 5;
            this.btnPreviousPage.Text = "上一页";
            this.btnPreviousPage.UseVisualStyleBackColor = true;
            this.btnPreviousPage.Click += new EventHandler(this.btnPreviousPage_Click);
            this.btnNextPage.Location = new Point(0x73, 0x115);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new Size(0x38, 0x17);
            this.btnNextPage.TabIndex = 3;
            this.btnNextPage.Text = "下一页";
            this.btnNextPage.UseVisualStyleBackColor = true;
            this.btnNextPage.Click += new EventHandler(this.btnNextPage_Click);
            this.btnFirstPage.Location = new Point(0x2f, 0x115);
            this.btnFirstPage.Name = "btnFirstPage";
            this.btnFirstPage.Size = new Size(0x3a, 0x17);
            this.btnFirstPage.TabIndex = 2;
            this.btnFirstPage.Text = "第一页";
            this.btnFirstPage.UseVisualStyleBackColor = true;
            this.btnFirstPage.Click += new EventHandler(this.btnFirstPage_Click);
            this.dataGrid1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.dataGrid1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid1.Location = new Point(9, 20);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.ReadOnly = true;
            this.dataGrid1.RowTemplate.Height = 0x17;
            this.dataGrid1.Size = new Size(0x2a7, 0xf7);
            this.dataGrid1.TabIndex = 1;
            this.groupBox6.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.groupBox6.Controls.Add(this.panel1);
            this.groupBox6.Controls.Add(this.button2);
            this.groupBox6.Controls.Add(this.url_cookie);
            this.groupBox6.Controls.Add(this.progressBar2);
            this.groupBox6.Controls.Add(this.checkBox1);
            this.groupBox6.Controls.Add(this.button1);
            this.groupBox6.Controls.Add(this.textBox19);
            this.groupBox6.Controls.Add(this.label27);
            this.groupBox6.Controls.Add(this.textBox16);
            this.groupBox6.Controls.Add(this.label28);
            this.groupBox6.Controls.Add(this.textBox17);
            this.groupBox6.Controls.Add(this.label29);
            this.groupBox6.Controls.Add(this.label30);
            this.groupBox6.Controls.Add(this.textBox18);
            this.groupBox6.Controls.Add(this.button12);
            this.groupBox6.Controls.Add(this.radioButton2);
            this.groupBox6.Controls.Add(this.radioButton1);
            this.groupBox6.Controls.Add(this.label26);
            this.groupBox6.Controls.Add(this.label25);
            this.groupBox6.Controls.Add(this.textBox15);
            this.groupBox6.Location = new Point(0xcd, 6);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new Size(0x2b6, 0x123);
            this.groupBox6.TabIndex = 1;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "发布";
            this.panel1.Controls.Add(this.listViewRequests);
            this.panel1.Location = new Point(9, 0xc5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x2a7, 0x58);
            this.panel1.TabIndex = 0x22;
            this.listViewRequests.BackColor = Color.WhiteSmoke;
            this.listViewRequests.Columns.AddRange(new ColumnHeader[] { this.columnHeader3, this.columnHeader1 });
            this.listViewRequests.Dock = DockStyle.Top;
            this.listViewRequests.FullRowSelect = true;
            this.listViewRequests.GridLines = true;
            this.listViewRequests.HideSelection = false;
            this.listViewRequests.Location = new Point(0, 0);
            this.listViewRequests.MultiSelect = false;
            this.listViewRequests.Name = "listViewRequests";
            this.listViewRequests.Size = new Size(0x2a7, 0x55);
            this.listViewRequests.TabIndex = 4;
            this.listViewRequests.UseCompatibleStateImageBehavior = false;
            this.listViewRequests.View = View.Details;
            this.columnHeader3.Text = "标题";
            this.columnHeader3.Width = 0x15f;
            this.columnHeader1.Text = "发布状态";
            this.columnHeader1.Width = 0xbc;
            this.button2.Location = new Point(0x232, 15);
            this.button2.Name = "button2";
            this.button2.Size = new Size(0x81, 0x17);
            this.button2.TabIndex = 0x21;
            this.button2.Text = "获取Domain和Cookie";
            this.button2.TextAlign = ContentAlignment.TopLeft;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new EventHandler(this.button2_Click);
            this.url_cookie.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.url_cookie.Location = new Point(13, 0x10);
            this.url_cookie.Name = "url_cookie";
            this.url_cookie.Size = new Size(0x220, 0x15);
            this.url_cookie.TabIndex = 0x20;
            this.progressBar2.Location = new Point(9, 0xa7);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new Size(0x1dc, 0x17);
            this.progressBar2.TabIndex = 0x1f;
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = CheckState.Checked;
            this.checkBox1.Location = new Point(0x19b, 0x88);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(0x66, 0x10);
            this.checkBox1.TabIndex = 0x1d;
            this.checkBox1.Text = "UrlEncode处理";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.button1.Location = new Point(0x1ef, 0xa8);
            this.button1.Name = "button1";
            this.button1.Size = new Size(0x6c, 0x17);
            this.button1.TabIndex = 0x1c;
            this.button1.Text = "测试发布第一条";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new EventHandler(this.button1_Click);
            this.textBox19.Location = new Point(13, 0x86);
            this.textBox19.Name = "textBox19";
            this.textBox19.Size = new Size(0x3f, 0x15);
            this.textBox19.TabIndex = 0x1b;
            this.label27.AutoSize = true;
            this.label27.Location = new Point(0x178, 0x8b);
            this.label27.Name = "label27";
            this.label27.Size = new Size(0x11, 12);
            this.label27.TabIndex = 0x1a;
            this.label27.Text = "条";
            this.textBox16.Location = new Point(0xec, 0x85);
            this.textBox16.Name = "textBox16";
            this.textBox16.Size = new Size(0x2e, 0x15);
            this.textBox16.TabIndex = 0x19;
            this.textBox16.Text = "1";
            this.label28.AutoSize = true;
            this.label28.Location = new Point(0xbd, 0x88);
            this.label28.Name = "label28";
            this.label28.Size = new Size(0x29, 12);
            this.label28.TabIndex = 0x18;
            this.label28.Text = "发布第";
            this.textBox17.Location = new Point(0x137, 0x85);
            this.textBox17.Name = "textBox17";
            this.textBox17.Size = new Size(0x3b, 0x15);
            this.textBox17.TabIndex = 0x17;
            this.label29.AutoSize = true;
            this.label29.Location = new Point(0x120, 0x8b);
            this.label29.Name = "label29";
            this.label29.Size = new Size(0x11, 12);
            this.label29.TabIndex = 0x16;
            this.label29.Text = "至";
            this.label30.AutoSize = true;
            this.label30.Location = new Point(0x59, 0x8b);
            this.label30.Name = "label30";
            this.label30.Size = new Size(0x11, 12);
            this.label30.TabIndex = 0x15;
            this.label30.Text = "共";
            this.textBox18.Location = new Point(0x70, 0x85);
            this.textBox18.Name = "textBox18";
            this.textBox18.Size = new Size(60, 0x15);
            this.textBox18.TabIndex = 20;
            this.button12.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.button12.Location = new Point(0x261, 0xa8);
            this.button12.Name = "button12";
            this.button12.Size = new Size(0x4f, 0x17);
            this.button12.TabIndex = 5;
            this.button12.Text = "发送到网站";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new EventHandler(this.button12_Click);
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new Point(0x287, 0x87);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new Size(0x29, 0x10);
            this.radioButton2.TabIndex = 4;
            this.radioButton2.Text = "UF8";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new Point(0x240, 0x88);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new Size(0x41, 0x10);
            this.radioButton1.TabIndex = 3;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Default";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.label26.AutoSize = true;
            this.label26.Location = new Point(0x217, 0x89);
            this.label26.Name = "label26";
            this.label26.Size = new Size(0x23, 12);
            this.label26.TabIndex = 2;
            this.label26.Text = "编码:";
            this.label25.AutoSize = true;
            this.label25.Location = new Point(11, 40);
            this.label25.Name = "label25";
            this.label25.Size = new Size(0x41, 12);
            this.label25.TabIndex = 1;
            this.label25.Text = "伪造Cookie";
            this.textBox15.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.textBox15.Location = new Point(9, 0x3d);
            this.textBox15.Multiline = true;
            this.textBox15.Name = "textBox15";
            this.textBox15.Size = new Size(0x2aa, 0x3e);
            this.textBox15.TabIndex = 0;
            this.groupBox5.Controls.Add(this.comboBox1);
            this.groupBox5.Controls.Add(this.textBox21);
            this.groupBox5.Controls.Add(this.label33);
            this.groupBox5.Controls.Add(this.label32);
            this.groupBox5.Controls.Add(this.textBox20);
            this.groupBox5.Controls.Add(this.label31);
            this.groupBox5.Controls.Add(this.radioButton4);
            this.groupBox5.Controls.Add(this.radioButton3);
            this.groupBox5.Location = new Point(4, 0x12f);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new Size(0xc4, 0x137);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "整理";
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] { "标题", "导读", "内容", "作者", "来源" });
            this.comboBox1.Location = new Point(0x2d, 0x25);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new Size(0x4e, 20);
            this.comboBox1.TabIndex = 7;
            this.comboBox1.Text = "标题";
            this.textBox21.Location = new Point(0x66, 0x4f);
            this.textBox21.Multiline = true;
            this.textBox21.Name = "textBox21";
            this.textBox21.Size = new Size(0x58, 0x61);
            this.textBox21.TabIndex = 6;
            this.label33.AutoSize = true;
            this.label33.Location = new Point(0x69, 60);
            this.label33.Name = "label33";
            this.label33.Size = new Size(0x35, 12);
            this.label33.TabIndex = 5;
            this.label33.Text = "替换后：";
            this.label32.AutoSize = true;
            this.label32.Location = new Point(12, 60);
            this.label32.Name = "label32";
            this.label32.Size = new Size(0x35, 12);
            this.label32.TabIndex = 4;
            this.label32.Text = "替换前：";
            this.textBox20.Location = new Point(11, 0x4f);
            this.textBox20.Multiline = true;
            this.textBox20.Name = "textBox20";
            this.textBox20.Size = new Size(0x57, 0x61);
            this.textBox20.TabIndex = 3;
            this.label31.AutoSize = true;
            this.label31.Location = new Point(10, 0x29);
            this.label31.Name = "label31";
            this.label31.Size = new Size(0x1d, 12);
            this.label31.TabIndex = 2;
            this.label31.Text = "替换";
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new Point(0x57, 0x12);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new Size(0x47, 0x10);
            this.radioButton4.TabIndex = 1;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "当前栏目";
            this.radioButton4.UseVisualStyleBackColor = true;
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new Point(10, 0x12);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new Size(0x47, 0x10);
            this.radioButton3.TabIndex = 0;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "所有栏目";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.tabPage17.Controls.Add(this.groupBox4);
            this.tabPage17.Location = new Point(4, 0x15);
            this.tabPage17.Name = "tabPage17";
            this.tabPage17.Size = new Size(0x389, 0x26b);
            this.tabPage17.TabIndex = 2;
            this.tabPage17.Text = "模块配置";
            this.tabPage17.UseVisualStyleBackColor = true;
            this.groupBox4.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.groupBox4.Controls.Add(this.textBox14);
            this.groupBox4.Controls.Add(this.label24);
            this.groupBox4.Controls.Add(this.button11);
            this.groupBox4.Controls.Add(this.button10);
            this.groupBox4.Controls.Add(this.button9);
            this.groupBox4.Controls.Add(this.textBox13);
            this.groupBox4.Controls.Add(this.RED_Module);
            this.groupBox4.Controls.Add(this.button8);
            this.groupBox4.Controls.Add(this.button7);
            this.groupBox4.Controls.Add(this.Module_Var);
            this.groupBox4.Controls.Add(this.label23);
            this.groupBox4.Controls.Add(this.Module_Name);
            this.groupBox4.Controls.Add(this.label22);
            this.groupBox4.Controls.Add(this.Post_Url);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.P_Failure);
            this.groupBox4.Controls.Add(this.P_Success);
            this.groupBox4.Controls.Add(this.label18);
            this.groupBox4.Controls.Add(this.label19);
            this.groupBox4.Controls.Add(this.label20);
            this.groupBox4.Controls.Add(this.Post_New);
            this.groupBox4.Controls.Add(this.label21);
            this.groupBox4.Location = new Point(6, 5);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new Size(0x377, 0x25e);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "WEB发布配置";
            this.textBox14.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.textBox14.Location = new Point(0x11d, 0x2e);
            this.textBox14.Name = "textBox14";
            this.textBox14.Size = new Size(0x24d, 0x15);
            this.textBox14.TabIndex = 30;
            this.textBox14.Text = "【栏目ID】【参照ID】【栏目名】【栏目深度】 　　";
            this.label24.AutoSize = true;
            this.label24.Location = new Point(0xd4, 0x31);
            this.label24.Name = "label24";
            this.label24.Size = new Size(0x41, 12);
            this.label24.TabIndex = 0x1d;
            this.label24.Text = "分类参数：";
            this.button11.Location = new Point(0x6c, 0x11c);
            this.button11.Name = "button11";
            this.button11.Size = new Size(0x1b, 0x17);
            this.button11.TabIndex = 0x1c;
            this.button11.Text = "删";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new EventHandler(this.button11_Click);
            this.button10.Location = new Point(0x4a, 0x11c);
            this.button10.Name = "button10";
            this.button10.Size = new Size(0x1c, 0x17);
            this.button10.TabIndex = 0x1b;
            this.button10.Text = "改";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new EventHandler(this.button10_Click);
            this.button9.Location = new Point(7, 0x11c);
            this.button9.Name = "button9";
            this.button9.Size = new Size(0x3d, 0x17);
            this.button9.TabIndex = 0x1a;
            this.button9.Text = "设为默认";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new EventHandler(this.button9_Click);
            this.textBox13.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.textBox13.Location = new Point(0x11d, 0x129);
            this.textBox13.Multiline = true;
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new Size(0x24f, 0x40);
            this.textBox13.TabIndex = 0x19;
            this.textBox13.Text = "【标题】【导读】【内容】【作者】【来源】【网址】【isout】【随机值1】";
            this.RED_Module.Columns.AddRange(new ColumnHeader[] { this.id, this.Columnmk, this.columnHeader7 });
            this.RED_Module.FullRowSelect = true;
            this.RED_Module.GridLines = true;
            this.RED_Module.Location = new Point(7, 0x15);
            this.RED_Module.Name = "RED_Module";
            this.RED_Module.Size = new Size(0xb9, 0xff);
            this.RED_Module.TabIndex = 0x18;
            this.RED_Module.UseCompatibleStateImageBehavior = false;
            this.RED_Module.View = View.Details;
            this.id.Text = "";
            this.id.Width = 1;
            this.Columnmk.Text = "模块名";
            this.Columnmk.Width = 0x7d;
            this.columnHeader7.Text = "默认";
            this.columnHeader7.Width = 0x2e;
            this.button8.Location = new Point(0x272, 530);
            this.button8.Name = "button8";
            this.button8.Size = new Size(0x4b, 0x17);
            this.button8.TabIndex = 0x17;
            this.button8.Text = "重置";
            this.button8.UseVisualStyleBackColor = true;
            this.button7.Location = new Point(0x2e3, 530);
            this.button7.Name = "button7";
            this.button7.Size = new Size(0x4b, 0x17);
            this.button7.TabIndex = 0x16;
            this.button7.Text = "保存";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new EventHandler(this.button7_Click);
            this.Module_Var.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.Module_Var.Location = new Point(0x20c, 0x1c6);
            this.Module_Var.Name = "Module_Var";
            this.Module_Var.Size = new Size(0x161, 0x15);
            this.Module_Var.TabIndex = 0x15;
            this.label23.AutoSize = true;
            this.label23.Location = new Point(0x1dd, 0x1c9);
            this.label23.Name = "label23";
            this.label23.Size = new Size(0x29, 12);
            this.label23.TabIndex = 20;
            this.label23.Text = "版本：";
            this.Module_Name.Location = new Point(0x11f, 0x1c6);
            this.Module_Name.Name = "Module_Name";
            this.Module_Name.Size = new Size(170, 0x15);
            this.Module_Name.TabIndex = 0x13;
            this.label22.AutoSize = true;
            this.label22.Location = new Point(0xd6, 0x1c9);
            this.label22.Name = "label22";
            this.label22.Size = new Size(0x41, 12);
            this.label22.TabIndex = 0x12;
            this.label22.Text = "模块名称：";
            this.Post_Url.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.Post_Url.Location = new Point(0x11b, 14);
            this.Post_Url.Name = "Post_Url";
            this.Post_Url.Size = new Size(0x24f, 0x15);
            this.Post_Url.TabIndex = 0x11;
            this.label17.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.label17.AutoSize = true;
            this.label17.Location = new Point(0xd4, 0x11);
            this.label17.Name = "label17";
            this.label17.Size = new Size(0x41, 12);
            this.label17.TabIndex = 0x10;
            this.label17.Text = "发布地址：";
            this.P_Failure.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.P_Failure.Enabled = false;
            this.P_Failure.Location = new Point(0x2a9, 0x18e);
            this.P_Failure.Multiline = true;
            this.P_Failure.Name = "P_Failure";
            this.P_Failure.Size = new Size(0xc3, 0x30);
            this.P_Failure.TabIndex = 14;
            this.P_Success.Location = new Point(0x11d, 0x18e);
            this.P_Success.Multiline = true;
            this.P_Success.Name = "P_Success";
            this.P_Success.Size = new Size(390, 0x30);
            this.P_Success.TabIndex = 13;
            this.P_Success.Text = "恭喜发布成功.";
            this.label18.AutoSize = true;
            this.label18.Location = new Point(0x2a7, 0x17a);
            this.label18.Name = "label18";
            this.label18.Size = new Size(0x41, 12);
            this.label18.TabIndex = 12;
            this.label18.Text = "失败标识：";
            this.label19.AutoSize = true;
            this.label19.Location = new Point(0x11b, 0x17a);
            this.label19.Name = "label19";
            this.label19.Size = new Size(0x15b, 12);
            this.label19.TabIndex = 11;
            this.label19.Text = "成功标识(很关键,请认真根据测试返回的成功标志填写)写中文：";
            this.label20.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.label20.AutoSize = true;
            this.label20.Location = new Point(0xd5, 300);
            this.label20.Name = "label20";
            this.label20.Size = new Size(0x41, 12);
            this.label20.TabIndex = 3;
            this.label20.Text = "发布参数：";
            this.Post_New.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.Post_New.Location = new Point(0x11b, 0x4c);
            this.Post_New.Multiline = true;
            this.Post_New.Name = "Post_New";
            this.Post_New.Size = new Size(0x24f, 200);
            this.Post_New.TabIndex = 2;
            this.label21.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.label21.AutoSize = true;
            this.label21.Location = new Point(0xd4, 0x4c);
            this.label21.Name = "label21";
            this.label21.Size = new Size(0x41, 12);
            this.label21.TabIndex = 1;
            this.label21.Text = "post参数：";
            this.tabPage1.Controls.Add(this.webBrowser2);
            this.tabPage1.Location = new Point(4, 0x15);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new Size(0x389, 0x26b);
            this.tabPage1.TabIndex = 3;
            this.tabPage1.Text = "测试结果";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.webBrowser2.Dock = DockStyle.Fill;
            this.webBrowser2.Location = new Point(0, 0);
            this.webBrowser2.MinimumSize = new Size(20, 20);
            this.webBrowser2.Name = "webBrowser2";
            this.webBrowser2.Size = new Size(0x389, 0x26b);
            this.webBrowser2.TabIndex = 0;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x396, 650);
            base.Controls.Add(this.tabControl4);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.Name = "WEB_Published";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "WEB_Published";
            base.Load += new EventHandler(this.WEB_Published_Load);
            this.tabControl4.ResumeLayout(false);
            this.tabPage15.ResumeLayout(false);
            this.tabPage15.PerformLayout();
            this.tabPage16.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            ((ISupportInitialize) this.dataGrid1).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.tabPage17.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void linkLabel1_Click(object sender, EventArgs e)
        {
            new Labelled().ShowDialog();
        }

        private void LoadPage()
        {
            int maxRec;
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["RedSpider"];
            OleDbConnection connection = new OleDbConnection(settings.ConnectionString);
            OleDbCommand selectCommand = new OleDbCommand("select News_Title as 标题,News_Review as 导读,News_Content as 内容,News_Author as 作者,News_Source as 来源,News_Url as 网址 ,Columns_id as 栏目ID,Parent_id as 参照ID,Columns_name as 栏目名,code as 栏目深度,isout as isout from Spider_News where Columns_id='" + Global.COLUMN_ID + "' and News_Published='未发布' order by id desc", connection);
            OleDbDataAdapter adapter = new OleDbDataAdapter(selectCommand);
            this.dss = new DataSet();
            this.dss.Clear();
            adapter.Fill(this.dss, "Spider_News");
            this.dtSource = this.dss.Tables["Spider_News"];
            DataTable table = this.dtSource.Clone();
            table.Clear();
            if (this.currentPage == this.PageCount)
            {
                maxRec = this.maxRec;
            }
            else
            {
                maxRec = this.pageSize * this.currentPage;
            }
            int recNo = this.recNo;
            try
            {
                for (int i = recNo; i < maxRec; i++)
                {
                    table.ImportRow(this.dtSource.Rows[i]);
                    this.recNo++;
                }
            }
            catch
            {
            }
            this.dataGrid1.DataSource = table;
            connection.Close();
            this.DisplayPageInfo();
        }

        private void News_Module()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["RedSpider_Module"];
            OleDbConnection connection = new OleDbConnection(settings.ConnectionString);
            string cmdText = "select * from News_Module";
            OleDbCommand command = new OleDbCommand(cmdText, connection);
            connection.Open();
            OleDbDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
            try
            {
                this.RED_Module.Items.Clear();
                while (reader.Read())
                {
                    string[] items = new string[] { reader.GetInt32(0).ToString(), reader.GetString(5), reader.GetString(7) };
                    this.RED_Module.Items.Add(new ListViewItem(items));
                }
                reader.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        public void newsload()
        {
            this.LoadPage();
            this.pageSize = Convert.ToInt32(this.txtPageSize.Text);
            this.maxRec = this.dtSource.Rows.Count;
            this.textBox18.Text = this.maxRec.ToString();
            this.textBox17.Text = this.maxRec.ToString();
            this.PageCount = this.maxRec / this.pageSize;
            if ((this.maxRec % this.pageSize) > 0)
            {
                this.PageCount++;
            }
            this.currentPage = 1;
            this.recNo = 0;
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

        public void post_fb(string cs)
        {
            this.listViewRequests.Items.Clear();
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["RedSpider_Module"];
            OleDbConnection connection = new OleDbConnection(settings.ConnectionString);
            string cmdText = "SELECT * FROM News_Module where Module_Default='是'";
            OleDbCommand selectCommand = new OleDbCommand(cmdText, connection);
            OleDbDataAdapter adapter = new OleDbDataAdapter(selectCommand);
            DataSet dataSet = new DataSet();
            dataSet.Clear();
            adapter.Fill(dataSet, "News_Module");
            this.WEB_Post_Url = dataSet.Tables["News_Module"].Rows[0]["Post_Url"].ToString();
            this.WEB_Post_News = dataSet.Tables["News_Module"].Rows[0]["Post_News"].ToString();
            string str2 = dataSet.Tables["News_Module"].Rows[0]["P_Success"].ToString();
            if (this.WEB_Post_Url == "")
            {
                MessageBox.Show("没有指定默认数据发布模块！");
            }
            else if (this.textBox15.Text == "")
            {
                MessageBox.Show("请 点 击 [获取Domain和Cookie] !");
            }
            else if (this.url_cookie.Text == "")
            {
                MessageBox.Show("请 点 击 [获取Domain和Cookie] !");
            }
            else
            {
                try
                {
                    if (this.textBox17.Text == "0")
                    {
                        MessageBox.Show(" 无 数 据！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else if (int.Parse(this.textBox17.Text) > int.Parse(this.textBox18.Text))
                    {
                        MessageBox.Show("要 同 步 转 移 的 结 尾 数 据 不 能 大 于 总 条 数 ！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else if (int.Parse(this.textBox16.Text) > int.Parse(this.textBox18.Text))
                    {
                        MessageBox.Show("要 同 步 转 移 的 开 始 数 据 不 能 大 于 总 条 数 ！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else if (int.Parse(this.textBox16.Text) > int.Parse(this.textBox17.Text))
                    {
                        MessageBox.Show("同 步 起 点 数 不 能 大 于 结 尾 数 ！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        if (this.textBox16.Text == "")
                        {
                            this.textBox16.Text = "1";
                        }
                        if (this.textBox17.Text == "")
                        {
                            this.textBox17.Text = this.textBox18.Text;
                        }
                        int num = int.Parse(this.textBox17.Text);
                        if (cs == "1")
                        {
                            num = 1;
                        }
                        int num2 = int.Parse(this.textBox16.Text) - 1;
                        this.progressBar2.Maximum = num;
                        for (int i = num2; i < num; i++)
                        {
                            if ((i % 100) == 0)
                            {
                                this.progressBar2.Value = i;
                            }
                            string str = this.dss.Tables["Spider_News"].Rows[i]["标题"].ToString();
                            string bt = str;
                            string str5 = this.dss.Tables["Spider_News"].Rows[i]["导读"].ToString();
                            string str6 = this.dss.Tables["Spider_News"].Rows[i]["内容"].ToString();
                            string str7 = this.dss.Tables["Spider_News"].Rows[i]["作者"].ToString();
                            string str8 = this.dss.Tables["Spider_News"].Rows[i]["来源"].ToString();
                            string str9 = this.dss.Tables["Spider_News"].Rows[i]["网址"].ToString();
                            string str10 = this.dss.Tables["Spider_News"].Rows[i]["栏目名"].ToString();
                            string newValue = this.dss.Tables["Spider_News"].Rows[i]["栏目ID"].ToString();
                            string str12 = this.dss.Tables["Spider_News"].Rows[i]["参照ID"].ToString();
                            string str13 = this.dss.Tables["Spider_News"].Rows[i]["栏目深度"].ToString();
                            string str14 = this.dss.Tables["Spider_News"].Rows[i]["isout"].ToString();
                            if (this.checkBox1.Checked)
                            {
                                str = HttpUtility.UrlEncode(str);
                                str5 = HttpUtility.UrlEncode(str5);
                                str6 = HttpUtility.UrlEncode(str6);
                                str7 = HttpUtility.UrlEncode(str7);
                                str8 = HttpUtility.UrlEncode(str8);
                                str10 = HttpUtility.UrlEncode(str10);
                                str9 = HttpUtility.UrlEncode(str9);
                            }
                            string postData = this.WEB_Post_News.Replace("【标题】", str).Replace("【导读】", str5).Replace("【内容】", str6).Replace("【作者】", str7).Replace("【来源】", str8).Replace("【网址】", str9).Replace("【isout】", str14).Replace("【栏目ID】", newValue).Replace("【参照ID】", str12).Replace("【栏目名】", str10).Replace("【栏目深度】", str13);
                            string url = this.WEB_Post_Url.Replace("【栏目ID】", newValue).Replace("【参照ID】", str12).Replace("【栏目名】", str10).Replace("【栏目深度】", str13);
                            this.datapost(url, postData, this.url_cookie.Text, bt, str2, cs);
                        }
                        this.newsload();
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.ToString());
                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Global.COLUMN_NAME = this.treeView1.SelectedNode.Text;
            string[] strArray = this.treeView1.SelectedNode.Tag.ToString().Split(new char[] { ',' });
            for (int i = 0; i < strArray.Length; i++)
            {
                Console.WriteLine(strArray[i]);
            }
            Console.Read();
            Global.COLUMN_ID = strArray[1];
            Global.PARENT_ID = strArray[2];
            Global.CODE = strArray[3];
            Global.WEBXML = true;
            this.textBox19.Text = strArray[0];
            this.newsload();
        }

        private void WEB_Published_Load(object sender, EventArgs e)
        {
            this.News_Module();
            this.WebNode();
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
                Global.WEBXMLID = dataSet.Tables["WEB_XML_Source"].Rows[0]["id"].ToString();
                XmlDocument document = new XmlDataDocument();
                document.Load(dataSet.Tables["WEB_XML_Source"].Rows[0]["Source_url"].ToString());
                this.treeView1.Nodes.Clear();
                this.populateTreeControl(document.DocumentElement, this.treeView1.Nodes);
            }
            catch (Exception)
            {
                MessageBox.Show("同步栏目源XML不能正确解析,请重新修改或设置!", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
    }
}

