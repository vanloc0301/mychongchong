namespace SkyMap.Net.GTOA.BillPrint
{
    using SkyMap.Net.Core;
    using System;

    public class IntYEditCommand : AbstractSpinEditCommand
    {
        public static int Y;

        public override void Run()
        {
            Y = Convert.ToInt32((this.Owner as ToolBarSpinEdit).EditValue);
        }
    }
}

