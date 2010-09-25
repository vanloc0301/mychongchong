namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;

    public class TaskSaveCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            WorkbenchTreePanel owner = this.Owner as WorkbenchTreePanel;
            ObjectNode selectedNode = owner.SelectedNode;
            if ((selectedNode != null) && PropertyView.Instance.IsDirty)
            {
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.Info("准备保存修改后的对象...");
                }
                PropertyView.Instance.Save();
                selectedNode.PropertyChanged();
                MessageHelper.ShowInfo("保存成功!");
            }
        }
    }
}

