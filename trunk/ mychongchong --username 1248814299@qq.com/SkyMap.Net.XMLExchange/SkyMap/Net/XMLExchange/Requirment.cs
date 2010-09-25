namespace SkyMap.Net.XMLExchange
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, DesignerCategory("code"), GeneratedCode("xsd", "2.0.50727.42"), DebuggerStepThrough, XmlRoot(Namespace="http://www.eappxml.gov.cn/eAppXML", IsNullable=false), XmlType(AnonymousType=true)]
    public class Requirment
    {
        private SkyMap.Net.XMLExchange.ProjectSummary projectSummaryField;
        private string reqDescField;
        private RequirmentRequirementType requirementTypeField;

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

        public string ReqDesc
        {
            get
            {
                return this.reqDescField;
            }
            set
            {
                this.reqDescField = value;
            }
        }

        [XmlAttribute]
        public RequirmentRequirementType RequirementType
        {
            get
            {
                return this.requirementTypeField;
            }
            set
            {
                this.requirementTypeField = value;
            }
        }
    }
}

