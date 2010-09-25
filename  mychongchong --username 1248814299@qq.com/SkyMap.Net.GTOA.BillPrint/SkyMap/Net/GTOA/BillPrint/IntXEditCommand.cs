namespace SkyMap.Net.GTOA.BillPrint
{
    using SkyMap.Net.Core;
    using System;

    public class IntXEditCommand : AbstractSpinEditCommand
    {
        public static int X;

        public override void Run()
        {
            X = Convert.ToInt32((this.Owner as ToolBarSpinEdit).EditValue);
        }
    }
}

