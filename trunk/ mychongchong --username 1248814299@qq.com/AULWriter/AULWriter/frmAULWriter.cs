namespace AULWriter
{
    using AutoUpdate;
    using SharpSvn;
    using SkyMap.Net.Core;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml;
    
    public class frmAULWriter : Form
    {
        private ToolStripMenuItem btDel;
        private Button btnDest;
        private Button btnExpt;
        private Button btnProduce;
        private Button btnSrc;
        private Button btSelectPath;
        private Button btUpload;
        private Button button1;
        private Button button2;
        private CheckBox checkBox1;
        private ColumnHeader chFileName;
        private ColumnHeader chFolder;
        private ColumnHeader chUrl;
        private IContainer components;
        private List<string[]> comps = new List<string[]>();
        private ContextMenuStrip contextMenuStrip1;
        private FolderBrowserDialog fbComponent;
        private GroupBox groupBox1;
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private ListView lvComList;
        private OpenFileDialog ofdExpt;
        private OpenFileDialog ofdSrc;
        private Panel panel1;
        private ProgressBar prbProd;
        private SaveFileDialog sfdDest;
        private TabControl tabControl1;
        private TabPage tbpBase;
        private TabPage tbpUpload;
        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox txtComponent;
        private TextBox txtComponentUrl;
        private TextBox txtDest;
        private TextBox txtExpt;
        private TextBox txtFtpLocation;
        private ComboBox txtHost;
        private TextBox txtLocation;
        private TextBox txtPwd;
        private TextBox txtSrc;
        private TextBox txtSubUrl;
        private TextBox txtUrl;
        private TextBox txtUser;
        private Button bt_mr;
        private List<string> waitUploads = new List<string>();

        public frmAULWriter()
        {
            this.InitializeComponent();
        }

        private void btAddComponent_Click(object sender, EventArgs e)
        {
            string[] items = new string[] { this.txtComponent.Text.Trim(), this.txtLocation.Text.Trim(), string.Format("{0}{1}", this.txtComponentUrl.Text.Trim(), this.txtSubUrl.Text.Trim()) };
            this.lvComList.Items.Add(new ListViewItem(items));
            this.comps.Add(items);
        }

        private void btDel_Click(object sender, EventArgs e)
        {
            for (int i = this.lvComList.Items.Count - 1; i >= 0; i--)
            {
                if (this.lvComList.Items[i].Selected)
                {
                    this.comps.RemoveAt(i);
                    this.lvComList.Items.RemoveAt(i);
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            base.Close();
            base.Dispose();
        }

        private void btnProduce_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(this.WriterAUList));
            if (this.btnProduce.Text == "生成(&G)")
            {
                if (!File.Exists(this.txtSrc.Text))
                {
                    MessageBox.Show(this, "请选择主入口程序!", "AutoUpdater", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    this.btnSrc_Click(sender, e);
                }
                if (this.txtUrl.Text.Trim().Length == 0)
                {
                    MessageBox.Show(this, "请输入自动更新网址!", "AutoUpdater", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    this.txtUrl.Focus();
                }
                else
                {
                    if (this.txtDest.Text.Trim() == string.Empty)
                    {
                        MessageBox.Show(this, "请选择AutoUpdaterList保存位置!", "AutoUpdater", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        this.btnSearDes_Click(sender, e);
                    }
                    if (this.lvComList.Items.Count == 0)
                    {
                        MessageBox.Show(this, "还没有添加任何组件!", "AutoUpdater", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    else
                    {
                        thread.IsBackground = true;
                        thread.Start();
                        this.btnProduce.Text = "停止(&S)";
                    }
                }
            }
            else
            {
                Application.DoEvents();
                if (MessageBox.Show(this, "是否停止文件生成更新文件?", "AutoUpdater", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (thread.IsAlive)
                    {
                        thread.Abort();
                        thread.Join();
                    }
                    this.btnProduce.Text = "生成(&G)";
                }
            }
        }

        private void btnSearDes_Click(object sender, EventArgs e)
        {
            this.sfdDest.ShowDialog(this);
        }

        private void btnSearExpt_Click(object sender, EventArgs e)
        {
            this.ofdExpt.ShowDialog(this);
        }

        private void btnSrc_Click(object sender, EventArgs e)
        {
            this.ofdSrc.ShowDialog(this);
        }

        private void btSelectPath_Click(object sender, EventArgs e)
        {
            if (this.fbComponent.ShowDialog(this) == DialogResult.OK)
            {
                this.txtComponent.Text = this.fbComponent.SelectedPath.Substring(this.fbComponent.SelectedPath.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                this.txtLocation.Text = this.fbComponent.SelectedPath;
                this.txtSubUrl.Text = this.txtComponent.Text;
            }
        }

        private void btUpload_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtHost.Text) && (this.fbComponent.ShowDialog(this) == DialogResult.OK))
            {
                string selectedPath = this.fbComponent.SelectedPath;
                if (selectedPath.Substring(selectedPath.LastIndexOf(Path.DirectorySeparatorChar) + 1) == "soft_release")
                {
                    this.Cursor = Cursors.WaitCursor;
                    try
                    {
                        try
                        {
                            this.Clear();
                            if (this.SVNUpdate(selectedPath))
                            {
                                this.FtpUpload(this.txtHost.Text, selectedPath, selectedPath);
                            }
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show(exception.Message + "\r\n" + exception.StackTrace);
                        }
                        return;
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default;
                    }
                }
                MessageBox.Show("必须选择soft_release所在的文件夹");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtHost.Text) && (this.fbComponent.ShowDialog(this) == DialogResult.OK))
            {
                string selectedPath = this.fbComponent.SelectedPath;
                int index = selectedPath.IndexOf(@"\soft_release\");
                if (index > 0)
                {
                    string str4;
                    if (!this.SVNUpdate(selectedPath))
                    {
                        return;
                    }
                    if (((str4 = selectedPath.Substring(selectedPath.LastIndexOf(Path.DirectorySeparatorChar) + 1)) != null) && (((str4 == "soft_release") || (str4 == "系统")) || (str4 == "组件")))
                    {
                        MessageBox.Show("你必须选择某一个组件，而不是所有组件或某个部分所有组件");
                        return;
                    }
                    this.Cursor = Cursors.WaitCursor;
                    try
                    {
                        try
                        {
                            this.Clear();
                            string rootpath = selectedPath.Substring(0, index + 11);
                            this.FtpUpload(this.txtHost.Text, rootpath, selectedPath);
                        }
                        catch (Exception exception)
                        {
                            this.WriteErrLog(exception.Message + "\r\n" + exception.StackTrace);
                        }
                        return;
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default;
                    }
                }
                MessageBox.Show("必须选择soft_release所在的文件夹的某个子目录");
            }
        }

        private bool CheckExist(string filePath)
        {
            foreach (string str in this.txtExpt.Text.Split(new char[] { ';' }))
            {
                if (filePath.Trim() == str.Trim())
                {
                    return true;
                }
            }
            return false;
        }

        private void Clear()
        {
            this.textBox1.Text = string.Empty;
            this.textBox2.Text = string.Empty;
            this.WriteLog(string.Format("开始：{0}", DateTime.Now));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmAULWriter_Load(object sender, EventArgs e)
        {
        }

        private void FtpUpload(string ftpurl, string rootpath, string path)
        {
            DoWorkEventHandler handler = null;
            RunWorkerCompletedEventHandler handler2 = null;
            string name = path.Substring(path.LastIndexOf(Path.DirectorySeparatorChar) + 1);
            switch (name)
            {
                case "soft_release":
                case "系统":
                case "组件":
                    foreach (string str in Directory.GetDirectories(path))
                    {
                        this.FtpUpload(ftpurl, rootpath, str);
                    }
                    return;

                case ".svn":
                    break;

                default:
                {
                    this.waitUploads.Add(name);
                    BackgroundWorker worker = new BackgroundWorker();
                    if (handler == null)
                    {
                        handler = delegate (object sender, DoWorkEventArgs e) {
                            this.UploadComponent(rootpath, path, name);
                        };
                    }
                    worker.DoWork += handler;
                    worker.WorkerReportsProgress = true;
                    if (handler2 == null)
                    {
                        handler2 = delegate (object sender, RunWorkerCompletedEventArgs e) {
                            this.waitUploads.Remove(name);
                            if (e.Error != null)
                            {
                                this.WriteLog(string.Format("上传：{0} 发生错误：{1}\r\n{2}", name, e.Error.Message, e.Error.StackTrace));
                            }
                            else
                            {
                                this.WriteLog(string.Format("上传：{0}  完成", name));
                            }
                            if (this.waitUploads.Count == 0)
                            {
                                this.WriteLog(string.Format("{0}:全部上传完成", DateTime.Now));
                            }
                        };
                    }
                    worker.RunWorkerCompleted += handler2;
                    worker.RunWorkerAsync();
                    break;
                }
            }
        }

        private StringCollection GetAllFiles(string rootdir)
        {
            StringCollection result = new StringCollection();
            this.GetAllFiles(rootdir, result);
            return result;
        }

        private void GetAllFiles(string parentDir, StringCollection result)
        {
            string[] directories = Directory.GetDirectories(parentDir);
            for (int i = 0; i < directories.Length; i++)
            {
                if (!directories[i].EndsWith(@"\.svn"))
                {
                    this.GetAllFiles(directories[i], result);
                }
            }
            string[] files = Directory.GetFiles(parentDir);
            for (int j = 0; j < files.Length; j++)
            {
                result.Add(files[j]);
            }
        }

        private void GetFileVersions(string rootPath, string path, Dictionary<string, string[]> oldfilevers, ref Dictionary<string, string[]> filevers)
        {
            if (path.Substring(path.LastIndexOf(Path.DirectorySeparatorChar) + 1) != ".svn")
            {
                long num = 0x295L;
                long num2 = 0x2d3L;
                using (SvnWorkingCopyClient client = new SvnWorkingCopyClient())
                {
                    if (SvnTools.IsManagedPath(path))
                    {
                        Collection<SvnWorkingCopyEntryEventArgs> collection;
                        if (client.GetEntries(path, out collection))
                        {
                            foreach (SvnWorkingCopyEntryEventArgs args in collection)
                            {
                                if (!(args.FullPath != path))
                                {
                                    continue;
                                }
                                string key = args.FullPath.Replace(rootPath, "");
                                if (!key.EndsWith("component.xml") && (args.LastChangeRevision > 0L))
                                {
                                    string str2 = "1.50.00";
                                    if ((oldfilevers != null) && !oldfilevers.ContainsKey(key))
                                    {
                                        str2 = "1.00.00";
                                    }
                                    else if (string.IsNullOrEmpty(oldfilevers[key][1]))
                                    {
                                        if ((args.LastChangeRevision > num) && (args.LastChangeRevision <= num2))
                                        {
                                            str2 = "1.60.00";
                                        }
                                        else if (args.LastChangeRevision > num2)
                                        {
                                            str2 = ((0x3e80L + args.LastChangeRevision) - num2).ToString();
                                            str2 = str2.Insert(str2.Length - 4, ".");
                                            str2 = str2.Insert(str2.Length - 2, ".");
                                        }
                                    }
                                    else
                                    {
                                        str2 = oldfilevers[key][0];
                                        if (oldfilevers[key][1] != args.Checksum)
                                        {
                                            str2 = (Convert.ToInt32(str2.Replace(".", string.Empty)) + 1).ToString();
                                            str2 = str2.Insert(str2.Length - 4, ".");
                                            str2 = str2.Insert(str2.Length - 2, ".");
                                        }
                                    }
                                    filevers.Add(key, new string[] { str2, args.Checksum });
                                }
                            }
                        }
                        foreach (string str3 in Directory.GetDirectories(path))
                        {
                            this.GetFileVersions(rootPath, str3, oldfilevers, ref filevers);
                        }
                    }
                    else
                    {
                        this.WriteLog(string.Format("目录：{0} 不受SVN管理", path));
                    }
                }
            }
        }

        private bool GetLogs(string path, string changelog)
        {
            SvnClient client = new SvnClient();
            Collection<SvnLogEventArgs> logItems = null;
            if (client.GetLog(path, out logItems))
            {
                DataTable table = new DataTable("log");
                table.Columns.Add("time", typeof(DateTime));
                table.Columns.Add("message", typeof(string));
                table.Columns.Add("author", typeof(string));
                foreach (SvnLogEventArgs args in logItems)
                {
                    if (!string.IsNullOrEmpty(args.LogMessage))
                    {
                        table.Rows.Add(new object[] { args.Time, args.LogMessage, args.Author });
                    }
                }
                if (table.Rows.Count > 0)
                {
                    DataSet set = new DataSet("logs");
                    set.Tables.Add(table);
                    set.WriteXml(changelog);
                    return true;
                }
            }
            return false;
        }

        private Dictionary<string, string[]> GetOldFileVersions(string localfile)
        {
            Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
            if (File.Exists(localfile))
            {
                try
                {
                    XmlFiles files = new XmlFiles(localfile);
                    foreach (XmlNode node in files.GetNodeList("Component"))
                    {
                        if (node.Name == "Files")
                        {
                            foreach (XmlNode node2 in node.ChildNodes)
                            {
                                if (node2.Name == "File")
                                {
                                    string key = node2.Attributes["Name"].Value.Trim();
                                    string str2 = node2.Attributes["Ver"].Value.Trim();
                                    string str3 = string.Empty;
                                    try
                                    {
                                        str3 = node2.Attributes["Checksum"].Value.Trim();
                                    }
                                    catch
                                    {
                                    }
                                    dictionary.Add(key, new string[] { str2, str3 });
                                }
                            }
                            continue;
                        }
                    }
                }
                catch (Exception exception)
                {
                    this.WriteErrLog(string.Format("解析“{0}”出错,请检查问题后再重新上传该组件：{1}", localfile, exception.Message));
                    throw exception;
                }
            }
            return dictionary;
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tbpUpload = new System.Windows.Forms.TabPage();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.txtHost = new System.Windows.Forms.ComboBox();
            this.txtFtpLocation = new System.Windows.Forms.TextBox();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.btUpload = new System.Windows.Forms.Button();
            this.tbpBase = new System.Windows.Forms.TabPage();
            this.txtLocation = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.txtSubUrl = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.lvComList = new System.Windows.Forms.ListView();
            this.chFileName = new System.Windows.Forms.ColumnHeader();
            this.chFolder = new System.Windows.Forms.ColumnHeader();
            this.chUrl = new System.Windows.Forms.ColumnHeader();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btDel = new System.Windows.Forms.ToolStripMenuItem();
            this.txtComponentUrl = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btSelectPath = new System.Windows.Forms.Button();
            this.txtComponent = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnProduce = new System.Windows.Forms.Button();
            this.prbProd = new System.Windows.Forms.ProgressBar();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnExpt = new System.Windows.Forms.Button();
            this.txtExpt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnDest = new System.Windows.Forms.Button();
            this.txtDest = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSrc = new System.Windows.Forms.Button();
            this.txtSrc = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ofdSrc = new System.Windows.Forms.OpenFileDialog();
            this.sfdDest = new System.Windows.Forms.SaveFileDialog();
            this.ofdExpt = new System.Windows.Forms.OpenFileDialog();
            this.fbComponent = new System.Windows.Forms.FolderBrowserDialog();
            this.bt_mr = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tbpUpload.SuspendLayout();
            this.tbpBase.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(461, 53);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 21.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(71, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(300, 33);
            this.label1.TabIndex = 0;
            this.label1.Text = "AutoUpdaterList Writer";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tabControl1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 53);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(461, 370);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tbpUpload);
            this.tabControl1.Controls.Add(this.tbpBase);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 17);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(455, 350);
            this.tabControl1.TabIndex = 0;
            // 
            // tbpUpload
            // 
            this.tbpUpload.Controls.Add(this.bt_mr);
            this.tbpUpload.Controls.Add(this.checkBox1);
            this.tbpUpload.Controls.Add(this.textBox2);
            this.tbpUpload.Controls.Add(this.textBox1);
            this.tbpUpload.Controls.Add(this.button2);
            this.tbpUpload.Controls.Add(this.txtHost);
            this.tbpUpload.Controls.Add(this.txtFtpLocation);
            this.tbpUpload.Controls.Add(this.txtPwd);
            this.tbpUpload.Controls.Add(this.txtUser);
            this.tbpUpload.Controls.Add(this.label10);
            this.tbpUpload.Controls.Add(this.label11);
            this.tbpUpload.Controls.Add(this.label12);
            this.tbpUpload.Controls.Add(this.label13);
            this.tbpUpload.Controls.Add(this.btUpload);
            this.tbpUpload.Location = new System.Drawing.Point(4, 21);
            this.tbpUpload.Name = "tbpUpload";
            this.tbpUpload.Padding = new System.Windows.Forms.Padding(3);
            this.tbpUpload.Size = new System.Drawing.Size(447, 325);
            this.tbpUpload.TabIndex = 2;
            this.tbpUpload.Text = "※上传";
            this.tbpUpload.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(5, 59);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(144, 16);
            this.checkBox1.TabIndex = 48;
            this.checkBox1.Text = "生成所有组件更新日志";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.textBox2.ForeColor = System.Drawing.Color.Red;
            this.textBox2.Location = new System.Drawing.Point(3, 248);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox2.Size = new System.Drawing.Size(434, 70);
            this.textBox2.TabIndex = 47;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.textBox1.ForeColor = System.Drawing.Color.Blue;
            this.textBox1.Location = new System.Drawing.Point(3, 84);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(434, 158);
            this.textBox1.TabIndex = 46;
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.Location = new System.Drawing.Point(352, 55);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(85, 23);
            this.button2.TabIndex = 45;
            this.button2.Text = "上传组件(&M)";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtHost
            // 
            this.txtHost.FormattingEnabled = true;
            this.txtHost.Items.AddRange(new object[] {
            "192.168.1.2"});
            this.txtHost.Location = new System.Drawing.Point(38, 6);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(161, 20);
            this.txtHost.TabIndex = 44;
            this.txtHost.Text = "192.168.1.2";
            // 
            // txtFtpLocation
            // 
            this.txtFtpLocation.Location = new System.Drawing.Point(268, 5);
            this.txtFtpLocation.Name = "txtFtpLocation";
            this.txtFtpLocation.Size = new System.Drawing.Size(168, 21);
            this.txtFtpLocation.TabIndex = 43;
            this.txtFtpLocation.Text = "PgSoft";
            // 
            // txtPwd
            // 
            this.txtPwd.Location = new System.Drawing.Point(268, 28);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.Size = new System.Drawing.Size(168, 21);
            this.txtPwd.TabIndex = 42;
            this.txtPwd.Text = "tmp";
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(38, 30);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(161, 21);
            this.txtUser.TabIndex = 41;
            this.txtUser.Text = "tmp";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(4, 33);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 12);
            this.label10.TabIndex = 40;
            this.label10.Text = "User:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(205, 33);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(59, 12);
            this.label11.TabIndex = 39;
            this.label11.Text = "Password:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(205, 8);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(59, 12);
            this.label12.TabIndex = 38;
            this.label12.Text = "Location:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(1, 8);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(35, 12);
            this.label13.TabIndex = 37;
            this.label13.Text = "Host:";
            // 
            // btUpload
            // 
            this.btUpload.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btUpload.Location = new System.Drawing.Point(250, 55);
            this.btUpload.Name = "btUpload";
            this.btUpload.Size = new System.Drawing.Size(85, 23);
            this.btUpload.TabIndex = 36;
            this.btUpload.Text = "上传全部(&F)";
            this.btUpload.UseVisualStyleBackColor = true;
            this.btUpload.Click += new System.EventHandler(this.btUpload_Click);
            // 
            // tbpBase
            // 
            this.tbpBase.Controls.Add(this.txtLocation);
            this.tbpBase.Controls.Add(this.label9);
            this.tbpBase.Controls.Add(this.button1);
            this.tbpBase.Controls.Add(this.txtSubUrl);
            this.tbpBase.Controls.Add(this.label8);
            this.tbpBase.Controls.Add(this.lvComList);
            this.tbpBase.Controls.Add(this.txtComponentUrl);
            this.tbpBase.Controls.Add(this.label6);
            this.tbpBase.Controls.Add(this.btSelectPath);
            this.tbpBase.Controls.Add(this.txtComponent);
            this.tbpBase.Controls.Add(this.label7);
            this.tbpBase.Controls.Add(this.btnProduce);
            this.tbpBase.Controls.Add(this.prbProd);
            this.tbpBase.Controls.Add(this.txtUrl);
            this.tbpBase.Controls.Add(this.label5);
            this.tbpBase.Controls.Add(this.btnExpt);
            this.tbpBase.Controls.Add(this.txtExpt);
            this.tbpBase.Controls.Add(this.label4);
            this.tbpBase.Controls.Add(this.btnDest);
            this.tbpBase.Controls.Add(this.txtDest);
            this.tbpBase.Controls.Add(this.label3);
            this.tbpBase.Controls.Add(this.btnSrc);
            this.tbpBase.Controls.Add(this.txtSrc);
            this.tbpBase.Controls.Add(this.label2);
            this.tbpBase.Location = new System.Drawing.Point(4, 21);
            this.tbpBase.Name = "tbpBase";
            this.tbpBase.Padding = new System.Windows.Forms.Padding(3);
            this.tbpBase.Size = new System.Drawing.Size(447, 325);
            this.tbpBase.TabIndex = 0;
            this.tbpBase.Text = "※生成UpdateList";
            this.tbpBase.UseVisualStyleBackColor = true;
            // 
            // txtLocation
            // 
            this.txtLocation.Location = new System.Drawing.Point(69, 125);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(288, 21);
            this.txtLocation.TabIndex = 36;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(4, 128);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 12);
            this.label9.TabIndex = 35;
            this.label9.Text = "组件位置:";
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Location = new System.Drawing.Point(363, 174);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(58, 21);
            this.button1.TabIndex = 34;
            this.button1.Text = "添加(&+)";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btAddComponent_Click);
            // 
            // txtSubUrl
            // 
            this.txtSubUrl.Location = new System.Drawing.Point(69, 174);
            this.txtSubUrl.Name = "txtSubUrl";
            this.txtSubUrl.Size = new System.Drawing.Size(288, 21);
            this.txtSubUrl.TabIndex = 33;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 177);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 12);
            this.label8.TabIndex = 32;
            this.label8.Text = "组件网址:";
            // 
            // lvComList
            // 
            this.lvComList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chFileName,
            this.chFolder,
            this.chUrl});
            this.lvComList.ContextMenuStrip = this.contextMenuStrip1;
            this.lvComList.Location = new System.Drawing.Point(7, 201);
            this.lvComList.Name = "lvComList";
            this.lvComList.Size = new System.Drawing.Size(414, 69);
            this.lvComList.TabIndex = 31;
            this.lvComList.UseCompatibleStateImageBehavior = false;
            this.lvComList.View = System.Windows.Forms.View.Details;
            // 
            // chFileName
            // 
            this.chFileName.Text = "组件名称";
            this.chFileName.Width = 123;
            // 
            // chFolder
            // 
            this.chFolder.Text = "文件夹";
            this.chFolder.Width = 150;
            // 
            // chUrl
            // 
            this.chUrl.Text = "更新地址";
            this.chUrl.Width = 140;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btDel});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(95, 26);
            // 
            // btDel
            // 
            this.btDel.Name = "btDel";
            this.btDel.Size = new System.Drawing.Size(94, 22);
            this.btDel.Text = "删除";
            this.btDel.Click += new System.EventHandler(this.btDel_Click);
            // 
            // txtComponentUrl
            // 
            this.txtComponentUrl.Location = new System.Drawing.Point(69, 150);
            this.txtComponentUrl.Name = "txtComponentUrl";
            this.txtComponentUrl.Size = new System.Drawing.Size(288, 21);
            this.txtComponentUrl.TabIndex = 30;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 153);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 29;
            this.label6.Text = "更新网址:";
            // 
            // btSelectPath
            // 
            this.btSelectPath.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btSelectPath.Location = new System.Drawing.Point(364, 101);
            this.btSelectPath.Name = "btSelectPath";
            this.btSelectPath.Size = new System.Drawing.Size(58, 21);
            this.btSelectPath.TabIndex = 28;
            this.btSelectPath.Text = "选择(&S)";
            this.btSelectPath.UseVisualStyleBackColor = true;
            this.btSelectPath.Click += new System.EventHandler(this.btSelectPath_Click);
            // 
            // txtComponent
            // 
            this.txtComponent.Location = new System.Drawing.Point(70, 101);
            this.txtComponent.Name = "txtComponent";
            this.txtComponent.Size = new System.Drawing.Size(288, 21);
            this.txtComponent.TabIndex = 27;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 105);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 26;
            this.label7.Text = "组件名称:";
            // 
            // btnProduce
            // 
            this.btnProduce.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnProduce.Font = new System.Drawing.Font("SimSun", 9F);
            this.btnProduce.Location = new System.Drawing.Point(350, 296);
            this.btnProduce.Name = "btnProduce";
            this.btnProduce.Size = new System.Drawing.Size(85, 23);
            this.btnProduce.TabIndex = 25;
            this.btnProduce.Text = "生成(&G)";
            this.btnProduce.UseVisualStyleBackColor = true;
            this.btnProduce.Click += new System.EventHandler(this.btnProduce_Click);
            // 
            // prbProd
            // 
            this.prbProd.Location = new System.Drawing.Point(7, 295);
            this.prbProd.Name = "prbProd";
            this.prbProd.Size = new System.Drawing.Size(335, 23);
            this.prbProd.TabIndex = 24;
            this.prbProd.Visible = false;
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(70, 31);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(288, 21);
            this.txtUrl.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "更新网址:";
            // 
            // btnExpt
            // 
            this.btnExpt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnExpt.Location = new System.Drawing.Point(365, 57);
            this.btnExpt.Name = "btnExpt";
            this.btnExpt.Size = new System.Drawing.Size(58, 21);
            this.btnExpt.TabIndex = 8;
            this.btnExpt.Text = "选择(&S)";
            this.btnExpt.UseVisualStyleBackColor = true;
            this.btnExpt.Click += new System.EventHandler(this.btnSearExpt_Click);
            // 
            // txtExpt
            // 
            this.txtExpt.Location = new System.Drawing.Point(70, 57);
            this.txtExpt.Multiline = true;
            this.txtExpt.Name = "txtExpt";
            this.txtExpt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtExpt.Size = new System.Drawing.Size(288, 41);
            this.txtExpt.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "排除文件:";
            // 
            // btnDest
            // 
            this.btnDest.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDest.Location = new System.Drawing.Point(365, 270);
            this.btnDest.Name = "btnDest";
            this.btnDest.Size = new System.Drawing.Size(58, 21);
            this.btnDest.TabIndex = 5;
            this.btnDest.Text = "选择(&S)";
            this.btnDest.UseVisualStyleBackColor = true;
            this.btnDest.Click += new System.EventHandler(this.btnSearDes_Click);
            // 
            // txtDest
            // 
            this.txtDest.Location = new System.Drawing.Point(70, 269);
            this.txtDest.Name = "txtDest";
            this.txtDest.Size = new System.Drawing.Size(288, 21);
            this.txtDest.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 273);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "保存位置:";
            // 
            // btnSrc
            // 
            this.btnSrc.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSrc.Location = new System.Drawing.Point(364, 6);
            this.btnSrc.Name = "btnSrc";
            this.btnSrc.Size = new System.Drawing.Size(58, 21);
            this.btnSrc.TabIndex = 2;
            this.btnSrc.Text = "选择(&S)";
            this.btnSrc.UseVisualStyleBackColor = true;
            this.btnSrc.Click += new System.EventHandler(this.btnSrc_Click);
            // 
            // txtSrc
            // 
            this.txtSrc.Location = new System.Drawing.Point(70, 6);
            this.txtSrc.Name = "txtSrc";
            this.txtSrc.Size = new System.Drawing.Size(288, 21);
            this.txtSrc.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "主程序:";
            // 
            // ofdSrc
            // 
            this.ofdSrc.DefaultExt = "*.exe";
            this.ofdSrc.Filter = "程序文件(*.exe)|*.exe|所有文件(*.*)|*.*";
            this.ofdSrc.Title = "请选择主程序文件";
            this.ofdSrc.FileOk += new System.ComponentModel.CancelEventHandler(this.ofdDest_FileOk);
            // 
            // sfdDest
            // 
            this.sfdDest.CheckPathExists = false;
            this.sfdDest.DefaultExt = "*.xml";
            this.sfdDest.FileName = "UpdateList.xml";
            this.sfdDest.Filter = "XML文件(*.xml)|*.xml";
            this.sfdDest.Title = "请选择AutoUpdaterList保存位置";
            this.sfdDest.FileOk += new System.ComponentModel.CancelEventHandler(this.sfdSrcPath_FileOk);
            // 
            // ofdExpt
            // 
            this.ofdExpt.DefaultExt = "*.*";
            this.ofdExpt.Filter = "所有文件(*.*)|*.*";
            this.ofdExpt.Multiselect = true;
            this.ofdExpt.Title = "请选择主程序文件";
            this.ofdExpt.FileOk += new System.ComponentModel.CancelEventHandler(this.ofdExpt_FileOk);
            // 
            // fbComponent
            // 
            this.fbComponent.Description = "选择组件所在文件夹";
            this.fbComponent.SelectedPath = "E:\\dev\\soft_release";
            // 
            // bt_mr
            // 
            this.bt_mr.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bt_mr.Location = new System.Drawing.Point(178, 55);
            this.bt_mr.Name = "bt_mr";
            this.bt_mr.Size = new System.Drawing.Size(66, 23);
            this.bt_mr.TabIndex = 49;
            this.bt_mr.Text = "默认上传(&U)";
            this.bt_mr.UseVisualStyleBackColor = true;
            this.bt_mr.Click += new System.EventHandler(this.bt_mr_Click);
            // 
            // frmAULWriter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(461, 423);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(500, 450);
            this.MinimizeBox = false;
            this.Name = "frmAULWriter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AULWriter for AutoUpdater";
            this.Load += new System.EventHandler(this.frmAULWriter_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tbpUpload.ResumeLayout(false);
            this.tbpUpload.PerformLayout();
            this.tbpBase.ResumeLayout(false);
            this.tbpBase.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void ofdDest_FileOk(object sender, CancelEventArgs e)
        {
            this.txtSrc.Text = this.ofdSrc.FileName;
        }

        private void ofdExpt_FileOk(object sender, CancelEventArgs e)
        {
            foreach (string str in this.ofdExpt.FileNames)
            {
                this.txtExpt.Text = this.txtExpt.Text + str.ToString() + "\n\r;";
            }
        }

        private void sfdSrcPath_FileOk(object sender, CancelEventArgs e)
        {
            this.txtDest.Text = this.sfdDest.FileName.Substring(0, this.sfdDest.FileName.LastIndexOf(@"\")) + @"\UpdateList.xml";
        }

        private bool SVNUpdate(string path)
        {
            SvnClient client = new SvnClient();            
            if (client.Update(path))
            {
                this.WriteLog("获取SVN最新版本文件成功");
                return true;
            }
            this.WriteErrLog("获取SVN最新版本文件失败");
            return false;
        }

        private void UploadComponent(string rootpath, string path, string name)
        {
            FTPclient pclient = new FTPclient {
                Hostname = this.txtHost.Text + "/" + this.txtFtpLocation.Text
            };
            if (!string.IsNullOrEmpty(this.txtUser.Text))
            {
                pclient.Username = this.txtUser.Text;
                pclient.Password = this.txtPwd.Text;
            }
            string str = path.Replace(rootpath, "");
            string filename = str + "/component.xml";
            string localFilename = Path.Combine(path, name + string.Format("component{0}.xml", DateTime.Now.ToString("yyMMdd_hhmmss")));
            Dictionary<string, string[]> oldfilevers = new Dictionary<string, string[]>();
            if (pclient.FtpFileExists(filename) && pclient.Download(filename, localFilename, true))
            {
                oldfilevers = this.GetOldFileVersions(localFilename);
            }
            Dictionary<string, string[]> filevers = new Dictionary<string, string[]>();
            this.GetFileVersions(path, path, oldfilevers, ref filevers);
            bool flag = false;
            foreach (KeyValuePair<string, string[]> pair in filevers)
            {
                string sourceFilename = str + pair.Key;
                bool flag2 = pclient.FtpFileExists(str + pair.Key);
                string str5 = ((oldfilevers == null) || !oldfilevers.ContainsKey(pair.Key)) ? "1.50.00" : ((string) oldfilevers[pair.Key][0]);
                if (((oldfilevers == null) && ((pair.Value[0].CompareTo("1.50.00") > 0) || !flag2)) || ((oldfilevers != null) && (!oldfilevers.ContainsKey(pair.Key) || (pair.Value[0].CompareTo((string) oldfilevers[pair.Key][0]) > 0))))
                {
                    if (flag2)
                    {
                        string str6 = sourceFilename + str5;
                        if (pclient.FtpFileExists(str6))
                        {
                            pclient.FtpDelete(str6);
                            this.WriteLog(string.Format("成功删除了已存在的同名备份文件：{0}", str6));
                        }
                        pclient.FtpRename(sourceFilename, str6);
                    }
                    else
                    {
                        this.WriteLog(string.Format("ftp服务器上没有找到：{0}{1} 文件，请确认", path, pair.Key));
                    }
                    if (pclient.Upload(path + pair.Key, str + pair.Key))
                    {
                        flag = true;
                        this.WriteLog(string.Format("上传：{0}{1} 成功", path, pair.Key));
                    }
                    else
                    {
                        this.WriteErrLog(string.Format("上传{0}{1}不成功,你需要排除错误后重新全部上传", path, pair.Key));
                        return;
                    }
                }
            }
            if (flag)
            {
                string str7 = Path.Combine(path, name + "component.xml");
                FileStream stream = new FileStream(str7, FileMode.Create);
                StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
                writer.Write("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>\r\n");
                writer.Write(string.Format("<Component Name=\"{0}\">\r\n", name));
                writer.Write("\t<Files>\r\n");
                foreach (KeyValuePair<string, string[]> pair2 in filevers)
                {
                    writer.Write(string.Format("\t\t<File Name=\"{0}\" Ver=\"{1}\" Checksum=\"{2}\" />\r\n", pair2.Key, pair2.Value[0], pair2.Value[1]));
                }
                writer.Write("\t</Files>\r\n");
                writer.Write("</Component>\r\n");
                writer.Close();
                stream.Close();
                this.WriteLog(string.Format("生成{0}成功", Path.Combine(path, name + "component.xml")));
                if (pclient.FtpFileExists(filename))
                {
                    pclient.FtpRename(filename, filename + DateTime.Now.ToString("yyMdHHmmss"));
                }
                if (pclient.Upload(str7, filename))
                {
                    this.WriteLog(string.Format("上传：{0}  成功", str7));
                }
            }
            if (this.checkBox1.Checked || flag)
            {
                string changelog = Path.Combine(path, name + "changelog.xml");
                if (this.GetLogs(path, changelog))
                {
                    string str9 = string.Format("{0}/{1}changelog.xml", str, name);
                    if (pclient.FtpFileExists(str9))
                    {
                        pclient.FtpRename(str9, str9 + DateTime.Now.ToString("yyMdHHmmss"));
                    }
                    if (pclient.Upload(changelog, str9))
                    {
                        this.WriteLog(string.Format("上传：{0}  成功", changelog));
                    }
                }
            }
        }

        private void WriteErrLog(string text)
        {
            if (this.textBox2.InvokeRequired)
            {
                this.textBox2.BeginInvoke(new Action<string>(this.WriteErrLog), new object[] { text });
            }
            else
            {
                this.textBox2.Lines = new List<string>(this.textBox2.Lines) { text }.ToArray();
                this.textBox2.Focus();
                this.textBox2.Select(this.textBox2.Text.Length, 0);
                this.textBox2.ScrollToCaret();
            }
        }

        private void WriteLog(string text)
        {
            if (this.textBox1.InvokeRequired)
            {
                this.textBox1.BeginInvoke(new Action<string>(this.WriteLog), new object[] { text });
            }
            else
            {
                this.textBox1.Lines = new List<string>(this.textBox1.Lines) { text }.ToArray();
                this.textBox1.Focus();
                this.textBox1.Select(this.textBox1.Text.Length, 0);
                this.textBox1.ScrollToCaret();
            }
        }

        private void WriterAUList()
        {
            string str = this.txtSrc.Text.Trim().Substring(this.txtSrc.Text.Trim().LastIndexOf(@"\") + 1, (this.txtSrc.Text.Trim().Length - this.txtSrc.Text.Trim().LastIndexOf(@"\")) - 1);
            FileStream stream = new FileStream(this.txtDest.Text.Trim(), FileMode.Create);
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
            writer.Write("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
            writer.Write("\r\n<AutoUpdater>\r\n");
            writer.Write("\t<Description>");
            writer.Write(str.Substring(0, str.LastIndexOf(".")) + " autoUpdate");
            writer.Write("</Description>\r\n");
            writer.Write("\t<Updater>\r\n");
            writer.Write("\t\t<Url>");
            writer.Write(this.txtUrl.Text.Trim());
            writer.Write("</Url>\r\n");
            writer.Write("\t\t<LastUpdateTime>");
            writer.Write(DateTime.Now.AddDays(-15.0).ToString("yyyy-MM-dd"));
            writer.Write("</LastUpdateTime>\r\n");
            writer.Write("\t</Updater>\r\n");
            writer.Write("\t<Application applicationId = \"" + str.Substring(0, str.LastIndexOf(".")) + "\">\r\n");
            writer.Write("\t\t<EntryPoint>");
            writer.Write(str);
            writer.Write("</EntryPoint>\r\n");
            writer.Write("\t\t<Location>");
            writer.Write("bin");
            writer.Write("</Location>\r\n");
            writer.Write("\t\t<BeforeExecutes></BeforeExecutes>\r\n");
            writer.Write("\t\t<KillProcesses></KillProcesses>\r\n");
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(this.txtSrc.Text);
            writer.Write("\t\t<Version>");
            writer.Write(string.Format("{0}.{1}.{2}.{3}", new object[] { versionInfo.FileMajorPart, versionInfo.FileMinorPart, versionInfo.FileBuildPart, versionInfo.FilePrivatePart }));
            writer.Write("</Version>\r\n");
            writer.Write("\t</Application>\r\n");
            writer.Write("\t<Components>\r\n");
            foreach (string[] strArray in this.comps)
            {
                writer.Write(string.Format("\t\t<Component Name=\"{0}\" Url=\"{1}\"/>\r\n", strArray[0], strArray[2]));
            }
            writer.Write("\t</Components>\r\n");
            writer.Write("</AutoUpdater>");
            writer.Close();
            stream.Close();
            MessageBox.Show(this, "自动更新文件生成成功:" + this.txtDest.Text.Trim(), "AutoUpdater", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            this.prbProd.Value = 0;
            this.prbProd.Visible = false;
            this.btnProduce.Text = "生成(&G)";
        }

        private void bt_mr_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtHost.Text))
            {
                string selectedPath = "D:\\soft_release";
                if (selectedPath.Substring(selectedPath.LastIndexOf(Path.DirectorySeparatorChar) + 1) == "soft_release")
                {
                    this.Cursor = Cursors.WaitCursor;
                    try
                    {
                        try
                        {
                            this.Clear();
                            if (this.SVNUpdate(selectedPath))
                            {
                                this.FtpUpload(this.txtHost.Text, selectedPath, selectedPath);
                            }
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show(exception.Message + "\r\n" + exception.StackTrace);
                        }
                        return;
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default;
                    }
                }
                MessageBox.Show("必须选择soft_release所在的文件夹");
            }
        }
    }
}

