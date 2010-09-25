using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Configuration;
using SkyMap.Net.Core;
using SkyMap.Net.Gui;
using SkyMap.Net.DataForms;
using System.Collections;
using SkyMap.Net.Gui.Components;
using SkyMap.Net.DAO;
using System.Xml;
using System.Xml.Xsl;
using System.IO;
using SkyMap.Net.Workflow.Client.View;
using DevExpress.XtraEditors;
using SkyMap.Net.DataAccess;
using System.Reflection;
using Microsoft.Data.ConnectionUI;

namespace ZBPM
{
    public partial class ZBPMDataForms : WfAbstractDataForm
    {
        #region 全局变量
        /// <summary>
        /// 地块信息
        /// </summary>
        private DataSet m_dstDk;
        private DataSet m_dstYw;
        private DataSet m_dstAll;
        private DataSet m_dstJT;//竞投数据
        private DataTable m_dtbYw;
        private DataTable m_dtbB;
        private DataTable m_dtbDK;
        private DataTable m_dtbDjksq;
        private DataTable m_dtbGpbj;
        private DataTable m_dtbPhdj;
        private string m_strType;
        private string m_strChange;
        private string m_strServerTime;
        private string m_strYwNo;//业务编号
        private string m_strEdition;
        private bool m_blnWatch;
        private bool m_blnConsign;
        private bool m_blnDirectness;
        int m_intFocusedRow;

        private PrintSet m_printSet;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit comboxB;
        #endregion

        #region 初始化业务数据及报表模板
        public ZBPMDataForms()
        {
            InitializeComponent();
            Init();

        }

        private void AwokeUpdateReport()
        {

            string strActdefId = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PActdefId, "");
            string strProjectID = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProjectId, "");

            string sql = @"select  count(1) as rcount from  dbo.WF_ACTDEFFORMPERMISSION 
                         inner join dbo.DF_TEMPLETPRINT_PRINTSET 
                        on WF_ACTDEFFORMPERMISSION.PRINTSET_ID = dbo.DF_TEMPLETPRINT_PRINTSET.PRINTSET_ID
                        inner join dbo.DF_TEMPLETPRINT f on dbo.DF_TEMPLETPRINT_PRINTSET.TEMPLETPRINT_ID = f.TEMPLETPRINT_ID
                        where  ACTDEF_ID ='{1}'   and  f.templetprint_id in(select templetprint_id from YW_tdzbpm_Report d where project_id='{0}' and f.templetprint_id=d.templetprint_id and f.REPLICATION_VERSION!=d.REPLICATION_VERSION)";
            sql = string.Format(sql, strProjectID, strActdefId);
            DataTable oldTable = SkyMap.Net.DAO.QueryHelper.ExecuteSql("Default", string.Empty, sql);

            if (!oldTable.Rows[0]["rcount"].Equals(0))
            {
                if (MessageBox.Show("有新版本报表，是否更新?", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    UpdateReport();

            }




        }

        private void Init()
        {



            if (cmb_type.Properties.DataSource == null)
            {
                DataWordLookUpEditHelper.Init(cmb_type, "type", "Name", "Code");
                DataWordLookUpEditHelper.Init(cmb_type1, "type", "Name", "Code");
                DataWordLookUpEditHelper.Init(cmb_type2, "type", "Name", "Code");
            }

            if (cmb_exchange.Properties.DataSource == null)
            {
                DataWordLookUpEditHelper.Init(cmb_exchange, "exchange", "Name", "Code");
                DataWordLookUpEditHelper.Init(cmb_exchange1, "exchange", "Name", "Code");
            }

            if (cmb_ZSTOWNSHIP.Properties.DataSource == null)
            {
                DataWordLookUpEditHelper.Init(cmb_ZSTOWNSHIP, "ZSTOWNSHIP", "Name", "Code");
            }

            if (cmb_Edition.Properties.DataSource == null)
            {
                DataWordLookUpEditHelper.Init(cmb_Edition, "Edition", "Name", "Code");
            }


            this.repositoryItemTextEdit后几天.Leave += new EventHandler(repositoryItemTextEdit后几天_Leave);

            this.repositoryItemTextEdit支付比例.Leave += new EventHandler(repositoryItemTextEdit支付比例_Leave);
            this.cmb_ZSTOWNSHIP.EditValueChanged += new System.EventHandler(this.cmb_type_EditValueChanged);
            this.cmb_exchange.EditValueChanged += new System.EventHandler(this.cmb_type_EditValueChanged);
            this.cmb_type.EditValueChanged += new System.EventHandler(this.cmb_type_EditValueChanged);

            this.cmb_exchange1.EditValueChanged += new System.EventHandler(this.cmb_type1_EditValueChanged);
            this.cmb_type1.EditValueChanged += new System.EventHandler(this.cmb_type1_EditValueChanged);
            this.cmb_type2.EditValueChanged += new EventHandler(cmb_type2_EditValueChanged);
            this.cmb_Edition.EditValueChanged += new EventHandler(cmb_Edition_EditValueChanged);

            this.cmb_IsWatch.SelectedValueChanged += new EventHandler(cmb_IsWatch_SelectedValueChanged);
            this.cmb_Consign.SelectedValueChanged += new EventHandler(cmb_Consign_SelectedValueChanged);


            this.cmb_exchange.MouseUp += new MouseEventHandler(cmb_exchange_MouseUp);
            this.cmb_type.MouseUp += new MouseEventHandler(cmb_exchange_MouseUp);
            this.cmb_exchange1.MouseUp += new MouseEventHandler(cmb_exchange_MouseUp);
            this.cmb_type1.MouseUp += new MouseEventHandler(cmb_exchange_MouseUp);
            this.cmb_type2.MouseUp += new MouseEventHandler(cmb_exchange_MouseUp);
            this.cmb_Edition.MouseUp += new MouseEventHandler(cmb_exchange_MouseUp);

            cmb_IsWatch.MouseUp += new MouseEventHandler(cmb_exchange_MouseUp);
            cmb_Consign.MouseUp += new MouseEventHandler(cmb_exchange_MouseUp);
            cmb业务状态.MouseUp += new MouseEventHandler(cmb_exchange_MouseUp);
            //dt_公告结束日期.MouseUp += new MouseEventHandler(cmb_exchange_MouseUp);
            dt_公告结束日期.EditValueChanged += new EventHandler(dt_公告结束日期_EditValueChanged);
            repositoryItemTextEdit后几天.KeyPress += new KeyPressEventHandler(cmb_公告天数_KeyPress);
            this.view.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(view_CellValueChanged);
            this.viewb.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(view_CellValueChanged);
            this.viewTD.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(view_CellValueChanged);
            this.gridViewB.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(view_CellValueChanged);
            this.gridViewDK.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(view_CellValueChanged);
            this.gridViewBdjksq.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(view_CellValueChanged);
            this.gridViewBgpbj.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(view_CellValueChanged);
            this.gridViewBphdj.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(view_CellValueChanged);
            this.gridViewBxcbj.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(view_CellValueChanged);
            gridViewB3.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(view_CellValueChanged);

            //出让合同编号
            repositoryItemButtonEdit1.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(repositoryItemButtonEdit1_ButtonClick);
            //土地交付中
            repositoryItemComboBox34.CloseUp += new DevExpress.XtraEditors.Controls.CloseUpEventHandler(repositoryItemComboBox34_CloseUp);
            //成交后的缴款日期
            //repositoryItemTextEdit13.KeyPress += new KeyPressEventHandler(repositoryItemTextEdit13_KeyPress);

            this.col评估地价.Visible = false;
            this.col评估地价1.Visible = false;
            this.col出让金.Visible = false;
            col耕地占用税.Visible = false;
            this.col评估地价4.Visible = false;
            dt_公告起始日期.DateTime = DateTime.Now;

            cmb_IsWatch.Visible = false;
        }

        void repositoryItemTextEdit13_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) || e.KeyChar == ((Char)Keys.Back) || Char.IsSymbol(e.KeyChar))
            {
                return;
            }
            else
            {
                e.Handled = true;
            }
        }

        void repositoryItemComboBox34_CloseUp(object sender, DevExpress.XtraEditors.Controls.CloseUpEventArgs e)
        {
            if (m_strType == "0")
            {

                if (e.Value.ToString() == "土地交付中业务" || e.Value.ToString() == "结案业务")
                {
                    if (!gridB4.FocusedView.IsDetailView)
                    {
                        this.gridViewB3.PostEditor();
                        this.gridViewB3.UpdateCurrentRow();
                        DataRow dr = ((DevExpress.XtraGrid.Views.Grid.GridView)gridB4.FocusedView).GetDataRow(((DevExpress.XtraGrid.Views.Grid.GridView)gridB4.FocusedView).FocusedRowHandle);
                        if (dr != null)
                        {
                            DataTable dataTable = SkyMap.Net.DAO.QueryHelper.ExecuteSqls("Default", string.Empty, new string[]{@"SELECT sum(成交价支付金额) as 出让金,sum( 实际支付金额) as 实收出让金
FROM YW_tdzbpm_djksq where B_id ="+dr["Bid"]}, new string[] { "YW_tdzbpm_djksq" }).Tables[0];

                            double douCrj = dataTable.Rows[0]["出让金"] == DBNull.Value ? 0 : Convert.ToDouble(dataTable.Rows[0]["出让金"].ToString());

                            double douSjcrj = dataTable.Rows[0]["实收出让金"] == DBNull.Value ? 0 : Convert.ToDouble(dataTable.Rows[0]["实收出让金"].ToString());

                            PatchForCRHTBH.clsCRHDClass obj = new PatchForCRHTBH.clsCRHDClass();

                            ADODB.Connection con = new ADODB.Connection();

                            con.Open("Provider=SQLOLEDB;data source=192.168.9.24;DATABASE=Gtoa;UID=gtoauser;PWD=chubangjiangyou;", "gtoauser", "chubangjiangyou", 0);
                            
                            //1:数据连接;2:业务编号;3:"招标拍卖挂牌";4:"招标办";5:原产权人;6:竞得人;7:实际面积;8:出让金;9:sscrje（实收出让金额） ;10:用户名称;11:用户角色;12:镇区编号;13:;14
                            obj.UpdateCRHT(ref con, dr["合同编号"].ToString(), dr["宗地编号标"].ToString(), txt权属单位.Text, dr["竞得人"].ToString(), dr["用地面积"].ToString(), douCrj, douSjcrj, SkyMap.Net.Security.SecurityUtil.GetSmIdentity().UserId, SkyMap.Net.Security.SecurityUtil.GetSmIdentity().UserName, cmb_ZSTOWNSHIP.Text);
                            obj.OkCRHT(ref con, dr["合同编号"].ToString(), dr["宗地编号标"].ToString(), SkyMap.Net.Security.SecurityUtil.GetSmIdentity().UserId, SkyMap.Net.Security.SecurityUtil.GetSmIdentity().UserName);
                            // dr["合同编号"] = obj.ObtainBH(ref con, dr["宗地编号标"].ToString(), "招标拍卖挂牌", "招标办", txt权属单位.Text, dr["竞得人"].ToString(), dr["用地面积"].ToString(), Convert.ToDouble(dr["出让金"].ToString()), Convert.ToDouble(dr["出让金"].ToString()), SkyMap.Net.Security.SecurityUtil.GetSmIdentity().UserId, SkyMap.Net.Security.SecurityUtil.GetSmIdentity().UserName, cmb_ZSTOWNSHIP.Properties.ValueMember, 0, 0);
                            // obj.UpdateCRHT

                            con.Close();
                            this.gridViewB3.PostEditor();
                            this.gridViewB3.UpdateCurrentRow();
                            OnChanged(this, null);


                        }
                        else
                        {
                            MessageHelper.ShowInfo("选择标！");
                        }
                    }
                    else
                    {
                        MessageHelper.ShowInfo("选择标！");
                    }
                }
            }
        }

        void repositoryItemButtonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (m_strType == "9") return;

            if (!gridB4.FocusedView.IsDetailView)
            {
                DataRow dr = ((DevExpress.XtraGrid.Views.Grid.GridView)gridB4.FocusedView).GetDataRow(((DevExpress.XtraGrid.Views.Grid.GridView)gridB4.FocusedView).FocusedRowHandle);
                if (dr != null)
                {

                    if (MessageHelper.ShowYesNoInfo("需要获取合同编号吗？") == DialogResult.Yes)
                    {
                        this.gridViewB3.PostEditor();
                        this.gridViewB3.UpdateCurrentRow();

                        PatchForCRHTBH.clsCRHDClass obj = new PatchForCRHTBH.clsCRHDClass();
                        //System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        //if (config.AppSettings.Settings["竞投数据连接"].Value.ToString() != "")
                        //{

                        //}
                        //else
                        //{
                        //    MessageHelper.ShowInfo("数据连接有误!请从新配置");
                        //    return false;
                        //}
                        ADODB.Connection con = new ADODB.Connection();
                        //con.ConnectionString = "Server=192.168.1.3;initial catalog=gtoa061009;User ID=sa;Password=dev;Min Pool Size=2";
                        //con.Open("Provider=SQLOLEDB;data source=192.168.1.3;DATABASE=gtoa061009;UID=sa;PWD=dev;", "sa", "dev", 0);
                        con.Open("Provider=SQLOLEDB;data source=192.168.9.24;DATABASE=Gtoa;UID=gtoauser;PWD=chubangjiangyou;", "gtoauser", "chubangjiangyou", 0);
                        dr.BeginEdit();
                        //1:数据连接;2:业务编号;3:"招标拍卖挂牌";4:"招标办";5:原产权人;6:竞得人;7:实际面积;8:出让金;9:sscrje（实收出让金额） ;10:用户名称;11:用户角色;12:镇区编号;13:;14

                        dr["合同编号"] = obj.ObtainBH(ref con, dr["宗地编号标"].ToString(), "招标拍卖挂牌", "招标办", txt权属单位.Text, dr["竞得人"].ToString(), dr["用地面积"].ToString(), Convert.ToDouble(dr["出让金"].ToString()), Convert.ToDouble(dr["出让金"].ToString()), SkyMap.Net.Security.SecurityUtil.GetSmIdentity().UserId, SkyMap.Net.Security.SecurityUtil.GetSmIdentity().UserName, cmb_ZSTOWNSHIP.Text, 0, 0);
                        // obj.UpdateCRHT
                        dr.EndEdit();
                        con.Close();
                        this.gridViewB3.PostEditor();
                        this.gridViewB3.UpdateCurrentRow();
                        OnChanged(this, null);

                    }
                }
                else
                {
                    MessageHelper.ShowInfo("选择标！");
                }
            }
            else
            {
                MessageHelper.ShowInfo("选择标！");
            }
        }

        void cmb_Consign_SelectedValueChanged(object sender, EventArgs e)
        {

            if (((ComboBoxEdit)sender).SelectedText == "法院委托")
            {

                m_blnConsign = true;
                cmb委托方.Visible = true;
            }
            else
            {

                m_blnConsign = false;
                cmb委托方.Text = "";
                cmb委托方.Visible = false;
            }
        }

        void cmb_IsWatch_SelectedValueChanged(object sender, EventArgs e)
        {
            if (((ComboBoxEdit)sender).SelectedText == "政府专项监管")
            {
                m_blnWatch = true;
            }
            else
            {
                m_blnWatch = false;
            }

        }

        void cmb_type2_EditValueChanged(object sender, EventArgs e)
        {
            m_strType = cmb_type2.EditValue.ToString();
        }

        void cbx_IsDirectness_CheckedChanged(object sender, EventArgs e)
        {
            m_blnDirectness = ((CheckBox)sender).Checked;
            if (m_blnDirectness)
            {
                simpleButton审批表二.Visible = false;
                simpleButton审批表一.Visible = false;
                simpleButton城区审批表.Visible = true;
            }
            else
            {
                simpleButton审批表二.Visible = true;
                simpleButton审批表一.Visible = true;
                simpleButton城区审批表.Visible = false;
            }
        }

        void cbx_IsWatch_CheckedChanged(object sender, EventArgs e)
        {
            m_blnWatch = ((CheckBox)sender).Checked;
        }

        void cmb_exchange_MouseUp(object sender, MouseEventArgs e)
        {
            OnChanged(sender, null);
        }

        private void InitReport()
        {
            string strActdefId = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PActdefId, "");
            string strProjectID = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProjectId, "");
            string strSQLInsert = @"insert into dbo.YW_tdzbpm_Report(ReportID,PROJECT_ID,TEMPLETPRINT_ID,ReportName,ReportData,REPLICATION_VERSION)
                                    select newid(),'{0}',DF_TEMPLETPRINT.TEMPLETPRINT_ID,DF_TEMPLETPRINT.TEMPLETPRINT_NAME,DF_TEMPLETPRINT.TEMPLETPRINT_DATA,DF_TEMPLETPRINT.REPLICATION_VERSION
                                    from dbo.WF_ACTDEFFORMPERMISSION  inner join dbo.DF_TEMPLETPRINT_PRINTSET 
                                    on WF_ACTDEFFORMPERMISSION.PRINTSET_ID = dbo.DF_TEMPLETPRINT_PRINTSET.PRINTSET_ID
                                    inner join dbo.DF_TEMPLETPRINT on dbo.DF_TEMPLETPRINT_PRINTSET.TEMPLETPRINT_ID = dbo.DF_TEMPLETPRINT.TEMPLETPRINT_ID
                                    where  ACTDEF_ID ='{1}'  and not exists(select * from YW_tdzbpm_Report where YW_tdzbpm_Report.TEMPLETPRINT_ID = DF_TEMPLETPRINT.TEMPLETPRINT_ID and
                                    YW_tdzbpm_Report.PROJECT_ID = '{0}' )";
            strSQLInsert = string.Format(strSQLInsert, strProjectID, strActdefId);
            SkyMap.Net.DAO.QueryHelper.ExecuteSql("Default", string.Empty, strSQLInsert);
        }

        private void BBindData()
        {
            gridB.DataSource = m_dstAll;
            gridB.DataMember = "YW_tdzbpm_b";
            gridB1.DataSource = m_dstAll;
            gridB1.DataMember = "YW_tdzbpm_b";
            gridB2.DataSource = m_dstAll;
            gridB2.DataMember = "YW_tdzbpm_b";
            gridB3.DataSource = m_dstAll;
            gridB3.DataMember = "YW_tdzbpm_b";
            gridB4.DataSource = m_dstAll;
            gridB4.DataMember = "YW_tdzbpm_b";
        }

        private DataSet GetAllData()
        {
            string strProjectId = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProjectId, "");
            m_dtbYw = this.DataFormConntroller.DataSource.Tables["YW_tdzbpm"];
            m_strType = m_dtbYw.Rows[0]["业务类型"].ToString();
            m_strEdition = m_dtbYw.Rows[0]["版本"].ToString();
            m_blnWatch = (bool)(m_dtbYw.Rows[0]["是否政府监管"] == DBNull.Value ? false : m_dtbYw.Rows[0]["是否政府监管"]);
            m_blnConsign = (bool)(m_dtbYw.Rows[0]["是否法院委托"] == DBNull.Value ? false : m_dtbYw.Rows[0]["是否法院委托"]);
            m_blnDirectness = (bool)(m_dtbYw.Rows[0]["是否直接受理"] == DBNull.Value ? false : m_dtbYw.Rows[0]["是否直接受理"]);
            this.cmb_IsWatch.SelectedValueChanged -= new EventHandler(cmb_IsWatch_SelectedValueChanged);
            if (m_blnWatch)
            {

                cmb_IsWatch.Text = "政府专项监管";
            }
            else
            {
                cmb_IsWatch.Text = "非政府专项监管";
            }
            this.cmb_IsWatch.SelectedValueChanged += new EventHandler(cmb_IsWatch_SelectedValueChanged);
            cmb_Consign.SelectedValueChanged -= new EventHandler(cmb_Consign_SelectedValueChanged);
            if (m_blnConsign)
            {

                cmb_Consign.Text = "法院委托";
                cmb委托方.Visible = true;
            }
            else
            {
                cmb_Consign.Text = "非法院委托";
                cmb委托方.Visible = false;
            }
            cmb_Consign.SelectedValueChanged += new EventHandler(cmb_Consign_SelectedValueChanged);
            cmb_Edition.TextChanged -= new EventHandler(DataControlTextChanged);
            cmb_Edition.EditValue = m_strEdition;
            cmb_Edition.TextChanged += new EventHandler(DataControlTextChanged);
            cmb_type1.TextChanged -= new EventHandler(DataControlTextChanged);
            cmb_type1.EditValue = m_strType;
            cmb_type1.TextChanged += new EventHandler(DataControlTextChanged);
            cmb_type2.TextChanged -= new EventHandler(DataControlTextChanged);
            cmb_type2.EditValue = m_strType;
            cmb_type2.TextChanged += new EventHandler(DataControlTextChanged);
            m_strChange = m_dtbYw.Rows[0]["交易方式"].ToString();
            cmb_exchange1.TextChanged += new EventHandler(DataControlTextChanged);
            cmb_exchange1.EditValue = m_strChange;
            cmb_exchange1.TextChanged -= new EventHandler(DataControlTextChanged);

            m_strServerTime = m_dtbYw.Rows[0]["当前服务器时间"].ToString();
            m_dstAll = SkyMap.Net.DAO.QueryHelper.ExecuteSqls("Default", string.Empty, new string[]{@"SELECT * 
FROM YW_tdzbpm_b where PROJECT_ID ='"+strProjectId+"' order by Bid asc","SELECT * FROM YW_tdzbpm_td where PROJECT_ID ='"+strProjectId+"'","SELECT * FROM YW_tdzbpm_djksq where PROJECT_ID ='"+strProjectId+"'","SELECT * FROM YW_tdzbpm_gpbj where PROJECT_ID ='"+strProjectId+"'","SELECT * FROM YW_tdzbpm_phdj where PROJECT_ID ='"+strProjectId+"'","SELECT * FROM YW_tdzbpm_xcbj where PROJECT_ID ='"+strProjectId+"'"}, new string[] { "YW_tdzbpm_b", "YW_tdzbpm_td", "YW_tdzbpm_djksq", "YW_tdzbpm_gpbj", "YW_tdzbpm_phdj", "YW_tdzbpm_xcbj" });
            if (m_dstAll != null && m_dstAll.Tables.Count != 0)
            {
                m_dstAll.Tables["YW_tdzbpm_b"].ExtendedProperties.Add("selectsql", @"SELECT  * 
            FROM YW_tdzbpm_b where PROJECT_ID ='" + strProjectId + "' order by bid asc");

                m_dstAll.Tables["YW_tdzbpm_td"].ExtendedProperties.Add("selectsql", @"SELECT  * 
            FROM YW_tdzbpm_td where PROJECT_ID ='" + strProjectId + "'");
                m_dstAll.Tables["YW_tdzbpm_djksq"].ExtendedProperties.Add("selectsql", @"SELECT * FROM YW_tdzbpm_djksq where PROJECT_ID ='" + strProjectId + "' order by  djksqid ");
                m_dstAll.Tables["YW_tdzbpm_gpbj"].ExtendedProperties.Add("selectsql", @"SELECT * FROM YW_tdzbpm_gpbj where PROJECT_ID ='" + strProjectId + "'");
                m_dstAll.Tables["YW_tdzbpm_phdj"].ExtendedProperties.Add("selectsql", @"SELECT * FROM YW_tdzbpm_phdj where PROJECT_ID ='" + strProjectId + "'");
                m_dstAll.Tables["YW_tdzbpm_xcbj"].ExtendedProperties.Add("selectsql", @"SELECT * FROM YW_tdzbpm_xcbj where PROJECT_ID ='" + strProjectId + "'");
                //地块
                DataRelation dtr = new DataRelation("Btd", m_dstAll.Tables["YW_tdzbpm_b"].Columns["Bid"], m_dstAll.Tables["YW_tdzbpm_td"].Columns["标"], false);
                m_dstAll.Relations.Add(dtr);
                //地价款
                dtr = new DataRelation("Bdjksq", m_dstAll.Tables["YW_tdzbpm_b"].Columns["Bid"], m_dstAll.Tables["YW_tdzbpm_djksq"].Columns["B_id"], false);
                m_dstAll.Relations.Add(dtr);
                InfoControl();
                // dtr = new DataRelation("B", m_dstAll.Tables["YW_tdzbpm_b"].Columns["Bid"], m_dstAll.Tables["YW_tdzbpm_b1"].Columns["Bid"], false);
                //  m_dstAll.Relations.Add(dtr);
                dtr = new DataRelation("Bxcbj", m_dstAll.Tables["YW_tdzbpm_b"].Columns["Bid"], m_dstAll.Tables["YW_tdzbpm_xcbj"].Columns["B_id"], false);
                m_dstAll.Relations.Add(dtr);

                m_dstAll.Tables["YW_tdzbpm_b"].TableNewRow += new DataTableNewRowEventHandler(ZBPMDataForms_BNewRow);
                //m_dstAll.Tables["YW_tdzbpm_b"].RowDeleting += new DataRowChangeEventHandler(ZBPMDataForms_RowDeleting);
                m_dstAll.Tables["YW_tdzbpm_b"].RowDeleted += new DataRowChangeEventHandler(ZBPMDataForms_RowDeleted);

                m_dstAll.Tables["YW_tdzbpm_td"].TableNewRow += new DataTableNewRowEventHandler(ZBPMDataForms_TableNewRow);
                m_dstAll.Tables["YW_tdzbpm_djksq"].TableNewRow += new DataTableNewRowEventHandler(ZBPMDataForms_djksqNewRow);
                m_dstAll.Tables["YW_tdzbpm_gpbj"].TableNewRow += new DataTableNewRowEventHandler(ZBPMDataForms_gpbjNewRow);
                m_dstAll.Tables["YW_tdzbpm_phdj"].TableNewRow += new DataTableNewRowEventHandler(ZBPMDataForms_phdjNewRow);
                m_dstAll.Tables["YW_tdzbpm_xcbj"].TableNewRow += new DataTableNewRowEventHandler(ZBPMDataForms_xcbjNewRow);
            }
            return m_dstAll;
        }



        void repositoryItemTextEdit支付比例_Leave(object sender, EventArgs e)
        {
            if (((DevExpress.XtraEditors.TextEdit)sender).Text.ToString() != "")
            {
                DataRow drB = ((DataRowView)((DevExpress.XtraGrid.Views.Grid.GridView)gridB1.FocusedView).SourceRow).Row;
                DataRow drDjksq = ((DevExpress.XtraGrid.Views.Grid.GridView)gridB1.FocusedView).GetDataRow(((DevExpress.XtraGrid.Views.Grid.GridView)gridB1.FocusedView).FocusedRowHandle);

                if (drB != null && drDjksq != null)
                {
                    if (drB["交易底价"] != DBNull.Value)
                    {
                        double total底价 = Convert.ToDouble(drB["交易底价"].ToString());// -paid;

                        double percent = Convert.ToDouble(((DevExpress.XtraEditors.TextEdit)sender).Text.ToString());
                        double pay底价 = total底价 * percent / 100;

                        drDjksq.BeginEdit();
                        drDjksq["支付金额"] = Math.Round(pay底价, 2);

                        drDjksq.EndEdit();
                    }
                }
            }
        }

        void repositoryItemTextEditcol支付金额_Leave(object sender, EventArgs e)
        {
            if (((DevExpress.XtraEditors.TextEdit)sender).Text.ToString() != "")
            {
                DataRow drB = ((DataRowView)((DevExpress.XtraGrid.Views.Grid.GridView)gridB1.FocusedView).SourceRow).Row;
                DataRow drDjksq = ((DevExpress.XtraGrid.Views.Grid.GridView)gridB1.FocusedView).GetDataRow(((DevExpress.XtraGrid.Views.Grid.GridView)gridB1.FocusedView).FocusedRowHandle);

                if (drB != null && drDjksq != null)
                {
                    double total底价 = Convert.ToDouble(drB["交易底价"].ToString());
                    double total成交价 = Convert.ToDouble(drB["成交价"].ToString());
                    double pay = Convert.ToDouble(((DevExpress.XtraEditors.TextEdit)sender).Text.ToString());
                    double percent = pay / total底价;
                    drDjksq.BeginEdit();
                    drDjksq["支付比例"] = Math.Round(percent, 2);
                    drDjksq.EndEdit();
                }
            }

        }

        void repositoryItemTextEdit后几天_Leave(object sender, EventArgs e)
        {
            try
            {
                if (((DevExpress.XtraEditors.TextEdit)sender).Text.ToString() != "")
                {
                    m_dtbYw = this.DataFormConntroller.DataSource.Tables["YW_tdzbpm"];

                    DataRow dr = ((DevExpress.XtraGrid.Views.Grid.GridView)gridB1.FocusedView).GetDataRow(((DevExpress.XtraGrid.Views.Grid.GridView)gridB1.FocusedView).FocusedRowHandle);
                    dr.BeginEdit();
                    dr["支付日期"] = Convert.ToDateTime(m_dtbYw.Rows[0]["公告结束日期"].ToString()).AddDays(Convert.ToInt32(((DevExpress.XtraEditors.TextEdit)sender).Text.ToString()));
                    dr.EndEdit();
                }
            }
            catch (Exception ex) { LoggingService.Debug(ex.Message); }
        }

        private void view_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            OnChanged(this, null);
        }

        #endregion

        #region 对表单基类数据绑定事件的重写
        protected override void BeforeBindData()
        {
            dt_公告起始日期.EditValueChanged -= new EventHandler(dt_公告起始日期_EditValueChanged);
            this.cmb_公告天数.SelectedValueChanged += new System.EventHandler(this.cmb_公告天数_SelectedValueChanged);
        }

        protected override void AfterBindData()
        {
            base.AfterBindData();
            m_dstAll = GetAllData();
            BBindData();

            switch (m_strChange)
            {
                case "P":
                    m_gbx挂牌报价时间.Visible = false;

                    break;
                case "G":

                    m_gbx挂牌报价时间.Visible = true;

                    break;
                case "W":

                    break;
                case "Z":

                    break;
                default:
                    break;
            }
            switch (m_strType)
            {
                case "0":
                    col年限.Visible = false;
                    col出让年限.Visible = true;
                    col年限1.Visible = false;
                    col出让年限1.Visible = true;


                    if (txt_地价款开户行地址.Text.Trim() == "")
                    {
                        txt_地价款开户行地址.TextChanged -= new EventHandler(DataControlTextChanged);
                        txt_地价款开户行地址.Text = "中山市兴中道2号";
                        txt_地价款开户行地址.TextChanged += new EventHandler(DataControlTextChanged);
                    }
                    if (txt_地价款开户银行.Text.Trim() == "")
                    {
                        txt_地价款开户银行.TextChanged -= new EventHandler(DataControlTextChanged);
                        txt_地价款开户银行.Text = "建设银行中山兴中道支行";
                        txt_地价款开户银行.TextChanged += new EventHandler(DataControlTextChanged);
                    }
                    if (txt_地价款人民币帐号.Text.Trim() == "")
                    {
                        txt_地价款人民币帐号.TextChanged -= new EventHandler(DataControlTextChanged);
                        txt_地价款人民币帐号.Text = "44001782301053001340";
                        txt_地价款人民币帐号.TextChanged += new EventHandler(DataControlTextChanged);
                    }
                    if (txt_地价款收款单位.Text.Trim() == "")
                    {
                        txt_地价款收款单位.TextChanged -= new EventHandler(DataControlTextChanged);
                        txt_地价款收款单位.Text = "中山市财政局";
                        txt_地价款收款单位.TextChanged += new EventHandler(DataControlTextChanged);
                    }
                    col评估地价.Visible = true;
                    col评估地价1.Visible = true;
                    col评估地价2.Visible = true;

                    col评估地价4.Visible = true;
                    this.col出让金.Visible = true;
                    col耕地占用税.Visible = true;
                    break;
                case "9":
                    if (txt_地价款收款单位.Text.Trim() == "")
                    {
                        txt_地价款收款单位.TextChanged -= new EventHandler(DataControlTextChanged);
                        txt_地价款收款单位.Text = txt权属单位.Text;
                        txt_地价款收款单位.TextChanged += new EventHandler(DataControlTextChanged);
                    }
                    //txt_地价款收款单位.TextChanged -= new EventHandler(DataControlTextChanged);
                    //txt_地价款收款单位.Text = txt权属单位.Text;
                    //txt_地价款收款单位.TextChanged += new EventHandler(DataControlTextChanged);
                    col年限.Visible = true;
                    col出让年限.Visible = false;
                    col年限1.Visible = true;
                    col出让年限1.Visible = false;



                    col评估地价.Visible = false;
                    col评估地价1.Visible = false;
                    col评估地价2.Visible = false;
                    //col评估地价3.Visible = false;
                    col评估地价4.Visible = false;

                    this.col出让金.Visible = false;
                    col耕地占用税.Visible = false;
                    //cmb_公告天数.Text = "15";
                    break;
            }


            int i = 0;

            for (i = viewb.RowCount; i >= 0; i--)
            {
                viewb.ExpandMasterRow(i);
            }
            for (i = gridViewB.RowCount; i >= 0; i--)
            {
                gridViewB.ExpandMasterRow(i);
            }
            for (i = gridViewB1.RowCount; i >= 0; i--)
            {
                gridViewB1.ExpandMasterRow(i);
            }
            for (i = gridViewB2.RowCount; i >= 0; i--)
            {
                gridViewB2.ExpandMasterRow(i);
            }
            for (i = gridViewB3.RowCount; i >= 0; i--)
            {
                gridViewB3.ExpandMasterRow(i);
            }
            Screen[] screens = Screen.AllScreens;
            Screen screen = screens[0];//获取屏幕变量

            this.repositoryItemImageEdit2.PopupStartSize = new System.Drawing.Size(screen.Bounds.Width, screen.Bounds.Height);
            this.repositoryItemImageEdit3.PopupStartSize = new System.Drawing.Size(screen.Bounds.Width, screen.Bounds.Height);
            this.repositoryItemImageEdit4.PopupStartSize = new System.Drawing.Size(screen.Bounds.Width, screen.Bounds.Height);
            this.repositoryItemImageEdit5.PopupStartSize = new System.Drawing.Size(screen.Bounds.Width, screen.Bounds.Height);
            this.repositoryItemImageEdit6.PopupStartSize = new System.Drawing.Size(screen.Bounds.Width, screen.Bounds.Height);
            this.repositoryItemImageEdit7.PopupStartSize = new System.Drawing.Size(screen.Bounds.Width, screen.Bounds.Height);
            if (m_strEdition == "Y")
            {
                gridBand9.Visible = true;
                gridBand19.Visible = true;
                gridBand35.Visible = true;
            }
            else
            {
                gridBand9.Visible = false;
                gridBand19.Visible = false;
                gridBand35.Visible = false;
            }
            dt_公告起始日期.EditValueChanged += new EventHandler(dt_公告起始日期_EditValueChanged);
            this.cmb_公告天数.SelectedValueChanged += new System.EventHandler(this.cmb_公告天数_SelectedValueChanged);
            InitReport();
            ChangeByType();
        }
        protected override void BeforeEndEdit()
        {
            base.BeforeEndEdit();
            this.gridB.FocusedView.PostEditor();
            this.gridB.FocusedView.UpdateCurrentRow();
            this.gridB1.FocusedView.PostEditor();
            this.gridB1.FocusedView.UpdateCurrentRow();
            this.gridB2.FocusedView.PostEditor();
            this.gridB2.FocusedView.UpdateCurrentRow();
            this.gridB3.FocusedView.PostEditor();
            this.gridB3.FocusedView.UpdateCurrentRow();
            this.gridB4.FocusedView.PostEditor();
            this.gridB4.FocusedView.UpdateCurrentRow();
        }

        protected override void BeforeSave()
        {
            txt业务编号1.EditValueChanged -= new EventHandler(this.txt业务编号_EditValueChanged);
            txt业务编号.EditValueChanged -= new EventHandler(this.txt业务编号_EditValueChanged);
            txt业务编号.EditValueChanging -= new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.txt业务编号_EditValueChanging);
            cmb_ZSTOWNSHIP.EditValueChanged -= new System.EventHandler(this.cmb_type_EditValueChanged);
            this.cmb_exchange.EditValueChanged -= new System.EventHandler(this.cmb_type_EditValueChanged);
            this.cmb_type.EditValueChanged -= new System.EventHandler(this.cmb_type_EditValueChanged);
            this.cmb_exchange1.EditValueChanged -= new System.EventHandler(this.cmb_type1_EditValueChanged);
            this.cmb_type1.EditValueChanged -= new System.EventHandler(this.cmb_type1_EditValueChanged);
            this.cmb_公告天数.EditValueChanged -= new EventHandler(dt_公告起始日期_EditValueChanged);
            this.dt_公告起始日期.EditValueChanged -= new System.EventHandler(this.dt_公告起始日期_EditValueChanged);
            this.cmb_Edition.EditValueChanged -= new EventHandler(cmb_Edition_EditValueChanged);
            //this.cmb_Consign.EditValueChanged -= new EventHandler(cmb
            m_dtbYw = this.DataFormConntroller.DataSource.Tables["YW_tdzbpm"];



            if (m_dtbYw.Rows.Count > 0)
            {
                DataRow dr = m_dtbYw.Rows[0];
                dr.BeginEdit();
                dr["交易方式"] = m_strChange;
                dr["业务类型"] = m_strType;
                //处理就数据修改业务编号时的问题，因为现在业务编号是按照当前时间和交易方式交易类型镇区来生成 的
                //if (string.IsNullOrEmpty(this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PActinstId, "").ToString()))
                //{
                //    dr["业务编号"] = txt业务编号.Text;
                //}
                //else
                //{
                dr["业务编号"] = m_strYwNo;
                //}

                dr["版本"] = m_strEdition;
                dr["是否政府监管"] = m_blnWatch;
                dr["是否法院委托"] = m_blnConsign;
                dr["是否直接受理"] = m_blnDirectness;
                dr.EndEdit();
            }
            base.BeforeSave();
        }

        protected override void AfterSave()
        {
            base.AfterSave();
            SaveAllData();
            this.cmb_公告天数.EditValueChanged += new EventHandler(dt_公告起始日期_EditValueChanged);
            this.dt_公告起始日期.EditValueChanged += new System.EventHandler(this.dt_公告起始日期_EditValueChanged);
            this.cmb_exchange1.EditValueChanged += new System.EventHandler(this.cmb_type1_EditValueChanged);
            this.cmb_type1.EditValueChanged += new System.EventHandler(this.cmb_type1_EditValueChanged);
            this.cmb_ZSTOWNSHIP.EditValueChanged += new System.EventHandler(this.cmb_type_EditValueChanged);
            this.cmb_exchange.EditValueChanged += new System.EventHandler(this.cmb_type_EditValueChanged);
            this.cmb_type.EditValueChanged += new System.EventHandler(this.cmb_type_EditValueChanged);
            txt业务编号.EditValueChanged += new EventHandler(this.txt业务编号_EditValueChanged);
            txt业务编号1.EditValueChanged += new EventHandler(this.txt业务编号_EditValueChanged);
            txt业务编号.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.txt业务编号_EditValueChanging);
            this.cmb_Edition.EditValueChanged += new EventHandler(cmb_Edition_EditValueChanged);
        }

        /// <summary>
        /// 保存标及与其有主从关系的数据信息
        /// bug：如果删处标后没有把对应的地块信息删除，可通过配置处理
        /// </summary>
        /// 
        private void SaveAllData()
        {
            SMDataSource smDs = this.dataFormController.DAODataForm.DataSource;
            SkyMap.Net.DataForms.DataEngine.SQLDataEngine sqlDataEngine = new SkyMap.Net.DataForms.DataEngine.SQLDataEngine();
            foreach (DataRow dr in m_dstAll.Tables["YW_tdzbpm_b"].Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                {
                    //地块
                    DataRow[] drs1 = m_dstAll.Tables["YW_tdzbpm_td"].Select("标 =" + dr["Bid", DataRowVersion.Original]);
                    foreach (DataRow dr1 in drs1)
                    {
                        dr1.Delete();
                        // m_dstAll.Tables["YW_tdzbpm_td"].Rows.Remove(dr1);
                    }
                    //低价款
                    DataRow[] drs2 = m_dstAll.Tables["YW_tdzbpm_djksq"].Select("B_id =" + dr["Bid", DataRowVersion.Original]);

                    foreach (DataRow dr2 in drs2)
                    {
                        dr2.Delete();
                        // m_dstAll.Tables["YW_tdzbpm_djksq"].Rows.Remove(dr2);
                    }
                    //挂牌报价
                    DataRow[] drs3 = m_dstAll.Tables["YW_tdzbpm_gpbj"].Select("B_id =" + dr["Bid", DataRowVersion.Original]);
                    foreach (DataRow dr3 in drs3)
                    {
                        dr3.Delete();
                        //m_dstAll.Tables["YW_tdzbpm_gpbj"].Rows.Remove(dr3);
                    }
                    //牌号
                    DataRow[] drs4 = m_dstAll.Tables["YW_tdzbpm_phdj"].Select("B_id =" + dr["Bid", DataRowVersion.Original]);
                    foreach (DataRow dr4 in drs4)
                    {
                        dr4.Delete();
                        //m_dstAll.Tables["YW_tdzbpm_phdj"].Rows.Remove(dr4);
                    }

                    //现场报价
                    DataRow[] drs5 = m_dstAll.Tables["YW_tdzbpm_xcbj"].Select("B_id =" + dr["Bid", DataRowVersion.Original]);
                    foreach (DataRow dr5 in drs5)
                    {
                        dr5.Delete();
                        //m_dstAll.Tables["YW_tdzbpm_xcbj"].Rows.Remove(dr5);
                    }
                }
            }
            sqlDataEngine.SaveData(smDs, m_dstAll.Tables["YW_tdzbpm_b"]);
            sqlDataEngine.SaveData(smDs, m_dstAll.Tables["YW_tdzbpm_td"]);
            sqlDataEngine.SaveData(smDs, m_dstAll.Tables["YW_tdzbpm_djksq"]);
            sqlDataEngine.SaveData(smDs, m_dstAll.Tables["YW_tdzbpm_gpbj"]);
            sqlDataEngine.SaveData(smDs, m_dstAll.Tables["YW_tdzbpm_phdj"]);
            sqlDataEngine.SaveData(smDs, m_dstAll.Tables["YW_tdzbpm_xcbj"]);
            sqlDataEngine.RefreshDataset(smDs, m_dstAll);
        }

        #endregion

        #region 子表数据默认值处理
        void ZBPMDataForms_BNewRow(object sender, DataTableNewRowEventArgs e)
        {
            if ((txt业务编号.EditValue != null && txt业务编号.EditValue.ToString() != "") || (txt业务编号1.EditValue != null && txt业务编号1.EditValue.ToString() != ""))
            {
                int intCount = 1;
                foreach (DataRow dr in e.Row.Table.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        intCount++;
                    }
                }
                e.Row["标名"] = "第" + convertchinese(intCount.ToString()) + "标";
                e.Row["标号"] = intCount.ToString();
                if (intCount <= 1)
                {
                    e.Row["宗地编号标"] = txt业务编号.EditValue.ToString();
                }
                else
                {
                    int i = 0;
                    foreach (DataRow dr in e.Row.Table.Rows)
                    {
                        if (dr.RowState != DataRowState.Deleted)
                        {
                            dr["宗地编号标"] = txt业务编号.EditValue.ToString() + "-" + (i + 1).ToString();
                            i++;
                        }
                    }
                    e.Row["宗地编号标"] = txt业务编号.EditValue.ToString() + "-" + (i + 1).ToString();
                }
                e.Row["容积率标准值标"] = "1.5";
                e.Row["PROJECT_ID"] = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProjectId, "");
            }
            else
            {
                MessageHelper.ShowInfo("请先选择业务类型和交易方式!");
            }
        }

        void ZBPMDataForms_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {

            e.Row["PROJECT_ID"] = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProjectId, "");

            e.Row["土地使用证编号"] = "中府国用（    ）第   号";
            if (m_dtbYw != null)
            {
                e.Row["权属单位"] = m_dtbYw.Rows[0]["权属单位"];
                e.Row["土地位置坐落"] = m_dtbYw.Rows[0]["宗地地址"];
            }
            DataRow drSource;

        }
        void ZBPMDataForms_djksqNewRow(object sender, DataTableNewRowEventArgs e)
        {

            e.Row["PROJECT_ID"] = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProjectId, "");


            DataRow drB = ((DevExpress.XtraGrid.Views.Grid.GridView)gridB1.FocusedView.ParentView).GetDataRow(((DevExpress.XtraGrid.Views.Grid.GridView)gridB1.FocusedView.ParentView).FocusedRowHandle);
            if (drB != null && drB["交易底价"] != DBNull.Value)
            {
                double pay支付金额 = Convert.ToDouble(drB["交易底价"]) - Convert.ToDouble(e.Row.Table.Compute("sum(支付金额)", "B_id = " + drB["Bid"]) == DBNull.Value ? 0 : e.Row.Table.Compute("sum(支付金额)", "B_id = " + drB["Bid"]));

                double percent = 100 - Convert.ToDouble(e.Row.Table.Compute("sum(支付比例)", "B_id = " + drB["Bid"].ToString()) == DBNull.Value ? 0 : e.Row.Table.Compute("sum(支付比例)", "B_id = " + drB["Bid"].ToString()));

                e.Row["支付金额"] = pay支付金额;

                e.Row["支付比例"] = percent;
            }

        }
        void ZBPMDataForms_gpbjNewRow(object sender, DataTableNewRowEventArgs e)
        {

            e.Row["PROJECT_ID"] = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProjectId, "");

        }
        void ZBPMDataForms_phdjNewRow(object sender, DataTableNewRowEventArgs e)
        {

            e.Row["PROJECT_ID"] = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProjectId, "");

        }
        void ZBPMDataForms_xcbjNewRow(object sender, DataTableNewRowEventArgs e)
        {

            e.Row["PROJECT_ID"] = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProjectId, "");

        }
        void ZBPMDataForms_RowDeleted(object sender, DataRowChangeEventArgs e)
        {

            if ((txt业务编号.EditValue != null && txt业务编号.EditValue.ToString() != "") || (txt业务编号1.EditValue != null && txt业务编号1.EditValue.ToString() != ""))
            {
                int intCount = 0;
                foreach (DataRow dr in e.Row.Table.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        intCount++;
                    }
                }
                if (intCount == 1)
                {
                    foreach (DataRow dr in e.Row.Table.Rows)
                    {
                        if (dr.RowState != DataRowState.Deleted)
                        {
                            dr["宗地编号标"] = txt业务编号.EditValue.ToString();
                        }
                    }

                }

            }
        }

        private string ComposeInfo(string p_strColumn)
        {
            string strValue = "";

            foreach (DataRow dr in m_dstAll.Tables["YW_tdzbpm_td"].Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    if (dr[p_strColumn].GetType() == typeof(string))
                    {
                        strValue += dr[p_strColumn].ToString() + "/";
                    }

                }
            }
            strValue = strValue.Substring(0, strValue.LastIndexOf("/"));
            return strValue;
        }
        /// 
        /// 将一位数字转换成中文大写数字 
        /// 
        public static string convertchinese(string str)
        {


            string cstr = "";

            switch (str)
            {
                case "0": cstr = "零"; break;
                case "1": cstr = "一"; break;
                case "2": cstr = "二"; break;
                case "3": cstr = "三"; break;
                case "4": cstr = "四"; break;
                case "5": cstr = "五"; break;
                case "6": cstr = "六"; break;
                case "7": cstr = "七"; break;
                case "8": cstr = "八"; break;
                case "9": cstr = "九"; break;
            }

            return (cstr);
        }

        #endregion

        #region 打印控制

        public override PrintSet FilterTempletPrints(PrintSet printSet)
        {
            PrintSet PrintSet = new PrintSet();
            PrintSet.Description = printSet.Description;
            PrintSet.DisplayOrder = printSet.DisplayOrder;
            PrintSet.EnableRemove = printSet.EnableRemove;
            PrintSet.EnableUpdate = printSet.EnableUpdate;
            PrintSet.Id = printSet.Id;
            PrintSet.IsActive = printSet.IsActive;
            PrintSet.Name = printSet.Name;
            PrintSet.ReplicationVersion = printSet.ReplicationVersion;
            int intCount = printSet.TempletPrints.Count;
            int intRow = 0;
            string strDescription = "";
            PrintSet.TempletPrints = new List<TempletPrint>();
            for (int i = intCount - 1; i >= 0; i--)
            {
                strDescription = (printSet.TempletPrints[i] as TempletPrint).Description;
                ///窗口
                if (tab交件信息.PageVisible == true && tab地块信息.PageVisible == false && tab镇区审核.PageVisible == false && tab招标办审核.PageVisible == false)
                {
                    if (strDescription.IndexOf("receipt") != -1)
                    {
                        PrintSet.TempletPrints.Add(printSet.TempletPrints[i]);
                    }
                }
                ////镇区经办
                //else if (tab交件信息.PageVisible == true && tab地块信息.PageVisible == true && tab镇区审核.PageVisible == false && tab招标办审核.PageVisible == false)
                //{
                //    if (strDescription.IndexOf("receipt") != -1)
                //    {
                //        PrintSet.TempletPrints.Add(printSet.TempletPrints[i]);
                //    }
                //}
                ////镇区经办
                //else if (tab交件信息.PageVisible == true && tab地块信息.PageVisible == true && tab镇区审核.PageVisible == true && tab招标办审核.PageVisible == false)
                //{
                //    if ((strDescription.IndexOf("receipt") != -1) || (strDescription.IndexOf("township") != -1 && strDescription.IndexOf(m_strType) != -1) || (strDescription.IndexOf("detail") != -1))//strDescription.IndexOf(cmb_exchange1.Text) != -1&& 
                //    {
                //        PrintSet.TempletPrints.Add(printSet.TempletPrints[i]);
                //    }
                //}
                else
                {
                    //过滤规则1：交易方式，2：业务类型，3：业务版本  备注：*代表不作区分
                    if ((strDescription.IndexOf("receipt") != -1) || (strDescription.IndexOf("detail") != -1) || ((strDescription.Substring(0, 1) == m_strChange || strDescription.Substring(0, 1) == "*") && (strDescription.Substring(1, 1) == m_strType || strDescription.Substring(1, 1) == "*") && (strDescription.Substring(2, 1) == m_strEdition || strDescription.Substring(2, 1) == "*") && ((strDescription.Substring(3, 1) == "Y" ? true : false) == m_blnWatch || strDescription.Substring(3, 1) == "*") && ((strDescription.Substring(4, 1) == "Y" ? true : false) == m_blnConsign || strDescription.Substring(4, 1) == "*")))//strDescription.IndexOf(cmb_exchange1.Text) != -1&& 
                    {
                        PrintSet.TempletPrints.Add(printSet.TempletPrints[i]);
                    }
                }
            }
            m_printSet = new PrintSet();
            m_printSet.Description = PrintSet.Description;
            m_printSet.DisplayOrder = PrintSet.DisplayOrder;
            m_printSet.EnableRemove = PrintSet.EnableRemove;
            m_printSet.EnableUpdate = PrintSet.EnableUpdate;
            m_printSet.Id = PrintSet.Id;
            m_printSet.IsActive = PrintSet.IsActive;
            m_printSet.Name = PrintSet.Name;
            m_printSet.ReplicationVersion = PrintSet.ReplicationVersion;
            m_printSet.TempletPrints = new List<TempletPrint>();
            foreach (TempletPrint temp in PrintSet.TempletPrints)
            {
                m_printSet.TempletPrints.Add(temp);
            }


            RemovePrint(ref PrintSet, "审批表城区");
            RemovePrint(ref PrintSet, "审批表招标办");
            RemovePrint(ref PrintSet, "审批表镇区");
            RemovePrint(ref PrintSet, "报纸公告");
            RemovePrint(ref PrintSet, "标书公告");
            RemovePrint(ref PrintSet, "成交确认书");
            RemovePrint(ref PrintSet, "11.合同");
            RemovePrint(ref PrintSet, "资料收件收据");
            RemovePrint(ref PrintSet, "须知");
            RemovePrint(ref PrintSet, "现场竞买规则");
            RemovePrint(ref PrintSet, "公开交易证明书");
            if (tab交件信息.PageVisible == true && (tab地块信息.PageVisible == true || tab镇区审核.PageVisible == true) && tab招标办审核.PageVisible == false)
            {
                RemovePrint(ref PrintSet, "04");
                RemovePrint(ref PrintSet, "05");
                RemovePrint(ref PrintSet, "07");
                RemovePrint(ref PrintSet, "09");
                RemovePrint(ref PrintSet, "12");
                RemovePrint(ref PrintSet, "15");
                RemovePrint(ref PrintSet, "22");
                RemovePrint(ref PrintSet, "25");
                RemovePrint(ref PrintSet, "29");
                RemovePrint(ref PrintSet, "31");
                RemovePrint(ref printSet, "32");
            }

            return PrintSet;
        }

        private bool CreateReportDataFile(string strFilePath, string strReportName, string strDescription)
        {
            string strProjectId;
            string strPROINST_ID;
            DataSet dtsYW_tdzbpm;
            string strXmlData = "";

            strProjectId = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProjectId, "");
            strPROINST_ID = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProinstId, "");

            string strYW_tdzbpm_td = @"SELECT dkid AS Expr1, PROJECT_ID AS Expr2, ISNULL(地块编号, N'') AS 地块编号, 
                                      ISNULL(权属单位, N'') AS 权属单位, ISNULL(土地位置坐落, N'') AS 土地位置坐落, 
                                      ISNULL(建设用地批文号, N'') AS 建设用地批文号, ISNULL(用地现状, N'') 
                                      AS 用地现状, ISNULL(土地使用证编号, N'') AS 土地使用证编号, 
                                      CASE WHEN YW_tdzbpm_td.原产权单位提供的底价 IS NOT NULL 
                                      THEN CONVERT(varchar(20), YW_tdzbpm_td.原产权单位提供的底价) 
                                      ELSE '' END AS 原产权单位提供的底价, ISNULL(原土地用途, N'') AS 原土地用途, 
                                      ISNULL(原土地性质, N'') AS 原土地性质, 
                                      CASE WHEN YW_tdzbpm_td.用地面积 IS NOT NULL THEN CONVERT(varchar(20), 
                                      YW_tdzbpm_td.用地面积) ELSE '' END AS 用地面积, ISNULL(图纸编号, N'') 
                                      AS 图纸编号, ISNULL(勘察情况, N'') AS 勘察情况, ISNULL(税费完善情况, N'') 
                                      AS 税费完善情况, ISNULL(权属争议, N'') AS 权属争议, ISNULL(清拆补偿负责人, N'') 
                                      AS 清拆补偿负责人, 
                                      CASE WHEN YW_tdzbpm_td.保证金冲减地价款期数 IS NOT NULL 
                                      THEN CONVERT(varchar(20), YW_tdzbpm_td.保证金冲减地价款期数) 
                                      ELSE '' END AS 保证金冲减地价款期数, ISNULL(过户税费, N'') AS 过户税费, 
                                      ISNULL(查封情况, N'') AS 查封情况, ISNULL(抵押情况, N'') AS 抵押情况, 
                                      ISNULL(是否有保留价, N'') AS 是否有保留价, 宗地图, 万分一图, 现状图, 规划图, 
                                      地价图,'wordml://'+PROJECT_ID+'Z.jpg' as 宗地图号,'wordml://'+PROJECT_ID+'W.jpg' as  万分一图号,'wordml://'+PROJECT_ID+'X.jpg' as  现状图号,'wordml://'+PROJECT_ID+'G.jpg' as  规划图号, 
                                      'wordml://'+PROJECT_ID+'D.jpg' as  地价图号, 图纸编号
                                      FROM dbo.YW_tdzbpm_td where YW_tdzbpm_td.PROJECT_ID = '" + strProjectId + @"'
                                      FOR XML AUTO,ELEMENTS,BINARY BASE64";


            string strMATER = @"SELECT PROINSTMATER_ID, isnull(PROINSTMATER_NAME,'') as PROINSTMATER_NAME, PROINST_ID, isnull(OLD_NUM,'') as OLD_NUM, 
                                isnull(DUPL_NUM,'') as DUPL_NUM, isnull(PROINSTMATER_MEM,'') as PROINSTMATER_MEM, REPLICATION_VERSION
                                FROM WF_PROINSTMATER where  PROINST_ID = '" + strPROINST_ID + @"' and SELECTED = 1 
                                FOR XML AUTO,ELEMENTS";
            dtsYW_tdzbpm = SkyMap.Net.DAO.QueryHelper.ExecuteSqls("Default", string.Empty, new string[] { strYW_tdzbpm_td, strMATER }, new string[] { "YW_tdzbpm_td", "WF_PROINSTMATER" });

            IDA0 dao = DAOFactory.GetInstanceByNameSpace("SkyMap.Net.DAO");
            IDbConnection dbConn = dao.Connection;
            IDbCommand dbCmd;
            try
            {
                dbCmd = dbConn.CreateCommand();
                dbCmd.CommandType = CommandType.StoredProcedure;
                dbCmd.CommandText = "ProcTdzbpmxml";

                dbCmd.Parameters.Add(new SqlParameter("@PROJECT_ID", strProjectId));

                using (SqlDataReader dataReader = (SqlDataReader)dbCmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        strXmlData += dataReader[0].ToString();
                    }
                }
            }
            catch (SqlException e)
            {
                MessageHelper.ShowError("出错", e);
            }
            strXmlData = strXmlData.Replace("dbo.", "");
            strXmlData = strXmlData.Replace("xmlns=\"\"", "");


            XmlDocument doc = new XmlDocument();

            strXmlData = "<Root xmlns=\"http://www.skymapsoft.com\">" + strXmlData + "</Root>";
            doc.LoadXml(strXmlData);

            strXmlData = "";
            foreach (DataRow dr in dtsYW_tdzbpm.Tables["YW_tdzbpm_td"].Rows)
            {
                strXmlData += dr[0].ToString();
            }

            strXmlData = strXmlData.Replace("dbo.", "");
            strXmlData = strXmlData.Replace("xmlns=\"\"", "");
            XmlDocument docYW_tdzbpm_td = new XmlDocument();
            strXmlData = "<Root xmlns=\"http://www.skymapsoft.com\">" + strXmlData + "</Root>";
            docYW_tdzbpm_td.LoadXml(strXmlData);

            XmlNode node;
            foreach (XmlNode nodeTD in docYW_tdzbpm_td.FirstChild.ChildNodes)
            {
                node = doc.ImportNode(nodeTD, true);
                doc.FirstChild.FirstChild.AppendChild(node);
            }

            strXmlData = "";

            dbCmd = dbConn.CreateCommand();
            dbCmd.CommandType = CommandType.StoredProcedure;
            dbCmd.CommandText = "ProcTdzbpmbxml";
            dbCmd.Parameters.Add(new SqlParameter("@bid", SqlDbType.Int));


            if (strDescription.EndsWith("B"))// || strReportName.IndexOf("审批表二") != -1)
            {
                if (typeof(DevExpress.XtraGrid.Views.BandedGrid.BandedGridView) != gridB1.FocusedView.GetType())
                {
                    MessageHelper.ShowInfo("请选择标的");
                    return false;
                }
                DataRow dr = ((DevExpress.XtraGrid.Views.BandedGrid.BandedGridView)gridB1.FocusedView).GetDataRow(((DevExpress.XtraGrid.Views.BandedGrid.BandedGridView)gridB1.FocusedView).FocusedRowHandle);
                if (dr == null)
                {
                    MessageHelper.ShowInfo("请选择标的");
                    return false;
                }
                try
                {
                    ((SqlParameter)dbCmd.Parameters["@bid"]).Value = dr["Bid"].ToString();
                    using (SqlDataReader dataReader = (SqlDataReader)dbCmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            strXmlData += dataReader[0].ToString();
                        }
                    }
                }
                catch (SqlException e)
                {
                    MessageHelper.ShowError("出错", e);
                }
            }
            else
            {
                foreach (DataRow dr in m_dstAll.Tables["YW_tdzbpm_b"].Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        try
                        {
                            ((SqlParameter)dbCmd.Parameters["@bid"]).Value = dr["Bid"].ToString();
                            using (SqlDataReader dataReader = (SqlDataReader)dbCmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    strXmlData += dataReader[0].ToString();
                                }
                            }
                        }
                        catch (SqlException e)
                        {
                            LoggingService.Debug("招标拍卖出错报表"+e.Message);
                            MessageHelper.ShowError("出错", e);
                        }
                    }
                }
            }
            strXmlData = strXmlData.Replace("dbo.", "");
            strXmlData = strXmlData.Replace("xmlns=\"\"", "");
            XmlDocument docYW_tdzbpm_b = new XmlDocument();
            strXmlData = "<Root xmlns=\"http://www.skymapsoft.com\">" + strXmlData + "</Root>";
            docYW_tdzbpm_b.LoadXml(strXmlData);
            foreach (XmlNode nodeB in docYW_tdzbpm_b.FirstChild.ChildNodes)
            {
                node = doc.ImportNode(nodeB, true);
                doc.FirstChild.FirstChild.AppendChild(node);
            }

            strXmlData = "";
            foreach (DataRow dr in dtsYW_tdzbpm.Tables["WF_PROINSTMATER"].Rows)
            {
                strXmlData += dr[0].ToString();
            }
            strXmlData = strXmlData.Replace("dbo.", "");
            strXmlData = strXmlData.Replace("xmlns=\"\"", "");
            XmlDocument docWF_PROINSTMATER = new XmlDocument();
            strXmlData = "<Root xmlns=\"http://www.skymapsoft.com\">" + strXmlData + "</Root>";
            docWF_PROINSTMATER.LoadXml(strXmlData);
            foreach (XmlNode nodeB in docWF_PROINSTMATER.FirstChild.ChildNodes)
            {
                node = doc.ImportNode(nodeB, true);
                doc.FirstChild.FirstChild.AppendChild(node);
            }
            doc.Save(strFilePath);
            return true;
        }

        protected override bool PrintBySelf(TempletPrint templetPrint)
        {
            AwokeUpdateReport();
            if (templetPrint.Type.EndsWith("xslt"))
            {



                string strProjectId = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProjectId, "");
                string inputfile = string.Format("{0}.xml", Path.GetTempFileName());
                try
                {
                    CreateReportDataFile(inputfile, templetPrint.Name, templetPrint.Description);

                    File.Copy(inputfile, Application.StartupPath + "\\data.xml", true);
                }
                catch (Exception ex)
                {
                    MessageHelper.ShowError("生成报表数据有误！", ex);
                }
                DataTable dtbReport = SkyMap.Net.DAO.QueryHelper.ExecuteSql("Default", string.Empty, @"SELECT ReportID, PROJECT_ID, TEMPLETPRINT_ID, ReportName, ReportData
                                        FROM YW_tdzbpm_Report where PROJECT_ID = '" + strProjectId + "' and TEMPLETPRINT_ID = '" + templetPrint.Id + "'");

                dtbReport.ExtendedProperties.Add("selectsql", @"SELECT ReportID, PROJECT_ID, TEMPLETPRINT_ID, ReportName, ReportData
                                        FROM YW_tdzbpm_Report where PROJECT_ID = '" + strProjectId + "' and TEMPLETPRINT_ID = '" + templetPrint.Id + "'");

                string xsltFile = string.Format("{0}.xslt", Path.GetTempFileName());

                System.IO.File.WriteAllBytes(xsltFile, (byte[])dtbReport.Rows[0]["ReportData"]);


                object fileName = System.IO.Path.GetTempFileName();
                object readOnly = false;
                object isVisible = true;
                object missing = System.Reflection.Missing.Value;
                //Create the XslTransform object.
                XslTransform xslt = new XslTransform();

                //Load the stylesheet.
                xslt.Load(xsltFile);

                if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + Path.DirectorySeparatorChar + m_strYwNo + "标书文件"))
                {
                    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + Path.DirectorySeparatorChar + m_strYwNo + "标书文件");
                }
                object outputfile = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + Path.DirectorySeparatorChar + m_strYwNo + "标书文件" + Path.DirectorySeparatorChar + m_strYwNo + "-" + templetPrint.Name;
                outputfile = string.Format("{0}.xml", outputfile);
                try
                {
                    xslt.Transform(inputfile, outputfile as string);
                }
                catch (Exception ex)
                {
                    MessageHelper.ShowInfo("该报表已经打开了！");
                }


                System.Diagnostics.Process process = new Process();
                process.StartInfo.FileName = outputfile as string;
                process.Start();
                process.WaitForExit();
                if (dtbReport.Rows.Count > 0 && dtbReport.Rows[0]["ReportName"].ToString() != "地块图片")
                {
                    if (MessageHelper.ShowYesNoInfo("模板已经修改修要保存吗？") == DialogResult.Yes)
                    {
                        try
                        {
                            System.Diagnostics.Process process1 = new Process();
                            process1.StartInfo.CreateNoWindow = true;
                            process1.StartInfo.UseShellExecute = false;
                            process1.StartInfo.FileName = Application.StartupPath + "\\WML2XSLT";
                            process1.StartInfo.Arguments = "\"" + outputfile + "\" -o \"" + xsltFile + "\" -ns http://www.skymapsoft.com";
                            process1.Start();
                            process1.WaitForExit();

                            byte[] bytefile;
                            FileStream file = File.Open(xsltFile, FileMode.Open);
                            bytefile = new byte[(int)file.Length];
                            file.Read(bytefile, 0, (int)file.Length);
                            dtbReport.Rows[0].BeginEdit();
                            dtbReport.Rows[0]["ReportData"] = bytefile;
                            dtbReport.Rows[0].EndEdit();
                            SkyMap.Net.DataForms.DataEngine.SQLDataEngine sqlDataEngine = new SkyMap.Net.DataForms.DataEngine.SQLDataEngine();
                            sqlDataEngine.SaveData(this.dataFormController.DAODataForm.DataSource, dtbReport);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
            return base.PrintBySelf(templetPrint);
        }

        private void RemovePrint(ref PrintSet p_printSet, string strTemp)
        {
            for (int i = 0; i < p_printSet.TempletPrints.Count; i++)
            {
                TempletPrint temp = p_printSet.TempletPrints[i] as SkyMap.Net.DataForms.TempletPrint;
                if (temp.Name.IndexOf(strTemp) != -1)
                {
                    p_printSet.TempletPrints.RemoveAt(i);
                    break;
                }
            }
        }

        #endregion       

        #region 通过业务生成业务编号
        private void cmb_type_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string strProjectId = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProjectId, "");
                if (string.IsNullOrEmpty(txt宗地地址.Text))
                {

                    txt宗地地址.Text = "中山市" + cmb_ZSTOWNSHIP.Text;
                }
                if (cmb_exchange.EditValue != null && cmb_ZSTOWNSHIP.EditValue != null && cmb_type.EditValue != null && cmb_exchange.EditValue.ToString() != "" && cmb_ZSTOWNSHIP.EditValue.ToString() != "" && cmb_type.EditValue.ToString() != "" && strProjectId != "")
                {
                    //处理旧数据的修改
                    string strDate = Convert.ToDateTime(this.DataFormConntroller.DataSource.Tables["YW_tdzbpm"].Rows[0]["当前服务器时间"]).Year.ToString().Substring(2, 2);
                    //string strDate = DateTime.Now.Year.ToString().Substring(2, 2);
                    //if ((string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProinstId, "") == "old")
                    //{

                    //    strDate = Convert.ToDateTime(this.DataFormConntroller.DataSource.Tables["YW_tdzbpm"].Rows[0]["公告起始日期"]).Year.ToString().Substring(2, 2);
                    //}
                    m_strYwNo = cmb_exchange.EditValue.ToString() + cmb_ZSTOWNSHIP.EditValue.ToString() + "-" + strDate + "-" + cmb_type.EditValue.ToString() + strProjectId.Substring(strProjectId.Length - 3, 3);
                    txt业务编号.Text = m_strYwNo;
                    txt业务编号1.Text = m_strYwNo;
                    m_strType = cmb_type.EditValue.ToString();
                    m_strChange = cmb_exchange.EditValue.ToString();
                }

                this.cmb_exchange1.EditValueChanged -= new System.EventHandler(this.cmb_type1_EditValueChanged);
                this.cmb_type1.EditValueChanged -= new System.EventHandler(this.cmb_type1_EditValueChanged);

                cmb_type1.EditValue = cmb_type.EditValue;
                cmb_exchange1.EditValue = cmb_exchange.EditValue;

                this.cmb_exchange1.EditValueChanged += new System.EventHandler(this.cmb_type1_EditValueChanged);
                this.cmb_type1.EditValueChanged += new System.EventHandler(this.cmb_type1_EditValueChanged);

                ChangeByType();
            }
            catch (Exception ex)
            {
                LoggingService.Debug(ex.Message);
            }

            //OnChanged(sender, null);
        }

        private void cmb_type1_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string strProjectId = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProjectId, "");
                //string m_strYwNo = "";
                this.cmb_exchange.EditValueChanged -= new System.EventHandler(this.cmb_type_EditValueChanged);
                this.cmb_type.EditValueChanged -= new System.EventHandler(this.cmb_type_EditValueChanged);
                cmb_type.EditValue = cmb_type1.EditValue;
                cmb_exchange.EditValue = cmb_exchange1.EditValue;
                if (cmb_exchange.EditValue != null && cmb_ZSTOWNSHIP.EditValue != null && cmb_type.EditValue != null && cmb_exchange.EditValue.ToString() != "" && cmb_ZSTOWNSHIP.EditValue.ToString() != "" && cmb_type.EditValue.ToString() != "" && strProjectId != "")
                {
                    string strDate = Convert.ToDateTime(this.DataFormConntroller.DataSource.Tables["YW_tdzbpm"].Rows[0]["当前服务器时间"]).Year.ToString().Substring(2, 2);
                    //string strDate = DateTime.Now.Year.ToString().Substring(2, 2);
                    //if ((string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProinstId, "") == "old")
                    //{
                    //    strDate = Convert.ToDateTime(this.DataFormConntroller.DataSource.Tables["YW_tdzbpm"].Rows[0]["公告起始日期"]).Year.ToString().Substring(2, 2);
                    //}
                    m_strYwNo = cmb_exchange.EditValue.ToString() + cmb_ZSTOWNSHIP.EditValue.ToString() + "-" + strDate + "-" + cmb_type.EditValue.ToString() + strProjectId.Substring(strProjectId.Length - 3, 3);
                    txt业务编号.Text = m_strYwNo;
                    txt业务编号1.Text = m_strYwNo;
                    m_strType = cmb_type.EditValue.ToString();
                    m_strChange = cmb_exchange.EditValue.ToString();
                }
                this.cmb_exchange.EditValueChanged += new System.EventHandler(this.cmb_type_EditValueChanged);
                this.cmb_type.EditValueChanged += new System.EventHandler(this.cmb_type_EditValueChanged);
                ChangeByType();
                //OnChanged(sender, null);

            }
            catch (Exception ex)
            {
                LoggingService.Debug(ex.Message);
            }

        }

        private void txt业务编号_EditValueChanged(object sender, EventArgs e)
        {
            if ((sender as DevExpress.XtraEditors.TextEdit).EditValue != "")
            {
                gridB.UseEmbeddedNavigator = true;
                gridB1.UseEmbeddedNavigator = true;

                contextMenuStrip1.Enabled = true;
                contextMenuStrip2.Enabled = true;
            }
            else
            {
                gridB.UseEmbeddedNavigator = false;
                gridB1.UseEmbeddedNavigator = false;

                contextMenuStrip1.Enabled = false;
                contextMenuStrip2.Enabled = false;
            }
        }

        private void txt业务编号_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            this.gridB.FocusedView.PostEditor();
            this.gridB.FocusedView.UpdateCurrentRow();
            if (e.OldValue == null || e.OldValue == "" || e.OldValue.ToString().Length == 0 || e.NewValue == null || e.NewValue == "" || e.NewValue.ToString().Length == 0)
            {
                return;
            }
            string strNo;
            m_dtbDK = m_dstAll.Tables["YW_tdzbpm_B"];

            foreach (DataRow dr in m_dtbDK.Rows)
            {
                strNo = dr["宗地编号标"].ToString();
                strNo = strNo.Replace(e.OldValue.ToString(), e.NewValue.ToString());
                dr.BeginEdit();
                dr["宗地编号标"] = strNo;
                dr.EndEdit();
            }
        }
        /// <summary>
        /// 交易方式和业务类别改变时重新调整界面
        /// </summary>
        private void ChangeByType()
        {
            string strBandTitle = "根据{0}及中府【2006】85号文粤国土资发【2005】156号文确定的工业用地要求";

            //========================
            if (m_dstAll != null)
            {

                //-------------------------------------------
                if (this.tblData.SelectedTabPage.Text.ToString() == "招标办审核")
                {

                    DataRelation dtr;
                    if (this.xtraTabControl1.SelectedTabPage.Text.ToString() == "地块审批信息")
                    {
                        if (m_dstAll.Relations.IndexOf("Bphdj") != -1)
                        {
                            m_dstAll.Relations.Remove("Bphdj");
                        }
                        if (m_dstAll.Relations.IndexOf("Bxcbj") != -1)
                        {
                            m_dstAll.Relations.Remove("Bxcbj");
                        }

                        if (m_dstAll.Relations.IndexOf("Btd") == -1)
                        {
                            dtr = new DataRelation("Btd", m_dstAll.Tables["YW_tdzbpm_b"].Columns["Bid"], m_dstAll.Tables["YW_tdzbpm_td"].Columns["标"], false);
                            m_dstAll.Relations.Add(dtr);
                        }
                        if (m_dstAll.Relations.IndexOf("Bdjksq") == -1)
                        {
                            //地价款
                            dtr = new DataRelation("Bdjksq", m_dstAll.Tables["YW_tdzbpm_b"].Columns["Bid"], m_dstAll.Tables["YW_tdzbpm_djksq"].Columns["B_id"], false);
                            m_dstAll.Relations.Add(dtr);
                        }

                        if (m_dstAll.Relations.IndexOf("Bgpbj") != -1)
                        {
                            m_dstAll.Relations.Remove("Bgpbj");
                        }
                    }
                    else if (this.xtraTabControl1.SelectedTabPage.Text.ToString() == "公告")
                    {
                        if (m_dstAll.Relations.IndexOf("Btd") != -1)
                        {
                            m_dstAll.Relations.Remove("Btd");
                        }
                        if (m_dstAll.Relations.IndexOf("Bdjksq") != -1)
                        {
                            m_dstAll.Relations.Remove("Bdjksq");
                        }
                        if (m_dstAll.Relations.IndexOf("Bphdj") == -1)
                        {
                            //领取竞投牌号登记信息表
                            m_dstAll.Relations.Add(new DataRelation("Bphdj", m_dstAll.Tables["YW_tdzbpm_b"].Columns["Bid"], m_dstAll.Tables["YW_tdzbpm_phdj"].Columns["B_id"], false));
                        }
                        if (m_dstAll.Relations.IndexOf("Bxcbj") == -1)
                        {
                            dtr = new DataRelation("Bxcbj", m_dstAll.Tables["YW_tdzbpm_b"].Columns["Bid"], m_dstAll.Tables["YW_tdzbpm_xcbj"].Columns["B_id"], false);
                            m_dstAll.Relations.Add(dtr);
                        }

                        if (m_dstAll.Relations.IndexOf("Bgpbj") == -1)
                        {
                            string staffid = SkyMap.Net.Security.SecurityUtil.GetSmIdentity().UserId;
                            if (!string.IsNullOrEmpty(this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PActinstId, "").ToString()))
                            {

                                SkyMap.Net.Workflow.Instance.Proinst proinst = SkyMap.Net.Workflow.Client.Services.WorkflowService.WfcInstance.GetProinst((string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProinstId, ""));
                                if (proinst.Status != SkyMap.Net.Workflow.Instance.WfStatusType.WF_COMPLETED)
                                {
                                    foreach (SkyMap.Net.Workflow.Instance.Actinst actinst in proinst.Actinsts)
                                    {
                                        if (actinst.Status == SkyMap.Net.Workflow.Instance.WfStatusType.WF_RUNNING)
                                        {
                                            foreach (SkyMap.Net.Workflow.Instance.WfAssigninst assign in actinst.Assigns)
                                            {
                                                if (assign.StaffId == staffid)
                                                {
                                                    m_dstAll.Relations.Add(new DataRelation("Bgpbj", m_dstAll.Tables["YW_tdzbpm_b"].Columns["Bid"], m_dstAll.Tables["YW_tdzbpm_gpbj"].Columns["B_id"], false));

                                                }
                                            }
                                        }
                                    }
                                }
                                RoleSecurityCommand role = new RoleSecurityCommand();
                                if (role.IsEnabled)
                                {
                                    if (m_dstAll.Relations.IndexOf("Bgpbj") == -1)
                                    {
                                        m_dstAll.Relations.Add(new DataRelation("Bgpbj", m_dstAll.Tables["YW_tdzbpm_b"].Columns["Bid"], m_dstAll.Tables["YW_tdzbpm_gpbj"].Columns["B_id"], false));

                                    }
                                }
                            }

                        }
                    }
                    else if (this.xtraTabControl1.SelectedTabPage.Text.ToString() == "报批")
                    {
                        if (m_dstAll.Relations.IndexOf("Btd") == -1)
                        {
                            //地块
                            dtr = new DataRelation("Btd", m_dstAll.Tables["YW_tdzbpm_b"].Columns["Bid"], m_dstAll.Tables["YW_tdzbpm_td"].Columns["标"], false);
                            m_dstAll.Relations.Add(dtr);
                        }
                        if (m_dstAll.Relations.IndexOf("Bdjksq") == -1)
                        {
                            //地价款
                            dtr = new DataRelation("Bdjksq", m_dstAll.Tables["YW_tdzbpm_b"].Columns["Bid"], m_dstAll.Tables["YW_tdzbpm_djksq"].Columns["B_id"], false);
                            m_dstAll.Relations.Add(dtr);
                        }
                        if (m_dstAll.Relations.IndexOf("Bphdj") != -1)
                        {
                            m_dstAll.Relations.Remove("Bphdj");
                        }
                        if (m_dstAll.Relations.IndexOf("Bxcbj") != -1)
                        {
                            m_dstAll.Relations.Remove("Bxcbj");
                        }

                        if (m_dstAll.Relations.IndexOf("Bgpbj") != -1)
                        {
                            m_dstAll.Relations.Remove("Bgpbj");
                        }
                    }
                    else if (this.xtraTabControl1.SelectedTabPage.Text.ToString() == "成交信息")
                    {
                        if (m_dstAll.Relations.IndexOf("Btd") != -1)
                        {
                            //地块
                            m_dstAll.Relations.Remove("Btd");
                        }
                        if (m_dstAll.Relations.IndexOf("Bdjksq") == -1)
                        {
                            //地价款
                            dtr = new DataRelation("Bdjksq", m_dstAll.Tables["YW_tdzbpm_b"].Columns["Bid"], m_dstAll.Tables["YW_tdzbpm_djksq"].Columns["B_id"], false);
                            m_dstAll.Relations.Add(dtr);
                        }
                        if (m_dstAll.Relations.IndexOf("Bphdj") == -1)
                        {
                            //领取竞投牌号登记信息表
                            m_dstAll.Relations.Add(new DataRelation("Bphdj", m_dstAll.Tables["YW_tdzbpm_b"].Columns["Bid"], m_dstAll.Tables["YW_tdzbpm_phdj"].Columns["B_id"], false));
                        }
                        if (m_dstAll.Relations.IndexOf("Bxcbj") == -1)
                        {
                            dtr = new DataRelation("Bxcbj", m_dstAll.Tables["YW_tdzbpm_b"].Columns["Bid"], m_dstAll.Tables["YW_tdzbpm_xcbj"].Columns["B_id"], false);
                            m_dstAll.Relations.Add(dtr);
                        }

                        if (m_dstAll.Relations.IndexOf("Bgpbj") == -1)
                        {
                            string staffid = SkyMap.Net.Security.SecurityUtil.GetSmIdentity().UserId;
                            if (!string.IsNullOrEmpty(this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PActinstId, "").ToString()))
                            {

                                SkyMap.Net.Workflow.Instance.Proinst proinst = SkyMap.Net.Workflow.Client.Services.WorkflowService.WfcInstance.GetProinst((string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProinstId, ""));
                                if (proinst.Status != SkyMap.Net.Workflow.Instance.WfStatusType.WF_COMPLETED)
                                {
                                    foreach (SkyMap.Net.Workflow.Instance.Actinst actinst in proinst.Actinsts)
                                    {
                                        if (actinst.Status == SkyMap.Net.Workflow.Instance.WfStatusType.WF_RUNNING)
                                        {
                                            foreach (SkyMap.Net.Workflow.Instance.WfAssigninst assign in actinst.Assigns)
                                            {
                                                if (assign.StaffId == staffid)
                                                {
                                                    m_dstAll.Relations.Add(new DataRelation("Bgpbj", m_dstAll.Tables["YW_tdzbpm_b"].Columns["Bid"], m_dstAll.Tables["YW_tdzbpm_gpbj"].Columns["B_id"], false));

                                                }
                                            }
                                        }
                                    }
                                }
                                RoleSecurityCommand role = new RoleSecurityCommand();
                                if (role.IsEnabled)
                                {
                                    if (m_dstAll.Relations.IndexOf("Bgpbj") == -1)
                                    {
                                        m_dstAll.Relations.Add(new DataRelation("Bgpbj", m_dstAll.Tables["YW_tdzbpm_b"].Columns["Bid"], m_dstAll.Tables["YW_tdzbpm_gpbj"].Columns["B_id"], false));

                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (m_dstAll.Relations.IndexOf("Btd") == -1)
                        {
                            //地块
                            dtr = new DataRelation("Btd", m_dstAll.Tables["YW_tdzbpm_b"].Columns["Bid"], m_dstAll.Tables["YW_tdzbpm_td"].Columns["标"], false);
                            m_dstAll.Relations.Add(dtr);
                        }
                        if (m_dstAll.Relations.IndexOf("Bdjksq") == -1)
                        {
                            //地价款
                            dtr = new DataRelation("Bdjksq", m_dstAll.Tables["YW_tdzbpm_b"].Columns["Bid"], m_dstAll.Tables["YW_tdzbpm_djksq"].Columns["B_id"], false);
                            m_dstAll.Relations.Add(dtr);
                        }
                        if (m_dstAll.Relations.IndexOf("Bphdj") == -1)
                        {
                            //领取竞投牌号登记信息表
                            m_dstAll.Relations.Add(new DataRelation("Bphdj", m_dstAll.Tables["YW_tdzbpm_b"].Columns["Bid"], m_dstAll.Tables["YW_tdzbpm_phdj"].Columns["B_id"], false));
                        }
                        if (m_dstAll.Relations.IndexOf("Bxcbj") == -1)
                        {
                            dtr = new DataRelation("Bxcbj", m_dstAll.Tables["YW_tdzbpm_b"].Columns["Bid"], m_dstAll.Tables["YW_tdzbpm_xcbj"].Columns["B_id"], false);
                            m_dstAll.Relations.Add(dtr);
                        }

                        if (m_dstAll.Relations.IndexOf("Bgpbj") == -1)
                        {
                            string staffid = SkyMap.Net.Security.SecurityUtil.GetSmIdentity().UserId;
                            if (!string.IsNullOrEmpty(this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PActinstId, "").ToString()))
                            {

                                SkyMap.Net.Workflow.Instance.Proinst proinst = SkyMap.Net.Workflow.Client.Services.WorkflowService.WfcInstance.GetProinst((string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProinstId, ""));
                                if (proinst.Status != SkyMap.Net.Workflow.Instance.WfStatusType.WF_COMPLETED)
                                {
                                    foreach (SkyMap.Net.Workflow.Instance.Actinst actinst in proinst.Actinsts)
                                    {
                                        if (actinst.Status == SkyMap.Net.Workflow.Instance.WfStatusType.WF_RUNNING)
                                        {
                                            foreach (SkyMap.Net.Workflow.Instance.WfAssigninst assign in actinst.Assigns)
                                            {
                                                if (assign.StaffId == staffid)
                                                {
                                                    m_dstAll.Relations.Add(new DataRelation("Bgpbj", m_dstAll.Tables["YW_tdzbpm_b"].Columns["Bid"], m_dstAll.Tables["YW_tdzbpm_gpbj"].Columns["B_id"], false));

                                                }
                                            }
                                        }
                                    }
                                }
                                RoleSecurityCommand role = new RoleSecurityCommand();
                                if (role.IsEnabled)
                                {
                                    if (m_dstAll.Relations.IndexOf("Bgpbj") == -1)
                                    {
                                        m_dstAll.Relations.Add(new DataRelation("Bgpbj", m_dstAll.Tables["YW_tdzbpm_b"].Columns["Bid"], m_dstAll.Tables["YW_tdzbpm_gpbj"].Columns["B_id"], false));

                                    }
                                }
                            }

                        }
                    }




                }

                #region  镇区地块信息
                if (tblData.SelectedTabPage.Text.ToString() == "镇区地块信息")
                {
                    DataRelation dtr;
                    if (m_dstAll.Relations.IndexOf("Btd") == -1)
                    {
                        //地块
                        dtr = new DataRelation("Btd", m_dstAll.Tables["YW_tdzbpm_b"].Columns["Bid"], m_dstAll.Tables["YW_tdzbpm_td"].Columns["标"], false);
                        m_dstAll.Relations.Add(dtr);
                    }


                    if (m_dstAll.Relations.IndexOf("Bdjksq") != -1)
                    {
                        //地价款
                        m_dstAll.Relations.Remove("Bdjksq");
                    }

                    if (m_dstAll.Relations.IndexOf("Bphdj") != -1)
                    {
                        //领取竞投牌号登记信息表
                        m_dstAll.Relations.Remove("Bphdj");
                    }
                    if (m_dstAll.Relations.IndexOf("Bxcbj") != -1)
                    {
                        m_dstAll.Relations.Remove("Bxcbj");
                    }

                }
                #endregion

                //------------------------------
            }

            switch (m_strChange)
            {
                case "P":
                    //cmb_公告天数.Text = "30";
                    m_gbx挂牌报价时间.Visible = false;
                    if (m_dstAll != null)
                    {
                        if (m_dstAll.Relations.IndexOf("Bgpbj") != -1)
                        {
                            m_dstAll.Relations.Remove("Bgpbj");
                        }

                    }

                    break;
                case "G":
                    //cmb_公告天数.Text = "15";
                    m_gbx挂牌报价时间.Visible = true;

                    break;
                case "W":
                    if (m_dstAll != null)
                    {
                        if (m_dstAll.Relations.IndexOf("Bgpbj") != -1)
                        {
                            m_dstAll.Relations.Remove("Bgpbj");
                        }
                    }
                    break;
                case "Z":
                    if (m_dstAll != null)
                    {
                        if (m_dstAll.Relations.IndexOf("Bgpbj") != -1)
                        {
                            m_dstAll.Relations.Remove("Bgpbj");
                        }
                    }
                    break;
                default:
                    break;
            }
            //====================
            switch (m_strType)
            {
                case "0":
                    col年限.Visible = false;
                    col出让年限.Visible = true;
                    col年限1.Visible = false;
                    col出让年限1.Visible = true;

                    col年限3.Caption = "出让年限";

                    if (txt_地价款开户行地址.Text.Trim() == "")
                    {
                        txt_地价款开户行地址.TextChanged -= new EventHandler(DataControlTextChanged);
                        txt_地价款开户行地址.Text = "中山市兴中道2号";
                        txt_地价款开户行地址.TextChanged += new EventHandler(DataControlTextChanged);
                    }
                    if (txt_地价款开户银行.Text.Trim() == "")
                    {
                        txt_地价款开户银行.TextChanged -= new EventHandler(DataControlTextChanged);
                        txt_地价款开户银行.Text = "建设银行中山兴中道支行";
                        txt_地价款开户银行.TextChanged += new EventHandler(DataControlTextChanged);
                    }
                    if (txt_地价款人民币帐号.Text.Trim() == "")
                    {
                        txt_地价款人民币帐号.TextChanged -= new EventHandler(DataControlTextChanged);
                        txt_地价款人民币帐号.Text = "44001782301053001340";
                        txt_地价款人民币帐号.TextChanged += new EventHandler(DataControlTextChanged);
                    }
                    if (txt_地价款收款单位.Text.Trim() == "")
                    {
                        txt_地价款收款单位.TextChanged -= new EventHandler(DataControlTextChanged);
                        txt_地价款收款单位.Text = "中山市财政局";
                        txt_地价款收款单位.TextChanged += new EventHandler(DataControlTextChanged);
                    }
                    col评估地价.Visible = true;
                    col评估地价1.Visible = true;
                    //col评估地价2.Visible = true;
                    //col评估地价3.Visible = true;
                    col评估地价4.Visible = true;
                    col土地来源.Visible = true;
                    col土地来源1.Visible = true;
                    // cmb_公告天数.Text = "30";
                    this.col出让金.Visible = true;
                    col耕地占用税.Visible = true;
                    gridBand9.Caption = string.Format(strBandTitle, "申请文件");
                    gridBand19.Caption = string.Format(strBandTitle, "申请文件");
                    gridBand35.Caption = string.Format(strBandTitle, "申请文件");

                    break;
                case "9":

                    //txt_地价款收款单位.TextChanged -= new EventHandler(DataControlTextChanged);
                    //txt_地价款收款单位.Text = txt权属单位.Text;
                    //txt_地价款收款单位.TextChanged += new EventHandler(DataControlTextChanged);
                    if (txt_地价款收款单位.Text.Trim() == "")
                    {
                        txt_地价款收款单位.TextChanged -= new EventHandler(DataControlTextChanged);
                        txt_地价款收款单位.Text = txt权属单位.Text;
                        txt_地价款收款单位.TextChanged += new EventHandler(DataControlTextChanged);
                    }
                    col年限.Visible = true;
                    col出让年限.Visible = false;

                    col年限1.Visible = true;
                    col出让年限1.Visible = false;

                    col年限3.Caption = "使用年限";

                    col评估地价.Visible = false;
                    col评估地价1.Visible = false;
                    col评估地价2.Visible = false;

                    col评估地价4.Visible = false;
                    this.col出让金.Visible = false;
                    col耕地占用税.Visible = false;
                    cmb_公告天数.TextChanged -= new EventHandler(DataControlTextChanged);
                    // cmb_公告天数.Text = "15";
                    cmb_公告天数.TextChanged += new EventHandler(DataControlTextChanged);

                    col土地来源.Visible = false;
                    col土地来源1.Visible = false;
                    gridBand9.Caption = string.Format(strBandTitle, "委托合同");
                    gridBand19.Caption = string.Format(strBandTitle, "委托合同");
                    gridBand35.Caption = string.Format(strBandTitle, "委托合同");
                    break;
            }
            //int i = 0;

            //for (i = viewb.RowCount; i >= 0; i--)
            //{
            //    viewb.ExpandMasterRow(i);
            //}
            //for (i = gridViewB.RowCount; i >= 0; i--)
            //{
            //    gridViewB.ExpandMasterRow(i);
            //}

            int ii = 0;

            for (ii = viewb.RowCount; ii >= 0; ii--)
            {
                viewb.CollapseMasterRow(ii);
                viewb.ExpandMasterRow(ii);
            }
            for (ii = gridViewB.RowCount; ii >= 0; ii--)
            {
                gridViewB.CollapseMasterRow(ii);
                gridViewB.ExpandMasterRow(ii);
            }
            for (ii = gridViewB1.RowCount; ii >= 0; ii--)
            {
                gridViewB1.CollapseMasterRow(ii);
                gridViewB1.ExpandMasterRow(ii);
            }
            for (ii = gridViewB2.RowCount; ii >= 0; ii--)
            {
                gridViewB2.CollapseMasterRow(ii);
                gridViewB2.ExpandMasterRow(ii);
            }
            for (ii = gridViewB3.RowCount; ii >= 0; ii--)
            {
                gridViewB3.CollapseMasterRow(ii);
                gridViewB3.ExpandMasterRow(ii);
            }
            //-------------08.06.30  转让合同不允许编号
            if (m_strType == "9")
            {
                this.bandedGridColumn122.Visible = false;
            }
            else
            {
                this.bandedGridColumn122.Visible = true;
            }
            //-------------重新绑定数据
            BBindData();
        }

        /// <summary>
        /// 竞买人信息控制
        /// </summary>
        /// 
        private void InfoControl()
        {

            string staffid = SkyMap.Net.Security.SecurityUtil.GetSmIdentity().UserId;
            //MessageBox.Show(this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PActinstId, "").ToString());
            if (string.IsNullOrEmpty(this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PActinstId, "").ToString()))
            {
                return;
            }
            if (SkyMap.Net.Workflow.Client.Services.WorkflowService.WfcInstance.GetProinst((string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProinstId, "")) == null)
            {
                return;
            }
            //SkyMap.Net.Workflow.Client.Services.WorkflowService.WfcInstance.get
            SkyMap.Net.Workflow.Instance.Proinst proinst = SkyMap.Net.Workflow.Client.Services.WorkflowService.WfcInstance.GetProinst((string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProinstId, ""));

            if (proinst.Status != SkyMap.Net.Workflow.Instance.WfStatusType.WF_COMPLETED)
            {
                foreach (SkyMap.Net.Workflow.Instance.Actinst actinst in proinst.Actinsts)
                {
                    if (actinst.Status == SkyMap.Net.Workflow.Instance.WfStatusType.WF_RUNNING)
                    {
                        foreach (SkyMap.Net.Workflow.Instance.WfAssigninst assign in actinst.Assigns)
                        {
                            if (assign.StaffId == staffid)
                            {
                                if (m_strChange.ToString().ToUpper() == "G")
                                {
                                    if (m_dstAll.Relations.IndexOf("Bgpbj") == -1)
                                    {
                                        //现场竞买报价信息表

                                        m_dstAll.Relations.Add(new DataRelation("Bgpbj", m_dstAll.Tables["YW_tdzbpm_b"].Columns["Bid"], m_dstAll.Tables["YW_tdzbpm_gpbj"].Columns["B_id"], false));
                                    }
                                }
                                //领取竞投牌号登记信息表
                                m_dstAll.Relations.Add(new DataRelation("Bphdj", m_dstAll.Tables["YW_tdzbpm_b"].Columns["Bid"], m_dstAll.Tables["YW_tdzbpm_phdj"].Columns["B_id"], false));
                            }
                        }
                    }
                }
            }
            RoleSecurityCommand role = new RoleSecurityCommand();
            if (role.IsEnabled)
            {
                if (m_strChange.ToString().ToUpper() == "G")
                {
                    if (m_dstAll.Relations.IndexOf("Bgpbj") == -1)
                    {
                        if (!m_dstAll.Relations.Contains("Bgpbj"))
                        {
                            //现场竞买报价信息表
                            m_dstAll.Relations.Add(new DataRelation("Bgpbj", m_dstAll.Tables["YW_tdzbpm_b"].Columns["Bid"], m_dstAll.Tables["YW_tdzbpm_gpbj"].Columns["B_id"], false));
                        }
                    }
                }
                if (!m_dstAll.Relations.Contains("Bphdj"))
                {
                    //领取竞投牌号登记信息表
                    m_dstAll.Relations.Add(new DataRelation("Bphdj", m_dstAll.Tables["YW_tdzbpm_b"].Columns["Bid"], m_dstAll.Tables["YW_tdzbpm_phdj"].Columns["B_id"], false));
                }
            }
        }
        void cmb_Edition_EditValueChanged(object sender, EventArgs e)
        {
            m_strEdition = cmb_Edition.EditValue.ToString();

            if (m_strEdition == "Y")
            {
                gridBand9.Visible = true;
                gridBand11.Visible = true;
                gridBand19.Visible = true;
                //gridBand27.Visible = true;
                //cbx_IsWatch.Visible = true;
                cmb_IsWatch.Visible = true;
                gridBand35.Visible = true;
            }
            else
            {
                gridBand9.Visible = false;
                gridBand11.Visible = false;
                gridBand19.Visible = false;
                //gridBand27.Visible = false;
                //cbx_IsWatch.Visible = false;
                cmb_IsWatch.Text = "非政府专项监管";
                //cbx_IsWatch.Checked = false;
                cmb_IsWatch.Visible = false;
                gridBand35.Visible = false;
            }

        }
        #endregion

        #region 右键添加标子表属性
        private void 添加地块ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (viewb.RowCount == 0)
            {
                return;
            }
            if (gridB.FocusedView.IsDetailView)
            {
                MessageHelper.ShowInfo("请选地块所属标");
                return;
            }
            this.viewb.PostEditor();
            this.viewb.UpdateCurrentRow();

            m_intFocusedRow = viewb.FocusedRowHandle;

            SkyMap.Net.DataForms.DataEngine.SQLDataEngine sqlDataEngine = new SkyMap.Net.DataForms.DataEngine.SQLDataEngine();
            sqlDataEngine.SaveData(this.dataFormController.DAODataForm.DataSource, m_dstAll.Tables["YW_tdzbpm_b"]);

            DataRow drB = viewb.GetDataRow(m_intFocusedRow);

            m_dtbDK = m_dstAll.Tables["YW_tdzbpm_td"];
            DataRow dr1 = m_dtbDK.NewRow();
            dr1["标"] = drB["Bid"];
            m_dtbDK.Rows.Add(dr1);

        }


        private void 添加地块ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (gridViewB.RowCount == 0)
            {
                return;
            }
            if (gridB1.FocusedView.IsDetailView)
            {
                MessageHelper.ShowInfo("请选地块所属标");
                return;
            }
            this.gridB1.FocusedView.PostEditor();
            this.gridB1.FocusedView.UpdateCurrentRow();

            m_intFocusedRow = gridViewB.FocusedRowHandle;


            SkyMap.Net.DataForms.DataEngine.SQLDataEngine sqlDataEngine = new SkyMap.Net.DataForms.DataEngine.SQLDataEngine();
            sqlDataEngine.SaveData(this.dataFormController.DAODataForm.DataSource, m_dstAll.Tables["YW_tdzbpm_b"]);

            DataRow drB = gridViewB.GetDataRow(m_intFocusedRow);

            m_dtbDK = m_dstAll.Tables["YW_tdzbpm_td"];
            DataRow dr1 = m_dtbDK.NewRow();
            dr1["标"] = drB["Bid"];
            m_dtbDK.Rows.Add(dr1);
        }

        private void 添加收款信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridViewB.RowCount == 0)
            {
                return;
            }
            if (gridB1.FocusedView.IsDetailView)
            {
                MessageHelper.ShowInfo("请选地块所属标");
                return;
            }
            this.gridB1.FocusedView.PostEditor();
            this.gridB1.FocusedView.UpdateCurrentRow();

            m_intFocusedRow = gridViewB.FocusedRowHandle;

            SkyMap.Net.DataForms.DataEngine.SQLDataEngine sqlDataEngine = new SkyMap.Net.DataForms.DataEngine.SQLDataEngine();
            sqlDataEngine.SaveData(this.dataFormController.DAODataForm.DataSource, m_dstAll.Tables["YW_tdzbpm_b"]);

            DataRow drB = gridViewB.GetDataRow(m_intFocusedRow);

            m_dtbDjksq = m_dstAll.Tables["YW_tdzbpm_djksq"];
            DataRow dr1 = m_dtbDjksq.NewRow();
            dr1["B_id"] = drB["Bid"];
            m_dtbDjksq.Rows.Add(dr1);
            //BBindData();

        }

        private void 添加挂牌竞买报价ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridViewB.RowCount == 0)
            {
                return;
            }
            if (gridB1.FocusedView.IsDetailView)
            {
                MessageHelper.ShowInfo("请选地块所属标");
                return;
            }
            this.gridB1.FocusedView.PostEditor();
            this.gridB1.FocusedView.UpdateCurrentRow();
            m_intFocusedRow = gridViewB.FocusedRowHandle;


            SkyMap.Net.DataForms.DataEngine.SQLDataEngine sqlDataEngine = new SkyMap.Net.DataForms.DataEngine.SQLDataEngine();
            sqlDataEngine.SaveData(this.dataFormController.DAODataForm.DataSource, m_dstAll.Tables["YW_tdzbpm_b"]);

            DataRow drB = gridViewB.GetDataRow(m_intFocusedRow);

            m_dtbGpbj = m_dstAll.Tables["YW_tdzbpm_gpbj"];
            DataRow dr1 = m_dtbGpbj.NewRow();
            dr1["B_id"] = drB["Bid"];
            m_dtbGpbj.Rows.Add(dr1);
            // BBindData();
        }

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridViewB.RowCount == 0)
            {
                return;
            }
            if (gridB1.FocusedView.IsDetailView)
            {
                MessageHelper.ShowInfo("请选地块所属标");
                return;
            }
            this.gridB1.FocusedView.PostEditor();
            this.gridB1.FocusedView.UpdateCurrentRow();
            m_intFocusedRow = gridViewB.FocusedRowHandle;
            SkyMap.Net.DataForms.DataEngine.SQLDataEngine sqlDataEngine = new SkyMap.Net.DataForms.DataEngine.SQLDataEngine();
            sqlDataEngine.SaveData(this.dataFormController.DAODataForm.DataSource, m_dstAll.Tables["YW_tdzbpm_b"]);

            DataRow drB = gridViewB.GetDataRow(m_intFocusedRow);

            m_dtbPhdj = m_dstAll.Tables["YW_tdzbpm_phdj"];
            DataRow dr1 = m_dtbPhdj.NewRow();
            dr1["B_id"] = drB["Bid"];
            m_dtbPhdj.Rows.Add(dr1);
            // BBindData();
        }

        private void 宗地图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridB1.FocusedView.IsDetailView)
                {
                    DataRow dr = ((DevExpress.XtraGrid.Views.Card.CardView)gridB1.FocusedView).GetDataRow(((DevExpress.XtraGrid.Views.Card.CardView)gridB1.FocusedView).FocusedRowHandle);
                    if (dr != null)
                    {
                        if (MessageHelper.ShowYesNoInfo("需要添加宗地图吗？") == DialogResult.Yes)
                        {
                            this.gridViewB.PostEditor();
                            this.gridViewB.UpdateCurrentRow();

                            string strMapID;
                            string strPictureFilePath;
                            string strTemp;
                            if (dr["图纸编号"] != DBNull.Value)
                            {
                                strMapID = dr["图纸编号"].ToString();
                            }
                            else
                            {
                                MessageHelper.ShowInfo("请填写图纸编号！");
                                return;
                            }
                            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + Path.DirectorySeparatorChar + m_strYwNo + "标书文件"))
                            {
                                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + Path.DirectorySeparatorChar + m_strYwNo + "标书文件");
                            }
                            strTemp = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + Path.DirectorySeparatorChar + m_strYwNo + "标书文件" + Path.DirectorySeparatorChar;

                            //ZsJZFDJGisMgr.CZsJZFDJGisMgrAppClass cls;


                            //Process[] proc = System.Diagnostics.Process.GetProcessesByName("ZsJZFDJGisMgr");
                            //if (proc.Length == 0)
                            //{
                            //    MessageHelper.ShowInfo("请先登陆基准地价查询系统");
                            //    cls = new ZsJZFDJGisMgr.CZsJZFDJGisMgrAppClass();

                            //    return;
                            //}
                            //else
                            //{
                            //    cls = new ZsJZFDJGisMgr.CZsJZFDJGisMgrAppClass();
                            //}
                            //cls.ZBB_QueryZd(ref m_strYwNo, ref strMapID, ref strTemp);

                            Process proc = new Process();
                            proc.StartInfo.CreateNoWindow = true;
                            proc.StartInfo.UseShellExecute = false;
                            proc.StartInfo.FileName = Application.StartupPath + "\\test.exe";
                            proc.StartInfo.Arguments = "/" + m_strYwNo + "/" + strMapID + "/" + strTemp;
                            proc.Start();
                        }
                    }
                    else
                    {
                        MessageHelper.ShowInfo("选择地块！");
                    }
                }
                else
                {
                    MessageHelper.ShowInfo("选择地块！");
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void 万分一图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridB1.Focused)
            {
                if (gridB1.FocusedView.IsDetailView)
                {
                    DataRow dr = ((DevExpress.XtraGrid.Views.Card.CardView)gridB1.FocusedView).GetDataRow(((DevExpress.XtraGrid.Views.Card.CardView)gridB1.FocusedView).FocusedRowHandle);
                    if (dr != null)
                    {
                        if (MessageHelper.ShowYesNoInfo("需要添加万分一图吗？") == DialogResult.Yes)
                        {
                            this.viewTD.PostEditor();
                            this.viewTD.UpdateCurrentRow();
                            string strMapID;
                            string strPictureFilePath;
                            string strTemp;
                            if (dr["图纸编号"] != DBNull.Value)
                            {
                                strMapID = dr["图纸编号"].ToString();
                            }
                            else
                            {
                                MessageHelper.ShowInfo("请填写图纸编号！");
                                return;
                            }
                            strTemp = Path.GetTempPath();
                            //ZsJZFDJGisMgr.CZsJZFDJGisMgrAppClass cls;
                            //Process[] proc = System.Diagnostics.Process.GetProcessesByName("ZsJZFDJGisMgr");
                            //if (proc.Length == 0)
                            //{
                            //    MessageHelper.ShowInfo("请先登陆基准地价查询系统");
                            //    cls = new ZsJZFDJGisMgr.CZsJZFDJGisMgrAppClass();

                            //    return;
                            //}
                            //else
                            //{
                            //    cls = new ZsJZFDJGisMgr.CZsJZFDJGisMgrAppClass();
                            //}
                            //cls.ZBB_QueryZd(ref m_strYwNo, ref strMapID, ref strTemp);
                            Process proc = new Process();
                            proc.StartInfo.CreateNoWindow = true;
                            proc.StartInfo.UseShellExecute = false;
                            proc.StartInfo.FileName = Application.StartupPath + "\\test.exe";
                            proc.StartInfo.Arguments = "/" + m_strYwNo + "/" + strMapID + "/" + strTemp;
                            proc.Start();
                        }
                    }
                    else
                    {
                        MessageHelper.ShowInfo("选择地块！");
                    }
                }
                else
                {
                    MessageHelper.ShowInfo("选择地块！");
                }
            }
            else
            {
                MessageHelper.ShowInfo("选择地块！");
            }
        }

        private void 地价图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridB1.Focused)
            {
                if (gridB1.FocusedView.IsDetailView)
                {
                    DataRow dr = ((DevExpress.XtraGrid.Views.Card.CardView)gridB1.FocusedView).GetDataRow(((DevExpress.XtraGrid.Views.Card.CardView)gridB1.FocusedView).FocusedRowHandle);
                    if (dr != null)
                    {
                        if (MessageHelper.ShowYesNoInfo("需要添加地价图吗？") == DialogResult.Yes)
                        {
                            this.viewTD.PostEditor();
                            this.viewTD.UpdateCurrentRow();
                            Map2PicTool.clsMap2PicClass cls = new Map2PicTool.clsMap2PicClass();
                            string strMapID;
                            if (dr["图纸编号"] != DBNull.Value)
                            {
                                strMapID = dr["图纸编号"].ToString();
                            }
                            else
                            {
                                MessageHelper.ShowInfo("请填写图纸编号！");
                                return;
                            }
                            cls.SearchMap(ref strMapID);
                            if (cls.PictureFilePath != null && cls.PictureFilePath != "" && File.Exists(cls.PictureFilePath))
                            {

                                byte[] byteImage;
                                Image img = Image.FromFile(cls.PictureFilePath);
                                MemoryStream mem = new MemoryStream();
                                img.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
                                mem.Flush();
                                byteImage = mem.ToArray();
                                dr.BeginEdit();
                                dr["地价图"] = byteImage;
                                dr.EndEdit();
                            }
                        }
                    }
                    else
                    {
                        MessageHelper.ShowInfo("选择地块！");
                    }
                }
                else
                {
                    MessageHelper.ShowInfo("选择地块！");
                }
            }
            else
            {
                MessageHelper.ShowInfo("选择地块！");
            }
        }

        void repositoryItemTextEdit1_Leave(object sender, EventArgs e)
        {
            if (MessageHelper.ShowYesNoInfo("需要添加宗地图吗？") == DialogResult.Yes)
            {
                DataRow dr = ((DevExpress.XtraGrid.Views.Card.CardView)gridB.FocusedView).GetDataRow(((DevExpress.XtraGrid.Views.Card.CardView)gridB.FocusedView).FocusedRowHandle);
                this.viewTD.PostEditor();
                this.viewTD.UpdateCurrentRow();
                Map2PicTool.clsMap2PicClass cls = new Map2PicTool.clsMap2PicClass();
                string strMapID = ((DevExpress.XtraEditors.TextEdit)sender).Text.Trim();
                cls.SearchMap(ref strMapID);
                if (cls.PictureFilePath != null && cls.PictureFilePath != "" && File.Exists(cls.PictureFilePath))
                {
                    byte[] byteImage;
                    Image img = Image.FromFile(cls.PictureFilePath);
                    MemoryStream mem = new MemoryStream();
                    img.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
                    mem.Flush();
                    byteImage = mem.ToArray();
                    dr.BeginEdit();
                    dr["宗地图"] = byteImage;
                    dr.EndEdit();
                }
            }

        }
        void repositoryItemTextEdit3_Leave(object sender, EventArgs e)
        {
            if (MessageHelper.ShowYesNoInfo("需要添加宗地图吗？") == DialogResult.Yes)
            {
                DataRow dr = ((DevExpress.XtraGrid.Views.Card.CardView)gridB1.FocusedView).GetDataRow(((DevExpress.XtraGrid.Views.Card.CardView)gridB1.FocusedView).FocusedRowHandle);
                this.viewTD.PostEditor();
                this.viewTD.UpdateCurrentRow();
                Map2PicTool.clsMap2PicClass cls = new Map2PicTool.clsMap2PicClass();
                string strMapID = ((DevExpress.XtraEditors.TextEdit)sender).Text.Trim();
                cls.SearchMap(ref strMapID);
                if (cls.PictureFilePath != null && cls.PictureFilePath != "" && File.Exists(cls.PictureFilePath))
                {

                    byte[] byteImage;
                    Image img = Image.FromFile(cls.PictureFilePath);
                    MemoryStream mem = new MemoryStream();
                    img.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
                    mem.Flush();
                    byteImage = mem.ToArray();
                    dr.BeginEdit();
                    dr["宗地图"] = byteImage;
                    dr.EndEdit();
                }
            }
        }

        #endregion

        #region 与现场竞价系统同步数据的相关操作
        public string GetDatabaseConnectionString()
        {
            string connString = "";
            Microsoft.Data.ConnectionUI.DataConnectionDialog dialog = new Microsoft.Data.ConnectionUI.DataConnectionDialog();

            //  必须增加以下四项中任一一项
            dialog.DataSources.Add(DataSource.AccessDataSource); // Access 
            dialog.DataSources.Add(DataSource.OdbcDataSource);  // ODBC
            dialog.DataSources.Add(DataSource.OracleDataSource); // Oracle 
            dialog.DataSources.Add(DataSource.SqlDataSource);  // Sql Server

            // 初始化
            dialog.SelectedDataSource = DataSource.SqlDataSource;
            dialog.SelectedDataProvider = DataProvider.SqlDataProvider;

            if (DataConnectionDialog.Show(dialog) == DialogResult.OK)
            {
                connString = dialog.ConnectionString;
            }

            return connString;
        }

        private void m_btn_Click(object sender, EventArgs e)
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove("竞投数据连接");
            config.AppSettings.Settings.Add("竞投数据连接", GetDatabaseConnectionString());
            config.Save(ConfigurationSaveMode.Full);

        }

        private bool JudgeConnection(string strConnectionString)
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings["竞投数据连接"].Value.ToString() != "")
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(config.AppSettings.Settings["竞投数据连接"].Value))
                    {

                        conn.Open();
                        return true;
                    }
                }
                catch (SqlException ex)
                {
                    MessageHelper.ShowInfo(ex.Message);
                    return false;
                }
            }
            else
            {
                MessageHelper.ShowInfo("数据连接有误!请从新配置");
                return false;
            }
        }

        private void m_btnImport_Click(object sender, EventArgs e)
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (JudgeConnection(config.AppSettings.Settings["竞投数据连接"].Value))
            {
                try
                {
                    foreach (DataRow dr in m_dstAll.Tables["YW_tdzbpm_b"].Rows)
                    {
                        if (dr.RowState != DataRowState.Deleted)
                        {
                            m_dstJT = SqlHelper.ExecuteDataset(config.AppSettings.Settings["竞投数据连接"].Value, CommandType.Text, @"select T_MFXX.*,T_DKXX.B_id from T_MFXX left outer join T_DKXX on T_MFXX.DK_ID =T_DKXX.ID where T_DKXX.B_id = " + dr["Bid"].ToString() + " order by 更新日期 desc    select T_PMXX.*,T_DKXX.B_id from T_PMXX  left outer join T_DKXX on T_PMXX.DK_ID =T_DKXX.ID where T_DKXX.B_id = " + dr["Bid"].ToString() + " order by 时间 desc");
                            m_dstJT.Tables[0].TableName = "T_MFXX";
                            m_dstJT.Tables[1].TableName = "T_PMXX";
                            foreach (DataRow drMFXX in m_dstJT.Tables["T_MFXX"].Rows)
                            {
                                DataRow drphdj = m_dstAll.Tables["YW_tdzbpm_phdj"].NewRow();
                                drphdj["竞投牌号"] = drMFXX["牌号"];
                                drphdj["竞投人"] = drMFXX["姓名"];
                                drphdj["联系电话"] = drMFXX["电话"];
                                drphdj["备注"] = drMFXX["公司"];
                                drphdj["B_id"] = drMFXX["B_id"];
                                m_dstAll.Tables["YW_tdzbpm_phdj"].Rows.Add(drphdj);
                            }

                            foreach (DataRow drPMXX in m_dstJT.Tables["T_PMXX"].Rows)
                            {
                                DataRow drxcbj = m_dstAll.Tables["YW_tdzbpm_xcbj"].NewRow();
                                drxcbj["牌号"] = drPMXX["牌号"];
                                drxcbj["总价"] = drPMXX["总价"];
                                drxcbj["时间"] = drPMXX["时间"];
                                drxcbj["B_id"] = drPMXX["B_id"];
                                m_dstAll.Tables["YW_tdzbpm_xcbj"].Rows.Add(drxcbj);
                            }

                            dr.BeginEdit();
                            double max = (double)m_dstJT.Tables["T_PMXX"].Compute("max(总价)", "true");
                            dr["成交价"] = max;
                            dr.EndEdit();
                            foreach (DataRow drDjksq in m_dstAll.Tables["YW_tdzbpm_djksq"].Rows)
                            {
                                if (drDjksq.RowState != DataRowState.Deleted)
                                {
                                    drDjksq.BeginEdit();
                                    drDjksq["成交价支付金额"] = max * Convert.ToDouble(drDjksq["支付比例"] == DBNull.Value ? 0 : drDjksq["支付比例"]);
                                    drDjksq.EndEdit();
                                }
                            }

                        }
                    }
                    MessageHelper.ShowInfo("数据导入成功");


                }
                catch (SqlException ex)
                {
                    MessageHelper.ShowInfo(ex.Message);
                    return;
                }
            }
            else
            {
                MessageHelper.ShowInfo("数据连接有误!请从新配置");
                return;
            }
        }

        private void m_btnExport_Click(object sender, EventArgs e)
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            try
            {
                if (JudgeConnection(config.AppSettings.Settings["竞投数据连接"].Value))
                {
                    if (m_dstAll != null)
                    {
                        SqlParameter[] param = new SqlParameter[10];
                        param[0] = new SqlParameter("地块编号", DbType.String);
                        param[1] = new SqlParameter("座落", DbType.String);
                        param[2] = new SqlParameter("用途", DbType.String);
                        param[3] = new SqlParameter("底价", DbType.Double);
                        param[4] = new SqlParameter("面积", DbType.Double);
                        param[5] = new SqlParameter("容积率", DbType.Double);
                        param[6] = new SqlParameter("税率", DbType.Double);
                        param[7] = new SqlParameter("图片", SqlDbType.Image);
                        param[8] = new SqlParameter("更新日期", DbType.String);
                        param[9] = new SqlParameter("B_ID", DbType.Int32);

                        IDA0 dao = DAOFactory.GetInstanceByNameSpace("SkyMap.Net.DAO");
                        IDbConnection dbConn = dao.Connection;
                        IDbCommand dbCmd;
                        foreach (DataRow dr in m_dstAll.Tables["YW_tdzbpm_b"].Rows)
                        {
                            if (dr.RowState != DataRowState.Deleted)
                            {
                                dbCmd = dbConn.CreateCommand();
                                dbCmd.CommandType = CommandType.Text;
                                dbCmd.CommandText = "select * from YW_tdzbpm_b where Bid = @Bid";
                                dbCmd.Parameters.Add(new SqlParameter("@bid", SqlDbType.Int));
                                ((SqlParameter)dbCmd.Parameters["@bid"]).Value = dr["Bid"].ToString();
                                using (SqlDataReader dataReader = (SqlDataReader)dbCmd.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        param[0].Value = dataReader["宗地编号标"];
                                        param[1].Value = dataReader["土地位置坐落"];
                                        param[2].Value = dataReader["原土地用途"];
                                        param[3].Value = dataReader["交易底价"];
                                        param[4].Value = dataReader["用地面积"];
                                        param[5].Value = dataReader["容积率标准值标"];//.ToString();//+ dr["容积率允许值标"].ToString();
                                        param[6].Value = DBNull.Value;
                                        param[7].Value = dataReader["宗地图标"];
                                        param[8].Value = DateTime.Now.ToString();
                                        param[9].Value = dataReader["Bid"];
                                    }
                                }


                                //SqlHelper.ExecuteNonQuery(config.AppSettings.Settings["竞投数据连接"].Value, CommandType.Text, "delete from T_DKXX where T_DKXX.B_id = " + dr["Bid"].ToString());
                                SqlHelper.ExecuteNonQuery(config.AppSettings.Settings["竞投数据连接"].Value, CommandType.Text,
                                    "insert into T_DKXX(地块编号,座落,用途,底价,面积,容积率,税率,图片,更新日期,B_id) values(@地块编号,@座落,@用途,@底价,@面积,@容积率,@税率,@图片,@更新日期,@B_id) ", param);
                            }
                        }
                        MessageHelper.ShowInfo("数据导出成功!");
                    }
                    else
                    {
                        MessageHelper.ShowInfo("不存在业务信息，请填写完整!");
                    }
                }
                else
                {
                    MessageHelper.ShowInfo("数据连接有误!请从新配置");
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageHelper.ShowInfo(ex.Message);
                return;
            }
        }

        #endregion

        #region 业务中关键时间的控制
        private void dt_保证金截止日期_EditValueChanged(object sender, EventArgs e)
        {

            try
            {
                lbl_保证金截止日期.Text = DateTimeHelper.GetCnDayOfWeek(dt_保证金截止日期.DateTime);
            }
            catch (Exception ex)
            {

                LoggingService.Debug(ex.Message);
            }
        }

        private void dt_现场竞价时间_EditValueChanged(object sender, EventArgs e)
        {
            lbl_现场竞价时间.Text = DateTimeHelper.GetCnDayOfWeek(dt_现场竞价时间.DateTime);
        }

        private void dt_挂牌报价申请截止时间_EditValueChanged(object sender, EventArgs e)
        {
            lbl_挂牌报价截止时间.Text = DateTimeHelper.GetCnDayOfWeek(dt_挂牌报价申请截止时间.DateTime);
        }

        private void dt_挂牌报价申请起始时间_EditValueChanged(object sender, EventArgs e)
        {
            lbl_挂牌报价起始时间.Text = DateTimeHelper.GetCnDayOfWeek(dt_挂牌报价申请起始时间.DateTime);
        }

        private void cmb_公告天数_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) || e.KeyChar == ((Char)Keys.Back))
            {
                return;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void dt_公告起始日期_EditValueChanged(object sender, EventArgs e)
        {

            try
            {
                if (dt_公告起始日期.OldEditValue != DBNull.Value && Convert.ToDateTime(dt_公告起始日期.OldEditValue).Year != 1)
                {

                    DateTime dtm = dt_公告起始日期.DateTime;
                    lbl_公告起始时间.Text = DateTimeHelper.GetCnDayOfWeek(dtm);
                    dt_公告结束日期.DateTime = Convert.ToDateTime(DateTimeHelper.GetDateExcludingHoli(dtm, Convert.ToDouble(cmb_公告天数.Text)).ToString("yyyy年MM月dd日 11:00"));
                    lbl_公告结束时间.Text = DateTimeHelper.GetCnDayOfWeek(dt_公告结束日期.DateTime);
                    if (dt_公告结束日期.DateTime.Year != 1)
                    {
                        dt_保证金截止日期.DateTime = Convert.ToDateTime(dt_公告结束日期.DateTime.AddDays(-1).ToString("yyyy年MM月dd日 17:00"));
                    }
                    lbl_保证金截止日期.Text = DateTimeHelper.GetCnDayOfWeek(dt_保证金截止日期.DateTime);
                    dt_现场竞价时间.DateTime = Convert.ToDateTime(dt_公告结束日期.DateTime.ToString("yyyy年MM月dd日 15:00"));
                    lbl_现场竞价时间.Text = DateTimeHelper.GetCnDayOfWeek(dt_现场竞价时间.DateTime);

                    dt_挂牌报价申请起始时间.DateTime = Convert.ToDateTime(dt_公告起始日期.DateTime.ToString("yyyy年MM月dd日 9:00"));
                    lbl_挂牌报价起始时间.Text = DateTimeHelper.GetCnDayOfWeek(dt_挂牌报价申请起始时间.DateTime);
                    dt_挂牌报价申请截止时间.DateTime = dt_公告结束日期.DateTime;
                    lbl_挂牌报价截止时间.Text = DateTimeHelper.GetCnDayOfWeek(dt_挂牌报价申请截止时间.DateTime);
                }
            }
            catch (Exception ex)
            {

                LoggingService.Debug(ex.Message);
            }
        }

        private void dt_公告结束日期_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                OnChanged(sender, null);
                lbl_公告结束时间.Text = DateTimeHelper.GetCnDayOfWeek(dt_公告结束日期.DateTime);
            }
            catch (Exception ex)
            {
                LoggingService.Debug(ex.Message);

            }
        }

        private void cmb_公告天数_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmb_公告天数.EditValue != cmb_公告天数.OldEditValue)
                {
                    DateTime dtm = dt_公告起始日期.DateTime;
                    lbl_公告起始时间.Text = DateTimeHelper.GetCnDayOfWeek(dtm);
                    dt_公告结束日期.DateTime = Convert.ToDateTime(DateTimeHelper.GetDateExcludingHoli(dtm, Convert.ToDouble(cmb_公告天数.Text)).ToString("yyyy年MM月dd日 11:00"));
                    lbl_公告结束时间.Text = DateTimeHelper.GetCnDayOfWeek(dt_公告结束日期.DateTime);
                    if (dt_公告结束日期.DateTime.Year != 1)
                    {
                        dt_保证金截止日期.DateTime = Convert.ToDateTime(dt_公告结束日期.DateTime.AddDays(-1).ToString("yyyy年MM月dd日 17:00"));
                    }
                    lbl_保证金截止日期.Text = DateTimeHelper.GetCnDayOfWeek(dt_保证金截止日期.DateTime);
                    dt_现场竞价时间.DateTime = Convert.ToDateTime(dt_公告结束日期.DateTime.ToString("yyyy年MM月dd日 15:00"));
                    lbl_现场竞价时间.Text = DateTimeHelper.GetCnDayOfWeek(dt_现场竞价时间.DateTime);
                }
            }
            catch (FormatException fex)
            {

                LoggingService.Debug(fex.Message + "selectValueChange event error ");

            }
            catch (Exception ex)
            {
                LoggingService.Debug(ex.Message);
            }


        }

        #endregion

        #region 表单上直接打印按钮
        private void DataFormPrint(string strTemp)
        {

            AwokeUpdateReport();
            (this.FindForm() as WfView).Save();
            PrintSet printSet = (this.FindForm() as WfView).PrintSet;//虚的
            printSet = m_printSet;//实的
            foreach (TempletPrint temp in printSet.TempletPrints)
            {
                if (temp.Name.IndexOf(strTemp) != -1)
                {
                    switch (temp.Type.ToLower())
                    {
                        case ".doc":
                            PrintWord(temp, false);
                            break;
                        case ".xslt":
                            PrintBySelf(temp);
                            break;

                        default:
                            throw new NotImplementedException("没有实现的报表打印方式");
                    }

                }
            }

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            DataFormPrint("审批表城区");
        }

        private void simpleButton审批表二_Click(object sender, EventArgs e)
        {
            DataFormPrint("审批表招标办");
        }

        private void simpleButton审批表一_Click(object sender, EventArgs e)
        {
            DataFormPrint("审批表镇区");
        }

        private void simpleButton报纸公告_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == MessageHelper.ShowOkCancelInfo("是否修改该公告？"))
            {
                DataFormPrint("报纸公告");
            }
        }

        private void simpleButton标书公告_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == MessageHelper.ShowOkCancelInfo("是否修改该公告？"))
            {
                DataFormPrint("标书公告");
            }
        }

        private void simpleButton成交确认书_Click(object sender, EventArgs e)
        {
            DataFormPrint("成交确认书");
        }

        private void simpleButton合同_Click(object sender, EventArgs e)
        {
            DataFormPrint("11.合同");
            // gridView6.OptionsView.
        }
        private void simpleButton20_Click(object sender, EventArgs e)
        {
            DataFormPrint("须知");
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            DataFormPrint("现场竞买规则");

        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            DataFormPrint("资料收件收据");
        }

        #endregion

        #region 右键功能
        private void 地块ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (gridB1.Focused)
            {
                if (gridB1.FocusedView.IsDetailView)
                {
                    DataRow dr = ((DevExpress.XtraGrid.Views.Card.CardView)gridB1.FocusedView).GetDataRow(((DevExpress.XtraGrid.Views.Card.CardView)gridB1.FocusedView).FocusedRowHandle);
                    if (dr != null)
                    {
                        if (MessageHelper.ShowYesNoInfo("要删除土地使用证编号为：{0}地块信息吗？", dr["土地使用证编号"].ToString()) == DialogResult.Yes)
                        {
                            ((DevExpress.XtraGrid.Views.Card.CardView)gridB1.FocusedView).DeleteSelectedRows();
                        }
                    }
                    else
                    {
                        MessageHelper.ShowInfo("选择地块！");
                    }
                }
                else
                {
                    MessageHelper.ShowInfo("选择地块！");
                }
            }
            else
            {
                MessageHelper.ShowInfo("选择地块！");
            }
        }

        private void 收款信息ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (gridB1.Focused)
            {
                if (gridB1.FocusedView.IsDetailView)
                {
                    DataRow dr = ((DevExpress.XtraGrid.Views.Grid.GridView)gridB1.FocusedView).GetDataRow(((DevExpress.XtraGrid.Views.Grid.GridView)gridB1.FocusedView).FocusedRowHandle);
                    if (dr != null)
                    {
                        if (MessageHelper.ShowYesNoInfo("要删除{0}地价款信息吗？", dr["期数"].ToString()) == DialogResult.Yes)
                        {
                            ((DevExpress.XtraGrid.Views.Grid.GridView)gridB1.FocusedView).DeleteSelectedRows();
                        }
                    }
                    else
                    {
                        MessageHelper.ShowInfo("选择地价款信息！");
                    }
                }
                else
                {
                    MessageHelper.ShowInfo("选择地价款信息！");
                }
            }
            else
            {
                MessageHelper.ShowInfo("选择地价款信息！");
            }
        }

        private void 挂牌竞买报价ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (gridB1.Focused)
            {
                if (gridB1.FocusedView.IsDetailView)
                {
                    DataRow dr = ((DevExpress.XtraGrid.Views.Grid.GridView)gridB1.FocusedView).GetDataRow(((DevExpress.XtraGrid.Views.Grid.GridView)gridB1.FocusedView).FocusedRowHandle);
                    if (dr != null)
                    {
                        if (MessageHelper.ShowYesNoInfo("要删除选种挂牌报价信息吗？") == DialogResult.Yes)
                        {
                            ((DevExpress.XtraGrid.Views.Grid.GridView)gridB1.FocusedView).DeleteSelectedRows();
                        }
                    }
                    else
                    {
                        MessageHelper.ShowInfo("选择挂牌报价信息！");
                    }
                }
                else
                {
                    MessageHelper.ShowInfo("选择挂牌报价信息！");
                }
            }
            else
            {
                MessageHelper.ShowInfo("选择挂牌报价信息！");
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            gridViewB.AddNewRow();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            viewb.AddNewRow();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //(DevExpress.XtraGrid.Views.Grid.GridView)
            if (gridB1.FocusedView is DevExpress.XtraGrid.Views.Grid.GridView)
            {
                ((DevExpress.XtraGrid.Views.Grid.GridView)gridB1.FocusedView).AddNewRow();

            }
            else if (gridB1.FocusedView is DevExpress.XtraGrid.Views.Card.CardView)
            {
                ((DevExpress.XtraGrid.Views.Card.CardView)gridB1.FocusedView).AddNewRow();
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridB1.FocusedView is DevExpress.XtraGrid.Views.Grid.GridView)
            {
                ((DevExpress.XtraGrid.Views.Grid.GridView)gridB1.FocusedView).DeleteSelectedRows();
            }
            else if (gridB1.FocusedView is DevExpress.XtraGrid.Views.Card.CardView)
            {
                ((DevExpress.XtraGrid.Views.Card.CardView)gridB1.FocusedView).DeleteSelectedRows();
            }
        }

        private void 删除ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (gridB.FocusedView is DevExpress.XtraGrid.Views.Grid.GridView)
            {
                ((DevExpress.XtraGrid.Views.Grid.GridView)gridB.FocusedView).DeleteSelectedRows();
            }
            else if (gridB.FocusedView is DevExpress.XtraGrid.Views.Card.CardView)
            {
                ((DevExpress.XtraGrid.Views.Card.CardView)gridB.FocusedView).DeleteSelectedRows();
            }
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            try
            {
                if (!gridB1.FocusedView.IsDetailView)
                {
                    DataRow dr = ((DevExpress.XtraGrid.Views.Grid.GridView)gridB1.FocusedView).GetDataRow(((DevExpress.XtraGrid.Views.Grid.GridView)gridB1.FocusedView).FocusedRowHandle);
                    if (dr != null)
                    {
                        if (MessageHelper.ShowYesNoInfo("需要添加宗地图吗？") == DialogResult.Yes)
                        {
                            this.gridViewDK.PostEditor();
                            this.gridViewDK.UpdateCurrentRow();

                            string strMapID;
                            string strPictureFilePath;
                            string strTemp;
                            if (dr["图纸编号"] != DBNull.Value)
                            {
                                strMapID = dr["图纸编号"].ToString();
                            }
                            else
                            {
                                MessageHelper.ShowInfo("请填写图纸编号！");
                                return;
                            }
                            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + Path.DirectorySeparatorChar + m_strYwNo + "标书文件"))
                            {
                                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + Path.DirectorySeparatorChar + m_strYwNo + "标书文件");
                            }
                            strTemp = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + Path.DirectorySeparatorChar + m_strYwNo + "标书文件" + Path.DirectorySeparatorChar;

                            //ZsJZFDJGisMgr.CZsJZFDJGisMgrAppClass cls;
                            //Process[] proc = System.Diagnostics.Process.GetProcessesByName("ZsJZFDJGisMgr");
                            //if (proc.Length == 0)
                            //{
                            //    MessageHelper.ShowInfo("请先登陆基准地价查询系统");
                            //    cls = new ZsJZFDJGisMgr.CZsJZFDJGisMgrAppClass();

                            //    return;
                            //}
                            //else
                            //{
                            //    cls = new ZsJZFDJGisMgr.CZsJZFDJGisMgrAppClass();
                            //}
                            //cls.ZBB_QueryZd(ref m_strYwNo, ref strMapID, ref strTemp);
                            Process proc = new Process();
                            //proc.StartInfo.CreateNoWindow = true;
                            //proc.StartInfo.UseShellExecute = false;
                            proc.StartInfo.FileName = Application.StartupPath + "\\test.exe";
                            proc.StartInfo.Arguments = "/" + m_strYwNo + "/" + strMapID + "/" + strTemp;
                            proc.Start();
                        }
                    }
                    else
                    {
                        MessageHelper.ShowInfo("选择标！");
                    }
                }
                else
                {
                    MessageHelper.ShowInfo("选择标！");
                }
            }
            catch (Exception ex)
            {
            }

        }

        private void simpleButton14_Click(object sender, EventArgs e)
        {
            if (MessageHelper.ShowYesNoInfo("确实要删除标记录吗?") == DialogResult.Yes)
            {
                viewb.DeleteSelectedRows();
            }
        }

        private void simpleButton15_Click(object sender, EventArgs e)
        {
            if (MessageHelper.ShowYesNoInfo("确实要删除地块记录吗?") == DialogResult.Yes)
            {
                viewTD.DeleteSelectedRows();
            }
        }

        private void simpleButton16_Click(object sender, EventArgs e)
        {
            if (MessageHelper.ShowYesNoInfo("确实要删除标记录吗?") == DialogResult.Yes)
            {
                gridViewB.DeleteSelectedRows();
            }
        }

        private void simpleButton18_Click(object sender, EventArgs e)
        {
            if (MessageHelper.ShowYesNoInfo("确实要删除地块记录吗?") == DialogResult.Yes)
            {
                gridViewDK.DeleteSelectedRows();
            }
        }

        private void simpleButton19_Click(object sender, EventArgs e)
        {
            if (MessageHelper.ShowYesNoInfo("确实要删除地价款记录吗?") == DialogResult.Yes)
            {
                gridViewBdjksq.DeleteSelectedRows();
            }
        }

        private void simpleButton17_Click(object sender, EventArgs e)
        {
            if (MessageHelper.ShowYesNoInfo("确实要删除挂牌报价记录吗?") == DialogResult.Yes)
            {
                gridViewBgpbj.DeleteSelectedRows();
            }
        }
        #endregion



        private void simpleButton24_Click_1(object sender, EventArgs e)
        {
            if (MessageHelper.ShowYesNoInfo("确实要获取最新标书模板吗？") == DialogResult.Yes)
            {
                string strProjectID = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProjectId, "");
                string strSQLDelete = @"delete from dbo.YW_tdzbpm_Report where 
YW_tdzbpm_Report.PROJECT_ID = '{0}' ";
                strSQLDelete = string.Format(strSQLDelete, strProjectID);
                SkyMap.Net.DAO.QueryHelper.ExecuteSql("Default", string.Empty, strSQLDelete);
                InitReport();
            }
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            ChangeByType();
        }

        private void simpleButton25_Click(object sender, EventArgs e)
        {
            DataFormPrint("公开交易证明书");
        }

        private void contextMenuStrip3_B1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void 添加ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            //(DevExpress.XtraGrid.Views.Grid.GridView)
            if (gridB2.FocusedView is DevExpress.XtraGrid.Views.Grid.GridView)
            {
                ((DevExpress.XtraGrid.Views.Grid.GridView)gridB2.FocusedView).AddNewRow();

            }
            else if (gridB2.FocusedView is DevExpress.XtraGrid.Views.Card.CardView)
            {
                ((DevExpress.XtraGrid.Views.Card.CardView)gridB2.FocusedView).AddNewRow();
            }
        }

        private void 添加ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //(DevExpress.XtraGrid.Views.Grid.GridView)
            if (gridB3.FocusedView is DevExpress.XtraGrid.Views.Grid.GridView)
            {
                ((DevExpress.XtraGrid.Views.Grid.GridView)gridB3.FocusedView).AddNewRow();

            }
            else if (gridB3.FocusedView is DevExpress.XtraGrid.Views.Card.CardView)
            {
                ((DevExpress.XtraGrid.Views.Card.CardView)gridB3.FocusedView).AddNewRow();
            }
        }

        private void 添加ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //(DevExpress.XtraGrid.Views.Grid.GridView)
            if (gridB4.FocusedView is DevExpress.XtraGrid.Views.Grid.GridView)
            {
                ((DevExpress.XtraGrid.Views.Grid.GridView)gridB4.FocusedView).AddNewRow();

            }
            else if (gridB4.FocusedView is DevExpress.XtraGrid.Views.Card.CardView)
            {
                ((DevExpress.XtraGrid.Views.Card.CardView)gridB4.FocusedView).AddNewRow();
            }
        }

        private void 删除ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (gridB2.FocusedView is DevExpress.XtraGrid.Views.Grid.GridView)
            {
                ((DevExpress.XtraGrid.Views.Grid.GridView)gridB2.FocusedView).DeleteSelectedRows();
            }
            else if (gridB2.FocusedView is DevExpress.XtraGrid.Views.Card.CardView)
            {
                ((DevExpress.XtraGrid.Views.Card.CardView)gridB2.FocusedView).DeleteSelectedRows();
            }
        }

        private void 删除ToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (gridB3.FocusedView is DevExpress.XtraGrid.Views.Grid.GridView)
            {
                ((DevExpress.XtraGrid.Views.Grid.GridView)gridB3.FocusedView).DeleteSelectedRows();
            }
            else if (gridB3.FocusedView is DevExpress.XtraGrid.Views.Card.CardView)
            {
                ((DevExpress.XtraGrid.Views.Card.CardView)gridB3.FocusedView).DeleteSelectedRows();
            }
        }

        private void 删除ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (gridB4.FocusedView is DevExpress.XtraGrid.Views.Grid.GridView)
            {
                ((DevExpress.XtraGrid.Views.Grid.GridView)gridB4.FocusedView).DeleteSelectedRows();
            }
            else if (gridB4.FocusedView is DevExpress.XtraGrid.Views.Card.CardView)
            {
                ((DevExpress.XtraGrid.Views.Card.CardView)gridB4.FocusedView).DeleteSelectedRows();
            }
        }



        private void tblData_SelectedPageChanging(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            ChangeByType();
        }

        private void simpleButton审批表2_Click(object sender, EventArgs e)
        {
            DataFormPrint("审批表镇区");
        }

        private void simpleButton26_Click(object sender, EventArgs e)
        {
            if (MessageHelper.ShowYesNoInfo("确实要获取最新标书模板吗？") == DialogResult.Yes)
            {
                UpdateReport();
            }
        }

        private void UpdateReport()
        {
            try
            {
                string strProjectID = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProjectId, "");
                string strSQLDelete = @"delete from dbo.YW_tdzbpm_Report where 
                                    YW_tdzbpm_Report.PROJECT_ID = '{0}' ";
                strSQLDelete = string.Format(strSQLDelete, strProjectID);
                SkyMap.Net.DAO.QueryHelper.ExecuteSql("Default", string.Empty, strSQLDelete);

                InitReport();
                MessageHelper.ShowInfo("已经获取最新版本的报表了");
            }
            catch (Exception ex)
            {
                LoggingService.Debug(ex.Message);
            }
        }

        private void tab交件信息_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cmb_公告天数_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                dt_公告结束日期.DateTime = dt_公告起始日期.DateTime.AddDays(Convert.ToDouble(cmb_公告天数.Text.Trim())).AddHours(Convert.ToDouble(2));
                dt_挂牌报价申请截止时间.DateTime = dt_公告结束日期.DateTime;
                dt_现场竞价时间.DateTime = dt_公告结束日期.DateTime.AddHours(Convert.ToDouble(4));
                dt_保证金截止日期.DateTime = dt_公告结束日期.DateTime.AddDays(Convert.ToDouble(-1)).AddHours(Convert.ToDouble(6));

            }
            catch (FormatException ex)
            {

                LoggingService.Debug(ex.Message + "日期转换出错!");
            }
        }

        private void cmdQm镇区经办人签名_Click(object sender, EventArgs e)
        {

        }

    }
}