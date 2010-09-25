namespace SkyMap.Net.XMLExchange
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, GeneratedCode("xsd", "2.0.50727.42"), DebuggerStepThrough, DesignerCategory("code"), XmlType(AnonymousType=true), XmlRoot(Namespace="http://www.eappxml.gov.cn/eAppXML", IsNullable=false)]
    public class QueryResult
    {
        private string resultNameField;
        private string resultValueField;

        [XmlAttribute]
        public string ResultName
        {
            get
            {
                return this.resultNameField;
            }
            set
            {
                this.resultNameField = value;
            }
        }

        [XmlAttribute]
        public string ResultValue
        {
            get
            {
                return this.resultValueField;
            }
            set
            {
                this.resultValueField = value;
            }
        }
    }
}

