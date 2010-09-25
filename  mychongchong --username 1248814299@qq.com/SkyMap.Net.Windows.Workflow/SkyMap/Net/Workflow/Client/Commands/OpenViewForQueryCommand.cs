namespace SkyMap.Net.Workflow.Client.Commands
{
    using System;

    public class OpenViewForQueryCommand : DefaultOpenViewCommand
    {
        protected override bool CanEdit
        {
            get
            {
                return true;
            }
        }
    }
}

