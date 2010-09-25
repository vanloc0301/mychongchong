namespace SkyMap.Net.XMLExchange
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, DebuggerStepThrough, XmlRoot(Namespace="http://www.eappxml.gov.cn/eAppXML", IsNullable=false), XmlType(AnonymousType=true), GeneratedCode("xsd", "2.0.50727.42"), DesignerCategory("code")]
    public class OpinionAttachments
    {
        private SkyMap.Net.XMLExchange.OpinionAttachmentInfo[] opinionAttachmentInfoField;

        [XmlElement("OpinionAttachmentInfo")]
        public SkyMap.Net.XMLExchange.OpinionAttachmentInfo[] OpinionAttachmentInfo
        {
            get
            {
                return this.opinionAttachmentInfoField;
            }
            set
            {
                this.opinionAttachmentInfoField = value;
            }
        }
    }
}

