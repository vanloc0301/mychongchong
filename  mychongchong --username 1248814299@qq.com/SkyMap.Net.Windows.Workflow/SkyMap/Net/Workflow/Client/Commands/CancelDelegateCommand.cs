namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Workflow.Client.Box;
    using SkyMap.Net.Workflow.Client.Services;
    using System;
    using System.Windows.Forms;

    public class CancelDelegateCommand : AbstractBoxCommand
    {
        public override void Run()
        {
            IWfBox owner = this.Owner as IWfBox;
            if ((owner != null) && (MessageHelper.ShowOkCancelInfo(ResourceService.GetString("Workflow.Message.ReallyCanncelDelegate")) == DialogResult.OK))
            {
                AbstractBoxCommand.WaitDialogHelper.Show();
                try
                {
                    WorkflowService.WfcInstance.CancelDelegate();
                }
                finally
                {
                    AbstractBoxCommand.WaitDialogHelper.Close();
                }
            }
        }
    }
}

