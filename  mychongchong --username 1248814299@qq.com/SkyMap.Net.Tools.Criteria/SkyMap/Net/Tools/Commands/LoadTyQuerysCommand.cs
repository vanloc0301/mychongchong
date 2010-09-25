namespace SkyMap.Net.Tools.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Gui;
    using System;
    using SkyMap.Net.Criteria;
    using SkyMap.Net.Tools.Criteria;

    public class LoadTyQuerysCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.Debug("Load tax type...");
            }
            TaskTreeNode owner = (TaskTreeNode) this.Owner;
            if (owner != null)
            {
                owner.AddNodes<TyQuery, TyQueryNode>(QueryHelper.List<TyQuery>("ALL_TyQuery"));
            }
        }
    }
}

