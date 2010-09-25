using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ZBPMPRINT
{
    public partial class Check : UserControl
    {
        clsCheck m_clsObject;

        public clsCheck ClsObject
        {
            get {
                m_clsObject.StrDate = m_dtm.Value.ToString("yyyy MM dd");
                m_clsObject.StrDateCapital = "";
                m_clsObject.StrMoney = m_cboMoney.Text;
                m_clsObject.StrCapital = SkyMap.Net.ZSTax.SupportHelper.convertsum(m_cboMoney.Text);
                m_clsObject.StrUse = m_cboUse.Text;
                m_clsObject.StrInfo = m_cboInfo.Text;
                m_clsObject.StrGatheringPerson = m_cboGatheringPerson.Text;

                return m_clsObject; }
            set { m_clsObject = value; }
        }

        public Check()
        {
            InitializeComponent();
        }

        private void Check_Load(object sender, EventArgs e)
        {
            m_clsObject = new clsCheck();
        }

    }
}
