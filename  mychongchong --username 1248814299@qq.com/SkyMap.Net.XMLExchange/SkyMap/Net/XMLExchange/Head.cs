namespace SkyMap.Net.XMLExchange
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, DebuggerStepThrough, DesignerCategory("code"), XmlType(AnonymousType=true), GeneratedCode("xsd", "2.0.50727.42"), XmlRoot(Namespace="http://www.eappxml.gov.cn/eAppXML", IsNullable=false)]
    public class Head
    {
        private SkyMap.Net.XMLExchange.InformationType informationTypeField;
        private SkyMap.Net.XMLExchange.MessageType messageTypeField;
        private Receiver[] receiversField;
        private RouterItem[] routerField;
        private Sender[] sendersField;
        private DateTime timeStampField;
        private string uUIDField;

        public SkyMap.Net.XMLExchange.InformationType InformationType
        {
            get
            {
                return this.informationTypeField;
            }
            set
            {
                this.informationTypeField = value;
            }
        }

        public SkyMap.Net.XMLExchange.MessageType MessageType
        {
            get
            {
                return this.messageTypeField;
            }
            set
            {
                this.messageTypeField = value;
            }
        }

        [XmlArrayItem("Receiver", IsNullable=false)]
        public Receiver[] Receivers
        {
            get
            {
                return this.receiversField;
            }
            set
            {
                this.receiversField = value;
            }
        }

        [XmlArrayItem("RouterItem", IsNullable=false)]
        public RouterItem[] Router
        {
            get
            {
                return this.routerField;
            }
            set
            {
                this.routerField = value;
            }
        }

        [XmlArrayItem("Sender", IsNullable=false)]
        public Sender[] Senders
        {
            get
            {
                return this.sendersField;
            }
            set
            {
                this.sendersField = value;
            }
        }

        public DateTime TimeStamp
        {
            get
            {
                return this.timeStampField;
            }
            set
            {
                this.timeStampField = value;
            }
        }

        public string UUID
        {
            get
            {
                return this.uUIDField;
            }
            set
            {
                this.uUIDField = value;
            }
        }
    }
}

