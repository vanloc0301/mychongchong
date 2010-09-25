namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Workflow.Client.Box;
    using System;

    public class OpenViewForQueryFocusCommand : DefaultOpenViewCommand
    {
        protected override bool CanEdit
        {
            get
            {
                return true;
            }
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

