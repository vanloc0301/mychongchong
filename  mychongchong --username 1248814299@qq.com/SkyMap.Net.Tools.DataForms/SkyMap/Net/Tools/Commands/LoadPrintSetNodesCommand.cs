namespace SkyMap.Net.Tools.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DataForms;
    using SkyMap.Net.Gui;
    using System;
    using SkyMap.Net.Tools.DataForms;

    public class LoadPrintSetNodesCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.Debug("Load my print set...");
            }
            TaskTreeNode owner = (TaskTreeNode) this.Owner;
            if (owner != null)
            {
                owner.AddNodes<PrintSet, PrintSetNode>(DataFormDAOService.Instance.PrintSets);
            }
        }
    }
}

