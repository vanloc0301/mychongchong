namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Workflow.Client.Box;
    using SkyMap.Net.Workflow.Client.Dialog;
    using SkyMap.Net.Workflow.Client.Services;
    using System;

    public class TerminateCommand : AbstractBoxCommand
    {
        public override void Run()
        {
            IWfBox owner = this.Owner as IWfBox;
            if (owner != null)
            {
                AbstractBoxCommand.WaitDialogHelper.Show();
                try
                {
                    BoxHelper.DelegateEvent(owner, new ReasonDialog(), new WfClientAPIHandler(WorkflowService.WfcInstance.Terminate));
                }
                finally
                {
                    AbstractBoxCommand.WaitDialogHelper.Close();
                }
            }
        }
    }
}

