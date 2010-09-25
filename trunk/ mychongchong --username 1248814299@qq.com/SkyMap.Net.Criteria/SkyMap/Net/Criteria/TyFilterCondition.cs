namespace SkyMap.Net.Criteria
{
    using SkyMap.Net.DAO;
    using System;

    [Serializable]
    public class TyFilterCondition : DomainObject
    {
        private string m_compare_value = string.Empty;
        private string m_conditionname = string.Empty;
        private string m_field_type = string.Empty;
        private string m_operation = string.Empty;
        private string m_relation = string.Empty;
        private string m_tablename = string.Empty;
        private SkyMap.Net.Criteria.TyFieldsSelect m_ty_fieldsselect;
        private SkyMap.Net.Criteria.TyFilter m_ty_filter;

        public override string ToString()
        {
            return this.ConditionName;
        }

        public string CompareValue
        {
            get
            {
                return this.m_compare_value;
            }
            set
            {
                if ((value != null) && (value.Length > 500))
                {
                    throw new ArgumentOutOfRangeException("Invalid value for CompareValue", value, value.ToString());
                }
                this.m_compare_value = value;
            }
        }

        public string ConditionName
        {
            get
            {
                return this.m_conditionname;
            }
            set
            {
                if ((value != null) && (value.Length > 50))
                {
                    throw new ArgumentOutOfRangeException("Invalid value for ConditionName", value, value.ToString());
                }
                this.m_conditionname = value;
            }
        }

        private string Description
        {
            get
            {
                return string.Empty;
            }
            set
            {
            }
        }

        public string FieldType
        {
            get
            {
                return this.m_field_type;
            }
            set
            {
                if ((value != null) && (value.Length > 50))
                {
                    throw new ArgumentOutOfRangeException("Invalid value for FieldType", value, value.ToString());
                }
                this.m_field_type = value;
            }
        }

        public string TABLENAME
        {
            get
            {
                return this.m_tablename;
            }
            set
            {
                if ((value != null) && (value.Length > 50))
                {
                    throw new ArgumentOutOfRangeException("Invalid value for FieldType", value, value.ToString());
                }
                this.m_tablename = value;
            }
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

        public string Operation
        {
            get
            {
                return this.m_operation;
            }
            set
            {
                if ((value != null) && (value.Length > 10))
                {
                    throw new ArgumentOutOfRangeException("Invalid value for Operation", value, value.ToString());
                }
                this.m_operation = value;
            }
        }

        public string Relation
        {
            get
            {
                return this.m_relation;
            }
            set
            {
                if ((value != null) && (value.Length > 10))
                {
                    throw new ArgumentOutOfRangeException("Invalid value for Relation", value, value.ToString());
                }
                this.m_relation = value;
            }
        }

        public SkyMap.Net.Criteria.TyFieldsSelect TyFieldsSelect
        {
            get
            {
                return this.m_ty_fieldsselect;
            }
            set
            {
                this.m_ty_fieldsselect = value;
            }
        }

        public SkyMap.Net.Criteria.TyFilter TyFilter
        {
            get
            {
                return this.m_ty_filter;
            }
            set
            {
                this.m_ty_filter = value;
            }
        }

    }
}

