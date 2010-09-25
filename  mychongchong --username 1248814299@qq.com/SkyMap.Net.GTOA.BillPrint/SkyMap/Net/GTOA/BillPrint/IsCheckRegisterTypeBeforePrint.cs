namespace SkyMap.Net.GTOA.BillPrint
{
    using SkyMap.Net.Core;
    using System;

    public class IsCheckRegisterTypeBeforePrint : AbstractCheckableMenuCommand
    {
        internal const string key = "IS_CHECK_REGISTERTYPE_BEFORE_PRINT";

        public override void Run()
        {
            PropertyService.Set<bool>("IS_CHECK_REGISTERTYPE_BEFORE_PRINT", (this.Owner as ToolBarCheckBox).Checked);
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
                (value as ToolBarCheckBox).Checked = PropertyService.Get<bool>("IS_CHECK_REGISTERTYPE_BEFORE_PRINT", true);
            }
        }
    }
}

