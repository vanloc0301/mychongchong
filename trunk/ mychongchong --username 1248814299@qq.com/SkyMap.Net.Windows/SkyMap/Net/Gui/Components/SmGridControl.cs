namespace SkyMap.Net.Gui.Components
{
    using DevExpress.XtraGrid;
    using DevExpress.XtraGrid.Localization;
    using System;
    using System.Windows.Forms;

    public class SmGridControl : GridControl
    {
        static SmGridControl()
        {
            GridLocalizer.Active = new ChineseGridLocalizer();
        }

        public SmGridControl()
        {
            try
            {
                base.ImeMode = ImeMode.NoControl;
            }
            catch
            {
            }
        }
    }
}

