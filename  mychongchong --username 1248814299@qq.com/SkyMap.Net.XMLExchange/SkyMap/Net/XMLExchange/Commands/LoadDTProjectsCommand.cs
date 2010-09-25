namespace SkyMap.Net.XMLExchange.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Gui;
    using System;
    using SkyMap.Net.XMLExchange.Model;
    using SkyMap.Net.XMLExchange.GUI.Nodes;

    public class LoadDTProjectsCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.Debug("Load xml change projects...");
            }
            ((TaskTreeNode) this.Owner).AddNodes<DTProject, DTProjectNode>(QueryHelper.List<DTProject>("ALL_DTPROJECT_DAO", new string[] { "DisplayOrder" }));
        }
    }
}

