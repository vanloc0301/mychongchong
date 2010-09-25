namespace SkyMap.Net.XMLExchange
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, XmlRoot(Namespace="http://www.eappxml.gov.cn/eAppXML", IsNullable=false), DesignerCategory("code"), XmlType(AnonymousType=true), GeneratedCode("xsd", "2.0.50727.42"), DebuggerStepThrough]
    public class Form
    {
        private byte[] formDataField;
        private string formDescField;

        [XmlElement(DataType="base64Binary")]
        public byte[] FormData
        {
            get
            {
                return this.formDataField;
            }
            set
            {
                this.formDataField = value;
            }
        }

        public string FormDesc
        {
            get
            {
                return this.formDescField;
            }
            set
            {
                this.formDescField = value;
            }
        }
    }
}

