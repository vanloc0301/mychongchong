namespace SkyMap.Net.XMLExchange
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, XmlType(AnonymousType=true), GeneratedCode("xsd", "2.0.50727.42"), DesignerCategory("code"), XmlRoot(Namespace="http://www.egxml.gov.cn/message", IsNullable=false), DebuggerStepThrough]
    public class eSign
    {
        private bool signFlagField = false;
        private string valueField;
        private bool verifyFlagField = false;

        [XmlAttribute, DefaultValue(false)]
        public bool signFlag
        {
            get
            {
                return this.signFlagField;
            }
            set
            {
                this.signFlagField = value;
            }
        }

        [XmlText]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        [DefaultValue(false), XmlAttribute]
        public bool verifyFlag
        {
            get
            {
                return this.verifyFlagField;
            }
            set
            {
                this.verifyFlagField = value;
            }
        }
    }
}

