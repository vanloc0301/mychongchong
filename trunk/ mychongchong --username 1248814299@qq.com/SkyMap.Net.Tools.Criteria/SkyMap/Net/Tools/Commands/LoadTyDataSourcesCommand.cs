namespace SkyMap.Net.Tools.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Tools.Criteria;
    using SkyMap.Net.Criteria;

    public class LoadTyDataSourcesCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.Debug("Load Data source...");
            }
            TaskTreeNode owner = (TaskTreeNode) this.Owner;
            if (owner != null)
            {
                owner.AddNodes<SMDataSource, TyDataSourceNode>(RemotingSingletonProvider<CriteriaDAOService>.Instance.TyDataSources);
            }
        }
    }
}

