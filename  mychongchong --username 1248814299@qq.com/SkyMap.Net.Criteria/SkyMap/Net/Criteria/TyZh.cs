namespace SkyMap.Net.Criteria
{
    using SkyMap.Net.DAO;
    using System;

    [Serializable]
    public class TyZh : DomainObject
    {
        private string m_searchx_id;

        public string SearchxId
        {
            get
            {
                return this.m_searchx_id;
            }
            set
            {
                if ((value != null) && (value.Length > 50))
                {
                    throw new ArgumentOutOfRangeException("Invalid value for SearchxId", value, value.ToString());
                }
                this.m_searchx_id = value;
            }
        }
    }
}

