namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using System;

    public class AcceptAndOpenCheckedCommand : AbstractCheckableMenuCommand
    {
        public const string key = "IsDoubleClickAcceptAndOpen";

        public override void Run()
        {
            if (this.Owner != null)
            {
                PropertyService.Set<bool>("IsDoubleClickAcceptAndOpen", (this.Owner as ToolBarCheckBox).Checked);
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
                if (this.Owner != null)
                {
                    (this.Owner as ToolBarCheckBox).Checked = PropertyService.Get<bool>("IsDoubleClickAcceptAndOpen", true);
                }
            }
        }
    }
}

