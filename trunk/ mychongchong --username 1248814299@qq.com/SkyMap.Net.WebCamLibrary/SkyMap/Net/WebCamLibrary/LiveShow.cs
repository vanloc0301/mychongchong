namespace SkyMap.Net.WebCamLibrary
{
    using DirectX.Capture;
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.DataForms;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Util;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Drawing.Printing;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;
    using System.Text;
    using System.Windows.Forms;

    [ComVisible(true), PermissionSet(SecurityAction.Demand, Name="FullTrust")]
    public class LiveShow : Form
    {
        private ToolStripButton btPrint;
        private DirectX.Capture.Capture capture;
        protected CategoryIdentities[] cis;
        private ComboBox comboBox1;
        private ComboBox comboBox2;
        private IContainer components;
        protected bool enableWrite;
        private Filters filters;
        protected FTPclient ftpClient;
        protected string ftpdir;
        private Uri initUri;
        private Label label2;
        private Label label3;
        private Label label6;
        protected string localPath;
        private string newWebCamFilename;
        private PictureBox pictureBox1;
        private string project_id;
        private string reportOID;
        protected bool retry;
        protected StatusStrip statusStrip1;
        private System.Windows.Forms.Timer timer1;
        protected ToolStrip toolStrip1;
        private ToolStripProgressBar toolStripProgressBar1;
        protected ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripStatusLabel toolStripStatusLabel2;
        private TrackBar trackBar1;
        private DirectX.Capture.Filter videoDevice;
        private WebBrowser webBrowser1;

        public LiveShow()
        {
            this.enableWrite = true;
            this.InitializeComponent();
            this.webBrowser1.Navigated += new WebBrowserNavigatedEventHandler(this.webBrowser1_Navigated);
            if ((this.Site == null) || !this.Site.DesignMode)
            {
                if (!string.IsNullOrEmpty(PropertyService.DataDirectory))
                {
                    this.initUri = new Uri(Path.Combine(PropertyService.DataDirectory, @"Resources\webcam\loading.htm"));
                }
                this.webBrowser1.Navigate(this.initUri);
            }
        }

        public LiveShow(bool p)
        {
            this.enableWrite = true;
        }

        private DataTable AddImgTable(CategoryIdentities cis)
        {
            DataTable table = new DataTable(cis.Name);
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Img", typeof(byte[]));
            table.Columns.Add("Id", typeof(string));
            foreach (CategoryIdentities.Identify identify in cis.Identifies)
            {
                string path = string.Format(@"{0}\{1}.jpg", this.localPath, identify.Name);
                if (System.IO.File.Exists(path))
                {
                    DataRow row = table.NewRow();
                    row[0] = identify.Name;
                    row[1] = System.IO.File.ReadAllBytes(path);
                    try
                    {
                        row[2] = identify.Id;
                    }
                    catch
                    {
                    }
                    table.Rows.Add(row);
                }
            }
            return table;
        }

        protected void AsyncFtpDirectory(string ftpDownDir, string localdir)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += delegate (object sender, DoWorkEventArgs de) {
                try
                {
                    if (this.ftpClient == null)
                    {
                        this.SetupFtp();
                    }
                    this.SetStatusText("正在从FTP同步下载文件...");
                    foreach (FTPfileInfo info in this.ftpClient.ListDirectoryDetail("/" + ftpDownDir))
                    {
                        LoggingService.DebugFormatted("准备下载：{0},类型：{1},扩展名:{2}", new object[] { info.NameOnly, info.FileType, info.Extension });
                        if ((info.FileType == FTPfileInfo.DirectoryEntryTypes.File) && (info.Extension == "jpg"))
                        {
                            string path = Path.Combine(localdir, info.Filename);
                            string sourceFilename = Path.Combine(ftpDownDir, info.Filename);
                            if (!System.IO.File.Exists(path))
                            {
                                LoggingService.DebugFormatted("下载{0}到{1}", new object[] { sourceFilename, path });
                                this.CreateDirtory(path);
                                this.ftpClient.Download(sourceFilename, path, true);
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    LoggingService.Error(exception);
                }
            };
            worker.RunWorkerCompleted += delegate (object sender, RunWorkerCompletedEventArgs re) {
                this.ReFreshPage();
                this.SetStatusText("FTP同步下载完成...");
                if (re.Error != null)
                {
                    MessageBox.Show("列表FTP时出现错误！" + re.Error.Message);
                }
            };
            worker.RunWorkerAsync();
        }

        private void btPrint_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.reportOID))
            {
                WaitDialogHelper.Show();
                try
                {
                    TempletPrint templetPrint = QueryHelper.Get<TempletPrint>("TempletPrint_" + this.reportOID, this.reportOID);
                    if (templetPrint != null)
                    {
                        DataSet reportDataSource = this.GetReportDataSource(templetPrint);
                        if (reportDataSource != null)
                        {
                            foreach (CategoryIdentities identities in this.cis)
                            {
                                reportDataSource.Tables.Add(this.AddImgTable(identities));
                            }
                            base.Hide();
                            PrintHelper.PrintOrShowRDLC(string.Format("{0}({1})", templetPrint.Name, this.ftpdir), templetPrint.PrintPreview, templetPrint.Data, reportDataSource, new PrintEventHandler(this.Printed), null, null);
                        }
                    }
                }
                catch (Exception exception)
                {
                    LoggingService.Error(exception);
                    MessageHelper.ShowInfo("打印报表时出错！");
                }
                finally
                {
                    WaitDialogHelper.Close();
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.capture != null)
            {
                this.capture.PropertyPages[this.comboBox2.SelectedIndex].Show(this);
            }
        }

        protected void CreateDirtory(string path)
        {
            if (!System.IO.File.Exists(path))
            {
                string[] strArray = path.Split(new char[] { '\\' });
                string str = string.Empty;
                for (int i = 0; i < (strArray.Length - 1); i++)
                {
                    str = str + strArray[i].Trim() + @"\";
                    if (!Directory.Exists(str))
                    {
                        Directory.CreateDirectory(str);
                    }
                }
            }
        }

        public void DialogOkClose()
        {
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        protected void FtpUpload(string localFile, string ftpsubpath)
        {
            if (System.IO.File.Exists(localFile))
            {
                if (this.ftpClient == null)
                {
                    this.SetupFtp();
                }
                if (this.ftpClient == null)
                {
                    LoggingService.Warn("不能创建FTPClient对象，请检查是否正确配置了FTP相关文件...");
                }
                else
                {
                    string targetFilename = ftpsubpath + "/" + Path.GetFileName(localFile);
                    try
                    {
                        LoggingService.DebugFormatted("将文件{0}上传到：{1}", new object[] { localFile, targetFilename });
                        this.ftpClient.Upload(localFile, targetFilename);
                    }
                    catch (WebException exception)
                    {
                        LoggingService.Error(exception);
                    }
                }
            }
            else
            {
                LoggingService.WarnFormatted("{0}不存在，所以不能上传", new object[] { localFile });
            }
        }

        private void FtpUpload(string name, string file, string ftpFile)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += delegate (object sender, DoWorkEventArgs e) {
                this.ftpClient.Upload(file, ftpFile);
                this.SetStatusText(string.Format("上传{0}照片中...", name));
            };
            worker.RunWorkerCompleted += delegate (object sender, RunWorkerCompletedEventArgs e) {
                this.SetStatusText(string.Empty);
                if (e.Error != null)
                {
                    LoggingService.Error(e.Error);
                    MessageHelper.ShowError("上传时发生错误", e.Error);
                }
            };
            worker.RunWorkerAsync();
        }

        protected virtual DataSet GetReportDataSource(TempletPrint templetPrint)
        {
            Dictionary<string, string> vals = new Dictionary<string, string>();
            vals.Add("{ProjectId}", this.project_id);
            return templetPrint.GetReportDataSource(false, vals);
        }

        private void he_Click(object sender, HtmlElementEventArgs e)
        {
            if (this.enableWrite)
            {
                HtmlElement element = sender as HtmlElement;
                this.TakeAPicture(element.GetAttribute("pz"));
            }
            else
            {
                MessageHelper.ShowInfo("没有拍照权限，不能拍照！");
            }
        }

        private void heExport_Click(object sender, HtmlElementEventArgs e)
        {
            HtmlElement element = sender as HtmlElement;
            string str = string.Format("{0}.jpg", element.GetAttribute("export"));
            string path = FileUtility.Combine(new string[] { this.localPath, str });
            if (System.IO.File.Exists(path))
            {
                using (FolderBrowserDialog dialog = new FolderBrowserDialog())
                {
                    dialog.Description = "请选择导出文件保存位置";
                    dialog.RootFolder = Environment.SpecialFolder.DesktopDirectory;
                    dialog.ShowDialog(WorkbenchSingleton.MainForm);
                    if (!string.IsNullOrEmpty(dialog.SelectedPath))
                    {
                        System.IO.File.Copy(path, FileUtility.Combine(new string[] { dialog.SelectedPath, str }), true);
                    }
                    return;
                }
            }
            MessageHelper.ShowInfo("照片不存在，请检查是否已经拍照！");
        }

        private void heImport_Click(object sender, HtmlElementEventArgs e)
        {
            HtmlElement element = sender as HtmlElement;
            string str = string.Format("{0}.jpg", element.GetAttribute("import"));
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.CheckFileExists = true;
                dialog.Title = "请选择要导入的文件，注意，只支持JPG格式的图片";
                dialog.Filter = "JPG图片|*.jpg";
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                dialog.ShowDialog(WorkbenchSingleton.MainForm);
                if (!string.IsNullOrEmpty(dialog.FileName))
                {
                    string path = FileUtility.Combine(new string[] { this.localPath, str });
                    this.CreateDirtory(path);
                    System.IO.File.Copy(dialog.FileName, path, true);
                    string ftpFile = this.ftpdir + "/" + str;
                    this.ReFreshPage();
                    this.FtpUpload(element.GetAttribute("import"), path, ftpFile);
                }
            }
        }

        private void InitCategroyList()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<html>\r\n<head>\r\n    <title>ImageView</title>\r\n    <style type='text/css'>\r\n        img\r\n        {\r\n            height: 120px; \r\n            width: 160px;\r\n            border-width:0pt;\r\n        }\r\n        td\r\n        {\r\n            text-align:center;\r\n            font-size:9pt;\r\n        }\r\n        div\r\n        {\r\n            font-size:9pt;\r\n        }\r\n        \r\n    </style>\r\n</head>\r\n<body>");
            for (int i = 0; i < this.cis.Length; i++)
            {
                if (this.cis[i] != null)
                {
                    builder.AppendFormat("<div><b>{0}</b>", this.cis[i].Name);
                    foreach (CategoryIdentities.Identify identify in this.cis[i].Identifies)
                    {
                        string str = new Uri(string.Format(@"{0}\{1}.jpg", this.localPath, identify.Name)).ToString();
                        string uriString = null;
                        if (!string.IsNullOrEmpty(identify.PicPath))
                        {
                            uriString = Path.GetTempFileName() + ".htm";
                        }
                        builder.AppendFormat("<table style='width: 160px;'>\r\n            <tr>\r\n                <td>\r\n                    <a target='_blank' href='{0}'><img src='{0}' alt='{1}常未拍照' /></a></td>\r\n            </tr>\r\n            <tr>\r\n                <td>\r\n                    {1}</td>\r\n            </tr>\r\n            <tr>\r\n                <td >\r\n                    <input type='button' pz='{1}' value='拍照'/>{2}</td>\r\n            </tr>\r\n            <tr>\r\n                <td >\r\n                    <input type='button' export='{1}' value='导出'/> <input type='button' import='{1}' value='导入'/></td>\r\n            </tr>\r\n        </table>", str, identify.Name, string.IsNullOrEmpty(identify.PicPath) ? string.Empty : string.Format(" <input type='button' onclick=\"window.open('{0}');\" value='比对'/>", new Uri(uriString).ToString()));
                        builder.Append("</div>");
                        if (!string.IsNullOrEmpty(identify.PicPath))
                        {
                            string contents = string.Format("<html>\r\n\t<head>\r\n\t\t<title></title>\r\n\t</head>\r\n\t<body>\r\n\t\r\n\t    <p>\r\n            <img alt='{0}尚未拍照' src='{1}' style='height: 240px; width: 320px' /><img alt='{0}无身份证照片' src='{2}' \r\n                style='height: 240px; width: 320px' /></p>\r\n\t\r\n\t</body>\r\n</html>", identify.Name, str, new Uri(identify.PicPath).ToString());
                            System.IO.File.WriteAllText(uriString, contents, Encoding.UTF8);
                        }
                    }
                }
            }
            builder.AppendFormat("</body></html>", new object[0]);
            string path = Path.GetTempFileName() + ".htm";
            System.IO.File.WriteAllText(path, builder.ToString(), Encoding.UTF8);
            this.webBrowser1.Navigate(new Uri(path));
        }

        protected void InitDevices()
        {
            Action action2 = null;
            if (this.capture == null)
            {
                if (action2 == null)
                {
                    action2 = delegate {
                        try
                        {
                            LoggingService.Debug("初始化视频设备参数...");
                            this.InitVideo();
                            LoggingService.Debug("初始化压缩等设备参数...");
                            this.initMenu();
                        }
                        catch (TargetInvocationException)
                        {
                        }
                    };
                }
                Action method = action2;
                base.BeginInvoke(method);
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.pictureBox1 = new PictureBox();
            this.toolStrip1 = new ToolStrip();
            this.trackBar1 = new TrackBar();
            this.comboBox1 = new ComboBox();
            this.label3 = new Label();
            this.label2 = new Label();
            this.comboBox2 = new ComboBox();
            this.label6 = new Label();
            this.webBrowser1 = new WebBrowser();
            this.statusStrip1 = new StatusStrip();
            this.toolStripProgressBar1 = new ToolStripProgressBar();
            this.toolStripStatusLabel1 = new ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((ISupportInitialize) this.pictureBox1).BeginInit();
            this.trackBar1.BeginInit();
            this.statusStrip1.SuspendLayout();
            base.SuspendLayout();
            this.pictureBox1.BackColor = SystemColors.ControlText;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new Point(4, 0x1d);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(640, 480);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.toolStrip1.Location = new Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.ShowItemToolTips = false;
            this.toolStrip1.Size = new Size(880, 0x19);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            this.trackBar1.Location = new Point(0x1f6, 0x203);
            this.trackBar1.Maximum = 100;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new Size(0x68, 0x2a);
            this.trackBar1.TabIndex = 14;
            this.trackBar1.TickFrequency = 10;
            this.trackBar1.Value = 80;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] { "320 X 240", "640 X 480" });
            this.comboBox1.Location = new Point(0x133, 0x204);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new Size(0x8e, 20);
            this.comboBox1.TabIndex = 12;
            this.comboBox1.Text = "640 X 480";
            this.label3.AutoSize = true;
            this.label3.Font = new Font("Tahoma", 11f);
            this.label3.Location = new Point(12, 0x204);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x53, 0x12);
            this.label3.TabIndex = 9;
            this.label3.Text = "设备设置：";
            this.label3.TextAlign = ContentAlignment.MiddleCenter;
            this.label2.AutoSize = true;
            this.label2.Font = new Font("Tahoma", 11f);
            this.label2.Location = new Point(0xf4, 0x204);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x44, 0x12);
            this.label2.TabIndex = 10;
            this.label2.Text = " 尺 寸 ：";
            this.label2.TextAlign = ContentAlignment.MiddleCenter;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new Point(90, 0x204);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new Size(0x8e, 20);
            this.comboBox2.TabIndex = 12;
            this.comboBox2.Text = "选择....";
            this.comboBox2.SelectedIndexChanged += new EventHandler(this.comboBox2_SelectedIndexChanged);
            this.label6.AutoSize = true;
            this.label6.Font = new Font("Tahoma", 11f);
            this.label6.Location = new Point(0x1ca, 0x207);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x3a, 0x12);
            this.label6.TabIndex = 10;
            this.label6.Text = "质量 ：";
            this.label6.TextAlign = ContentAlignment.MiddleCenter;
            this.webBrowser1.Location = new Point(0x288, 0x1d);
            this.webBrowser1.MinimumSize = new Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new Size(0xe0, 480);
            this.webBrowser1.TabIndex = 15;
            this.webBrowser1.Url = new Uri("", UriKind.Relative);
            this.statusStrip1.Items.AddRange(new ToolStripItem[] { this.toolStripProgressBar1, this.toolStripStatusLabel1, this.toolStripStatusLabel2 });
            this.statusStrip1.Location = new Point(0, 0x231);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new Size(880, 0x16);
            this.statusStrip1.TabIndex = 0x10;
            this.statusStrip1.Text = "statusStrip1";
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new Size(400, 0x10);
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new Size(0, 0x11);
            this.toolStripStatusLabel2.ForeColor = Color.Red;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new Size(0, 0x11);
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new EventHandler(this.timer1_Tick);
            this.AutoScaleBaseSize = new Size(6, 14);
            this.BackColor = SystemColors.Control;
            base.ClientSize = new Size(880, 0x247);
            base.Controls.Add(this.statusStrip1);
            base.Controls.Add(this.webBrowser1);
            base.Controls.Add(this.trackBar1);
            base.Controls.Add(this.comboBox2);
            base.Controls.Add(this.comboBox1);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.label6);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.toolStrip1);
            base.Controls.Add(this.pictureBox1);
            base.MaximizeBox = false;
            this.MaximumSize = new Size(0x378, 610);
            this.MinimumSize = new Size(0x378, 610);
            base.Name = "LiveShow";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "拍摄业务照片视频作为证据";
            base.TopMost = true;
            base.WindowState = FormWindowState.Minimized;
            base.FormClosed += new FormClosedEventHandler(this.LiveShow_FormClosed);
            ((ISupportInitialize) this.pictureBox1).EndInit();
            this.trackBar1.EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void initMenu()
        {
            string[] items = null;
            if ((this.capture != null) && (this.capture.PropertyPages != null))
            {
                items = new string[this.capture.PropertyPages.Count];
                for (int i = 0; i < this.capture.PropertyPages.Count; i++)
                {
                    items[i] = this.capture.PropertyPages[i].Name;
                }
                try
                {
                    this.comboBox2.Items.Clear();
                    this.comboBox2.Items.AddRange(items);
                }
                catch (Exception exception)
                {
                    LoggingService.Error(exception);
                }
            }
        }

        private void InitVideo()
        {
            this.filters = new Filters();
            if (this.filters.VideoInputDevices.Count > 0)
            {
                LoggingService.DebugFormatted("共检测到系统有{0}个视频设备...", new object[] { this.filters.VideoInputDevices.Count });
                this.videoDevice = this.filters.VideoInputDevices[this.filters.VideoInputDevices.Count - 1];
                this.capture = new DirectX.Capture.Capture(this.videoDevice, null);
                this.capture.FrameSize = new Size(640, 480);
                this.capture.PreviewWindow = this.pictureBox1;
                this.capture.RenderPreview();
            }
            else if (this.toolStripStatusLabel2 != null)
            {
                this.toolStripStatusLabel2.Text = "系统没有找到摄像头设备，你只能查看已经拍好的照片";
            }
        }

        private void LiveShow_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.capture != null)
            {
                this.capture.Dispose();
                this.capture = null;
            }
        }

        protected void LoadParameters(string subftppath, string localPath, string project_id, bool enableWrite)
        {
            this.ftpdir = subftppath;
            this.localPath = localPath;
            this.enableWrite = enableWrite;
            this.project_id = project_id;
            this.AsyncFtpDirectory(this.ftpdir, localPath);
        }

        protected void LoadParameters(string subftppath, string localPath, string project_id, bool enableWrite, params CategoryIdentities[] cis)
        {
            this.ftpdir = subftppath;
            this.localPath = localPath;
            this.cis = cis;
            this.project_id = project_id;
            this.enableWrite = enableWrite;
            this.webBrowser1.Navigate(this.initUri);
            this.AsyncFtpDirectory(this.ftpdir, localPath);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (((this.Site == null) || !this.Site.DesignMode) && (this.capture == null))
            {
                this.InitDevices();
            }
        }

        protected virtual void Printed(object sender, PrintEventArgs e)
        {
        }

        public virtual void ReFreshPage()
        {
            LoggingService.DebugFormatted("{0},{1}", new object[] { this.webBrowser1.Url, this.initUri });
            if (this.webBrowser1.Url == this.initUri)
            {
                this.InitCategroyList();
            }
            else
            {
                this.webBrowser1.Navigate(this.webBrowser1.Url);
            }
        }

        private void SetStatusText(string text)
        {
            if (base.InvokeRequired)
            {
                base.BeginInvoke(new Action<string>(this.SetStatusText), new object[] { text });
            }
            this.toolStripStatusLabel1.Text = text;
        }

        public void SetToolbarPath(string addinToolbarPath, bool removeExistButton)
        {
            if (removeExistButton)
            {
                this.toolStrip1.Items.Clear();
            }
            if (!string.IsNullOrEmpty(addinToolbarPath))
            {
                this.toolStrip1.Items.AddRange(ToolbarService.CreateToolStripItems(this, addinToolbarPath));
            }
            if (this.toolStrip1.Items.Count == 0)
            {
                this.toolStrip1.Visible = false;
            }
        }

        public void SetupFtp()
        {
            SkyMap.Net.WebCamLibrary.FtpSetting setting = SkyMap.Net.WebCamLibrary.FtpSetting.Get(Path.Combine(PropertyService.ConfigDirectory, "ftpCamera.config"));
            if (string.IsNullOrEmpty(setting.Host))
            {
                throw new ArgumentNullException("host");
            }
            this.ftpClient = new FTPclient();
            this.ftpClient.Hostname = setting.Host + "/" + setting.Path;
            if (!string.IsNullOrEmpty(setting.User))
            {
                this.ftpClient.Username = setting.User;
                this.ftpClient.Password = setting.Password;
            }
        }

        public DialogResult ShowDialog(string subftppath, string localPath, string project_id, bool enableWrite, params CategoryIdentities[] cis)
        {
            this.LoadParameters(subftppath, localPath, project_id, enableWrite, cis);
            return base.ShowDialog();
        }

        protected void SyncFtpDirectory(string ftpDownDir, string localdir)
        {
            try
            {
                if (this.ftpClient == null)
                {
                    this.SetupFtp();
                }
                this.SetStatusText("正在从FTP同步下载文件...");
                foreach (FTPfileInfo info in this.ftpClient.ListDirectoryDetail("/" + ftpDownDir))
                {
                    LoggingService.DebugFormatted("准备下载：{0},类型：{1},扩展名:{2}", new object[] { info.NameOnly, info.FileType, info.Extension });
                    if ((info.FileType == FTPfileInfo.DirectoryEntryTypes.File) && (info.Extension == "jpg"))
                    {
                        string path = Path.Combine(localdir, info.Filename);
                        string sourceFilename = Path.Combine(ftpDownDir, info.Filename);
                        if (!System.IO.File.Exists(path))
                        {
                            LoggingService.DebugFormatted("下载{0}到{1}", new object[] { sourceFilename, path });
                            this.CreateDirtory(path);
                            this.ftpClient.Download(sourceFilename, path, true);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
            }
        }

        public void TakeAPicture(string name)
        {
            if ((this.capture != null) && !this.capture.Capturing)
            {
                try
                {
                    this.SetStatusText(string.Format("给{0}拍照中...", name));
                    this.newWebCamFilename = name + ".jpg";
                    string path = Path.Combine(this.localPath, this.newWebCamFilename);
                    string ftpFile = this.ftpdir + "/" + this.newWebCamFilename;
                    LoggingService.DebugFormatted("将生成照片文件{0}", new object[] { path });
                    this.CreateDirtory(path);
                    this.capture.CaptureFrame(path, ImageFormat.Jpeg);
                    this.ReFreshPage();
                    this.FtpUpload(name, path, ftpFile);
                }
                catch (Exception exception)
                {
                    LoggingService.Error(exception);
                    MessageHelper.ShowError("拍照时发生异常", exception);
                }
            }
            else
            {
                MessageHelper.ShowInfo("你的系统没有安装摄像头设备，\r\n或者正在录制视频，请先停止录制视频！");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.toolStripProgressBar1.Value < 100)
            {
                this.toolStripProgressBar1.Increment(10);
            }
            else
            {
                this.toolStripProgressBar1.Value = 0;
                this.timer1.Enabled = false;
            }
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            foreach (HtmlElement element in this.webBrowser1.Document.GetElementsByTagName("input"))
            {
                if (!string.IsNullOrEmpty(element.GetAttribute("pz")))
                {
                    element.Click += new HtmlElementEventHandler(this.he_Click);
                }
                else
                {
                    if (!string.IsNullOrEmpty(element.GetAttribute("export")))
                    {
                        element.Click += new HtmlElementEventHandler(this.heExport_Click);
                        continue;
                    }
                    if (!string.IsNullOrEmpty(element.GetAttribute("import")))
                    {
                        element.Click += new HtmlElementEventHandler(this.heImport_Click);
                    }
                }
            }
        }

        public string ReportOID
        {
            set
            {
                if (this.reportOID != value)
                {
                    this.reportOID = value;
                }
                if (!string.IsNullOrEmpty(this.reportOID) && (this.btPrint == null))
                {
                    this.btPrint = new ToolStripButton("打印");
                    this.btPrint.Click += new EventHandler(this.btPrint_Click);
                    this.toolStrip1.Items.Add(this.btPrint);
                }
                this.toolStrip1.Visible = this.toolStrip1.Items.Count > 0;
            }
        }
    }
}

