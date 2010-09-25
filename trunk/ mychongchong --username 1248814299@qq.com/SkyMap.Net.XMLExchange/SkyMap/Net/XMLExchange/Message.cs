namespace SkyMap.Net.XMLExchange
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, DesignerCategory("code"), GeneratedCode("xsd", "2.0.50727.42"), XmlType(AnonymousType=true), XmlRoot(Namespace="http://www.egxml.gov.cn/message", IsNullable=false), DebuggerStepThrough]
    public class Message
    {
        private string displayField;
        private string dSignDataField;
        private string dSignField;
        private SkyMap.Net.XMLExchange.eAppXML eAppXMLField;
        private eSeal[] eSealContainerField;
        private SkyMap.Net.XMLExchange.eSign eSignField;
        private string versionField;

        public string display
        {
            get
            {
                return this.displayField;
            }
            set
            {
                this.displayField = value;
            }
        }

        public string dSign
        {
            get
            {
                return this.dSignField;
            }
            set
            {
                this.dSignField = value;
            }
        }

        public string dSignData
        {
            get
            {
                return this.dSignDataField;
            }
            set
            {
                this.dSignDataField = value;
            }
        }

        [XmlElement(Namespace="http://www.eappxml.gov.cn/eAppXML")]
        public SkyMap.Net.XMLExchange.eAppXML eAppXML
        {
            get
            {
                return this.eAppXMLField;
            }
            set
            {
                this.eAppXMLField = value;
            }
        }

        [XmlArrayItem("eSeal", IsNullable=false)]
        public eSeal[] eSealContainer
        {
            get
            {
                return this.eSealContainerField;
            }
            set
            {
                this.eSealContainerField = value;
            }
        }

        public SkyMap.Net.XMLExchange.eSign eSign
        {
            get
            {
                return this.eSignField;
            }
            set
            {
                this.eSignField = value;
            }
        }

        [XmlAttribute]
        public string Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }
    }
}

