namespace SkyMap.Net.DataForms
{
    using SkyMap.Net.DAO;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    [Serializable]
    public class DAODataColumn : DomainObject, ISaveAs
    {
        private IList<DataControl> bindControls;
        private DAODataTable dataTable;
        private string dataType;
        private int displayIndex;
        private string initialValue;
        private bool isDisplay;
        private bool isNeedInitial;
        private bool isQuery;
        private int length;

        public void SaveAs(UnitOfWork unitOfWork)
        {
            unitOfWork.RegisterNew(this);
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

        public DAODataTable DataTable
        {
            get
            {
                return this.dataTable;
            }
            set
            {
                this.dataTable = value;
            }
        }

        public string DataType
        {
            get
            {
                return this.dataType;
            }
            set
            {
                this.dataType = value;
            }
        }

        public int DisplayIndex
        {
            get
            {
                return this.displayIndex;
            }
            set
            {
                this.displayIndex = value;
            }
        }

        public string InitialValue
        {
            get
            {
                return this.initialValue;
            }
            set
            {
                this.initialValue = value;
            }
        }

        public bool IsDisplay
        {
            get
            {
                return this.isDisplay;
            }
            set
            {
                this.isDisplay = value;
            }
        }

        public bool IsNeedInitial
        {
            get
            {
                return this.isNeedInitial;
            }
            set
            {
                this.isNeedInitial = value;
            }
        }

        public bool IsQuery
        {
            get
            {
                return this.isQuery;
            }
            set
            {
                this.isQuery = value;
            }
        }

        public int Length
        {
            get
            {
                return this.length;
            }
            set
            {
                this.length = value;
            }
        }
    }
}

