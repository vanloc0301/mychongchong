namespace SkyMap.Net.XMLExchange
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, XmlRoot(Namespace="http://www.eappxml.gov.cn/eAppXML", IsNullable=false), DebuggerStepThrough, DesignerCategory("code"), GeneratedCode("xsd", "2.0.50727.42"), XmlType(AnonymousType=true)]
    public class Receivers
    {
        private SkyMap.Net.XMLExchange.Receiver[] receiverField;

        [XmlElement("Receiver")]
        public SkyMap.Net.XMLExchange.Receiver[] Receiver
        {
            get
            {
                return this.receiverField;
            }
            set
            {
                this.receiverField = value;
            }
        }
    }
}

