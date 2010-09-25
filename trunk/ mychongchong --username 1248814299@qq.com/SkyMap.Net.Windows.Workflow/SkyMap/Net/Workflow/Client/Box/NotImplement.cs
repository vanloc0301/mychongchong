namespace SkyMap.Net.Workflow.Client.Box
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Workflow.Client.Config;
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    public class NotImplement : AbstractBox
    {
        private IContainer components = null;

        public NotImplement()
        {
            this.InitializeComponent();
            this.BackgroundImage = ResourceService.GetBitmap("Workflow.Box.NotImplement");
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
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            base.AutoScaleMode = AutoScaleMode.Font;
        }

        public override void RefreshData()
        {
        }
    }
}

