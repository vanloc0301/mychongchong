namespace SkyMap.Net.Workflow.Client.Contents
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Workflow.Client.Box;
    using System;
    using System.Windows.Forms;

    public class BoxViewContext : AbstractViewContent
    {
        private IBox box;

        public BoxViewContext()
        {
        }

        public BoxViewContext(string boxName)
        {
            this.box = BoxHelper.GetBox(boxName);
        }

        public override void Load(string fileName)
        {
        }

        public void RefreshData()
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("开始载入 {0} 数据", new object[] { this.box.BoxName });
            }
            if (this.box != null)
            {
                this.box.RefreshData();
            }
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("完成 {0} 数据载入", new object[] { this.box.BoxName });
            }
        }

        public override void Save()
        {
        }

        public void SetBox(string boxName)
        {
        }

        public override System.Windows.Forms.Control Control
        {
            get
            {
                return (this.box as System.Windows.Forms.Control);
            }
        }

        public override bool IsDirty
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        public override string TitleName
        {
            get
            {
                return this.box.BoxName;
            }
            set
            {
            }
        }
    }
}

