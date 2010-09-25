namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Workflow.Client.Services;
    using System;

    public class LoadBoxCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.Debug("Will load box...");
            }
            TaskTreeNode owner = (TaskTreeNode) this.Owner;
            if (owner != null)
            {
                BoxService.OpenBox(owner.Text);
            }
        }
    }
}

