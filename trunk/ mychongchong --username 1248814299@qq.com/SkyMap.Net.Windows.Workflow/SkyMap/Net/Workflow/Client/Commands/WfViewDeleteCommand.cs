namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Security;
    using SkyMap.Net.Workflow.Client.Services;
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using SkyMap.Net.Workflow.Engine;

    public class WfViewDeleteCommand : AbstractWfViewCommand
    {
        public override void Run()
        {
            if (this.IsEnabled && (MessageHelper.ShowOkCancelInfo("这将完全删除这些业务相关的所有数据，你确定吗？") == DialogResult.OK))
            {
                WaitDialogHelper.Show();
                try
                {
                    base.view.ForceCloseWithoutSaveChanged = true;
                    Dictionary<string, WorkItem> workItems = new Dictionary<string, WorkItem>(1);
                    workItems.Add(base.view.WorkItem.ProinstId, base.view.WorkItem);
                    base.view.Close();
                    WorkflowService.WfcInstance.ComleteDelete(workItems);
                    BoxService.CurrentBox.RefreshData();
                }
                finally
                {
                    WaitDialogHelper.Close();
                }
            }
        }

        public override bool IsEnabled
        {
            get
            {
                bool canRemove = false;
                canRemove = base.view.CanRemove;
                if (!canRemove)
                {
                    canRemove = SecurityUtil.IsCanAccess("AdminData,Admin");
                }
                LoggingService.InfoFormatted("是否具有删除权限：{0}", new object[] { canRemove });
                return canRemove;
            }
            set
            {
            }
        }
    }
}

