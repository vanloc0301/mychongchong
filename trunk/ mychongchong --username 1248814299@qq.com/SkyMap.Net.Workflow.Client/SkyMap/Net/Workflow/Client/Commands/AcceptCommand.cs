namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Workflow.Client.Box;
    using SkyMap.Net.Workflow.Client.Services;
    using System;

    public class AcceptCommand : AbstractBoxCommand
    {
        public override void Run()
        {
            IWfBox owner = this.Owner as IWfBox;
            if (owner != null)
            {
                AbstractBoxCommand.WaitDialogHelper.Show();
                try
                {
                    WorkflowService.DelegateEvent(owner, new WfClientAPIHandler(WorkflowService.WfcInstance.Accept));
                }
                finally
                {
                    AbstractBoxCommand.WaitDialogHelper.Close();
                }
            }
        }
    }
}

