namespace SkyMap.Net.XMLExchange
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, DesignerCategory("code"), XmlRoot(Namespace="http://www.eappxml.gov.cn/eAppXML", IsNullable=false), GeneratedCode("xsd", "2.0.50727.42"), DebuggerStepThrough, XmlType(AnonymousType=true)]
    public class Body
    {
        private string associateApplyingIDField;
        private File[] attachmentField;
        private SkyMap.Net.XMLExchange.CaseIDInfo caseIDInfoField;
        private SkyMap.Net.XMLExchange.Content contentField;

        public string AssociateApplyingID
        {
            get
            {
                return this.associateApplyingIDField;
            }
            set
            {
                this.associateApplyingIDField = value;
            }
        }

        [XmlArrayItem("File", IsNullable=false)]
        public File[] Attachment
        {
            get
            {
                return this.attachmentField;
            }
            set
            {
                this.attachmentField = value;
            }
        }

        public SkyMap.Net.XMLExchange.CaseIDInfo CaseIDInfo
        {
            get
            {
                return this.caseIDInfoField;
            }
            set
            {
                this.caseIDInfoField = value;
            }
        }

        public SkyMap.Net.XMLExchange.Content Content
        {
            get
            {
                return this.contentField;
            }
            set
            {
                this.contentField = value;
            }
        }
    }
}

