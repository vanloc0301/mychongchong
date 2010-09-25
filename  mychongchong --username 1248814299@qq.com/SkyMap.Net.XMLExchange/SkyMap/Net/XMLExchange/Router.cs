namespace SkyMap.Net.XMLExchange
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, XmlRoot(Namespace="http://www.eappxml.gov.cn/eAppXML", IsNullable=false), XmlType(AnonymousType=true), GeneratedCode("xsd", "2.0.50727.42"), DebuggerStepThrough, DesignerCategory("code")]
    public class Router
    {
        private SkyMap.Net.XMLExchange.RouterItem[] routerItemField;

        [XmlElement("RouterItem")]
        public SkyMap.Net.XMLExchange.RouterItem[] RouterItem
        {
            get
            {
                return this.routerItemField;
            }
            set
            {
                this.routerItemField = value;
            }
        }
    }
}

