namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Workflow.Client.Box;
    using System;

    public class AcceptAndOpenFocusCommand : DefaultAcceptAndOpenCommand
    {
        public override void Run()
        {
            base.accept = PropertyService.Get<bool>("IsDoubleClickAcceptAndOpen", true);
            base.Run();
        }

        protected override int Index
        {
            get
            {
                return (this.Owner as IWfBox).FocusedRowIndex;
            }
        }
    }
}

