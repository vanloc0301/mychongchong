namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Workflow.Client.Box;
    using System;

    public class OpenZbViewForFocusCommand : DefaultOpenZbViewCommand
    {
        protected override int Index
        {
            get
            {
                return (this.Owner as IWfBox).FocusedRowIndex;
            }
        }

        protected override bool CanEdit
        {
            get
            {
                return true;
            }
        }
    }
}

