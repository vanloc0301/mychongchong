namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;

    public class CloseFile : AbstractMenuCommand
    {
        public override void Run()
        {
            if (WorkbenchSingleton.Workbench.ActiveWorkbenchWindow != null)
            {
                WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.CloseWindow(false);
            }
        }
    }
}

