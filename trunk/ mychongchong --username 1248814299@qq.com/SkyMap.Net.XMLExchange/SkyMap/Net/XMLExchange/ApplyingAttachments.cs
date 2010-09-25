namespace SkyMap.Net.XMLExchange
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, GeneratedCode("xsd", "2.0.50727.42"), XmlRoot(Namespace="http://www.eappxml.gov.cn/eAppXML", IsNullable=false), DebuggerStepThrough, DesignerCategory("code"), XmlType(AnonymousType=true)]
    public class ApplyingAttachments
    {
        private SkyMap.Net.XMLExchange.ApplyingAttachmentInfo[] applyingAttachmentInfoField;

        [XmlElement("ApplyingAttachmentInfo")]
        public SkyMap.Net.XMLExchange.ApplyingAttachmentInfo[] ApplyingAttachmentInfo
        {
            get
            {
                return this.applyingAttachmentInfoField;
            }
            set
            {
                this.applyingAttachmentInfoField = value;
            }
        }
    }
}

