namespace SkyMap.Net.XMLExchange
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, DebuggerStepThrough, GeneratedCode("xsd", "2.0.50727.42"), DesignerCategory("code"), XmlType(AnonymousType=true), XmlRoot(Namespace="http://www.eappxml.gov.cn/eAppXML", IsNullable=false)]
    public class ApplyingAttachmentInfo
    {
        private string fileNameField;
        private string idField;

        [XmlAttribute]
        public string FileName
        {
            get
            {
                return this.fileNameField;
            }
            set
            {
                this.fileNameField = value;
            }
        }

        [XmlAttribute]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }
}

