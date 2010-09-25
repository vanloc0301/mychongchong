namespace SkyMap.Net.XMLExchange
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, DesignerCategory("code"), XmlRoot(Namespace="http://www.eappxml.gov.cn/eAppXML", IsNullable=false), GeneratedCode("xsd", "2.0.50727.42"), DebuggerStepThrough, XmlType(AnonymousType=true)]
    public class Procdure
    {
        private SkyMap.Net.XMLExchange.Opinion[] opinionField;

        [XmlElement("Opinion")]
        public SkyMap.Net.XMLExchange.Opinion[] Opinion
        {
            get
            {
                return this.opinionField;
            }
            set
            {
                this.opinionField = value;
            }
        }
    }
}

