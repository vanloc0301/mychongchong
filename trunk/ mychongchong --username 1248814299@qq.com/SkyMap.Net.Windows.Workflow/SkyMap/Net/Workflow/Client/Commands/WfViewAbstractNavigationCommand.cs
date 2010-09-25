namespace SkyMap.Net.Workflow.Client.Commands
{
    using System;

    public abstract class WfViewAbstractNavigationCommand : AbstractWfViewCommand
    {
        protected WfViewAbstractNavigationCommand()
        {
        }

        public override void Run()
        {
            if (this.IsEnabled)
            {
                base.view.WfViewNavigation(this.Item);
            }
        }

        public override bool IsEnabled
        {
            get
            {
                return ((base.view.NavigationDataRowViews != null) && (base.view.NavigationDataRowViews[this.Item] != null));
            }
            set
            {
                base.IsEnabled = value;
            }
        }

        protected abstract int Item { get; }
    }
}

