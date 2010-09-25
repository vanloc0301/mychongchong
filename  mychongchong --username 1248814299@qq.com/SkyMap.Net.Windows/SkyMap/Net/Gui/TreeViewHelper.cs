namespace SkyMap.Net.Gui
{
    using System;
    using System.Windows.Forms;
    using System.Xml;
    using SkyMap.Net.Core;
    using System.IO;

    public class TreeViewHelper
    {
        private static string xmlPath = string.Concat(new object[] { PropertyService.DataDirectory, Path.PathSeparator, "TreeView", Path.PathSeparator });

        private static TreeNode AddNode(TreeNodeCollection nodes, XmlElement e)
        {
            TreeNode node = nodes.Add(e.GetAttribute("text"));
            foreach (XmlNode node2 in e.ChildNodes)
            {
                if ((node2 is XmlElement) && (node2.Name == "node"))
                {
                    AddNode(node.Nodes, node2 as XmlElement);
                }
            }
            return node;
        }

        public static void InitFormXML(TreeView treeView, string xmlFile)
        {
            XmlDocument document = new XmlDocument();
            document.Load(xmlPath + xmlFile);
            XmlNodeList elementsByTagName = document.GetElementsByTagName("nodes");
            if (elementsByTagName.Count == 1)
            {
                foreach (XmlNode node in elementsByTagName)
                {
                    if ((node is XmlElement) && (node.Name == "node"))
                    {
                        AddNode(treeView.Nodes, node as XmlElement);
                    }
                }
            }
        }
    }
}

