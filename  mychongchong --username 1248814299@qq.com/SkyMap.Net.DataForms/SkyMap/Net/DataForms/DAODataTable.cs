namespace SkyMap.Net.DataForms
{
    using SkyMap.Net.DAO;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    [Serializable]
    public class DAODataTable : DomainObject, ISaveAs
    {
        private IList<DataControl> bindControls;
        private IList<DAODataForm> bindForms;
        private IList<DAODataColumn> dataColumns;
        private string key = string.Empty;
        private bool level;
        private string relKey = string.Empty;
        private DAODataSet templetDataSet;

        public bool Contains(string column)
        {
            if (this.dataColumns != null)
            {
                column = column.ToLower();
                foreach (DAODataColumn column2 in this.dataColumns)
                {
                    if (column2.Name.ToLower() == column)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void SaveAs(UnitOfWork unitOfWork)
        {
            DAODataTable table = this.Clone<DAODataTable>();
            table.BindForms = new List<DAODataForm>();
            table.BindControls = new List<DataControl>();
            table.DataColumns = new List<DAODataColumn>();
            unitOfWork.RegisterNew(table);
            foreach (DAODataColumn column in this.dataColumns)
            {
                column.SaveAs(unitOfWork);
            }
        }

        [Browsable(false)]
        public IList<DataControl> BindControls
        {
            get
            {
                return this.bindControls;
            }
            set
            {
                this.bindControls = value;
            }
        }

        [Browsable(false)]
        public IList<DAODataForm> BindForms
        {
            get
            {
                return this.bindForms;
            }
            set
            {
                this.bindForms = value;
            }
        }

        [Browsable(false)]
        public IList<DAODataColumn> DataColumns
        {
            get
            {
                return this.dataColumns;
            }
            set
            {
                this.dataColumns = value;
            }
        }

        [DisplayName("主键")]
        public string Key
        {
            get
            {
                return this.key;
            }
            set
            {
                this.key = value;
            }
        }

        [DisplayName("是否子表")]
        public bool Level
        {
            get
            {
                return this.level;
            }
            set
            {
                this.level = value;
            }
        }

        [DisplayName("外键")]
        public string RelKey
        {
            get
            {
                return this.relKey;
            }
            set
            {
                this.relKey = value;
            }
        }

        [Browsable(false)]
        public DAODataSet TempletDataSet
        {
            get
            {
                return this.templetDataSet;
            }
            set
            {
                this.templetDataSet = value;
            }
        }
    }
}

