namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Workflow.Client.Box;
    using System;

    public class SetDefaultBoxCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            IBox owner = this.Owner as IBox;
            if (owner != null)
            {
                PropertyService.Set<string>("DefaultBox", owner.BoxName);
            }
        }
    }
}

