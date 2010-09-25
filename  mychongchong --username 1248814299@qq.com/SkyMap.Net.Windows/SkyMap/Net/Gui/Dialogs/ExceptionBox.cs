namespace SkyMap.Net.Gui.Dialogs
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Reflection;
    using System.Resources;
    using System.Threading;
    using System.Windows.Forms;

    public class ExceptionBox : Form
    {
        private Button closeButton;
        private Button continueButton;
        private CheckBox copyErrorCheckBox;
        private TextBox exceptionTextBox;
        private Exception exceptionThrown;
        private Label label;
        private Label label2;
        private Label label3;
        private string message;
        private PictureBox pictureBox;
        private Button reportButton;

        public ExceptionBox(Exception e, string message, bool mustTerminate)
        {
            base.TopMost = true;
            this.exceptionThrown = e;
            this.message = message;
            this.InitializeComponent();
            if (mustTerminate)
            {
                this.closeButton.Visible = false;
                this.continueButton.Text = this.closeButton.Text;
                this.continueButton.Left -= this.closeButton.Width - this.continueButton.Width;
                this.continueButton.Width = this.closeButton.Width;
            }
            try
            {
                this.Translate(this);
            }
            catch
            {
            }
            this.exceptionTextBox.Text = this.getClipboardString();
            try
            {
                ResourceManager manager = new ResourceManager("Resources.BitmapResources", Assembly.GetEntryAssembly());
                this.pictureBox.Image = (Bitmap) manager.GetObject("ErrorReport");
            }
            catch
            {
            }
        }

        private void buttonClick(object sender, EventArgs e)
        {
            this.CopyInfoToClipboard();
            StartUrl("http://www.skymapsoft.com/");
        }

        private void CloseButtonClick(object sender, EventArgs e)
        {
            if (MessageHelper.ShowYesNoCancelInfo("真的要关闭退出系统吗?") == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void continueButtonClick(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Ignore;
            base.Close();
        }

        private void CopyInfoToClipboard()
        {
            ThreadStart start = null;
            if (this.copyErrorCheckBox.Checked)
            {
                if (Application.OleRequired() == ApartmentState.STA)
                {
                    ClipboardWrapper.SetText(this.getClipboardString());
                }
                else
                {
                    if (start == null)
                    {
                        start = delegate {
                            ClipboardWrapper.SetText(this.getClipboardString());
                        };
                    }
                    Thread thread = new Thread(start);
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.Start();
                }
            }
        }

        private string getClipboardString()
        {
            object obj2;
            string str = "";
            str = (str + ".NET 版本         : " + Environment.Version.ToString() + Environment.NewLine) + "操作系统版本      : " + Environment.OSVersion.ToString() + Environment.NewLine;
            string name = null;
            try
            {
                name = CultureInfo.CurrentCulture.Name;
                string str4 = str;
                str = str4 + "系统语言          : " + CultureInfo.CurrentCulture.EnglishName + " (" + name + ")" + Environment.NewLine;
            }
            catch
            {
            }
            try
            {
                if (!((name != null) && name.StartsWith(ResourceService.Language)))
                {
                    str = str + "界面语言  : " + ResourceService.Language + Environment.NewLine;
                }
            }
            catch
            {
            }
            if (IntPtr.Size != 4)
            {
                obj2 = str;
                str = string.Concat(new object[] { obj2, "Running as ", IntPtr.Size * 8, " bit process", Environment.NewLine });
            }
            try
            {
                if (SystemInformation.TerminalServerSession)
                {
                    str = str + "终端服务会话" + Environment.NewLine;
                }
                if (SystemInformation.BootMode != BootMode.Normal)
                {
                    obj2 = str;
                    str = string.Concat(new object[] { obj2, "启动模式            : ", SystemInformation.BootMode, Environment.NewLine });
                }
            }
            catch
            {
            }
            obj2 = str;
            str = string.Concat(new object[] { obj2, "内容空量  : ", Environment.WorkingSet / 0x400L, "kb", Environment.NewLine });
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            obj2 = str;
            str = string.Concat(new object[] { obj2, "SMOA 版本 : ", version.Major, ".", version.Minor, ".", version.Build, ".", version.Revision, Environment.NewLine }) + Environment.NewLine;
            if (this.message != null)
            {
                str = str + this.message + Environment.NewLine;
            }
            return ((str + "异常发生在: " + Environment.NewLine) + this.exceptionThrown.ToString());
        }

        private void InitializeComponent()
        {
            this.pictureBox = new PictureBox();
            base.ClientSize = new Size(0x2b0, 0x1c5);
            this.closeButton = new Button();
            this.closeButton.Location = new Point(0x1c6, 0x1a8);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new Size(140, 0x17);
            this.closeButton.TabIndex = 5;
            this.closeButton.Text = "终止";
            this.closeButton.Click += new EventHandler(this.CloseButtonClick);
            this.label3 = new Label();
            this.label3.Location = new Point(0xe8, 0x98);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x1c0, 0x17);
            this.label3.TabIndex = 9;
            this.label3.Text = "感谢您为完善本系统所做的努力!";
            this.label2 = new Label();
            this.label2.Location = new Point(0xe8, 0x40);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x1c0, 80);
            this.label2.TabIndex = 8;
            this.label2.Text = "如何快速成有效的报告错误: 我们在公司的网站上建立了一个相应论坛，你只需要将下面的错误提示在论坛里发表，注上你的紧急的程度，我们的技术人员就会与你联系,并再终解决它的.";
            this.label = new Label();
            this.label.Location = new Point(0xe8, 8);
            this.label.Name = "label";
            this.label.Size = new Size(0x1c0, 0x30);
            this.label.TabIndex = 6;
            this.label.Text = "系统发生了一个未处理的异常. 它是没有被预料到的，'d所以我们希望你能向我们的开发团队报告这个错误，这将有助于我们进一步改善系统质量";
            this.continueButton = new Button();
            this.continueButton.Location = new Point(600, 0x1a8);
            this.continueButton.Name = "continueButton";
            this.continueButton.Size = new Size(0x4b, 0x17);
            this.continueButton.TabIndex = 6;
            this.continueButton.Text = "继续";
            this.continueButton.Click += new EventHandler(this.continueButtonClick);
            this.reportButton = new Button();
            this.reportButton.Location = new Point(0xe8, 0x1a8);
            this.reportButton.Name = "reportButton";
            this.reportButton.Size = new Size(0xd8, 0x17);
            this.reportButton.TabIndex = 4;
            this.reportButton.Text = "报告错误给置信";
            this.reportButton.Click += new EventHandler(this.buttonClick);
            this.copyErrorCheckBox = new CheckBox();
            this.copyErrorCheckBox.Checked = true;
            this.copyErrorCheckBox.CheckState = CheckState.Checked;
            this.copyErrorCheckBox.Location = new Point(0xe8, 0x170);
            this.copyErrorCheckBox.Name = "copyErrorCheckBox";
            this.copyErrorCheckBox.Size = new Size(440, 0x18);
            this.copyErrorCheckBox.TabIndex = 2;
            this.copyErrorCheckBox.Text = "复制错误消息到剪切板";
            this.exceptionTextBox = new TextBox();
            this.exceptionTextBox.Location = new Point(0xe8, 0xb0);
            this.exceptionTextBox.Multiline = true;
            this.exceptionTextBox.Name = "exceptionTextBox";
            this.exceptionTextBox.ReadOnly = true;
            this.exceptionTextBox.ScrollBars = ScrollBars.Vertical;
            this.exceptionTextBox.Size = new Size(0x1c0, 0xb8);
            this.exceptionTextBox.TabIndex = 1;
            this.exceptionTextBox.Text = "textBoxExceptionText";
            this.pictureBox = new PictureBox();
            ((ISupportInitialize) this.pictureBox).BeginInit();
            this.pictureBox.Location = new Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new Size(0xe0, 0x1d0);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            ((ISupportInitialize) this.pictureBox).EndInit();
            base.Controls.Add(this.closeButton);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label);
            base.Controls.Add(this.continueButton);
            base.Controls.Add(this.reportButton);
            base.Controls.Add(this.copyErrorCheckBox);
            base.Controls.Add(this.exceptionTextBox);
            base.Controls.Add(this.pictureBox);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "ExceptionBox";
            this.Text = "未处理的异常";
            base.SuspendLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private static void StartUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch (Exception exception)
            {
                LoggingService.Warn("Cannot start " + url, exception);
            }
        }

        private void Translate(Control ctl)
        {
            ctl.Text = SkyMap.Net.Core.StringParser.Parse(ctl.Text);
            foreach (Control control in ctl.Controls)
            {
                this.Translate(control);
            }
        }
    }
}

