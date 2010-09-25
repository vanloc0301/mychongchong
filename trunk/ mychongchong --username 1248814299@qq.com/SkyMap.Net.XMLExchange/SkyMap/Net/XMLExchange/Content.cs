namespace SkyMap.Net.XMLExchange
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, DesignerCategory("code"), DebuggerStepThrough, GeneratedCode("xsd", "2.0.50727.42"), XmlRoot(Namespace="http://www.eappxml.gov.cn/eAppXML", IsNullable=false), XmlType(AnonymousType=true)]
    public class Content
    {
        private object itemField;

        [XmlElement("Requirment", typeof(Requirment)), XmlElement("Query", typeof(Query)), XmlElement("Project", typeof(Project))]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }
}

