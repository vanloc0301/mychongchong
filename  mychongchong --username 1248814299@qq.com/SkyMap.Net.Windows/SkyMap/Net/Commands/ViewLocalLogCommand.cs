namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Security;
    using System;
    using System.Diagnostics;
    using System.IO;

    public class ViewLocalLogCommand : AbstractRoleSecurityCommand
    {
        public override void Run()
        {
            if (this.IsEnabled)
            {
                string path = Path.Combine(FileUtility.ApplicationRootPath, @"bin\log.txt");
                if (File.Exists(path))
                {
                    Process.Start(path);
                }
                else
                {
                    MessageHelper.ShowInfo("没有找到日志文件" + path);
                }
            }
        }
    }
}

