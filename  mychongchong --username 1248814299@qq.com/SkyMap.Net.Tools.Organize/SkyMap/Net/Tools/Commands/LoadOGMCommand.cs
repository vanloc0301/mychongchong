namespace SkyMap.Net.Tools.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Security;
    using System;
    using SkyMap.Net.OGM;
    using SkyMap.Net.Tools.Organize;

    public class LoadOGMCommand : AbstractMenuCommand
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
                owner.AddNodes<CDept, DeptNode>(OGMService.GetTopDepts());
            }
        }
    }
}

