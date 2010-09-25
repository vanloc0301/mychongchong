namespace SkyMap.Net.Workflow.Client.Commands
{
    using System;

    public class WfViewNavigationNextCommand : WfViewAbstractNavigationCommand
    {
        protected override int Item
        {
            get
            {
                return 2;
            }
        }
    }
}

