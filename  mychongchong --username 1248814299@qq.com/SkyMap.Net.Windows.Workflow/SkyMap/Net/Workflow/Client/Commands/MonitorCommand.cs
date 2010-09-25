namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Gui;
    using SkyMap.Net.Security;
    using SkyMap.Net.Workflow.Client.Box;
    using SkyMap.Net.Workflow.Client.Services;
    using System;

    public class MonitorCommand : AbstractRoleSecurityCommand
    {
        public override void Run()
        {
            IWfBox owner = this.Owner as IWfBox;
            if (owner != null)
            {
                WaitDialogHelper.Show();
                try
                {
                    WorkflowService.DelegateEvent(owner, new WfClientAPIHandler(WorkflowService.WfcInstance.Monitor));
                }
                finally
                {
                    WaitDialogHelper.Close();
                }
            }
        }
    }
}

