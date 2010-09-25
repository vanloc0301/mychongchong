namespace SkyMap.Net.XMLExchange
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, DebuggerStepThrough, GeneratedCode("xsd", "2.0.50727.42"), DesignerCategory("code"), XmlType(AnonymousType=true), XmlRoot(Namespace="http://www.eappxml.gov.cn/eAppXML", IsNullable=false)]
    public class Conditon
    {
        private string conditionNameField;
        private string conditionnValueField;

        [XmlAttribute]
        public string ConditionName
        {
            get
            {
                return this.conditionNameField;
            }
            set
            {
                this.conditionNameField = value;
            }
        }

        [XmlAttribute]
        public string ConditionnValue
        {
            get
            {
                return this.conditionnValueField;
            }
            set
            {
                this.conditionnValueField = value;
            }
        }
    }
}

