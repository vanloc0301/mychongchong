namespace SkyMap.Net.Workflow.Client.View
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Workflow.Instance;
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public class RemoveMaterCommand : MatersEditCommand
    {
        public override void Run()
        {
            List<WfProinstMater> selectedOrFocusMaters = base.me.GetSelectedOrFocusMaters();
            if (selectedOrFocusMaters.Count > 0)
            {
                if (MessageHelper.ShowOkCancelInfo(ResourceService.GetString("Global.Message.Delete")) == DialogResult.OK)
                {
                    foreach (WfProinstMater mater in selectedOrFocusMaters)
                    {
                        base.me.CurrentUnitOfWork.RegisterRemoved(mater);
                    }
                    base.me.RemoveMaters(selectedOrFocusMaters);
                }
            }
            else
            {
                MessageHelper.ShowInfo("请先选择要删除的收件资料");
            }
        }

        public override bool IsEnabled
        {
            get
            {
                return (base.me.GetSelectedOrFocusMaters().Count > 0);
            }
            set
            {
            }
        }
    }
}

