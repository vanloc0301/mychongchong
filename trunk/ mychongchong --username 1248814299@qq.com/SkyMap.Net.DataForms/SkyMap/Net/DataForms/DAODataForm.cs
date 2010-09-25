namespace SkyMap.Net.DataForms
{
    using SkyMap.Net.Components;
    using SkyMap.Net.DAO;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing.Design;

    [Serializable]
    public class DAODataForm : ManyToOneDataSource, ISaveAs
    {
        private string assemblyName;
        private IList<DAODataTable> bindTables;
        private string cascadeSql;
        private IList<DataControl> dataControls;
        private IList<SMDataSource> saveAsTos = new List<SMDataSource>();

        public void SaveAs(UnitOfWork unitOfWork)
        {
            DAODataForm form = this.Clone<DAODataForm>();
            form.BindTables = new List<DAODataTable>();
            List<string> list = new List<string>();
            foreach (DAODataTable table in this.BindTables)
            {
                if (!list.Contains(table.TempletDataSet.Id))
                {
                    table.TempletDataSet.SaveAs(unitOfWork);
                    list.Add(table.TempletDataSet.Id);
                }
                form.BindTables.Add(table);
            }
            unitOfWork.RegisterNew(form);
            foreach (DataControl control in this.dataControls)
            {
                control.SaveAs(unitOfWork);
            }
        }

        [DisplayName("程序集名")]
        public string AssemblyName
        {
            get
            {
                return this.assemblyName;
            }
            set
            {
                this.assemblyName = value;
            }
        }

        [Browsable(false)]
        public IList<DAODataTable> BindTables
        {
            get
            {
                return this.bindTables;
            }
            set
            {
                this.bindTables = value;
            }
        }

        [DisplayName("级联SQL"), Description("保存数据后常需要使用触发器之类更新业务相关信息，使用这个SQL，你就可以达到目的保存数据库自动调用SQL的功能，避免使用触发器"), Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public string CascadeSql
        {
            get
            {
                return this.cascadeSql;
            }
            set
            {
                this.cascadeSql = value;
            }
        }

        [Browsable(false)]
        public IList<DataControl> DataControls
        {
            get
            {
                return this.dataControls;
            }
            set
            {
                this.dataControls = value;
            }
        }

        [DisplayName("另存至数据源..."), Description("请保证另存的数据库中包含相同名称且结构相同的数据表"), DomainObjectList(CacheKey="ALL_SMDataSource_DAO", type=typeof(SMDataSource), OrderBys=new string[] { "DisplayOrder" }), Editor("SkyMap.Net.Gui.Components.ListEditor,SkyMap.Net.Windows", typeof(UITypeEditor))]
        public IList<SMDataSource> SaveAsTos
        {
            get
            {
                return this.saveAsTos;
            }
            set
            {
                this.saveAsTos = value;
            }
        }
    }
}

