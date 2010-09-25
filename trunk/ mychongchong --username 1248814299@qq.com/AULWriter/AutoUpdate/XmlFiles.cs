namespace AutoUpdate
{
    using System;
    using System.IO;
    using System.Xml;

    public class XmlFiles : XmlDocument
    {
        public XmlFiles(Stream reader)
        {
            this.Load(reader);
        }

        public XmlFiles(string xmlFile)
        {
            this.Load(xmlFile);
        }

        public XmlFiles(XmlReader reader)
        {
            this.Load(reader);
        }

        public XmlNode FindNode(string xPath)
        {
            return base.SelectSingleNode(xPath);
        }

        public XmlNodeList GetNodeList(string xPath)
        {
            return base.SelectSingleNode(xPath).ChildNodes;
        }

        public string GetNodeValue(string xPath)
        {
            return base.SelectSingleNode(xPath).InnerText;
        }
    }
}

