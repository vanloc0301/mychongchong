namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Workflow.Client.Box;
    using SkyMap.Net.Workflow.Client.Dialog;
    using System;

    public class DelegateCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            IWfBox owner = this.Owner as IWfBox;
            if (owner != null)
            {
                WaitDialogHelper.Show();
                try
                {
                    BoxHelper.Delegate(owner, new WtDialog());
                }
                finally
                {
                    WaitDialogHelper.Close();
                }
            }
        }
    }
}

