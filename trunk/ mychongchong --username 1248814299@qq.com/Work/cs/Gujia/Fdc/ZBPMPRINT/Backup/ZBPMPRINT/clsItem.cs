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
        private double m_strMoney;

        [DisplayName("½ð¶î")]
        public double StrMoney
        {
            get { return m_strMoney; }
            set { m_strMoney = value; }
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