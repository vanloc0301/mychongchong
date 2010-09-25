namespace Crawler
{
    using FTAlgorithm;
    using HTML_CODE_MO;
    using LiteLib;
    using Lucene.Net.Analysis.KTDictSeg;
    using Lucene.Net.Documents;
    using Lucene.Net.Index;
    using Lucene.Net.QueryParsers;
    using Lucene.Net.Search;
    using Microsoft.Win32;
    using Net.LikeShow.ContentAnalyze;
    using Net.LikeShow.ContentAnalyze.DataClass;
    using Red_Spider_GetInside;
    using RedSpider;
    using RedSpider.db_class;
    using Redspider_Components;
    using Spider_Global_variables;
    using Sunisoft.IrisSkin;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Data;
    using System.Data.OleDb;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Management;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Web.Security;
    using System.Windows.Forms;
    using System.Xml;

    public class CrawlerForm : Form
    {
        private ToolStripMenuItem advancedToolStripMenuItem;
        private bool bAllMIMETypes;
        private bool bKeepAlive;
        private bool bKeepSameServer;
        private string bm;
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
        private int cgcount;
        private CheckBox checkBox1;
        private CheckBox checkBox2;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private ColumnHeader columnHeader5;
        private ColumnHeader columnHeader6;
        private ColumnHeader columnHeader7;
        private ColumnHeader columnHeaderDate;
        private ColumnHeader columnHeaderErrorDescription;
        private ColumnHeader columnHeaderErrorID;
        private ColumnHeader columnHeaderErrorItem;
        private ColumnHeader columnHeaderThreadAction;
        private ColumnHeader columnHeaderThreadBytes;
        private ColumnHeader columnHeaderThreadDepth;
        private ColumnHeader columnHeaderTHreadID;
        private ColumnHeader columnHeaderThreadPersentage;
        private ColumnHeader columnHeaderThreadURL;
        private ComboBox comboBox1;
        private ComboBox comboBox2;
        private ComboBox comboBoxWeb;
        private IContainer components;
        private ContextMenu contextMenuBrowse;
        private ContextMenu contextMenuNavigate;
        private ContextMenu contextMenuSettings;
        private ContextMenuStrip contextMenuStrip1;
        private int countfile = 0;
        private PerformanceCounter cpuCounter;
        private OleDbDataAdapter da;
        private DateTimePicker dateTimePicker1;
        private DateTimePicker dateTimePicker2;
        private DateTimePicker dateTimePicker3;
        private DataSet dn = new DataSet();
        private DataSet ds = new DataSet();
        private int dscjwzgs = 1;
        private DataSet dss;
        private DataTable dtSource;
        private Encoding encoding = GetTextEncoding();
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private ImageList imageList1;
        private ImageList imageList2;
        private ImageList imageList3;
        private ImageList imageList4;
        private ImageList imageList5;
        private ImageList imageListPercentage;
        private string[] index_content = new string[15];
        private int index_count = 0;
        private string index_file;
        private string[] index_title = new string[15];
        private int j;
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label15;
        private Label label16;
        private Label label17;
        private Label label18;
        private Label label19;
        private Label label2;
        private Label label20;
        private Label label21;
        private Label label22;
        private Label label24;
        private Label label26;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private int lbytes = 0;
        private int lczl;
        private LinkLabel linkLabel10;
        private LinkLabel linkLabel11;
        private LinkLabel linkLabel12;
        private LinkLabel linkLabel13;
        private LinkLabel linkLabel14;
        private LinkLabel linkLabel15;
        private LinkLabel linkLabel16;
        private LinkLabel linkLabel17;
        private LinkLabel linkLabel18;
        private LinkLabel linkLabel19;
        private LinkLabel linkLabel20;
        private LinkLabel linkLabel21;
        private LinkLabel linkLabel8;
        private LinkLabel linkLabel9;
        private ListView listView1;
        private ListView listViewErrors;
        private ListView listViewRequests;
        private ListView listViewThreads;
        private MenuItem menuItem5;
        private MenuItem menuItemBrowseHttp;
        private MenuItem menuItemCopy;
        private MenuItem menuItemCut;
        private MenuItem menuItemDelete;
        private MenuItem menuItemHttp;
        private MenuItem menuItemPaste;
        private MenuItem menuItemSettingsAdvanced;
        private MenuItem menuItemSettingsConnections;
        private MenuItem menuItemSettingsFileMatches;
        private MenuItem menuItemSettingsOutput;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem mIMETypesToolStripMenuItem;
        private int nByteCount;
        private int nCPUUsage;
        private int nErrorCount;
        private int nFileCount;
        private int nFirstTimeCheckConnection = 0;
        private float nFreeMemory;
        private int nLastRequestCount;
        private NotifyIcon notifyIcon1;
        private int nRequestTimeout;
        private int nSleepConnectTime;
        private int nSleepFetchTime;
        private int nThreadCount;
        private int nURLCount;
        private int nWebDepth;
        private string page_id;
        private string page_name;
        private string page_py;
        private string path;
        private string pic_html;
        private string pic_html_one;
        private System.Collections.Queue queueURLS;
        private PerformanceCounter ramCounter;
        private string save_file;
        private TextBox seva_webindex;
        private string skin;
        private SkinEngine skinEngine1;
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private Splitter splitter3;
        private bool ssjk = false;
        private DateTime start = DateTime.Now;
        private StatusBar statusBar;
        private StatusBarPanel statusBarPanelByteCount;
        private StatusBarPanel statusBarPanelCPU;
        private StatusBarPanel statusBarPanelErrors;
        private StatusBarPanel statusBarPanelFiles;
        private StatusBarPanel statusBarPanelInfo;
        private StatusBarPanel statusBarPanelMem;
        private StatusBarPanel statusBarPanelURLs;
        private string str;
        private string strDownloadfolder;
        private string[] strExcludeFiles;
        private string[] strExcludeHosts;
        private string[] strExcludeWords;
        private string strMIMETypes = GetMIMETypes();
        private TabControl tabControl1;
        private TabControl tabControl2;
        private TabControl tabControlRightView;
        private TabPage tabPage1;
        private TabPage tabPage10;
        private TabPage tabPage11;
        private TabPage tabPage12;
        private TabPage tabPage13;
        private TabPage tabPage14;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage5;
        private TabPage tabPage9;
        private TabPage tabPageErrors;
        private TabPage tabPageRequests;
        private TabPage tabPageThreads;
        private TextBox textBox1;
        private TextBox textBox10;
        private TextBox textBox11;
        private TextBox textBox12;
        private TextBox textBox13;
        private TextBox textBox2;
        private TextBox textBox3;
        private TextBox textBox4;
        private TextBox textBox5;
        private TextBox textBox6;
        private TextBox textBox7;
        private TextBox textBox8;
        private TextBox textBox9;
        private TextBox textBoxErrorDescription;
        private Thread threadParse;
        private Thread[] threadsRun;
        private bool ThreadsRunning;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Timer timerConnectionInfo;
        private System.Windows.Forms.Timer timerMem;
        private ToolBarButton toolBarButton1;
        private ToolBarButton toolBarButton2;
        private ToolBarButton toolBarButton3;
        private ToolBarButton toolBarButton4;
        private ToolBarButton toolBarButton5;
        private ToolBarButton toolBarButton6;
        private ToolBarButton toolBarButtonContinue;
        private ToolBarButton toolBarButtonDeleteAll;
        private ToolBarButton toolBarButtonPause;
        private ToolBarButton toolBarButtonSettings;
        private ToolBarButton toolBarButtonStop;
        private ToolBar toolBarMain;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem toolStripMenuItem10;
        private ToolStripMenuItem toolStripMenuItem11;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripSeparator toolStripMenuItem3;
        private ToolStripSeparator toolStripMenuItem4;
        private ToolStripMenuItem toolStripMenuItem5;
        private ToolStripSeparator toolStripMenuItem7;
        private ToolStripMenuItem toolStripMenuItem8;
        private ToolStripMenuItem toolStripMenuItem9;
        private OleDbDataAdapter towebda;
        private DataSet towebds;
        private TreeView treeView1;
        private string[] urllhost;
        private SortTree urlStorage;
        private string web_html;
        private string web_URL;
        private WebBrowser webBrowser1;
        private FolderBrowserDialog webindex;
        private ContextMenuStrip webtongbu;
        private IndexWriter writer = null;
        private ContextMenuStrip zidinyicalss;
        private string zl_yes = null;
        private ToolStripMenuItem 风格六ToolStripMenuItem;
        private ToolStripMenuItem 风格皮肤ToolStripMenuItem;
        private ToolStripMenuItem 风格七ToolStripMenuItem;
        private ToolStripMenuItem 风格五ToolStripMenuItem;
        private ToolStripMenuItem 开始抓取所有分类ToolStripMenuItem;
        private ToolStripMenuItem 连接ConnectionsToolStripMenuItem;
        private ToolStripMenuItem 删除项目规则ToolStripMenuItem;
        private ToolStripMenuItem 设置SettingsToolStripMenuItem;
        private ToolStripMenuItem 输出OutputToolStripMenuItem;
        private ToolStripMenuItem 添加根节点ToolStripMenuItem;
        private ToolStripMenuItem 添加项目则规ToolStripMenuItem;
        private ToolStripMenuItem 添加子节点ToolStripMenuItem;
        private ToolStripMenuItem 退出系统ToolStripMenuItem;
        private ToolStripMenuItem 退出系统ToolStripMenuItem1;
        private ToolStripMenuItem 系统ToolStripMenuItem;
        private ToolStripMenuItem 系统设定OptionsToolStripMenuItem;
        private ToolStripMenuItem 修改项目规则ToolStripMenuItem;
        private ToolStripMenuItem 原始经典ToolStripMenuItem;
        private ToolStripMenuItem 最大化ToolStripMenuItem;
        private ToolStripMenuItem 最小化程序ToolStripMenuItem;
        private ToolBarButton toolBarButton7;
        private ToolStripMenuItem 最小化程序ToolStripMenuItem1;

        public CrawlerForm()
        {
            this.InitializeComponent();
            this.urlStorage = new SortTree();
            this.threadsRun = new Thread[200];
            this.queueURLS = new System.Collections.Queue();
            this.cpuCounter = new PerformanceCounter();
            this.ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            this.cpuCounter.CategoryName = "Processor";
            this.cpuCounter.CounterName = "% Processor Time";
            this.cpuCounter.InstanceName = "_Total";
        }

        public void A_nalysis(string bt, string dd, string nr, string zz, string ly, string url)
        {
            string str;
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["RedSpider"];
            OleDbConnection connection = new OleDbConnection(settings.ConnectionString);
            OleDbCommand command = new OleDbCommand("SELECT * FROM Spider_News where News_Url='" + url + "'", connection);
            connection.Open();
            if (command.ExecuteReader(CommandBehavior.CloseConnection).Read())
            {
                str = "已排除(重复)";
            }
            else
            {
                ConnectionStringSettings settings2 = ConfigurationManager.ConnectionStrings["RedSpider"];
                OleDbConnection connection2 = new OleDbConnection(settings2.ConnectionString);
                OleDbCommand command2 = new OleDbCommand("insert into Spider_News (Columns_id,Columns_name,Parent_id,code,News_Title,News_Review,News_Content,News_Author,News_Source,News_Url) values ('" + Global.WEB_COLUMN_ID.ToString() + "','" + Global.WEB_COLUMN_NAME.ToString() + "','" + Global.WEB_PARENT_ID.ToString() + "','" + Global.WEB_CODE.ToString() + "','" + bt.ToString() + "','" + dd.ToString() + "','" + nr.ToString() + "','" + zz.ToString() + "','" + ly.ToString() + "','" + url.ToString() + "')", connection2);
                connection2.Open();
                command2.ExecuteNonQuery();
                connection2.Close();
                Global.BT = bt.ToString();
                str = "入库成功";
            }
            this.LogUri(bt, str);
        }

        public void AddTree(int ParentID, TreeNode pNode)
        {
            foreach (DataRowView view2 in new DataView(this.ds.Tables[0]) { RowFilter = "[parentid] = " + ParentID })
            {
                TreeNode node;
                if (pNode == null)
                {
                    node = this.treeView1.Nodes.Add(view2["column_name"].ToString());
                    this.AddTree(int.Parse(view2["id"].ToString()), node);
                    node.Tag = view2["id"].ToString();
                }
                else
                {
                    node = pNode.Nodes.Add(view2["column_name"].ToString());
                    this.AddTree(int.Parse(view2["id"].ToString()), node);
                    node.Tag = view2["id"].ToString();
                }
            }
        }

        private bool AddURL(ref MyUri uri)
        {
            foreach (string str in this.ExcludeHosts)
            {
                if ((str.Trim().Length > 0) && (uri.Host.ToLower().IndexOf(str.Trim()) != -1))
                {
                    this.LogError(uri.AbsoluteUri, "\r\nHost excluded as it includes reserved pattern (" + str + ")");
                    return false;
                }
            }
            Monitor.Enter(this.urlStorage);
            bool flag = false;
            try
            {
                string absoluteUri = uri.AbsoluteUri;
                flag = this.urlStorage.Add(ref absoluteUri).Count == 1;
            }
            catch (Exception)
            {
            }
            Monitor.Exit(this.urlStorage);
            return flag;
        }

        private void advancedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ShowSettings(3);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.seva_webindex.Text = "";
            if (this.webindex.ShowDialog() == DialogResult.OK)
            {
                this.seva_webindex.Text = this.webindex.SelectedPath.ToString();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.RunParser();
        }

        private void button10_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.writer.Optimize();
            this.writer.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string str = "adbcd";
            MessageBox.Show(str.Substring(2).ToString());
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            IndexSearcher searcher = new IndexSearcher(@"E:\2008-12-22");
            Query query = new QueryParser("J_md5_bai", new KTDictSegAnalyzer()).Parse("B9D7929DF4E58E3906356832703DB072");
            MessageBox.Show(searcher.Search(query).Length().ToString());
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.label18.Text = this.dateTimePicker1.Value.AddDays((double) int.Parse(this.comboBox1.Text)).ToShortDateString() + " " + this.comboBox2.Text + ":00:00";
            this.dateTimePicker2.Value = this.dateTimePicker1.Value.AddDays((double) int.Parse(this.comboBox1.Text));
            Mission_Time time = new Mission_Time();
            try
            {
                time.M_timeup(this.label18.Text, this.checkBox1.Checked.ToString());
                MessageBox.Show("定时任务保存成功");
            }
            catch (Exception)
            {
                MessageBox.Show("参数或数据库错误,请联系我们");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.label18.Text = this.dateTimePicker2.Value.AddDays((double) int.Parse(this.comboBox1.Text)).ToShortDateString() + " " + this.comboBox2.Text + ":00:00";
            this.dateTimePicker2.Value = this.dateTimePicker2.Value.AddDays((double) int.Parse(this.comboBox1.Text));
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
        }

        public void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
        {
            foreach (TreeNode node in treeNode.Nodes)
            {
                node.Checked = nodeChecked;
                if (node.Nodes.Count > 0)
                {
                    this.CheckAllChildNodes(node, nodeChecked);
                }
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            OleDbConnection connection;
            string str;
            OleDbCommand command;
            if (this.checkBox2.Checked)
            {
                Registry.LocalMachine.CreateSubKey("SOFTWARE").CreateSubKey("Microsoft").CreateSubKey("Windows").CreateSubKey("CurrentVersion").CreateSubKey("Run").SetValue("军长搜索", Application.ExecutablePath);
                connection = new OleDbConnection(CONN_ACCESS.ConnString);
                str = "update Mission_Time SET [qidong]='是'";
                command = new OleDbCommand(str, connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            else
            {
                Registry.LocalMachine.CreateSubKey("SOFTWARE").CreateSubKey("Microsoft").CreateSubKey("Windows").CreateSubKey("CurrentVersion").CreateSubKey("Run").SetValue("军长搜索", false);
                connection = new OleDbConnection(CONN_ACCESS.ConnString);
                str = "update Mission_Time SET [qidong]='否'";
                command = new OleDbCommand(str, connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public string checkStr(string html)
        {
            Regex regex = new Regex(@"<script[\s\S]+</script *>", RegexOptions.IgnoreCase);
            Regex regex2 = new Regex(@" href *= *[\s\S]*script *:", RegexOptions.IgnoreCase);
            Regex regex3 = new Regex(@" no[\s\S]*=", RegexOptions.IgnoreCase);
            Regex regex4 = new Regex(@"<iframe[\s\S]+</iframe *>", RegexOptions.IgnoreCase);
            Regex regex5 = new Regex(@"<frameset[\s\S]+</frameset *>", RegexOptions.IgnoreCase);
            Regex regex6 = new Regex(@"\<img[^\>]+\>", RegexOptions.IgnoreCase);
            Regex regex7 = new Regex("</p>", RegexOptions.IgnoreCase);
            Regex regex8 = new Regex("<p>", RegexOptions.IgnoreCase);
            Regex regex9 = new Regex("<[^>]*>", RegexOptions.IgnoreCase);
            html = regex.Replace(html, "");
            html = regex2.Replace(html, "");
            html = regex3.Replace(html, " _disibledevent=");
            html = regex4.Replace(html, "");
            html = regex5.Replace(html, "");
            html = regex6.Replace(html, "");
            html = regex7.Replace(html, "");
            html = regex8.Replace(html, "");
            html = regex9.Replace(html, "");
            html = html.Replace(" ", "");
            html = html.Replace("</strong>", "");
            html = html.Replace("<strong>", "");
            html = Regex.Replace(html, " ", "");
            html = Regex.Replace(html, "　", "");
            html = Regex.Replace(html, "&nbsp;", "");
            html = Regex.Replace(html, "\r", "");
            html = Regex.Replace(html, "\r\n", "");
            return html;
        }

        private void CloseCtiServer()
        {
            base.Close();
            base.Dispose();
            Application.Exit();
        }

        private void comboBoxWeb_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Return) && ((this.threadParse == null) || (this.threadParse.ThreadState != System.Threading.ThreadState.Running)))
            {
                this.StartParsing();
            }
        }

        private string Commas(int nNum)
        {
            string str = nNum.ToString();
            for (int i = str.Length; i > 3; i -= 3)
            {
                str = str.Insert(i - 3, ",");
            }
            return str;
        }

        public bool Compared(string file)
        {
            switch (file)
            {
                case "":
                    return false;

                case "html":
                    return false;

                case "htm":
                    return false;

                case "jsp":
                    return false;

                case "asp":
                    return false;

                case "aspx":
                    return false;

                case "php":
                    return false;

                case "shtml":
                    return false;
            }
            return true;
        }

        public bool Compared_jpg(string file)
        {
            switch (file)
            {
                case "":
                    return true;

                case "html":
                    return true;

                case "htm":
                    return true;

                case "jsp":
                    return true;

                case "asp":
                    return true;

                case "aspx":
                    return true;

                case "php":
                    return true;

                case "shtml":
                    return true;
            }
            return false;
        }

        private void ConnectionInfo()
        {
            try
            {
                int lpdwFlags = 0;
                if ((InternetGetConnectedState(ref lpdwFlags, 0) == 0) && ((this.nFirstTimeCheckConnection++ == 0) && (InternetAutodial(1, 0) != 0)))
                {
                    InternetGetConnectedState(ref lpdwFlags, 0);
                }
                if (((lpdwFlags & 2) == 2) || ((lpdwFlags & 4) == 4))
                {
                    this.nFirstTimeCheckConnection = 0;
                }
            }
            catch
            {
            }
            this.statusBarPanelInfo.Text = this.InternetGetConnectedStateString();
        }

        private void ContinueParsing()
        {
            if (this.threadParse != null)
            {
                if (this.threadParse.ThreadState == System.Threading.ThreadState.Suspended)
                {
                    this.threadParse.Resume();
                }
                this.ThreadCount = Settings.GetValue("Threads count", 10);
                this.toolBarButtonContinue.Enabled = false;
                this.toolBarButtonPause.Enabled = true;
            }
        }

        private void CrawlerForm_Closing(object sender, CancelEventArgs e)
        {
            Settings.SetValue((Form) this);
        }

        private void CrawlerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void CrawlerForm_Load(object sender, EventArgs e)
        {
            this.ThreadCount = Settings.GetValue("Threads count", 10);
            this.Skin();
            this.redspider_column();
            Settings.GetValue((Form) this);
            this.InitValues();
            this.statusBarPanelInfo.Text = this.InternetGetConnectedStateString();
            this.tabControlRightView.SelectedTab = this.tabControlRightView.TabPages[1];
            this.webBrowser1.DocumentText = "<a href='http://www.baidu.com'>进入官方</a>";
            this.timer2.Enabled = true;
            this.M_jk_time();
        }

        private void CrawlerForm_SizeChanged(object sender, EventArgs e)
        {
        }

        private void DeleteAllItems()
        {
            if (MessageBox.Show(this, "清空线程和列队?", "核实", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.listViewErrors.Items.Clear();
                this.listViewRequests.Items.Clear();
                this.urlStorage = new SortTree();
                this.URLCount = 0;
                this.FileCount = 0;
                this.ByteCount = 0;
                this.ErrorCount = 0;
            }
        }

        private MyUri DequeueUri()
        {
            Monitor.Enter(this.queueURLS);
            MyUri uri = null;
            try
            {
                uri = (MyUri) this.queueURLS.Dequeue();
            }
            catch (Exception)
            {
            }
            Monitor.Exit(this.queueURLS);
            return uri;
        }

        protected override void Dispose(bool disposing)
        {
            this.StopParsing();
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
            Environment.Exit(0);
        }

        private bool EnqueueUri(MyUri uri, bool bCheckRepetition)
        {
            if (!(!bCheckRepetition || this.AddURL(ref uri)))
            {
                return false;
            }
            Monitor.Enter(this.queueURLS);
            try
            {
                this.queueURLS.Enqueue(uri);
            }
            catch (Exception)
            {
            }
            Monitor.Exit(this.queueURLS);
            return true;
        }

        private void EraseItem(ListViewItem item)
        {
            lock (this.listViewThreads)
            {
                item.SubItems[1].Text = "";
                item.ImageIndex = 0;
                item.BackColor = System.Drawing.Color.WhiteSmoke;
                item.ForeColor = System.Drawing.Color.Black;
                item.SubItems[2].Text = "";
                item.SubItems[3].Text = "";
                item.SubItems[4].Text = "";
                item.SubItems[5].Text = "";
                this.j = 0;
            }
        }

        public static void FlushMemory()
        {
            GarbageCollect();
        }

        public void fx(string url)
        {
            string html = spider_Getinside.GetHtml(url, Global.BM.ToString());
            OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
            OleDbCommand command = new OleDbCommand("SELECT * FROM RedSpider_Label where url_id='" + Global.COLUMN_ID.ToString() + "'", connection);
            connection.Open();
            OleDbDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
            string str3 = "";
            string htmlcode = "";
            this.index_content = new string[15];
            this.index_title = new string[15];
            for (int i = 0; reader.Read(); i++)
            {
                if (reader["zzyes"].ToString() == "是")
                {
                    htmlcode = "";
                    string[] strArray = reader["B_egin"].ToString().Split(new char[] { ' ' });
                    for (int j = 0; j < strArray.Length; j++)
                    {
                        string zenze = spider_Getinside.Getzenze(html, strArray[j]);
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
                    connection2.Close();
                }
                if (reader["R_Parts"].ToString() == "是")
                {
                    OleDbConnection connection3 = new OleDbConnection(CONN_ACCESS.ConnString);
                    OleDbCommand command3 = new OleDbCommand("SELECT * FROM RedSpider_Exclude_replacement where url_id='" + Global.WEB_COLUMN_ID + "' and lable_name='" + reader["Label_name"].ToString() + "' and pc_or_tt='替换'", connection3);
                    connection3.Open();
                    OleDbDataReader reader3 = command3.ExecuteReader(CommandBehavior.CloseConnection);
                    while (reader3.Read())
                    {
                        htmlcode = spider_Getinside.Gettihuan(htmlcode, reader3["pc_tt"].ToString(), reader3["tt_h"].ToString());
                    }
                    connection3.Close();
                }
                str3 = str3 + htmlcode;
                this.index_content[i] = this.checkStr(htmlcode);
                this.index_title[i] = reader["Label_name"].ToString();
            }
            connection.Close();
        }

        public static void GarbageCollect()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private void GetAllChecked(TreeNodeCollection tnc)
        {
            foreach (TreeNode node in tnc)
            {
                if (node.Checked)
                {
                }
                this.GetAllChecked(node.Nodes);
            }
        }

        private static string GetCharType(string tmp1)
        {
            string str = "";
            str = tmp1;
            int startIndex = str.IndexOf("charset=") + 8;
            return str.Substring(startIndex, str.Length - startIndex);
        }

        public static string GetKeyWordsSplitBySpace(string keywords, KTDictSegTokenizer ktTokenizer)
        {
            StringBuilder builder = new StringBuilder();
            List<T_WordInfo> list = ktTokenizer.SegmentToWordInfos(keywords);
            foreach (T_WordInfo info in list)
            {
                if (info != null)
                {
                    builder.AppendFormat("{0}^{1}.0 ", info.Word, (int) Math.Pow(3.0, (double) info.Rank));
                }
            }
            return builder.ToString().Trim();
        }

        private static string GetMIMETypes()
        {
            string str = "";
            if (System.IO.File.Exists(Application.StartupPath + @"\Settings.xml"))
            {
                XmlDocument document = new XmlDocument();
                document.Load(Application.StartupPath + @"\Settings.xml");
                XmlNode node = document.DocumentElement.SelectSingleNode("SettingsForm-listViewFileMatches");
                if (node == null)
                {
                    return str;
                }
                for (int i = 0; i < node.ChildNodes.Count; i++)
                {
                    XmlNode node2 = node.ChildNodes[i];
                    XmlAttribute attribute = node2.Attributes["Checked"];
                    if ((attribute != null) && !(attribute.Value.ToLower() != "true"))
                    {
                        string[] strArray = node2.InnerText.Split(new char[] { '\t' });
                        if (strArray.Length > 1)
                        {
                            str = str + strArray[0];
                            if (strArray.Length > 2)
                            {
                                object obj2 = str;
                                str = string.Concat(new object[] { obj2, '[', strArray[1], ',', strArray[2], ']' });
                            }
                            str = str + ';';
                        }
                    }
                }
            }
            return str;
        }

        private static string GetPage(string url)
        {
            string str = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.0)";
            WebResponse response = null;
            Stream responseStream = null;
            StreamReader reader = null;
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            request.UserAgent = str;
            response = request.GetResponse();
            responseStream = response.GetResponseStream();
            if (response.ContentType.IndexOf("charset") > 0)
            {
                reader = new StreamReader(responseStream, Encoding.GetEncoding(GetCharType(response.ContentType.ToString())));
            }
            else
            {
                reader = new StreamReader(responseStream, Encoding.Default);
            }
            return reader.ReadToEnd();
        }

        private static int GetPhisicalMemory()
        {
            ManagementObjectCollection.ManagementObjectEnumerator enumerator = new ManagementObjectSearcher { Query = new SelectQuery("Win32_PhysicalMemory", "", new string[] { "Capacity" }) }.Get().GetEnumerator();
            int num = 0;
            while (enumerator.MoveNext())
            {
                ManagementBaseObject current = enumerator.Current;
                if (current.Properties["Capacity"].Value != null)
                {
                    try
                    {
                        num += int.Parse(current.Properties["Capacity"].Value.ToString());
                    }
                    catch
                    {
                        return 0;
                    }
                }
            }
            return ((num / 0x400) / 0x400);
        }

        private static Encoding GetTextEncoding()
        {
            if (Settings.GetValue("Use windows default code page", true))
            {
                return Encoding.Default;
            }
            string input = Settings.GetValue("Settings code page");
            Regex regex = new Regex(@"\([0-9]*\)");
            return Encoding.GetEncoding(int.Parse(regex.Match(input).Value.Trim(new char[] { '(', ')' })));
        }

        public void I_nventory(string url)
        {
            string html = spider_Getinside.GetHtml(url, Global.BM.ToString());
            OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
            OleDbCommand command = new OleDbCommand("SELECT * FROM RedSpider_Label where url_id='" + Global.URL_BIANHAO.ToString() + "'", connection);
            connection.Open();
            OleDbDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
            string htmlcode = "";
            string bt = "";
            string dd = "";
            string nr = "";
            string zz = "";
            string ly = "";
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
                htmlcode = htmlcode.Replace("'", "''");
                if (reader["Label_name"].ToString() == "标題")
                {
                    bt = htmlcode.ToString();
                }
                if (reader["Label_name"].ToString() == "导读")
                {
                    dd = htmlcode.ToString();
                }
                if (reader["Label_name"].ToString() == "内容")
                {
                    nr = htmlcode.ToString();
                }
                if (reader["Label_name"].ToString() == "作者")
                {
                    zz = htmlcode.ToString();
                }
                if (reader["Label_name"].ToString() == "来源")
                {
                    ly = htmlcode.ToString();
                }
            }
            this.A_nalysis(bt, dd, nr, zz, ly, url);
        }

        private void Index_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            base.Visible = false;
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CrawlerForm));
            this.contextMenuSettings = new System.Windows.Forms.ContextMenu();
            this.menuItemSettingsFileMatches = new System.Windows.Forms.MenuItem();
            this.menuItemSettingsOutput = new System.Windows.Forms.MenuItem();
            this.menuItemSettingsConnections = new System.Windows.Forms.MenuItem();
            this.menuItemSettingsAdvanced = new System.Windows.Forms.MenuItem();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.statusBar = new System.Windows.Forms.StatusBar();
            this.statusBarPanelInfo = new System.Windows.Forms.StatusBarPanel();
            this.statusBarPanelURLs = new System.Windows.Forms.StatusBarPanel();
            this.statusBarPanelFiles = new System.Windows.Forms.StatusBarPanel();
            this.statusBarPanelByteCount = new System.Windows.Forms.StatusBarPanel();
            this.statusBarPanelErrors = new System.Windows.Forms.StatusBarPanel();
            this.statusBarPanelCPU = new System.Windows.Forms.StatusBarPanel();
            this.statusBarPanelMem = new System.Windows.Forms.StatusBarPanel();
            this.contextMenuBrowse = new System.Windows.Forms.ContextMenu();
            this.menuItemBrowseHttp = new System.Windows.Forms.MenuItem();
            this.menuItemHttp = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.imageList4 = new System.Windows.Forms.ImageList(this.components);
            this.tabControlRightView = new System.Windows.Forms.TabControl();
            this.tabPageThreads = new System.Windows.Forms.TabPage();
            this.listViewThreads = new System.Windows.Forms.ListView();
            this.columnHeaderTHreadID = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderThreadDepth = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderThreadAction = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderThreadURL = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderThreadBytes = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderThreadPersentage = new System.Windows.Forms.ColumnHeader();
            this.imageList3 = new System.Windows.Forms.ImageList(this.components);
            this.tabPageRequests = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.listViewRequests = new System.Windows.Forms.ListView();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.tabPageErrors = new System.Windows.Forms.TabPage();
            this.textBoxErrorDescription = new System.Windows.Forms.TextBox();
            this.splitter3 = new System.Windows.Forms.Splitter();
            this.listViewErrors = new System.Windows.Forms.ListView();
            this.columnHeaderErrorID = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderDate = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderErrorItem = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderErrorDescription = new System.Windows.Forms.ColumnHeader();
            this.contextMenuNavigate = new System.Windows.Forms.ContextMenu();
            this.menuItemCut = new System.Windows.Forms.MenuItem();
            this.menuItemCopy = new System.Windows.Forms.MenuItem();
            this.menuItemPaste = new System.Windows.Forms.MenuItem();
            this.menuItemDelete = new System.Windows.Forms.MenuItem();
            this.toolBarButton4 = new System.Windows.Forms.ToolBarButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.timerMem = new System.Windows.Forms.Timer(this.components);
            this.imageListPercentage = new System.Windows.Forms.ImageList(this.components);
            this.timerConnectionInfo = new System.Windows.Forms.Timer(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.webtongbu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.添加项目则规ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.开始抓取所有分类ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList5 = new System.Windows.Forms.ImageList(this.components);
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker3 = new System.Windows.Forms.DateTimePicker();
            this.button10 = new System.Windows.Forms.Button();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.label19 = new System.Windows.Forms.Label();
            this.button9 = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button8 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.textBox13 = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.button7 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.seva_webindex = new System.Windows.Forms.TextBox();
            this.comboBoxWeb = new System.Windows.Forms.ComboBox();
            this.zidinyicalss = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加子节点ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加根节点ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.修改项目规则ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除项目规则ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.系统ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.最小化程序ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出系统ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.系统设定OptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置SettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.mIMETypesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.输出OutputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.连接ConnectionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.advancedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.风格皮肤ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripMenuItem();
            this.风格五ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.风格六ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.风格七ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.原始经典ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.skinEngine1 = new Sunisoft.IrisSkin.SkinEngine(((System.ComponentModel.Component)(this)));
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.tabPage10 = new System.Windows.Forms.TabPage();
            this.tabPage11 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.linkLabel8 = new System.Windows.Forms.LinkLabel();
            this.linkLabel9 = new System.Windows.Forms.LinkLabel();
            this.linkLabel10 = new System.Windows.Forms.LinkLabel();
            this.linkLabel11 = new System.Windows.Forms.LinkLabel();
            this.linkLabel12 = new System.Windows.Forms.LinkLabel();
            this.linkLabel13 = new System.Windows.Forms.LinkLabel();
            this.linkLabel14 = new System.Windows.Forms.LinkLabel();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.tabPage12 = new System.Windows.Forms.TabPage();
            this.tabPage13 = new System.Windows.Forms.TabPage();
            this.tabPage14 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.linkLabel15 = new System.Windows.Forms.LinkLabel();
            this.linkLabel16 = new System.Windows.Forms.LinkLabel();
            this.linkLabel17 = new System.Windows.Forms.LinkLabel();
            this.linkLabel18 = new System.Windows.Forms.LinkLabel();
            this.linkLabel19 = new System.Windows.Forms.LinkLabel();
            this.linkLabel20 = new System.Windows.Forms.LinkLabel();
            this.linkLabel21 = new System.Windows.Forms.LinkLabel();
            this.label14 = new System.Windows.Forms.Label();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.button6 = new System.Windows.Forms.Button();
            this.toolBarButton3 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonSettings = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton2 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonDeleteAll = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonStop = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonPause = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonContinue = new System.Windows.Forms.ToolBarButton();
            this.toolBarMain = new System.Windows.Forms.ToolBar();
            this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton5 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton6 = new System.Windows.Forms.ToolBarButton();
            this.webindex = new System.Windows.Forms.FolderBrowserDialog();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.最小化程序ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.最大化ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出系统ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolBarButton7 = new System.Windows.Forms.ToolBarButton();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanelInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanelURLs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanelFiles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanelByteCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanelErrors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanelCPU)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanelMem)).BeginInit();
            this.tabControlRightView.SuspendLayout();
            this.tabPageThreads.SuspendLayout();
            this.tabPageRequests.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabPageErrors.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.webtongbu.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.zidinyicalss.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tabPage11.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage14.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuSettings
            // 
            this.contextMenuSettings.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemSettingsFileMatches,
            this.menuItemSettingsOutput,
            this.menuItemSettingsConnections,
            this.menuItemSettingsAdvanced});
            // 
            // menuItemSettingsFileMatches
            // 
            this.menuItemSettingsFileMatches.Index = 0;
            this.menuItemSettingsFileMatches.Text = "&MIME类型...";
            this.menuItemSettingsFileMatches.Click += new System.EventHandler(this.menuItemSettingsFileMatches_Click);
            // 
            // menuItemSettingsOutput
            // 
            this.menuItemSettingsOutput.Index = 1;
            this.menuItemSettingsOutput.Text = "&输出...";
            this.menuItemSettingsOutput.Click += new System.EventHandler(this.menuItemSettingsOutput_Click);
            // 
            // menuItemSettingsConnections
            // 
            this.menuItemSettingsConnections.Index = 2;
            this.menuItemSettingsConnections.Text = "&连接...";
            this.menuItemSettingsConnections.Click += new System.EventHandler(this.menuItemSettingsConnections_Click);
            // 
            // menuItemSettingsAdvanced
            // 
            this.menuItemSettingsAdvanced.Index = 3;
            this.menuItemSettingsAdvanced.Text = "&排除...";
            this.menuItemSettingsAdvanced.Click += new System.EventHandler(this.menuItemSettingsAdvanced_Click);
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "");
            this.imageList2.Images.SetKeyName(1, "");
            this.imageList2.Images.SetKeyName(2, "");
            this.imageList2.Images.SetKeyName(3, "");
            this.imageList2.Images.SetKeyName(4, "");
            this.imageList2.Images.SetKeyName(5, "Applications AFP Client - 复制.ico");
            this.imageList2.Images.SetKeyName(6, "cuteftp01.ico");
            this.imageList2.Images.SetKeyName(7, "GOVERM~1.ICO");
            this.imageList2.Images.SetKeyName(8, "Misci.ico");
            this.imageList2.Images.SetKeyName(9, "My Favourites [F].ico");
            // 
            // statusBar
            // 
            this.statusBar.Location = new System.Drawing.Point(0, 540);
            this.statusBar.Name = "statusBar";
            this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.statusBarPanelInfo,
            this.statusBarPanelURLs,
            this.statusBarPanelFiles,
            this.statusBarPanelByteCount,
            this.statusBarPanelErrors,
            this.statusBarPanelCPU,
            this.statusBarPanelMem});
            this.statusBar.ShowPanels = true;
            this.statusBar.Size = new System.Drawing.Size(926, 24);
            this.statusBar.TabIndex = 1;
            this.statusBar.Text = "Ready";
            // 
            // statusBarPanelInfo
            // 
            this.statusBarPanelInfo.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.statusBarPanelInfo.Name = "statusBarPanelInfo";
            this.statusBarPanelInfo.ToolTipText = "View total parsed uris";
            this.statusBarPanelInfo.Width = 639;
            // 
            // statusBarPanelURLs
            // 
            this.statusBarPanelURLs.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
            this.statusBarPanelURLs.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.statusBarPanelURLs.Name = "statusBarPanelURLs";
            this.statusBarPanelURLs.ToolTipText = "View unique hits count";
            this.statusBarPanelURLs.Width = 10;
            // 
            // statusBarPanelFiles
            // 
            this.statusBarPanelFiles.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
            this.statusBarPanelFiles.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.statusBarPanelFiles.Name = "statusBarPanelFiles";
            this.statusBarPanelFiles.ToolTipText = "View total hits count";
            this.statusBarPanelFiles.Width = 10;
            // 
            // statusBarPanelByteCount
            // 
            this.statusBarPanelByteCount.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
            this.statusBarPanelByteCount.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.statusBarPanelByteCount.Name = "statusBarPanelByteCount";
            this.statusBarPanelByteCount.ToolTipText = "View total bytes of parsed items";
            this.statusBarPanelByteCount.Width = 10;
            // 
            // statusBarPanelErrors
            // 
            this.statusBarPanelErrors.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
            this.statusBarPanelErrors.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.statusBarPanelErrors.Icon = ((System.Drawing.Icon)(resources.GetObject("statusBarPanelErrors.Icon")));
            this.statusBarPanelErrors.Name = "statusBarPanelErrors";
            this.statusBarPanelErrors.ToolTipText = "View errors count";
            this.statusBarPanelErrors.Width = 31;
            // 
            // statusBarPanelCPU
            // 
            this.statusBarPanelCPU.Icon = ((System.Drawing.Icon)(resources.GetObject("statusBarPanelCPU.Icon")));
            this.statusBarPanelCPU.Name = "statusBarPanelCPU";
            this.statusBarPanelCPU.ToolTipText = "CPU usage";
            this.statusBarPanelCPU.Width = 110;
            // 
            // statusBarPanelMem
            // 
            this.statusBarPanelMem.Name = "statusBarPanelMem";
            this.statusBarPanelMem.ToolTipText = "Available memory";
            // 
            // contextMenuBrowse
            // 
            this.contextMenuBrowse.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemBrowseHttp});
            // 
            // menuItemBrowseHttp
            // 
            this.menuItemBrowseHttp.Index = 0;
            this.menuItemBrowseHttp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemHttp,
            this.menuItem5});
            this.menuItemBrowseHttp.Text = "&Http(s)";
            // 
            // menuItemHttp
            // 
            this.menuItemHttp.Index = 0;
            this.menuItemHttp.Text = "&http://";
            this.menuItemHttp.Click += new System.EventHandler(this.menuItemHttp_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 1;
            this.menuItem5.Text = "-";
            // 
            // imageList4
            // 
            this.imageList4.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList4.ImageStream")));
            this.imageList4.TransparentColor = System.Drawing.Color.Teal;
            this.imageList4.Images.SetKeyName(0, "");
            // 
            // tabControlRightView
            // 
            this.tabControlRightView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlRightView.Controls.Add(this.tabPageThreads);
            this.tabControlRightView.Controls.Add(this.tabPageRequests);
            this.tabControlRightView.Controls.Add(this.tabPageErrors);
            this.tabControlRightView.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControlRightView.ImageList = this.imageList3;
            this.tabControlRightView.Location = new System.Drawing.Point(4, 33);
            this.tabControlRightView.Name = "tabControlRightView";
            this.tabControlRightView.SelectedIndex = 0;
            this.tabControlRightView.ShowToolTips = true;
            this.tabControlRightView.Size = new System.Drawing.Size(715, 407);
            this.tabControlRightView.TabIndex = 7;
            this.tabControlRightView.Tag = "Main Tab";
            this.tabControlRightView.SelectedIndexChanged += new System.EventHandler(this.tabControlRightView_SelectedIndexChanged);
            // 
            // tabPageThreads
            // 
            this.tabPageThreads.Controls.Add(this.listViewThreads);
            this.tabPageThreads.ImageIndex = 6;
            this.tabPageThreads.Location = new System.Drawing.Point(4, 25);
            this.tabPageThreads.Name = "tabPageThreads";
            this.tabPageThreads.Size = new System.Drawing.Size(707, 378);
            this.tabPageThreads.TabIndex = 3;
            this.tabPageThreads.Text = "线程";
            this.tabPageThreads.ToolTipText = "View working threads status";
            this.tabPageThreads.UseVisualStyleBackColor = true;
            // 
            // listViewThreads
            // 
            this.listViewThreads.BackColor = System.Drawing.Color.WhiteSmoke;
            this.listViewThreads.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderTHreadID,
            this.columnHeaderThreadDepth,
            this.columnHeaderThreadAction,
            this.columnHeaderThreadURL,
            this.columnHeaderThreadBytes,
            this.columnHeaderThreadPersentage});
            this.listViewThreads.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewThreads.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listViewThreads.FullRowSelect = true;
            this.listViewThreads.GridLines = true;
            this.listViewThreads.HideSelection = false;
            this.listViewThreads.Location = new System.Drawing.Point(0, 0);
            this.listViewThreads.MultiSelect = false;
            this.listViewThreads.Name = "listViewThreads";
            this.listViewThreads.Size = new System.Drawing.Size(707, 378);
            this.listViewThreads.SmallImageList = this.imageList3;
            this.listViewThreads.TabIndex = 0;
            this.listViewThreads.UseCompatibleStateImageBehavior = false;
            this.listViewThreads.View = System.Windows.Forms.View.Details;
            this.listViewThreads.SelectedIndexChanged += new System.EventHandler(this.listViewThreads_SelectedIndexChanged);
            // 
            // columnHeaderTHreadID
            // 
            this.columnHeaderTHreadID.Text = "ID";
            this.columnHeaderTHreadID.Width = 40;
            // 
            // columnHeaderThreadDepth
            // 
            this.columnHeaderThreadDepth.Text = "深度";
            this.columnHeaderThreadDepth.Width = 57;
            // 
            // columnHeaderThreadAction
            // 
            this.columnHeaderThreadAction.Text = "状态";
            this.columnHeaderThreadAction.Width = 108;
            // 
            // columnHeaderThreadURL
            // 
            this.columnHeaderThreadURL.Text = "UrL";
            this.columnHeaderThreadURL.Width = 387;
            // 
            // columnHeaderThreadBytes
            // 
            this.columnHeaderThreadBytes.Text = "字节";
            this.columnHeaderThreadBytes.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeaderThreadBytes.Width = 70;
            // 
            // columnHeaderThreadPersentage
            // 
            this.columnHeaderThreadPersentage.Text = "%";
            this.columnHeaderThreadPersentage.Width = 40;
            // 
            // imageList3
            // 
            this.imageList3.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList3.ImageStream")));
            this.imageList3.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList3.Images.SetKeyName(0, "");
            this.imageList3.Images.SetKeyName(1, "");
            this.imageList3.Images.SetKeyName(2, "");
            this.imageList3.Images.SetKeyName(3, "");
            this.imageList3.Images.SetKeyName(4, "");
            this.imageList3.Images.SetKeyName(5, "");
            this.imageList3.Images.SetKeyName(6, "");
            this.imageList3.Images.SetKeyName(7, "");
            this.imageList3.Images.SetKeyName(8, "");
            // 
            // tabPageRequests
            // 
            this.tabPageRequests.Controls.Add(this.splitContainer2);
            this.tabPageRequests.ImageIndex = 8;
            this.tabPageRequests.Location = new System.Drawing.Point(4, 25);
            this.tabPageRequests.Name = "tabPageRequests";
            this.tabPageRequests.Size = new System.Drawing.Size(707, 378);
            this.tabPageRequests.TabIndex = 5;
            this.tabPageRequests.Text = "处理列队";
            this.tabPageRequests.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.Location = new System.Drawing.Point(3, 8);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.listView1);
            this.splitContainer2.Panel1.Controls.Add(this.listViewRequests);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.webBrowser1);
            this.splitContainer2.Size = new System.Drawing.Size(701, 367);
            this.splitContainer2.SplitterDistance = 181;
            this.splitContainer2.TabIndex = 6;
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(8, 3);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(699, 111);
            this.listView1.TabIndex = 5;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "时间";
            this.columnHeader5.Width = 122;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "标题";
            this.columnHeader6.Width = 351;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "处理状态";
            this.columnHeader7.Width = 179;
            // 
            // listViewRequests
            // 
            this.listViewRequests.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewRequests.BackColor = System.Drawing.Color.WhiteSmoke;
            this.listViewRequests.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader1,
            this.columnHeader4});
            this.listViewRequests.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listViewRequests.FullRowSelect = true;
            this.listViewRequests.GridLines = true;
            this.listViewRequests.HideSelection = false;
            this.listViewRequests.Location = new System.Drawing.Point(8, 120);
            this.listViewRequests.MultiSelect = false;
            this.listViewRequests.Name = "listViewRequests";
            this.listViewRequests.Size = new System.Drawing.Size(687, 74);
            this.listViewRequests.TabIndex = 4;
            this.listViewRequests.UseCompatibleStateImageBehavior = false;
            this.listViewRequests.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "时间";
            this.columnHeader2.Width = 126;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "标题";
            this.columnHeader3.Width = 351;
            // 
            // columnHeader1
            // 
            this.columnHeader1.DisplayIndex = 3;
            this.columnHeader1.Text = "入库状态";
            this.columnHeader1.Width = 188;
            // 
            // columnHeader4
            // 
            this.columnHeader4.DisplayIndex = 2;
            this.columnHeader4.Text = "Description";
            this.columnHeader4.Width = 0;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser1.Location = new System.Drawing.Point(-5, 3);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(707, 176);
            this.webBrowser1.TabIndex = 6;
            // 
            // tabPageErrors
            // 
            this.tabPageErrors.Controls.Add(this.textBoxErrorDescription);
            this.tabPageErrors.Controls.Add(this.splitter3);
            this.tabPageErrors.Controls.Add(this.listViewErrors);
            this.tabPageErrors.ImageIndex = 7;
            this.tabPageErrors.Location = new System.Drawing.Point(4, 25);
            this.tabPageErrors.Name = "tabPageErrors";
            this.tabPageErrors.Size = new System.Drawing.Size(707, 378);
            this.tabPageErrors.TabIndex = 4;
            this.tabPageErrors.Text = "错误";
            this.tabPageErrors.ToolTipText = "View reported errors";
            this.tabPageErrors.UseVisualStyleBackColor = true;
            // 
            // textBoxErrorDescription
            // 
            this.textBoxErrorDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxErrorDescription.BackColor = System.Drawing.Color.WhiteSmoke;
            this.textBoxErrorDescription.Location = new System.Drawing.Point(0, 292);
            this.textBoxErrorDescription.Multiline = true;
            this.textBoxErrorDescription.Name = "textBoxErrorDescription";
            this.textBoxErrorDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxErrorDescription.Size = new System.Drawing.Size(707, 88);
            this.textBoxErrorDescription.TabIndex = 2;
            this.textBoxErrorDescription.WordWrap = false;
            // 
            // splitter3
            // 
            this.splitter3.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter3.Location = new System.Drawing.Point(0, 286);
            this.splitter3.Name = "splitter3";
            this.splitter3.Size = new System.Drawing.Size(707, 3);
            this.splitter3.TabIndex = 1;
            this.splitter3.TabStop = false;
            // 
            // listViewErrors
            // 
            this.listViewErrors.BackColor = System.Drawing.Color.WhiteSmoke;
            this.listViewErrors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderErrorID,
            this.columnHeaderDate,
            this.columnHeaderErrorItem,
            this.columnHeaderErrorDescription});
            this.listViewErrors.Dock = System.Windows.Forms.DockStyle.Top;
            this.listViewErrors.FullRowSelect = true;
            this.listViewErrors.GridLines = true;
            this.listViewErrors.HideSelection = false;
            this.listViewErrors.Location = new System.Drawing.Point(0, 0);
            this.listViewErrors.MultiSelect = false;
            this.listViewErrors.Name = "listViewErrors";
            this.listViewErrors.Size = new System.Drawing.Size(707, 286);
            this.listViewErrors.TabIndex = 0;
            this.listViewErrors.UseCompatibleStateImageBehavior = false;
            this.listViewErrors.View = System.Windows.Forms.View.Details;
            this.listViewErrors.SelectedIndexChanged += new System.EventHandler(this.listViewErrors_SelectedIndexChanged);
            // 
            // columnHeaderErrorID
            // 
            this.columnHeaderErrorID.Text = "ID";
            // 
            // columnHeaderDate
            // 
            this.columnHeaderDate.Text = "时间";
            this.columnHeaderDate.Width = 160;
            // 
            // columnHeaderErrorItem
            // 
            this.columnHeaderErrorItem.Text = "错误";
            this.columnHeaderErrorItem.Width = 343;
            // 
            // columnHeaderErrorDescription
            // 
            this.columnHeaderErrorDescription.Text = "Description";
            this.columnHeaderErrorDescription.Width = 0;
            // 
            // contextMenuNavigate
            // 
            this.contextMenuNavigate.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemCut,
            this.menuItemCopy,
            this.menuItemPaste,
            this.menuItemDelete});
            // 
            // menuItemCut
            // 
            this.menuItemCut.Index = 0;
            this.menuItemCut.Text = "Cu&t";
            this.menuItemCut.Click += new System.EventHandler(this.menuItemCut_Click);
            // 
            // menuItemCopy
            // 
            this.menuItemCopy.Index = 1;
            this.menuItemCopy.Text = "&Copy";
            this.menuItemCopy.Click += new System.EventHandler(this.menuItemCopy_Click);
            // 
            // menuItemPaste
            // 
            this.menuItemPaste.Index = 2;
            this.menuItemPaste.Text = "&Paste";
            this.menuItemPaste.Click += new System.EventHandler(this.menuItemPaste_Click);
            // 
            // menuItemDelete
            // 
            this.menuItemDelete.Index = 3;
            this.menuItemDelete.Text = "&Delete";
            this.menuItemDelete.Click += new System.EventHandler(this.menuItemDelete_Click);
            // 
            // toolBarButton4
            // 
            this.toolBarButton4.Name = "toolBarButton4";
            this.toolBarButton4.Text = "Go";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Teal;
            this.imageList1.Images.SetKeyName(0, "");
            // 
            // imageListPercentage
            // 
            this.imageListPercentage.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListPercentage.ImageStream")));
            this.imageListPercentage.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListPercentage.Images.SetKeyName(0, "");
            this.imageListPercentage.Images.SetKeyName(1, "");
            this.imageListPercentage.Images.SetKeyName(2, "");
            this.imageListPercentage.Images.SetKeyName(3, "");
            this.imageListPercentage.Images.SetKeyName(4, "");
            this.imageListPercentage.Images.SetKeyName(5, "");
            this.imageListPercentage.Images.SetKeyName(6, "");
            this.imageListPercentage.Images.SetKeyName(7, "");
            this.imageListPercentage.Images.SetKeyName(8, "");
            this.imageListPercentage.Images.SetKeyName(9, "");
            this.imageListPercentage.Images.SetKeyName(10, "");
            // 
            // timerConnectionInfo
            // 
            this.timerConnectionInfo.Enabled = true;
            this.timerConnectionInfo.Interval = 15000;
            this.timerConnectionInfo.Tick += new System.EventHandler(this.timerConnectionInfo_Tick);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.splitContainer1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("splitContainer1.BackgroundImage")));
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Location = new System.Drawing.Point(0, 60);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            this.splitContainer1.Panel1MinSize = 150;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl2);
            this.splitContainer1.Size = new System.Drawing.Size(926, 474);
            this.splitContainer1.SplitterDistance = 193;
            this.splitContainer1.TabIndex = 12;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(183, 464);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.treeView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(175, 439);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "数据源";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.ContextMenuStrip = this.webtongbu;
            this.treeView1.ImageIndex = 1;
            this.treeView1.ImageList = this.imageList5;
            this.treeView1.Location = new System.Drawing.Point(-4, 3);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(181, 433);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // webtongbu
            // 
            this.webtongbu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem5,
            this.添加项目则规ToolStripMenuItem,
            this.toolStripMenuItem4,
            this.开始抓取所有分类ToolStripMenuItem});
            this.webtongbu.Name = "webtongbu";
            this.webtongbu.Size = new System.Drawing.Size(185, 76);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem5.Image")));
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(184, 22);
            this.toolStripMenuItem5.Text = "开始抓取任务";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.toolStripMenuItem5_Click);
            // 
            // 添加项目则规ToolStripMenuItem
            // 
            this.添加项目则规ToolStripMenuItem.Name = "添加项目则规ToolStripMenuItem";
            this.添加项目则规ToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.添加项目则规ToolStripMenuItem.Text = "[添加/管理]网址则规";
            this.添加项目则规ToolStripMenuItem.Click += new System.EventHandler(this.添加项目则规ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(181, 6);
            // 
            // 开始抓取所有分类ToolStripMenuItem
            // 
            this.开始抓取所有分类ToolStripMenuItem.Name = "开始抓取所有分类ToolStripMenuItem";
            this.开始抓取所有分类ToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.开始抓取所有分类ToolStripMenuItem.Text = "开始抓取所有分类";
            this.开始抓取所有分类ToolStripMenuItem.Click += new System.EventHandler(this.开始抓取所有分类ToolStripMenuItem_Click);
            // 
            // imageList5
            // 
            this.imageList5.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList5.ImageStream")));
            this.imageList5.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList5.Images.SetKeyName(0, "READ ME.ico");
            this.imageList5.Images.SetKeyName(1, "BMWDicons.ico");
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.checkBox2);
            this.tabPage2.Controls.Add(this.dateTimePicker1);
            this.tabPage2.Controls.Add(this.dateTimePicker3);
            this.tabPage2.Controls.Add(this.button10);
            this.tabPage2.Controls.Add(this.dateTimePicker2);
            this.tabPage2.Controls.Add(this.label19);
            this.tabPage2.Controls.Add(this.button9);
            this.tabPage2.Controls.Add(this.label18);
            this.tabPage2.Controls.Add(this.label17);
            this.tabPage2.Controls.Add(this.comboBox2);
            this.tabPage2.Controls.Add(this.label16);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.button8);
            this.tabPage2.Controls.Add(this.checkBox1);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.comboBox1);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Location = new System.Drawing.Point(4, 21);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(175, 439);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "定时任务";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(12, 263);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(132, 16);
            this.checkBox2.TabIndex = 20;
            this.checkBox2.Text = "系统启动时加载程序";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(13, 344);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(156, 21);
            this.dateTimePicker1.TabIndex = 19;
            this.dateTimePicker1.Visible = false;
            // 
            // dateTimePicker3
            // 
            this.dateTimePicker3.Location = new System.Drawing.Point(13, 399);
            this.dateTimePicker3.Name = "dateTimePicker3";
            this.dateTimePicker3.Size = new System.Drawing.Size(156, 21);
            this.dateTimePicker3.TabIndex = 18;
            this.dateTimePicker3.Visible = false;
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(11, 306);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(144, 23);
            this.button10.TabIndex = 17;
            this.button10.Text = "还原到上一次正常配置";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Visible = false;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Location = new System.Drawing.Point(11, 371);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(158, 21);
            this.dateTimePicker2.TabIndex = 16;
            this.dateTimePicker2.Visible = false;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label19.ForeColor = System.Drawing.Color.Blue;
            this.label19.Location = new System.Drawing.Point(5, 33);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(71, 16);
            this.label19.TabIndex = 15;
            this.label19.Text = "label19";
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(9, 234);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(144, 23);
            this.button9.TabIndex = 14;
            this.button9.Text = "跳过下次执行时间";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label18.ForeColor = System.Drawing.Color.Blue;
            this.label18.Location = new System.Drawing.Point(5, 203);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(59, 16);
            this.label18.TabIndex = 13;
            this.label18.Text = "待配置";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(7, 176);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(83, 12);
            this.label17.TabIndex = 12;
            this.label17.Text = "下次任务时间:";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24"});
            this.comboBox2.Location = new System.Drawing.Point(27, 102);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(46, 20);
            this.comboBox2.TabIndex = 11;
            this.comboBox2.Text = "23";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(80, 72);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(17, 12);
            this.label16.TabIndex = 10;
            this.label16.Text = "天";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "第";
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(82, 136);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(66, 23);
            this.button8.TabIndex = 8;
            this.button8.Text = "保存配置";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(9, 140);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(72, 16);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.Text = "启用监控";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(80, 109);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "时执行任务";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "7",
            "15",
            "30"});
            this.comboBox1.Location = new System.Drawing.Point(27, 68);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(46, 20);
            this.comboBox1.TabIndex = 3;
            this.comboBox1.Text = "7";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "每";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "当前时间:";
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.textBox13);
            this.tabPage5.Controls.Add(this.label21);
            this.tabPage5.Controls.Add(this.textBox4);
            this.tabPage5.Controls.Add(this.textBox3);
            this.tabPage5.Controls.Add(this.textBox2);
            this.tabPage5.Controls.Add(this.textBox1);
            this.tabPage5.Controls.Add(this.label26);
            this.tabPage5.Controls.Add(this.label24);
            this.tabPage5.Controls.Add(this.label22);
            this.tabPage5.Controls.Add(this.label20);
            this.tabPage5.Location = new System.Drawing.Point(4, 21);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(175, 439);
            this.tabPage5.TabIndex = 2;
            this.tabPage5.Text = "报告";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // textBox13
            // 
            this.textBox13.Location = new System.Drawing.Point(73, 159);
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new System.Drawing.Size(99, 21);
            this.textBox13.TabIndex = 22;
            this.textBox13.Text = "1";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(8, 137);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(125, 12);
            this.label21.TabIndex = 21;
            this.label21.Text = "我想从第?个开始引索:";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(73, 94);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(99, 21);
            this.textBox4.TabIndex = 20;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(73, 63);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(99, 21);
            this.textBox3.TabIndex = 19;
            // 
            // textBox2
            // 
            this.textBox2.Enabled = false;
            this.textBox2.Location = new System.Drawing.Point(73, 32);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(99, 21);
            this.textBox2.TabIndex = 18;
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(73, 5);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(99, 21);
            this.textBox1.TabIndex = 17;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(32, 97);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(35, 12);
            this.label26.TabIndex = 16;
            this.label26.Text = "网址:";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(8, 66);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(59, 12);
            this.label24.TabIndex = 15;
            this.label24.Text = "网址名称:";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(8, 38);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(59, 12);
            this.label22.TabIndex = 14;
            this.label22.Text = "正在引索:";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(8, 9);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(59, 12);
            this.label20.TabIndex = 13;
            this.label20.Text = "共要引索:";
            // 
            // tabControl2
            // 
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Location = new System.Drawing.Point(0, 3);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(727, 464);
            this.tabControl2.TabIndex = 11;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.button7);
            this.tabPage3.Controls.Add(this.button2);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Controls.Add(this.button1);
            this.tabPage3.Controls.Add(this.seva_webindex);
            this.tabPage3.Controls.Add(this.tabControlRightView);
            this.tabPage3.Controls.Add(this.comboBoxWeb);
            this.tabPage3.Location = new System.Drawing.Point(4, 21);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(719, 439);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "采集-挖掘";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            this.button7.Enabled = false;
            this.button7.Location = new System.Drawing.Point(474, 6);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(20, 23);
            this.button7.TabIndex = 14;
            this.button7.Text = "button7";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Visible = false;
            this.button7.Click += new System.EventHandler(this.button7_Click_1);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(393, 6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 13;
            this.button2.Text = "合并引索";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "指定引索保存目录:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(350, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(32, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // seva_webindex
            // 
            this.seva_webindex.Enabled = false;
            this.seva_webindex.Location = new System.Drawing.Point(117, 6);
            this.seva_webindex.Name = "seva_webindex";
            this.seva_webindex.Size = new System.Drawing.Size(227, 21);
            this.seva_webindex.TabIndex = 10;
            // 
            // comboBoxWeb
            // 
            this.comboBoxWeb.AllowDrop = true;
            this.comboBoxWeb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxWeb.BackColor = System.Drawing.Color.WhiteSmoke;
            this.comboBoxWeb.ContextMenu = this.contextMenuNavigate;
            this.comboBoxWeb.Enabled = false;
            this.comboBoxWeb.ItemHeight = 12;
            this.comboBoxWeb.Location = new System.Drawing.Point(474, 6);
            this.comboBoxWeb.MaxDropDownItems = 20;
            this.comboBoxWeb.Name = "comboBoxWeb";
            this.comboBoxWeb.Size = new System.Drawing.Size(237, 20);
            this.comboBoxWeb.TabIndex = 9;
            this.comboBoxWeb.Tag = "Settings";
            this.comboBoxWeb.Text = "http://";
            this.comboBoxWeb.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBoxWeb_KeyDown);
            // 
            // zidinyicalss
            // 
            this.zidinyicalss.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加子节点ToolStripMenuItem,
            this.添加根节点ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.修改项目规则ToolStripMenuItem,
            this.删除项目规则ToolStripMenuItem,
            this.toolStripMenuItem2});
            this.zidinyicalss.Name = "contextMenuStrip1";
            this.zidinyicalss.Size = new System.Drawing.Size(143, 104);
            // 
            // 添加子节点ToolStripMenuItem
            // 
            this.添加子节点ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("添加子节点ToolStripMenuItem.Image")));
            this.添加子节点ToolStripMenuItem.Name = "添加子节点ToolStripMenuItem";
            this.添加子节点ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.添加子节点ToolStripMenuItem.Text = "添加项目则规";
            this.添加子节点ToolStripMenuItem.Click += new System.EventHandler(this.添加子节点ToolStripMenuItem_Click);
            // 
            // 添加根节点ToolStripMenuItem
            // 
            this.添加根节点ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("添加根节点ToolStripMenuItem.Image")));
            this.添加根节点ToolStripMenuItem.Name = "添加根节点ToolStripMenuItem";
            this.添加根节点ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.添加根节点ToolStripMenuItem.Text = "添加新频道";
            this.添加根节点ToolStripMenuItem.Click += new System.EventHandler(this.添加根节点ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(139, 6);
            // 
            // 修改项目规则ToolStripMenuItem
            // 
            this.修改项目规则ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("修改项目规则ToolStripMenuItem.Image")));
            this.修改项目规则ToolStripMenuItem.Name = "修改项目规则ToolStripMenuItem";
            this.修改项目规则ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.修改项目规则ToolStripMenuItem.Text = "修改项目规则";
            // 
            // 删除项目规则ToolStripMenuItem
            // 
            this.删除项目规则ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("删除项目规则ToolStripMenuItem.Image")));
            this.删除项目规则ToolStripMenuItem.Name = "删除项目规则ToolStripMenuItem";
            this.删除项目规则ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.删除项目规则ToolStripMenuItem.Text = "删除项目规则";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(139, 6);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.系统ToolStripMenuItem,
            this.系统设定OptionsToolStripMenuItem,
            this.风格皮肤ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(926, 24);
            this.menuStrip1.TabIndex = 13;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 系统ToolStripMenuItem
            // 
            this.系统ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.最小化程序ToolStripMenuItem,
            this.退出系统ToolStripMenuItem});
            this.系统ToolStripMenuItem.Name = "系统ToolStripMenuItem";
            this.系统ToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.系统ToolStripMenuItem.Text = "系统";
            // 
            // 最小化程序ToolStripMenuItem
            // 
            this.最小化程序ToolStripMenuItem.Name = "最小化程序ToolStripMenuItem";
            this.最小化程序ToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.最小化程序ToolStripMenuItem.Text = "隐藏到托盘";
            this.最小化程序ToolStripMenuItem.Click += new System.EventHandler(this.最小化程序ToolStripMenuItem_Click);
            // 
            // 退出系统ToolStripMenuItem
            // 
            this.退出系统ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("退出系统ToolStripMenuItem.Image")));
            this.退出系统ToolStripMenuItem.Name = "退出系统ToolStripMenuItem";
            this.退出系统ToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.退出系统ToolStripMenuItem.Text = "退出　系统";
            this.退出系统ToolStripMenuItem.Click += new System.EventHandler(this.退出系统ToolStripMenuItem_Click);
            // 
            // 系统设定OptionsToolStripMenuItem
            // 
            this.系统设定OptionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.设置SettingsToolStripMenuItem,
            this.toolStripMenuItem3,
            this.mIMETypesToolStripMenuItem,
            this.输出OutputToolStripMenuItem,
            this.连接ConnectionsToolStripMenuItem,
            this.advancedToolStripMenuItem});
            this.系统设定OptionsToolStripMenuItem.Name = "系统设定OptionsToolStripMenuItem";
            this.系统设定OptionsToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.系统设定OptionsToolStripMenuItem.Text = "系统设定";
            // 
            // 设置SettingsToolStripMenuItem
            // 
            this.设置SettingsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("设置SettingsToolStripMenuItem.Image")));
            this.设置SettingsToolStripMenuItem.Name = "设置SettingsToolStripMenuItem";
            this.设置SettingsToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.设置SettingsToolStripMenuItem.Text = "设置";
            this.设置SettingsToolStripMenuItem.Click += new System.EventHandler(this.设置SettingsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(91, 6);
            // 
            // mIMETypesToolStripMenuItem
            // 
            this.mIMETypesToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("mIMETypesToolStripMenuItem.Image")));
            this.mIMETypesToolStripMenuItem.Name = "mIMETypesToolStripMenuItem";
            this.mIMETypesToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.mIMETypesToolStripMenuItem.Text = "类型";
            this.mIMETypesToolStripMenuItem.Click += new System.EventHandler(this.mIMETypesToolStripMenuItem_Click);
            // 
            // 输出OutputToolStripMenuItem
            // 
            this.输出OutputToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("输出OutputToolStripMenuItem.Image")));
            this.输出OutputToolStripMenuItem.Name = "输出OutputToolStripMenuItem";
            this.输出OutputToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.输出OutputToolStripMenuItem.Text = "输出";
            this.输出OutputToolStripMenuItem.Click += new System.EventHandler(this.输出OutputToolStripMenuItem_Click);
            // 
            // 连接ConnectionsToolStripMenuItem
            // 
            this.连接ConnectionsToolStripMenuItem.Name = "连接ConnectionsToolStripMenuItem";
            this.连接ConnectionsToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.连接ConnectionsToolStripMenuItem.Text = "连接";
            this.连接ConnectionsToolStripMenuItem.Click += new System.EventHandler(this.连接ConnectionsToolStripMenuItem_Click);
            // 
            // advancedToolStripMenuItem
            // 
            this.advancedToolStripMenuItem.Name = "advancedToolStripMenuItem";
            this.advancedToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.advancedToolStripMenuItem.Text = "排除";
            this.advancedToolStripMenuItem.Click += new System.EventHandler(this.advancedToolStripMenuItem_Click);
            // 
            // 风格皮肤ToolStripMenuItem
            // 
            this.风格皮肤ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem8,
            this.toolStripMenuItem9,
            this.toolStripMenuItem10,
            this.toolStripMenuItem11,
            this.风格五ToolStripMenuItem,
            this.风格六ToolStripMenuItem,
            this.风格七ToolStripMenuItem,
            this.toolStripMenuItem7,
            this.原始经典ToolStripMenuItem});
            this.风格皮肤ToolStripMenuItem.Name = "风格皮肤ToolStripMenuItem";
            this.风格皮肤ToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.风格皮肤ToolStripMenuItem.Text = "风格皮肤";
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem8.Image")));
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(118, 22);
            this.toolStripMenuItem8.Text = "风格(一)";
            this.toolStripMenuItem8.Click += new System.EventHandler(this.toolStripMenuItem8_Click);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem9.Image")));
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(118, 22);
            this.toolStripMenuItem9.Text = "风格(二)";
            this.toolStripMenuItem9.Click += new System.EventHandler(this.toolStripMenuItem9_Click);
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem10.Image")));
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new System.Drawing.Size(118, 22);
            this.toolStripMenuItem10.Text = "风格(三)";
            this.toolStripMenuItem10.Click += new System.EventHandler(this.toolStripMenuItem10_Click);
            // 
            // toolStripMenuItem11
            // 
            this.toolStripMenuItem11.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem11.Image")));
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new System.Drawing.Size(118, 22);
            this.toolStripMenuItem11.Text = "风格(四)";
            this.toolStripMenuItem11.Click += new System.EventHandler(this.toolStripMenuItem11_Click);
            // 
            // 风格五ToolStripMenuItem
            // 
            this.风格五ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("风格五ToolStripMenuItem.Image")));
            this.风格五ToolStripMenuItem.Name = "风格五ToolStripMenuItem";
            this.风格五ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.风格五ToolStripMenuItem.Text = "风格(五)";
            this.风格五ToolStripMenuItem.Click += new System.EventHandler(this.风格五ToolStripMenuItem_Click);
            // 
            // 风格六ToolStripMenuItem
            // 
            this.风格六ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("风格六ToolStripMenuItem.Image")));
            this.风格六ToolStripMenuItem.Name = "风格六ToolStripMenuItem";
            this.风格六ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.风格六ToolStripMenuItem.Text = "风格(六)";
            this.风格六ToolStripMenuItem.Click += new System.EventHandler(this.风格六ToolStripMenuItem_Click);
            // 
            // 风格七ToolStripMenuItem
            // 
            this.风格七ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("风格七ToolStripMenuItem.Image")));
            this.风格七ToolStripMenuItem.Name = "风格七ToolStripMenuItem";
            this.风格七ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.风格七ToolStripMenuItem.Text = "风格(七)";
            this.风格七ToolStripMenuItem.Click += new System.EventHandler(this.风格七ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(115, 6);
            // 
            // 原始经典ToolStripMenuItem
            // 
            this.原始经典ToolStripMenuItem.Name = "原始经典ToolStripMenuItem";
            this.原始经典ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.原始经典ToolStripMenuItem.Text = "原始经典";
            this.原始经典ToolStripMenuItem.Click += new System.EventHandler(this.原始经典ToolStripMenuItem_Click);
            // 
            // skinEngine1
            // 
            this.skinEngine1.SerialNumber = "";
            this.skinEngine1.SkinFile = null;
            // 
            // tabPage9
            // 
            this.tabPage9.Location = new System.Drawing.Point(4, 22);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage9.Size = new System.Drawing.Size(544, 520);
            this.tabPage9.TabIndex = 0;
            this.tabPage9.Text = "WEB登录";
            this.tabPage9.UseVisualStyleBackColor = true;
            // 
            // tabPage10
            // 
            this.tabPage10.Location = new System.Drawing.Point(4, 22);
            this.tabPage10.Name = "tabPage10";
            this.tabPage10.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage10.Size = new System.Drawing.Size(946, 520);
            this.tabPage10.TabIndex = 1;
            this.tabPage10.Text = "数据整理发布";
            this.tabPage10.UseVisualStyleBackColor = true;
            // 
            // tabPage11
            // 
            this.tabPage11.Controls.Add(this.groupBox2);
            this.tabPage11.Location = new System.Drawing.Point(4, 22);
            this.tabPage11.Name = "tabPage11";
            this.tabPage11.Size = new System.Drawing.Size(780, 514);
            this.tabPage11.TabIndex = 2;
            this.tabPage11.Text = "发布模块配置";
            this.tabPage11.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.textBox5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.textBox6);
            this.groupBox2.Controls.Add(this.textBox7);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.linkLabel8);
            this.groupBox2.Controls.Add(this.linkLabel9);
            this.groupBox2.Controls.Add(this.linkLabel10);
            this.groupBox2.Controls.Add(this.linkLabel11);
            this.groupBox2.Controls.Add(this.linkLabel12);
            this.groupBox2.Controls.Add(this.linkLabel13);
            this.groupBox2.Controls.Add(this.linkLabel14);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.textBox8);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.button4);
            this.groupBox2.Location = new System.Drawing.Point(18, 41);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(595, 424);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "WEB发布配置";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(90, 24);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(280, 21);
            this.textBox5.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 16;
            this.label6.Text = "模块名称：";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(442, 347);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 15;
            this.button3.Text = "重　置";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(92, 270);
            this.textBox6.Multiline = true;
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(278, 55);
            this.textBox6.TabIndex = 14;
            this.textBox6.Text = "失败了.";
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(91, 203);
            this.textBox7.Multiline = true;
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(279, 60);
            this.textBox7.TabIndex = 13;
            this.textBox7.Text = "恭喜发布成功.";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(20, 273);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 12;
            this.label7.Text = "失败标识：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(20, 204);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 11;
            this.label8.Text = "成功标识：";
            // 
            // linkLabel8
            // 
            this.linkLabel8.AutoSize = true;
            this.linkLabel8.Location = new System.Drawing.Point(416, 153);
            this.linkLabel8.Name = "linkLabel8";
            this.linkLabel8.Size = new System.Drawing.Size(77, 12);
            this.linkLabel8.TabIndex = 10;
            this.linkLabel8.TabStop = true;
            this.linkLabel8.Text = "六位随机字母";
            // 
            // linkLabel9
            // 
            this.linkLabel9.AutoSize = true;
            this.linkLabel9.Location = new System.Drawing.Point(324, 153);
            this.linkLabel9.Name = "linkLabel9";
            this.linkLabel9.Size = new System.Drawing.Size(77, 12);
            this.linkLabel9.TabIndex = 9;
            this.linkLabel9.TabStop = true;
            this.linkLabel9.Text = "六位随机数字";
            // 
            // linkLabel10
            // 
            this.linkLabel10.AutoSize = true;
            this.linkLabel10.Location = new System.Drawing.Point(272, 152);
            this.linkLabel10.Name = "linkLabel10";
            this.linkLabel10.Size = new System.Drawing.Size(29, 12);
            this.linkLabel10.TabIndex = 8;
            this.linkLabel10.TabStop = true;
            this.linkLabel10.Text = "来源";
            // 
            // linkLabel11
            // 
            this.linkLabel11.AutoSize = true;
            this.linkLabel11.Location = new System.Drawing.Point(224, 152);
            this.linkLabel11.Name = "linkLabel11";
            this.linkLabel11.Size = new System.Drawing.Size(29, 12);
            this.linkLabel11.TabIndex = 7;
            this.linkLabel11.TabStop = true;
            this.linkLabel11.Text = "作者";
            // 
            // linkLabel12
            // 
            this.linkLabel12.AutoSize = true;
            this.linkLabel12.Location = new System.Drawing.Point(179, 152);
            this.linkLabel12.Name = "linkLabel12";
            this.linkLabel12.Size = new System.Drawing.Size(29, 12);
            this.linkLabel12.TabIndex = 6;
            this.linkLabel12.TabStop = true;
            this.linkLabel12.Text = "内容";
            // 
            // linkLabel13
            // 
            this.linkLabel13.AutoSize = true;
            this.linkLabel13.Location = new System.Drawing.Point(134, 152);
            this.linkLabel13.Name = "linkLabel13";
            this.linkLabel13.Size = new System.Drawing.Size(29, 12);
            this.linkLabel13.TabIndex = 5;
            this.linkLabel13.TabStop = true;
            this.linkLabel13.Text = "导读";
            // 
            // linkLabel14
            // 
            this.linkLabel14.AutoSize = true;
            this.linkLabel14.Location = new System.Drawing.Point(89, 152);
            this.linkLabel14.Name = "linkLabel14";
            this.linkLabel14.Size = new System.Drawing.Size(29, 12);
            this.linkLabel14.TabIndex = 4;
            this.linkLabel14.TabStop = true;
            this.linkLabel14.Text = "标题";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(18, 153);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 3;
            this.label9.Text = "发布参数：";
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(91, 65);
            this.textBox8.Multiline = true;
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(602, 67);
            this.textBox8.TabIndex = 2;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(18, 65);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 1;
            this.label10.Text = "发布地址：";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(549, 347);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 0;
            this.button4.Text = "确　定";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // tabPage12
            // 
            this.tabPage12.Location = new System.Drawing.Point(4, 22);
            this.tabPage12.Name = "tabPage12";
            this.tabPage12.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage12.Size = new System.Drawing.Size(780, 514);
            this.tabPage12.TabIndex = 0;
            this.tabPage12.Text = "WEB登录";
            this.tabPage12.UseVisualStyleBackColor = true;
            // 
            // tabPage13
            // 
            this.tabPage13.Location = new System.Drawing.Point(4, 22);
            this.tabPage13.Name = "tabPage13";
            this.tabPage13.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage13.Size = new System.Drawing.Size(780, 514);
            this.tabPage13.TabIndex = 1;
            this.tabPage13.Text = "数据整理发布";
            this.tabPage13.UseVisualStyleBackColor = true;
            // 
            // tabPage14
            // 
            this.tabPage14.Controls.Add(this.groupBox3);
            this.tabPage14.Location = new System.Drawing.Point(4, 22);
            this.tabPage14.Name = "tabPage14";
            this.tabPage14.Size = new System.Drawing.Size(780, 514);
            this.tabPage14.TabIndex = 2;
            this.tabPage14.Text = "发布模块配置";
            this.tabPage14.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.textBox9);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.button5);
            this.groupBox3.Controls.Add(this.textBox10);
            this.groupBox3.Controls.Add(this.textBox11);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.linkLabel15);
            this.groupBox3.Controls.Add(this.linkLabel16);
            this.groupBox3.Controls.Add(this.linkLabel17);
            this.groupBox3.Controls.Add(this.linkLabel18);
            this.groupBox3.Controls.Add(this.linkLabel19);
            this.groupBox3.Controls.Add(this.linkLabel20);
            this.groupBox3.Controls.Add(this.linkLabel21);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.textBox12);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.button6);
            this.groupBox3.Location = new System.Drawing.Point(18, 41);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(595, 424);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "WEB发布配置";
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(90, 24);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(280, 21);
            this.textBox9.TabIndex = 17;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(18, 27);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 12);
            this.label11.TabIndex = 16;
            this.label11.Text = "模块名称：";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(442, 347);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 15;
            this.button5.Text = "重　置";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // textBox10
            // 
            this.textBox10.Location = new System.Drawing.Point(92, 270);
            this.textBox10.Multiline = true;
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(278, 55);
            this.textBox10.TabIndex = 14;
            this.textBox10.Text = "失败了.";
            // 
            // textBox11
            // 
            this.textBox11.Location = new System.Drawing.Point(91, 203);
            this.textBox11.Multiline = true;
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(279, 60);
            this.textBox11.TabIndex = 13;
            this.textBox11.Text = "恭喜发布成功.";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(20, 273);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(65, 12);
            this.label12.TabIndex = 12;
            this.label12.Text = "失败标识：";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(20, 204);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(65, 12);
            this.label13.TabIndex = 11;
            this.label13.Text = "成功标识：";
            // 
            // linkLabel15
            // 
            this.linkLabel15.AutoSize = true;
            this.linkLabel15.Location = new System.Drawing.Point(416, 153);
            this.linkLabel15.Name = "linkLabel15";
            this.linkLabel15.Size = new System.Drawing.Size(77, 12);
            this.linkLabel15.TabIndex = 10;
            this.linkLabel15.TabStop = true;
            this.linkLabel15.Text = "六位随机字母";
            // 
            // linkLabel16
            // 
            this.linkLabel16.AutoSize = true;
            this.linkLabel16.Location = new System.Drawing.Point(324, 153);
            this.linkLabel16.Name = "linkLabel16";
            this.linkLabel16.Size = new System.Drawing.Size(77, 12);
            this.linkLabel16.TabIndex = 9;
            this.linkLabel16.TabStop = true;
            this.linkLabel16.Text = "六位随机数字";
            // 
            // linkLabel17
            // 
            this.linkLabel17.AutoSize = true;
            this.linkLabel17.Location = new System.Drawing.Point(272, 152);
            this.linkLabel17.Name = "linkLabel17";
            this.linkLabel17.Size = new System.Drawing.Size(29, 12);
            this.linkLabel17.TabIndex = 8;
            this.linkLabel17.TabStop = true;
            this.linkLabel17.Text = "来源";
            // 
            // linkLabel18
            // 
            this.linkLabel18.AutoSize = true;
            this.linkLabel18.Location = new System.Drawing.Point(224, 152);
            this.linkLabel18.Name = "linkLabel18";
            this.linkLabel18.Size = new System.Drawing.Size(29, 12);
            this.linkLabel18.TabIndex = 7;
            this.linkLabel18.TabStop = true;
            this.linkLabel18.Text = "作者";
            // 
            // linkLabel19
            // 
            this.linkLabel19.AutoSize = true;
            this.linkLabel19.Location = new System.Drawing.Point(179, 152);
            this.linkLabel19.Name = "linkLabel19";
            this.linkLabel19.Size = new System.Drawing.Size(29, 12);
            this.linkLabel19.TabIndex = 6;
            this.linkLabel19.TabStop = true;
            this.linkLabel19.Text = "内容";
            // 
            // linkLabel20
            // 
            this.linkLabel20.AutoSize = true;
            this.linkLabel20.Location = new System.Drawing.Point(134, 152);
            this.linkLabel20.Name = "linkLabel20";
            this.linkLabel20.Size = new System.Drawing.Size(29, 12);
            this.linkLabel20.TabIndex = 5;
            this.linkLabel20.TabStop = true;
            this.linkLabel20.Text = "导读";
            // 
            // linkLabel21
            // 
            this.linkLabel21.AutoSize = true;
            this.linkLabel21.Location = new System.Drawing.Point(89, 152);
            this.linkLabel21.Name = "linkLabel21";
            this.linkLabel21.Size = new System.Drawing.Size(29, 12);
            this.linkLabel21.TabIndex = 4;
            this.linkLabel21.TabStop = true;
            this.linkLabel21.Text = "标题";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(18, 153);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(65, 12);
            this.label14.TabIndex = 3;
            this.label14.Text = "发布参数：";
            // 
            // textBox12
            // 
            this.textBox12.Location = new System.Drawing.Point(91, 65);
            this.textBox12.Multiline = true;
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new System.Drawing.Size(602, 67);
            this.textBox12.TabIndex = 2;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(18, 65);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(65, 12);
            this.label15.TabIndex = 1;
            this.label15.Text = "发布地址：";
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(549, 347);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 0;
            this.button6.Text = "确　定";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // toolBarButton3
            // 
            this.toolBarButton3.Name = "toolBarButton3";
            this.toolBarButton3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButtonSettings
            // 
            this.toolBarButtonSettings.DropDownMenu = this.contextMenuSettings;
            this.toolBarButtonSettings.ImageIndex = 4;
            this.toolBarButtonSettings.Name = "toolBarButtonSettings";
            this.toolBarButtonSettings.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
            this.toolBarButtonSettings.Text = "系统设置";
            this.toolBarButtonSettings.ToolTipText = "Show settings form";
            // 
            // toolBarButton2
            // 
            this.toolBarButton2.Name = "toolBarButton2";
            this.toolBarButton2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButtonDeleteAll
            // 
            this.toolBarButtonDeleteAll.ImageIndex = 3;
            this.toolBarButtonDeleteAll.Name = "toolBarButtonDeleteAll";
            this.toolBarButtonDeleteAll.Text = "清空日志";
            this.toolBarButtonDeleteAll.ToolTipText = "Delete all results";
            // 
            // toolBarButtonStop
            // 
            this.toolBarButtonStop.ImageIndex = 2;
            this.toolBarButtonStop.Name = "toolBarButtonStop";
            this.toolBarButtonStop.Text = "停止工作";
            this.toolBarButtonStop.ToolTipText = "Stop parsing process";
            // 
            // toolBarButtonPause
            // 
            this.toolBarButtonPause.Enabled = false;
            this.toolBarButtonPause.ImageIndex = 1;
            this.toolBarButtonPause.Name = "toolBarButtonPause";
            this.toolBarButtonPause.Text = "暂停抓取";
            this.toolBarButtonPause.ToolTipText = "Pause parsing process";
            this.toolBarButtonPause.Visible = false;
            // 
            // toolBarButtonContinue
            // 
            this.toolBarButtonContinue.Enabled = false;
            this.toolBarButtonContinue.ImageIndex = 0;
            this.toolBarButtonContinue.Name = "toolBarButtonContinue";
            this.toolBarButtonContinue.Text = "开始任务";
            this.toolBarButtonContinue.ToolTipText = "Coninue parsing process";
            this.toolBarButtonContinue.Visible = false;
            // 
            // toolBarMain
            // 
            this.toolBarMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.toolBarMain.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.toolBarMain.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarButtonContinue,
            this.toolBarButtonPause,
            this.toolBarButtonStop,
            this.toolBarButtonDeleteAll,
            this.toolBarButton2,
            this.toolBarButtonSettings,
            this.toolBarButton3,
            this.toolBarButton1,
            this.toolBarButton5,
            this.toolBarButton6,
            this.toolBarButton7});
            this.toolBarMain.ButtonSize = new System.Drawing.Size(30, 10);
            this.toolBarMain.Dock = System.Windows.Forms.DockStyle.None;
            this.toolBarMain.DropDownArrows = true;
            this.toolBarMain.ImageList = this.imageList2;
            this.toolBarMain.Location = new System.Drawing.Point(0, 26);
            this.toolBarMain.Name = "toolBarMain";
            this.toolBarMain.ShowToolTips = true;
            this.toolBarMain.Size = new System.Drawing.Size(927, 28);
            this.toolBarMain.TabIndex = 0;
            this.toolBarMain.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
            this.toolBarMain.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBarMain_ButtonClick);
            // 
            // toolBarButton1
            // 
            this.toolBarButton1.ImageIndex = 8;
            this.toolBarButton1.Name = "toolBarButton1";
            this.toolBarButton1.Text = "分类管理";
            // 
            // toolBarButton5
            // 
            this.toolBarButton5.ImageIndex = 7;
            this.toolBarButton5.Name = "toolBarButton5";
            this.toolBarButton5.Text = "规则管理";
            // 
            // toolBarButton6
            // 
            this.toolBarButton6.ImageIndex = 5;
            this.toolBarButton6.Name = "toolBarButton6";
            this.toolBarButton6.Text = "多引擎合并";
            // 
            // timer1
            // 
            this.timer1.Interval = 10000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Interval = 1000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "军长搜索";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.最小化程序ToolStripMenuItem1,
            this.最大化ToolStripMenuItem,
            this.退出系统ToolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(131, 70);
            // 
            // 最小化程序ToolStripMenuItem1
            // 
            this.最小化程序ToolStripMenuItem1.Name = "最小化程序ToolStripMenuItem1";
            this.最小化程序ToolStripMenuItem1.Size = new System.Drawing.Size(130, 22);
            this.最小化程序ToolStripMenuItem1.Text = "隐藏到托盘";
            this.最小化程序ToolStripMenuItem1.Click += new System.EventHandler(this.最小化程序ToolStripMenuItem1_Click);
            // 
            // 最大化ToolStripMenuItem
            // 
            this.最大化ToolStripMenuItem.Name = "最大化ToolStripMenuItem";
            this.最大化ToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.最大化ToolStripMenuItem.Text = "还原到桌面";
            this.最大化ToolStripMenuItem.Click += new System.EventHandler(this.最大化ToolStripMenuItem_Click);
            // 
            // 退出系统ToolStripMenuItem1
            // 
            this.退出系统ToolStripMenuItem1.Name = "退出系统ToolStripMenuItem1";
            this.退出系统ToolStripMenuItem1.Size = new System.Drawing.Size(130, 22);
            this.退出系统ToolStripMenuItem1.Text = "退出　程序";
            this.退出系统ToolStripMenuItem1.Click += new System.EventHandler(this.退出系统ToolStripMenuItem1_Click);
            // 
            // toolBarButton7
            // 
            this.toolBarButton7.ImageIndex = 9;
            this.toolBarButton7.Name = "toolBarButton7";
            this.toolBarButton7.Text = "Xml设置";
            // 
            // CrawlerForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(926, 564);
            this.Controls.Add(this.toolBarMain);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "CrawlerForm";
            this.Text = "军长搜索 V 1.5";
            this.Load += new System.EventHandler(this.CrawlerForm_Load);
            this.SizeChanged += new System.EventHandler(this.CrawlerForm_SizeChanged);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.CrawlerForm_Closing);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CrawlerForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanelInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanelURLs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanelFiles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanelByteCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanelErrors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanelCPU)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanelMem)).EndInit();
            this.tabControlRightView.ResumeLayout(false);
            this.tabPageThreads.ResumeLayout(false);
            this.tabPageRequests.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.tabPageErrors.ResumeLayout(false);
            this.tabPageErrors.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.webtongbu.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.zidinyicalss.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabPage11.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage14.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void InitValues()
        {
            this.WebDepth = Settings.GetValue("Web depth", 3);
            this.RequestTimeout = Settings.GetValue("Request timeout", 20);
            this.SleepFetchTime = Settings.GetValue("Sleep fetch time", 2);
            this.SleepConnectTime = Settings.GetValue("Sleep connect time", 1);
            this.KeepSameServer = Settings.GetValue("Keep same URL server", false);
            this.AllMIMETypes = Settings.GetValue("Allow all MIME types", true);
            this.KeepAlive = Settings.GetValue("Keep connection alive", true);
            this.ExcludeHosts = Settings.GetValue("Exclude Hosts", ".org; .gov;").Replace("*", "").ToLower().Split(new char[] { ';' });
            this.ExcludeWords = Settings.GetValue("Exclude words", "").Split(new char[] { ';' });
            this.ExcludeFiles = Settings.GetValue("Exclude files", "").Replace("*", "").ToLower().Split(new char[] { ';' });
            this.LastRequestCount = Settings.GetValue("View last requests count", 20);
            this.Downloadfolder = Settings.GetValue("Download folder", Environment.GetFolderPath(Environment.SpecialFolder.Personal));
            this.MIMETypes = GetMIMETypes();
            this.TextEncoding = GetTextEncoding();
        }

        [DllImport("wininet")]
        public static extern int InternetAutodial(int dwFlags, int hwndParent);
        [DllImport("wininet")]
        public static extern int InternetGetConnectedState(ref int lpdwFlags, int dwReserved);
        private string InternetGetConnectedStateString()
        {
            string str = "";
            try
            {
                int lpdwFlags = 0;
                if (InternetGetConnectedState(ref lpdwFlags, 0) == 0)
                {
                    return "您目前没有连接到互联网";
                }
                if ((lpdwFlags & 1) == 1)
                {
                    str = "调制解调器连接";
                }
                else if ((lpdwFlags & 2) == 2)
                {
                    str = "LAN连接";
                }
                else if ((lpdwFlags & 4) == 4)
                {
                    str = "代理连接";
                }
                else if ((lpdwFlags & 8) == 8)
                {
                    str = "非网际网路连线";
                }
                else if ((lpdwFlags & 0x10) == 0x10)
                {
                    str = "远程访问服务器";
                }
                else
                {
                    if ((lpdwFlags & 0x20) == 0x20)
                    {
                        return "离线";
                    }
                    if ((lpdwFlags & 0x40) == 0x40)
                    {
                        return "网际网路连线";
                    }
                }
                IPHostEntry entry = Dns.Resolve(Dns.GetHostName());
                str = str + ",本机IP: " + entry.AddressList[0].ToString();
            }
            catch
            {
            }
            return str;
        }

        private void listViewErrors_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listViewErrors.SelectedItems.Count != 0)
            {
                ListViewItem item = this.listViewErrors.SelectedItems[0];
                this.textBoxErrorDescription.Text = item.SubItems[3].Text;
            }
        }

        private void listViewRequests_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listViewRequests.SelectedItems.Count > 0)
            {
                ListViewItem item = this.listViewRequests.SelectedItems[this.listViewRequests.SelectedItems.Count - 1];
                ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["RedSpider"];
                OleDbConnection connection = new OleDbConnection(settings.ConnectionString);
                OleDbCommand selectCommand = new OleDbCommand("SELECT * FROM Spider_News where News_Title='" + item.SubItems[1].Text + "'", connection);
                OleDbDataAdapter adapter = new OleDbDataAdapter(selectCommand);
                DataSet dataSet = new DataSet();
                dataSet.Clear();
                adapter.Fill(dataSet, "Spider_News");
                string str2 = dataSet.Tables["Spider_News"].Rows[0]["News_Title"].ToString();
                string str3 = dataSet.Tables["Spider_News"].Rows[0]["News_Review"].ToString();
                string str4 = dataSet.Tables["Spider_News"].Rows[0]["News_Content"].ToString();
                string str5 = dataSet.Tables["Spider_News"].Rows[0]["News_Author"].ToString();
                string str6 = dataSet.Tables["Spider_News"].Rows[0]["News_Source"].ToString();
                string str7 = dataSet.Tables["Spider_News"].Rows[0]["News_Url"].ToString();
                string str8 = dataSet.Tables["Spider_News"].Rows[0]["News_Addtime"].ToString();
                this.webBrowser1.DocumentText = "<html><head><title>Redspider</title><style type=\"text/css\"><!--body,td,th {font-family: 新宋体;font-size: 14px;}body {margin-left: 0px;margin-top: 0px;margin-right: 0px;margin-bottom: 0px;background-color: #F0F0F0;}.style1 {font-size: 18px;font-weight: bold;}--></style></head><body><table width=\"99%\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\"><tr><td align=\"center\"><hr><table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr><td height=\"40\" align=\"center\"><span class=\"style1\">" + str2 + "</span></td></tr><tr><td><table width=\"80%\"  border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\"><tr><td width=\"5%\">作者:</td><td width=\"20%\">" + str5 + "</td><td width=\"5%\">来源:</td><td width=\"22%\">" + str6 + "</td><td width=\"6%\">时间:</td><td width=\"32%\">" + str8 + "</td><td width=\"10%\"><a href=\"" + str7 + "\" target=\"_blank\">查看原文</a></td></tr></table><hr></td></tr></table></td></tr><tr><td height=\"7\" bgcolor=\"#FFCC00\"></td></tr><tr><td align=\"center\"><table width=\"100%\"  border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tr><td>&nbsp;</td></tr><tr><td align=\"left\">" + str3 + "<br></td></tr><tr><td align=\"left\">" + str4 + "<br></td></tr><tr><td><table width=\"50%\"  border=\"0\" align=\"right\" cellpadding=\"0\" cellspacing=\"0\"><tr><td width=\"15%\">入库信息:</td><td width=\"85%\">【入库信息】</td></tr></table></td></tr></table><hr><br>⊙ RedSpider 2008</td></tr></table></body></html>";
            }
        }

        private void listViewThreads_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void LogCell(ref ListViewItem itemLog, int nCell, string str)
        {
            Monitor.Enter(this.listViewThreads);
            try
            {
                itemLog.SubItems[nCell].Text = str;
            }
            catch (Exception)
            {
            }
            Monitor.Exit(this.listViewThreads);
        }

        private void LogError(string strHead, string strBody)
        {
            Monitor.Enter(this.listViewErrors);
            try
            {
                int num = this.ErrorCount + 1;
                this.listViewErrors.Items.Add(num.ToString()).SubItems.AddRange(new string[] { DateTime.Now.ToString(), strHead, strBody });
                while (this.listViewErrors.Items.Count > 500)
                {
                    this.listViewErrors.Items.RemoveAt(0);
                }
            }
            catch (Exception)
            {
            }
            Monitor.Exit(this.listViewErrors);
            this.ErrorCount++;
        }

        public void LogUri(string bt, string zt)
        {
            Monitor.Enter(this.listView1);
            try
            {
                ListViewItem item = this.listView1.Items.Insert(0, DateTime.Now.ToString());
                item.SubItems.AddRange(new string[] { bt });
                item.SubItems.AddRange(new string[] { zt });
                while (this.listView1.Items.Count > 200)
                {
                    this.listView1.Items.Clear();
                }
            }
            catch (Exception)
            {
            }
            Monitor.Exit(this.listView1);
        }

        public void M_jk_time()
        {
            try
            {
                OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
                string cmdText = "SELECT * FROM Mission_Time";
                OleDbCommand selectCommand = new OleDbCommand(cmdText, connection);
                OleDbDataAdapter adapter = new OleDbDataAdapter(selectCommand);
                DataSet dataSet = new DataSet();
                dataSet.Clear();
                adapter.Fill(dataSet, "Mission_Time");
                this.label18.Text = dataSet.Tables["Mission_Time"].Rows[0]["Mission_Time"].ToString();
                this.seva_webindex.Text = dataSet.Tables["Mission_Time"].Rows[0]["index_file"].ToString();
                this.dateTimePicker3.Value = DateTime.Parse(this.label18.Text);
                if (dataSet.Tables["Mission_Time"].Rows[0]["Monitoring"].ToString() == "False")
                {
                    this.checkBox1.Checked = false;
                    this.ssjk = false;
                }
                else
                {
                    this.checkBox1.Checked = true;
                    this.ssjk = true;
                }
                if (dataSet.Tables["Mission_Time"].Rows[0]["qidong"].ToString() == "是")
                {
                    this.checkBox2.Checked = true;
                }
                else
                {
                    this.checkBox2.Checked = false;
                }
                this.textBox13.Text = dataSet.Tables["Mission_Time"].Rows[0]["wzgs"].ToString();
                if (dataSet.Tables["Mission_Time"].Rows[0]["ramCounter"].ToString() == "是")
                {
                    Thread.Sleep(0x1388);
                    OleDbConnection connection2 = new OleDbConnection(CONN_ACCESS.ConnString);
                    string str2 = "update Mission_Time SET [ramCounter]='否'";
                    OleDbCommand command2 = new OleDbCommand(str2, connection2);
                    connection2.Open();
                    command2.ExecuteNonQuery();
                    connection2.Close();
                    this.timer1.Enabled = true;
                    this.ssjk = true;
                    if (this.ssjk)
                    {
                        this.M_jk_URL();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("任务配置发生错误，这种错误一般由数库损坏造成，请联系我们", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        public void M_jk_URL()
        {
            try
            {
                OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
                string cmdText = "SELECT * FROM Web_URL WHERE parentid<>'0'";
                OleDbCommand selectCommand = new OleDbCommand(cmdText, connection);
                OleDbDataAdapter adapter = new OleDbDataAdapter(selectCommand);
                this.dss = new DataSet();
                this.dss.Clear();
                adapter.Fill(this.dss, "Web_URL");
                this.dtSource = this.dss.Tables["Web_URL"];
                Global.COLUMN_ID = this.dss.Tables["Web_URL"].Rows[int.Parse(this.textBox13.Text) - 1]["id"].ToString();
                this.textBox2.Text = this.textBox13.Text;
                this.textBox3.Text = this.dss.Tables["Web_URL"].Rows[int.Parse(this.textBox13.Text) - 1]["column_name"].ToString();
                this.textBox4.Text = this.dss.Tables["Web_URL"].Rows[int.Parse(this.textBox13.Text) - 1]["web_url"].ToString();
                this.dscjwzgs = int.Parse(this.textBox13.Text) - 1;
                this.threadParse = new Thread(new ThreadStart(this.StartParsing));
                this.threadParse.Start();
                this.textBox1.Text = this.dtSource.Rows.Count.ToString();
            }
            catch (Exception)
            {
            }
        }

        [STAThread]
        private static void Main()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CrawlerForm());
        }

        public string MD5string(string input)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(input, "MD5");
        }

        private void menuItemAbout_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }

        private void menuItemAdvanced_Click(object sender, EventArgs e)
        {
            this.ShowSettings(3);
        }

        private void menuItemConnections_Click(object sender, EventArgs e)
        {
            this.ShowSettings(2);
        }

        private void menuItemCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(this.comboBoxWeb.SelectedText);
        }

        private void menuItemCut_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(this.comboBoxWeb.SelectedText);
            this.comboBoxWeb.SelectedText = "";
        }

        private void menuItemDelete_Click(object sender, EventArgs e)
        {
            this.comboBoxWeb.SelectedText = "";
        }

        private void menuItemExit_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void menuItemFileBrowse_Click(object sender, EventArgs e)
        {
            this.OnFileBrowse();
        }

        private void menuItemFileMatches_Click(object sender, EventArgs e)
        {
            this.ShowSettings(0);
        }

        private void menuItemHttp_Click(object sender, EventArgs e)
        {
            this.comboBoxWeb.Text = "http://";
        }

        private void menuItemListClear_Click(object sender, EventArgs e)
        {
            string text = this.tabControlRightView.SelectedTab.Text;
            if (text != null)
            {
                if (!(text == "Threads"))
                {
                    if (text == "Requests")
                    {
                        this.listViewRequests.Items.Clear();
                    }
                    else if (text == "Errors")
                    {
                        this.listViewErrors.Items.Clear();
                    }
                }
                else
                {
                    this.listViewThreads.Items.Clear();
                }
            }
        }

        private void menuItemListDeleteAll_Click(object sender, EventArgs e)
        {
            this.DeleteAllItems();
        }

        private void menuItemOutput_Click(object sender, EventArgs e)
        {
            this.ShowSettings(1);
        }

        private void menuItemPaste_Click(object sender, EventArgs e)
        {
            IDataObject dataObject = Clipboard.GetDataObject();
            if (dataObject != null)
            {
                this.comboBoxWeb.SelectedText = dataObject.GetData("System.String").ToString();
            }
        }

        private void menuItemSettings_Click(object sender, EventArgs e)
        {
            this.ShowSettings(-1);
        }

        private void menuItemSettingsAdvanced_Click(object sender, EventArgs e)
        {
            this.ShowSettings(3);
        }

        private void menuItemSettingsConnections_Click(object sender, EventArgs e)
        {
            this.ShowSettings(2);
        }

        private void menuItemSettingsFileMatches_Click(object sender, EventArgs e)
        {
            this.ShowSettings(0);
        }

        private void menuItemSettingsOutput_Click(object sender, EventArgs e)
        {
            this.ShowSettings(1);
        }

        private void mIMETypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ShowSettings(0);
        }

        private void Normalize(ref string strURL)
        {
            if (!strURL.StartsWith("http://"))
            {
                strURL = "http://" + strURL;
            }
            if (strURL.IndexOf("/", 8) == -1)
            {
                strURL = strURL + '/';
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            base.ShowInTaskbar = true;
            base.WindowState = FormWindowState.Normal;
            base.Visible = true;
        }

        private void OnBrowse(object sender, EventArgs e)
        {
            this.comboBoxWeb.Text = ((MenuItem) sender).Text;
        }

        private void OnFileBrowse()
        {
            OpenFileDialog dialog = new OpenFileDialog {
                Title = "Select file to parse",
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.comboBoxWeb.Text = dialog.FileName;
            }
        }

        private void ParseFolder(string folderName, int nDepth)
        {
            DirectoryInfo info = new DirectoryInfo(folderName);
            FileInfo[] files = info.GetFiles("*.txt");
            foreach (FileInfo info2 in files)
            {
                if (!this.ThreadsRunning)
                {
                    break;
                }
                MyUri uri = new MyUri(info2.FullName) {
                    Depth = nDepth
                };
                this.EnqueueUri(uri, true);
            }
            DirectoryInfo[] directories = info.GetDirectories();
            foreach (DirectoryInfo info3 in directories)
            {
                if (!this.ThreadsRunning)
                {
                    break;
                }
                this.ParseFolder(info3.FullName, nDepth + 1);
            }
        }

        private void ParseUri(MyUri uri, ref MyWebRequest request)
        {
            string str = "";
            if ((request != null) && request.response.KeepAlive)
            {
                str = str + "连接转至: " + uri.Host + "\r\n\r\n";
            }
            else
            {
                str = str + "连接: " + uri.Host + "\r\n\r\n";
            }
            ListViewItem item = null;
            Monitor.Enter(this.listViewThreads);
            try
            {
                item = this.listViewThreads.Items[int.Parse(Thread.CurrentThread.Name)];
                item.SubItems[1].Text = uri.Depth.ToString();
                item.ImageIndex = 1;
                item.BackColor = System.Drawing.Color.WhiteSmoke;
                item.SubItems[2].Text = "正在连接";
                item.ForeColor = System.Drawing.Color.Red;
                item.SubItems[3].Text = uri.AbsoluteUri;
                item.SubItems[4].Text = "";
                item.SubItems[5].Text = "";
            }
            catch (Exception)
            {
            }
            Monitor.Exit(this.listViewThreads);
            try
            {
                object obj2;
                request = MyWebRequest.Create(uri, request, this.KeepAlive);
                request.Timeout = this.RequestTimeout * 0x3e8;
                MyWebResponse response = request.GetResponse();
                str = str + request.Header + response.Header;
                if (!response.ResponseUri.Equals(uri))
                {
                    this.EnqueueUri(new MyUri(response.ResponseUri.AbsoluteUri), true);
                    obj2 = str;
                    str = string.Concat(new object[] { obj2, "重定向到: ", response.ResponseUri, "\r\n" });
                    request = null;
                }
                else
                {
                    if ((!this.AllMIMETypes && (response.ContentType != null)) && (this.MIMETypes.Length > 0))
                    {
                        string str2 = response.ContentType.ToLower();
                        int index = str2.IndexOf(';');
                        if (index != -1)
                        {
                            str2 = str2.Substring(0, index);
                        }
                        if ((str2.IndexOf('*') == -1) && ((index = this.MIMETypes.IndexOf(str2)) == -1))
                        {
                            this.LogError(uri.AbsoluteUri, str + "\r\nUnlisted Content-Type (" + str2 + "), check settings.");
                            request = null;
                            return;
                        }
                        Match match = new Regex(@"\d+").Match(this.MIMETypes, index);
                        int num3 = int.Parse(match.Value) * 0x400;
                        int num4 = int.Parse(match.NextMatch().Value) * 0x400;
                        if ((num3 < num4) && ((response.ContentLength < num3) || (response.ContentLength > num4)))
                        {
                            this.LogError(uri.AbsoluteUri, string.Concat(new object[] { str, "\r\nContentLength limit error (", response.ContentLength, ")" }));
                            request = null;
                            return;
                        }
                    }
                    string[] strArray = new string[] { ".gif", ".jpg", ".css", ".zip", ".exe" };
                    bool flag = true;
                    foreach (string str3 in strArray)
                    {
                        if (uri.AbsoluteUri.ToLower().EndsWith(str3))
                        {
                            flag = false;
                            break;
                        }
                    }
                    foreach (string str3 in this.ExcludeFiles)
                    {
                        if ((str3.Trim().Length > 0) && uri.AbsoluteUri.ToLower().EndsWith(str3))
                        {
                            flag = false;
                            break;
                        }
                    }
                    string strBody = uri.ToString();
                    if (this.Compared(uri.LocalPath.Substring(uri.LocalPath.LastIndexOf('.') + 1).ToLower()) && (uri.ToString().Substring(uri.ToString().Length - 1, 1) != "/"))
                    {
                        this.LogError("丢弃--非网页文件", strBody);
                    }
                    else
                    {
                        int num5;
                        UriKind absolute = UriKind.Absolute;
                        if (!string.IsNullOrEmpty(strBody) && Uri.IsWellFormedUriString(strBody, absolute))
                        {
                            string page = GetPage(strBody);
                            Stopwatch stopwatch = new Stopwatch();
                            stopwatch.Start();
                            Html html = new Html {
                                Web = page,
                                Url = strBody
                            };
                            CommonAnalyze analyze = new CommonAnalyze();
                            analyze.LoadHtml(html);
                            Net.LikeShow.ContentAnalyze.Document result = analyze.GetResult();
                            stopwatch.Stop();
                            string bt = result.Title.Replace("[(title)]", "");
                            switch (bt)
                            {
                                case null:
                                case "":
                                    bt = result.Doc.Substring(20).ToString();
                                    break;
                            }
                            if ((result.Doc == null) || (result.Doc == ""))
                            {
                                this.LogError("丢弃--空内容或非内空页", strBody);
                            }
                            else
                            {
                                Lucene.Net.Documents.Document document3;
                                string str7 = result.Doc + bt;
                                if (this.cgcount >= 10)
                                {
                                    string keywords = this.MD5string(result.Doc.ToString());
                                    string keyWordsSplitBySpace = "";
                                    IndexSearcher searcher = new IndexSearcher(this.path);
                                    keyWordsSplitBySpace = GetKeyWordsSplitBySpace(keywords, new KTDictSegTokenizer());
                                    Query query = new QueryParser("J_md5_bai", new KTDictSegAnalyzer(true)).Parse(keyWordsSplitBySpace);
                                    if (searcher.Search(query).Doc(0).Get("J_md5_bai") == keywords)
                                    {
                                        this.LogError("排除--重复", strBody);
                                    }
                                    else
                                    {
                                        this.cgcount++;
                                        this.LogUri(bt, "引索完成");
                                        document3 = new Lucene.Net.Documents.Document();
                                        document3.Add(new Field("分类", this.page_py, Field.Store.YES, Field.Index.TOKENIZED));
                                        document3.Add(new Field("J_title_bai", bt, Field.Store.YES, Field.Index.TOKENIZED));
                                        document3.Add(new Field("J_msgContent_bai", str7, Field.Store.YES, Field.Index.TOKENIZED));
                                        document3.Add(new Field("J_SiteType_bai", result.SiteType.ToString(), Field.Store.YES, Field.Index.NO));
                                        document3.Add(new Field("J_URL_bai", strBody, Field.Store.YES, Field.Index.NO));
                                        document3.Add(new Field("J_addtime_bai", DateTime.Now.ToShortDateString(), Field.Store.YES, Field.Index.NO));
                                        document3.Add(new Field("J_md5_bai", this.MD5string(result.Doc.ToString()), Field.Store.YES, Field.Index.TOKENIZED));
                                        this.writer.AddDocument(document3);
                                    }
                                }
                                else
                                {
                                    this.cgcount++;
                                    this.LogUri(bt, "引索完成");
                                    document3 = new Lucene.Net.Documents.Document();
                                    document3.Add(new Field("分类", this.page_py, Field.Store.YES, Field.Index.TOKENIZED));
                                    document3.Add(new Field("J_title_bai", bt, Field.Store.YES, Field.Index.TOKENIZED));
                                    document3.Add(new Field("J_msgContent_bai", str7, Field.Store.YES, Field.Index.TOKENIZED));
                                    document3.Add(new Field("J_SiteType_bai", result.SiteType.ToString(), Field.Store.YES, Field.Index.NO));
                                    document3.Add(new Field("J_URL_bai", strBody, Field.Store.YES, Field.Index.NO));
                                    document3.Add(new Field("J_addtime_bai", DateTime.Now.ToShortDateString(), Field.Store.YES, Field.Index.NO));
                                    document3.Add(new Field("J_md5_bai", this.MD5string(result.Doc.ToString()), Field.Store.YES, Field.Index.TOKENIZED));
                                    this.writer.AddDocument(document3);
                                }
                            }
                        }
                        item.SubItems[2].Text = "正在下载";
                        item.ForeColor = System.Drawing.Color.Black;
                        string input = "";
                        byte[] buffer = new byte[0x2800];
                        int nNum = 0;
                        while ((num5 = response.socket.Receive(buffer, 0, 0x2800, SocketFlags.None)) > 0)
                        {
                            nNum += num5;
                            if (flag)
                            {
                                input = input + Encoding.ASCII.GetString(buffer, 0, num5);
                            }
                            item.SubItems[4].Text = this.Commas(nNum);
                            if (response.ContentLength > 0)
                            {
                                item.SubItems[5].Text = '%' + ((100 - (((response.ContentLength - nNum) * 100) / response.ContentLength))).ToString();
                            }
                            if ((response.KeepAlive && (nNum >= response.ContentLength)) && (response.ContentLength > 0))
                            {
                                break;
                            }
                        }
                        if (response.KeepAlive)
                        {
                            str = str + "Connection kept alive to be used in subpages.\r\n";
                        }
                        else
                        {
                            response.Close();
                            str = str + "Connection closed.\r\n";
                        }
                        this.FileCount++;
                        this.ByteCount += nNum;
                        if ((this.ThreadsRunning && flag) && (uri.Depth < this.WebDepth))
                        {
                            str = str + "\r\nParsing page ...\r\n";
                            string pattern = "(href|HREF|src|SRC)[ ]*=[ ]*[\"'][^\"'#>]+[\"']";
                            MatchCollection matchs = new Regex(pattern).Matches(input);
                            obj2 = str;
                            str = string.Concat(new object[] { obj2, "Found: ", matchs.Count, " ref(s)\r\n" });
                            this.URLCount += matchs.Count;
                            foreach (Match match in matchs)
                            {
                                pattern = match.Value.Substring(match.Value.IndexOf('=') + 1).Trim(new char[] { '"', '\'', '#', ' ', '>' });
                                try
                                {
                                    if (!(((pattern.IndexOf("..") == -1) && !pattern.StartsWith("/")) && pattern.StartsWith("http://")))
                                    {
                                        pattern = new Uri(uri, pattern).AbsoluteUri;
                                    }
                                    this.Normalize(ref pattern);
                                    MyUri uri2 = new MyUri(pattern);
                                    if ((((uri2.Scheme != Uri.UriSchemeHttp) && (uri2.Scheme != Uri.UriSchemeHttps)) || ((uri2.Host.Split(new char[] { '.' })[1] != this.urllhost[1]) && this.KeepSameServer)) || !this.Compared_jpg(uri2.LocalPath.Substring(uri2.LocalPath.LastIndexOf('.') + 1).ToLower()))
                                    {
                                        continue;
                                    }
                                    Global.URL = uri2.ToString();
                                    if ((Global.BXBH != "") && (Redspider_link.bxbh() == 2))
                                    {
                                        continue;
                                    }
                                    uri2.Depth = uri.Depth + 1;
                                    if (this.EnqueueUri(uri2, true))
                                    {
                                        str = str + uri2.AbsoluteUri + "\r\n";
                                    }
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.LogError(uri.AbsoluteUri, str + exception.Message);
                request = null;
            }
            finally
            {
                this.EraseItem(item);
            }
        }

        private void PauseParsing()
        {
            if (this.threadParse.ThreadState == System.Threading.ThreadState.Running)
            {
                this.threadParse.Suspend();
            }
            for (int i = 0; i < this.ThreadCount; i++)
            {
                Thread thread = this.threadsRun[i];
                if (thread.ThreadState == System.Threading.ThreadState.Running)
                {
                    thread.Suspend();
                }
            }
            this.toolBarButtonContinue.Enabled = true;
            this.toolBarButtonPause.Enabled = false;
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

        public void redspider_column()
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
            this.AddTree(0, null);
        }

        private void RunParser()
        {
            this.ThreadsRunning = true;
            try
            {
                long num = long.Parse(Global.COLUMN_ID.ToString());
                OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
                OleDbCommand command = new OleDbCommand("SELECT * FROM Web_URL where id=" + num, connection);
                connection.Open();
                OleDbDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                string path = "";
                while (reader.Read())
                {
                    path = reader["web_url"].ToString().Replace("\r", "");
                    Global.BM = reader["bm"].ToString();
                    Global.BXBH = reader["bxbh"].ToString();
                    Global.BDBH = reader["bdbh"].ToString();
                    Global.WEB_COLUMN_ID = reader["id"].ToString();
                    Global.WEB_COLUMN_NAME = reader["column_name"].ToString();
                    this.page_py = reader["column_name"].ToString();
                    Global.WEB_PARENT_ID = reader["parentid"].ToString();
                    Global.WEB_CODE = reader["code"].ToString();
                    Global.URL_BIANHAO = reader["id"].ToString();
                    this.zl_yes = reader["class"].ToString();
                    Global.ONEURL = path;
                    this.urllhost = path.ToString().Split(new char[] { '.' });
                    if (Directory.Exists(path))
                    {
                        this.ParseFolder(path, 0);
                    }
                    else
                    {
                        if (!System.IO.File.Exists(path))
                        {
                            this.Normalize(ref path);
                            this.comboBoxWeb.Text = path;
                        }
                        MyUri uri = new MyUri(path);
                        this.EnqueueUri(uri, false);
                    }
                }
            }
            catch (Exception exception)
            {
                this.LogError(exception.Message, exception.Message);
                return;
            }
            this.toolBarButtonContinue.Enabled = false;
        }

        [DllImport("kernel32.dll")]
        public static extern bool SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);
        private void ShowSettings(int nSelectedIndex)
        {
            if (new SettingsForm { SelectedIndex = nSelectedIndex }.ShowDialog(this) == DialogResult.OK)
            {
                this.ThreadCount = Settings.GetValue("Threads count", 10);
                this.InitValues();
            }
        }

        public void Skin()
        {
            OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
            string cmdText = "SELECT * FROM RedSpider_Skin where Skin_Default='是'";
            OleDbCommand selectCommand = new OleDbCommand(cmdText, connection);
            OleDbDataAdapter adapter = new OleDbDataAdapter(selectCommand);
            DataSet dataSet = new DataSet();
            dataSet.Clear();
            adapter.Fill(dataSet, "RedSpider_Skin ");
            this.skin = dataSet.Tables["RedSpider_Skin "].Rows[0]["Skin_link"].ToString();
            this.skinEngine1.SkinFile = @"Skin\" + this.skin + ".ssk";
        }

        private void StartParsing()
        {
            this.path = this.seva_webindex.Text;
            if (!Directory.Exists(this.path))
            {
                Directory.CreateDirectory(this.path);
            }
            try
            {
                this.writer = new IndexWriter(this.path, new KTDictSegAnalyzer(), false);
                this.timer1.Enabled = true;
            }
            catch (Exception)
            {
                this.writer = new IndexWriter(this.path, new KTDictSegAnalyzer(), true);
                this.timer1.Enabled = true;
            }
            if (this.comboBoxWeb.FindStringExact(this.comboBoxWeb.Text) < 0)
            {
                this.comboBoxWeb.Items.Insert(0, this.comboBoxWeb.Text);
            }
            if ((this.threadParse == null) || (this.threadParse.ThreadState != System.Threading.ThreadState.Suspended))
            {
                this.urlStorage.Clear();
                this.threadParse = new Thread(new ThreadStart(this.RunParser));
                this.threadParse.Start();
            }
            this.ThreadCount = Settings.GetValue("Threads count", 10);
            this.toolBarButtonContinue.Enabled = false;
            this.toolBarButtonPause.Enabled = true;
        }

        private void StopParsing()
        {
            this.queueURLS.Clear();
            this.ThreadsRunning = false;
            Thread.Sleep(500);
            try
            {
                if (this.threadParse.ThreadState == System.Threading.ThreadState.Suspended)
                {
                    this.threadParse.Resume();
                }
                this.threadParse.Abort();
            }
            catch (Exception)
            {
            }
            Monitor.Enter(this.listViewThreads);
            for (int i = 0; i < this.ThreadCount; i++)
            {
                try
                {
                    Thread thread = this.threadsRun[i];
                    ListViewItem item = this.listViewThreads.Items[int.Parse(thread.Name)];
                    item.SubItems[2].Text = "停止";
                    item.BackColor = System.Drawing.Color.WhiteSmoke;
                    item.ImageIndex = 3;
                    if ((thread != null) && thread.IsAlive)
                    {
                        if (thread.ThreadState == System.Threading.ThreadState.Suspended)
                        {
                            thread.Resume();
                        }
                        thread.Abort();
                    }
                }
                catch (Exception)
                {
                }
            }
            Monitor.Exit(this.listViewThreads);
            this.toolBarButtonContinue.Enabled = true;
            this.toolBarButtonPause.Enabled = false;
            this.queueURLS.Clear();
            this.urlStorage.Clear();
        }

        private void StopParsingExit()
        {
            this.queueURLS.Clear();
            this.ThreadsRunning = false;
            Thread.Sleep(500);
            try
            {
                if (this.threadParse.ThreadState == System.Threading.ThreadState.Suspended)
                {
                    this.threadParse.Resume();
                }
                this.threadParse.Abort();
            }
            catch (Exception)
            {
            }
            Monitor.Enter(this.listViewThreads);
            for (int i = 0; i < this.ThreadCount; i++)
            {
                try
                {
                    Thread thread = this.threadsRun[i];
                    ListViewItem item = this.listViewThreads.Items[int.Parse(thread.Name)];
                    item.SubItems[2].Text = "停止";
                    item.BackColor = System.Drawing.Color.WhiteSmoke;
                    item.ImageIndex = 3;
                    if ((thread != null) && thread.IsAlive)
                    {
                        if (thread.ThreadState == System.Threading.ThreadState.Suspended)
                        {
                            thread.Resume();
                        }
                        thread.Abort();
                    }
                }
                catch (Exception)
                {
                }
            }
            Monitor.Exit(this.listViewThreads);
            this.toolBarButtonContinue.Enabled = true;
            this.toolBarButtonPause.Enabled = false;
            this.queueURLS.Clear();
            this.urlStorage.Clear();
            this.CloseCtiServer();
            this.writer.Optimize();
            this.writer.Close();
        }

        private void tabControlRightView_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void ThreadRunFunction()
        {
            MyWebRequest request = null;
            while (this.ThreadsRunning && (int.Parse(Thread.CurrentThread.Name) < this.ThreadCount))
            {
                MyUri uri = this.DequeueUri();
                if (uri != null)
                {
                    if (this.SleepConnectTime > 0)
                    {
                        Thread.Sleep((int) (this.SleepConnectTime * 0x3e8));
                    }
                    this.ParseUri(uri, ref request);
                }
                else
                {
                    Thread.Sleep((int) (this.SleepFetchTime * 0x3e8));
                }
            }
            Monitor.Enter(this.listViewThreads);
            try
            {
                ListViewItem item = this.listViewThreads.Items[int.Parse(Thread.CurrentThread.Name)];
                if (!this.ThreadsRunning)
                {
                    item.SubItems[2].Text = "停止";
                }
                item.ImageIndex = 0;
            }
            catch (Exception)
            {
            }
            Monitor.Exit(this.listViewThreads);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.tj();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            this.label19.Text = DateTime.Now.ToString();
            if (DateTime.Now.ToString() == this.label18.Text)
            {
                this.timer1.Enabled = true;
                if (this.ssjk)
                {
                    this.M_jk_URL();
                }
                this.label18.Text = this.dateTimePicker3.Value.AddDays((double) int.Parse(this.comboBox1.Text)).ToShortDateString() + " " + this.comboBox2.Text + ":00:00";
                this.dateTimePicker2.Value = this.dateTimePicker3.Value.AddDays((double) int.Parse(this.comboBox1.Text));
                Mission_Time time = new Mission_Time();
                try
                {
                    time.M_timeup(this.label18.Text, this.checkBox1.Checked.ToString());
                }
                catch (Exception)
                {
                }
            }
        }

        private void timerConnectionInfo_Tick(object sender, EventArgs e)
        {
            this.ConnectionInfo();
        }

        private void timerMem_Tick(object sender, EventArgs e)
        {
            this.FreeMemory = this.ramCounter.NextValue();
            this.CPUUsage = (int) this.cpuCounter.NextValue();
        }

        public void tj()
        {
            FlushMemory();
            string str = "";
            for (int i = 0; i < this.ThreadCount; i++)
            {
                str = str + this.threadsRun[i].ThreadState.ToString();
            }
            switch (str.Replace("WaitSleepJoin", "").Replace("Stopped", ""))
            {
                case "":
                case "Unstarted":
                    this.j++;
                    break;
            }
            if (this.j == 2)
            {
                OleDbConnection connection;
                string str2;
                OleDbCommand command;
                if (this.ramCounter.NextValue() < 200f)
                {
                    connection = new OleDbConnection(CONN_ACCESS.ConnString);
                    str2 = "update Mission_Time SET [ramCounter]='是'";
                    command = new OleDbCommand(str2, connection);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    string fileName = Process.GetCurrentProcess().MainModule.FileName;
                    new Process { StartInfo = { FileName = fileName, WorkingDirectory = Application.ExecutablePath } }.Start();
                    Application.Exit();
                }
                if (this.ssjk)
                {
                    if (this.dscjwzgs < this.dtSource.Rows.Count)
                    {
                        FlushMemory();
                        this.ssjk = true;
                        Global.COLUMN_ID = this.dss.Tables["Web_URL"].Rows[this.dscjwzgs]["id"].ToString();
                        this.threadParse = new Thread(new ThreadStart(this.StartParsing));
                        this.threadParse.Start();
                        this.dscjwzgs++;
                        Application.DoEvents();
                        this.textBox2.Text = this.dscjwzgs.ToString();
                        this.textBox3.Text = this.dss.Tables["Web_URL"].Rows[this.dscjwzgs]["column_name"].ToString();
                        this.textBox4.Text = this.dss.Tables["Web_URL"].Rows[this.dscjwzgs]["web_url"].ToString();
                        connection = new OleDbConnection(CONN_ACCESS.ConnString);
                        command = new OleDbCommand("update Mission_Time SET [wzgs]='" + this.dscjwzgs + "'", connection);
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                    else
                    {
                        this.writer.Optimize();
                        this.writer.Close();
                        this.j = 0;
                        this.timer1.Enabled = false;
                        this.ssjk = false;
                        connection = new OleDbConnection(CONN_ACCESS.ConnString);
                        str2 = "update Mission_Time SET [wzgs]='1'";
                        command = new OleDbCommand(str2, connection);
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                else
                {
                    this.writer.Optimize();
                    this.writer.Close();
                    this.j = 0;
                    this.timer1.Enabled = false;
                    this.ssjk = false;
                }
            }
        }

        private void toolBarButton5_Cilck(object sender, EventArgs e)
        {
        }

        private void toolBarMain_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            if (e.Button == this.toolBarButtonContinue)
            {
                this.ContinueParsing();
            }
            else if (e.Button == this.toolBarButtonPause)
            {
                this.PauseParsing();
            }
            else if (e.Button == this.toolBarButtonStop)
            {
                this.StopParsing();
            }
            else if (e.Button == this.toolBarButtonDeleteAll)
            {
                this.DeleteAllItems();
            }
            else if (e.Button == this.toolBarButtonSettings)
            {
                this.ShowSettings(-1);
            }
            else if (e.Button == this.toolBarButton1)
            {
                new Add_Column(this).ShowDialog();
            }
            else if (e.Button == this.toolBarButton5)
            {
                new Rules_panel(this).ShowDialog();
            }
            else if (e.Button == this.toolBarButton6)
            {
                new index_hb().ShowDialog();
            }
            else if(e.Button == this.toolBarButton7)
            {
                WEB_XML_Manager wx = new WEB_XML_Manager(this);
                wx.Show();
            }
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            string str = "RedSpider_DiamondGreen";
            new WEB_XML_Source().Default_Skin(str);
            this.Skin();
        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            string str = "RedSpider_MP10";
            new WEB_XML_Source().Default_Skin(str);
            this.Skin();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            this.cgcount = 0;
            if (this.seva_webindex.Text == "")
            {
                MessageBox.Show("请 选 择 指 定 引 索 存 放 的 目 录　！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else if ((Global.COLUMN_ID == "") || (Global.COLUMN_ID == null))
            {
                MessageBox.Show("请　选　择　要　抓　取　数　据　的　节　点　！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                try
                {
                    long num2 = long.Parse(Global.COLUMN_ID.ToString());
                    OleDbConnection connection = new OleDbConnection(CONN_ACCESS.ConnString);
                    OleDbCommand command = new OleDbCommand("SELECT * FROM Web_URL where id=" + num2, connection);
                    connection.Open();
                    if (!command.ExecuteReader(CommandBehavior.CloseConnection).Read())
                    {
                        MessageBox.Show("当　前　节　点　无　任　何　数　据　！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        return;
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("请　选　择　要　抓　取　数　据　的　节　点！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
                this.ssjk = false;
                this.timer1.Enabled = true;
                this.threadParse = new Thread(new ThreadStart(this.StartParsing));
                this.threadParse.Start();
                new Mission_Time().up_index_file(this.seva_webindex.Text);
            }
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            string str = "RedSpider_Calmness";
            new WEB_XML_Source().Default_Skin(str);
            this.Skin();
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            string str = "RedSpider_diamondblue";
            new WEB_XML_Source().Default_Skin(str);
            this.Skin();
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Global.COLUMN_NAME = this.treeView1.SelectedNode.Text;
            Global.COLUMN_ID = this.treeView1.SelectedNode.Tag.ToString();
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

        private void wEB栏目XML数据源设定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        public void wyfx(string urll)
        {
            string str = urll;
            UriKind absolute = UriKind.Absolute;
            if (!string.IsNullOrEmpty(str) && Uri.IsWellFormedUriString(str, absolute))
            {
                string page = GetPage(str);
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                Html html = new Html {
                    Web = page,
                    Url = str
                };
                CommonAnalyze analyze = new CommonAnalyze();
                analyze.LoadHtml(html);
                Net.LikeShow.ContentAnalyze.Document result = analyze.GetResult();
                stopwatch.Stop();
                string bt = result.Title.Replace("[(title)]", "");
                if ((result.Doc == null) || (result.Doc == ""))
                {
                    this.LogUri(bt, "丢弃--空内容");
                }
                else if ((bt == null) || (bt == ""))
                {
                    bt = result.Doc.Substring(20).ToString();
                }
                else
                {
                    this.LogUri(bt, "引索完成");
                    Lucene.Net.Documents.Document doc = new Lucene.Net.Documents.Document();
                    doc.Add(new Field("J_title_bai", bt, Field.Store.YES, Field.Index.TOKENIZED));
                    doc.Add(new Field("J_msgContent_bai", result.Doc, Field.Store.YES, Field.Index.TOKENIZED));
                    doc.Add(new Field("J_URL_bai", DateTime.Now.ToShortDateString(), Field.Store.YES, Field.Index.NO));
                    this.writer.AddDocument(doc);
                }
            }
            Console.Read();
        }

        private void 风格八ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string str = "RedSpider_steelblue";
            new WEB_XML_Source().Default_Skin(str);
            this.Skin();
        }

        private void 风格六ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string str = "RedSpider_onecyan";
            new WEB_XML_Source().Default_Skin(str);
            this.Skin();
        }

        private void 风格七ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string str = "RedSPider_page_color2";
            new WEB_XML_Source().Default_Skin(str);
            this.Skin();
        }

        private void 风格五ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string str = "RedSpider_emerald";
            new WEB_XML_Source().Default_Skin(str);
            this.Skin();
        }

        private void 开始抓取所有分类ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.timer1.Enabled = true;
            this.ssjk = true;
            if (this.ssjk)
            {
                this.M_jk_URL();
            }
        }

        private void 连接ConnectionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ShowSettings(2);
        }

        private void 软件作者AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }

        private void 设置SettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ShowSettings(-1);
        }

        private void 输出OutputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ShowSettings(1);
        }

        private void 添加根节点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Add_Column(this).ShowDialog();
        }

        private void 添加项目则规ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Global.WEBXML = true;
            new Rules_panel(this).ShowDialog();
        }

        private void 添加子节点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Global.PARENT_ID == "0")
            {
                MessageBox.Show("不 能 将 数 据 加 入 根 频 道 中, 请 重 新 选 择 所 属 子 分 类 ！", "军长提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                Global.MAINTREEVIEW = false;
                Global.WEBXML = false;
                new Rules_panel(this).ShowDialog();
            }
        }

        private void 退出系统ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.StopParsingExit();
        }

        private void 退出系统ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.StopParsingExit();
        }

        private void 原始经典ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string str = "RedSpider520";
            new WEB_XML_Source().Default_Skin(str);
            this.Skin();
            MessageBox.Show("重新启动程序后还原原始风格");
        }

        private void 最大化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            base.ShowInTaskbar = true;
            base.WindowState = FormWindowState.Normal;
            base.Visible = true;
        }

        private void 最小化程序ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            base.Visible = false;
        }

        private void 最小化程序ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            base.Visible = false;
        }

        private bool AllMIMETypes
        {
            get
            {
                return this.bAllMIMETypes;
            }
            set
            {
                this.bAllMIMETypes = value;
            }
        }

        private int ByteCount
        {
            get
            {
                return this.nByteCount;
            }
            set
            {
                this.nByteCount = value;
                this.statusBarPanelByteCount.Text = this.Commas((this.nByteCount / 0x400) + 1) + " KB";
            }
        }

        private int CPUUsage
        {
            get
            {
                return this.nCPUUsage;
            }
            set
            {
                this.nCPUUsage = value;
                this.statusBarPanelCPU.Text = "CPU占用 " + this.nCPUUsage + "%";
                Icon icon = Icon.FromHandle(((Bitmap) this.imageListPercentage.Images[value / 10]).GetHicon());
                this.statusBarPanelCPU.Icon = icon;
            }
        }

        private string Downloadfolder
        {
            get
            {
                return this.strDownloadfolder;
            }
            set
            {
                this.strDownloadfolder = value;
                this.strDownloadfolder = this.strDownloadfolder.TrimEnd(new char[] { '\\' });
            }
        }

        private int ErrorCount
        {
            get
            {
                return this.nErrorCount;
            }
            set
            {
                this.nErrorCount = value;
                this.statusBarPanelErrors.Text = "错误 " + this.Commas(this.nErrorCount);
            }
        }

        private string[] ExcludeFiles
        {
            get
            {
                return this.strExcludeFiles;
            }
            set
            {
                this.strExcludeFiles = value;
            }
        }

        private string[] ExcludeHosts
        {
            get
            {
                return this.strExcludeHosts;
            }
            set
            {
                this.strExcludeHosts = value;
            }
        }

        private string[] ExcludeWords
        {
            get
            {
                return this.strExcludeWords;
            }
            set
            {
                this.strExcludeWords = value;
            }
        }

        private int FileCount
        {
            get
            {
                return this.nFileCount;
            }
            set
            {
                this.nFileCount = value;
                this.statusBarPanelFiles.Text = this.Commas(this.nFileCount) + "成功";
            }
        }

        private float FreeMemory
        {
            get
            {
                return this.nFreeMemory;
            }
            set
            {
                this.nFreeMemory = value;
                this.statusBarPanelMem.Text = this.nFreeMemory + " Mb内存可用";
            }
        }

        private bool KeepAlive
        {
            get
            {
                return this.bKeepAlive;
            }
            set
            {
                this.bKeepAlive = value;
            }
        }

        private bool KeepSameServer
        {
            get
            {
                return this.bKeepSameServer;
            }
            set
            {
                this.bKeepSameServer = value;
            }
        }

        private int LastRequestCount
        {
            get
            {
                return this.nLastRequestCount;
            }
            set
            {
                this.nLastRequestCount = value;
            }
        }

        private string MIMETypes
        {
            get
            {
                return this.strMIMETypes;
            }
            set
            {
                this.strMIMETypes = value;
            }
        }

        private int RequestTimeout
        {
            get
            {
                return this.nRequestTimeout;
            }
            set
            {
                this.nRequestTimeout = value;
            }
        }

        private int SleepConnectTime
        {
            get
            {
                return this.nSleepConnectTime;
            }
            set
            {
                this.nSleepConnectTime = value;
            }
        }

        private int SleepFetchTime
        {
            get
            {
                return this.nSleepFetchTime;
            }
            set
            {
                this.nSleepFetchTime = value;
            }
        }

        private Encoding TextEncoding
        {
            get
            {
                return this.encoding;
            }
            set
            {
                this.encoding = value;
            }
        }

        private int ThreadCount
        {
            get
            {
                return this.nThreadCount;
            }
            set
            {
                Monitor.Enter(this.listViewThreads);
                try
                {
                    for (int i = 0; i < value; i++)
                    {
                        ListViewItem item;
                        if ((this.threadsRun[i] == null) || (this.threadsRun[i].ThreadState != System.Threading.ThreadState.Suspended))
                        {
                            this.threadsRun[i] = new Thread(new ThreadStart(this.ThreadRunFunction));
                            this.threadsRun[i].Name = i.ToString();
                            this.threadsRun[i].Start();
                            if (i == this.listViewThreads.Items.Count)
                            {
                                int num2 = i + 1;
                                item = this.listViewThreads.Items.Add(num2.ToString(), 0);
                                string[] items = new string[] { "", "", "", "0", "0%" };
                                item.SubItems.AddRange(items);
                            }
                        }
                        else if (this.threadsRun[i].ThreadState == System.Threading.ThreadState.Suspended)
                        {
                            item = this.listViewThreads.Items[i];
                            item.ImageIndex = 1;
                            item.SubItems[2].Text = "恢复工作";
                            this.threadsRun[i].Resume();
                        }
                    }
                    this.nThreadCount = value;
                }
                catch (Exception)
                {
                }
                Monitor.Exit(this.listViewThreads);
            }
        }

        private int URLCount
        {
            get
            {
                return this.nURLCount;
            }
            set
            {
                this.nURLCount = value;
                this.statusBarPanelURLs.Text = "发现 " + this.Commas(this.nURLCount) + " URL";
            }
        }

        private int WebDepth
        {
            get
            {
                return this.nWebDepth;
            }
            set
            {
                this.nWebDepth = value;
            }
        }

        public class MyUri : Uri
        {
            public int Depth;

            public MyUri(string uriString) : base(uriString)
            {
            }
        }

        public class MyWebRequest
        {
            public string Header;
            public WebHeaderCollection Headers = new WebHeaderCollection();
            public bool KeepAlive;
            public string Method;
            public Uri RequestUri;
            public CrawlerForm.MyWebResponse response;
            public int Timeout;

            public MyWebRequest(Uri uri, bool bKeepAlive)
            {
                this.RequestUri = uri;
                this.Headers["Host"] = uri.Host;
                this.KeepAlive = bKeepAlive;
                if (this.KeepAlive)
                {
                    this.Headers["Connection"] = "Keep-Alive";
                }
                this.Method = "GET";
            }

            public static CrawlerForm.MyWebRequest Create(Uri uri, CrawlerForm.MyWebRequest AliveRequest, bool bKeepAlive)
            {
                if ((((bKeepAlive && (AliveRequest != null)) && ((AliveRequest.response != null) && AliveRequest.response.KeepAlive)) && AliveRequest.response.socket.Connected) && (AliveRequest.RequestUri.Host == uri.Host))
                {
                    AliveRequest.RequestUri = uri;
                    return AliveRequest;
                }
                return new CrawlerForm.MyWebRequest(uri, bKeepAlive);
            }

            public CrawlerForm.MyWebResponse GetResponse()
            {
                if (!(((this.response != null) && (this.response.socket != null)) && this.response.socket.Connected))
                {
                    this.response = new CrawlerForm.MyWebResponse();
                    this.response.Connect(this);
                    this.response.SetTimeout(this.Timeout);
                }
                this.response.SendRequest(this);
                this.response.ReceiveHeader();
                return this.response;
            }
        }

        public class MyWebResponse
        {
            public int ContentLength;
            public string ContentType;
            public string Header;
            public WebHeaderCollection Headers;
            public bool KeepAlive;
            public Uri ResponseUri;
            public Socket socket;

            public void Close()
            {
                this.socket.Close();
            }

            public void Connect(CrawlerForm.MyWebRequest request)
            {
                this.ResponseUri = request.RequestUri;
                this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint remoteEP = new IPEndPoint(Dns.Resolve(this.ResponseUri.Host).AddressList[0], this.ResponseUri.Port);
                this.socket.Connect(remoteEP);
            }

            public void ReceiveHeader()
            {
                this.Header = "";
                this.Headers = new WebHeaderCollection();
                byte[] buffer = new byte[10];
                while (this.socket.Receive(buffer, 0, 1, SocketFlags.None) > 0)
                {
                    this.Header = this.Header + Encoding.ASCII.GetString(buffer, 0, 1);
                    if ((buffer[0] == 10) && this.Header.EndsWith("\r\n\r\n"))
                    {
                        break;
                    }
                }
                MatchCollection matchs = new Regex("[^\r\n]+").Matches(this.Header.TrimEnd(new char[] { '\r', '\n' }));
                for (int i = 1; i < matchs.Count; i++)
                {
                    string[] strArray = matchs[i].Value.Split(new char[] { ':' }, 2);
                    if (strArray.Length > 0)
                    {
                        this.Headers[strArray[0].Trim()] = strArray[1].Trim();
                    }
                }
                if (((matchs.Count > 0) && ((matchs[0].Value.IndexOf(" 302 ") != -1) || (matchs[0].Value.IndexOf(" 301 ") != -1))) && (this.Headers["Location"] != null))
                {
                    try
                    {
                        this.ResponseUri = new Uri(this.Headers["Location"]);
                    }
                    catch
                    {
                        this.ResponseUri = new Uri(this.ResponseUri, this.Headers["Location"]);
                    }
                }
                this.ContentType = this.Headers["Content-Type"];
                if (this.Headers["Content-Length"] != null)
                {
                    this.ContentLength = int.Parse(this.Headers["Content-Length"]);
                }
                this.KeepAlive = ((this.Headers["Connection"] != null) && (this.Headers["Connection"].ToLower() == "keep-alive")) || ((this.Headers["Proxy-Connection"] != null) && (this.Headers["Proxy-Connection"].ToLower() == "keep-alive"));
            }

            public void SendRequest(CrawlerForm.MyWebRequest request)
            {
                this.ResponseUri = request.RequestUri;
                request.Header = string.Concat(new object[] { request.Method, " ", this.ResponseUri.PathAndQuery, " HTTP/1.0\r\n", request.Headers });
                this.socket.Send(Encoding.ASCII.GetBytes(request.Header));
            }

            public void SetTimeout(int Timeout)
            {
                this.socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, (int) (Timeout * 0x3e8));
                this.socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, (int) (Timeout * 0x3e8));
            }
        }
    }
}

