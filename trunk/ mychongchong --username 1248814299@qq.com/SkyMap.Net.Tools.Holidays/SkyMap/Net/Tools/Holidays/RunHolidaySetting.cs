namespace SkyMap.Net.Tools.Holidays
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;

    public class RunHolidaySetting : AbstractMenuCommand
    {
        public override void Run()
        {
            HolidaysViewContent view = (HolidaysViewContent) ViewContentService.GetView(typeof(HolidaysViewContent));
            if (view == null)
            {
                view = new HolidaysViewContent();
                WorkbenchSingleton.Workbench.ShowView(view);
            }
            else
            {
                view.WorkbenchWindow.SelectWindow();
            }
        }
    }
}

