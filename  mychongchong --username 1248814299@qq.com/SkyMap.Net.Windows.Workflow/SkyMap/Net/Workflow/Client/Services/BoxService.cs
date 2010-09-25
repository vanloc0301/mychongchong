namespace SkyMap.Net.Workflow.Client.Services
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Workflow.Client.Box;
    using SkyMap.Net.Workflow.Client.Contents;
    using System;

    public static class BoxService
    {
        private static string currentBoxName = null;

        public static void OpenBox(string boxName)
        {
            BoxViewContext context = null;
            currentBoxName = boxName;
            foreach (IViewContent content in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.Info("查询 :" + content.TitleName);
                }
                if ((content.TitleName == boxName) && (content is BoxViewContext))
                {
                    context = (BoxViewContext) content;
                }
            }
            if (context == null)
            {
                context = new BoxViewContext(boxName);
                WorkbenchSingleton.Workbench.ShowView(context);
            }
            else
            {
                context.RefreshData();
                context.WorkbenchWindow.SelectWindow();
            }
        }

        public static IBox CurrentBox
        {
            get
            {
                if (((WorkbenchSingleton.Workbench.ActiveWorkbenchWindow != null) && (WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.ActiveViewContent != null)) && (WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.ActiveViewContent is BoxViewContext))
                {
                    return (IBox) WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.ActiveViewContent.Control;
                }
                return null;
            }
        }
    }
}

