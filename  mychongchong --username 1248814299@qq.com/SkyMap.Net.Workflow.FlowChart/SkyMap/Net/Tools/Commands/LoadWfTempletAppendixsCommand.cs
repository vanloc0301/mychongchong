namespace SkyMap.Net.Tools.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Gui;
    using System;
    using SkyMap.Net.Workflow.XPDL.ExtendElement;
    using SkyMap.Net.Tools.Workflow;

    public class LoadWfTempletAppendixsCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.Debug("Load WfTempletAppendixs...");
            }
            TaskTreeNode owner = (TaskTreeNode) this.Owner;
            if (owner != null)
            {
                owner.AddNodes<WfTempletAppendixs, WfTempletAppendixsNode>(QueryHelper.List<WfTempletAppendixs>("ALL_WfTempletAppendixs"));
            }
        }
    }
}

