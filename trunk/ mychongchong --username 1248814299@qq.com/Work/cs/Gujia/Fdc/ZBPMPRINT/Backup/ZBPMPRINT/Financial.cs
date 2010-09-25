using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ZBPMPRINT
{
    public partial class Financial : UserControl
    {
  

        clsFinancial m_clsObject;

        public clsFinancial ClsObject
        {
            get
            {
                double dlbMoney = 0;
                m_clsObject.StrDate = m_dtm.Value.ToString("yyyy MM dd");
                m_clsObject.StrGatheringAccount = m_cboGatheringAccount.Text;
                m_clsObject.StrGatheringBank = m_cboGatheringBank.Text;
                m_clsObject.StrGatheringPerson = m_cboGatheringPerson.Text;
                m_clsObject.StrRemitAccount = m_cboRemitAccount.Text;
                m_clsObject.StrRemitBank = m_cboRemitBank.Text;
                m_clsObject.StrRemitPerson = m_cboRemitPerson.Text;
                m_clsObject.StrInfo = m_cboInfo.Text;
                foreach (clsItem item in m_clsObject.Items)
                {
                    dlbMoney += Convert.ToDouble(item.StrMoney);
                }
                m_clsObject.StrMoney = dlbMoney.ToString();
                m_clsObject.StrCapital = SkyMap.Net.ZSTax.SupportHelper.SimpleConverChinese(dlbMoney, 999999999);
                return m_clsObject;
            }
            set { m_clsObject = value; }
        }
        public Financial()
        {
            InitializeComponent();
           
        }

        private void Financial_Load(object sender, EventArgs e)
        {
            m_clsObject = new clsFinancial();
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
