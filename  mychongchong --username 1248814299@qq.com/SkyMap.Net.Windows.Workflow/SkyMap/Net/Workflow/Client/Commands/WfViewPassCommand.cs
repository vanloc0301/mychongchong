namespace SkyMap.Net.Workflow.Client.Commands
{
    using System;

    public class WfViewPassCommand : AbstractWfViewCommand
    {
        public override void Run()
        {
            if (this.IsEnabled)
            {
                base.view.Pass();
            }
        }

        public override bool IsEnabled
        {
            get
            {
                return base.view.CanPass;
            }
            set
            {
                base.IsEnabled = value;
            }
        }
    }
}

