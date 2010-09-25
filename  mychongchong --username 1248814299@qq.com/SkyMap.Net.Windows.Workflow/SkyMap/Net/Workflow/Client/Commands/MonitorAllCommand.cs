namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Workflow.Client.Box;
    using System;

    public class MonitorAllCommand : AbstractBoxCommand
    {
        public override void Run()
        {
            WfBox owner = this.Owner as WfBox;
            if (owner != null)
            {
                AbstractBoxCommand.WaitDialogHelper.Show();
                try
                {
                    owner.DataSource = BoxHelper.GetData(owner, "SkyMap.Net.Workflow", base.ID, null);
                    owner.RefreshColumnAutoWidth();
                }
                finally
                {
                    AbstractBoxCommand.WaitDialogHelper.Close();
                }
            }
        }
    }
}

