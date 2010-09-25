namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Workflow.Client.Box;
    using System;

    public class OpenViewForFocusCommand : DefaultOpenViewCommand
    {
        protected override int Index
        {
            get
            {
                return (this.Owner as IWfBox).FocusedRowIndex;
            }
        }
    }
}

