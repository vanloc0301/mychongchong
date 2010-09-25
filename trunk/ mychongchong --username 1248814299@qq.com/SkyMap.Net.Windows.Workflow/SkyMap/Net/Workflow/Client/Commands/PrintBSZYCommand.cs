namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Util;
    using System;
    using System.IO;

    public class PrintBSZYCommand : AbstractBoxCommand
    {
        private FTPclient ftpClient = null;
        private static string templetPath = FileUtility.Combine(new string[] { PropertyService.DataDirectory, "resources", "办事指引" });

        public PrintBSZYCommand()
        {
            this.SetupFtp();
        }

        public override void Run()
        {
            AbstractBoxCommand.WaitDialogHelper.Show();
            try
            {
                if ((DisplayMyFirstProdefsCommand.SelectProdef != null) && (ListTempletsCommand.TempletAppendix != null))
                {
                    string localFilename = null;
                    string filename = string.Format("{0}-{1}.doc", DisplayMyFirstProdefsCommand.SelectProdef.Name, ListTempletsCommand.TempletAppendix.Name);
                    if ((this.ftpClient != null) && this.ftpClient.FtpFileExists(filename))
                    {
                        localFilename = Path.GetTempFileName();
                        this.ftpClient.Download(filename, localFilename, true);
                        PrintHelper.PrintWord(localFilename, true, true);
                    }
                    else
                    {
                        string path = FileUtility.Combine(new string[] { templetPath, filename });
                        if (File.Exists(path))
                        {
                            localFilename = Path.GetTempFileName();
                            File.Copy(path, localFilename, true);
                            PrintHelper.PrintWord(localFilename, true, true);
                        }
                    }
                    if (localFilename == null)
                    {
                        MessageHelper.ShowInfo("找不到对应的办事指引模板文件,\r\n请添加:'{0}'文件.", filename);
                    }
                }
                else
                {
                    MessageHelper.ShowInfo("请选择业务类型和收件类型.");
                }
            }
            finally
            {
                AbstractBoxCommand.WaitDialogHelper.Close();
            }
        }

        public void SetupFtp()
        {
            string path = Path.Combine(PropertyService.ConfigDirectory, "ftp.config");
            if (File.Exists(path) || File.Exists(Path.Combine(PropertyService.ConfigDirectory, "ftp.cconfig")))
            {
                FtpSetting setting = FtpSetting.Get(path);
                if (string.IsNullOrEmpty(setting.Host))
                {
                    throw new ArgumentNullException("host");
                }
                this.ftpClient = new FTPclient();
                this.ftpClient.Hostname = setting.Host + "/办事指引";
                if (!string.IsNullOrEmpty(setting.User))
                {
                    this.ftpClient.Username = setting.User;
                    this.ftpClient.Password = setting.Password;
                }
            }
        }
    }
}

