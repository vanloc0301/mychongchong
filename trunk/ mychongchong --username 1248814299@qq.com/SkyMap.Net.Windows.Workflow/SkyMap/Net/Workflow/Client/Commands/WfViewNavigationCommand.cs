namespace SkyMap.Net.Workflow.Client.Commands
{
    using System;

    public class WfViewNavigationCommand : AbstractWfViewCommand
    {
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

        protected int Item
        {
            get
            {
                int result = 1;
                if (base.Codon.Properties.Contains("index"))
                {
                    int.TryParse(base.Codon.Properties["index"], out result);
                }
                return result;
            }
        }
    }
}

