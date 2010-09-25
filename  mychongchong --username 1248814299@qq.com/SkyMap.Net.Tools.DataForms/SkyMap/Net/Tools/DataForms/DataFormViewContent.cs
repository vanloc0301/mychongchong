namespace SkyMap.Net.Tools.DataForms
{
    using SkyMap.Net.Commands;
    using SkyMap.Net.DAO;
    using System;
    using System.Windows.Forms;

    public class DataFormViewContent : AbstractEditViewContent
    {
        private DataFormSetting dfs = new DataFormSetting();

        public DataFormViewContent()
        {
            this.TitleName = "表单数据绑定定义";
        }

        public override void Dispose()
        {
            this.dfs.Dispose();
        }

        public override void Load(string fileName)
        {
        }

        public override void Load(DomainObject domainObject, UnitOfWork unitOfWork)
        {
            this.dfs.LoadMe(domainObject, unitOfWork);
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
                return this.dfs;
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

