namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;
    using System.Windows.Forms;

    public class ManualUpdateCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            if (MessageHelper.ShowOkCancelInfo("将关闭当前程序，你确认要手动升级吗？") == DialogResult.OK)
            {
                AutoUpdateHepler.TryUpdate("0.00.01");
            }
        }
    }
}

