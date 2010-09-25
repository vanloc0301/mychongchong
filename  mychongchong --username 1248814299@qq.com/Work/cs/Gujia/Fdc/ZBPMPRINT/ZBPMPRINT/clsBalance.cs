using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace ZBPMPRINT
{
    public class clsBalance
    {

        private string m_strPayUnit;
        private string m_strDate;

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
        private string m_strCapital;

        public string StrCapital
        {
            get { return m_strCapital; }
            set { m_strCapital = value; }
        }
        private string m_strRemark;

        public string StrRemark
        {
            get { return m_strRemark; }
            set { m_strRemark = value; }
        }

        public string StrDate
        {
            get { return m_strDate; }
            set { m_strDate = value; }
        }

        public string StrPayUnit
        {
            get { return m_strPayUnit; }
            set { m_strPayUnit = value; }
        }
        
        clsItems items;

        public virtual IList Items
        {
            get
            {
                return items;
            }
            set
            {
                if (value == null)
                {
                    items = null;
                    return;
                }
                if (!(value is clsItems))
                {
                    items = new clsItems();
                    foreach (clsItem item in value)
                    {
                        items.AddNew();
                    }
                }
                else
                {
                    items = (clsItems)value;
                }
            }
        }

        public clsBalance()
        {
            items = new clsItems();
        }

        private string m_strUser;

        public string StrUser
        {
            get { return m_strUser; }
            set { m_strUser = value; }
        }
    }
}
