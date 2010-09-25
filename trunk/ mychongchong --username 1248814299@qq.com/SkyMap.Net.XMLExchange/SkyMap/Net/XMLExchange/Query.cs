namespace SkyMap.Net.XMLExchange
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, XmlType(AnonymousType=true), XmlRoot(Namespace="http://www.eappxml.gov.cn/eAppXML", IsNullable=false), GeneratedCode("xsd", "2.0.50727.42"), DebuggerStepThrough, DesignerCategory("code")]
    public class Query
    {
        private SkyMap.Net.XMLExchange.Conditon[] conditonField;
        private SkyMap.Net.XMLExchange.QueryResult[] queryResultField;
        private QueryQueryType queryTypeField;

        [XmlElement("Conditon")]
        public SkyMap.Net.XMLExchange.Conditon[] Conditon
        {
            get
            {
                return this.conditonField;
            }
            set
            {
                this.conditonField = value;
            }
        }

        [XmlElement("QueryResult")]
        public SkyMap.Net.XMLExchange.QueryResult[] QueryResult
        {
            get
            {
                return this.queryResultField;
            }
            set
            {
                this.queryResultField = value;
            }
        }

        [XmlAttribute]
        public QueryQueryType QueryType
        {
            get
            {
                return this.queryTypeField;
            }
            set
            {
                this.queryTypeField = value;
            }
        }
    }
}

