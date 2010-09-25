namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;
    using System.Windows.Forms;

    public class ExitWorkbenchCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            ((Form) WorkbenchSingleton.Workbench).Close();
        }
    }
}

