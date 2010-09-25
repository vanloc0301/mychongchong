namespace AutoUpdate
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Net;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using System.Xml;

    public class AppUpdater : IDisposable
    {
        private Component component = new Component();
        private bool disposed;
        private IntPtr handle;

        public int CheckForUpdate(string temppath, string serverXmlFile, string localXmlFile, out List<string[]> updateFileList)
        {
            updateFileList = new List<string[]>();
            if (!System.IO.File.Exists(localXmlFile) || !System.IO.File.Exists(serverXmlFile))
            {
                return -1;
            }
            XmlFiles files = new XmlFiles(serverXmlFile);
            XmlFiles files2 = new XmlFiles(localXmlFile);
            XmlNodeList nodeList = files.GetNodeList("AutoUpdater/Components");
            XmlNodeList list2 = files2.GetNodeList("AutoUpdater/Components");
            Dictionary<string, XmlNode> dictionary = new Dictionary<string, XmlNode>(list2.Count);
            string[] commandLineArgs = Environment.GetCommandLineArgs();
            if (((commandLineArgs == null) || (commandLineArgs.Length <= 1)) || (commandLineArgs[1].ToLower() != "true"))
            {
                for (int j = 0; j < list2.Count; j++)
                {
                    dictionary.Add(list2.Item(j).Attributes["Name"].Value.Trim(), list2[j]);
                }
            }
            int num2 = 0;
            bool result = false;
            try
            {
                string nodeValue = files.GetNodeValue("//reset");
                if (!string.IsNullOrEmpty(nodeValue))
                {
                    bool.TryParse(nodeValue, out result);
                }
            }
            catch
            {
            }
            Dictionary<string, bool> dictionary2 = new Dictionary<string, bool>();
            for (int i = nodeList.Count - 1; i >= 0; i--)
            {
                XmlNode node = nodeList[i];
                string componentName = node.Attributes["Name"].Value.Trim();
                string updateUrl = node.Attributes["Url"].Value.Trim();
                string path = this.DownAutoUpdateFile(temppath, componentName, updateUrl);
                string str5 = Path.Combine(Environment.CurrentDirectory, string.Format("{0}component.xml", componentName));
                if (!System.IO.File.Exists(path))
                {
                    MessageBox.Show("找不到文件:" + path);
                }
                XmlNode node2 = null;
                if (!System.IO.File.Exists(str5))
                {
                    foreach (XmlNode node3 in list2)
                    {
                        if (node3.Attributes["Name"].Value.Trim() == componentName)
                        {
                            foreach (XmlNode node4 in node3.ChildNodes)
                            {
                                if (node4.Name == "Files")
                                {
                                    node2 = node4;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                else
                {
                    XmlFiles files3 = new XmlFiles(str5);
                    foreach (XmlNode node5 in files3.GetNodeList("Component"))
                    {
                        if (node5.Name == "Files")
                        {
                            node2 = node5;
                            break;
                        }
                    }
                }
                XmlFiles files4 = new XmlFiles(path);
                foreach (XmlNode node6 in files4.GetNodeList("Component"))
                {
                    if (!(node6.Name == "Files"))
                    {
                        continue;
                    }
                    bool flag2 = dictionary.ContainsKey(componentName);
                    Dictionary<string, string> dictionary3 = new Dictionary<string, string>();
                    if (flag2 && (node2 != null))
                    {
                        foreach (XmlNode node7 in node2.ChildNodes)
                        {
                            if (node7.Name == "File")
                            {
                                if (!dictionary3.ContainsKey(node7.Attributes["Name"].Value.Trim()))
                                {
                                    dictionary3.Add(node7.Attributes["Name"].Value.Trim(), node7.Attributes["Ver"].Value.Trim());
                                }
                                else
                                {
                                    MessageBox.Show(string.Format("包含重复的升级项目文件:{0},系统将忽略!", node7.Attributes["Name"].Value.Trim()));
                                }
                            }
                        }
                    }
                    foreach (XmlNode node8 in node6.ChildNodes)
                    {
                        if (!(node8.Name == "File"))
                        {
                            continue;
                        }
                        string[] item = new string[5];
                        string key = node8.Attributes["Name"].Value.Trim();
                        string str7 = node8.Attributes["Ver"].Value.Trim();
                        if (!dictionary3.ContainsKey(key) || (dictionary3.ContainsKey(key) && ((str7.CompareTo(dictionary3[key]) > 0) || (result && (str7.CompareTo(dictionary3[key]) < 0)))))
                        {
                            item[0] = key;
                            item[1] = str7;
                            item[3] = updateUrl;
                            if (!dictionary3.ContainsKey(key) || (str7.CompareTo(dictionary3[key]) > 0))
                            {
                                item[4] = key;
                            }
                            else
                            {
                                item[4] = key + str7;
                            }
                            if (!dictionary2.ContainsKey(key))
                            {
                                updateFileList.Add(item);
                                dictionary2.Add(key, true);
                            }
                            num2++;
                        }
                    }
                }
            }
            return num2;
        }

        [DllImport("Kernel32")]
        private static extern bool CloseHandle(IntPtr handle);
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.component.Dispose();
                }
                CloseHandle(this.handle);
                this.handle = IntPtr.Zero;
            }
            this.disposed = true;
        }

        public string DownAutoUpdateFile(string downpath, string componentName, string updateUrl)
        {
            if (!Directory.Exists(downpath))
            {
                Directory.CreateDirectory(downpath);
            }
            string filename = string.Format("{0}/{1}{2}.xml", downpath, componentName, string.IsNullOrEmpty(componentName) ? "UpdateList" : "component");
            string requestUriString = updateUrl + (string.IsNullOrEmpty(componentName) ? "/UpdateList.xml" : "/component.xml");
            try
            {
                WebRequest request = (FtpWebRequest) WebRequest.Create(requestUriString);
                request.Proxy = null;
                WebResponse response = request.GetResponse();
                XmlDocument document = new XmlDocument();
                document.Load(response.GetResponseStream());
                document.Save(filename);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, string.Format("下载{0}失败", requestUriString));
                return string.Empty;
            }
            return filename;
        }

        ~AppUpdater()
        {
            this.Dispose(false);
        }
    }
}

