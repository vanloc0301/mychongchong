namespace SkyMap.Net.BrowserDisplayBinding
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;

    public class NewWindow : AbstractCommand
    {
        public override void Run()
        {
            WorkbenchSingleton.Workbench.ShowView(new BrowserPane());
        }
    }
}

