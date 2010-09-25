namespace SkyMap.Net.DataForms
{
    using SkyMap.Net.DAO;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    [Serializable]
    public class DAODataSet : ManyToOneDataSource, ISaveAs
    {
        private IList<DAODataTable> DataTables;

        public void SaveAs(UnitOfWork unitOfWork)
        {
            unitOfWork.RegisterNew(this);
            foreach (DAODataTable table in this.DAODataTables)
            {
                table.SaveAs(unitOfWork);
            }
        }

        [Browsable(false)]
        public IList<DAODataTable> DAODataTables
        {
            get
            {
                return this.DataTables;
            }
            set
            {
                this.DataTables = value;
            }
        }
    }
}

