namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Workflow.Client.Box;
    using System;

    public class SetWfBoxColumnAutoWidthCommand : AbstractMenuCommand, ICheckableMenuCommand, IMenuCommand, ICommand
    {
        public override void Run()
        {
            WfBox owner = this.Owner as WfBox;
            if (owner != null)
            {
                this.IsChecked = !this.IsChecked;
                owner.RefreshColumnAutoWidth();
            }
        }

        public bool IsChecked
        {
            get
            {
                WfBox owner = this.Owner as WfBox;
                if (owner != null)
                {
                    return PropertyService.Get<bool>("ColumnAutoWidth_" + owner.BoxName, false);
                }
                return false;
            }
            set
            {
                WfBox owner = this.Owner as WfBox;
                if (owner != null)
                {
                    PropertyService.Set<bool>("ColumnAutoWidth_" + owner.BoxName, value);
                }
            }
        }

        public override bool Visible
        {
            get
            {
                return (this.Owner is WfBox);
            }
            set
            {
            }
        }
    }
}

