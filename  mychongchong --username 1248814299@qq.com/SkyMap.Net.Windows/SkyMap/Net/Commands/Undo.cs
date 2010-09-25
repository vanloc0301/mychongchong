namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;
    using System.Windows.Forms;

    public class Undo : AbstractMenuCommand
    {
        public override void Run()
        {
            IUndoHandler activeContent = WorkbenchSingleton.Workbench.ActiveContent as IUndoHandler;
            if (activeContent != null)
            {
                activeContent.Undo();
            }
            else
            {
                TextBoxBase activeControl = WorkbenchSingleton.ActiveControl as TextBoxBase;
                if (activeControl != null)
                {
                    activeControl.Undo();
                }
            }
        }

        public override bool IsEnabled
        {
            get
            {
                IUndoHandler activeContent = WorkbenchSingleton.Workbench.ActiveContent as IUndoHandler;
                if (activeContent != null)
                {
                    return activeContent.EnableUndo;
                }
                TextBoxBase activeControl = WorkbenchSingleton.ActiveControl as TextBoxBase;
                return ((activeControl != null) && activeControl.CanUndo);
            }
        }
    }
}

