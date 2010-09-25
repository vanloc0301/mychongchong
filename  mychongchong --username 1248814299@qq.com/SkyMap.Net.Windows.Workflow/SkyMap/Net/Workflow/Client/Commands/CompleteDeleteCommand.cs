namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Gui;
    using SkyMap.Net.Security;
    using SkyMap.Net.Workflow.Client.Box;
    using SkyMap.Net.Workflow.Client.Services;
    using System;
    using System.Windows.Forms;

    public class CompleteDeleteCommand : AbstractRoleSecurityCommand
    {
        public override void Run()
        {
            if (this.IsEnabled)
            {
                IWfBox owner = this.Owner as IWfBox;
                if ((owner != null) && (MessageHelper.ShowOkCancelInfo("这将完全删除这些业务相关的所有数据，你确定吗？") == DialogResult.OK))
                {
                    WaitDialogHelper.Show();
                    try
                    {
                        WorkflowService.DelegateEvent(owner, new WfClientAPIHandler(WorkflowService.WfcInstance.ComleteDelete));
                    }
                    finally
                    {
                        WaitDialogHelper.Close();
                    }
                }
            }
        }
    }
}

