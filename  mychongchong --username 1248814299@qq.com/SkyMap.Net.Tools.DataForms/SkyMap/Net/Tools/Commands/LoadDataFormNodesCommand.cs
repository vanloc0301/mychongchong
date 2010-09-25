namespace SkyMap.Net.Tools.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Gui;
    using System;
    using SkyMap.Net.DataForms;
    using SkyMap.Net.Tools.Nodes;

    public class LoadDataFormNodesCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.Debug("Load dataform nodes...");
            }
            TaskTreeNode owner = (TaskTreeNode) this.Owner;
            if (owner != null)
            {
                owner.AddNodes<DAODataForm, DAODataFormNode>(QueryHelper.List<DAODataForm>("ALL_DAODataForm"));
            }
        }
    }
}

