namespace SkyMap.Net.Criteria
{
    using SkyMap.Net.DAO;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    [Serializable]
    public class TySearchx : ManyToOneDataSource, ISaveAs
    {
        private bool m_ismany = false;
        private string m_preCondition;
        private string m_tablename = string.Empty;
        private IList<TyFieldsSelect> m_ty_fieldsselects = new List<TyFieldsSelect>();
        private IList<TyFilter> m_ty_filters = new List<TyFilter>();
        private IList<TySearchtable> m_ty_searchtables = new List<TySearchtable>();
        private int m_zh_id = 0;

        public void SaveAs(UnitOfWork unitOfWork)
        {
            unitOfWork.RegisterNew(this);
            foreach (TySearchtable searchtable in this.TySearchtables)
            {
                unitOfWork.RegisterNew(searchtable);
            }
            foreach (TyFieldsSelect select in this.TyFieldsSelects)
            {
                unitOfWork.RegisterNew(select);
            }
        }

        [DisplayName("是否多表联查")]
        public bool Ismany
        {
            get
            {
                return this.m_ismany;
            }
            set
            {
                this.m_ismany = value;
            }
        }

        [DisplayName("预置条件")]
        public string PreCondition
        {
            get
            {
                return this.m_preCondition;
            }
            set
            {
                this.m_preCondition = value;
            }
        }

        [DisplayName("主表")]
        public string TableName
        {
            get
            {
                return this.m_tablename;
            }
            set
            {
                if ((value != null) && (value.Length > 50))
                {
                    throw new ArgumentOutOfRangeException("Invalid value for TableName", value, value.ToString());
                }
                this.m_tablename = value;
            }
        }

        [Browsable(false)]
        public IList<TyFieldsSelect> TyFieldsSelects
        {
            get
            {
                return this.m_ty_fieldsselects;
            }
            set
            {
                this.m_ty_fieldsselects = value;
            }
        }

        [Browsable(false)]
        public IList<TyFilter> TyFilters
        {
            get
            {
                return this.m_ty_filters;
            }
            set
            {
                this.m_ty_filters = value;
            }
        }

        [Browsable(false)]
        public IList<TySearchtable> TySearchtables
        {
            get
            {
                return this.m_ty_searchtables;
            }
            set
            {
                this.m_ty_searchtables = value;
            }
        }

        [DisplayName("复合查询ID"), Browsable(false)]
        public int ZhId
        {
            get
            {
                return this.m_zh_id;
            }
            set
            {
                this.m_zh_id = value;
            }
        }
    }
}

