namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;

    public class SelectNextWindow : AbstractMenuCommand
    {
        public override void Run()
        {
            if (WorkbenchSingleton.Workbench.ActiveWorkbenchWindow != null)
            {
                int index = WorkbenchSingleton.Workbench.ViewContentCollection.IndexOf(WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.ViewContent);
                WorkbenchSingleton.Workbench.ViewContentCollection[(index + 1) % WorkbenchSingleton.Workbench.ViewContentCollection.Count].WorkbenchWindow.SelectWindow();
            }
        }
    }
}

