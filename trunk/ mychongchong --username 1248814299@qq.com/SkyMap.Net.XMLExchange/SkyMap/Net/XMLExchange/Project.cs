namespace SkyMap.Net.XMLExchange
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, GeneratedCode("xsd", "2.0.50727.42"), DesignerCategory("code"), XmlType(AnonymousType=true), XmlRoot(Namespace="http://www.eappxml.gov.cn/eAppXML", IsNullable=false), DebuggerStepThrough]
    public class Project
    {
        private SkyMap.Net.XMLExchange.Applying applyingField;
        private Opinion[] procdureField;
        private SkyMap.Net.XMLExchange.Result resultField;

        public SkyMap.Net.XMLExchange.Applying Applying
        {
            get
            {
                return this.applyingField;
            }
            set
            {
                this.applyingField = value;
            }
        }

        [XmlArrayItem("Opinion", IsNullable=false)]
        public Opinion[] Procdure
        {
            get
            {
                return this.procdureField;
            }
            set
            {
                this.procdureField = value;
            }
        }

        public SkyMap.Net.XMLExchange.Result Result
        {
            get
            {
                return this.resultField;
            }
            set
            {
                this.resultField = value;
            }
        }
    }
}

