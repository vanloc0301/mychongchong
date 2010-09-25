namespace SkyMap.Net.Workflow.Client.Commands
{
    using System;

    public class WfViewNavigationPreviousCommand : WfViewAbstractNavigationCommand
    {
        protected override int Item
        {
            get
            {
                return 1;
            }
        }
    }
}

