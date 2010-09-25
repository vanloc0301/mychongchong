namespace SkyMap.Net.Workflow.Client.Box
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui.Components;
    using SkyMap.Net.Security;
    using SkyMap.Net.Workflow.Client.Config;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public class AbstractBox : SmBarUserControl, IBox
    {
        protected string _toolbarPath;
        protected BackgroundWorker bgInitWorker;
        private string boxName;
        private Container components = null;
        private bool initialized;
        protected bool waitInitializeAsyncCompletedToRefreshData;

        public AbstractBox()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public virtual void Init(CBoxConfig boxConfig)
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("开始异步载入工作流箱：{0}", new object[] { boxConfig.Name });
            }
            this._toolbarPath = boxConfig.ToolbarPath;
            this.bgInitWorker = new BackgroundWorker();
            this.bgInitWorker.DoWork += new DoWorkEventHandler(this.InitializeAsync);
            this.bgInitWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.InitializeAsyncCompleted);
            this.bgInitWorker.RunWorkerAsync(boxConfig);
            this.ContextMenuStrip = base.CreateContextMenu("/Workflow/Box/ContextMenu/");
        }

        protected virtual void InitializeAsync(object sender, DoWorkEventArgs e)
        {
        }

        protected virtual void InitializeAsyncCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                LoggingService.Error("异步初始化工作流箱时发生错误：", e.Error);
            }
            else
            {
                this.CreateToolbar();
                SecurityUtil.CurrentPrincipalChanged += new EventHandler(this.SecurityUtil_CurrentPrincipalChanged);
                this.initialized = true;
                if (this.waitInitializeAsyncCompletedToRefreshData)
                {
                    this.RefreshData();
                    this.waitInitializeAsyncCompletedToRefreshData = false;
                }
            }
        }

        private void InitializeComponent()
        {
            this.bgInitWorker = new BackgroundWorker();
            base.SuspendLayout();
            base.Name = "AbstractBox";
            base.Size = new Size(0x144, 0x138);
            base.ResumeLayout(false);
        }

        public virtual void RefreshData()
        {
            if (this.initialized)
            {
                base.BarStatusUpdate();
            }
            else
            {
                this.waitInitializeAsyncCompletedToRefreshData = true;
            }
        }

        protected virtual void SecurityUtil_CurrentPrincipalChanged(object sender, EventArgs e)
        {
            this.RefreshData();
        }

        public string BoxName
        {
            get
            {
                return this.boxName;
            }
            set
            {
                this.boxName = value;
            }
        }

        public bool Initialized
        {
            get
            {
                return this.initialized;
            }
        }

        protected override string ToolbarPath
        {
            get
            {
                return this._toolbarPath;
            }
        }
    }
}

