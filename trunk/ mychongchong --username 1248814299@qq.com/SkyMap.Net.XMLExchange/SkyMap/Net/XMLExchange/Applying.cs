namespace SkyMap.Net.XMLExchange
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, XmlType(AnonymousType=true), DebuggerStepThrough, XmlRoot(Namespace="http://www.eappxml.gov.cn/eAppXML", IsNullable=false), DesignerCategory("code"), GeneratedCode("xsd", "2.0.50727.42")]
    public class Applying
    {
        private ApplyingAttachmentInfo[] applyingAttachmentsField;
        private SkyMap.Net.XMLExchange.Form formField;
        private string projectCodeField;
        private SkyMap.Net.XMLExchange.ProjectSummary projectSummaryField;
        private SkyMap.Net.XMLExchange.ProposorInformation proposorInformationField;

        [XmlArrayItem("ApplyingAttachmentInfo", IsNullable=false)]
        public ApplyingAttachmentInfo[] ApplyingAttachments
        {
            get
            {
                return this.applyingAttachmentsField;
            }
            set
            {
                this.applyingAttachmentsField = value;
            }
        }

        public SkyMap.Net.XMLExchange.Form Form
        {
            get
            {
                return this.formField;
            }
            set
            {
                this.formField = value;
            }
        }

        [XmlAttribute]
        public string ProjectCode
        {
            get
            {
                return this.projectCodeField;
            }
            set
            {
                this.projectCodeField = value;
            }
        }

        public SkyMap.Net.XMLExchange.ProjectSummary ProjectSummary
        {
            get
            {
                return this.projectSummaryField;
            }
            set
            {
                this.projectSummaryField = value;
            }
        }

        public SkyMap.Net.XMLExchange.ProposorInformation ProposorInformation
        {
            get
            {
                return this.proposorInformationField;
            }
            set
            {
                this.proposorInformationField = value;
            }
        }
    }
}

