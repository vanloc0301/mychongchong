namespace SkyMap.Net.XMLExchange
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, XmlType(AnonymousType=true), XmlRoot(Namespace="http://www.eappxml.gov.cn/eAppXML", IsNullable=false), GeneratedCode("xsd", "2.0.50727.42"), DebuggerStepThrough, DesignerCategory("code")]
    public class Sender
    {
        private string identifierField;
        private string valueField;

        [XmlAttribute]
        public string Identifier
        {
            get
            {
                return this.identifierField;
            }
            set
            {
                this.identifierField = value;
            }
        }

        [XmlText]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }
}

