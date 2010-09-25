using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.ComponentModel;
namespace ZBPMPRINT
{
    
    public class clsItem
    {
        private string m_strName;

        [DisplayName("ÏîÄ¿")]
        public string StrName
        {
            get { return m_strName; }
            set { m_strName = value; }
        }
        private decimal? m_decMoney;

        [DisplayName("½ð¶î")]
        public decimal? Money
        {
            get { return m_decMoney; }
            set { m_decMoney = value; }
        }

        public clsItem()
        {
           // throw new System.NotImplementedException();
        }
    }

    public class clsItems : CollectionBase
    {
        public clsItems()
        {
            //throw new System.NotImplementedException();
        }
    
        public object AddNew()
        {
            clsItem item = new clsItem();
            List.Add(item);
            return item;
        }
    }
}