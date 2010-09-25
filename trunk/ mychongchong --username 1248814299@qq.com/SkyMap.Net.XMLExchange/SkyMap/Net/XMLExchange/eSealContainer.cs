namespace SkyMap.Net.XMLExchange
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, DebuggerStepThrough, DesignerCategory("code"), XmlType(AnonymousType=true), XmlRoot(Namespace="http://www.egxml.gov.cn/message", IsNullable=false), GeneratedCode("xsd", "2.0.50727.42")]
    public class eSealContainer
    {
        private SkyMap.Net.XMLExchange.eSeal[] eSealField;

        [XmlElement("eSeal")]
        public SkyMap.Net.XMLExchange.eSeal[] eSeal
        {
            get
            {
                return this.eSealField;
            }
            set
            {
                this.eSealField = value;
            }
        }
    }
}

