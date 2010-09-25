namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;

    public class CloseAllWindows : AbstractMenuCommand
    {
        public override void Run()
        {
            WorkbenchSingleton.Workbench.CloseAllViews();
        }
    }
}

