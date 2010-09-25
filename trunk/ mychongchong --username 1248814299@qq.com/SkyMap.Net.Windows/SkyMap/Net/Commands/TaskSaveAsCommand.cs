namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;

    public class TaskSaveAsCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            WorkbenchTreePanel owner = this.Owner as WorkbenchTreePanel;
            owner.SelectedNode.SaveAs();
        }
    }
}

