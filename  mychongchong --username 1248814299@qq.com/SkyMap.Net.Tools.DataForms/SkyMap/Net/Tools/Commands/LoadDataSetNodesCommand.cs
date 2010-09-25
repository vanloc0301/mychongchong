namespace SkyMap.Net.Tools.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Gui;
    using System;
    using SkyMap.Net.DataForms;
    using SkyMap.Net.Tools.Nodes;

    public class LoadDataSetNodesCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.Debug("Load my data set nodes...");
            }
            TaskTreeNode owner = (TaskTreeNode) this.Owner;
            if (owner != null)
            {
                owner.AddNodes<DAODataSet, DAODataSetNode>(QueryHelper.List<DAODataSet>("ALL_DAODataSet"));
            }
        }
    }
}

