namespace SkyMap.Net.Tools.Commands
{
    using SkyMap.Net.BrowserDisplayBinding;
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;

    public class ShowStartPageCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            foreach (IViewContent content in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                BrowserPane pane = content as BrowserPane;
                if ((pane != null) && (pane.Url.Scheme == "startpage"))
                {
                    content.WorkbenchWindow.SelectWindow();
                    return;
                }
            }
            WorkbenchSingleton.Workbench.ShowView(new BrowserPane(new Uri("startpage://start/")));
        }
    }
}

