namespace SkyMap.Net.XMLExchange
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, XmlType(AnonymousType=true), XmlRoot(Namespace="http://www.eappxml.gov.cn/eAppXML", IsNullable=false), DesignerCategory("code"), DebuggerStepThrough, GeneratedCode("xsd", "2.0.50727.42")]
    public class ProjectSummary
    {
        private string acceptDepartmentField;
        private DateTime applyingDateField;
        private string projectNameField;
        private string proposorNameField;

        public string AcceptDepartment
        {
            get
            {
                return this.acceptDepartmentField;
            }
            set
            {
                this.acceptDepartmentField = value;
            }
        }

        public DateTime ApplyingDate
        {
            get
            {
                return this.applyingDateField;
            }
            set
            {
                this.applyingDateField = value;
            }
        }

        public string ProjectName
        {
            get
            {
                return this.projectNameField;
            }
            set
            {
                this.projectNameField = value;
            }
        }

        public string ProposorName
        {
            get
            {
                return this.proposorNameField;
            }
            set
            {
                this.proposorNameField = value;
            }
        }
    }
}

