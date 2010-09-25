namespace SkyMap.Net.XMLExchange
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, XmlRoot(Namespace="http://www.eappxml.gov.cn/eAppXML", IsNullable=false), DebuggerStepThrough, XmlType(AnonymousType=true), GeneratedCode("xsd", "2.0.50727.42"), DesignerCategory("code")]
    public class Senders
    {
        private SkyMap.Net.XMLExchange.Sender[] senderField;

        [XmlElement("Sender")]
        public SkyMap.Net.XMLExchange.Sender[] Sender
        {
            get
            {
                return this.senderField;
            }
            set
            {
                this.senderField = value;
            }
        }
    }
}

