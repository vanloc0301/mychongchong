namespace SkyMap.Net.Workflow.Client.Commands
{
    using System;

    public class WfViewNavigationFirstCommand : WfViewAbstractNavigationCommand
    {
        protected override int Item
        {
            get
            {
                return 0;
            }
        }
    }
}

