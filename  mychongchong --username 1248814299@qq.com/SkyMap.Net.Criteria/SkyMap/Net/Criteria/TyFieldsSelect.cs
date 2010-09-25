namespace SkyMap.Net.Criteria
{
    using SkyMap.Net.Components;
    using SkyMap.Net.DAO;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing.Design;

    [Serializable]
    public class TyFieldsSelect : DomainObject
    {
        private string m_condition_name = string.Empty;
        private int m_display_index;
        private string m_display_name = string.Empty;
        private string m_is_condtion = string.Empty;
        private string m_is_display = string.Empty;
        private string m_table_name = string.Empty;
        private IList<TyFilterCondition> m_TR_FILTER_CONDITIONs = new List<TyFilterCondition>();
        private SkyMap.Net.Criteria.TySearchx m_ty_searchx = new SkyMap.Net.Criteria.TySearchx();
        private string m_type = string.Empty;

        [DisplayName("条件名称")]
        public string ConditionName
        {
            get
            {
                return this.m_condition_name;
            }
            set
            {
                if ((value != null) && (value.Length > 200))
                {
                    throw new ArgumentOutOfRangeException("Invalid value for ConditionName", value, value.ToString());
                }
                this.m_condition_name = value;
            }
        }

        [DisplayName("查询结果显示顺序")]
        public int DisplayIndex
        {
            get
            {
                return this.m_display_index;
            }
            set
            {
                this.m_display_index = value;
            }
        }

        [DisplayName("查询结果显示名称")]
        public string DisplayName
        {
            get
            {
                return this.m_display_name;
            }
            set
            {
                if ((value != null) && (value.Length > 50))
                {
                    throw new ArgumentOutOfRangeException("Invalid value for DisplayName", value, value.ToString());
                }
                this.m_display_name = value;
            }
        }

        [DisplayName("是否条件"), StringsDropDown(dataSource=new string[] { "是", "否" }), Editor("SkyMap.Net.Gui.Components.DropDownEditor,SkyMap.Net.Windows", typeof(UITypeEditor))]
        public string IsCondtion
        {
            get
            {
                return this.m_is_condtion;
            }
            set
            {
                if ((value != null) && (value.Length > 50))
                {
                    throw new ArgumentOutOfRangeException("Invalid value for IsCondtion", value, value.ToString());
                }
                this.m_is_condtion = value;
            }
        }

        [StringsDropDown(dataSource=new string[] { "是", "否" }), Editor("SkyMap.Net.Gui.Components.DropDownEditor,SkyMap.Net.Windows", typeof(UITypeEditor)), DisplayName("是否显示")]
        public string IsDisplay
        {
            get
            {
                return this.m_is_display;
            }
            set
            {
                if ((value != null) && (value.Length > 50))
                {
                    throw new ArgumentOutOfRangeException("Invalid value for IsDisplay", value, value.ToString());
                }
                this.m_is_display = value;
            }
        }

        [DisplayName("所属表名")]
        public string TableName
        {
            get
            {
                return this.m_table_name;
            }
            set
            {
                if ((value != null) && (value.Length > 50))
                {
                    throw new ArgumentOutOfRangeException("Invalid value for TableName", value, value.ToString());
                }
                this.m_table_name = value;
            }
        }

        [Browsable(false)]
        public IList<TyFilterCondition> TyFilterConditions
        {
            get
            {
                return this.m_TR_FILTER_CONDITIONs;
            }
            set
            {
                this.m_TR_FILTER_CONDITIONs = value;
            }
        }

        [DisplayName("类型")]
        public string Type
        {
            get
            {
                return this.m_type;
            }
            set
            {
                if ((value != null) && (value.Length > 50))
                {
                    throw new ArgumentOutOfRangeException("Invalid value for Type", value, value.ToString());
                }
                this.m_type = value;
            }
        }

        [Browsable(false)]
        public SkyMap.Net.Criteria.TySearchx TySearchx
        {
            get
            {
                return this.m_ty_searchx;
            }
            set
            {
                this.m_ty_searchx = value;
            }
        }
    }
}

