namespace SkyMap.Net.XMLExchange.Model
{
    using SkyMap.Net.DAO;
    using System;
    using System.ComponentModel;

    [Serializable]
    public class DTColumn : DomainObject
    {
        private string defaultValue;
        private SkyMap.Net.XMLExchange.Model.DTTable dTTable;
        private bool ifExport;
        private bool ifImport;
        private string mapXMLElementName;
        private string namespacePrefix;
        private string namespaceUri;
        private string type;

        public DTColumn()
        {
            this.ifExport = true;
            this.ifImport = true;
        }

        public DTColumn(SkyMap.Net.XMLExchange.Model.DTTable dtTable, string name)
        {
            this.ifExport = true;
            this.ifImport = true;
            this.DTTable = dtTable;
            dtTable.DTColumns.Add(this);
            this.Name = name;
        }

        public DTColumn(string namespacePrefix, string namespaceUri, SkyMap.Net.XMLExchange.Model.DTTable dtTable, string name, string description, int displayOrder, string defaultValue, string mapXMLElementName, string type) : this(dtTable, name)
        {
            this.DisplayOrder = displayOrder;
            this.Description = description;
            this.defaultValue = defaultValue;
            this.mapXMLElementName = mapXMLElementName;
            this.type = type;
            this.namespacePrefix = namespacePrefix;
            this.namespaceUri = namespaceUri;
        }

        public DTColumn Clone(SkyMap.Net.XMLExchange.Model.DTTable dtTable)
        {
            DTColumn column = this.Clone<DTColumn>();
            column.DTTable = dtTable;
            dtTable.DTColumns.Add(column);
            return column;
        }

        [DisplayName("缺省值")]
        public string DefaultValue
        {
            get
            {
                return this.defaultValue;
            }
            set
            {
                this.defaultValue = value;
            }
        }

        [DisplayName("表")]
        public SkyMap.Net.XMLExchange.Model.DTTable DTTable
        {
            get
            {
                return this.dTTable;
            }
            set
            {
                this.dTTable = value;
            }
        }

        [DisplayName("是否导出")]
        public bool IfExport
        {
            get
            {
                return this.ifExport;
            }
            set
            {
                this.ifExport = value;
            }
        }

        [DisplayName("是否导入")]
        public bool IfImport
        {
            get
            {
                return this.ifImport;
            }
            set
            {
                this.ifImport = value;
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

        [DisplayName("命名空间前缀")]
        public string NamespacePrefix
        {
            get
            {
                return this.namespacePrefix;
            }
            set
            {
                this.namespacePrefix = value;
            }
        }

        [DisplayName("命名空间")]
        public string NamespaceUri
        {
            get
            {
                return this.namespaceUri;
            }
            set
            {
                this.namespaceUri = value;
            }
        }

        [DisplayName("类型")]
        public string Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }
    }
}

