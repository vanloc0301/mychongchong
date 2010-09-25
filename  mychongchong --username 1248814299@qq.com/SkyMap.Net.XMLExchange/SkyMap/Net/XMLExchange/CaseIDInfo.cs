namespace SkyMap.Net.XMLExchange
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, XmlRoot(Namespace="http://www.eappxml.gov.cn/eAppXML", IsNullable=false), DesignerCategory("code"), XmlType(AnonymousType=true), DebuggerStepThrough, GeneratedCode("xsd", "2.0.50727.42")]
    public class CaseIDInfo
    {
        private string applyingIDField;
        private string codeIDField;

        public string ApplyingID
        {
            get
            {
                return this.applyingIDField;
            }
            set
            {
                this.applyingIDField = value;
            }
        }

        public string CodeID
        {
            get
            {
                return this.codeIDField;
            }
            set
            {
                this.codeIDField = value;
            }
        }
    }
}

