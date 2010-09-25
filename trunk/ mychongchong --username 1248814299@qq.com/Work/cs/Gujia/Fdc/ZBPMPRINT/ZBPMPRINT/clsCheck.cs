using System;
using System.Collections.Generic;
using System.Text;

namespace ZBPMPRINT
{
   public class clsCheck
    {
        private string m_strDate;

        public string StrDate
        {
            get { return m_strDate; }
            set { m_strDate = value; }
        }
      
        private string m_strDateCapital;

        public string StrDateCapital
        {
            get { return m_strDateCapital; }
            set { m_strDateCapital = value; }
        }
        private string m_strYearCapital;
        public string StrYearCapital
        {
            get { return m_strYearCapital; }
            set { m_strYearCapital = value; }
        }
        private string m_strMonthCapital;
        public string StrMonthCapital
        {
            get { return m_strMonthCapital; }
            set { m_strMonthCapital = value; }
        }
        private string m_strDayCapital;
        public string StrDayCapital
        {
            get { return m_strDayCapital; }
            set { m_strDayCapital = value; }
        }
        private string m_strGatheringPerson;

        public string StrGatheringPerson
        {
            get { return m_strGatheringPerson; }
            set { m_strGatheringPerson = value; }
        }

        private string m_strInfo;

        public string StrInfo
        {
            get { return m_strInfo; }
            set { m_strInfo = value; }
        }
        private string m_strCapital;

        public string StrCapital
        {
            get { return m_strCapital; }
            set { m_strCapital = value; }
        }

        private string m_strMoney;

        public string StrMoney
        {
            get { return m_strMoney; }
            set { m_strMoney = value; }
        }
        private decimal m_decMoney;
        public decimal DecMoney
        {
            get { return m_decMoney; }
            set { m_decMoney = value; }
        }
        private string m_strUse;

        public string StrUse
        {
            get { return m_strUse; }
            set { m_strUse = value; }
        }

        public clsCheck()
        {
            //throw new System.NotImplementedException();
        }
    }
}
