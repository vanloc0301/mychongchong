namespace SkyMap.Net.Holidays
{
    using SkyMap.Net.DAO;
    using System;

    [Serializable]
    public sealed class Fixholiday : DomainObject
    {
        private string m_description = string.Empty;
        private string m_holi_date = string.Empty;

        public string HoliDate
        {
            get
            {
                return this.m_holi_date;
            }
            set
            {
                if ((value != null) && (value.Length > 5))
                {
                    throw new ArgumentOutOfRangeException("Invalid value for HoliDate", value, value.ToString());
                }
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

