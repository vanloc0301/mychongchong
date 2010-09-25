namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Workflow.Client.Box;
    using System;

    public class CancelSelectCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            WfBox owner = this.Owner as WfBox;
            if (owner != null)
            {
                WaitDialogHelper.Show();
                try
                {
                    owner.CancelSelect();
                }
                finally
                {
                    WaitDialogHelper.Close();
                }
            }
        }
    }
}

