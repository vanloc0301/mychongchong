namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui.Dialogs;
    using System;

    public class DBExportCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            DBExportDialog dialog = new DBExportDialog();
            try
            {
                dialog.ShowDialog();
            }
            finally
            {
                dialog.Close();
            }
        }
    }
}

