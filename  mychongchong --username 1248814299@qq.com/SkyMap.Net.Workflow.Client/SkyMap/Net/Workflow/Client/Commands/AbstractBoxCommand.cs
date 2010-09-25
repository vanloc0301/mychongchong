namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using System;

    public abstract class AbstractBoxCommand : AbstractMenuCommand
    {
        protected static WaitUI WaitDialogHelper = WaitUI.Create();

        protected AbstractBoxCommand()
        {
        }
    }
}

