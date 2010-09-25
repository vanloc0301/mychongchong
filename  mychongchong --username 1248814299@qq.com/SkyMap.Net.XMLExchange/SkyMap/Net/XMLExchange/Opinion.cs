namespace SkyMap.Net.XMLExchange
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, XmlRoot(Namespace="http://www.eappxml.gov.cn/eAppXML", IsNullable=false), GeneratedCode("xsd", "2.0.50727.42"), DebuggerStepThrough, DesignerCategory("code"), XmlType(AnonymousType=true)]
    public class Opinion
    {
        private DateTime approveDateField;
        private string approverField;
        private string departmentField;
        private string detailField;
        private OpinionAttachmentInfo[] opinionAttachmentsField;
        private string processField;

        public DateTime ApproveDate
        {
            get
            {
                return this.approveDateField;
            }
            set
            {
                this.approveDateField = value;
            }
        }

        public string Approver
        {
            get
            {
                return this.approverField;
            }
            set
            {
                this.approverField = value;
            }
        }

        public string Department
        {
            get
            {
                return this.departmentField;
            }
            set
            {
                this.departmentField = value;
            }
        }

        public string Detail
        {
            get
            {
                return this.detailField;
            }
            set
            {
                this.detailField = value;
            }
        }

        [XmlArrayItem("OpinionAttachmentInfo", IsNullable=false)]
        public OpinionAttachmentInfo[] OpinionAttachments
        {
            get
            {
                return this.opinionAttachmentsField;
            }
            set
            {
                this.opinionAttachmentsField = value;
            }
        }

        public string Process
        {
            get
            {
                return this.processField;
            }
            set
            {
                this.processField = value;
            }
        }
    }
}

