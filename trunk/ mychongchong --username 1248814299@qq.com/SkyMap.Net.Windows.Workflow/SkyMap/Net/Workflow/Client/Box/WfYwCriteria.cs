namespace SkyMap.Net.Workflow.Client.Box
{
    using SkyMap.Net.Workflow.Client.Commands;
    using SkyMap.Net.Workflow.Client.Config;
    using System;
    using System.ComponentModel;
    using System.Drawing;

    public class WfYwCriteria : WfBox
    {
        private IContainer components = null;
        private OpenViewForQueryFocusCommand openViewForQueryFocusCommand;

        public WfYwCriteria()
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

        public override void Init(CBoxConfig boxConfig)
        {
            base.Init(boxConfig);
            base.BarStatusUpdate();
        }

        private void InitializeComponent()
        {
            base.SuspendLayout();
            base.Name = "WfYwCriteria";
            base.Size = new Size(0x278, 0x1b0);
            base.ResumeLayout(false);
        }

        public override void RefreshData()
        {
        }

        protected override void SecurityUtil_CurrentPrincipalChanged(object sender, EventArgs e)
        {
            base.grid.DataSource = null;
        }

        public override DefaultOpenViewCommand OpenViewCommand
        {
            get
            {
                if (this.openViewForQueryFocusCommand == null)
                {
                    this.openViewForQueryFocusCommand = new OpenViewForQueryFocusCommand();
                    this.openViewForQueryFocusCommand.Owner = this;
                }
                return this.openViewForQueryFocusCommand;
            }
        }
    }
}

