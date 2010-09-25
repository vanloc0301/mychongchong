namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Gui;
    using System;
    using System.Collections.Generic;

    public class LoadEditTaskNodesCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.Debug("Load my Data words...");
            }
            IList<TaskNode> topTaskNode = TaskNode.GetTopTaskNode();
            ((TaskTreeNode) this.Owner).AddNodes<TaskNode, EditTaskNode>(topTaskNode);
        }
    }
}

