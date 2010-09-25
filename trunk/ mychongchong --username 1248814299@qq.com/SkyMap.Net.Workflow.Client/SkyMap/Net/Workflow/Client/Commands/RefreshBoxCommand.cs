namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Workflow.Client.Box;
    using System;

    public class RefreshBoxCommand : AbstractBoxCommand
    {
        public override void Run()
        {
            IWfBox owner = this.Owner as IWfBox;
            if (owner != null)
            {
                AbstractBoxCommand.WaitDialogHelper.Show();
                try
                {
                    owner.RefreshData();
                }
                finally
                {
                    AbstractBoxCommand.WaitDialogHelper.Close();
                }
            }
        }
    }
}

