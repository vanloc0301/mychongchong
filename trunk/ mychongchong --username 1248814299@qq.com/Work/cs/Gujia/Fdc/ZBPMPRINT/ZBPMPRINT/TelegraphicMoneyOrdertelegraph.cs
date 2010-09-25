using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ZBPMPRINT
{
    public partial class TelegraphicMoneyOrdertelegraph : UserControl
    {
        clsTelegraphicMoneyOrdertelegraph m_clsObject;

        public clsTelegraphicMoneyOrdertelegraph ClsObject
        {
            get {
                m_clsObject.StrDate = m_dtm.Value.ToString();//.ToString("yyyy MM dd");
                m_clsObject.StrGatheringAccount = m_cboGatheringAccount.Text;
                m_clsObject.StrGatheringBank = m_cboGatheringBank.Text;
                m_clsObject.StrGatheringPerson = m_cboGatheringPerson.Text;
                m_clsObject.StrRemitAccount = m_cboRemitAccount.Text;
                m_clsObject.StrRemitBank = m_cboRemitBank.Text;
                m_clsObject.StrRemitPerson = m_cboRemitPerson.Text;
                m_clsObject.StrInfo = m_cboInfo.Text;
                m_clsObject.DecMoney =string.IsNullOrEmpty(m_cboMoney.Text)?0: Convert.ToDecimal(m_cboMoney.Text);
                m_clsObject.StrMoney = m_cboMoney.Text;
                m_clsObject.StrCapital = SkyMap.Net.ZSTax.SupportHelper.convertsum1(m_cboMoney.Text);
                return m_clsObject; }
            set { m_clsObject = value; }
        }
        public TelegraphicMoneyOrdertelegraph()
        {
            InitializeComponent();
        }

        private void TelegraphicMoneyOrdertelegraph_Load(object sender, EventArgs e)
        {
            m_clsObject = new clsTelegraphicMoneyOrdertelegraph();
        }

        private void m_cboMoney_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }
    }
}
