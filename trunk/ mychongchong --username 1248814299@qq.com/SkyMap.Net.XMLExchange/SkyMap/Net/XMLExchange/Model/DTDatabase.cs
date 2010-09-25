namespace SkyMap.Net.XMLExchange.Model
{
    using SkyMap.Net.DAO;
    using System;
    using System.Collections;
    using System.ComponentModel;

    [Serializable]
    public class DTDatabase : SMDataSource
    {
        private string afterProcedure;
        private string beforeProcedure;
        private SkyMap.Net.XMLExchange.Model.DTProject dTProject;
        private IList dTTables;
        private bool ifExport;
        private bool ifImport;

        public DTDatabase()
        {
            this.ifExport = true;
            this.ifImport = true;
        }

        public DTDatabase(SkyMap.Net.XMLExchange.Model.DTProject dtProject, string name)
        {
            this.ifExport = true;
            this.ifImport = true;
            this.dTProject = dtProject;
            this.Name = name;
            this.dTProject.DTDatabases.Add(this);
        }

        public DTDatabase(SkyMap.Net.XMLExchange.Model.DTProject dtProject, string name, string description, string type, string connectionString, string beforeProcedure, string afterProcedure) : this(dtProject, name)
        {
            this.Description = description;
            base.DSType = type;
            base.ConnectionString = connectionString;
            this.beforeProcedure = beforeProcedure;
            this.afterProcedure = afterProcedure;
        }

        public DTDatabase Clone(SkyMap.Net.XMLExchange.Model.DTProject project)
        {
            DTDatabase database = this.Clone<DTDatabase>();
            database.DTProject = project;
            database.DTTables = new ArrayList();
            project.DTDatabases.Add(project);
            return database;
        }

        [DisplayName("导入后执行")]
        public string AfterProcedure
        {
            get
            {
                return this.afterProcedure;
            }
            set
            {
                this.afterProcedure = value;
            }
        }

        [DisplayName("导入前执行")]
        public string BeforeProcedure
        {
            get
            {
                return this.beforeProcedure;
            }
            set
            {
                this.beforeProcedure = value;
            }
        }

        [DisplayName("项目名称")]
        public SkyMap.Net.XMLExchange.Model.DTProject DTProject
        {
            get
            {
                return this.dTProject;
            }
            set
            {
                this.dTProject = value;
            }
        }

        [Browsable(false)]
        public IList DTTables
        {
            get
            {
                if (this.dTTables != null)
                {
                    return this.dTTables;
                }
                return new ArrayList();
            }
            set
            {
                this.dTTables = value;
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
    }
}

