namespace SkyMap.Net.Workflow.XPDL
{
    using SkyMap.Net.Components;
    using SkyMap.Net.DAO;
    using SkyMap.Net.DataForms;
    using SkyMap.Net.Workflow.XPDL.ExtendElement;
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;

    [Serializable]
    public abstract class AbstractCommonProdef : AbstractWfElement
    {
        private string daoDataFormId = string.Empty;
        private string daoDataFormName = string.Empty;
        private string daoDataSetId = string.Empty;
        private string daoDataSetName = string.Empty;
        private string logo = string.Empty;
        private string style = string.Empty;
        private string templetAppendixsId = string.Empty;
        private string templetAppendixsName = string.Empty;
        private string type = "FLW";

        protected AbstractCommonProdef()
        {
        }

        [DomainObjectDropDown(type=typeof(SkyMap.Net.DataForms.DAODataForm), CacheKey="ALL_DAODataForm_DAO"), DisplayName("表单定义"), Editor("SkyMap.Net.Gui.Components.DropDownEditor,SkyMap.Net.Windows", typeof(UITypeEditor))]
        public SkyMap.Net.DataForms.DAODataForm DAODataForm
        {
            get
            {
                if (!string.IsNullOrEmpty(this.daoDataFormId))
                {
                    return QueryHelper.Get<SkyMap.Net.DataForms.DAODataForm>("DAODataForm_" + this.daoDataFormId, this.daoDataFormId);
                }
                return null;
            }
            set
            {
                this.daoDataFormId = value.Id;
                this.daoDataFormName = value.Name;
            }
        }

        [Browsable(false)]
        public string DaoDataFormId
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

        [Browsable(false)]
        public string DaoDataFormName
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

        [DomainObjectDropDown(type=typeof(SkyMap.Net.DataForms.DAODataSet), CacheKey="ALL_DAODataSet_DAO"), DisplayName("数据集定义"), Editor("SkyMap.Net.Gui.Components.DropDownEditor,SkyMap.Net.Windows", typeof(UITypeEditor))]
        public SkyMap.Net.DataForms.DAODataSet DAODataSet
        {
            get
            {
                if (!string.IsNullOrEmpty(this.daoDataSetId))
                {
                    return QueryHelper.Get<SkyMap.Net.DataForms.DAODataSet>("DAODataSet_" + this.daoDataSetId, this.daoDataSetId);
                }
                return null;
            }
            set
            {
                this.daoDataSetId = value.Id;
                this.daoDataSetName = value.Name;
            }
        }

        [Browsable(false)]
        public string DaoDataSetId
        {
            get
            {
                return this.daoDataSetId;
            }
            set
            {
                this.daoDataSetId = value;
            }
        }

        [Browsable(false)]
        public string DaoDataSetName
        {
            get
            {
                return this.daoDataSetName;
            }
            set
            {
                this.daoDataSetName = value;
            }
        }

        [DisplayName("标识字符串")]
        public virtual string Logo
        {
            get
            {
                return this.logo;
            }
            set
            {
                this.logo = value;
            }
        }

        [Editor("SkyMap.Net.Gui.Components.ListStringEditor,SkyMap.Net.Windows", typeof(UITypeEditor)), DisplayName("表单内容"), StringsDropDown(dataSource=new string[] { "业务数据", "经办人意见", "收件资料", "业务附件", "流程信息" })]
        public string Style
        {
            get
            {
                return this.style;
            }
            set
            {
                this.style = value;
            }
        }

        [Browsable(false)]
        public string TempletAppendixsId
        {
            get
            {
                return this.templetAppendixsId;
            }
            set
            {
                this.templetAppendixsId = value;
            }
        }

        [Browsable(false)]
        public string TempletAppendixsName
        {
            get
            {
                return this.templetAppendixsName;
            }
            set
            {
                this.templetAppendixsName = value;
            }
        }

        [Editor("SkyMap.Net.Gui.Components.DropDownEditor,SkyMap.Net.Windows", typeof(UITypeEditor)), DisplayName("类型"), StringsDropDown(dataSource=new string[] { "FLW", "REG", "STA" })]
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

        [DisplayName("收件资料"), DomainObjectDropDown(type=typeof(SkyMap.Net.Workflow.XPDL.ExtendElement.WfTempletAppendixs), CacheKey="ALL_WfTempletAppendixs_DAO"), Editor("SkyMap.Net.Gui.Components.DropDownEditor,SkyMap.Net.Windows", typeof(UITypeEditor))]
        public SkyMap.Net.Workflow.XPDL.ExtendElement.WfTempletAppendixs WfTempletAppendixs
        {
            get
            {
                if (!string.IsNullOrEmpty(this.templetAppendixsId))
                {
                    return QueryHelper.Get<SkyMap.Net.Workflow.XPDL.ExtendElement.WfTempletAppendixs>("WfTempletAppendixs_" + this.templetAppendixsId, this.templetAppendixsId);
                }
                return null;
            }
            set
            {
                this.templetAppendixsId = value.Id;
                this.templetAppendixsName = value.Name;
            }
        }
    }
}

