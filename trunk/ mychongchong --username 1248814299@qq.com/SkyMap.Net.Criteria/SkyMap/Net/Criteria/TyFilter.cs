namespace SkyMap.Net.Criteria
{
    using SkyMap.Net.DAO;
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class TyFilter : DomainObject
    {
        private IList<TyFilterCondition> m_TR_FILTER_CONDITIONs = new List<TyFilterCondition>();
        private SkyMap.Net.Criteria.TySearchx m_ty_searchx;

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

