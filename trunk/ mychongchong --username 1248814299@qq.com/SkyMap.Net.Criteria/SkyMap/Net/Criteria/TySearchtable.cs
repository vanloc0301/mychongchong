namespace SkyMap.Net.Criteria
{
    using SkyMap.Net.DAO;
    using System;
    using System.ComponentModel;

    [Serializable]
    public class TySearchtable : DomainObject
    {
        private string m_relation_table_key = string.Empty;
        private string m_relation_table_name = string.Empty;
        private string m_table_key = string.Empty;
        private string m_table_name;
        private int m_table_order = 0;
        private SkyMap.Net.Criteria.TySearchx m_ty_searchx;

        public override string ToString()
        {
            return this.m_table_name;
        }

        private string Name
        {
            get
            {
                return string.Empty;
            }
            set
            {
            }
        }

        [DisplayName("外键")]
        public string RelationTableKey
        {
            get
            {
                return this.m_relation_table_key;
            }
            set
            {
                if ((value != null) && (value.Length > 50))
                {
                    throw new ArgumentOutOfRangeException("Invalid value for RelationTableKey", value, value.ToString());
                }
                this.m_relation_table_key = value;
            }
        }

        [DisplayName("关联表名")]
        public string RelationTableName
        {
            get
            {
                return this.m_relation_table_name;
            }
            set
            {
                if ((value != null) && (value.Length > 50))
                {
                    throw new ArgumentOutOfRangeException("Invalid value for RelationTableName", value, value.ToString());
                }
                this.m_relation_table_name = value;
            }
        }

        [DisplayName("主键")]
        public string TableKey
        {
            get
            {
                return this.m_table_key;
            }
            set
            {
                if ((value != null) && (value.Length > 50))
                {
                    throw new ArgumentOutOfRangeException("Invalid value for TableKey", value, value.ToString());
                }
                this.m_table_key = value;
            }
        }

        [DisplayName("表名")]
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

        [DisplayName("顺序")]
        public int TableOrder
        {
            get
            {
                return this.m_table_order;
            }
            set
            {
                this.m_table_order = value;
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

