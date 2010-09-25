namespace SkyMap.Net.XMLExchange.Model
{
    using SkyMap.Net.DAO;
    using System;
    using System.Collections;
    using System.ComponentModel;

    [Serializable]
    public class DTProject : DomainObject
    {
        private IList dTDatabases;
        private string mapXMLElementName;
        private string prodefID;
        private string templet;

        public DTProject()
        {
        }

        public DTProject(string name)
        {
            this.Name = name;
        }

        public DTProject(string name, string description, string mapXMLElementName) : this(name)
        {
            this.Description = description;
            this.mapXMLElementName = mapXMLElementName;
        }

        [Browsable(false)]
        public IList DTDatabases
        {
            get
            {
                if (this.dTDatabases != null)
                {
                    return this.dTDatabases;
                }
                return new ArrayList();
            }
            set
            {
                this.dTDatabases = value;
            }
        }

        [DisplayName("映射的XMLElement名称")]
        public string MapXMLElementName
        {
            get
            {
                return this.mapXMLElementName;
            }
            set
            {
                this.mapXMLElementName = value;
            }
        }

        [DisplayName("业务标识ID")]
        public string ProdefID
        {
            get
            {
                return this.prodefID;
            }
            set
            {
                this.prodefID = value;
            }
        }

        [DisplayName("XML交换模板")]
        public string Templet
        {
            get
            {
                return this.templet;
            }
            set
            {
                this.templet = value;
            }
        }
    }
}

