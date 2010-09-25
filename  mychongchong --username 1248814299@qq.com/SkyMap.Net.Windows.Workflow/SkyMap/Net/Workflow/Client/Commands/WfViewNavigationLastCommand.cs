namespace SkyMap.Net.Workflow.Client.Commands
{
    using System;

    public class WfViewNavigationLastCommand : WfViewAbstractNavigationCommand
    {
        protected override int Item
        {
            get
            {
                return 3;
            }
        }
    }
}

