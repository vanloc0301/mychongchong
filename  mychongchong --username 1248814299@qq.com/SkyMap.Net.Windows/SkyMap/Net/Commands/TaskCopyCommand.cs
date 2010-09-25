namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;

    public class TaskCopyCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            LoggingService.Info("Copy object run...");
            WorkbenchTreePanel owner = this.Owner as WorkbenchTreePanel;
            ObjectNode selectedNode = owner.SelectedNode;
            if ((selectedNode != null) && selectedNode.EnableCopy)
            {
                try
                {
                    selectedNode.Copy();
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
                return ((selectedNode != null) && selectedNode.EnableCopy);
            }
            set
            {
                base.IsEnabled = value;
            }
        }
    }
}

