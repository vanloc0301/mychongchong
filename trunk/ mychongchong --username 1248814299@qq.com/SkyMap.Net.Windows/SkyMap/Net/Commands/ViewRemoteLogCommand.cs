namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Security;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Windows.Forms;

    public class ViewRemoteLogCommand : AbstractRoleSecurityCommand
    {
        public override void Run()
        {
            if (this.IsEnabled)
            {
                InputBox box = new InputBox("请输入使用的远程服务自动升级对象地址：", "提示：", "http://127.0.0.1:7502/RemoteUpdate");
                string result = string.Empty;
                if (box.ShowDialog(WorkbenchSingleton.MainForm) == DialogResult.OK)
                {
                    result = box.Result;
                    if (string.IsNullOrEmpty(result))
                    {
                        MessageHelper.ShowInfo("没有输入远程对象地址");
                    }
                    else
                    {
                        try
                        {
                            string remoteLog = (Activator.GetObject(typeof(IRemoteUpdate), result) as IRemoteUpdate).GetRemoteLog();
                            string path = Path.GetTempFileName() + ".txt";
                            File.WriteAllText(path, remoteLog);
                            Process.Start(path);
                        }
                        catch (Exception exception)
                        {
                            MessageHelper.ShowError("远程服务执行升级操作发生错误", exception);
                            LoggingService.Error(exception);
                        }
                    }
                }
            }
        }
    }
}

