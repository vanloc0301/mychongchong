namespace SkyMap.Net.XMLExchange.Model
{
    using SkyMap.Net.DAO;
    using System;
    using System.Collections;
    using System.ComponentModel;

    [Serializable]
    public class DTTable : DomainObject
    {
        private string dataSetName;
        private IList dTColumns;
        private SkyMap.Net.XMLExchange.Model.DTDatabase dTDatabase;
        private int hiberarchy;
        private bool ifExport;
        private bool ifImport;
        private string importToTable;
        private string mapXMLElementName;
        private string namespacePrefix;
        private string namespaceUri;
        private SkyMap.Net.XMLExchange.Model.SourceType sourceType;

        public DTTable()
        {
            this.ifExport = true;
            this.ifImport = true;
        }

        public DTTable(SkyMap.Net.XMLExchange.Model.DTDatabase dtDatabase, string name)
        {
            this.ifExport = true;
            this.ifImport = true;
            this.dTDatabase = dtDatabase;
            this.Name = name;
            this.dTDatabase.DTTables.Add(this);
        }

        public DTTable(string namespacePrefix, string namespaceUri, SkyMap.Net.XMLExchange.Model.DTDatabase dtDatabase, string name, string description, string mapXMLElementName, int hiberarchy, SkyMap.Net.XMLExchange.Model.SourceType sourceType) : this(dtDatabase, name)
        {
            this.Description = description;
            this.mapXMLElementName = mapXMLElementName;
            this.hiberarchy = hiberarchy;
            this.sourceType = sourceType;
            this.namespacePrefix = namespacePrefix;
            this.namespaceUri = namespaceUri;
        }

        public DTTable Clone(SkyMap.Net.XMLExchange.Model.DTDatabase dtDatabase)
        {
            DTTable table = this.Clone<DTTable>();
            table.DTDatabase = dtDatabase;
            dtDatabase.DTTables.Add(table);
            table.DTColumns = new ArrayList();
            return table;
        }

        [DisplayName("数据集名称")]
        public string DataSetName
        {
            get
            {
                return this.dataSetName;
            }
            set
            {
                this.dataSetName = value;
            }
        }

        [Browsable(false)]
        public IList DTColumns
        {
            get
            {
                if (this.dTColumns != null)
                {
                    return this.dTColumns;
                }
                return new ArrayList();
            }
            set
            {
                this.dTColumns = value;
            }
        }

        [DisplayName("数据库")]
        public SkyMap.Net.XMLExchange.Model.DTDatabase DTDatabase
        {
            get
            {
                return this.dTDatabase;
            }
            set
            {
                this.dTDatabase = value;
            }
        }

        [DisplayName("层级")]
        public int Hiberarchy
        {
            get
            {
                return this.hiberarchy;
            }
            set
            {
                this.hiberarchy = value;
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

        [DisplayName("导入至表(通常与名称相同,使用视图导出时可能不同)")]
        public string ImportToTable
        {
            get
            {
                return this.importToTable;
            }
            set
            {
                this.importToTable = value;
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

        [DisplayName("来源")]
        public SkyMap.Net.XMLExchange.Model.SourceType SourceType
        {
            get
            {
                return this.sourceType;
            }
            set
            {
                this.sourceType = value;
            }
        }
    }
}

