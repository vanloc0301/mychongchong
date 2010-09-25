namespace SkyMap.Net.Workflow.Client.Commands
{
    using System;

    public class WfViewPrintCommand : AbstractWfViewCommand
    {
        public override void Run()
        {
            base.view.Print();
        }

        public override bool IsEnabled
        {
            get
            {
                return (base.view.TemplatePrint != null);
            }
            set
            {
                base.IsEnabled = value;
            }
        }
    }
}

