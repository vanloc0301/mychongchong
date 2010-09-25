namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;

    public class ToggleFullscreenCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            ((DefaultWorkbench) WorkbenchSingleton.Workbench).FullScreen = !((DefaultWorkbench) WorkbenchSingleton.Workbench).FullScreen;
        }
    }
}

