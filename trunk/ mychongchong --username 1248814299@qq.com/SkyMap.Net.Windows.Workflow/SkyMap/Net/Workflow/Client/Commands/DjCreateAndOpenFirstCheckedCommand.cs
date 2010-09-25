namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using System;

    public class DjCreateAndOpenFirstCheckedCommand : AbstractCheckableMenuCommand
    {
        public static bool IsChecked = false;

        public override void Run()
        {
            if (this.Owner != null)
            {
                IsChecked = (this.Owner as ToolBarCheckBox).Checked;
            }
        }

        public override object Owner
        {
            get
            {
                return base.Owner;
            }
            set
            {
                base.Owner = value;
            }
        }
    }
}

