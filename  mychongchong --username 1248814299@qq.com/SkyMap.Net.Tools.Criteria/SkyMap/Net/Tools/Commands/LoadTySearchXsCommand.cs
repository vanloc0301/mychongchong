namespace SkyMap.Net.Tools.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;
    using SkyMap.Net.Criteria;
    using SkyMap.Net.Tools.Criteria;

    public class LoadTySearchXsCommand : AbstractMenuCommand
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
                owner.AddNodes<TySearchx, TySearchXNode>(RemotingSingletonProvider<CriteriaDAOService>.Instance.TySearchXs);
            }
        }
    }
}

