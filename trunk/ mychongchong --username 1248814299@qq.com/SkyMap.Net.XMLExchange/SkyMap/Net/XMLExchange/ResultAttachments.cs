namespace SkyMap.Net.XMLExchange
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, DesignerCategory("code"), XmlRoot(Namespace="http://www.eappxml.gov.cn/eAppXML", IsNullable=false), XmlType(AnonymousType=true), GeneratedCode("xsd", "2.0.50727.42"), DebuggerStepThrough]
    public class ResultAttachments
    {
        private SkyMap.Net.XMLExchange.ResultAttachmentInfo[] resultAttachmentInfoField;

        [XmlElement("ResultAttachmentInfo")]
        public SkyMap.Net.XMLExchange.ResultAttachmentInfo[] ResultAttachmentInfo
        {
            get
            {
                return this.resultAttachmentInfoField;
            }
            set
            {
                this.resultAttachmentInfoField = value;
            }
        }
    }
}

