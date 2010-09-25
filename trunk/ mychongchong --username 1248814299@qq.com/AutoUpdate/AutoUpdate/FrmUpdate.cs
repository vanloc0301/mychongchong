namespace AutoUpdate
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Threading;
    using System.Windows.Forms;

    public class FrmUpdate : Form
    {
        private BackgroundWorker[] bgWorkers;
        private Button btnCancel;
        private Button btnFinish;
        private Button btnNext;
        private ColumnHeader chFileName;
        private ColumnHeader chProgress;
        private ColumnHeader chUrl;
        private ColumnHeader chVersion;
        private Container components;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label lbState;
        private LinkLabel linkLabel1;
        private ListView lvUpdateList;
        private string mainAppExe = string.Empty;
        private Panel panel1;
        private Panel panel2;
        private ProgressBar pbDownFile;
        private PictureBox pictureBox1;
        private string tempUpdatePath = string.Empty;
        private List<string[]> updateFileUrls;
        private XmlFiles updaterXmlFiles;
        private string updateUrl = string.Empty;

        public FrmUpdate()
        {
            this.InitializeComponent();
        }

        private void AsyncDownloadAFile(List<string> UpdateFiles, List<string> updateFileUrls, List<int> indexs)
        {
            for (int i = 0; i < UpdateFiles.Count; i++)
            {
                string str = UpdateFiles[i];
                string requestUriString = updateFileUrls[i];
                string path = this.tempUpdatePath + str;
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                WebRequest request = WebRequest.Create(requestUriString);
                request.Timeout = 0x927c0;
                request.Proxy = null;
                WebResponse response = request.GetResponse();
                Stream responseStream = null;
                BufferedStream sourceStream = null;
                FileStream fs = null;
                try
                {
                    responseStream = response.GetResponseStream();
                    this.CreateDirtory(path);
                    if (System.IO.File.Exists(path))
                    {
                        this.lvUpdateList.Items[indexs[i]].SubItems[2].Text = "100%";
                    }
                    else
                    {
                        fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 0x1000, true);
                        byte[] buffer = new byte[0x10000];
                        sourceStream = new BufferedStream(responseStream);
                        int count = sourceStream.Read(buffer, 0, buffer.Length);
                        State state = new State(fs, sourceStream, indexs[i], buffer);
                        fs.BeginWrite(buffer, 0, count, new AsyncCallback(this.EndWrite), state);
                    }
                }
                catch (Exception exception)
                {
                    throw new ApplicationException(string.Format("下载文件{0}失败\r\n", requestUriString, exception.Message), exception);
                }
            }
        }

        private void BakupFile(string source, string dest)
        {
            this.CreateDirtory(dest);
            if (System.IO.File.Exists(source))
            {
                System.IO.File.Copy(source, dest, true);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
            Application.ExitThread();
            Application.Exit();
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                string bakPath = string.Format(@"{0}\updatebak\{1}", Environment.CurrentDirectory, DateTime.Now.ToString("yyMMddHHmmss"));
                this.CopyFile(this.tempUpdatePath, Environment.CurrentDirectory, bakPath);
                Directory.Delete(this.tempUpdatePath, true);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            string xmlFile = Environment.CurrentDirectory + @"\UpdateList.xml";
            try
            {
                XmlFiles files = new XmlFiles(xmlFile);
                string nodeValue = files.GetNodeValue("//EntryPoint");
                string str4 = files.GetNodeValue("//Location");
                string str5 = string.Empty;
                try
                {
                    str5 = files.GetNodeValue("//BeforeExecutes");
                }
                catch
                {
                }
                foreach (string str6 in str5.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (System.IO.File.Exists(str6))
                    {
                        Process.Start(str6);
                    }
                    else
                    {
                        string path = string.Format(@"{0}\{1}", Environment.CurrentDirectory, str6);
                        if (System.IO.File.Exists(path))
                        {
                            Process.Start(path);
                        }
                        else
                        {
                            MessageBox.Show("没有找到升级完成后要自动执行的程序:{0}", path);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(nodeValue))
                {
                    if (!string.IsNullOrEmpty(str4))
                    {
                        nodeValue = string.Format(@"{0}\{1}\{2}", Environment.CurrentDirectory, str4, nodeValue);
                    }
                    if (System.IO.File.Exists(nodeValue))
                    {
                        Process.Start(nodeValue);
                    }
                    else
                    {
                        MessageBox.Show(string.Format("没找到要升级后需要执行的主程序:{0}", nodeValue));
                    }
                }
            }
            catch (Exception exception2)
            {
                MessageBox.Show("出错!" + exception2.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            finally
            {
                base.Close();
                base.Dispose();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            this.btnNext.Visible = false;
            Thread thread = new Thread(new ThreadStart(this.DownUpdateFile));
            thread.IsBackground = true;
            thread.Start();
        }

        private bool CheckHasNewVersion()
        {
            string xmlFile = Environment.CurrentDirectory + @"\UpdateList.xml";
            string path = string.Empty;
            try
            {
                this.updaterXmlFiles = new XmlFiles(xmlFile);
            }
            catch
            {
                MessageBox.Show("配置文件出错!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
            }
            this.updateUrl = this.updaterXmlFiles.GetNodeValue("//Url");
            AppUpdater updater = new AppUpdater();
            try
            {
                this.tempUpdatePath = string.Format(@"{0}\{1}_{2}", Environment.GetEnvironmentVariable("Temp"), this.updaterXmlFiles.FindNode("//Application").Attributes["applicationId"].Value, DateTime.Now.ToString("yyyyMMddHHmmss"));
                updater.DownAutoUpdateFile(this.tempUpdatePath, string.Empty, this.updateUrl);
            }
			catch
            {
                MessageBox.Show("与服务器连接失败,操作超时!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return false;
            }
            path = this.tempUpdatePath + @"\UpdateList.xml";
            if (!System.IO.File.Exists(path))
            {
                return false;
            }
            return (updater.CheckForUpdate(this.tempUpdatePath, path, xmlFile, out this.updateFileUrls) > 0);
        }

        public void CopyFile(string sourcePath, string objPath, string bakPath)
        {
            if (!Directory.Exists(objPath))
            {
                Directory.CreateDirectory(objPath);
            }
            string[] files = Directory.GetFiles(sourcePath);
            for (int i = 0; i < files.Length; i++)
            {
                string[] strArray2 = files[i].Split(new char[] { '\\' });
                string path = objPath + @"\" + strArray2[strArray2.Length - 1];
                if (System.IO.File.Exists(path))
                {
                    if ((System.IO.File.GetAttributes(path) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    {
                        System.IO.File.SetAttributes(path, FileAttributes.Normal);
                    }
                    try
                    {
                        string dest = bakPath + @"\" + strArray2[strArray2.Length - 1];
                        this.BakupFile(path, dest);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(string.Format("备份文件{0}至{1}时发生错误：\r\n{1}\r\n不影响升级完成，但是可能不能恢复至原来版本", path, bakPath, exception.Message));
                    }
                }
                System.IO.File.Copy(files[i], objPath + @"\" + strArray2[strArray2.Length - 1], true);
            }
            string[] directories = Directory.GetDirectories(sourcePath);
            for (int j = 0; j < directories.Length; j++)
            {
                string[] strArray4 = directories[j].Split(new char[] { '\\' });
                this.CopyFile(directories[j], objPath + @"\" + strArray4[strArray4.Length - 1], bakPath + @"\" + strArray4[strArray4.Length - 1]);
            }
        }

        private void CreateDirtory(string path)
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

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DownUpdateFile()
        {
            this.Cursor = Cursors.WaitCursor;
            this.KillProcess();
            new WebClient();
            this.pbDownFile.Maximum = this.updateFileUrls.Count;
            this.lbState.Text = "正在下载更新文件,请稍后...";
            this.bgWorkers = new BackgroundWorker[5];
            for (int i = 0; i < 5; i++)
            {
                DoWorkEventHandler handler = null;
                List<string> UpdateFiles = null;
                List<string> bgUpdateFileUrls = null;
                List<int> indexs = null;
                if (this.TryGetBgWorkData(this.updateFileUrls, i, ref UpdateFiles, ref bgUpdateFileUrls, ref indexs))
                {
                    this.bgWorkers[i] = new BackgroundWorker();
                    this.bgWorkers[i].WorkerReportsProgress = true;
                    if (handler == null)
                    {
                        handler = delegate (object sender, DoWorkEventArgs e) {
                            this.AsyncDownloadAFile(UpdateFiles, bgUpdateFileUrls, indexs);
                        };
                    }
                    this.bgWorkers[i].DoWork += handler;
                    this.bgWorkers[i].RunWorkerCompleted += delegate (object sender, RunWorkerCompletedEventArgs e) {
                        if (e.Error != null)
                        {
                            MessageBox.Show(e.Error.Message.ToString() + e.Error.StackTrace, "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        }
                    };
                }
            }
            foreach (BackgroundWorker worker in this.bgWorkers)
            {
                if (worker != null)
                {
                    worker.RunWorkerAsync();
                }
            }
        }

        private void EndWrite(IAsyncResult ar)
        {
            State asyncState = (State) ar.AsyncState;
            BufferedStream sourceStream = asyncState.sourceStream;
            FileStream fs = asyncState.fs;
            byte[] buffer = asyncState.buffer;
            if (sourceStream.CanRead)
            {
                int count = sourceStream.Read(buffer, 0, buffer.Length);
                if (count > 0)
                {
                    base.BeginInvoke(new AutoUpdate.NotifyListViewProgress(this.NotifyListViewProgress), new object[] { asyncState.index, string.Format("{0}kb", fs.Length / 0x400L) });
                    fs.BeginWrite(buffer, 0, count, new AsyncCallback(this.EndWrite), asyncState);
                }
                else
                {
                    try
                    {
                        sourceStream.Close();
                        fs.Close();
                    }
                    catch
                    {
                    }
                    base.BeginInvoke(new AutoUpdate.NotifyListViewProgress(this.NotifyListViewProgress), new object[] { asyncState.index, "100%" });
                }
            }
            else
            {
                try
                {
                    sourceStream.Close();
                    fs.Close();
                }
                catch
                {
                }
                base.BeginInvoke(new AutoUpdate.NotifyListViewProgress(this.NotifyListViewProgress), new object[] { asyncState.index, "100%" });
            }
        }

        private void FrmUpdate_Load(object sender, EventArgs e)
        {
            this.panel2.Visible = false;
            this.btnFinish.Visible = false;
            for (int i = 0; i < this.updateFileUrls.Count; i++)
            {
                string[] items = this.updateFileUrls[i];
                this.lvUpdateList.Items.Add(new ListViewItem(items));
            }
        }

        private void FrmUpdate_Shown(object sender, EventArgs e)
        {
            if (!this.IsNoManAutoUpdate())
            {
                base.TopMost = true;
                base.Focus();
            }
            else
            {
                this.btnNext_Click(this.btnNext, null);
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmUpdate));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lvUpdateList = new System.Windows.Forms.ListView();
            this.chFileName = new System.Windows.Forms.ColumnHeader();
            this.chVersion = new System.Windows.Forms.ColumnHeader();
            this.chProgress = new System.Windows.Forms.ColumnHeader();
            this.chUrl = new System.Windows.Forms.ColumnHeader();
            this.pbDownFile = new System.Windows.Forms.ProgressBar();
            this.lbState = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnFinish = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(8, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(106, 240);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.lvUpdateList);
            this.panel1.Controls.Add(this.pbDownFile);
            this.panel1.Controls.Add(this.lbState);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Location = new System.Drawing.Point(120, 8);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(280, 240);
            this.panel1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 16);
            this.label1.TabIndex = 9;
            this.label1.Text = "以下为更新文件列表";
            // 
            // groupBox2
            // 
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 238);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(280, 2);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "groupBox2";
            // 
            // lvUpdateList
            // 
            this.lvUpdateList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chFileName,
            this.chVersion,
            this.chProgress,
            this.chUrl});
            this.lvUpdateList.Location = new System.Drawing.Point(3, 48);
            this.lvUpdateList.Name = "lvUpdateList";
            this.lvUpdateList.Size = new System.Drawing.Size(272, 120);
            this.lvUpdateList.TabIndex = 6;
            this.lvUpdateList.UseCompatibleStateImageBehavior = false;
            this.lvUpdateList.View = System.Windows.Forms.View.Details;
            // 
            // chFileName
            // 
            this.chFileName.Text = "组件名";
            this.chFileName.Width = 140;
            // 
            // chVersion
            // 
            this.chVersion.Text = "版本号";
            this.chVersion.Width = 70;
            // 
            // chProgress
            // 
            this.chProgress.Text = "进度";
            // 
            // chUrl
            // 
            this.chUrl.Text = "Url";
            this.chUrl.Width = 0;
            // 
            // pbDownFile
            // 
            this.pbDownFile.Location = new System.Drawing.Point(3, 200);
            this.pbDownFile.Name = "pbDownFile";
            this.pbDownFile.Size = new System.Drawing.Size(274, 17);
            this.pbDownFile.TabIndex = 5;
            // 
            // lbState
            // 
            this.lbState.Location = new System.Drawing.Point(3, 176);
            this.lbState.Name = "lbState";
            this.lbState.Size = new System.Drawing.Size(240, 16);
            this.lbState.TabIndex = 4;
            this.lbState.Text = "点击“下一步”开始更新文件";
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(0, 32);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(280, 8);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(224, 264);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(80, 24);
            this.btnNext.TabIndex = 3;
            this.btnNext.Text = "下一步(&N)>";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(312, 264);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 24);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "取消(&C)";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.linkLabel1);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Controls.Add(this.groupBox4);
            this.panel2.Location = new System.Drawing.Point(8, 264);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(112, 24);
            this.panel2.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(144, 184);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 16);
            this.label4.TabIndex = 13;
            this.label4.Text = "置信";
            // 
            // linkLabel1
            // 
            this.linkLabel1.Location = new System.Drawing.Point(110, 208);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(160, 16);
            this.linkLabel1.TabIndex = 12;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "http://www.skymapsoft.com";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(24, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(184, 16);
            this.label3.TabIndex = 11;
            this.label3.Text = "欢迎以后继续关注我们的产品。";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(24, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(232, 48);
            this.label2.TabIndex = 10;
            this.label2.Text = "     程序更新完成,如果程序更新期间被关闭,点击\"完成\"自动更新程序会自动重新启动系统。";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(16, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(136, 24);
            this.label5.TabIndex = 9;
            this.label5.Text = "感谢使用在线升级";
            // 
            // groupBox3
            // 
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox3.Location = new System.Drawing.Point(0, 22);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(112, 2);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "groupBox2";
            // 
            // groupBox4
            // 
            this.groupBox4.Location = new System.Drawing.Point(0, 32);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(280, 8);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            // 
            // btnFinish
            // 
            this.btnFinish.Location = new System.Drawing.Point(136, 264);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(80, 24);
            this.btnFinish.TabIndex = 3;
            this.btnFinish.Text = "完成(&F)";
            this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
            // 
            // FrmUpdate
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(424, 301);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnFinish);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmUpdate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "自动更新";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FrmUpdate_Load);
            this.Shown += new System.EventHandler(this.FrmUpdate_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void InvalidateControl()
        {
            this.panel2.Location = this.panel1.Location;
            this.panel2.Size = this.panel1.Size;
            this.panel1.Visible = false;
            this.panel2.Visible = true;
            this.btnNext.Visible = false;
            this.btnCancel.Visible = false;
            this.btnFinish.Location = this.btnCancel.Location;
            this.btnFinish.Visible = true;
        }

        private bool IsMainAppRun()
        {
            string nodeValue = this.updaterXmlFiles.GetNodeValue("//EntryPoint");
            bool flag = false;
            foreach (Process process in Process.GetProcesses())
            {
                if ((process.ProcessName.ToLower() + ".exe") == nodeValue.ToLower())
                {
                    flag = true;
                }
            }
            return flag;
        }

        private bool IsNoManAutoUpdate()
        {
            return System.IO.File.Exists(Path.Combine(Environment.CurrentDirectory, "nomanautoupate.nau"));
        }

        private void KillProcess()
        {
            try
            {
                this.mainAppExe = string.Empty;
                try
                {
                    this.mainAppExe = this.updaterXmlFiles.GetNodeValue("//EntryPoint");
                }
                catch
                {
                }
                string nodeValue = string.Empty;
                try
                {
                    nodeValue = this.updaterXmlFiles.GetNodeValue("//KillProcesses");
                }
                catch
                {
                }
                string[] strArray = (this.mainAppExe + "," + nodeValue).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (Process process in Process.GetProcesses())
                {
                    foreach (string str2 in strArray)
                    {
                        if ((process.ProcessName.ToLower() + ".exe") == str2.ToLower())
                        {
                            for (int i = 0; i < process.Threads.Count; i++)
                            {
                                process.Threads[i].Dispose();
                            }
                            process.Kill();
                        }
                    }
                }
                string str3 = string.Empty;
                try
                {
                    str3 = this.updaterXmlFiles.GetNodeValue("//BeforeUpdateExecute");
                }
                catch
                {
                }
                if (!string.IsNullOrEmpty(str3))
                {
                    foreach (string str4 in str3.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        Process.Start(str4).WaitForExit();
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("执行升级前任务（如关闭主程序）等出错：" + exception.Message, "错误");
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(this.linkLabel1.Text);
        }

        [STAThread]
        private static void Main(string[] args)
        {
            string location = Assembly.GetExecutingAssembly().Location;
            Environment.CurrentDirectory = Path.GetDirectoryName(location);
            if (!location.EndsWith("temp.exe"))
            {
                string destFileName = location.Replace(".exe", "temp.exe");
                foreach (Process process in Process.GetProcesses())
                {
                    if (destFileName.EndsWith(process.ProcessName.ToLower() + ".exe"))
                    {
                        for (int i = 0; i < process.Threads.Count; i++)
                        {
                            process.Threads[i].Dispose();
                        }
                        process.Kill();
                    }
                }
                try
                {
                    System.IO.File.Copy(location, destFileName, true);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                    return;
                }
                if (System.IO.File.Exists(destFileName))
                {
                    string arguments = "";
                    foreach (string str4 in args)
                    {
                        object obj2 = arguments;
                        arguments = string.Concat(new object[] { obj2, '"', str4, '"', " " });
                    }
                    arguments = arguments.Trim();
                    Process.Start(destFileName, arguments);
                    return;
                }
            }
            Control.CheckForIllegalCrossThreadCalls = false;
            FrmUpdate mainForm = new FrmUpdate();
            if (mainForm.CheckHasNewVersion())
            {
                Application.Run(mainForm);
            }
        }

        private void NotifyListViewProgress(int index, string progress)
        {
            this.lvUpdateList.Items[index].EnsureVisible();
            this.lvUpdateList.Items[index].SubItems[2].Text = progress;
            if (progress == "100%")
            {
                this.pbDownFile.Value++;
            }
            if (this.pbDownFile.Value == this.pbDownFile.Maximum)
            {
                if (!this.IsNoManAutoUpdate())
                {
                    this.InvalidateControl();
                }
                else
                {
                    this.btnFinish_Click(this.btnFinish, null);
                }
                this.Cursor = Cursors.Default;
            }
        }

        private bool TryGetBgWorkData(List<string[]> updateFileUrls, int k, ref List<string> UpdateFiles, ref List<string> bgUpdateFileUrls, ref List<int> indexs)
        {
            if (k > updateFileUrls.Count)
            {
                return false;
            }
            int item = k;
            UpdateFiles = new List<string>();
            bgUpdateFileUrls = new List<string>();
            indexs = new List<int>();
            while (item < updateFileUrls.Count)
            {
                UpdateFiles.Add(updateFileUrls[item][0]);
                bgUpdateFileUrls.Add(((string) updateFileUrls[item][3]) + ((string) updateFileUrls[item][4]));
                indexs.Add(item);
                item += 5;
            }
            return true;
        }

        private class State
        {
            public byte[] buffer;
            public FileStream fs;
            public int index;
            public BufferedStream sourceStream;

            public State(FileStream fs, BufferedStream sourceStream, int index, byte[] buffer)
            {
                this.fs = fs;
                this.sourceStream = sourceStream;
                this.index = index;
                this.buffer = buffer;
            }
        }
    }
}

