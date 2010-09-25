namespace SkyMap.Net.XMLExchange
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, GeneratedCode("xsd", "2.0.50727.42"), DebuggerStepThrough, XmlType(AnonymousType=true), XmlRoot(Namespace="http://www.eappxml.gov.cn/eAppXML", IsNullable=false), DesignerCategory("code")]
    public class eAppXML
    {
        private SkyMap.Net.XMLExchange.Body bodyField;
        private SkyMap.Net.XMLExchange.Head headField;
        private string versionField;

        public SkyMap.Net.XMLExchange.Body Body
        {
            get
            {
                return this.bodyField;
            }
            set
            {
                this.bodyField = value;
            }
        }

        public SkyMap.Net.XMLExchange.Head Head
        {
            get
            {
                return this.headField;
            }
            set
            {
                this.headField = value;
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

