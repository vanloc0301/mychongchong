namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;

    public class TaskPasteCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            LoggingService.Info("Paste object run...");
            WorkbenchTreePanel owner = this.Owner as WorkbenchTreePanel;
            ObjectNode selectedNode = owner.SelectedNode;
            if ((selectedNode != null) && selectedNode.EnablePaste)
            {
                try
                {
                    selectedNode.Paste();
                }
                catch (NotImplementedException)
                {
                }
            }
        }

        public override bool IsEnabled
        {
            get
            {
                WorkbenchTreePanel owner = this.Owner as WorkbenchTreePanel;
                ObjectNode selectedNode = owner.SelectedNode;
                return ((selectedNode != null) && selectedNode.EnablePaste);
            }
            set
            {
                base.IsEnabled = value;
            }
        }
    }
}

