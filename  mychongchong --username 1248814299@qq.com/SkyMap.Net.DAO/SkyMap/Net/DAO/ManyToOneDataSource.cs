namespace SkyMap.Net.DAO
{
    using SkyMap.Net.Components;
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;

    [Serializable]
    public abstract class ManyToOneDataSource : DomainObject
    {
        private string smDataSourceName = "系统主数据库";
        private string smDataSourceOID = "SYSTEM_MAIN";

        protected ManyToOneDataSource()
        {
        }

        [Editor("SkyMap.Net.Gui.Components.DropDownEditor,SkyMap.Net.Windows", typeof(UITypeEditor)), DomainObjectDropDown(type=typeof(SMDataSource), CacheKey="ALL_SMDataSource"), DisplayName("数据源")]
        public SMDataSource DataSource
        {
            get
            {
                if (string.IsNullOrEmpty(this.smDataSourceOID))
                {
                    this.smDataSourceOID = "SYSTEM_MAIN";
                }
                return QueryHelper.Get<SMDataSource>("SMDataSource_" + this.smDataSourceOID, this.smDataSourceOID);
            }
            set
            {
                if (value != null)
                {
                    this.smDataSourceOID = value.Id;
                    this.smDataSourceName = value.Name;
                }
                else
                {
                    this.smDataSourceOID = "SYSTEM_MAIN";
                    this.smDataSourceName = "系统主数据库";
                }
            }
        }
    }
}

