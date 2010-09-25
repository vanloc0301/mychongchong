namespace SkyMap.Net.Workflow.Client.Dialog
{
    using SkyMap.Net.Gui;
    using SkyMap.Net.Gui.Dialogs;
    using SkyMap.Net.Workflow.Engine;
    using System;
    using System.Drawing;
    using System.Runtime.Remoting.Messaging;

    public class AbstractWorkflowDialog : AbstractDialog, IWfDialog
    {
        private SkyMap.Net.Workflow.Engine.WorkItem workItem;

        public AbstractWorkflowDialog()
        {
            try
            {
                if (!base.DesignMode)
                {
                    base.Icon = MessageHelper.GetSystemIcon();
                }
            }
            catch
            {
            }
        }

        private void InitializeComponent()
        {
            base.SuspendLayout();
            base.btOk.Name = "btOk";
            base.btCancel.Name = "btCancel";
            this.AutoScaleBaseSize = new Size(6, 15);
            base.ClientSize = new Size(0x124, 0xf5);
            base.Name = "AbstractWorkflowDialog";
            base.ResumeLayout(false);
        }

        public virtual void SetContextData(ILogicalThreadAffinative contextData)
        {
            throw new NotImplementedException("Not impement set context data method");
        }

        void IWfDialog.Close()
        {
            base.Close();
        }

        public SkyMap.Net.Workflow.Engine.WorkItem WorkItem
        {
            get
            {
                return this.workItem;
            }
            set
            {
                this.workItem = value;
            }
        }
    }
}

