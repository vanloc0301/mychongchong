namespace SkyMap.Net.GTOA.BillPrint
{
    using SkyMap.Net.Core;
    using System;

    public class IsCancelSelectedItemAfterPrint : AbstractCheckableMenuCommand
    {
        internal const string key = "IsCancelSelectedItemAfterPrint";

        public override void Run()
        {
            PropertyService.Set<bool>("IsCancelSelectedItemAfterPrint", (this.Owner as ToolBarCheckBox).Checked);
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
                (value as ToolBarCheckBox).Checked = PropertyService.Get<bool>("IsCancelSelectedItemAfterPrint", true);
            }
        }
    }
}

