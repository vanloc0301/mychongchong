namespace SkyMap.Net.Tools.DataForms
{
    using SkyMap.Net.Commands;
    using SkyMap.Net.DAO;
    using SkyMap.Net.DataForms;
    using System;
    using System.Windows.Forms;

    public class DataSetViewContent : AbstractEditViewContent
    {
        private DataSetSetting dss = new DataSetSetting();

        public DataSetViewContent()
        {
            this.TitleName = "数据列定义";
        }

        public override void Dispose()
        {
            this.dss.Dispose();
        }

        public override void Load(string fileName)
        {
        }

        public override void Load(DomainObject domainObject, UnitOfWork unitOfWork)
        {
            this.dss.LoadMe((DAODataTable) domainObject, unitOfWork);
        }

        public override void RedrawContent()
        {
        }

        public override void Save()
        {
        }

        public override void Save(string fileName)
        {
        }

        public override System.Windows.Forms.Control Control
        {
            get
            {
                return this.dss;
            }
        }

        public override bool IsDirty
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                return true;
            }
        }
    }
}

