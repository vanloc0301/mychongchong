namespace SkyMap.Net.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class CoreStartup
    {
        private List<string> addInFiles = new List<string>();
        private string applicationName;
        private string configDirectory;
        private string dataDirectory;
        private List<string> disabledAddIns = new List<string>();
        private string propertiesName;

        public CoreStartup(string applicationName)
        {
            if (applicationName == null)
            {
                throw new ArgumentNullException("applicationName");
            }
            this.applicationName = applicationName;
            this.propertiesName = "Properties";
            MessageService.DefaultMessageBoxTitle = applicationName;
            MessageService.ProductName = applicationName;
        }

        public void AddAddInsFromDirectory(string addInDir)
        {
            this.addInFiles.AddRange(FileUtility.SearchDirectory(addInDir, "*.addin"));
        }

        public void ConfigureExternalAddIns(string addInConfigurationFile)
        {
            AddInManager.ConfigurationFileName = addInConfigurationFile;
            AddInManager.LoadAddInConfiguration(this.addInFiles, this.disabledAddIns);
        }

        public void ConfigureUserAddIns(string addInInstallTemp, string userAddInPath)
        {
            AddInManager.AddInInstallTemp = addInInstallTemp;
            AddInManager.UserAddInPath = userAddInPath;
            if (Directory.Exists(addInInstallTemp))
            {
                AddInManager.InstallAddIns(this.disabledAddIns);
            }
            if (Directory.Exists(userAddInPath))
            {
                this.AddAddInsFromDirectory(userAddInPath);
            }
        }

        public void RunInitialization()
        {
            AddInTree.Load(this.addInFiles, this.disabledAddIns);
            LoggingService.Info("执行所有自动运行命令...");
            foreach (ICommand command in AddInTree.BuildItems<ICommand>("/Workspace/Autostart", null, false))
            {
                LoggingService.InfoFormatted("运行自运行命令：{0}", new object[] { command.GetType() });
                command.Run();
            }
        }

        public void RunInitialization(string addinPath)
        {
            AddInTree.Load(this.addInFiles, this.disabledAddIns);
            LoggingService.Info("Running autostart commands...");
            if (!string.IsNullOrEmpty(addinPath))
            {
                foreach (ICommand command in AddInTree.BuildItems<ICommand>(addinPath, null, false))
                {
                    command.Run();
                }
            }
        }

        public void StartCoreServices()
        {
            string path = FileUtility.Combine(new string[] { Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".SkyMapSoft" });
            string str2 = FileUtility.Combine(new string[] { path, this.applicationName });
            if (Directory.Exists(path) && !Directory.Exists(str2))
            {
                string[] directories = Directory.GetDirectories(path);
                int index = 0;
                while (index < directories.Length)
                {
                    string str3 = directories[index];
                    str2 = str3;
                    break;
                }
            }
            this.configDirectory = PropertyService.ConfigDirectory;
            LoggingService.DebugFormatted("原来的配置文件夹'{0}',新的配置文件夹'{1}'", new object[] { str2, this.configDirectory });
            if (((Directory.GetFiles(this.configDirectory, "*" + this.propertiesName + ".xml") == null) || (Directory.GetFiles(this.configDirectory, "*" + this.propertiesName + ".xml").Length == 0)) && Directory.Exists(str2))
            {
                LoggingService.DebugFormatted("存在原来的配置文件夹‘{0}’，将复制并删除原来的配置 ... ", new object[] { str2 });
                try
                {
                    FileUtility.DeepCopy(str2, this.configDirectory, false);
                }
                catch
                {
                }
                try
                {
                    Directory.Delete(str2, true);
                }
                catch (Exception exception)
                {
                    LoggingService.Error("删除原来配置文件目录时出错", exception);
                }
            }
            if (this.configDirectory == null)
            {
            }
            PropertyService.InitializeService(this.configDirectory, this.dataDirectory ?? Path.Combine(FileUtility.ApplicationRootPath, "data"), this.propertiesName);
            PropertyService.Load();
            ResourceService.InitializeService(FileUtility.Combine(new string[] { PropertyService.DataDirectory, "resources" }));
            SkyMap.Net.Core.StringParser.Properties["AppName"] = this.applicationName;
        }

        public string ConfigDirectory
        {
            get
            {
                return this.configDirectory;
            }
            set
            {
            }
        }

        public string DataDirectory
        {
            get
            {
                return this.dataDirectory;
            }
            set
            {
                this.dataDirectory = value;
            }
        }

        public string PropertiesName
        {
            get
            {
                return this.propertiesName;
            }
            set
            {
                if ((value == null) || (value.Length == 0))
                {
                    throw new ArgumentNullException("value");
                }
                this.propertiesName = value;
            }
        }
    }
}

