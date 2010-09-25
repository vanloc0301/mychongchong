namespace SkyMap.Net.Tools.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Gui;
    using System;
    using SkyMap.Net.Tools.Workflow;

    public class LoadConditionsCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.Debug("加载所有条件定义...");
            }
            TaskTreeNode owner = (TaskTreeNode) this.Owner;
            if (owner != null)
            {
                //owner.AddNodes<Condition , ConditionNode>(QueryHelper.List<Condition>("ALL_Condition"));
                owner.AddNodes<SkyMap.Net.Workflow.XPDL.Condition,ConditionNode>(QueryHelper.List<SkyMap.Net.Workflow.XPDL.Condition>("ALL_Condition"));
            }
        }
    }
}

