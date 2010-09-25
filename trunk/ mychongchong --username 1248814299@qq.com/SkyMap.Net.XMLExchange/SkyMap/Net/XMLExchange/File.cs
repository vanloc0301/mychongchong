namespace SkyMap.Net.XMLExchange
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, XmlType(AnonymousType=true), DesignerCategory("code"), GeneratedCode("xsd", "2.0.50727.42"), DebuggerStepThrough, XmlRoot(Namespace="http://www.eappxml.gov.cn/eAppXML", IsNullable=false)]
    public class File
    {
        private string attachmentIDField;
        private byte[] dataField;
        private string descriptionField;
        private string encodeField;
        private string fileListField;
        private string filenameField;
        private string fileTypeField;

        [XmlAttribute]
        public string AttachmentID
        {
            get
            {
                return this.attachmentIDField;
            }
            set
            {
                this.attachmentIDField = value;
            }
        }

        [XmlElement(DataType="base64Binary")]
        public byte[] Data
        {
            get
            {
                return this.dataField;
            }
            set
            {
                this.dataField = value;
            }
        }

        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        [XmlAttribute]
        public string Encode
        {
            get
            {
                return this.encodeField;
            }
            set
            {
                this.encodeField = value;
            }
        }

        [XmlAttribute]
        public string FileList
        {
            get
            {
                return this.fileListField;
            }
            set
            {
                this.fileListField = value;
            }
        }

        [XmlAttribute]
        public string Filename
        {
            get
            {
                return this.filenameField;
            }
            set
            {
                this.filenameField = value;
            }
        }

        [XmlAttribute]
        public string FileType
        {
            get
            {
                return this.fileTypeField;
            }
            set
            {
                this.fileTypeField = value;
            }
        }
    }
}

