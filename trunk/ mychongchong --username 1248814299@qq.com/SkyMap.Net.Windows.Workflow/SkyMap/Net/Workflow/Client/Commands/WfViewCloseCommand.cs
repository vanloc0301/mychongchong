namespace SkyMap.Net.Workflow.Client.Commands
{
    using System;

    public class WfViewCloseCommand : AbstractWfViewCommand
    {
        public override void Run()
        {
            base.view.Close();
        }
    }
}

