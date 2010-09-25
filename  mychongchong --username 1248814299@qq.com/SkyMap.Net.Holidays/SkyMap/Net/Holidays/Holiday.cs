namespace SkyMap.Net.Holidays
{
    using SkyMap.Net.DAO;
    using System;

    [Serializable]
    public sealed class Holiday : DomainObject
    {
        private string m_description = string.Empty;
        private DateTime m_holi_date;

        public DateTime HoliDate
        {
            get
            {
                return this.m_holi_date;
            }
            set
            {
                this.m_holi_date = value;
            }
        }

        public override string Id
        {
            get
            {
                return this.HoliDate.ToString();
            }
            set
            {
            }
        }
    }
}

