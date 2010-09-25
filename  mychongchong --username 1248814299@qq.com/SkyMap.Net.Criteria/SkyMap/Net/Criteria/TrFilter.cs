namespace SkyMap.Net.Criteria
{
    using SkyMap.Net.DAO;
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class TrFilter : DomainObject
    {
        private IList<TrFilterCondition> m_TR_FILTER_CONDITIONs = new List<TrFilterCondition>();
        private SkyMap.Net.Criteria.TyQuery m_ty_query;

        public IList<TrFilterCondition> TrFilterConditions
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

        public SkyMap.Net.Criteria.TyQuery TyQuery
        {
            get
            {
                return this.m_ty_query;
            }
            set
            {
                this.m_ty_query = value;
            }
        }
    }
}

