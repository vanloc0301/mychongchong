namespace SkyMap.Net.XMLExchange
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, DesignerCategory("code"), GeneratedCode("xsd", "2.0.50727.42"), XmlRoot(Namespace="http://www.eappxml.gov.cn/eAppXML", IsNullable=false), XmlType(AnonymousType=true), DebuggerStepThrough]
    public class RouterItem
    {
        private string descriptionField;
        private string routerIdentifierField;
        private DateTime routerTimeField;

        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        public string RouterIdentifier
        {
            get
            {
                return this.routerIdentifierField;
            }
            set
            {
                this.routerIdentifierField = value;
            }
        }

        public DateTime RouterTime
        {
            get
            {
                return this.routerTimeField;
            }
            set
            {
                this.routerTimeField = value;
            }
        }
    }
}

