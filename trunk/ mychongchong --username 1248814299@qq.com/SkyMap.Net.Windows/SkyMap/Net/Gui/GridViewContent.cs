namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui.Components;
    using System;
    using System.Windows.Forms;

    public class GridViewContent : AbstractViewContent
    {
        private GridPanel gridPanel;

        public GridViewContent(string titleName, GridPanel gridPanel)
        {
            base.TitleName = titleName;
            this.gridPanel = gridPanel;
        }

        public static GridViewContent Find(string titleName)
        {
            foreach (IViewContent content in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.Info("查询 :" + content.TitleName);
                }
                if ((content.TitleName == titleName) && (content is GridViewContent))
                {
                    return (GridViewContent) content;
                }
            }
            return null;
        }

        public override System.Windows.Forms.Control Control
        {
            get
            {
                return this.gridPanel;
            }
        }
    }
}

