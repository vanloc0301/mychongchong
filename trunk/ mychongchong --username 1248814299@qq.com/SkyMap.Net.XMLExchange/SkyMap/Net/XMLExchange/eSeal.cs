namespace SkyMap.Net.XMLExchange
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, DesignerCategory("code"), XmlType(AnonymousType=true), XmlRoot(Namespace="http://www.egxml.gov.cn/message", IsNullable=false), GeneratedCode("xsd", "2.0.50727.42"), DebuggerStepThrough]
    public class eSeal
    {
        private string gongwenshengxiaobiaoshiField;
        private string modeField;
        private string shujuField;

        public string Gongwenshengxiaobiaoshi
        {
            get
            {
                return this.gongwenshengxiaobiaoshiField;
            }
            set
            {
                this.gongwenshengxiaobiaoshiField = value;
            }
        }

        public string Mode
        {
            get
            {
                return this.modeField;
            }
            set
            {
                this.modeField = value;
            }
        }

        public string Shuju
        {
            get
            {
                return this.shujuField;
            }
            set
            {
                this.shujuField = value;
            }
        }
    }
}

