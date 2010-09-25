namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Gui;
    using System;
    using System.Collections.Generic;

    public class LoadDataWordTypesCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.Debug("Load my Data words...");
            }
            IList<DataWordType> objs = QueryHelper.List<DataWordType>("ALL_DataWordType", new string[] { "DisplayOrder" });
            ((TaskTreeNode) this.Owner).AddNodes<DataWordType, DataWordTypeNode>(objs);
        }
    }
}

