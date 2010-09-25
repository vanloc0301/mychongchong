namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Gui.Dialogs;
    using System;
    using System.Windows.Forms;

    public class AboutSkyMapSoft : AbstractMenuCommand
    {
        public override void Run()
        {
            using (CommonAboutDialog dialog = new CommonAboutDialog())
            {
                dialog.Owner = (Form) WorkbenchSingleton.Workbench;
                dialog.ShowDialog();
            }
        }
    }
}

