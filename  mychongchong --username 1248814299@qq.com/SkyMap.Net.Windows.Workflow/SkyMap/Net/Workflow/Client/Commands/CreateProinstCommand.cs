namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Workflow.Client.Box;
    using SkyMap.Net.Workflow.Client.Services;
    using SkyMap.Net.Workflow.XPDL;
    using System;

    public class CreateProinstCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            IWfBox owner = this.Owner as IWfBox;
            if (this.IsEnabled && (owner != null))
            {
                WaitDialogHelper.Show();
                try
                {
                    ProdefRow selectProdef = DisplayMyStartProdefsCommand.SelectProdef;
                    int num = DisplayAddNumEditCommand.Num;
                    WorkflowService.WfcInstance.CreateWfProcess(selectProdef.Id, num);
                    owner.RefreshData();
                }
                finally
                {
                    WaitDialogHelper.Close();
                }
            }
        }

        public override bool IsEnabled
        {
            get
            {
                return ((DisplayMyStartProdefsCommand.SelectProdef != null) && (DisplayAddNumEditCommand.Num > 0));
            }
            set
            {
                base.IsEnabled = value;
            }
        }
    }
}

