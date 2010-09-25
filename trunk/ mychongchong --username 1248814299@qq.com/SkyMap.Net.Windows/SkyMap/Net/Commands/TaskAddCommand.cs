namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;
    using System.Windows.Forms;

    public class TaskAddCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.Debug("Task add command running...");
            }
            WorkbenchTreePanel owner = this.Owner as WorkbenchTreePanel;
            ObjectNode selectedNode = owner.SelectedNode;
            if (selectedNode == null)
            {
                LoggingService.Warn("没有选择相应的节点来执行命令");
            }
            if ((selectedNode != null) && selectedNode.EnableAddChild)
            {
                if (PropertyView.Instance.IsDirty)
                {
                    if (MessageHelper.ShowYesNoInfo(ResourceService.GetString("Global.Message.SaveChanged")) == DialogResult.Yes)
                    {
                        PropertyView.Instance.Save();
                        selectedNode.PropertyChanged();
                    }
                    else
                    {
                        PropertyView.Instance.CancelEdit();
                    }
                }
                if (!selectedNode.IsExpanded)
                {
                    selectedNode.Expand();
                }
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.Info("将执行节点添加添加命令");
                }
                selectedNode.TreeView.SelectedNode = owner.SelectedNode.AddChild();
            }
        }

        public override bool IsEnabled
        {
            get
            {
                WorkbenchTreePanel owner = this.Owner as WorkbenchTreePanel;
                ObjectNode selectedNode = owner.SelectedNode;
                return ((selectedNode != null) && selectedNode.EnableAddChild);
            }
            set
            {
                base.IsEnabled = value;
            }
        }
    }
}

