namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;

    public class Redo : AbstractMenuCommand
    {
        public override void Run()
        {
            IUndoHandler activeContent = WorkbenchSingleton.Workbench.ActiveContent as IUndoHandler;
            if (activeContent != null)
            {
                activeContent.Redo();
            }
        }

        public override bool IsEnabled
        {
            get
            {
                IUndoHandler activeContent = WorkbenchSingleton.Workbench.ActiveContent as IUndoHandler;
                return ((activeContent != null) && activeContent.EnableRedo);
            }
        }
    }
}

