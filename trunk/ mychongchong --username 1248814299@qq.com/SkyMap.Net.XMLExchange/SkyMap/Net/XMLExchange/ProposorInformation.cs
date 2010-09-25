namespace SkyMap.Net.XMLExchange
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    [Serializable, XmlType(AnonymousType=true), XmlRoot(Namespace="http://www.eappxml.gov.cn/eAppXML", IsNullable=false), DebuggerStepThrough, DesignerCategory("code"), GeneratedCode("xsd", "2.0.50727.42")]
    public class ProposorInformation
    {
        private string addressField;
        private string emailField;
        private string mobileNoField;
        private string nameField;
        private string phoneNoField;
        private string postCodeField;

        public string Address
        {
            get
            {
                return this.addressField;
            }
            set
            {
                this.addressField = value;
            }
        }

        public string Email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }

        public string MobileNo
        {
            get
            {
                return this.mobileNoField;
            }
            set
            {
                this.mobileNoField = value;
            }
        }

        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        public string PhoneNo
        {
            get
            {
                return this.phoneNoField;
            }
            set
            {
                this.phoneNoField = value;
            }
        }

        public string PostCode
        {
            get
            {
                return this.postCodeField;
            }
            set
            {
                this.postCodeField = value;
            }
        }
    }
}

