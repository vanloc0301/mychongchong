namespace SkyMap.Net.XMLExchange
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, DesignerCategory("code"), XmlRoot(Namespace="http://www.eappxml.gov.cn/eAppXML", IsNullable=false), GeneratedCode("xsd", "2.0.50727.42"), XmlType(AnonymousType=true), DebuggerStepThrough]
    public class Result
    {
        private ResultAttachmentInfo[] resultAttachmentsField;
        private byte[] resultDataField;
        private string resultDescField;

        [XmlArrayItem("ResultAttachmentInfo", IsNullable=false)]
        public ResultAttachmentInfo[] ResultAttachments
        {
            get
            {
                return this.resultAttachmentsField;
            }
            set
            {
                this.resultAttachmentsField = value;
            }
        }

        [XmlElement(DataType="base64Binary")]
        public byte[] ResultData
        {
            get
            {
                return this.resultDataField;
            }
            set
            {
                this.resultDataField = value;
            }
        }

        public string ResultDesc
        {
            get
            {
                return this.resultDescField;
            }
            set
            {
                this.resultDescField = value;
            }
        }
    }
}

