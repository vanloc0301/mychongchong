namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;
    using System.Windows.Forms;

    public class TaskRemoveCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            WorkbenchTreePanel owner = this.Owner as WorkbenchTreePanel;
            ObjectNode selectedNode = owner.SelectedNode;
            if (((selectedNode != null) && selectedNode.EnableDelete) && (MessageHelper.ShowOkCancelInfo(ResourceService.GetString("Global.Message.Delete")) == DialogResult.OK))
            {
                owner.SelectedNode.Delete();
            }
        }

        public override bool IsEnabled
        {
            get
            {
                WorkbenchTreePanel owner = this.Owner as WorkbenchTreePanel;
                ObjectNode selectedNode = owner.SelectedNode;
                return ((selectedNode != null) && selectedNode.EnableDelete);
            }
            set
            {
                base.IsEnabled = value;
            }
        }
    }
}

