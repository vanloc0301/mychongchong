namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;
    using System.Diagnostics;
    using System.IO;

    public class OpenFileCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            if (base.Codon.Properties.Contains("filename"))
            {
                string path = FileUtility.Combine(new string[] { FileUtility.ApplicationRootPath, base.Codon.Properties["filename"] });
                if (File.Exists(path))
                {
                    if (base.Codon.Properties.Contains("copy"))
                    {
                        string destFileName = FileUtility.Combine(new string[] { Path.GetTempPath(), Path.GetFileName(path) });
                        File.Copy(path, destFileName, true);
                        path = destFileName;
                    }
                }
                else
                {
                    MessageHelper.ShowInfo("没有找到：{0}", path);
                    return;
                }
                ProcessStartInfo startInfo = new ProcessStartInfo(path);
                if (base.Codon.Properties.Contains("args"))
                {
                    startInfo.Arguments = base.Codon.Properties["args"];
                }
                startInfo.WindowStyle = ProcessWindowStyle.Normal;
                Process.Start(startInfo);
            }
            else
            {
                MessageHelper.ShowInfo("系统配置错误：没有配置FileName！");
            }
        }
    }
}

