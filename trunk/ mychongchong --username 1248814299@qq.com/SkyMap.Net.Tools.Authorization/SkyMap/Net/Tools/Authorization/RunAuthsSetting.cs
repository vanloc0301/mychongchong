namespace SkyMap.Net.Tools.Authorization
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;

    public class RunAuthsSetting : AbstractMenuCommand
    {
        public override void Run()
        {
            AuthsViewContent view = (AuthsViewContent) ViewContentService.GetView(typeof(AuthsViewContent));
            if (view == null)
            {
                view = new AuthsViewContent();
                WorkbenchSingleton.Workbench.ShowView(view);
            }
            else
            {
                view.WorkbenchWindow.SelectWindow();
            }
        }
    }
}

