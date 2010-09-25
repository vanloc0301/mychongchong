namespace AutoUpdate
{
    using System;
    using System.Xml;

    public class XmlFiles : XmlDocument
    {
        private string _xmlFileName;

        public XmlFiles(string xmlFile)
        {
            this.XmlFileName = xmlFile;
            this.Load(xmlFile);
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

        public string XmlFileName
        {
            get
            {
                return this._xmlFileName;
            }
            set
            {
                this._xmlFileName = value;
            }
        }
    }
}

