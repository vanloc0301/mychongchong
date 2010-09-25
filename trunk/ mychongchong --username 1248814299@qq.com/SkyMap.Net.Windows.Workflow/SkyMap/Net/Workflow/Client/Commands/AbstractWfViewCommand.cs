namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Workflow.Client.View;
    using System;

    public abstract class AbstractWfViewCommand : AbstractMenuCommand
    {
        protected WfView view;

        protected AbstractWfViewCommand()
        {
        }

        public override object Owner
        {
            get
            {
                return this.view;
            }
            set
            {
                this.view = value as WfView;
            }
        }
    }
}

