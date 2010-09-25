using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ZBPMPRINT
{
    public partial class Balance : UserControl
    {
        clsBalance m_clsObject;

        public clsBalance ClsObject
        {
            get {
                decimal decMoney = 0;
                m_clsObject.StrDate = m_dtm.Value.ToString();
                m_clsObject.StrPayUnit = m_cboPayUnit.Text;
                m_clsObject.StrRemark = m_cboRemark.Text;
                foreach (clsItem item in m_clsObject.Items)
                {
                    decMoney += item.Money.HasValue ? item.Money.Value : 0;
                }
                m_clsObject.StrMoney = decMoney.ToString();
                m_clsObject.StrCapital = SkyMap.Net.ZSTax.SupportHelper.SimpleConverChinese(Convert.ToDouble( decMoney.ToString()), 999999999);
                m_clsObject.StrCapital = m_clsObject.StrCapital.Substring(m_clsObject.StrCapital.LastIndexOf(" ") + 2);
                m_clsObject.StrUser = m_cboUser.Text;
                return m_clsObject; }
            set { m_clsObject = value; }
        }

        public Balance()
        {
            InitializeComponent();
        }

        private void Balance_Load(object sender, EventArgs e)
        {
            m_clsObject = new clsBalance();
           
            ((clsItems)m_clsObject.Items).AddNew();
            ((clsItems)m_clsObject.Items).AddNew();
            ((clsItems)m_clsObject.Items).AddNew();
            m_grvItems.DataSource = m_clsObject.Items;
        }
        private void m_grvItems_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("数值类型有误!");
        }
    }
}
