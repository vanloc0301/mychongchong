namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class SmStatusBar : StatusStrip, IProgressMonitor
    {
        private bool cancelEnabled;
        private ToolStripStatusLabel companyStatusBarPanel = new ToolStripStatusLabel();
        private string currentMessage;
        private ToolStripStatusLabel cursorStatusBarPanel = new ToolStripStatusLabel();
        private ToolStripStatusLabel jobNamePanel = new ToolStripStatusLabel();
        private ToolStripStatusLabel loginInfoStatusBarPanel = new ToolStripStatusLabel();
        private ToolStripStatusLabel modeStatusBarPanel = new ToolStripStatusLabel();
        private ToolStripStatusLabel springLabel = new ToolStripStatusLabel();
        private ToolStripProgressBar statusProgressBar = new ToolStripProgressBar();
        private string taskName;
        private Timer timer;
        private int totalWork;
        private ToolStripStatusLabel txtStatusBarPanel = new ToolStripStatusLabel();
        private int workDone;

        public SmStatusBar()
        {
            this.springLabel.Spring = true;
            this.cursorStatusBarPanel.AutoSize = false;
            this.cursorStatusBarPanel.Width = 150;
            this.modeStatusBarPanel.AutoSize = false;
            this.modeStatusBarPanel.Width = 0x19;
            this.statusProgressBar.Visible = false;
            this.statusProgressBar.Width = 100;
            this.loginInfoStatusBarPanel.AutoSize = false;
            this.loginInfoStatusBarPanel.Width = 400;
            this.companyStatusBarPanel.AutoSize = false;
            this.companyStatusBarPanel.Width = 300;
            this.companyStatusBarPanel.Text = string.Format("{0}({1})-{2}", ResourceService.GetString("XML.CompanyName"), AutoUpdateHepler.GetLastVersion(), PropertyService.Get<string>("DefaultServer", string.Empty));
            this.Items.AddRange(new ToolStripItem[] { this.txtStatusBarPanel, this.springLabel, this.jobNamePanel, this.statusProgressBar, this.loginInfoStatusBarPanel, this.companyStatusBarPanel });
        }

        public void BeginTask(string name, int totalWork)
        {
            this.taskName = name;
            this.totalWork = totalWork;
            if (base.IsHandleCreated)
            {
                base.BeginInvoke(new MethodInvoker(this.MakeVisible));
            }
        }

        public void Done()
        {
            this.taskName = null;
            if (base.IsHandleCreated)
            {
                base.BeginInvoke(new MethodInvoker(this.MakeInvisible));
            }
        }

        private void MakeInvisible()
        {
            if ((this.timer != null) && this.timer.Enabled)
            {
                this.timer.Stop();
                this.timer.Enabled = false;
            }
            this.jobNamePanel.Text = "";
            this.statusProgressBar.Visible = false;
        }

        private void MakeVisible()
        {
            this.statusProgressBar.Value = 0;
            if (this.totalWork > 0)
            {
                this.statusProgressBar.Maximum = this.totalWork;
            }
            else
            {
                this.statusProgressBar.Maximum = 100;
                if (this.timer == null)
                {
                    this.timer = new Timer();
                    this.timer.Interval = 300;
                    this.timer.Tick += new EventHandler(this.timer_Tick);
                }
                this.timer.Enabled = true;
                this.timer.Start();
            }
            this.jobNamePanel.Text = this.taskName;
            this.jobNamePanel.Visible = true;
            this.statusProgressBar.Visible = true;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.UpdateText();
        }

        public void SetLoginInfo(string loginInfo)
        {
            this.loginInfoStatusBarPanel.Text = loginInfo;
        }

        public void SetMessage(string message)
        {
            this.currentMessage = message;
            if (base.IsHandleCreated)
            {
                base.BeginInvoke(new MethodInvoker(this.UpdateText));
            }
        }

        public void SetMessage(Image image, string message)
        {
            this.SetMessage(message);
        }

        public void SetOpMessage(string message)
        {
            this.springLabel.Text = message;
        }

        private void SetTaskName()
        {
            this.jobNamePanel.Text = this.taskName;
        }

        private void SetWorkDone()
        {
            if (this.workDone < this.statusProgressBar.Maximum)
            {
                this.statusProgressBar.Value = this.workDone;
            }
        }

        public void ShowErrorMessage(string message)
        {
            this.SetMessage("Error : " + message);
        }

        public void ShowErrorMessage(Image image, string message)
        {
            this.SetMessage(image, "Error : " + message);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (this.statusProgressBar.Value < this.statusProgressBar.Maximum)
            {
                this.statusProgressBar.PerformStep();
            }
            else
            {
                this.statusProgressBar.Value = 0;
            }
        }

        private void UpdateText()
        {
            this.txtStatusBarPanel.Text = this.currentMessage;
        }

        public bool CancelEnabled
        {
            get
            {
                return this.cancelEnabled;
            }
            set
            {
                this.cancelEnabled = value;
            }
        }

        public ToolStripStatusLabel CursorStatusBarPanel
        {
            get
            {
                return this.cursorStatusBarPanel;
            }
        }

        public ToolStripStatusLabel ModeStatusBarPanel
        {
            get
            {
                return this.modeStatusBarPanel;
            }
        }

        public string TaskName
        {
            get
            {
                return this.taskName;
            }
            set
            {
                if (this.taskName != value)
                {
                    this.taskName = value;
                    base.BeginInvoke(new MethodInvoker(this.SetTaskName));
                }
            }
        }

        public int WorkDone
        {
            get
            {
                return this.workDone;
            }
            set
            {
                if (this.workDone != value)
                {
                    this.workDone = value;
                    base.BeginInvoke(new MethodInvoker(this.SetWorkDone));
                }
            }
        }
    }
}

