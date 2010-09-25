namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui.Dialogs;
    using System;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.Remoting;
    using System.Windows.Forms;
    using System.Xml;

    public sealed class RemoteHelper
    {
        public static HybridDictionary Servers = new HybridDictionary(1);
        public static bool useRemote;

        private RemoteHelper()
        {
        }

        public static void Initialize()
        {
            string serversConfigFileName = ServersConfigFileName;
            if (!File.Exists(serversConfigFileName))
            {
                PropertyService.Set<string>("DefaultServer", string.Empty);
            }
            else
            {
                XmlDocument document = new XmlDocument();
                document.Load(serversConfigFileName);
                XmlNodeList list = document.SelectNodes("/Servers/Server");
                string str2 = PropertyService.Get<string>("DefaultServer", string.Empty);
                bool flag = PropertyService.Get<bool>("IsHideServerDialogOnNext", false);
                string path = string.Empty;
                string key = string.Empty;
                string attribute = string.Empty;
                foreach (XmlNode node in list)
                {
                    if (node is XmlElement)
                    {
                        XmlElement element = node as XmlElement;
                        key = element.GetAttribute("name");
                        attribute = element.GetAttribute("value");
                        if (File.Exists(RemoteConfigDirectory + Path.DirectorySeparatorChar + attribute))
                        {
                            Servers.Add(key, attribute);
                            if (flag && (str2 == key))
                            {
                                path = RemoteConfigDirectory + Path.DirectorySeparatorChar + attribute;
                            }
                        }
                    }
                }
                if (Servers.Count == 0)
                {
                    PropertyService.Set<string>("DefaultServer", string.Empty);
                }
                else
                {
                    if (path == string.Empty)
                    {
                        if (Servers.Count == 1)
                        {
                            PropertyService.Set<string>("DefaultServer", key);
                            path = RemoteConfigDirectory + Path.DirectorySeparatorChar + attribute;
                        }
                        else
                        {
                            ServerDialog dialog = new ServerDialog();
                            try
                            {
                                dialog.InitServers(Servers);
                                if (dialog.ShowDialog() == DialogResult.OK)
                                {
                                    path = RemoteConfigDirectory + Path.DirectorySeparatorChar + ((string) Servers[dialog.ServerName]);
                                    PropertyService.Set<string>("DefaultServer", dialog.ServerName);
                                    PropertyService.Set<bool>("IsHideServerDialogOnNext", dialog.IsHideServerDialogOnNext);
                                }
                            }
                            finally
                            {
                                dialog.Close();
                            }
                            if (path == string.Empty)
                            {
                                if (MessageHelper.ShowYesNoInfo("你不想使用远程服务器登录吗?\r如果选择是将使用本地配置登录服务器，选择否将退出系统！") != DialogResult.No)
                                {
                                    PropertyService.Set<string>("DefaultServer", string.Empty);
                                    return;
                                }
                                Environment.Exit(0);
                            }
                            else
                            {
                                useRemote = true;
                            }
                        }
                    }
                    if (!File.Exists(path))
                    {
                        throw new FileNotFoundException("不能找到远程调用配置文件：" + path);
                    }
                    RemotingConfiguration.Configure(path, false);
                }
            }
        }

        public static void Reset()
        {
            PropertyService.Set<bool>("IsHideServerDialogOnNext", false);
            PropertyService.Save();
            (WorkbenchSingleton.Workbench as Form).Close();
            Process.Start(Application.ExecutablePath);
            Environment.Exit(0);
        }

        private static string RemoteConfigDirectory
        {
            get
            {
                return (PropertyService.DataDirectory + Path.DirectorySeparatorChar + "Remote");
            }
        }

        private static string ServersConfigFileName
        {
            get
            {
                return (RemoteConfigDirectory + Path.DirectorySeparatorChar + "RemoteServers.xml");
            }
        }
    }
}

