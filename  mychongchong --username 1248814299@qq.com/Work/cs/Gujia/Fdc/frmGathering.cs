using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ZBPM
{
    public partial class frmGathering : Form
    {
        public string m_strDkid;
        public DataSet m_dstDjksq;
        public DataSet m_dstSjdjksq;
        public frmGathering()
        {
            InitializeComponent();
        }

        private void frmGathering_Load(object sender, EventArgs e)
        {
            m_dstDjksq = SkyMap.Net.DAO.QueryHelper.ExecuteSqls("SkyMap.Net.DAO", new string[]{@"SELECT djksqid, B_id, 支付日期, 支付比例, 支付金额, 实际支付日期, 实际支付比例, 
      实际支付金额
FROM YW_tdzbpm_djksq where B_id ="+m_strDkid}, new string[] { "" });
            if (m_dstDjksq != null && m_dstDjksq.Tables.Count != 0)
            {
                m_dstDjksq.Tables[0].ExtendedProperties.Add("selectsql", @"SELECT *
FROM YW_tdzbpm_djksq where B_id =" + m_strDkid);
                grid_djksq.DataMember = m_dstDjksq.Tables[0].TableName;
                grid_djksq.DataSource = m_dstDjksq;
            }

           
        }

        private void m_btnSave_Click(object sender, EventArgs e)
        {

            SaveData();
        }
        private void SaveData()
        {
            SkyMap.Net.DataForms.DataEngine.SQLDataEngine sqlDataEngine ;
            if (m_dstDjksq.HasChanges() == true)
            {
                foreach (DataRow dr in m_dstDjksq.Tables[0].Rows)
                {
                    dr["B_id"] = m_strDkid;
                }
                sqlDataEngine = new SkyMap.Net.DataForms.DataEngine.SQLDataEngine();
                sqlDataEngine.SaveData(m_dstDjksq);
            }

          
        }
    }
}