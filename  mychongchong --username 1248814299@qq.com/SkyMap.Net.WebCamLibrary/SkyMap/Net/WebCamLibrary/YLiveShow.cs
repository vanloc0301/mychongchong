namespace SkyMap.Net.WebCamLibrary
{
    using DirectX.Capture;
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    public class YLiveShow : Form
    {
        private DirectX.Capture.Filter audioDevice;
        private DirectX.Capture.Capture capture;
        private ComboBox comboBox1;
        private ComboBox comboBox2;
        private ComboBox comboBox3;
        private IContainer components;
        private string[] Comprossors = new string[] { "ACDV 2.0.1", "DivX\x00ae 5.2.1 Codec", "DV Video Encoder", "Indeo? video 5.10", "Indeo? video 5.10 Compression Filter", "Microsoft H.261 Video Codec", "Microsoft H.263 Video Codec", "Microsoft Video 1" };
        private string[] fileMasks;
        private Filters filters;
        private FTPclient ftpClient;
        private string ftpdir;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private string localPath;
        private string newWebCamFilename;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private Size size;
        private ToolStrip toolStrip1;
        private TrackBar trackBar1;
        private TrackBar trackBar2;
        private ToolStripButton tsBtnCapturePic;
        private ToolStripButton tsBtnCaptureVideo;
        private ToolStripButton tsBtnDel;
        private ExtTreeView tvImage;
        private DirectX.Capture.Filter videoComprossor;
        private DirectX.Capture.Filter videoDevice;

        public YLiveShow()
        {
            this.InitializeComponent();
        }

        private void AddImageNode(string name)
        {
            Action method = delegate {
                ExtTreeNode node = new ExtTreeNode {
                    Text = name,
                    ToolTipText = "双击查看"
                };
                node.AddTo(this.tvImage);
                this.tvImage.SelectedNode = node;
            };
            base.Invoke(method);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.capture.PropertyPages[this.comboBox2.SelectedIndex].Show(this);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            int num = Convert.ToInt32(this.comboBox3.Text.Substring(this.comboBox3.Text.LastIndexOf("=") + 1));
            this.capture.VideoCompressor = this.filters.VideoCompressors[num];
        }

        private void CreateDirtory(string path)
        {
            if (!File.Exists(path))
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

        public void DeleteImage()
        {
            if (this.tvImage.SelectedNode != null)
            {
                string path = Path.Combine(this.localPath, this.tvImage.SelectedNode.Text);
                if (!File.Exists(path))
                {
                    MessageHelper.ShowInfo("找不到照片文件：{0}", path);
                }
                else if (MessageBox.Show("确定要删除照片" + this.tvImage.SelectedNode.Text, "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
                {
                    this.pictureBox2.Image = null;
                    this.label1.Text = "";
                    File.Delete(this.localPath + "/" + this.tvImage.SelectedNode.Text);
                    this.ftpClient.FtpDelete(this.ftpdir + "/" + this.tvImage.SelectedNode.Text);
                    this.tvImage.Nodes.RemoveAt(this.tvImage.SelectedNode.Index);
                }
            }
            else
            {
                MessageHelper.ShowInfo("请选择要删除的照片文件");
            }
        }

        public void DialogOkClose()
        {
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private string GetFileNameByFilePre(string filePre, string extension)
        {
            int num = 1;
            while (true)
            {
                string str = string.Format("{0}{1}{2}", filePre, num, extension);
                if (!File.Exists(Path.Combine(this.localPath, str)))
                {
                    return str;
                }
                num++;
            }
        }

        public void Initialize(string localPath)
        {
            this.localPath = localPath;
            if (Directory.Exists(localPath))
            {
                foreach (string str in FileUtility.SearchDirectory(localPath, "*.*", false, true))
                {
                    this.AddImageNode(str.Substring(str.LastIndexOf(Path.DirectorySeparatorChar) + 1));
                }
            }
        }

        public void Initialize(string host, string user, string pwd, string ftpPath, string subftppath, string localPath)
        {
            if (string.IsNullOrEmpty(host))
            {
                throw new ArgumentNullException("host");
            }
            this.ftpClient = new FTPclient();
            this.ftpClient.Hostname = host + "/" + ftpPath;
            if (!string.IsNullOrEmpty(user))
            {
                this.ftpClient.Username = user;
                this.ftpClient.Password = pwd;
            }
            this.ftpdir = subftppath;
            this.localPath = localPath;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(YLiveShow));
            ExtTreeViewComparer comparer = new ExtTreeViewComparer();
            this.pictureBox1 = new PictureBox();
            this.toolStrip1 = new ToolStrip();
            this.tsBtnCapturePic = new ToolStripButton();
            this.tsBtnCaptureVideo = new ToolStripButton();
            this.tsBtnDel = new ToolStripButton();
            this.trackBar2 = new TrackBar();
            this.trackBar1 = new TrackBar();
            this.comboBox1 = new ComboBox();
            this.label4 = new Label();
            this.label3 = new Label();
            this.label2 = new Label();
            this.label1 = new Label();
            this.pictureBox2 = new PictureBox();
            this.comboBox2 = new ComboBox();
            this.label6 = new Label();
            this.label5 = new Label();
            this.comboBox3 = new ComboBox();
            this.tvImage = new ExtTreeView();
            ((ISupportInitialize) this.pictureBox1).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.trackBar2.BeginInit();
            this.trackBar1.BeginInit();
            ((ISupportInitialize) this.pictureBox2).BeginInit();
            base.SuspendLayout();
            this.pictureBox1.BackColor = SystemColors.ControlText;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new Point(4, 0x1d);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(640, 480);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.toolStrip1.Items.AddRange(new ToolStripItem[] { this.tsBtnCapturePic, this.tsBtnCaptureVideo, this.tsBtnDel });
            this.toolStrip1.Location = new Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(840, 0x19);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            this.tsBtnCapturePic.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tsBtnCapturePic.Image = (Image) manager.GetObject("tsBtnCapturePic.Image");
            this.tsBtnCapturePic.ImageTransparentColor = Color.Magenta;
            this.tsBtnCapturePic.Name = "tsBtnCapturePic";
            this.tsBtnCapturePic.Size = new Size(0x4b, 0x16);
            this.tsBtnCapturePic.Text = "    拍照   ";
            this.tsBtnCapturePic.ToolTipText = " ";
            this.tsBtnCapturePic.Click += new EventHandler(this.tsBtnCapturePic_Click);
            this.tsBtnCaptureVideo.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tsBtnCaptureVideo.Image = (Image) manager.GetObject("tsBtnCaptureVideo.Image");
            this.tsBtnCaptureVideo.ImageTransparentColor = Color.Magenta;
            this.tsBtnCaptureVideo.Name = "tsBtnCaptureVideo";
            this.tsBtnCaptureVideo.Size = new Size(0x5d, 0x16);
            this.tsBtnCaptureVideo.Text = "   录制视频   ";
            this.tsBtnCaptureVideo.Click += new EventHandler(this.tsBtnCaptureVideo_Click);
            this.tsBtnDel.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.tsBtnDel.Image = (Image) manager.GetObject("tsBtnDel.Image");
            this.tsBtnDel.ImageTransparentColor = Color.Magenta;
            this.tsBtnDel.Name = "tsBtnDel";
            this.tsBtnDel.Size = new Size(0x45, 0x16);
            this.tsBtnDel.Text = "   删除   ";
            this.tsBtnDel.ToolTipText = "  ";
            this.tsBtnDel.Click += new EventHandler(this.tsBtnDel_Click);
            this.trackBar2.Location = new Point(0x1fc, 0x21b);
            this.trackBar2.Maximum = 0x3b;
            this.trackBar2.Minimum = 15;
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Size = new Size(0x68, 0x2a);
            this.trackBar2.TabIndex = 13;
            this.trackBar2.TickFrequency = 4;
            this.trackBar2.Value = 30;
            this.trackBar1.Location = new Point(0x133, 0x21b);
            this.trackBar1.Maximum = 100;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new Size(0x68, 0x2a);
            this.trackBar1.TabIndex = 14;
            this.trackBar1.TickFrequency = 10;
            this.trackBar1.Value = 80;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] { "320 X 240", "640 X 480" });
            this.comboBox1.Location = new Point(90, 0x221);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new Size(0x8e, 20);
            this.comboBox1.TabIndex = 12;
            this.comboBox1.Text = "320 X 240";
            this.label4.AutoSize = true;
            this.label4.Font = new Font("Tahoma", 11f);
            this.label4.Location = new Point(0x1c3, 0x221);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x41, 0x12);
            this.label4.TabIndex = 11;
            this.label4.Text = "Frame：";
            this.label4.TextAlign = ContentAlignment.MiddleCenter;
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
            this.label2.Location = new Point(0x1b, 0x221);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x44, 0x12);
            this.label2.TabIndex = 10;
            this.label2.Text = " 尺 寸 ：";
            this.label2.TextAlign = ContentAlignment.MiddleCenter;
            this.label1.AutoSize = true;
            this.label1.Font = new Font("Tahoma", 9f);
            this.label1.Location = new Point(0x284, 0x202);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x43, 14);
            this.label1.TabIndex = 8;
            this.label1.Text = "拍照时间：";
            this.label1.TextAlign = ContentAlignment.MiddleCenter;
            this.pictureBox2.BackColor = SystemColors.ControlDarkDark;
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Location = new Point(0x287, 0x167);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new Size(200, 150);
            this.pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 15;
            this.pictureBox2.TabStop = false;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new Point(90, 0x204);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new Size(0x8e, 20);
            this.comboBox2.TabIndex = 12;
            this.comboBox2.Text = "选择....";
            this.comboBox2.SelectedIndexChanged += new EventHandler(this.comboBox2_SelectedIndexChanged);
            this.label6.AutoSize = true;
            this.label6.Font = new Font("Tahoma", 11f);
            this.label6.Location = new Point(0x107, 0x21f);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x3a, 0x12);
            this.label6.TabIndex = 10;
            this.label6.Text = "质量 ：";
            this.label6.TextAlign = ContentAlignment.MiddleCenter;
            this.label5.AutoSize = true;
            this.label5.Font = new Font("Tahoma", 11f);
            this.label5.Location = new Point(0x106, 0x204);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x71, 0x12);
            this.label5.TabIndex = 9;
            this.label5.Text = "视频压缩方式：";
            this.label5.TextAlign = ContentAlignment.MiddleCenter;
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new Point(0x171, 0x204);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new Size(0xf3, 20);
            this.comboBox3.TabIndex = 12;
            this.comboBox3.Text = "选择....";
            this.comboBox3.SelectedIndexChanged += new EventHandler(this.comboBox3_SelectedIndexChanged);
            this.tvImage.AllowDrop = true;
            this.tvImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tvImage.CanClearSelection = true;
            this.tvImage.DrawMode = TreeViewDrawMode.OwnerDrawText;
            this.tvImage.Font = new Font("SimSun", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.tvImage.HideSelection = false;
            this.tvImage.ImageIndex = 0;
            this.tvImage.IsSorted = true;
            this.tvImage.Location = new Point(0x287, 0x1d);
            this.tvImage.Name = "tvImage";
            this.tvImage.NodeSorter = comparer;
            this.tvImage.SelectedImageIndex = 0;
            this.tvImage.ShowLines = false;
            this.tvImage.ShowNodeToolTips = true;
            this.tvImage.ShowRootLines = false;
            this.tvImage.Size = new Size(190, 0x144);
            this.tvImage.TabIndex = 3;
            this.tvImage.DoubleClick += new EventHandler(this.tvImage_DoubleClick);
            this.AutoScaleBaseSize = new Size(6, 14);
            this.BackColor = SystemColors.Control;
            base.ClientSize = new Size(840, 0x247);
            base.Controls.Add(this.pictureBox2);
            base.Controls.Add(this.trackBar2);
            base.Controls.Add(this.trackBar1);
            base.Controls.Add(this.comboBox3);
            base.Controls.Add(this.comboBox2);
            base.Controls.Add(this.comboBox1);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.label6);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.tvImage);
            base.Controls.Add(this.toolStrip1);
            base.Controls.Add(this.pictureBox1);
            base.MaximizeBox = false;
            this.MaximumSize = new Size(0x350, 610);
            this.MinimumSize = new Size(0x350, 610);
            base.Name = "LiveShow";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "拍摄业务照片视频作为证据";
            base.FormClosed += new FormClosedEventHandler(this.LiveShow_FormClosed);
            ((ISupportInitialize) this.pictureBox1).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.trackBar2.EndInit();
            this.trackBar1.EndInit();
            ((ISupportInitialize) this.pictureBox2).EndInit();
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
            }
            List<string> list = new List<string>();
            if ((this.filters.VideoCompressors != null) && (this.filters.VideoCompressors != null))
            {
                for (int j = 0; j < this.filters.VideoCompressors.Count; j++)
                {
                    foreach (string str in this.Comprossors)
                    {
                        if (this.filters.VideoCompressors[j].Name == str)
                        {
                            list.Add(string.Format("{0}&id={1}", str, j));
                        }
                    }
                }
            }
            this.comboBox2.SelectedIndexChanged -= new EventHandler(this.comboBox2_SelectedIndexChanged);
            this.comboBox3.SelectedIndexChanged -= new EventHandler(this.comboBox3_SelectedIndexChanged);
            this.comboBox2.Items.Clear();
            this.comboBox3.Items.Clear();
            if (items != null)
            {
                this.comboBox2.Items.AddRange(items);
            }
            if (list != null)
            {
                this.comboBox3.Items.AddRange(list.ToArray());
            }
            this.comboBox2.SelectedIndexChanged += new EventHandler(this.comboBox2_SelectedIndexChanged);
            this.comboBox3.SelectedIndexChanged += new EventHandler(this.comboBox3_SelectedIndexChanged);
        }

        private void InitVideo()
        {
            this.filters = new Filters();
            if (this.filters.VideoInputDevices.Count > 0)
            {
                this.videoDevice = this.filters.VideoInputDevices[0];
            }
            if (this.filters.AudioInputDevices.Count > 0)
            {
                this.audioDevice = this.filters.AudioInputDevices[0];
            }
            this.capture = new DirectX.Capture.Capture(this.videoDevice, this.audioDevice);
            if (this.filters.VideoCompressors.Count > 0)
            {
                this.videoComprossor = this.filters.VideoCompressors[0];
                this.capture.VideoCompressor = this.videoComprossor;
            }
            this.capture.PreviewWindow = this.pictureBox1;
            this.capture.RenderPreview();
        }

        private void ListFtpDirectory(string ftpDir)
        {
            try
            {
                foreach (FTPfileInfo info in this.ftpClient.ListDirectoryDetail("/" + ftpDir))
                {
                    if ((info.FileType != FTPfileInfo.DirectoryEntryTypes.File) || (!(info.Extension == "jpg") && !(info.Extension == "AVI")))
                    {
                        continue;
                    }
                    if (this.fileMasks != null)
                    {
                        foreach (string str in this.fileMasks)
                        {
                            if (Regex.IsMatch(info.Filename, str.Replace("*", ".*"), RegexOptions.IgnoreCase))
                            {
                                string path = Path.Combine(this.localPath, info.Filename);
                                if (!File.Exists(path))
                                {
                                    this.ftpClient.Download(info, path, true);
                                }
                                this.AddImageNode(info.Filename);
                            }
                        }
                        continue;
                    }
                    this.AddImageNode(info.Filename);
                }
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
                this.ftpClient.FtpCreateDirectory(ftpDir);
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

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += delegate (object sender, DoWorkEventArgs de) {
                LoggingService.Debug("列表FTP...");
                this.ListFtpDirectory(this.ftpdir);
            };
            worker.RunWorkerCompleted += delegate (object sender, RunWorkerCompletedEventArgs re) {
                if (re.Error != null)
                {
                    MessageBox.Show("列表FTP时出现错误！" + re.Error.Message);
                }
            };
            worker.RunWorkerAsync();
            Action method = delegate {
                LoggingService.Debug("初始化视频设备参数...");
                this.InitVideo();
                LoggingService.Debug("初始化压缩等设备参数...");
                this.initMenu();
            };
            base.BeginInvoke(method);
        }

        public DialogResult ShowDialog(string addinToolbarPath, bool removeExistButton, string host, string user, string pwd, string ftpPath, string subftppath, string localPath, string[] fileMasks)
        {
            if (removeExistButton)
            {
                this.toolStrip1.Items.Clear();
            }
            if (!string.IsNullOrEmpty(addinToolbarPath))
            {
                this.toolStrip1.Items.AddRange(ToolbarService.CreateToolStripItems(this, addinToolbarPath));
            }
            this.Initialize(host, user, pwd, ftpPath, subftppath, localPath);
            this.fileMasks = fileMasks;
            return base.ShowDialog();
        }

        private void ShowImage(string file)
        {
            using (FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                this.pictureBox2.Image = Image.FromStream(stream);
            }
        }

        public void TakeAPicture(string filePre)
        {
            RunWorkerCompletedEventHandler handler = null;
            LoggingService.DebugFormatted("视频设备是否正在录像：{0}", new object[] { this.capture.Capturing });
            if (!this.capture.Capturing)
            {
                this.tsBtnCapturePic.Enabled = false;
                try
                {
                    if (string.IsNullOrEmpty(filePre))
                    {
                        this.newWebCamFilename = DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒") + ".jpg";
                    }
                    else
                    {
                        this.newWebCamFilename = this.GetFileNameByFilePre(filePre, ".jpg");
                    }
                    string file = Path.Combine(this.localPath, this.newWebCamFilename);
                    LoggingService.DebugFormatted("将生成照片文件{0}", new object[] { file });
                    this.CreateDirtory(file);
                    this.capture.CaptureFrame(file, ImageFormat.Jpeg);
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.WorkerReportsProgress = true;
                    worker.DoWork += delegate (object sender, DoWorkEventArgs e) {
                        Action method = delegate {
                            this.ShowImage(file);
                            this.AddImageNode(this.newWebCamFilename);
                        };
                        this.Invoke(method);
                        string targetFilename = this.ftpdir + "/" + this.newWebCamFilename;
                        this.ftpClient.Upload(file, targetFilename);
                    };
                    if (handler == null)
                    {
                        handler = delegate (object sender, RunWorkerCompletedEventArgs e) {
                            this.tsBtnCapturePic.Enabled = true;
                            if (e.Error != null)
                            {
                                LoggingService.Error(e.Error);
                                MessageHelper.ShowError("拍照时发生异常", e.Error);
                            }
                        };
                    }
                    worker.RunWorkerCompleted += handler;
                    worker.RunWorkerAsync();
                }
                catch (Exception exception)
                {
                    LoggingService.Error(exception);
                    MessageHelper.ShowError("拍照时发生异常", exception);
                }
            }
            else
            {
                MessageHelper.ShowInfo("正在录制视频，请先停止录制视频！");
            }
        }

        public void TakeAVideo()
        {
            DoWorkEventHandler handler = null;
            Cursor.Current = Cursors.WaitCursor;
            if (!this.capture.Capturing)
            {
                this.newWebCamFilename = DateTime.Now.ToString("yyyy年MM月dd日mm分ss秒") + ".AVI";
                string path = Path.Combine(this.localPath, this.newWebCamFilename);
                this.CreateDirtory(path);
                if (this.comboBox1.SelectedIndex == 0)
                {
                    this.size = new Size(320, 240);
                }
                else
                {
                    this.size = new Size(640, 480);
                }
                this.capture.FrameSize = this.size;
                this.capture.FrameRate = this.trackBar2.Value;
                this.capture.Filename = path;
                try
                {
                    this.capture.Start();
                }
                catch (Exception exception)
                {
                    LoggingService.Error(exception);
                    MessageHelper.ShowInfo(exception.Message);
                }
                this.tsBtnCaptureVideo.Text = "   停止录制   ";
            }
            else
            {
                this.capture.Stop();
                this.capture.RenderPreview();
                this.tsBtnCaptureVideo.Text = "   开始录制   ";
                BackgroundWorker worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                if (handler == null)
                {
                    handler = delegate (object sender, DoWorkEventArgs e) {
                        this.AddImageNode(this.newWebCamFilename);
                        if (this.ftpClient != null)
                        {
                            this.ftpClient.Upload(Path.Combine(this.localPath, this.newWebCamFilename), this.ftpdir + "/" + this.newWebCamFilename);
                        }
                        this.newWebCamFilename = null;
                    };
                }
                worker.DoWork += handler;
                worker.RunWorkerCompleted += delegate (object sender, RunWorkerCompletedEventArgs e) {
                    LoggingService.DebugFormatted("视频保存上传FTP完成。。。", new object[0]);
                    if (e.Error != null)
                    {
                        MessageHelper.ShowError(e.Error.Message, e.Error);
                    }
                };
                worker.RunWorkerAsync();
            }
            Cursor.Current = Cursors.Default;
        }

        private void tsBtnCapturePic_Click(object sender, EventArgs e)
        {
            this.TakeAPicture(string.Empty);
        }

        private void tsBtnCaptureVideo_Click(object sender, EventArgs e)
        {
            this.TakeAVideo();
        }

        private void tsBtnDel_Click(object sender, EventArgs e)
        {
            this.DeleteImage();
        }

        private void tvImage_DoubleClick(object sender, EventArgs e)
        {
            string path = Path.Combine(this.localPath, this.tvImage.SelectedNode.Text);
            if (!File.Exists(path) && (this.ftpClient != null))
            {
                this.ftpClient.Download(this.ftpdir + "/" + this.tvImage.SelectedNode.Text, path, true);
            }
            if (!File.Exists(path))
            {
                MessageHelper.ShowInfo("找不到文件：{0}", path);
            }
            else if (path.EndsWith("AVI"))
            {
                Process.Start(path);
            }
            else
            {
                try
                {
                    this.ShowImage(path);
                }
                catch (Exception)
                {
                    throw;
                }
                this.label1.Text = "拍照时间：" + File.GetCreationTime(path).ToString();
            }
        }
    }
}

