namespace SkyMap.Net.Gui.Components
{
    using SkyMap.Net.Core;
    using System;
    using System.Diagnostics;

    public class GridPanelExportToExcelCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            if (this.Owner is GridPanel)
            {
                Process.Start((this.Owner as GridPanel).ExportToExcel());
            }
        }
    }
}

