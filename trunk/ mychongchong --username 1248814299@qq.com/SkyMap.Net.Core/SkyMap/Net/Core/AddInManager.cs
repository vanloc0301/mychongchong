namespace SkyMap.Net.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Xml;

    public static class AddInManager
    {
        private static string addInInstallTemp;
        private static string configurationFileName;
        private static string userAddInPath;

        public static void AbortRemoveUserAddInOnNextStart(string identity)
        {
            string path = Path.Combine(addInInstallTemp, "remove.txt");
            if (File.Exists(path))
            {
                List<string> list = new List<string>();
                using (StreamReader reader = new StreamReader(path))
                {
                    string str2;
                    while ((str2 = reader.ReadLine()) != null)
                    {
                        str2 = str2.Trim();
                        if (str2.Length > 0)
                        {
                            list.Add(str2);
                        }
                    }
                }
                if (list.Remove(identity))
                {
                    using (StreamWriter writer = new StreamWriter(path))
                    {
                        list.ForEach(new Action<string>(writer.WriteLine));
                    }
                }
            }
        }

        public static void AddExternalAddIns(IList<AddIn> addIns)
        {
            List<string> addInFiles = new List<string>();
            List<string> disabledAddIns = new List<string>();
            LoadAddInConfiguration(addInFiles, disabledAddIns);
            foreach (AddIn @in in addIns)
            {
                if (!addInFiles.Contains(@in.FileName))
                {
                    addInFiles.Add(@in.FileName);
                }
                @in.Enabled = false;
                @in.Action = AddInAction.Install;
                AddInTree.InsertAddIn(@in);
            }
            SaveAddInConfiguration(addInFiles, disabledAddIns);
        }

        public static void Disable(IList<AddIn> addIns)
        {
            List<string> addInFiles = new List<string>();
            List<string> disabledAddIns = new List<string>();
            LoadAddInConfiguration(addInFiles, disabledAddIns);
            foreach (AddIn @in in addIns)
            {
                string primaryIdentity = @in.Manifest.PrimaryIdentity;
                if (primaryIdentity == null)
                {
                    throw new ArgumentException("The AddIn cannot be disabled because it has no identity.");
                }
                if (!disabledAddIns.Contains(primaryIdentity))
                {
                    disabledAddIns.Add(primaryIdentity);
                }
                @in.Action = AddInAction.Disable;
            }
            SaveAddInConfiguration(addInFiles, disabledAddIns);
        }

        public static void Enable(IList<AddIn> addIns)
        {
            List<string> addInFiles = new List<string>();
            List<string> disabledAddIns = new List<string>();
            LoadAddInConfiguration(addInFiles, disabledAddIns);
            foreach (AddIn @in in addIns)
            {
                foreach (string str in @in.Manifest.Identities.Keys)
                {
                    disabledAddIns.Remove(str);
                }
                if (@in.Action == AddInAction.Uninstall)
                {
                    if (FileUtility.IsBaseDirectory(userAddInPath, @in.FileName))
                    {
                        foreach (string str in @in.Manifest.Identities.Keys)
                        {
                            AbortRemoveUserAddInOnNextStart(str);
                        }
                    }
                    else if (!addInFiles.Contains(@in.FileName))
                    {
                        addInFiles.Add(@in.FileName);
                    }
                }
                @in.Action = AddInAction.Enable;
            }
            SaveAddInConfiguration(addInFiles, disabledAddIns);
        }

        public static void InstallAddIns(List<string> disabled)
        {
            if (Directory.Exists(addInInstallTemp))
            {
                string fileName;
                string str3;
                LoggingService.Info("AddInManager.InstallAddIns started");
                if (!Directory.Exists(userAddInPath))
                {
                    Directory.CreateDirectory(userAddInPath);
                }
                string path = Path.Combine(addInInstallTemp, "remove.txt");
                bool flag = true;
                List<string> list = new List<string>();
                if (File.Exists(path))
                {
                    using (StreamReader reader = new StreamReader(path))
                    {
                        while ((fileName = reader.ReadLine()) != null)
                        {
                            fileName = fileName.Trim();
                            if (fileName.Length != 0)
                            {
                                str3 = Path.Combine(userAddInPath, fileName);
                                if (!UninstallAddIn(disabled, fileName, str3))
                                {
                                    list.Add(fileName);
                                    flag = false;
                                }
                            }
                        }
                    }
                    if (list.Count == 0)
                    {
                        LoggingService.Info("Deleting remove.txt");
                        File.Delete(path);
                    }
                    else
                    {
                        LoggingService.Info("Rewriting remove.txt");
                        using (StreamWriter writer = new StreamWriter(path))
                        {
                            list.ForEach(new Action<string>(writer.WriteLine));
                        }
                    }
                }
                foreach (string str4 in Directory.GetDirectories(addInInstallTemp))
                {
                    fileName = Path.GetFileName(str4);
                    str3 = Path.Combine(userAddInPath, fileName);
                    if (list.Contains(fileName))
                    {
                        LoggingService.Info("Skipping installation of " + fileName + " because deinstallation failed.");
                    }
                    else if (UninstallAddIn(disabled, fileName, str3))
                    {
                        LoggingService.Info("Installing " + fileName + "...");
                        Directory.Move(str4, str3);
                    }
                    else
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    try
                    {
                        Directory.Delete(addInInstallTemp, false);
                    }
                    catch (Exception exception)
                    {
                        LoggingService.Warn("Error removing install temp", exception);
                    }
                }
                LoggingService.Info("AddInManager.InstallAddIns finished");
            }
        }

        public static void LoadAddInConfiguration(List<string> addInFiles, List<string> disabledAddIns)
        {
            if (File.Exists(configurationFileName))
            {
                using (XmlTextReader reader = new XmlTextReader(configurationFileName))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            if (reader.Name == "AddIn")
                            {
                                string attribute = reader.GetAttribute("file");
                                if ((attribute != null) && (attribute.Length > 0))
                                {
                                    addInFiles.Add(attribute);
                                }
                            }
                            else if (reader.Name == "Disable")
                            {
                                string item = reader.GetAttribute("addin");
                                if ((item != null) && (item.Length > 0))
                                {
                                    disabledAddIns.Add(item);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void RemoveExternalAddIns(IList<AddIn> addIns)
        {
            List<string> addInFiles = new List<string>();
            List<string> disabledAddIns = new List<string>();
            LoadAddInConfiguration(addInFiles, disabledAddIns);
            foreach (AddIn @in in addIns)
            {
                foreach (string str in @in.Manifest.Identities.Keys)
                {
                    disabledAddIns.Remove(str);
                }
                addInFiles.Remove(@in.FileName);
                @in.Action = AddInAction.Uninstall;
                if (!@in.Enabled)
                {
                    AddInTree.RemoveAddIn(@in);
                }
            }
            SaveAddInConfiguration(addInFiles, disabledAddIns);
        }

        public static void RemoveUserAddInOnNextStart(string identity)
        {
            List<string> list = new List<string>();
            string path = Path.Combine(addInInstallTemp, "remove.txt");
            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string str2;
                    while ((str2 = reader.ReadLine()) != null)
                    {
                        str2 = str2.Trim();
                        if (str2.Length > 0)
                        {
                            list.Add(str2);
                        }
                    }
                }
                if (list.Contains(identity))
                {
                    return;
                }
            }
            list.Add(identity);
            if (!Directory.Exists(addInInstallTemp))
            {
                Directory.CreateDirectory(addInInstallTemp);
            }
            using (StreamWriter writer = new StreamWriter(path))
            {
                list.ForEach(new Action<string>(writer.WriteLine));
            }
        }

        public static void SaveAddInConfiguration(List<string> addInFiles, List<string> disabledAddIns)
        {
            using (XmlTextWriter writer = new XmlTextWriter(configurationFileName, Encoding.UTF8))
            {
                writer.Formatting = Formatting.Indented;
                writer.WriteStartDocument();
                writer.WriteStartElement("AddInConfiguration");
                foreach (string str in addInFiles)
                {
                    writer.WriteStartElement("AddIn");
                    writer.WriteAttributeString("file", str);
                    writer.WriteEndElement();
                }
                foreach (string str2 in disabledAddIns)
                {
                    writer.WriteStartElement("Disable");
                    writer.WriteAttributeString("addin", str2);
                    writer.WriteEndElement();
                }
                writer.WriteEndDocument();
            }
        }

        private static bool UninstallAddIn(List<string> disabled, string addInName, string targetDir)
        {
            if (Directory.Exists(targetDir))
            {
                LoggingService.Info("Removing " + addInName + "...");
                try
                {
                    Directory.Delete(targetDir, true);
                }
                catch (Exception exception)
                {
                    disabled.Add(addInName);
                    MessageService.ShowError("Error removing " + addInName + ":\n" + exception.Message + "\nThe AddIn will be removed on the next start of " + MessageService.ProductName + " and is disabled for now.");
                    return false;
                }
            }
            return true;
        }

        public static string AddInInstallTemp
        {
            get
            {
                return addInInstallTemp;
            }
            set
            {
                addInInstallTemp = value;
            }
        }

        public static string ConfigurationFileName
        {
            get
            {
                return configurationFileName;
            }
            set
            {
                configurationFileName = value;
            }
        }

        public static string UserAddInPath
        {
            get
            {
                return userAddInPath;
            }
            set
            {
                userAddInPath = value;
            }
        }
    }
}

