namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;

    public class RemoteTryCommand : AbstractCommand
    {
        public override void Run()
        {
            RemoteHelper.Initialize();
        }
    }
}

