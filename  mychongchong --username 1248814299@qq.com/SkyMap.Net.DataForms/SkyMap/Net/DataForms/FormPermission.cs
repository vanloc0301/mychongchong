namespace SkyMap.Net.DataForms
{
    using SkyMap.Net.DAO;
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;

    [Serializable]
    public abstract class FormPermission : DomainObject, ISaveAs
    {
        private string daoDataFormId = string.Empty;
        private string daoDataFormName = string.Empty;
        private bool enableDelete;
        private string inVisibleFrame = string.Empty;
        private int pageIndex = 0;
        private string printSetId;
        private string printSetName;
        private string unableFrame = string.Empty;

        protected FormPermission()
        {
        }

        public void SaveAs(UnitOfWork unitOfWork)
        {
            unitOfWork.RegisterNew(this);
        }

        public override string ToString()
        {
            return this.daoDataFormName;
        }

        [Browsable(false)]
        public virtual string DaoDataFormId
        {
            get
            {
                return this.daoDataFormId;
            }
            set
            {
                this.daoDataFormId = value;
            }
        }

        [Editor("SkyMap.Net.Tools.DataForms.FormPermissionEditor,SkyMap.Net.Tools.DataForms", typeof(UITypeEditor)), DisplayName("设定表单权限")]
        public virtual string DaoDataFormName
        {
            get
            {
                return this.daoDataFormName;
            }
            set
            {
                this.daoDataFormName = value;
            }
        }

        [Browsable(false), DisplayName("是否具有删除权限")]
        public virtual bool EnableDelete
        {
            get
            {
                return this.enableDelete;
            }
            set
            {
                this.enableDelete = value;
            }
        }

        [Browsable(false)]
        public virtual string InVisibleFrame
        {
            get
            {
                return this.inVisibleFrame;
            }
            set
            {
                this.inVisibleFrame = value;
            }
        }

        [DisplayName("初始化页面")]
        public virtual int PageIndex
        {
            get
            {
                return this.pageIndex;
            }
            set
            {
                this.pageIndex = value;
            }
        }

        [Browsable(false)]
        public virtual string PrintSetId
        {
            get
            {
                return this.printSetId;
            }
            set
            {
                this.printSetId = value;
            }
        }

        [DisplayName("报表权限集合名称"), ReadOnly(true)]
        public virtual string PrintSetName
        {
            get
            {
                return this.printSetName;
            }
            set
            {
                this.printSetName = value;
            }
        }

        [Browsable(false)]
        public virtual string UnableFrame
        {
            get
            {
                return this.unableFrame;
            }
            set
            {
                this.unableFrame = value;
            }
        }
    }
}

