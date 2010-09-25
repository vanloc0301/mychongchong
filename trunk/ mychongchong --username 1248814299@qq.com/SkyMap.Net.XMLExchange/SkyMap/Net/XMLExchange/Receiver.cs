namespace SkyMap.Net.XMLExchange
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, XmlType(AnonymousType=true), DesignerCategory("code"), XmlRoot(Namespace="http://www.eappxml.gov.cn/eAppXML", IsNullable=false), GeneratedCode("xsd", "2.0.50727.42"), DebuggerStepThrough]
    public class Receiver
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

