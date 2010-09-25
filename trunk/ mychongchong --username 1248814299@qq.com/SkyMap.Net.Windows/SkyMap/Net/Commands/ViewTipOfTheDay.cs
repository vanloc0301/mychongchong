namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;
    using System.Windows.Forms;

    public class ViewTipOfTheDay : AbstractMenuCommand
    {
        public override void Run()
        {
            using (TipOfTheDayDialog dialog = new TipOfTheDayDialog())
            {
                dialog.Owner = (Form) WorkbenchSingleton.Workbench;
                dialog.ShowDialog(WorkbenchSingleton.MainForm);
            }
        }
    }
}

