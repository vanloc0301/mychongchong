namespace SkyMap.Net.Tools.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.OGM;
    using System;
    using SkyMap.Net.Tools.Organize;

    public class LoadAuthTypesCommand : AbstractMenuCommand
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
                owner.AddNodes<CAuthType, AuthTypeNode>(OGMDAOService.Instance.AuthTypes);
            }
        }
    }
}

