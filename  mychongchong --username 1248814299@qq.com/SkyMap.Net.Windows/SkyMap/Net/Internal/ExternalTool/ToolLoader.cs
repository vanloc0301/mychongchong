namespace SkyMap.Net.Internal.ExternalTool
{
    using SkyMap.Net.Core;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    public class ToolLoader
    {
        private static List<SkyMap.Net.Internal.ExternalTool.ExternalTool> tool = new List<SkyMap.Net.Internal.ExternalTool.ExternalTool>();
        private static string TOOLFILE = "SmApp-tools.xml";
        private static string TOOLFILEVERSION = "1";

        static ToolLoader()
        {
            if (!LoadToolsFromStream(Path.Combine(PropertyService.ConfigDirectory, TOOLFILE)) && !LoadToolsFromStream(FileUtility.Combine(new string[] { PropertyService.DataDirectory, "options", TOOLFILE })))
            {
                MessageService.ShowWarning("${res:Internal.ExternalTool.CantLoadToolConfigWarining}");
            }
        }

        private static bool LoadToolsFromStream(string filename)
        {
            if (!File.Exists(filename))
            {
                return false;
            }
            XmlDocument document = new XmlDocument();
            try
            {
                document.Load(filename);
                if (document.DocumentElement.Attributes["VERSION"].InnerText != TOOLFILEVERSION)
                {
                    return false;
                }
                tool = new List<SkyMap.Net.Internal.ExternalTool.ExternalTool>();
                XmlNodeList childNodes = document.DocumentElement.ChildNodes;
                foreach (XmlElement element in childNodes)
                {
                    tool.Add(new SkyMap.Net.Internal.ExternalTool.ExternalTool(element));
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static void SaveTools()
        {
            WriteToolsToFile(Path.Combine(PropertyService.ConfigDirectory, TOOLFILE));
        }

        private static void WriteToolsToFile(string fileName)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<TOOLS VERSION = \"" + TOOLFILEVERSION + "\" />");
            foreach (SkyMap.Net.Internal.ExternalTool.ExternalTool tool in ToolLoader.tool)
            {
                doc.DocumentElement.AppendChild(tool.ToXmlElement(doc));
            }
            FileUtility.ObservedSave(new NamedFileOperationDelegate(doc.Save), fileName, FileErrorPolicy.ProvideAlternative);
        }

        public static List<SkyMap.Net.Internal.ExternalTool.ExternalTool> Tool
        {
            get
            {
                return tool;
            }
            set
            {
                tool = value;
            }
        }
    }
}

