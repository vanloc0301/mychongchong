namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;
    using System.Windows.Forms;

    public class ChangeServerCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            if (MessageHelper.ShowOkCancelInfo("这将关闭当前系统，确认要切换服务器吗？") == DialogResult.OK)
            {
                RemoteHelper.Reset();
            }
        }

        public override bool IsEnabled
        {
            get
            {
                return (RemoteHelper.Servers.Count > 1);
            }
            set
            {
            }
        }
    }
}

