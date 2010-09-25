namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using System;

    public class AutoUpdateCommand : AbstractCommand
    {
        public override void Run()
        {
            AutoUpdateHepler.TryUpdate();
        }
    }
}

