using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Collections;

namespace ZBPMPRINT
{
    public class clsFinancial
    {
        private string m_strDate;

        public string StrDate
        {
            get { return m_strDate; }
            set { m_strDate = value; }
        }
        private string m_strRemitPerson;

        public string StrRemitPerson
        {
            get { return m_strRemitPerson; }
            set { m_strRemitPerson = value; }
        }
        private string m_strRemitAccount;

        public string StrRemitAccount
        {
            get { return m_strRemitAccount; }
            set { m_strRemitAccount = value; }
        }
        private string m_strRemitBank;

        public string StrRemitBank
        {
            get { return m_strRemitBank; }
            set { m_strRemitBank = value; }
        }
        private string m_strMoney;

        public string StrMoney
        {
            get { return m_strMoney; }
            set { m_strMoney = value; }
        }
        private string m_strGatheringPerson;

        public string StrGatheringPerson
        {
            get { return m_strGatheringPerson; }
            set { m_strGatheringPerson = value; }
        }
        private string m_strGatheringAccount;

        public string StrGatheringAccount
        {
            get { return m_strGatheringAccount; }
            set { m_strGatheringAccount = value; }
        }
        private string m_strGatheringBank;

        public string StrGatheringBank
        {
            get { return m_strGatheringBank; }
            set { m_strGatheringBank = value; }
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

        public clsFinancial()
        {
            //throw new System.NotImplementedException();
            items = new clsItems();
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

    }
}