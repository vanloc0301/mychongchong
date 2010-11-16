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
using DevExpress.XtraGrid.Views.Base;
using System.Linq;
using System.Xml.Linq;
using Yogesh.ExcelXml;
using System.Threading;
using DevExpress.XtraGrid.Views.Grid;
using Taramon.Exceller;

using ZBPM.fj;
using ZBPM.yd;

using ExcelOper;


namespace ZBPM
{
    public partial class FJ : WfAbstractDataForm
    {
        public FJ()
        {
            InitializeComponent();
            Init();
        }
        public delegate void CloseDialog();    // 定义委托
        public void closeDialog()
        {
            try
            {
                WaitDialogHelper.Close();
            }
            catch { }
        }

        static DateTime beforeTime;            //Excel启动之前时间
        static DateTime afterTime;
        ExcelMapper mapper = new ExcelMapper();
        string projectid = "";
        bool blxbz = false; //加入修改类型和备注的描述;2010-06-02
        bool byd = false;  //加入样点计算功能;2010-06-03
        iyd yd = null;
        DataSet m_dstAll;
        DataSet m_dstYdk;
        BFlag bflag = new BFlag();

        ExcelFromArrayList excelop = null;   //excel操作 样点导出功能时使用; 2010-08-11

        string excelFileName = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"..\addins\AppraiseMethod\fj.xls";

        #region 样点自动计算功能
        string fjconst = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"..\addins\AppraiseMethod\fjconst.xml";
        List<ydconst> listConst = new List<ydconst>();
        ydcollection ydcol;
        #endregion

        protected override void AfterSave()
        {
            base.AfterSave();
            SaveData();
        }

        private void SaveData()
        {
            SMDataSource smDs = this.dataFormController.DAODataForm.DataSource;
            SkyMap.Net.DataForms.DataEngine.SQLDataEngine sqlDataEngine = new SkyMap.Net.DataForms.DataEngine.SQLDataEngine();
            sqlDataEngine.SaveData(smDs, m_dstAll.Tables["yw_yddj"]);
            sqlDataEngine.SaveData(smDs, m_dstAll.Tables["yw_ydfdj"]);
            sqlDataEngine.RefreshDataset(smDs, m_dstAll);
        }

        protected override void AfterBindData()
        {
            base.AfterBindData();
            this.tblData.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.tblData_SelectedPageChanged);
            //Calu(txt地价区片价.Text.ToString(), txt房价区片价.Text.ToString(), txt基准容积率.Text.ToString(), txt基准年限.Text.ToString(), f
            txt地价区片价.Text = base.GetControlBindValue(this.txt地价区片价).ToString();
            //txt房价区片价.Text = base.GetControlBindValue(txt房价区片价).ToString();
            txt基准年限.Text = base.GetControlBindValue(txt基准年限).ToString();
            txt基准容积率.Text = base.GetControlBindValue(txt基准容积率).ToString();
            projectid = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProjectId, "");
            try
            {
                txt房价区片价.Text = ((DevExpress.XtraGrid.Views.Grid.GridView)(smGridView5)).GetDataRow(0)["区片价"].ToString();
            }
            catch
            {

            }

            this.gv结构类型修正.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.Gv_FocusedColumnChanged);
            this.gv朝向修正.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.Gv_FocusedColumnChanged);
            this.gv临路情况修正.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.Gv_FocusedColumnChanged);
            this.gv楼龄修正.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.Gv_FocusedColumnChanged);

            this.gv交通修正.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.Gv_FocusedColumnChanged);
            this.gv容积率修正.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.Gv_FocusedColumnChanged);

            this.gv建筑面积修正.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.Gv_FocusedColumnChanged);
            this.gv电梯修正.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.Gv_FocusedColumnChanged);
            this.gv楼型修正.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.Gv_FocusedColumnChanged);
            this.gv公摊修正.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.Gv_FocusedColumnChanged);
            this.gv复式修正.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.Gv_FocusedColumnChanged);
            this.gv物业管理修正.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.Gv_FocusedColumnChanged);
            this.gv电梯房楼层修正.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.Gv_FocusedColumnChanged);
            this.gv非电梯房楼层修正.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.Gv_FocusedColumnChanged);
            GetAllData();
            BindData();
            FjConst();
            BindYdData();
        }


        /// <summary>
        /// 样点自动计算功能模块
        /// </summary>
        private void FjConst()
        {
            string tmp;
            string[] stra;
            if (listConst.Count >= 1) return;
            XElement xe = XElement.Load(fjconst);
            var f = from ab in xe.Descendants("fj").Elements() select ab;
            foreach (XElement x in f)
            {
                ydconst jg = new ydconst();
                tmp = x.Value;
                stra = x.Value.ToString().Split(new char[] { '#' });
                jg.Name = x.Name.ToString();
                foreach (var v in stra)
                {
                    jg.Al.Add(v.Split(new char[] { '|' }).ToList()[0].ToString());
                    jg.Ls.Add(int.Parse(v.Split(new char[] { '|' }).ToList()[1].ToString()));
                }
                listConst.Add(jg);
            }
        }

        public void BindYdData()
        {
            foreach (ydconst yd in listConst)
            {
                #region 单家
                if (yd.Name == "jglx")
                {
                    repositoryItemComboBoxYddjjglx.Items.Clear();
                    foreach (string s in yd.Al)
                    {
                        repositoryItemComboBoxYddjjglx.Items.Add(s);
                        repositoryItemComboBoxYddjjglx.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                    }
                }
                if (yd.Name == "cx")
                {
                    repositoryItemComboBoxYddjcx.Items.Clear();
                    foreach (string s in yd.Al)
                    {
                        repositoryItemComboBoxYddjcx.Items.Add(s);
                        repositoryItemComboBoxYddjcx.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                    }
                }
                if (yd.Name == "ll")
                {
                    repositoryItemComboBoxYddjll.Items.Clear();
                    foreach (string s in yd.Al)
                    {
                        repositoryItemComboBoxYddjll.Items.Add(s);
                        repositoryItemComboBoxYddjll.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                    }
                }
                if (yd.Name == "lnqk")
                {
                    repositoryItemComboBoxYddjlnqk.Items.Clear();
                    foreach (string s in yd.Al)
                    {
                        repositoryItemComboBoxYddjlnqk.Items.Add(s);
                        repositoryItemComboBoxYddjlnqk.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                    }
                }
                if (yd.Name == "jt")
                {
                    repositoryItemComboBoxYddjjt.Items.Clear();
                    foreach (string s in yd.Al)
                    {
                        repositoryItemComboBoxYddjjt.Items.Add(s);
                        repositoryItemComboBoxYddjjt.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                    }
                }
                if (yd.Name == "rjl")
                {
                    repositoryItemComboBoxYddjrjl.Items.Clear();
                    foreach (string s in yd.Al)
                    {
                        repositoryItemComboBoxYddjrjl.Items.Add(s);
                    }
                }
                if (yd.Name == "ly")
                {
                    repositoryItemComboBoxYddjly.Items.Clear();
                    foreach (string s in yd.Al)
                    {
                        repositoryItemComboBoxYddjly.Items.Add(s);
                        repositoryItemComboBoxYddjly.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                    }
                }
                #endregion
                #region 非单家
                if (yd.Name == "jglx")
                {
                    repositoryItemComboBoxFdjjglx.Items.Clear();
                    foreach (string s in yd.Al)
                    {
                        repositoryItemComboBoxFdjjglx.Items.Add(s);
                        repositoryItemComboBoxFdjjglx.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                    }
                }
                if (yd.Name == "cx")
                {
                    repositoryItemComboBoxFdjcx.Items.Clear();
                    foreach (string s in yd.Al)
                    {
                        repositoryItemComboBoxFdjcx.Items.Add(s);
                        repositoryItemComboBoxFdjcx.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                    }
                }
                if (yd.Name == "ll")
                {
                    repositoryItemComboBoxFdjll.Items.Clear();
                    foreach (string s in yd.Al)
                    {
                        repositoryItemComboBoxFdjll.Items.Add(s);
                        repositoryItemComboBoxFdjll.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                    }
                }
                if (yd.Name == "lnqk")
                {
                    repositoryItemComboBoxFdjllqk.Items.Clear();
                    foreach (string s in yd.Al)
                    {
                        repositoryItemComboBoxFdjllqk.Items.Add(s);
                        repositoryItemComboBoxFdjllqk.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                    }
                }
                if (yd.Name == "lx")
                {
                    repositoryItemComboBoxFdjlx.Items.Clear();
                    foreach (string s in yd.Al)
                    {
                        repositoryItemComboBoxFdjlx.Items.Add(s);
                        repositoryItemComboBoxFdjlx.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                    }
                }
                if (yd.Name == "gt")
                {
                    repositoryItemComboBoxFdjgt.Items.Clear();
                    foreach (string s in yd.Al)
                    {
                        repositoryItemComboBoxFdjgt.Items.Add(s);
                        repositoryItemComboBoxFdjgt.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                    }
                }
                if (yd.Name == "wy")
                {
                    repositoryItemComboBoxFdjwy.Items.Clear();
                    foreach (string s in yd.Al)
                    {
                        repositoryItemComboBoxFdjwy.Items.Add(s);
                        repositoryItemComboBoxFdjwy.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                    }
                }
                if (yd.Name == "fs")
                {
                    repositoryItemComboBoxFdjfs.Items.Clear();
                    foreach (string s in yd.Al)
                    {
                        repositoryItemComboBoxFdjfs.Items.Add(s);
                        repositoryItemComboBoxFdjfs.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                    }
                }
                if (yd.Name == "ly")
                {
                    repositoryItemComboBoxFdjly.Items.Clear();
                    foreach (string s in yd.Al)
                    {
                        repositoryItemComboBoxFdjly.Items.Add(s);
                        repositoryItemComboBoxFdjly.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                    }
                }
                if (yd.Name == "ywdt")
                {
                    repositoryItemComboBoxFdjywdt.Items.Clear();
                    foreach (string s in yd.Al)
                    {
                        repositoryItemComboBoxFdjywdt.Items.Add(s);
                        repositoryItemComboBoxFdjywdt.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                    }
                }
                #endregion
            }
        }

        private DataSet GetAllData()
        {
            string strProjectId = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProjectId, "");
            m_dstAll = SkyMap.Net.DAO.QueryHelper.ExecuteSqls("Default", string.Empty, new string[]{@"SELECT * 
FROM yw_yddj where PROJECT_ID ='"+strProjectId+"' order by id asc","SELECT * FROM yw_ydfdj where PROJECT_ID ='"+strProjectId+"'"}, new string[] { "yw_yddj", "yw_ydfdj" });
            if (m_dstAll != null && m_dstAll.Tables.Count != 0)
            {
                m_dstAll.Tables["yw_yddj"].ExtendedProperties.Add("selectsql", @"SELECT  * FROM yw_yddj where PROJECT_ID ='" + strProjectId + "' order by id asc");
                m_dstAll.Tables["yw_ydfdj"].ExtendedProperties.Add("selectsql", @"SELECT  * FROM yw_ydfdj where PROJECT_ID ='" + strProjectId + "' order by id asc");
                //m_dstAll.Tables["YW_gujia_b"].ExtendedProperties.Add("selectsql", @"SELECT  * FROM YW_gujia_b where PROJECT_ID ='" + strProjectId + "' order by bid asc");
                //m_dstAll.Tables["YW_tdzbpm_td"].ExtendedProperties.Add("selectsql", @"SELECT  *  FROM YW_tdzbpm_td where PROJECT_ID ='" + strProjectId + "'");
                //m_dstAll.Tables["YW_tdzbpm_djksq"].ExtendedProperties.Add("selectsql", @"SELECT * FROM YW_tdzbpm_djksq where PROJECT_ID ='" + strProjectId + "' order by  djksqid ");
                //m_dstAll.Tables["YW_tdzbpm_gpbj"].ExtendedProperties.Add("selectsql", @"SELECT * FROM YW_tdzbpm_gpbj where PROJECT_ID ='" + strProjectId + "'");
                //DataRelation dtr = new DataRelation("Btd", m_dstAll.Tables["YW_gujia_b"].Columns["Bid"], m_dstAll.Tables["YW_tdzbpm_td"].Columns["标"], false);
                //m_dstAll.Relations.Add(dtr);
            }
            m_dstAll.Tables["yw_yddj"].TableNewRow += new DataTableNewRowEventHandler(DJ_NewRow);
            m_dstAll.Tables["yw_ydfdj"].TableNewRow += new DataTableNewRowEventHandler(FDJ_NewRow);
            m_dstAll.Tables["yw_yddj"].RowDeleted += new DataRowChangeEventHandler(DJ_RowDeleted);
            m_dstAll.Tables["yw_ydfdj"].RowDeleted += new DataRowChangeEventHandler(DJ_RowDeleted);
            return m_dstAll;
        }

        public void DJ_RowDeleted(object sender, DataRowChangeEventArgs e)
        {
            OnChanged(this, null);
        }

        public void DJ_NewRow(object sender, DataTableNewRowEventArgs e)
        {
            e.Row["PROJECT_ID"] = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProjectId, "");
            e.Row["序号"] = e.Row.Table.Rows.Count + 1;
            e.Row["区片号"] = txt区片号.Text.ToString();
            e.Row["样点编号"] = e.Row.Table.Rows.Count + 1;
            e.Row["区镇"] = lue_镇区.Text.ToString();
            OnChanged(this, null);
        }

        public void FDJ_NewRow(object sender, DataTableNewRowEventArgs e)
        {
            e.Row["PROJECT_ID"] = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProjectId, "");
            e.Row["序号"] = e.Row.Table.Rows.Count + 1;
            e.Row["区片号"] = txt区片号.Text.ToString();
            e.Row["样点编号"] = e.Row.Table.Rows.Count + 1;
            e.Row["区镇"] = lue_镇区.Text.ToString();
            OnChanged(this, null);
        }

        private void BindData()
        {
            gcyddj.DataSource = m_dstAll;
            gcyddj.DataMember = "yw_yddj";
            gcydfdj.DataSource = m_dstAll;
            gcydfdj.DataMember = "yw_ydfdj";
        }

        private void Init()
        {

            //楼龄
            this.repositoryItemLookUpEdit1 = DataWordLookUpEditHelper.Create("FJLN", "Name", "Code");
            this.repositoryItemLookUpEdit1.NullText = "";
            this.col结构类型.ColumnEdit = this.repositoryItemLookUpEdit1;

            //容积率
            this.repositoryItemLookUpEdit2 = DataWordLookUpEditHelper.Create("FJRJL", "Name", "Code");
            this.repositoryItemLookUpEdit2.NullText = "";
            this.col容积类型.ColumnEdit = this.repositoryItemLookUpEdit2;

            //建筑面积
            this.repositoryItemLookUpEdit3 = DataWordLookUpEditHelper.Create("FJLX", "Name", "Code");
            this.repositoryItemLookUpEdit3.NullText = "";
            this.col类型.ColumnEdit = this.repositoryItemLookUpEdit3;

            //镇区
            DataWordLookUpEditHelper.Init(lue_镇区, "FJZQ", "Name", "Name");

            smGridView5.InitNewRow += new InitNewRowEventHandler(smGridView5_InitNewRow);
        }

        private void Btn单家独户_Click(object sender, EventArgs e)
        {
            try
            {
                this.Save();
                blxbz = this.chk修改类型修改备注.Checked;
                ThreadStart ts = null;
                SkyMap.Net.Gui.WaitDialogHelper.Show();
                if (this.cbeType.Text.ToString().IndexOf("非单家独户") >= 0)
                {
                    ts = new ThreadStart(ThreadRun1);
                }
                else if (this.cbeType.Text.ToString().IndexOf("单家独户") >= 0)
                {
                    ts = new ThreadStart(ThreadRun);
                }
                else if (this.cbeType.Text.ToString().IndexOf("车房") >= 0)
                {
                    ts = new ThreadStart(ThreadRun2);
                }
                else
                {
                    MessageHelper.ShowInfo("业务类型不明确");
                    SkyMap.Net.Gui.WaitDialogHelper.Close();
                    return;

                }

                Thread t = new Thread(ts);
                t.IsBackground = true;
                t.Start();
            }
            catch
            {
                SkyMap.Net.Gui.WaitDialogHelper.Close();
            }

        }

        #region 单家独户
        private void ThreadRun()
        {
            string sourceFileName;
            try
            {
                JZFJ oJzfj = new JZFJ();
                DataSet dsData;
                string projectid;
                projectid = this.txtProjectid.Text.ToString();

                dsData = new DataSet();
                dsData.Tables.Add(oJzfj.Jzfj(projectid).Copy());

                dsData.Tables.Add(oJzfj.Cxxz(projectid).Copy());
                dsData.Tables.Add(oJzfj.Cflx(projectid).Copy());//yw_车房类型修正
                dsData.Tables.Add(oJzfj.Cfqp(projectid).Copy());//yw_车房区片价
                dsData.Tables.Add(oJzfj.Dtflc(projectid, 1).Copy());//yw_电梯房楼层修正
                dsData.Tables.Add(oJzfj.Dtflc(projectid, 2).Copy());//yw_电梯房楼层修正
                dsData.Tables.Add(oJzfj.Dtflc(projectid, 3).Copy());//yw_电梯房楼层修正
                dsData.Tables.Add(oJzfj.Dtflc(projectid, 4).Copy());//yw_电梯房楼层修正
                dsData.Tables.Add(oJzfj.Dtflc(projectid, 5).Copy());//yw_电梯房楼层修正
                dsData.Tables.Add(oJzfj.Dtflc(projectid, 6).Copy());//yw_电梯房楼层修正
                dsData.Tables.Add(oJzfj.Dtflc(projectid, 7).Copy());//yw_电梯房楼层修正
                dsData.Tables.Add(oJzfj.Dtflc(projectid, 8).Copy());//yw_电梯房楼层修正
                dsData.Tables.Add(oJzfj.Dtflc(projectid, 9).Copy());//yw_电梯房楼层修正
                dsData.Tables.Add(oJzfj.Dtflc(projectid, 10).Copy());//yw_电梯房楼层修正
                dsData.Tables.Add(oJzfj.Dtflc(projectid, 11).Copy());//yw_电梯房楼层修正

                dsData.Tables.Add(oJzfj.Dt(projectid).Copy());//yw_电梯修正
                dsData.Tables.Add(oJzfj.Fs(projectid).Copy());//yw_复式修正
                dsData.Tables.Add(oJzfj.Gt(projectid).Copy());//yw_公摊修正
                dsData.Tables.Add(oJzfj.Jzmd(projectid).Copy());//yw_建筑密度修正
                dsData.Tables.Add(oJzfj.Jzmj(projectid).Copy());//yw_建筑面积修正
                dsData.Tables.Add(oJzfj.Jt(projectid).Copy());//yw_交通修正
                dsData.Tables.Add(oJzfj.Jglx(projectid).Copy());//yw_结构类型修正
                dsData.Tables.Add(oJzfj.Llxz(projectid).Copy());//yw_临路情况修正
                dsData.Tables.Add(oJzfj.LouLingXz(projectid).Copy());//yw_楼龄修正
                dsData.Tables.Add(oJzfj.LouLingXz1(projectid).Copy());//yw_楼龄修正
                dsData.Tables.Add(oJzfj.LouLingXz2(projectid).Copy());//yw_楼龄修正
                dsData.Tables.Add(oJzfj.LouLingXz3(projectid).Copy());//yw_楼龄修正
                dsData.Tables.Add(oJzfj.Lxxz(projectid).Copy());//yw_楼型修正
                dsData.Tables.Add(oJzfj.Qp(projectid).Copy());//yw_区片信息
                dsData.Tables.Add(oJzfj.Rjl(projectid).Copy());//yw_容积率修正
                dsData.Tables.Add(oJzfj.Wdt(projectid, 1).Copy());//yw_无电梯房楼层修正
                dsData.Tables.Add(oJzfj.Wdt(projectid, 2).Copy());//yw_无电梯房楼层修正
                dsData.Tables.Add(oJzfj.Wdt(projectid, 3).Copy());//yw_无电梯房楼层修正
                dsData.Tables.Add(oJzfj.Wdt(projectid, 4).Copy());//yw_无电梯房楼层修正
                dsData.Tables.Add(oJzfj.Wdt(projectid, 5).Copy());//yw_无电梯房楼层修正
                dsData.Tables.Add(oJzfj.Wdt(projectid, 6).Copy());//yw_无电梯房楼层修正
                dsData.Tables.Add(oJzfj.Wdt(projectid, 7).Copy());//yw_无电梯房楼层修正
                dsData.Tables.Add(oJzfj.Wdt(projectid, 8).Copy());//yw_无电梯房楼层修正
                dsData.Tables.Add(oJzfj.Wdt(projectid, 9).Copy());//yw_无电梯房楼层修正
                dsData.Tables.Add(oJzfj.Wdt(projectid, 10).Copy());//yw_无电梯房楼层修正
                dsData.Tables.Add(oJzfj.Wdt(projectid, 11).Copy());//yw_无电梯房楼层修正

                dsData.Tables.Add(oJzfj.Wygl(projectid).Copy());//yw_物业管理修正
                dsData.Tables.Add(oJzfj.Jzmjxzlx().Copy());////建筑面积修正楼型
                dsData.Tables.Add(oJzfj.Llxzjglx().Copy());//楼龄修正结构类型
                dsData.Tables.Add(oJzfj.Rjlxzlx().Copy());//容积率修正类型

                if (blxbz)
                {
                    sourceFileName = Application.StartupPath + @"\fj模板\单家独户类型备注.doc";
                }
                else
                {
                    sourceFileName = Application.StartupPath + @"\fj模板\单家独户.doc";
                }
                string destFileName = Application.StartupPath + @"\生成结果\单家独户-" + this.txt区片号.EditValue.ToString() + ".doc";
                File.Copy(sourceFileName, destFileName, true);
                WordDataSetTemplateEditorlhm.CWordDOCFiller filler = new WordDataSetTemplateEditorlhm.CWordDOCFiller(dsData, destFileName);
                if (!filler.OperationFailed)
                {
                    filler.Transform();
                    if (filler.OperationFailed)
                    {
                        foreach (string str3 in filler.ErrorList)
                        {
                            // MessageBox.Show(str3, "Error", 0, 0x10);
                            MessageBox.Show(str3, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                    }
                }
                else
                {
                    foreach (string str4 in filler.ErrorList)
                    {
                        //MessageBox.Show(str4, "Error", 0, 0x10);
                        MessageBox.Show(str4, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
            }
            catch
            {
            }
            finally
            {
                this.Invoke(new CloseDialog(closeDialog), null);
            }
        }
        #endregion

        #region 非单家独户
        private void ThreadRun1()
        {
            string sourceFileName;
            try
            {
                JZFJ oJzfj = new JZFJ();
                DataSet dsData;
                string projectid;
                projectid = this.txtProjectid.Text.ToString();

                dsData = new DataSet();
                dsData.Tables.Add(oJzfj.Jzfj(projectid).Copy());

                dsData.Tables.Add(oJzfj.Cxxz(projectid).Copy());
                dsData.Tables.Add(oJzfj.Cflx(projectid).Copy());//yw_车房类型修正
                dsData.Tables.Add(oJzfj.Cfqp(projectid).Copy());//yw_车房区片价
                dsData.Tables.Add(oJzfj.Dtflc(projectid, 1).Copy());//yw_电梯房楼层修正
                dsData.Tables.Add(oJzfj.Dtflc(projectid, 2).Copy());//yw_电梯房楼层修正
                dsData.Tables.Add(oJzfj.Dtflc(projectid, 3).Copy());//yw_电梯房楼层修正
                dsData.Tables.Add(oJzfj.Dtflc(projectid, 4).Copy());//yw_电梯房楼层修正
                dsData.Tables.Add(oJzfj.Dtflc(projectid, 5).Copy());//yw_电梯房楼层修正
                dsData.Tables.Add(oJzfj.Dt(projectid).Copy());//yw_电梯修正
                dsData.Tables.Add(oJzfj.Fs(projectid).Copy());//yw_复式修正
                dsData.Tables.Add(oJzfj.Gt(projectid).Copy());//yw_公摊修正
                dsData.Tables.Add(oJzfj.Jzmd(projectid).Copy());//yw_建筑密度修正
                dsData.Tables.Add(oJzfj.Jzmj(projectid).Copy());//yw_建筑面积修正
                dsData.Tables.Add(oJzfj.Jt(projectid).Copy());//yw_交通修正
                dsData.Tables.Add(oJzfj.Jglx(projectid).Copy());//yw_结构类型修正
                dsData.Tables.Add(oJzfj.Llxz(projectid).Copy());//yw_临路情况修正
                dsData.Tables.Add(oJzfj.LouLingXz(projectid).Copy());//yw_楼龄修正
                dsData.Tables.Add(oJzfj.LouLingXz1(projectid).Copy());//yw_楼龄修正
                dsData.Tables.Add(oJzfj.LouLingXz2(projectid).Copy());//yw_楼龄修正
                dsData.Tables.Add(oJzfj.LouLingXz3(projectid).Copy());//yw_楼龄修正
                dsData.Tables.Add(oJzfj.Lxxz(projectid).Copy());//yw_楼型修正
                dsData.Tables.Add(oJzfj.Qp(projectid).Copy());//yw_区片信息
                dsData.Tables.Add(oJzfj.Rjl(projectid).Copy());//yw_容积率修正
                dsData.Tables.Add(oJzfj.Wdt(projectid, 1).Copy());//yw_无电梯房楼层修正
                dsData.Tables.Add(oJzfj.Wdt(projectid, 2).Copy());//yw_无电梯房楼层修正
                dsData.Tables.Add(oJzfj.Wdt(projectid, 3).Copy());//yw_无电梯房楼层修正
                dsData.Tables.Add(oJzfj.Wdt(projectid, 4).Copy());//yw_无电梯房楼层修正
                dsData.Tables.Add(oJzfj.Wdt(projectid, 5).Copy());//yw_无电梯房楼层修正
                dsData.Tables.Add(oJzfj.Wygl(projectid).Copy());//yw_物业管理修正
                dsData.Tables.Add(oJzfj.Jzmjxzlx().Copy());////建筑面积修正楼型
                dsData.Tables.Add(oJzfj.Llxzjglx().Copy());//楼龄修正结构类型
                dsData.Tables.Add(oJzfj.Rjlxzlx().Copy());//容积率修正类型

                if (blxbz)
                {
                    sourceFileName = Application.StartupPath + @"\fj模板\非单家独户类型备注.doc";
                }
                else
                {
                    sourceFileName = Application.StartupPath + @"\fj模板\非单家独户.doc";
                }
                string destFileName = Application.StartupPath + @"\生成结果\非单家独户-" + this.txt区片号.EditValue.ToString() + ".doc";
                File.Copy(sourceFileName, destFileName, true);
                WordDataSetTemplateEditorlhm.CWordDOCFiller filler = new WordDataSetTemplateEditorlhm.CWordDOCFiller(dsData, destFileName);
                if (!filler.OperationFailed)
                {
                    filler.Transform();
                    if (filler.OperationFailed)
                    {
                        foreach (string str3 in filler.ErrorList)
                        {
                            // MessageBox.Show(str3, "Error", 0, 0x10);
                            MessageBox.Show(str3, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                    }
                }
                else
                {
                    foreach (string str4 in filler.ErrorList)
                    {
                        //MessageBox.Show(str4, "Error", 0, 0x10);
                        MessageBox.Show(str4, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
            }
            catch
            {
            }
            finally
            {
                this.Invoke(new CloseDialog(closeDialog), null);
            }
        }
        #endregion

        #region 车房
        private void ThreadRun2()
        {
            try
            {
                JZFJ oJzfj = new JZFJ();
                DataSet dsData;
                string projectid;
                projectid = this.txtProjectid.Text.ToString();

                dsData = new DataSet();
                dsData.Tables.Add(oJzfj.Jzfj(projectid).Copy());

                dsData.Tables.Add(oJzfj.Cxxz(projectid).Copy());
                dsData.Tables.Add(oJzfj.Cflx(projectid).Copy());//yw_车房类型修正
                dsData.Tables.Add(oJzfj.Cfqp(projectid).Copy());//yw_车房区片价
                dsData.Tables.Add(oJzfj.Dtflc(projectid, 1).Copy());//yw_电梯房楼层修正
                dsData.Tables.Add(oJzfj.Dtflc(projectid, 2).Copy());//yw_电梯房楼层修正
                dsData.Tables.Add(oJzfj.Dtflc(projectid, 3).Copy());//yw_电梯房楼层修正
                dsData.Tables.Add(oJzfj.Dtflc(projectid, 4).Copy());//yw_电梯房楼层修正
                dsData.Tables.Add(oJzfj.Dtflc(projectid, 5).Copy());//yw_电梯房楼层修正
                dsData.Tables.Add(oJzfj.Dt(projectid).Copy());//yw_电梯修正
                dsData.Tables.Add(oJzfj.Fs(projectid).Copy());//yw_复式修正
                dsData.Tables.Add(oJzfj.Gt(projectid).Copy());//yw_公摊修正
                dsData.Tables.Add(oJzfj.Jzmd(projectid).Copy());//yw_建筑密度修正
                dsData.Tables.Add(oJzfj.Jzmj(projectid).Copy());//yw_建筑面积修正
                dsData.Tables.Add(oJzfj.Jt(projectid).Copy());//yw_交通修正
                dsData.Tables.Add(oJzfj.Jglx(projectid).Copy());//yw_结构类型修正
                dsData.Tables.Add(oJzfj.Llxz(projectid).Copy());//yw_临路情况修正
                dsData.Tables.Add(oJzfj.LouLingXz(projectid).Copy());//yw_楼龄修正
                dsData.Tables.Add(oJzfj.LouLingXz1(projectid).Copy());//yw_楼龄修正
                dsData.Tables.Add(oJzfj.LouLingXz2(projectid).Copy());//yw_楼龄修正
                dsData.Tables.Add(oJzfj.LouLingXz3(projectid).Copy());//yw_楼龄修正
                dsData.Tables.Add(oJzfj.Lxxz(projectid).Copy());//yw_楼型修正
                dsData.Tables.Add(oJzfj.Qp(projectid).Copy());//yw_区片信息
                dsData.Tables.Add(oJzfj.Rjl(projectid).Copy());//yw_容积率修正
                dsData.Tables.Add(oJzfj.Wdt(projectid, 1).Copy());//yw_无电梯房楼层修正
                dsData.Tables.Add(oJzfj.Wdt(projectid, 2).Copy());//yw_无电梯房楼层修正
                dsData.Tables.Add(oJzfj.Wdt(projectid, 3).Copy());//yw_无电梯房楼层修正
                dsData.Tables.Add(oJzfj.Wdt(projectid, 4).Copy());//yw_无电梯房楼层修正
                dsData.Tables.Add(oJzfj.Wdt(projectid, 5).Copy());//yw_无电梯房楼层修正
                dsData.Tables.Add(oJzfj.Wygl(projectid).Copy());//yw_物业管理修正
                dsData.Tables.Add(oJzfj.Jzmjxzlx().Copy());////建筑面积修正楼型
                dsData.Tables.Add(oJzfj.Llxzjglx().Copy());//楼龄修正结构类型
                dsData.Tables.Add(oJzfj.Rjlxzlx().Copy());//容积率修正类型

                string sourceFileName = Application.StartupPath + @"\fj模板\车房.doc";
                string destFileName = Application.StartupPath + @"\生成结果\车房-" + this.txt区片号.EditValue.ToString() + ".doc";
                File.Copy(sourceFileName, destFileName, true);
                WordDataSetTemplateEditorlhm.CWordDOCFiller filler = new WordDataSetTemplateEditorlhm.CWordDOCFiller(dsData, destFileName);
                if (!filler.OperationFailed)
                {
                    filler.Transform();
                    if (filler.OperationFailed)
                    {
                        foreach (string str3 in filler.ErrorList)
                        {
                            // MessageBox.Show(str3, "Error", 0, 0x10);
                            MessageBox.Show(str3, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                    }
                }
                else
                {
                    foreach (string str4 in filler.ErrorList)
                    {
                        //MessageBox.Show(str4, "Error", 0, 0x10);
                        MessageBox.Show(str4, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
            }
            catch
            {
            }
            finally
            {
                this.Invoke(new CloseDialog(closeDialog), null);
            }
        }
        #endregion

        private void grid朝向修正_Click(object sender, EventArgs e)
        {

        }

        //====临路情况
        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tmpstring = ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString();
            if (tmpstring == "不相同")
            {
                txt临路情况修正.Text = "";
            }
            else if (tmpstring == "自定义")
            {
                txt临路情况修正.Text = "因该区片临路状况一致，故修正系数为0。";
            }
            else
            {

            }
        }

        private void cbe楼型修正_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tmpstring = ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString();
            if (tmpstring == "不相同")
            {
                txt楼型修正.Text = "";
            }
            else if (tmpstring == "1梯1户" || tmpstring == "1梯2户" || tmpstring == "1梯3户及以上")
            {
                txt楼型修正.Text = "因该区片均为" + tmpstring + "，故修正系数为0。";
            }

        }

        private void cbe物业管理修正_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tmpstring = ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString();
            if (tmpstring == "不相同")
            {
                txt物业管理修正.Text = "";
            }
            else if (tmpstring == "有" || tmpstring == "无")
            {
                txt物业管理修正.Text = "因该区片均为" + tmpstring + "物业管理，故修正系数为0。";
            }
        }

        private void cbe复式修正_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tmpstring = ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString();
            if (tmpstring == "不相同")
            {
                txt复式修正.Text = "";
            }
            else if (tmpstring == "复式" || tmpstring == "不是复式")
            {
                txt复式修正.Text = "因该区片均为" + tmpstring + "结构，故修正系数为0。";
            }
        }

        private void cbe结构类型修正_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tmpstring = ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString();
            if (tmpstring == "不相同")
            {
                txt结构类型修正.Text = "";
            }
            else if (tmpstring == "钢筋混凝土" || tmpstring == "混合" || tmpstring == "砖木" || tmpstring == "其它")
            {
                txt结构类型修正.Text = "因该区片建筑结构均为" + tmpstring + "结构，故修正系数为0。";
            }
        }

        private void cbe楼龄修正_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tmpstring = ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString();
            if (tmpstring == "不相同")
            {
                txt楼龄修正.Text = "";
            }
            else if (tmpstring == "自定义")
            {
                txt楼龄修正.Text = "因该区片建筑均建于" + tmpstring + "年，故修正系数为0。";
            }
            else
            {

            }
        }

        private void cbe交通修正_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tmpstring = ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString();
            if (tmpstring == "不相同")
            {
                txt交通修正.Text = "";
            }
            else if (tmpstring == "自定义")
            {
                txt交通修正.Text = "因该区片交通情况一致，故修正系数为0。";
            }
            else
            {

            }
        }

        private void cbe朝向修正_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tmpstring = ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString();
            if (tmpstring == "不相同")
            {
                txt朝向修正.Text = "";
            }
            else if (tmpstring == "自定义")
            {
                txt朝向修正.Text = "因该区片朝向一致，故修正系数为0。";
            }
            else
            {

            }

        }

        private void cbe建筑面积修正_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tmpstring = ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString();
            if (tmpstring == "不相同")
            {
                txt建筑面积修正.Text = "";
            }
            else if (tmpstring == "自定义")
            {
                txt建筑面积修正.Text = "因该区片建筑面积一致，故修正系数为0。";
            }
            else
            {

            }
        }

        private void cbe修改类型_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tmpstring = ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString();
            if (tmpstring == "只修改属性")
            {
                txt修改类型.Text = "只修改属性";
            }
            else if (tmpstring == "只修改图形")
            {
                txt修改类型.Text = "只修改图形";
            }
            else if (tmpstring == "修改图形后，修改修正方式")
            {
                txt修改类型.Text = "修改图形后，修改修正方式（原来区片修正方式为连乘，修改为连加方式）";
            }
            else if (tmpstring == "同时修改属性及图形")
            {
                txt修改类型.Text = "同时修改属性及图形";
            }
            else if (tmpstring == "新增区片")
            {
                txt修改类型.Text = "新增区片";
            }
            else if (tmpstring == "其他")
            {
                txt修改类型.Text = "其他";
            }
            else if (tmpstring == "不修改")
            {
                txt修改类型.Text = "";
            }
            else
            {
                txt修改类型.Text = "";
            }
        }

        private void cbe修改备注_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tmpstring = ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString();
            if (tmpstring == "调整区片价格")
            {
                txt修改备注.Text = "调整区片价格";
            }
            else if (tmpstring == "车房区片新加一种类型的区片价")
            {
                txt修改备注.Text = "车房区片新加一种类型的区片价";
            }
            else if (tmpstring == "其它")
            {
                txt修改备注.Text = "";
            }
            else
            {
                txt修改备注.Text = "";
            }
        }

        private void txt修改类型_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void smGridView5_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            view.SetRowCellValue(e.RowHandle, view.Columns["创建日期"], DateTime.Today);
            view.SetRowCellValue(e.RowHandle, view.Columns["修改日期"], DateTime.Today);
        }

        private void xtraTabPage4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void qm_calu_Click(object sender, EventArgs e)
        {


        }

        private void CaluFj(string type, ref ArrayList result, ref ArrayList name)
        {
            AbstractFj fj = null;
            if (type.IndexOf("楼龄修正-") >= 0) type = type.Replace("楼龄修正-", "");
            switch (type)
            {
                #region 容积率修正
                case "容积率修正-钢混":
                    fj = new Rjlgh();
                    break;
                case "容积率修正-混合":
                    fj = new Rjlhh();
                    break;
                case "容积率修正-砖木":
                    fj = new Rjlzm();
                    break;
                case "容积率修正-其它":
                    fj = new Rjlqt();
                    break;
                #endregion
                #region 楼龄修正
                case "自建房-钢混":
                    fj = new Llszzjhgh();
                    break;
                case "自建房-砖混":
                    fj = new Llszzjhzh();
                    break;
                case "自建房-砖木":
                    fj = new Llszzjhzm();
                    break;
                case "自建房-其他":
                    fj = new Llszzjhqt();
                    break;
                case "商品房-砖混":
                    fj = new Llszsphzh();
                    break;
                case "商品房-钢混(7层以下)":
                    fj = new Llszsphgh1();
                    break;
                case "商品房-钢混(8-18层)":
                    fj = new Llszsphgh2();
                    break;
                case "商品房-钢混(19-25层)":
                    fj = new Llszsphgh3();
                    break;
                #endregion
                #region 结构类型修正
                case "结构类型修正-钢混":
                    fj = new Jglxgh();
                    break;
                case "结构类型修正-混合":
                    fj = new Jglxhh();
                    break;
                case "结构类型修正-砖木":
                    fj = new Jglxzm();
                    break;
                case "结构类型修正-其它":
                    fj = new Jglxqt();
                    break;
                #endregion

                default:
                    break;
            }
            if (fj == null)
            {
                MessageBox.Show("无效的类型！", "提示：");
            }
            else
            {
                Calu(txt地价区片价.Text.ToString(), txt房价区片价.Text.ToString(), txt基准容积率.Text.ToString(), txt基准年限.Text.ToString(), fj, ref result, ref name);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tdqpj">土地区片价</param>
        /// <param name="fjqpj">房价区片价</param>
        /// <param name="rjl">容积率</param>
        /// <param name="jzlx">基准年限</param>
        private void Calu(string tdqpj, string fjqpj, string rjl, string jzlx, AbstractFj gh, ref ArrayList result, ref ArrayList name)
        {
            beforeTime = DateTime.Now;
            double tmpdb = 0;
            try
            {
                if (double.TryParse(jzlx, out tmpdb))
                {
                    gh.Jzlx = double.Parse(jzlx);
                }
                if (double.TryParse(rjl, out tmpdb))
                {
                    gh.Rjl = double.Parse(rjl);
                }
                if (double.TryParse(tdqpj, out tmpdb))
                {
                    gh.Tdqpj = double.Parse(tdqpj);
                }
                if (double.TryParse(fjqpj, out tmpdb))
                {
                    gh.Fjqpj = double.Parse(fjqpj);
                }
                mapper.Write(gh, excelFileName);
                mapper.Read(gh, excelFileName);
                mapper.Write(gh, @"c:\tmp.xls", excelFileName);
                result = gh.Result();
                name = gh.Name();
            }
            finally
            {
                afterTime = DateTime.Now;
                KillExcel.KillExcelProcess(beforeTime, afterTime);
            }
        }

        private void tblData_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            try
            {
                txt房价区片价.Text = ((DevExpress.XtraGrid.Views.Grid.GridView)(smGridView5)).GetDataRow(0)["区片价"].ToString();
            }
            catch
            {

            }
        }



        /// <summary>
        /// 容积率修正
        /// 准备数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="rjllx"></param>
        private void RjlSzData(string type, int rjllx)
        {
            double dfrom;
            double dto;
            if (double.TryParse(txtRjlfrom.Text.ToString(), out dfrom))
            {
                if (double.TryParse(txtRjlto.Text.ToString(), out dto))
                {
                    if (dto < dfrom)
                    {
                        MessageBox.Show(string.Format("{0}不允许大于{1},范围输入请按从小到大的顺序!", dfrom, dto), "提示：");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("请检查容积率的范围输入是否有误", "提示：");
                    return;
                }
            }
            else
            {
                MessageBox.Show("请检查容积率的范围输入是否有误", "提示：");
                return;
            }
            SzRule(grid容积率修正, type, rjllx, dfrom, dto);
        }


        private void SzRule(SmGridControl smgrid, string type)
        {
            SzRule(smgrid, type, 0, 0, 0);
        }
        /// <summary>
        /// 自动计算并填表
        /// </summary>
        /// <param name="smgrid"></param>
        /// <param name="type"></param>
        /// <param name="rjllx"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        private void SzRule(SmGridControl smgrid, string type, int rjllx, double from, double to)
        {
            ArrayList result = null;// = new ArrayList();
            ArrayList name = null;//=  new ArrayList();
            ArrayList colname = new ArrayList();
            try
            {
                #region 得到返回结果
                CaluFj(type, ref result, ref name);
                #endregion
                string tmp;
                DataTable dt = ((System.Data.DataView)(smgrid.DataSource)).Table;
                colname.Clear();
                #region 得到表中的所有列名
                foreach (DataColumn dc in dt.Columns)
                {

                    tmp = dc.ColumnName.ToString();
                    if (type.IndexOf("容积率修正") >= 0)
                    {
                        if (tmp.IndexOf("_") >= 0)
                        {
                            colname.Add(tmp.Replace("_", "."));
                        }
                        else
                        {
                            colname.Add(tmp);
                        }
                    }
                    else if (type.IndexOf("楼龄修正") >= 0)
                    {
                        if (tmp.IndexOf("年") >= 0)
                        {
                            colname.Add(tmp.Replace("年", ""));
                        }
                        else
                        {
                            colname.Add(tmp);
                        }
                    }
                    else if (type.IndexOf("结构类型修正") >= 0)
                    {
                        colname.Add(tmp);
                    }

                    tmp = "";
                }
                #endregion
                if (dt != null)
                {
                    DataRow dr = dt.NewRow();
                    ///容积率修正
                    if (type.IndexOf("容积率修正") >= 0)
                    {
                        dr["容积类型"] = rjllx;
                    }
                    else if (type.IndexOf("楼龄修正") >= 0)
                    {
                        dr["结构类型"] = rjllx;
                    }
                    dr["PROJECT_ID"] = projectid;
                    for (int i = 0; i < name.Count; i++)
                    {
                        if (type.IndexOf("容积率修正") >= 0)
                        {
                            if (Convert.ToDouble(name[i].ToString()) >= from && Convert.ToDouble(name[i]) <= to)
                            {
                                if (colname.Contains(name[i]))
                                {
                                    if (name[i].ToString().IndexOf(".") >= 0)
                                    {
                                        dr[name[i].ToString().Replace(".", "_")] = result[i].ToString();
                                    }
                                    else
                                    {
                                        dr[name[i].ToString()] = result[i].ToString();
                                    }
                                }
                            }
                        }
                        else if (type.IndexOf("楼龄修正") >= 0)
                        {
                            #region 20100521 lhm
                            if (double.Parse(result[0].ToString()) == -9999)
                            {
                                MessageBox.Show("请检查数据输入是否有误！", "提示：");
                                return;
                            }

                            for (int ii = 1; ii < result.Count; ii++)
                            {
                                if (double.Parse(result[ii].ToString()) == -9999)
                                {
                                    result[ii] = result[ii - 1];
                                }
                            }
                            #endregion
                            if (Convert.ToDouble(name[i].ToString()) >= from && Convert.ToDouble(name[i]) <= to)
                            {
                                if (colname.Contains(name[i]))
                                {
                                    if (name[i].ToString().IndexOf("年") >= 0)
                                    {
                                        dr[name[i].ToString()] = result[i].ToString();
                                    }
                                    else
                                    {
                                        dr[string.Format("{0}年", name[i].ToString())] = result[i].ToString();
                                    }
                                }
                            }
                        }
                        else if (type.IndexOf("结构类型修正") >= 0)
                        {
                            if (colname.Contains(name[i]))
                            {
                                dr[name[i].ToString()] = result[i].ToString();
                            }
                        }
                    }
                    dt.Rows.Add(dr);
                }
            }
            catch
            {

            }
        }

        private void qm_容积率修正混合_Click(object sender, EventArgs e)
        {
            RjlSzData("容积率修正-混合", 3);
        }

        private void qm_容积率修正砖木_Click(object sender, EventArgs e)
        {
            RjlSzData("容积率修正-砖木", 4);
        }

        private void qm_容积率修正其它_Click(object sender, EventArgs e)
        {
            RjlSzData("容积率修正-其它", 5);
        }

        private void qm_容积率修正钢混_Click(object sender, EventArgs e)
        {
            RjlSzData("容积率修正-钢混", 2);
        }

        private void cbe计算楼龄修正_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tmpstring = ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString();
            double dfrom = 0;
            double dto = 0;
            int lx = 0;
            if (!string.IsNullOrEmpty(tmpstring))
            {
                switch (tmpstring)
                {
                    case "自建房-钢混":
                        dto = 60;
                        dfrom = 1;
                        lx = 1;
                        break;
                    case "自建房-砖混":
                        lx = 2;
                        dfrom = 1;
                        dto = 50;
                        break;
                    case "自建房-砖木":
                        lx = 3;
                        dfrom = 1;
                        dto = 40;
                        break;
                    case "自建房-其他":
                        lx = 4;
                        dfrom = 1;
                        dto = 10;
                        break;
                    case "商品房-砖混":
                        lx = 2;
                        dfrom = 1;
                        dto = 60;
                        break;
                    case "商品房-钢混(7层以下)":
                        lx = 1;
                        dfrom = 1;
                        dto = 60;
                        break;
                    case "商品房-钢混(8-18层)":
                        lx = 1;
                        dfrom = 1;
                        dto = 60;
                        break;
                    case "商品房-钢混(19-25层)":
                        lx = 1;
                        dfrom = 1;
                        dto = 60;
                        break;

                    default:
                        break;
                }

                SzRule(grid楼龄修正, string.Format("楼龄修正-{0}", tmpstring), lx, dfrom, dto);
            }
        }

        private void qm_结构类型修正钢混_Click(object sender, EventArgs e)
        {
            SzRule(grid结构类型修正, "结构类型修正-钢混");
        }

        private void qm_结构类型修正混合_Click(object sender, EventArgs e)
        {
            SzRule(grid结构类型修正, "结构类型修正-混合");
        }

        private void qm_结构类型修正砖木_Click(object sender, EventArgs e)
        {
            SzRule(grid结构类型修正, "结构类型修正-砖木");
        }

        private void qm_结构类型修正其它_Click(object sender, EventArgs e)
        {
            SzRule(grid结构类型修正, "结构类型修正-其它");
        }

        private void smGridView5_RowUpdated(object sender, RowObjectEventArgs e)
        {
            try
            {
                txt房价区片价.Text = ((DevExpress.XtraGrid.Views.Grid.GridView)(smGridView5)).GetDataRow(0)["区片价"].ToString();
            }
            catch
            {

            }
        }

        #region 样点计算相关
        /// <summary>
        /// 类型改变需要将yd设为空；
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (byd)
            {
                if (yd != null) yd = null;
            }
            if (cbeType.Text.ToString().Equals("单家独户"))
            {
                groupControl单家独户.Visible = true;
                gcyddj.Visible = true;
                gcydfdj.Visible = false;
                groupControl非单家独户.Visible = false;
            }
            else if (cbeType.Text.ToString().Equals("非单家独户"))
            {
                groupControl非单家独户.Visible = true;
                gcydfdj.Visible = true;
                gcyddj.Visible = false;
                groupControl单家独户.Visible = false;
            }
            else
            {
                groupControl非单家独户.Visible = false;
                groupControl单家独户.Visible = false;
            }

        }

        private void chk样点计算_CheckedChanged(object sender, EventArgs e)
        {
            byd = chk样点计算.Checked;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ydcreate()
        {
            if (yd != null) return;
            if (cbeType.Text.ToString() == "单家独户")
            {
                yd = new yddj();
            }
            else if (cbeType.Text.ToString() == "非单家独户")
            {
                yd = new ydfdj();
            }
            if (yd != null)
            {
                try
                {
                    yd.Fjqpj = double.Parse(txt房价区片价.Text.ToString());
                    if (yd is yddj)
                    {
                        txt单家独户区片基准房价.Text = yd.Fjqpj.ToString();
                    }
                    else if (yd is ydfdj)
                    {
                        txt非单家独户区片基准房价.Text = yd.Fjqpj.ToString();
                    }
                }
                catch
                {
                    throw new Exception("请检查房价区片价！");
                }
            }
        }

        private void Gv_FocusedColumnChanged(object sender, FocusedColumnChangedEventArgs e)
        {
            if (!byd) return;
            double tmpsz;
            ydcreate();
            if (((DevExpress.XtraGrid.Views.Base.ColumnView)(sender)).FocusedValue == null) return;
            if (double.TryParse(((DevExpress.XtraGrid.Views.Base.ColumnView)(sender)).FocusedValue.ToString(), out tmpsz))
            {


                //this.gv结构类型修正.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.Gv_FocusedColumnChanged);
                //this.gv朝向修正.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.Gv_FocusedColumnChanged);
                //this.gv临路情况修正.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.Gv_FocusedColumnChanged);
                //this.gv楼龄修正.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.Gv_FocusedColumnChanged);

                //this.gv交通修正.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.Gv_FocusedColumnChanged);
                //this.gv容积率修正.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.Gv_FocusedColumnChanged);

                //this.gv建筑面积修正.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.Gv_FocusedColumnChanged);
                //this.gv电梯修正.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.Gv_FocusedColumnChanged);
                //this.gv楼型修正.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.Gv_FocusedColumnChanged);
                //this.gv公摊修正.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.Gv_FocusedColumnChanged);
                //this.gv复式修正.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.Gv_FocusedColumnChanged);
                //this.gv物业管理修正.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.Gv_FocusedColumnChanged);
                //this.gv电梯房楼层修正.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.Gv_FocusedColumnChanged);
                //this.gv非电梯房楼层修正.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.Gv_FocusedColumnChanged);

                if (yd != null)
                {
                    if (yd is yddj) //单家修正
                    {

                        if (((DevExpress.XtraGrid.Views.Grid.GridView)(sender)).Name.Contains("结构类型修正"))
                        {
                            yd.Jglxsz = tmpsz;
                            txt单家独户结构类型修正系数.Text = tmpsz.ToString();
                            lbl结构类型修正系数.Text = tmpsz.ToString();
                        }
                        else if (((DevExpress.XtraGrid.Views.Grid.GridView)(sender)).Name.Contains("朝向修正"))
                        {
                            yd.Cxsz = tmpsz;
                            txt单家独户朝向修正系数.Text = tmpsz.ToString();
                            lbl朝向修正系数.Text = tmpsz.ToString();
                        }
                        else if (((DevExpress.XtraGrid.Views.Grid.GridView)(sender)).Name.Contains("临路情况修正"))
                        {
                            yd.Lnqksz = tmpsz;
                            txt单家独户临路情况修正系数.Text = tmpsz.ToString();
                            lbl临路情况修正系数.Text = tmpsz.ToString();
                        }
                        else if (((DevExpress.XtraGrid.Views.Grid.GridView)(sender)).Name.Contains("楼龄修正"))
                        {
                            yd.Llsz = tmpsz;
                            txt单家独户楼龄修正系数.Text = tmpsz.ToString();
                            lbl楼龄修正系数.Text = tmpsz.ToString();
                        }
                        else if (((DevExpress.XtraGrid.Views.Grid.GridView)(sender)).Name.Contains("交通修正"))
                        {
                            (yd as iyddj).Jtsz = tmpsz;
                            txt单家独户交通修正系数.Text = tmpsz.ToString();
                            lbl交通修正系数.Text = tmpsz.ToString();
                        }
                        else if (((DevExpress.XtraGrid.Views.Grid.GridView)(sender)).Name.Contains("容积率修正"))
                        {
                            (yd as iyddj).Rjlsz = tmpsz;
                            txt单家独户容积率修正系数.Text = tmpsz.ToString();
                            lbl容积率修正系数.Text = tmpsz.ToString();
                        }
                        txt单家独户样点单价.Text = (yd as iyd).Calu().ToString("#");

                    }
                    else if (yd is ydfdj) //非单家修正
                    {
                        if (((DevExpress.XtraGrid.Views.Grid.GridView)(sender)).Name.Contains("结构类型修正"))
                        {
                            yd.Jglxsz = tmpsz;
                            txt非单家独户结构类型修正系数.Text = tmpsz.ToString();
                            lbl结构类型修正系数.Text = tmpsz.ToString();
                        }
                        else if (((DevExpress.XtraGrid.Views.Grid.GridView)(sender)).Name.Contains("朝向修正"))
                        {
                            yd.Cxsz = tmpsz;
                            txt非单家独户朝向修正系数.Text = tmpsz.ToString();
                            lbl朝向修正系数.Text = tmpsz.ToString();
                        }
                        else if (((DevExpress.XtraGrid.Views.Grid.GridView)(sender)).Name.Contains("临路情况修正"))
                        {
                            yd.Lnqksz = tmpsz;
                            txt非单家独户临路情况修正系数.Text = tmpsz.ToString();
                            lbl临路情况修正系数.Text = tmpsz.ToString();
                        }
                        else if (((DevExpress.XtraGrid.Views.Grid.GridView)(sender)).Name.Contains("楼龄修正"))
                        {
                            yd.Llsz = tmpsz;
                            txt非单家独户楼龄修正系数.Text = tmpsz.ToString();
                            lbl楼龄修正系数.Text = tmpsz.ToString();
                        }
                        #region---------------------非单家
                        else if (((DevExpress.XtraGrid.Views.Grid.GridView)(sender)).Name.Contains("建筑面积"))
                        {
                            (yd as iydfdj).Jzmjsz = tmpsz;
                            txt非单家独户建筑面积修正系数.Text = tmpsz.ToString();
                            lbl建筑面积修正.Text = tmpsz.ToString();
                        }
                        else if (((DevExpress.XtraGrid.Views.Grid.GridView)(sender)).Name.Contains("电梯修正"))
                        {
                            //电梯修正没有使用
                        }
                        else if (((DevExpress.XtraGrid.Views.Grid.GridView)(sender)).Name.Contains("楼型修正"))
                        {
                            (yd as iydfdj).Lxsz = tmpsz;
                            txt非单家独户楼型修正系数.Text = tmpsz.ToString();
                            lbl楼型修正系数.Text = tmpsz.ToString();
                        }
                        else if (((DevExpress.XtraGrid.Views.Grid.GridView)(sender)).Name.Contains("公摊修正"))
                        {
                            (yd as iydfdj).Gtsz = tmpsz;
                            txt非单家独户公摊修正系数.Text = tmpsz.ToString();
                            lbl公摊修正系数.Text = tmpsz.ToString();
                        }
                        else if (((DevExpress.XtraGrid.Views.Grid.GridView)(sender)).Name.Contains("复式修正"))
                        {
                            (yd as iydfdj).Fssz = tmpsz;
                            txt非单家独户复式修正系数.Text = tmpsz.ToString();
                            lbl复式修正系数.Text = tmpsz.ToString();
                        }
                        else if (((DevExpress.XtraGrid.Views.Grid.GridView)(sender)).Name.Contains("物业管理修正"))
                        {
                            (yd as iydfdj).Fssz = tmpsz;
                            txt非单家独户物业管理修正系数.Text = tmpsz.ToString();
                            lbl物业修正系数.Text = tmpsz.ToString();
                        }
                        else if (((DevExpress.XtraGrid.Views.Grid.GridView)(sender)).Name.Contains("电梯房楼层修正"))
                        {
                            (yd as iydfdj).Lcsz = tmpsz;
                            txt非单家独户楼层修正系数.Text = tmpsz.ToString();
                            lbl电梯房楼层修正系数.Text = tmpsz.ToString();
                        }
                        else if (((DevExpress.XtraGrid.Views.Grid.GridView)(sender)).Name.Contains("非电梯房楼层修正"))
                        {
                            (yd as iydfdj).Lcsz = tmpsz;
                            txt非单家独户楼层修正系数.Text = tmpsz.ToString();
                            lbl非电梯房楼层修正系数.Text = tmpsz.ToString();
                        }
                        #endregion
                        txt非单家独户样点单价.Text = (yd as iyd).Calu().ToString("#");
                    }
                }
            }
        }

        #endregion

        private void xtraTabPage5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lbl公摊修正系数_Click(object sender, EventArgs e)
        {

        }

        private void label47_Click(object sender, EventArgs e)
        {

        }

        private void textEdit4_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void txt样点座落_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void bt_searchydk_Click(object sender, EventArgs e)
        {
            iydsearch yds = new ydksearch();
            if (!string.IsNullOrEmpty(lue_镇区.Text.ToString()))
            {
                (yds as ydksearch).Qz = lue_镇区.Text.ToString();
            }
            if (!string.IsNullOrEmpty(txt样点座落.Text.ToString()))
            {
                (yds as ydksearch).Dz = txt样点座落.Text.ToString();
            }
            string tmp = yds.Where();

            string strProjectId = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProjectId, "");
            if (m_dstAll != null)
            {
                if (m_dstAll.Tables["yw_fjydk"] != null)
                {
                    m_dstAll.Tables.Remove("yw_fjydk");
                }
            }
            m_dstYdk = SkyMap.Net.DAO.QueryHelper.ExecuteSqls("Default", string.Empty,
                       new string[] { @"SELECT * FROM yw_fjydk where " + tmp + " " },
                       new string[] { "yw_fjydk" });

            gcYdk.DataSource = m_dstYdk;
            gcYdk.DataMember = "yw_fjydk";
        }

        private void gvYdk_DoubleClick(object sender, EventArgs e)
        {
            double zj;
            double dj;
            double jzmj;
            double tdmj;
            DataRow dr = gvYdk.GetFocusedDataRow();
            if (dr != null)
            {
                if (cbeType.Text.ToString() == "单家独户")
                {
                    DataRow djr = m_dstAll.Tables["yw_yddj"].NewRow();
                    djr["地址"] = dr["座落"];
                    djr["建筑面积"] = dr["建筑面积"];
                    djr["套内面积"] = dr["套内面积"];
                    djr["土地面积"] = dr["土地面积"];
                    if (double.TryParse(dr["总价"].ToString(), out zj))
                    {
                        djr["总价"] = zj.ToString("#");
                        if (double.TryParse(dr["建筑面积"].ToString(), out dj))
                        {
                            djr["单价"] = (zj / dj).ToString("#");
                        }
                    }
                    if (double.TryParse(dr["建筑面积"].ToString(), out jzmj))
                    {
                        if (double.TryParse(dr["土地面积"].ToString(), out tdmj))
                        {
                            djr["容积率"] = (jzmj / tdmj).ToString("#.##");
                        }
                    }
                    m_dstAll.Tables["yw_yddj"].Rows.Add(djr);
                }
                else if (cbeType.Text.ToString() == "非单家独户")
                {
                    DataRow fdjr = m_dstAll.Tables["yw_ydfdj"].NewRow();
                    fdjr["地址"] = dr["座落"];
                    fdjr["建筑面积"] = dr["建筑面积"];
                    fdjr["套内面积"] = dr["套内面积"];
                    fdjr["土地面积"] = dr["土地面积"];
                    if (double.TryParse(dr["总价"].ToString(), out zj))
                    {
                        fdjr["总价"] = zj.ToString("#");
                        if (double.TryParse(dr["建筑面积"].ToString(), out dj))
                        {
                            fdjr["单价"] = (zj / dj).ToString("#");
                        }
                    }
                    m_dstAll.Tables["yw_ydfdj"].Rows.Add(fdjr);
                }

            }
        }

        private void bt_test_Click(object sender, EventArgs e)
        {
            if (cbeType.Text.ToString() == "单家独户")
            {
                EvaluateDjYdata();
            }
            else if (cbeType.Text.ToString() == "非单家独户")
            {
                EvaluateFdjYddata();
            }
        }


        private void EvaluateDjYdata()
        {
            string jglxsz;
            string cxsz;
            string lnqksz;
            double jzmj;
            double tdmj;
            double llsz;
            double rjlsz;
            string jtsz;
            #region 单家
            bflag.Ss = true;
            yddjdata tmpyddjdata = new yddjdata();
            DataTable dtdj = m_dstAll.Tables["yw_yddj"];
            if (dtdj.Rows.Count > 0)
            {
                for (int i = 0; i < dtdj.Rows.Count; i++)
                {
                    if (dtdj.Rows[i]["样点来源"].ToString().Trim() != "评估样点")
                    {
                        continue;
                    }
                    if (double.TryParse(dtdj.Rows[i]["建筑面积"].ToString(), out jzmj))
                    {
                    }
                    else
                    {
                        if (double.TryParse(dtdj.Rows[i]["套内面积"].ToString(), out jzmj))
                        {

                        }
                    }
                    if (jzmj <= 0)
                    {
                        SkyMap.Net.Gui.MessageHelper.ShowInfo(String.Format("请输入样点{0}的建筑面积或套内面积！", dtdj.Rows[i]["序号"].ToString()));
                        return;
                    }

                    if (double.TryParse(dtdj.Rows[i]["土地面积"].ToString(), out tdmj))
                    {
                        dtdj.Rows[i]["容积率"] = (jzmj / tdmj).ToString("#.##");
                    }

                    jglxsz = dtdj.Rows[i]["结构"].ToString();
                    cxsz = dtdj.Rows[i]["朝向"].ToString();
                    lnqksz = dtdj.Rows[i]["临路情况"].ToString();
                    jtsz = dtdj.Rows[i]["交通情况"].ToString();
                    if (double.TryParse(dtdj.Rows[i]["容积率"].ToString(), out rjlsz))
                    {
                    }
                    else
                    {
                        SkyMap.Net.Gui.MessageHelper.ShowInfo(String.Format("请输入样点{0}的容积率！", dtdj.Rows[i]["序号"].ToString()));
                        return;
                    }
                    if (double.TryParse(dtdj.Rows[i]["楼龄"].ToString(), out llsz))
                    {

                    }
                    else
                    {
                        SkyMap.Net.Gui.MessageHelper.ShowInfo(String.Format("请输入样点{0}的楼龄！", dtdj.Rows[i]["序号"].ToString()));
                        return;
                    }
                    tmpyddjdata.Cxsz = cxsz;
                    tmpyddjdata.Jglxsz = jglxsz;
                    tmpyddjdata.Jtsz = jtsz;
                    tmpyddjdata.Jzmj = jzmj;
                    tmpyddjdata.Llsz = llsz;
                    tmpyddjdata.Lnqksz = lnqksz;
                    tmpyddjdata.Rjlsz = rjlsz;
                    EvaluateYd(tmpyddjdata, yd);
                    if (yd != null)
                    {
                        dtdj.Rows[i]["单价"] = yd.Calu().ToString("#");
                        try
                        {
                            dtdj.Rows[i]["总价"] = (yd.Calu() * double.Parse(dtdj.Rows[i]["建筑面积"].ToString())).ToString("#");
                        }
                        catch
                        {
                            try
                            {
                                dtdj.Rows[i]["总价"] = (yd.Calu() * double.Parse(dtdj.Rows[i]["套内面积"].ToString())).ToString("#");
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
            #endregion
        }

        private void EvaluateFdjYddata()
        {
            string jglxsz;
            string cxsz;
            string lnqksz;
            double llsz;
            double jzmj;
            //string lxsz;
            double lcsz;
            double zlc;
            //string wyglsz;
            //string fssz;
            //string gtsz;
            //string ywdt;
            #region 非单家
            bflag.Ss = true;

            ydfdjdata tmpydfdjdata = new ydfdjdata();
            DataTable dtfdj = m_dstAll.Tables["yw_ydfdj"];
            if (dtfdj.Rows.Count > 0)
            {
                for (int i = 0; i < dtfdj.Rows.Count; i++)
                {
                    if (dtfdj.Rows[i]["样点来源"].ToString().Trim() != "评估样点")
                    {
                        continue;
                    }
                    if (double.TryParse(dtfdj.Rows[i]["建筑面积"].ToString(), out jzmj))
                    {
                    }
                    else
                    {
                        if (double.TryParse(dtfdj.Rows[i]["套内面积"].ToString(), out jzmj))
                        {

                        }
                    }
                    if (jzmj <= 0)
                    {
                        SkyMap.Net.Gui.MessageHelper.ShowInfo(String.Format("请输入样点{0}的建筑面积或套内面积！", dtfdj.Rows[i]["序号"].ToString()));
                        return;
                    }

                    jglxsz = dtfdj.Rows[i]["结构"].ToString();
                    cxsz = dtfdj.Rows[i]["朝向"].ToString();
                    lnqksz = dtfdj.Rows[i]["临路情况"].ToString();

                    if (double.TryParse(dtfdj.Rows[i]["楼龄"].ToString(), out llsz))
                    {

                    }
                    else
                    {
                        //SkyMap.Net.Gui.MessageHelper.ShowInfo(String.Format("请输入样点{0}的楼龄！", dtfdj.Rows[i]["序号"].ToString()));
                        //return;
                    }
                    tmpydfdjdata.Cxsz = cxsz;
                    tmpydfdjdata.Jglxsz = jglxsz;
                    tmpydfdjdata.Jzmj = jzmj;
                    tmpydfdjdata.Llsz = llsz;
                    tmpydfdjdata.Lnqksz = lnqksz;
                    //string lxsz;
                    //double lcsz;
                    //double zlc;
                    //string wyglsz;
                    //string fssz;
                    //string gtsz;
                    //string ywdt;
                    tmpydfdjdata.Lxsz = dtfdj.Rows[i]["楼型"].ToString();
                    if (double.TryParse(dtfdj.Rows[i]["所处楼层"].ToString(), out lcsz))
                    {
                        tmpydfdjdata.Lcsz = lcsz;
                    }
                    if (double.TryParse(dtfdj.Rows[i]["总楼层"].ToString(), out zlc))
                    {
                        tmpydfdjdata.Zlc = zlc;
                    }
                    tmpydfdjdata.Wyglsz = dtfdj.Rows[i]["物业"].ToString();
                    tmpydfdjdata.Fssz = dtfdj.Rows[i]["复式"].ToString();
                    tmpydfdjdata.Gtsz = dtfdj.Rows[i]["公摊"].ToString();
                    tmpydfdjdata.Ywdt = dtfdj.Rows[i]["有无电梯"].ToString();
                    EvaluateYd(tmpydfdjdata, yd);
                    if (yd != null)
                    {
                        dtfdj.Rows[i]["单价"] = yd.Calu().ToString("#");
                        try
                        {
                            dtfdj.Rows[i]["总价"] = (yd.Calu() * double.Parse(dtfdj.Rows[i]["建筑面积"].ToString())).ToString("#");
                        }
                        catch
                        {
                            try
                            {
                                dtfdj.Rows[i]["总价"] = (yd.Calu() * double.Parse(dtfdj.Rows[i]["套内面积"].ToString())).ToString("#");
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
            #endregion
        }

        private void EvaluateYd(iyddata tmpyddata, iyd ydreturn)
        {
            string jglxsz = "";
            string cxsz = "";
            string lnqksz = "";
            double jzmj = 0d;
            double llsz = 0d;

            double rjlsz = 0d;
            string jtsz = "";

            string lxsz = "";
            double lcsz = 0d;
            double zlc = 0d;
            string wy = "";
            string fs = "";
            string gt = "";
            string ywdt = "";

            DataSet ds = (this as IDataForm).DataFormConntroller.DataSource;
            XElement fjxml;
            DataTable dtjglx = ds.Tables["yw_结构类型修正"].Copy();
            DataTable dtcx = ds.Tables["yw_朝向修正"].Copy();
            DataTable dtll = ds.Tables["yw_楼龄修正"].Copy();
            DataTable dtllqk = ds.Tables["yw_临路情况修正"].Copy();
            DataTable dtjt = ds.Tables["yw_交通修正"].Copy();
            DataTable dtjzmj = ds.Tables["yw_建筑面积修正"].Copy();
            DataTable dtlx = ds.Tables["yw_楼型修正"].Copy();
            DataTable dtgt = ds.Tables["yw_公摊修正"].Copy();
            DataTable dtwy = ds.Tables["yw_物业管理修正"].Copy();
            DataTable dtfs = ds.Tables["yw_复式修正"].Copy();
            DataTable dtdt = ds.Tables["yw_电梯房楼层修正"].Copy();
            DataTable dtfdt = ds.Tables["yw_无电梯房楼层修正"].Copy();
            DataTable dtrjl = ds.Tables["yw_容积率修正"].Copy();
            DataTable dtsz = ds.Tables["yw_电梯修正"].Copy();
            if (tmpyddata is yddjdata)
            {
                //==========
                cxsz = (tmpyddata as yddjdata).Cxsz;
                jglxsz = (tmpyddata as yddjdata).Jglxsz;
                jtsz = (tmpyddata as yddjdata).Jtsz;
                jzmj = (tmpyddata as yddjdata).Jzmj;
                llsz = (tmpyddata as yddjdata).Llsz;
                lnqksz = (tmpyddata as yddjdata).Lnqksz;
                rjlsz = (tmpyddata as yddjdata).Rjlsz;

            }
            if (tmpyddata is ydfdjdata)
            {
                cxsz = (tmpyddata as ydfdjdata).Cxsz;
                jglxsz = (tmpyddata as ydfdjdata).Jglxsz;
                jzmj = (tmpyddata as ydfdjdata).Jzmj;
                llsz = (tmpyddata as ydfdjdata).Llsz;
                lnqksz = (tmpyddata as ydfdjdata).Lnqksz;

                lxsz = (tmpyddata as ydfdjdata).Lxsz;
                lcsz = (tmpyddata as ydfdjdata).Lcsz;
                zlc = (tmpyddata as ydfdjdata).Zlc;
                wy = (tmpyddata as ydfdjdata).Wyglsz;
                fs = (tmpyddata as ydfdjdata).Fssz;
                gt = (tmpyddata as ydfdjdata).Gtsz;
                ywdt = (tmpyddata as ydfdjdata).Ywdt;

                if (ywdt.Contains("有"))
                {
                    dtfdt.Rows.Clear();
                    dtfdt.AcceptChanges();
                    if (dtdt.Rows.Count >= 1)
                    {
                        foreach (DataRow dr in dtdt.Rows)
                        {
                            try
                            {
                                if (int.Parse(dr["楼层数"].ToString()) != zlc)
                                {
                                    dr.Delete();
                                }
                            }
                            catch
                            {
                            }
                        }
                        dtdt.AcceptChanges();
                    }
                }
                else if (ywdt.Contains("无"))
                {
                    dtdt.Rows.Clear();
                    dtdt.AcceptChanges();
                    if (dtfdt.Rows.Count >= 1)
                    {
                        foreach (DataRow dr in dtfdt.Rows)
                        {
                            try
                            {
                                if (int.Parse(dr["楼层数"].ToString()) != zlc)
                                {
                                    dr.Delete();
                                }
                            }
                            catch
                            {
                            }
                        }
                        dtfdt.AcceptChanges();
                    }

                }
                else
                {

                }
            }
            //==========
            if (dtll.Rows.Count >= 1)
            {
                int tmpjglx = 0;
                if (tmpyddata is yddjdata)
                {
                    if ((tmpyddata as yddjdata).Jglxsz.Contains("钢筋混凝土"))
                    {
                        tmpjglx = 1;
                    }
                    else if ((tmpyddata as yddjdata).Jglxsz.Contains("混合"))
                    {
                        tmpjglx = 2;
                    }
                    else if ((tmpyddata as yddjdata).Jglxsz.Contains("转木"))
                    {
                        tmpjglx = 3;
                    }
                    else if ((tmpyddata as yddjdata).Jglxsz.Contains("其它"))
                    {
                        tmpjglx = 4;
                    }
                }
                else if (tmpyddata is ydfdjdata)
                {
                    if ((tmpyddata as ydfdjdata).Jglxsz.Contains("钢筋混凝土"))
                    {
                        tmpjglx = 1;
                    }
                    else if ((tmpyddata as ydfdjdata).Jglxsz.Contains("混合"))
                    {
                        tmpjglx = 2;
                    }
                    else if ((tmpyddata as ydfdjdata).Jglxsz.Contains("转木"))
                    {
                        tmpjglx = 3;
                    }
                    else if ((tmpyddata as ydfdjdata).Jglxsz.Contains("其它"))
                    {
                        tmpjglx = 4;
                    }
                }
                else
                {
                    return;
                }

                foreach (DataRow dr in dtll.Rows)
                {
                    try
                    {
                        if (int.Parse(dr["结构类型"].ToString()) != tmpjglx)
                        {
                            dr.Delete();
                        }
                    }
                    catch
                    {
                    }
                }
                dtll.AcceptChanges();
            }

            #region
            if (ydcol != null)
            {
                ydcol.Cx.Clear();
                ydcol.Dt.Clear();
                ydcol.Fdt.Clear();
                ydcol.Fs.Clear();
                ydcol.Gt.Clear();
                ydcol.Jglx.Clear();
                ydcol.Jt.Clear();
                ydcol.Jzmj.Clear();
                ydcol.Ll.Clear();
                ydcol.Lnqk.Clear();
                ydcol.Lx.Clear();
                ydcol.Wy.Clear();
                ydcol.Rjl.Clear();
            }
            else
            {
                ydcol = new ydcollection();
            }
            ydcol.Jglx.Add(dtjglx.Rows.Count == 1 ? (dtjglx.Rows[0]["钢筋混凝土"] == System.DBNull.Value ? 8881 : double.Parse(dtjglx.Rows[0]["钢筋混凝土"].ToString())) : 999);
            ydcol.Jglx.Add(dtjglx.Rows.Count == 1 ? (dtjglx.Rows[0]["混合"] == System.DBNull.Value ? 8882 : double.Parse(dtjglx.Rows[0]["混合"].ToString())) : 999);
            ydcol.Jglx.Add(dtjglx.Rows.Count == 1 ? (dtjglx.Rows[0]["砖木"] == System.DBNull.Value ? 8883 : double.Parse(dtjglx.Rows[0]["砖木"].ToString())) : 999);
            ydcol.Jglx.Add(dtjglx.Rows.Count == 1 ? (dtjglx.Rows[0]["其他"] == System.DBNull.Value ? 8884 : double.Parse(dtjglx.Rows[0]["其他"].ToString())) : 999);

            ydcol.Cx.Add(dtcx.Rows.Count == 1 ? (dtcx.Rows[0]["东"] == System.DBNull.Value ? 8881 : double.Parse(dtcx.Rows[0]["东"].ToString())) : 999);
            ydcol.Cx.Add(dtcx.Rows.Count == 1 ? (dtcx.Rows[0]["东南"] == System.DBNull.Value ? 8882 : double.Parse(dtcx.Rows[0]["东南"].ToString())) : 999);
            ydcol.Cx.Add(dtcx.Rows.Count == 1 ? (dtcx.Rows[0]["南"] == System.DBNull.Value ? 8883 : double.Parse(dtcx.Rows[0]["南"].ToString())) : 999);
            ydcol.Cx.Add(dtcx.Rows.Count == 1 ? (dtcx.Rows[0]["西南"] == System.DBNull.Value ? 8884 : double.Parse(dtcx.Rows[0]["西南"].ToString())) : 999);
            ydcol.Cx.Add(dtcx.Rows.Count == 1 ? (dtcx.Rows[0]["西"] == System.DBNull.Value ? 8885 : double.Parse(dtcx.Rows[0]["西"].ToString())) : 999);
            ydcol.Cx.Add(dtcx.Rows.Count == 1 ? (dtcx.Rows[0]["西北"] == System.DBNull.Value ? 8886 : double.Parse(dtcx.Rows[0]["西北"].ToString())) : 999);
            ydcol.Cx.Add(dtcx.Rows.Count == 1 ? (dtcx.Rows[0]["北"] == System.DBNull.Value ? 8887 : double.Parse(dtcx.Rows[0]["北"].ToString())) : 999);
            ydcol.Cx.Add(dtcx.Rows.Count == 1 ? (dtcx.Rows[0]["东北"] == System.DBNull.Value ? 8888 : double.Parse(dtcx.Rows[0]["东北"].ToString())) : 999);

            ydcol.Ll.Add(dtll.Rows.Count == 1 ? (dtll.Rows[0]["1年"] == System.DBNull.Value ? 8881 : double.Parse(dtll.Rows[0]["1年"].ToString())) : 999);
            ydcol.Ll.Add(dtll.Rows.Count == 1 ? (dtll.Rows[0]["2年"] == System.DBNull.Value ? 8882 : double.Parse(dtll.Rows[0]["2年"].ToString())) : 999);
            ydcol.Ll.Add(dtll.Rows.Count == 1 ? (dtll.Rows[0]["3年"] == System.DBNull.Value ? 8883 : double.Parse(dtll.Rows[0]["3年"].ToString())) : 999);
            ydcol.Ll.Add(dtll.Rows.Count == 1 ? (dtll.Rows[0]["4年"] == System.DBNull.Value ? 8884 : double.Parse(dtll.Rows[0]["4年"].ToString())) : 999);
            ydcol.Ll.Add(dtll.Rows.Count == 1 ? (dtll.Rows[0]["5年"] == System.DBNull.Value ? 8885 : double.Parse(dtll.Rows[0]["5年"].ToString())) : 999);
            ydcol.Ll.Add(dtll.Rows.Count == 1 ? (dtll.Rows[0]["6年"] == System.DBNull.Value ? 8886 : double.Parse(dtll.Rows[0]["6年"].ToString())) : 999);
            ydcol.Ll.Add(dtll.Rows.Count == 1 ? (dtll.Rows[0]["7年"] == System.DBNull.Value ? 8887 : double.Parse(dtll.Rows[0]["7年"].ToString())) : 999);
            ydcol.Ll.Add(dtll.Rows.Count == 1 ? (dtll.Rows[0]["8年"] == System.DBNull.Value ? 8888 : double.Parse(dtll.Rows[0]["8年"].ToString())) : 999);
            ydcol.Ll.Add(dtll.Rows.Count == 1 ? (dtll.Rows[0]["10年"] == System.DBNull.Value ? 8889 : double.Parse(dtll.Rows[0]["10年"].ToString())) : 999);
            ydcol.Ll.Add(dtll.Rows.Count == 1 ? (dtll.Rows[0]["12年"] == System.DBNull.Value ? 88810 : double.Parse(dtll.Rows[0]["12年"].ToString())) : 999);
            ydcol.Ll.Add(dtll.Rows.Count == 1 ? (dtll.Rows[0]["13年"] == System.DBNull.Value ? 88811 : double.Parse(dtll.Rows[0]["13年"].ToString())) : 999);
            ydcol.Ll.Add(dtll.Rows.Count == 1 ? (dtll.Rows[0]["15年"] == System.DBNull.Value ? 88812 : double.Parse(dtll.Rows[0]["15年"].ToString())) : 999);
            ydcol.Ll.Add(dtll.Rows.Count == 1 ? (dtll.Rows[0]["18年"] == System.DBNull.Value ? 88813 : double.Parse(dtll.Rows[0]["18年"].ToString())) : 999);
            ydcol.Ll.Add(dtll.Rows.Count == 1 ? (dtll.Rows[0]["20年"] == System.DBNull.Value ? 88814 : double.Parse(dtll.Rows[0]["20年"].ToString())) : 999);
            ydcol.Ll.Add(dtll.Rows.Count == 1 ? (dtll.Rows[0]["22年"] == System.DBNull.Value ? 88815 : double.Parse(dtll.Rows[0]["22年"].ToString())) : 999);
            ydcol.Ll.Add(dtll.Rows.Count == 1 ? (dtll.Rows[0]["25年"] == System.DBNull.Value ? 88816 : double.Parse(dtll.Rows[0]["25年"].ToString())) : 999);
            ydcol.Ll.Add(dtll.Rows.Count == 1 ? (dtll.Rows[0]["26年"] == System.DBNull.Value ? 88817 : double.Parse(dtll.Rows[0]["26年"].ToString())) : 999);
            ydcol.Ll.Add(dtll.Rows.Count == 1 ? (dtll.Rows[0]["30年"] == System.DBNull.Value ? 88818 : double.Parse(dtll.Rows[0]["30年"].ToString())) : 999);
            ydcol.Ll.Add(dtll.Rows.Count == 1 ? (dtll.Rows[0]["35年"] == System.DBNull.Value ? 88819 : double.Parse(dtll.Rows[0]["35年"].ToString())) : 999);
            ydcol.Ll.Add(dtll.Rows.Count == 1 ? (dtll.Rows[0]["40年"] == System.DBNull.Value ? 88820 : double.Parse(dtll.Rows[0]["40年"].ToString())) : 999);
            ydcol.Ll.Add(dtll.Rows.Count == 1 ? (dtll.Rows[0]["45年"] == System.DBNull.Value ? 88821 : double.Parse(dtll.Rows[0]["45年"].ToString())) : 999);
            ydcol.Ll.Add(dtll.Rows.Count == 1 ? (dtll.Rows[0]["50年"] == System.DBNull.Value ? 88822 : double.Parse(dtll.Rows[0]["50年"].ToString())) : 999);
            ydcol.Ll.Add(dtll.Rows.Count == 1 ? (dtll.Rows[0]["55年"] == System.DBNull.Value ? 88823 : double.Parse(dtll.Rows[0]["55年"].ToString())) : 999);
            ydcol.Ll.Add(dtll.Rows.Count == 1 ? (dtll.Rows[0]["60年"] == System.DBNull.Value ? 88824 : double.Parse(dtll.Rows[0]["60年"].ToString())) : 999);
            ydcol.Ll.Add(dtll.Rows.Count == 1 ? (dtll.Rows[0]["65年"] == System.DBNull.Value ? 88825 : double.Parse(dtll.Rows[0]["65年"].ToString())) : 999);
            ydcol.Ll.Add(dtll.Rows.Count == 1 ? (dtll.Rows[0]["70年"] == System.DBNull.Value ? 88826 : double.Parse(dtll.Rows[0]["70年"].ToString())) : 999);

            ydcol.Lnqk.Add(dtllqk.Rows.Count == 1 ? (dtllqk.Rows[0]["临主要交通干道"] == System.DBNull.Value ? 8881 : double.Parse(dtllqk.Rows[0]["临主要交通干道"].ToString())) : 999);
            ydcol.Lnqk.Add(dtllqk.Rows.Count == 1 ? (dtllqk.Rows[0]["临一般交通干道"] == System.DBNull.Value ? 8882 : double.Parse(dtllqk.Rows[0]["临一般交通干道"].ToString())) : 999);
            ydcol.Lnqk.Add(dtllqk.Rows.Count == 1 ? (dtllqk.Rows[0]["不临交通干道"] == System.DBNull.Value ? 8883 : double.Parse(dtllqk.Rows[0]["不临交通干道"].ToString())) : 999);
            ydcol.Lnqk.Add(dtllqk.Rows.Count == 1 ? (dtllqk.Rows[0]["临支路"] == System.DBNull.Value ? 8884 : double.Parse(dtllqk.Rows[0]["临支路"].ToString())) : 999);
            ydcol.Lnqk.Add(dtllqk.Rows.Count == 1 ? (dtllqk.Rows[0]["临小区交通干道"] == System.DBNull.Value ? 8885 : double.Parse(dtllqk.Rows[0]["临小区交通干道"].ToString())) : 999);

            ydcol.Jt.Add(dtjt.Rows.Count == 1 ? (dtjt.Rows[0]["不能通摩托车"] == System.DBNull.Value ? 8881 : double.Parse(dtjt.Rows[0]["不能通摩托车"].ToString())) : 999);
            ydcol.Jt.Add(dtjt.Rows.Count == 1 ? (dtjt.Rows[0]["可同摩托车，不同小汽车"] == System.DBNull.Value ? 8882 : double.Parse(dtjt.Rows[0]["可同摩托车，不同小汽车"].ToString())) : 999);
            ydcol.Jt.Add(dtjt.Rows.Count == 1 ? (dtjt.Rows[0]["可通1小车"] == System.DBNull.Value ? 8883 : double.Parse(dtjt.Rows[0]["可通1小车"].ToString())) : 999);
            ydcol.Jt.Add(dtjt.Rows.Count == 1 ? (dtjt.Rows[0]["可通2小车"] == System.DBNull.Value ? 8884 : double.Parse(dtjt.Rows[0]["可通2小车"].ToString())) : 999);
            ydcol.Jt.Add(dtjt.Rows.Count == 1 ? (dtjt.Rows[0]["可通3小车"] == System.DBNull.Value ? 8885 : double.Parse(dtjt.Rows[0]["可通3小车"].ToString())) : 999);

            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["0_1"] == System.DBNull.Value ? 8881 : double.Parse(dtrjl.Rows[0]["0_1"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["0_2"] == System.DBNull.Value ? 8882 : double.Parse(dtrjl.Rows[0]["0_2"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["0_3"] == System.DBNull.Value ? 8883 : double.Parse(dtrjl.Rows[0]["0_3"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["0_35"] == System.DBNull.Value ? 8884 : double.Parse(dtrjl.Rows[0]["0_35"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["0_4"] == System.DBNull.Value ? 8885 : double.Parse(dtrjl.Rows[0]["0_4"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["0_5"] == System.DBNull.Value ? 8886 : double.Parse(dtrjl.Rows[0]["0_5"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["0_6"] == System.DBNull.Value ? 8887 : double.Parse(dtrjl.Rows[0]["0_6"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["0_65"] == System.DBNull.Value ? 8888 : double.Parse(dtrjl.Rows[0]["0_65"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["0_7"] == System.DBNull.Value ? 8889 : double.Parse(dtrjl.Rows[0]["0_7"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["0_8"] == System.DBNull.Value ? 88810 : double.Parse(dtrjl.Rows[0]["0_8"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["0_9"] == System.DBNull.Value ? 88811 : double.Parse(dtrjl.Rows[0]["0_9"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["1"] == System.DBNull.Value ? 88812 : double.Parse(dtrjl.Rows[0]["1"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["1_1"] == System.DBNull.Value ? 88813 : double.Parse(dtrjl.Rows[0]["1_1"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["1_2"] == System.DBNull.Value ? 88814 : double.Parse(dtrjl.Rows[0]["1_2"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["1_25"] == System.DBNull.Value ? 88815 : double.Parse(dtrjl.Rows[0]["1_25"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["1_3"] == System.DBNull.Value ? 88816 : double.Parse(dtrjl.Rows[0]["1_3"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["1_4"] == System.DBNull.Value ? 88817 : double.Parse(dtrjl.Rows[0]["1_4"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["1_5"] == System.DBNull.Value ? 88818 : double.Parse(dtrjl.Rows[0]["1_5"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["1_58"] == System.DBNull.Value ? 88819 : double.Parse(dtrjl.Rows[0]["1_58"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["1_6"] == System.DBNull.Value ? 88820 : double.Parse(dtrjl.Rows[0]["1_6"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["1_63"] == System.DBNull.Value ? 88821 : double.Parse(dtrjl.Rows[0]["1_63"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["1_7"] == System.DBNull.Value ? 88822 : double.Parse(dtrjl.Rows[0]["1_7"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["1_8"] == System.DBNull.Value ? 88823 : double.Parse(dtrjl.Rows[0]["1_8"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["1_9"] == System.DBNull.Value ? 88824 : double.Parse(dtrjl.Rows[0]["1_9"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["2"] == System.DBNull.Value ? 88825 : double.Parse(dtrjl.Rows[0]["2"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["2_1"] == System.DBNull.Value ? 88826 : double.Parse(dtrjl.Rows[0]["2_1"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["2_2"] == System.DBNull.Value ? 88827 : double.Parse(dtrjl.Rows[0]["2_2"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["2_3"] == System.DBNull.Value ? 88828 : double.Parse(dtrjl.Rows[0]["2_3"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["2_4"] == System.DBNull.Value ? 88829 : double.Parse(dtrjl.Rows[0]["2_4"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["2_5"] == System.DBNull.Value ? 88830 : double.Parse(dtrjl.Rows[0]["2_5"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["2_6"] == System.DBNull.Value ? 88831 : double.Parse(dtrjl.Rows[0]["2_6"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["2_7"] == System.DBNull.Value ? 88832 : double.Parse(dtrjl.Rows[0]["2_7"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["2_8"] == System.DBNull.Value ? 88833 : double.Parse(dtrjl.Rows[0]["2_8"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["2_9"] == System.DBNull.Value ? 88834 : double.Parse(dtrjl.Rows[0]["2_9"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["3"] == System.DBNull.Value ? 88835 : double.Parse(dtrjl.Rows[0]["3"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["3_1"] == System.DBNull.Value ? 88836 : double.Parse(dtrjl.Rows[0]["3_1"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["3_2"] == System.DBNull.Value ? 88837 : double.Parse(dtrjl.Rows[0]["3_2"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["3_3"] == System.DBNull.Value ? 88838 : double.Parse(dtrjl.Rows[0]["3_3"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["3_4"] == System.DBNull.Value ? 88839 : double.Parse(dtrjl.Rows[0]["3_4"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["3_5"] == System.DBNull.Value ? 88840 : double.Parse(dtrjl.Rows[0]["3_5"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["3_6"] == System.DBNull.Value ? 88841 : double.Parse(dtrjl.Rows[0]["3_6"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["3_7"] == System.DBNull.Value ? 88842 : double.Parse(dtrjl.Rows[0]["3_7"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["3_8"] == System.DBNull.Value ? 88843 : double.Parse(dtrjl.Rows[0]["3_8"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["3_9"] == System.DBNull.Value ? 88844 : double.Parse(dtrjl.Rows[0]["3_9"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["4"] == System.DBNull.Value ? 88845 : double.Parse(dtrjl.Rows[0]["4"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["4_1"] == System.DBNull.Value ? 88846 : double.Parse(dtrjl.Rows[0]["4_1"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["4_2"] == System.DBNull.Value ? 88847 : double.Parse(dtrjl.Rows[0]["4_2"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["4_3"] == System.DBNull.Value ? 88848 : double.Parse(dtrjl.Rows[0]["4_3"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["4_4"] == System.DBNull.Value ? 88849 : double.Parse(dtrjl.Rows[0]["4_4"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["4_5"] == System.DBNull.Value ? 88850 : double.Parse(dtrjl.Rows[0]["4_5"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["4_6"] == System.DBNull.Value ? 88851 : double.Parse(dtrjl.Rows[0]["4_6"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["4_7"] == System.DBNull.Value ? 88852 : double.Parse(dtrjl.Rows[0]["4_7"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["4_8"] == System.DBNull.Value ? 88853 : double.Parse(dtrjl.Rows[0]["4_8"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["4_9"] == System.DBNull.Value ? 88854 : double.Parse(dtrjl.Rows[0]["4_9"].ToString())) : 999);
            ydcol.Rjl.Add(dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["5"] == System.DBNull.Value ? 88855 : double.Parse(dtrjl.Rows[0]["5"].ToString())) : 999);

            ydcol.Jzmj.Add(dtjzmj.Rows.Count == 1 ? (dtjzmj.Rows[0]["小于60平方米"] == System.DBNull.Value ? 8881 : double.Parse(dtjzmj.Rows[0]["小于60平方米"].ToString())) : 999);
            ydcol.Jzmj.Add(dtjzmj.Rows.Count == 1 ? (dtjzmj.Rows[0]["60~80平方米"] == System.DBNull.Value ? 8882 : double.Parse(dtjzmj.Rows[0]["60~80平方米"].ToString())) : 999);
            ydcol.Jzmj.Add(dtjzmj.Rows.Count == 1 ? (dtjzmj.Rows[0]["80~100平方米"] == System.DBNull.Value ? 8883 : double.Parse(dtjzmj.Rows[0]["80~100平方米"].ToString())) : 999);
            ydcol.Jzmj.Add(dtjzmj.Rows.Count == 1 ? (dtjzmj.Rows[0]["100~120平方米"] == System.DBNull.Value ? 8884 : double.Parse(dtjzmj.Rows[0]["100~120平方米"].ToString())) : 999);
            ydcol.Jzmj.Add(dtjzmj.Rows.Count == 1 ? (dtjzmj.Rows[0]["120~140平方米"] == System.DBNull.Value ? 8885 : double.Parse(dtjzmj.Rows[0]["120~140平方米"].ToString())) : 999);
            ydcol.Jzmj.Add(dtjzmj.Rows.Count == 1 ? (dtjzmj.Rows[0]["140~160平方米"] == System.DBNull.Value ? 8886 : double.Parse(dtjzmj.Rows[0]["140~160平方米"].ToString())) : 999);
            ydcol.Jzmj.Add(dtjzmj.Rows.Count == 1 ? (dtjzmj.Rows[0]["160~200平方米"] == System.DBNull.Value ? 8887 : double.Parse(dtjzmj.Rows[0]["160~200平方米"].ToString())) : 999);
            ydcol.Jzmj.Add(dtjzmj.Rows.Count == 1 ? (dtjzmj.Rows[0]["200~250平方米"] == System.DBNull.Value ? 8888 : double.Parse(dtjzmj.Rows[0]["200~250平方米"].ToString())) : 999);
            ydcol.Jzmj.Add(dtjzmj.Rows.Count == 1 ? (dtjzmj.Rows[0]["250平方米以上"] == System.DBNull.Value ? 8889 : double.Parse(dtjzmj.Rows[0]["250平方米以上"].ToString())) : 999);

            ydcol.Lx.Add(dtlx.Rows.Count == 1 ? (dtlx.Rows[0]["1梯1户及2户"] == System.DBNull.Value ? 8881 : double.Parse(dtlx.Rows[0]["1梯1户及2户"].ToString())) : 999);
            ydcol.Lx.Add(dtlx.Rows.Count == 1 ? (dtlx.Rows[0]["1梯3户及以上"] == System.DBNull.Value ? 8882 : double.Parse(dtlx.Rows[0]["1梯3户及以上"].ToString())) : 999);

            ydcol.Gt.Add(dtgt.Rows.Count == 1 ? (dtgt.Rows[0]["电梯房含公摊"] == System.DBNull.Value ? 8881 : double.Parse(dtgt.Rows[0]["电梯房含公摊"].ToString())) : 999);
            ydcol.Gt.Add(dtgt.Rows.Count == 1 ? (dtgt.Rows[0]["电梯房不含公摊"] == System.DBNull.Value ? 8882 : double.Parse(dtgt.Rows[0]["电梯房不含公摊"].ToString())) : 999);
            ydcol.Gt.Add(dtgt.Rows.Count == 1 ? (dtgt.Rows[0]["非电梯房含公摊"] == System.DBNull.Value ? 8883 : double.Parse(dtgt.Rows[0]["非电梯房含公摊"].ToString())) : 999);
            ydcol.Gt.Add(dtgt.Rows.Count == 1 ? (dtgt.Rows[0]["非电梯房不含公摊"] == System.DBNull.Value ? 8884 : double.Parse(dtgt.Rows[0]["非电梯房不含公摊"].ToString())) : 999);

            ydcol.Wy.Add(dtwy.Rows.Count == 1 ? (dtwy.Rows[0]["有物业管理"] == System.DBNull.Value ? 8881 : double.Parse(dtwy.Rows[0]["有物业管理"].ToString())) : 999);
            ydcol.Wy.Add(dtwy.Rows.Count == 1 ? (dtwy.Rows[0]["无物业管理"] == System.DBNull.Value ? 8882 : double.Parse(dtwy.Rows[0]["无物业管理"].ToString())) : 999);

            ydcol.Fs.Add(dtfs.Rows.Count == 1 ? (dtfs.Rows[0]["复式"] == System.DBNull.Value ? 8881 : double.Parse(dtfs.Rows[0]["复式"].ToString())) : 999);
            ydcol.Fs.Add(dtfs.Rows.Count == 1 ? (dtfs.Rows[0]["不是复式"] == System.DBNull.Value ? 8882 : double.Parse(dtfs.Rows[0]["不是复式"].ToString())) : 999);

            ydcol.Dt.Add(dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["1楼"] == System.DBNull.Value ? 8881 : double.Parse(dtdt.Rows[0]["1楼"].ToString())) : 999);
            ydcol.Dt.Add(dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["2楼"] == System.DBNull.Value ? 8882 : double.Parse(dtdt.Rows[0]["2楼"].ToString())) : 999);
            ydcol.Dt.Add(dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["3楼"] == System.DBNull.Value ? 8883 : double.Parse(dtdt.Rows[0]["3楼"].ToString())) : 999);
            ydcol.Dt.Add(dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["4楼"] == System.DBNull.Value ? 8884 : double.Parse(dtdt.Rows[0]["4楼"].ToString())) : 999);
            ydcol.Dt.Add(dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["5楼"] == System.DBNull.Value ? 8885 : double.Parse(dtdt.Rows[0]["5楼"].ToString())) : 999);
            ydcol.Dt.Add(dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["6楼"] == System.DBNull.Value ? 8886 : double.Parse(dtdt.Rows[0]["6楼"].ToString())) : 999);
            ydcol.Dt.Add(dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["7楼"] == System.DBNull.Value ? 8887 : double.Parse(dtdt.Rows[0]["7楼"].ToString())) : 999);
            ydcol.Dt.Add(dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["8楼"] == System.DBNull.Value ? 8888 : double.Parse(dtdt.Rows[0]["8楼"].ToString())) : 999);
            ydcol.Dt.Add(dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["9楼"] == System.DBNull.Value ? 8889 : double.Parse(dtdt.Rows[0]["9楼"].ToString())) : 999);
            ydcol.Dt.Add(dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["10楼"] == System.DBNull.Value ? 88810 : double.Parse(dtdt.Rows[0]["10楼"].ToString())) : 999);
            ydcol.Dt.Add(dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["11楼"] == System.DBNull.Value ? 88811 : double.Parse(dtdt.Rows[0]["11楼"].ToString())) : 999);
            ydcol.Dt.Add(dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["12楼"] == System.DBNull.Value ? 88812 : double.Parse(dtdt.Rows[0]["12楼"].ToString())) : 999);
            ydcol.Dt.Add(dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["13楼"] == System.DBNull.Value ? 88813 : double.Parse(dtdt.Rows[0]["13楼"].ToString())) : 999);
            ydcol.Dt.Add(dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["14楼"] == System.DBNull.Value ? 88814 : double.Parse(dtdt.Rows[0]["14楼"].ToString())) : 999);
            ydcol.Dt.Add(dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["15楼"] == System.DBNull.Value ? 88815 : double.Parse(dtdt.Rows[0]["15楼"].ToString())) : 999);
            ydcol.Dt.Add(dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["16楼"] == System.DBNull.Value ? 88816 : double.Parse(dtdt.Rows[0]["16楼"].ToString())) : 999);
            ydcol.Dt.Add(dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["17楼"] == System.DBNull.Value ? 88817 : double.Parse(dtdt.Rows[0]["17楼"].ToString())) : 999);
            ydcol.Dt.Add(dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["18楼"] == System.DBNull.Value ? 88818 : double.Parse(dtdt.Rows[0]["18楼"].ToString())) : 999);
            ydcol.Dt.Add(dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["19楼"] == System.DBNull.Value ? 88819 : double.Parse(dtdt.Rows[0]["19楼"].ToString())) : 999);
            ydcol.Dt.Add(dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["20楼"] == System.DBNull.Value ? 88820 : double.Parse(dtdt.Rows[0]["20楼"].ToString())) : 999);
            ydcol.Dt.Add(dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["21楼"] == System.DBNull.Value ? 88821 : double.Parse(dtdt.Rows[0]["21楼"].ToString())) : 999);
            ydcol.Dt.Add(dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["22楼"] == System.DBNull.Value ? 88822 : double.Parse(dtdt.Rows[0]["22楼"].ToString())) : 999);
            ydcol.Dt.Add(dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["23楼"] == System.DBNull.Value ? 88823 : double.Parse(dtdt.Rows[0]["23楼"].ToString())) : 999);
            ydcol.Dt.Add(dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["24楼"] == System.DBNull.Value ? 88824 : double.Parse(dtdt.Rows[0]["24楼"].ToString())) : 999);
            ydcol.Dt.Add(dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["25楼"] == System.DBNull.Value ? 88825 : double.Parse(dtdt.Rows[0]["25楼"].ToString())) : 999);
            ydcol.Dt.Add(dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["26楼"] == System.DBNull.Value ? 88826 : double.Parse(dtdt.Rows[0]["26楼"].ToString())) : 999);
            ydcol.Dt.Add(dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["27楼"] == System.DBNull.Value ? 88827 : double.Parse(dtdt.Rows[0]["27楼"].ToString())) : 999);
            ydcol.Dt.Add(dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["28楼"] == System.DBNull.Value ? 88828 : double.Parse(dtdt.Rows[0]["28楼"].ToString())) : 999);
            ydcol.Dt.Add(dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["29楼"] == System.DBNull.Value ? 88829 : double.Parse(dtdt.Rows[0]["29楼"].ToString())) : 999);
            ydcol.Dt.Add(dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["30楼"] == System.DBNull.Value ? 88830 : double.Parse(dtdt.Rows[0]["30楼"].ToString())) : 999);
            ydcol.Dt.Add(dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["31楼"] == System.DBNull.Value ? 88831 : double.Parse(dtdt.Rows[0]["31楼"].ToString())) : 999);
            ydcol.Dt.Add(dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["32楼"] == System.DBNull.Value ? 88832 : double.Parse(dtdt.Rows[0]["32楼"].ToString())) : 999);
            ydcol.Dt.Add(dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["33楼"] == System.DBNull.Value ? 88833 : double.Parse(dtdt.Rows[0]["33楼"].ToString())) : 999);
            ydcol.Dt.Add(dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["34楼"] == System.DBNull.Value ? 88834 : double.Parse(dtdt.Rows[0]["34楼"].ToString())) : 999);
            ydcol.Dt.Add(dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["35楼"] == System.DBNull.Value ? 88835 : double.Parse(dtdt.Rows[0]["35楼"].ToString())) : 999);
            ydcol.Dt.Add(dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["36楼"] == System.DBNull.Value ? 88836 : double.Parse(dtdt.Rows[0]["36楼"].ToString())) : 999);

            ydcol.Fdt.Add(dtfdt.Rows.Count == 1 ? (dtfdt.Rows[0]["1楼"] == System.DBNull.Value ? 8881 : double.Parse(dtfdt.Rows[0]["1楼"].ToString())) : 999);
            ydcol.Fdt.Add(dtfdt.Rows.Count == 1 ? (dtfdt.Rows[0]["2楼"] == System.DBNull.Value ? 8882 : double.Parse(dtfdt.Rows[0]["2楼"].ToString())) : 999);
            ydcol.Fdt.Add(dtfdt.Rows.Count == 1 ? (dtfdt.Rows[0]["3楼"] == System.DBNull.Value ? 8883 : double.Parse(dtfdt.Rows[0]["3楼"].ToString())) : 999);
            ydcol.Fdt.Add(dtfdt.Rows.Count == 1 ? (dtfdt.Rows[0]["4楼"] == System.DBNull.Value ? 8884 : double.Parse(dtfdt.Rows[0]["4楼"].ToString())) : 999);
            ydcol.Fdt.Add(dtfdt.Rows.Count == 1 ? (dtfdt.Rows[0]["5楼"] == System.DBNull.Value ? 8885 : double.Parse(dtfdt.Rows[0]["5楼"].ToString())) : 999);
            ydcol.Fdt.Add(dtfdt.Rows.Count == 1 ? (dtfdt.Rows[0]["6楼"] == System.DBNull.Value ? 8886 : double.Parse(dtfdt.Rows[0]["6楼"].ToString())) : 999);
            ydcol.Fdt.Add(dtfdt.Rows.Count == 1 ? (dtfdt.Rows[0]["7楼"] == System.DBNull.Value ? 8887 : double.Parse(dtfdt.Rows[0]["7楼"].ToString())) : 999);
            ydcol.Fdt.Add(dtfdt.Rows.Count == 1 ? (dtfdt.Rows[0]["8楼"] == System.DBNull.Value ? 8888 : double.Parse(dtfdt.Rows[0]["8楼"].ToString())) : 999);
            ydcol.Fdt.Add(dtfdt.Rows.Count == 1 ? (dtfdt.Rows[0]["9楼"] == System.DBNull.Value ? 8889 : double.Parse(dtfdt.Rows[0]["9楼"].ToString())) : 999);
            ydcol.Fdt.Add(dtfdt.Rows.Count == 1 ? (dtfdt.Rows[0]["10楼"] == System.DBNull.Value ? 88810 : double.Parse(dtfdt.Rows[0]["10楼"].ToString())) : 999);
            ydcol.Fdt.Add(dtfdt.Rows.Count == 1 ? (dtfdt.Rows[0]["11楼"] == System.DBNull.Value ? 88811 : double.Parse(dtfdt.Rows[0]["11楼"].ToString())) : 999);
            ydcol.Fdt.Add(dtfdt.Rows.Count == 1 ? (dtfdt.Rows[0]["12楼"] == System.DBNull.Value ? 88812 : double.Parse(dtfdt.Rows[0]["12楼"].ToString())) : 999);
            ydcol.Fdt.Add(dtfdt.Rows.Count == 1 ? (dtfdt.Rows[0]["13楼"] == System.DBNull.Value ? 88813 : double.Parse(dtfdt.Rows[0]["13楼"].ToString())) : 999);
            ydcol.Fdt.Add(dtfdt.Rows.Count == 1 ? (dtfdt.Rows[0]["14楼"] == System.DBNull.Value ? 88814 : double.Parse(dtfdt.Rows[0]["14楼"].ToString())) : 999);
            ydcol.Fdt.Add(dtfdt.Rows.Count == 1 ? (dtfdt.Rows[0]["15楼"] == System.DBNull.Value ? 88815 : double.Parse(dtfdt.Rows[0]["15楼"].ToString())) : 999);

            ydcol.Dtsz.Add(dtsz.Rows.Count == 1 ? (dtsz.Rows[0]["有电梯"] == System.DBNull.Value ? 8881 : double.Parse(dtsz.Rows[0]["有电梯"].ToString())) : 999);
            ydcol.Dtsz.Add(dtsz.Rows.Count == 1 ? (dtsz.Rows[0]["无电梯"] == System.DBNull.Value ? 8882 : double.Parse(dtsz.Rows[0]["无电梯"].ToString())) : 999);
            #endregion
            fjxml = new XElement("fj",
            #region jglx
 new XElement("jglx", jglxsz.Trim()),
                new XElement("jglxxz", ""),
                new XElement("jglxgj", dtjglx.Rows.Count == 1 ? (dtjglx.Rows[0]["钢筋混凝土"] == System.DBNull.Value ? "8881" : dtjglx.Rows[0]["钢筋混凝土"]) : 999),
                new XElement("jglxhh", dtjglx.Rows.Count == 1 ? (dtjglx.Rows[0]["混合"] == System.DBNull.Value ? "8882" : dtjglx.Rows[0]["混合"]) : 999),
                new XElement("jglxzm", dtjglx.Rows.Count == 1 ? (dtjglx.Rows[0]["砖木"] == System.DBNull.Value ? "8883" : dtjglx.Rows[0]["砖木"]) : 999),
                new XElement("jglxqt", dtjglx.Rows.Count == 1 ? (dtjglx.Rows[0]["其他"] == System.DBNull.Value ? "8884" : dtjglx.Rows[0]["其他"]) : 999),
            #endregion
            #region cx
 new XElement("cx", cxsz.Trim()),
                new XElement("cxxz", ""),
                new XElement("cxd", dtcx.Rows.Count == 1 ? (dtcx.Rows[0]["东"] == System.DBNull.Value ? "8881" : dtcx.Rows[0]["东"]) : 999),
                new XElement("cxdn", dtcx.Rows.Count == 1 ? (dtcx.Rows[0]["东南"] == System.DBNull.Value ? "8882" : dtcx.Rows[0]["东南"]) : 999),
                new XElement("cxn", dtcx.Rows.Count == 1 ? (dtcx.Rows[0]["南"] == System.DBNull.Value ? "8883" : dtcx.Rows[0]["南"]) : 999),
                new XElement("cxxn", dtcx.Rows.Count == 1 ? (dtcx.Rows[0]["西南"] == System.DBNull.Value ? "8884" : dtcx.Rows[0]["西南"]) : 999),
                new XElement("cxx", dtcx.Rows.Count == 1 ? (dtcx.Rows[0]["西"] == System.DBNull.Value ? "8885" : dtcx.Rows[0]["西"]) : 999),
                new XElement("cxxb", dtcx.Rows.Count == 1 ? (dtcx.Rows[0]["西北"] == System.DBNull.Value ? "8886" : dtcx.Rows[0]["西北"]) : 999),
                new XElement("cxb", dtcx.Rows.Count == 1 ? (dtcx.Rows[0]["北"] == System.DBNull.Value ? "8887" : dtcx.Rows[0]["北"]) : 999),
                new XElement("cxdb", dtcx.Rows.Count == 1 ? (dtcx.Rows[0]["东北"] == System.DBNull.Value ? "8888" : dtcx.Rows[0]["东北"]) : 999),
            #endregion
            #region ll
 new XElement("ll", llsz),
                new XElement("llxz", ""),
                new XElement("ll1", dtll.Rows.Count == 1 ? (dtll.Rows[0]["1年"] == System.DBNull.Value ? "8881" : dtll.Rows[0]["1年"]) : 999),
                new XElement("ll2", dtll.Rows.Count == 1 ? (dtll.Rows[0]["2年"] == System.DBNull.Value ? "8882" : dtll.Rows[0]["2年"]) : 999),
                new XElement("ll3", dtll.Rows.Count == 1 ? (dtll.Rows[0]["3年"] == System.DBNull.Value ? "8883" : dtll.Rows[0]["3年"]) : 999),
                new XElement("ll4", dtll.Rows.Count == 1 ? (dtll.Rows[0]["4年"] == System.DBNull.Value ? "8884" : dtll.Rows[0]["4年"]) : 999),
                new XElement("ll5", dtll.Rows.Count == 1 ? (dtll.Rows[0]["5年"] == System.DBNull.Value ? "8885" : dtll.Rows[0]["5年"]) : 999),
                new XElement("ll6", dtll.Rows.Count == 1 ? (dtll.Rows[0]["6年"] == System.DBNull.Value ? "8886" : dtll.Rows[0]["6年"]) : 999),
                new XElement("ll7", dtll.Rows.Count == 1 ? (dtll.Rows[0]["7年"] == System.DBNull.Value ? "8887" : dtll.Rows[0]["7年"]) : 999),
                new XElement("ll8", dtll.Rows.Count == 1 ? (dtll.Rows[0]["8年"] == System.DBNull.Value ? "8888" : dtll.Rows[0]["8年"]) : 999),
                new XElement("ll10", dtll.Rows.Count == 1 ? (dtll.Rows[0]["10年"] == System.DBNull.Value ? "8889" : dtll.Rows[0]["10年"]) : 999),
                new XElement("ll12", dtll.Rows.Count == 1 ? (dtll.Rows[0]["12年"] == System.DBNull.Value ? "88810" : dtll.Rows[0]["12年"]) : 999),
                new XElement("ll13", dtll.Rows.Count == 1 ? (dtll.Rows[0]["13年"] == System.DBNull.Value ? "88811" : dtll.Rows[0]["13年"]) : 999),
                new XElement("ll15", dtll.Rows.Count == 1 ? (dtll.Rows[0]["15年"] == System.DBNull.Value ? "88812" : dtll.Rows[0]["15年"]) : 999),
                new XElement("ll18", dtll.Rows.Count == 1 ? (dtll.Rows[0]["18年"] == System.DBNull.Value ? "88813" : dtll.Rows[0]["18年"]) : 999),
                new XElement("ll20", dtll.Rows.Count == 1 ? (dtll.Rows[0]["20年"] == System.DBNull.Value ? "88814" : dtll.Rows[0]["20年"]) : 999),
                new XElement("ll22", dtll.Rows.Count == 1 ? (dtll.Rows[0]["22年"] == System.DBNull.Value ? "88815" : dtll.Rows[0]["22年"]) : 999),
                new XElement("ll25", dtll.Rows.Count == 1 ? (dtll.Rows[0]["25年"] == System.DBNull.Value ? "88816" : dtll.Rows[0]["25年"]) : 999),
                new XElement("ll26", dtll.Rows.Count == 1 ? (dtll.Rows[0]["26年"] == System.DBNull.Value ? "88817" : dtll.Rows[0]["26年"]) : 999),
                new XElement("ll30", dtll.Rows.Count == 1 ? (dtll.Rows[0]["30年"] == System.DBNull.Value ? "88818" : dtll.Rows[0]["30年"]) : 999),
                new XElement("ll35", dtll.Rows.Count == 1 ? (dtll.Rows[0]["35年"] == System.DBNull.Value ? "88819" : dtll.Rows[0]["35年"]) : 999),
                new XElement("ll40", dtll.Rows.Count == 1 ? (dtll.Rows[0]["40年"] == System.DBNull.Value ? "88820" : dtll.Rows[0]["40年"]) : 999),
                new XElement("ll45", dtll.Rows.Count == 1 ? (dtll.Rows[0]["45年"] == System.DBNull.Value ? "88821" : dtll.Rows[0]["45年"]) : 999),
                new XElement("ll50", dtll.Rows.Count == 1 ? (dtll.Rows[0]["50年"] == System.DBNull.Value ? "88822" : dtll.Rows[0]["50年"]) : 999),
                new XElement("ll55", dtll.Rows.Count == 1 ? (dtll.Rows[0]["55年"] == System.DBNull.Value ? "88823" : dtll.Rows[0]["55年"]) : 999),
                new XElement("ll60", dtll.Rows.Count == 1 ? (dtll.Rows[0]["60年"] == System.DBNull.Value ? "88824" : dtll.Rows[0]["60年"]) : 999),
                new XElement("ll65", dtll.Rows.Count == 1 ? (dtll.Rows[0]["65年"] == System.DBNull.Value ? "88825" : dtll.Rows[0]["65年"]) : 999),
                new XElement("ll70", dtll.Rows.Count == 1 ? (dtll.Rows[0]["70年"] == System.DBNull.Value ? "88826" : dtll.Rows[0]["70年"]) : 999),
            #endregion
            #region llqk
 new XElement("llqk", lnqksz.Trim()),
                new XElement("llqkxz", ""),
                new XElement("llqkzyjtgd", dtllqk.Rows.Count == 1 ? (dtllqk.Rows[0]["临主要交通干道"] == System.DBNull.Value ? "8881" : dtllqk.Rows[0]["临主要交通干道"]) : 999),
                new XElement("llqkybjtgd", dtllqk.Rows.Count == 1 ? (dtllqk.Rows[0]["临一般交通干道"] == System.DBNull.Value ? "8882" : dtllqk.Rows[0]["临一般交通干道"]) : 999),
                new XElement("llqkbnjtgd", dtllqk.Rows.Count == 1 ? (dtllqk.Rows[0]["不临交通干道"] == System.DBNull.Value ? "8883" : dtllqk.Rows[0]["不临交通干道"]) : 999),
                new XElement("llqknzl", dtllqk.Rows.Count == 1 ? (dtllqk.Rows[0]["临支路"] == System.DBNull.Value ? "8884" : dtllqk.Rows[0]["临支路"]) : 999),
                new XElement("llqknxqjtgd", dtllqk.Rows.Count == 1 ? (dtllqk.Rows[0]["临小区交通干道"] == System.DBNull.Value ? "8885" : dtllqk.Rows[0]["临小区交通干道"]) : 999),
            #endregion
            #region jt
 new XElement("jt", jtsz.Trim()),
                new XElement("jtxz", ""),
                new XElement("jtbtmtc", dtjt.Rows.Count == 1 ? (dtjt.Rows[0]["不能通摩托车"] == System.DBNull.Value ? "8881" : dtjt.Rows[0]["不能通摩托车"]) : 999),
                new XElement("jtktmtc", dtjt.Rows.Count == 1 ? (dtjt.Rows[0]["可同摩托车，不同小汽车"] == System.DBNull.Value ? "8882" : dtjt.Rows[0]["可同摩托车，不同小汽车"]) : 999),
                new XElement("jtktxc1", dtjt.Rows.Count == 1 ? (dtjt.Rows[0]["可通1小车"] == System.DBNull.Value ? "8883" : dtjt.Rows[0]["可通1小车"]) : 999),
                new XElement("jtktxc2", dtjt.Rows.Count == 1 ? (dtjt.Rows[0]["可通2小车"] == System.DBNull.Value ? "8884" : dtjt.Rows[0]["可通2小车"]) : 999),
                new XElement("jtktxc3", dtjt.Rows.Count == 1 ? (dtjt.Rows[0]["可通3小车"] == System.DBNull.Value ? "8885" : dtjt.Rows[0]["可通3小车"]) : 999),
            #endregion
            #region rjl
 new XElement("rjl", rjlsz),
                new XElement("rjlxz", ""),
                new XElement("rjl1", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["0_1"] == System.DBNull.Value ? "8881" : dtrjl.Rows[0]["0_1"]) : 999),
                new XElement("rjl2", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["0_2"] == System.DBNull.Value ? "8882" : dtrjl.Rows[0]["0_2"]) : 999),
                new XElement("rjl3", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["0_3"] == System.DBNull.Value ? "8883" : dtrjl.Rows[0]["0_3"]) : 999),
                new XElement("rjl4", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["0_35"] == System.DBNull.Value ? "8884" : dtrjl.Rows[0]["0_35"]) : 999),
                new XElement("rjl5", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["0_4"] == System.DBNull.Value ? "8885" : dtrjl.Rows[0]["0_4"]) : 999),
                new XElement("rjl6", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["0_5"] == System.DBNull.Value ? "8886" : dtrjl.Rows[0]["0_5"]) : 999),
                new XElement("rjl7", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["0_6"] == System.DBNull.Value ? "8887" : dtrjl.Rows[0]["0_6"]) : 999),
                new XElement("rjl8", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["0_65"] == System.DBNull.Value ? "8888" : dtrjl.Rows[0]["0_65"]) : 999),
                new XElement("rjl9", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["0_7"] == System.DBNull.Value ? "8889" : dtrjl.Rows[0]["0_7"]) : 999),
                new XElement("rjl10", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["0_8"] == System.DBNull.Value ? "88810" : dtrjl.Rows[0]["0_8"]) : 999),
                new XElement("rjl11", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["0_9"] == System.DBNull.Value ? "88811" : dtrjl.Rows[0]["0_9"]) : 999),
                new XElement("rjl12", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["1"] == System.DBNull.Value ? "88812" : dtrjl.Rows[0]["1"]) : 999),
                new XElement("rjl13", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["1_1"] == System.DBNull.Value ? "88813" : dtrjl.Rows[0]["1_1"]) : 999),
                new XElement("rjl14", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["1_2"] == System.DBNull.Value ? "88814" : dtrjl.Rows[0]["1_2"]) : 999),
                new XElement("rjl15", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["1_25"] == System.DBNull.Value ? "88815" : dtrjl.Rows[0]["1_25"]) : 999),
                new XElement("rjl16", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["1_3"] == System.DBNull.Value ? "88816" : dtrjl.Rows[0]["1_3"]) : 999),
                new XElement("rjl17", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["1_4"] == System.DBNull.Value ? "88817" : dtrjl.Rows[0]["1_4"]) : 999),
                new XElement("rjl18", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["1_5"] == System.DBNull.Value ? "88818" : dtrjl.Rows[0]["1_5"]) : 999),
                new XElement("rjl19", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["1_58"] == System.DBNull.Value ? "88819" : dtrjl.Rows[0]["1_58"]) : 999),
                new XElement("rjl20", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["1_6"] == System.DBNull.Value ? "88820" : dtrjl.Rows[0]["1_6"]) : 999),
                new XElement("rjl21", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["1_63"] == System.DBNull.Value ? "88821" : dtrjl.Rows[0]["1_63"]) : 999),
                new XElement("rjl22", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["1_7"] == System.DBNull.Value ? "88822" : dtrjl.Rows[0]["1_7"]) : 999),
                new XElement("rjl23", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["1_8"] == System.DBNull.Value ? "88823" : dtrjl.Rows[0]["1_8"]) : 999),
                new XElement("rjl24", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["1_9"] == System.DBNull.Value ? "88824" : dtrjl.Rows[0]["1_9"]) : 999),
                new XElement("rjl25", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["2"] == System.DBNull.Value ? "88825" : dtrjl.Rows[0]["2"]) : 999),
                new XElement("rjl26", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["2_1"] == System.DBNull.Value ? "88826" : dtrjl.Rows[0]["2_1"]) : 999),
                new XElement("rjl27", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["2_2"] == System.DBNull.Value ? "88827" : dtrjl.Rows[0]["2_2"]) : 999),
                new XElement("rjl28", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["2_3"] == System.DBNull.Value ? "88828" : dtrjl.Rows[0]["2_3"]) : 999),
                new XElement("rjl29", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["2_4"] == System.DBNull.Value ? "88829" : dtrjl.Rows[0]["2_4"]) : 999),
                new XElement("rjl30", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["2_5"] == System.DBNull.Value ? "88830" : dtrjl.Rows[0]["2_5"]) : 999),
                new XElement("rjl31", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["2_6"] == System.DBNull.Value ? "88831" : dtrjl.Rows[0]["2_6"]) : 999),
                new XElement("rjl32", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["2_7"] == System.DBNull.Value ? "88832" : dtrjl.Rows[0]["2_7"]) : 999),
                new XElement("rjl33", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["2_8"] == System.DBNull.Value ? "88833" : dtrjl.Rows[0]["2_8"]) : 999),
                new XElement("rjl34", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["2_9"] == System.DBNull.Value ? "88834" : dtrjl.Rows[0]["2_9"]) : 999),
                new XElement("rjl35", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["3"] == System.DBNull.Value ? "88835" : dtrjl.Rows[0]["3"]) : 999),
                new XElement("rjl36", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["3_1"] == System.DBNull.Value ? "88836" : dtrjl.Rows[0]["3_1"]) : 999),
                new XElement("rjl37", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["3_2"] == System.DBNull.Value ? "88837" : dtrjl.Rows[0]["3_2"]) : 999),
                new XElement("rjl38", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["3_3"] == System.DBNull.Value ? "88838" : dtrjl.Rows[0]["3_3"]) : 999),
                new XElement("rjl39", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["3_4"] == System.DBNull.Value ? "88839" : dtrjl.Rows[0]["3_4"]) : 999),
                new XElement("rjl40", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["3_5"] == System.DBNull.Value ? "88840" : dtrjl.Rows[0]["3_5"]) : 999),
                new XElement("rjl41", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["3_6"] == System.DBNull.Value ? "88841" : dtrjl.Rows[0]["3_6"]) : 999),
                new XElement("rjl42", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["3_7"] == System.DBNull.Value ? "88842" : dtrjl.Rows[0]["3_7"]) : 999),
                new XElement("rjl43", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["3_8"] == System.DBNull.Value ? "88843" : dtrjl.Rows[0]["3_8"]) : 999),
                new XElement("rjl44", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["3_9"] == System.DBNull.Value ? "88844" : dtrjl.Rows[0]["3_9"]) : 999),
                new XElement("rjl45", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["4"] == System.DBNull.Value ? "88845" : dtrjl.Rows[0]["4"]) : 999),
                new XElement("rjl46", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["4_1"] == System.DBNull.Value ? "88846" : dtrjl.Rows[0]["4_1"]) : 999),
                new XElement("rjl47", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["4_2"] == System.DBNull.Value ? "88847" : dtrjl.Rows[0]["4_2"]) : 999),
                new XElement("rjl48", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["4_3"] == System.DBNull.Value ? "88848" : dtrjl.Rows[0]["4_3"]) : 999),
                new XElement("rjl49", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["4_4"] == System.DBNull.Value ? "88849" : dtrjl.Rows[0]["4_4"]) : 999),
                new XElement("rjl50", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["4_5"] == System.DBNull.Value ? "88850" : dtrjl.Rows[0]["4_5"]) : 999),
                new XElement("rjl51", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["4_6"] == System.DBNull.Value ? "88851" : dtrjl.Rows[0]["4_6"]) : 999),
                new XElement("rjl52", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["4_7"] == System.DBNull.Value ? "88852" : dtrjl.Rows[0]["4_7"]) : 999),
                new XElement("rjl53", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["4_8"] == System.DBNull.Value ? "88853" : dtrjl.Rows[0]["4_8"]) : 999),
                new XElement("rjl54", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["4_9"] == System.DBNull.Value ? "88854" : dtrjl.Rows[0]["4_9"]) : 999),
                new XElement("rjl55", dtrjl.Rows.Count == 1 ? (dtrjl.Rows[0]["5"] == System.DBNull.Value ? "88855" : dtrjl.Rows[0]["5"]) : 999),
            #endregion
            #region jzmj
 new XElement("jzmj", jzmj),
                new XElement("jzmjxz", ""),
                new XElement("jzmj60", dtjzmj.Rows.Count == 1 ? (dtjzmj.Rows[0]["小于60平方米"] == System.DBNull.Value ? "8881" : dtjzmj.Rows[0]["小于60平方米"]) : 999),
                new XElement("jzmj80", dtjzmj.Rows.Count == 1 ? (dtjzmj.Rows[0]["60~80平方米"] == System.DBNull.Value ? "8882" : dtjzmj.Rows[0]["60~80平方米"]) : 999),
                new XElement("jzmj100", dtjzmj.Rows.Count == 1 ? (dtjzmj.Rows[0]["80~100平方米"] == System.DBNull.Value ? "8883" : dtjzmj.Rows[0]["80~100平方米"]) : 999),
                new XElement("jzmj120", dtjzmj.Rows.Count == 1 ? (dtjzmj.Rows[0]["100~120平方米"] == System.DBNull.Value ? "8884" : dtjzmj.Rows[0]["100~120平方米"]) : 999),
                new XElement("jzmj140", dtjzmj.Rows.Count == 1 ? (dtjzmj.Rows[0]["120~140平方米"] == System.DBNull.Value ? "8885" : dtjzmj.Rows[0]["120~140平方米"]) : 999),
                new XElement("jzmj160", dtjzmj.Rows.Count == 1 ? (dtjzmj.Rows[0]["140~160平方米"] == System.DBNull.Value ? "8886" : dtjzmj.Rows[0]["140~160平方米"]) : 999),
                new XElement("jzmj200", dtjzmj.Rows.Count == 1 ? (dtjzmj.Rows[0]["160~200平方米"] == System.DBNull.Value ? "8887" : dtjzmj.Rows[0]["160~200平方米"]) : 999),
                new XElement("jzmj250", dtjzmj.Rows.Count == 1 ? (dtjzmj.Rows[0]["200~250平方米"] == System.DBNull.Value ? "8888" : dtjzmj.Rows[0]["200~250平方米"]) : 999),
                new XElement("jzmj300", dtjzmj.Rows.Count == 1 ? (dtjzmj.Rows[0]["250平方米以上"] == System.DBNull.Value ? "8889" : dtjzmj.Rows[0]["250平方米以上"]) : 999),
            #endregion
            #region lx
 new XElement("lx", lxsz.Trim()),
                new XElement("lxxz", ""),
                new XElement("lxxz1t1hj2h", dtlx.Rows.Count == 1 ? (dtlx.Rows[0]["1梯1户及2户"] == System.DBNull.Value ? "8881" : dtlx.Rows[0]["1梯1户及2户"]) : 999),
                new XElement("lx1t3hjys", dtlx.Rows.Count == 1 ? (dtlx.Rows[0]["1梯3户及以上"] == System.DBNull.Value ? "8882" : dtlx.Rows[0]["1梯3户及以上"]) : 999),
            #endregion
            #region gt
 new XElement("gt", gt.Trim()),
                new XElement("gtxz", ""),
                new XElement("gtdthgt", dtgt.Rows.Count == 1 ? (dtgt.Rows[0]["电梯房含公摊"] == System.DBNull.Value ? "8881" : dtgt.Rows[0]["电梯房含公摊"]) : 999),
                new XElement("gtdtbhgt", dtgt.Rows.Count == 1 ? (dtgt.Rows[0]["电梯房不含公摊"] == System.DBNull.Value ? "8882" : dtgt.Rows[0]["电梯房不含公摊"]) : 999),
                new XElement("gtfdthgt", dtgt.Rows.Count == 1 ? (dtgt.Rows[0]["非电梯房含公摊"] == System.DBNull.Value ? "8883" : dtgt.Rows[0]["非电梯房含公摊"]) : 999),
                new XElement("gtfdtbhgt", dtgt.Rows.Count == 1 ? (dtgt.Rows[0]["非电梯房不含公摊"] == System.DBNull.Value ? "8884" : dtgt.Rows[0]["非电梯房不含公摊"]) : 999),
            #endregion
            #region wy
 new XElement("wy", wy.Trim()),
                new XElement("wyxz", ""),
                new XElement("wyy", dtwy.Rows.Count == 1 ? (dtwy.Rows[0]["有物业管理"] == System.DBNull.Value ? "8881" : dtwy.Rows[0]["有物业管理"]) : 999),
                new XElement("wyw", dtwy.Rows.Count == 1 ? (dtwy.Rows[0]["无物业管理"] == System.DBNull.Value ? "8882" : dtwy.Rows[0]["无物业管理"]) : 999),
            #endregion
            #region fs
 new XElement("fs", fs.Trim()),
                new XElement("fsxz", ""),
                new XElement("fss", dtfs.Rows.Count == 1 ? (dtfs.Rows[0]["复式"] == System.DBNull.Value ? "8881" : dtfs.Rows[0]["复式"]) : 999),
                new XElement("fsbs", dtfs.Rows.Count == 1 ? (dtfs.Rows[0]["不是复式"] == System.DBNull.Value ? "8882" : dtfs.Rows[0]["不是复式"]) : 999),
            #endregion
            #region dt
 new XElement("dt", lcsz),
                new XElement("dtxz", ""),
                new XElement("dt1", dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["1楼"] == System.DBNull.Value ? "8881" : dtdt.Rows[0]["1楼"]) : 999),
                new XElement("dt2", dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["2楼"] == System.DBNull.Value ? "8882" : dtdt.Rows[0]["2楼"]) : 999),
                new XElement("dt3", dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["3楼"] == System.DBNull.Value ? "8883" : dtdt.Rows[0]["3楼"]) : 999),
                new XElement("dt4", dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["4楼"] == System.DBNull.Value ? "8884" : dtdt.Rows[0]["4楼"]) : 999),
                new XElement("dt5", dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["5楼"] == System.DBNull.Value ? "8885" : dtdt.Rows[0]["5楼"]) : 999),
                new XElement("dt6", dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["6楼"] == System.DBNull.Value ? "8886" : dtdt.Rows[0]["6楼"]) : 999),
                new XElement("dt7", dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["7楼"] == System.DBNull.Value ? "8887" : dtdt.Rows[0]["7楼"]) : 999),
                new XElement("dt8", dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["8楼"] == System.DBNull.Value ? "8888" : dtdt.Rows[0]["8楼"]) : 999),
                new XElement("dt9", dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["9楼"] == System.DBNull.Value ? "8889" : dtdt.Rows[0]["9楼"]) : 999),
                new XElement("dt10", dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["10楼"] == System.DBNull.Value ? "88810" : dtdt.Rows[0]["10楼"]) : 999),
                new XElement("dt11", dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["11楼"] == System.DBNull.Value ? "88811" : dtdt.Rows[0]["11楼"]) : 999),
                new XElement("dt12", dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["12楼"] == System.DBNull.Value ? "88812" : dtdt.Rows[0]["12楼"]) : 999),
                new XElement("dt13", dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["13楼"] == System.DBNull.Value ? "88813" : dtdt.Rows[0]["13楼"]) : 999),
                new XElement("dt14", dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["14楼"] == System.DBNull.Value ? "88814" : dtdt.Rows[0]["14楼"]) : 999),
                new XElement("dt15", dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["15楼"] == System.DBNull.Value ? "88815" : dtdt.Rows[0]["15楼"]) : 999),
                new XElement("dt16", dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["16楼"] == System.DBNull.Value ? "88816" : dtdt.Rows[0]["16楼"]) : 999),
                new XElement("dt17", dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["17楼"] == System.DBNull.Value ? "88817" : dtdt.Rows[0]["17楼"]) : 999),
                new XElement("dt18", dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["18楼"] == System.DBNull.Value ? "88818" : dtdt.Rows[0]["18楼"]) : 999),
                new XElement("dt19", dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["19楼"] == System.DBNull.Value ? "88819" : dtdt.Rows[0]["19楼"]) : 999),
                new XElement("dt20", dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["20楼"] == System.DBNull.Value ? "88820" : dtdt.Rows[0]["20楼"]) : 999),
                new XElement("dt21", dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["21楼"] == System.DBNull.Value ? "88821" : dtdt.Rows[0]["21楼"]) : 999),
                new XElement("dt22", dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["22楼"] == System.DBNull.Value ? "88822" : dtdt.Rows[0]["22楼"]) : 999),
                new XElement("dt23", dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["23楼"] == System.DBNull.Value ? "88823" : dtdt.Rows[0]["23楼"]) : 999),
                new XElement("dt24", dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["24楼"] == System.DBNull.Value ? "88824" : dtdt.Rows[0]["24楼"]) : 999),
                new XElement("dt25", dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["25楼"] == System.DBNull.Value ? "88825" : dtdt.Rows[0]["25楼"]) : 999),
                new XElement("dt26", dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["26楼"] == System.DBNull.Value ? "88826" : dtdt.Rows[0]["26楼"]) : 999),
                new XElement("dt27", dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["27楼"] == System.DBNull.Value ? "88827" : dtdt.Rows[0]["27楼"]) : 999),
                new XElement("dt28", dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["28楼"] == System.DBNull.Value ? "88828" : dtdt.Rows[0]["28楼"]) : 999),
                new XElement("dt29", dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["29楼"] == System.DBNull.Value ? "88829" : dtdt.Rows[0]["29楼"]) : 999),
                new XElement("dt30", dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["30楼"] == System.DBNull.Value ? "88830" : dtdt.Rows[0]["30楼"]) : 999),
                new XElement("dt31", dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["31楼"] == System.DBNull.Value ? "88831" : dtdt.Rows[0]["31楼"]) : 999),
                new XElement("dt32", dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["32楼"] == System.DBNull.Value ? "88832" : dtdt.Rows[0]["32楼"]) : 999),
                new XElement("dt33", dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["33楼"] == System.DBNull.Value ? "88833" : dtdt.Rows[0]["33楼"]) : 999),
                new XElement("dt34", dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["34楼"] == System.DBNull.Value ? "88834" : dtdt.Rows[0]["34楼"]) : 999),
                new XElement("dt35", dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["35楼"] == System.DBNull.Value ? "88835" : dtdt.Rows[0]["35楼"]) : 999),
                new XElement("dt36", dtdt.Rows.Count == 1 ? (dtdt.Rows[0]["36楼"] == System.DBNull.Value ? "88836" : dtdt.Rows[0]["36楼"]) : 999),

            #endregion
            #region fdt
 new XElement("fdt", lcsz),
                new XElement("fdtxz", ""),
                new XElement("fdt1", dtfdt.Rows.Count == 1 ? (dtfdt.Rows[0]["1楼"] == System.DBNull.Value ? "8881" : dtfdt.Rows[0]["1楼"]) : 999),
                new XElement("fdt2", dtfdt.Rows.Count == 1 ? (dtfdt.Rows[0]["2楼"] == System.DBNull.Value ? "8882" : dtfdt.Rows[0]["2楼"]) : 999),
                new XElement("fdt3", dtfdt.Rows.Count == 1 ? (dtfdt.Rows[0]["3楼"] == System.DBNull.Value ? "8883" : dtfdt.Rows[0]["3楼"]) : 999),
                new XElement("fdt4", dtfdt.Rows.Count == 1 ? (dtfdt.Rows[0]["4楼"] == System.DBNull.Value ? "8884" : dtfdt.Rows[0]["4楼"]) : 999),
                new XElement("fdt5", dtfdt.Rows.Count == 1 ? (dtfdt.Rows[0]["5楼"] == System.DBNull.Value ? "8885" : dtfdt.Rows[0]["5楼"]) : 999),
                new XElement("fdt6", dtfdt.Rows.Count == 1 ? (dtfdt.Rows[0]["6楼"] == System.DBNull.Value ? "8886" : dtfdt.Rows[0]["6楼"]) : 999),
                new XElement("fdt7", dtfdt.Rows.Count == 1 ? (dtfdt.Rows[0]["7楼"] == System.DBNull.Value ? "8887" : dtfdt.Rows[0]["7楼"]) : 999),
                new XElement("fdt8", dtfdt.Rows.Count == 1 ? (dtfdt.Rows[0]["8楼"] == System.DBNull.Value ? "8888" : dtfdt.Rows[0]["8楼"]) : 999),
                new XElement("fdt9", dtfdt.Rows.Count == 1 ? (dtfdt.Rows[0]["9楼"] == System.DBNull.Value ? "8889" : dtfdt.Rows[0]["9楼"]) : 999),
                new XElement("fdt10", dtfdt.Rows.Count == 1 ? (dtfdt.Rows[0]["10楼"] == System.DBNull.Value ? "88810" : dtfdt.Rows[0]["10楼"]) : 999),
                new XElement("fdt11", dtfdt.Rows.Count == 1 ? (dtfdt.Rows[0]["11楼"] == System.DBNull.Value ? "88811" : dtfdt.Rows[0]["11楼"]) : 999),
                new XElement("fdt12", dtfdt.Rows.Count == 1 ? (dtfdt.Rows[0]["12楼"] == System.DBNull.Value ? "88812" : dtfdt.Rows[0]["12楼"]) : 999),
                new XElement("fdt13", dtfdt.Rows.Count == 1 ? (dtfdt.Rows[0]["13楼"] == System.DBNull.Value ? "88813" : dtfdt.Rows[0]["13楼"]) : 999),
                new XElement("fdt14", dtfdt.Rows.Count == 1 ? (dtfdt.Rows[0]["14楼"] == System.DBNull.Value ? "88814" : dtfdt.Rows[0]["14楼"]) : 999),
                new XElement("fdt15", dtfdt.Rows.Count == 1 ? (dtfdt.Rows[0]["15楼"] == System.DBNull.Value ? "88815" : dtfdt.Rows[0]["15楼"]) : 999),
            #endregion
            #region
 new XElement("dtsz", ywdt.Contains("无") ? "无" : "有"),
                new XElement("dtszxz", ""),
                new XElement("dtsz1", dtsz.Rows.Count == 1 ? (dtsz.Rows[0]["有电梯"] == System.DBNull.Value ? "8881" : dtsz.Rows[0]["有电梯"]) : 999),
                new XElement("dtsz2", dtsz.Rows.Count == 1 ? (dtsz.Rows[0]["无电梯"] == System.DBNull.Value ? "8882" : dtsz.Rows[0]["无电梯"]) : 999)
            #endregion
);
            fjxml.Save(AppDomain.CurrentDomain.BaseDirectory.ToString() + @"..\addins\AppraiseMethod\fjxml.xml");
            TestHarness(tmpyddata, ydreturn);

        }

        public void TestHarness(iyddata tmpyddata, iyd ydreturn)
        {
            XmlReaderSettings xrs = new XmlReaderSettings();
            xrs.ConformanceLevel = ConformanceLevel.Document;
            xrs.IgnoreComments = true;
            xrs.IgnoreProcessingInstructions = true;
            xrs.IgnoreWhitespace = true;
            using (Stream s = new StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + @"..\addins\AppraiseMethod\Compiler.xml").BaseStream)
            {
                XmlReader reader = XmlReader.Create(s, xrs);

                string comment;
                string ruleset;
                ArrayList model = null;
                RuleEngine.ROM rom = null;

                bool inTest = false;
                while (!reader.EOF)
                {
                    if (reader.IsStartElement("test"))
                    {
                        Debug.WriteLine("START TEST");
                        inTest = true;
                        model = new ArrayList();
                        reader.Read();
                    }

                    else if (inTest && reader.Name == "comment")
                    {
                        comment = reader.ReadElementContentAsString();
                        Debug.WriteLine(comment);
                    }

                    else if (inTest && reader.Name == "ruleset")
                    {
                        ruleset = AppDomain.CurrentDomain.BaseDirectory.ToString() + reader.ReadElementContentAsString();
                        Debug.WriteLine(ruleset);
                        XmlDocument doc = new XmlDocument();
                        doc.Load(ruleset);
                        rom = RuleEngine.Compiler.Compiler.Compile(doc);
                    }

                    else if (inTest && reader.Name == "model")
                    {
                        string mid = reader.GetAttribute("modelId");
                        string m = AppDomain.CurrentDomain.BaseDirectory.ToString() + reader.ReadElementContentAsString();
                        XmlDocument mod = new XmlDocument();
                        mod.Load(m);
                        model.Add(mod);
                        rom.AddModel(mid, mod);
                    }

                    else if (inTest && reader.Name == "evaluate")
                    {
                        //evaluate
                        Debug.WriteLine("Evaluate");
                        rom.Evaluate();
                        reader.Read();
                    }

                    else if (inTest && reader.Name == "assign")
                    {
                        Debug.WriteLine("Assign");
                        string mid = reader.GetAttribute("factId");
                        string m = reader.ReadElementContentAsString();

                        object value;
                        //determine value type
                        switch (rom[mid].ValueType.ToString()) //deterrmine the type of value returned by xpath
                        {
                            case "System.Double":
                                value = Double.Parse(m);
                                break;
                            case "System.Boolean":
                                value = Boolean.Parse(m);
                                break;
                            case "System.String":
                                value = m;
                                break;
                            default:
                                throw new Exception("Invalid type: " + m);
                        }
                        rom[mid].Value = value;
                    }

                    else if (inTest && reader.Name == "result")
                    {
                        string mid = reader.GetAttribute("factId");
                        string m = reader.ReadElementContentAsString();
                        //==============此处返回处理后的结果
                        if (tmpyddata is yddjdata)
                        {
                            double dqpj;
                            //==========
                            //rom.Evidence["Fjglxxz"].Value
                            //rom.Evidence["Fcxxz"].Value
                            //rom.Evidence["Llxz"].Value
                            //rom.Evidence["Fllqkxz"].Value
                            //rom.Evidence["Fjtxz"].Value
                            //rom.Evidence["Rjlxz"].Value
                            if (yd != null)
                            {
                                yd = null;
                                yd = new yddj();
                            }
                            else
                            {
                                yd = new yddj();
                            }
                            if (bflag.Ss)
                            {
                                if (double.TryParse(this.txt房价区片价.Text.ToString(), out dqpj))
                                {
                                    yd.Fjqpj = dqpj;
                                }
                                else
                                {
                                    yd = null;
                                    SkyMap.Net.Gui.MessageHelper.ShowInfo("房价区片价不能为空");
                                    return;
                                }
                            }
                            else
                            {
                                if (double.TryParse(bflag.Dj.ToString(), out dqpj))
                                {
                                    yd.Fjqpj = dqpj;
                                }
                                else
                                {
                                    yd = null;
                                    SkyMap.Net.Gui.MessageHelper.ShowInfo("单价有误！");
                                    return;
                                }
                            }
                            yd.Jglxsz = rom["Fjglxxz"].Value.ToString() == "999" ? 0 : double.Parse(rom["Fjglxxz"].Value.ToString());
                            if (yd.Jglxsz > 1000)
                            {
                                if ((yd as iyd).Jglxsz.ToString().Substring(0, 3) == "888")
                                {
                                    (yd as iyd).Jglxsz = 0;
                                }
                            }

                            yd.Cxsz = rom["Fcxxz"].Value.ToString() == "999" ? 0 : double.Parse(rom["Fcxxz"].Value.ToString());
                            if (yd.Cxsz > 1000)
                            {
                                if ((yd as iyd).Cxsz.ToString().Substring(0, 3) == "888")
                                {
                                    (yd as iyd).Cxsz = 0;
                                }
                            }

                            yd.Llsz = rom["Llxz"].Value.ToString() == "999" ? 0 : double.Parse(rom["Llxz"].Value.ToString());
                            if (yd.Llsz > 1000)
                            {
                                if ((yd as iyd).Llsz.ToString().Substring(0, 3) == "888")
                                {
                                    int i = int.Parse((yd as iyd).Llsz.ToString().Substring(3, 1)) - 1; //当前所在点
                                    double d1 = 0d;
                                    double d2 = 0d;
                                    int t1 = 0;  //后面点
                                    int t2 = 0;  //前面点
                                    for (int t = i + 1; t < ydcol.Ll.Count; t++)
                                    {
                                        if (ydcol.Ll[t] < 1000)
                                        {
                                            d1 = ydcol.Ll[t];
                                            t1 = t + 1;
                                            break;
                                        }
                                    }
                                    for (int j = i - 1; j >= 0; j--)
                                    {
                                        if (ydcol.Ll[j] < 1000)
                                        {
                                            d2 = ydcol.Ll[j];
                                            t2 = j + 1;
                                            break;
                                        }
                                    }
                                    if (t1 == 0 && t2 == 0)
                                    {
                                        (yd as iyd).Llsz = 0;
                                    }
                                    else if (t1 != 0 && t2 == 0)
                                    {
                                        (yd as iyd).Llsz = d1;
                                    }
                                    else if (t1 == 0 && t2 != 0)
                                    {
                                        (yd as iyd).Llsz = d2;
                                    }
                                    else if (t1 != 0 && t2 != 0)
                                    {
                                        int ti = int.Parse((yd as iyd).Llsz.ToString().Substring(3, 1));
                                        if ((t1 - ti) > (ti - t2))
                                        {
                                            (yd as iyd).Llsz = d2;
                                        }
                                        else if ((t1 - ti) < (ti - t2))
                                        {
                                            (yd as iyd).Llsz = d1;
                                        }
                                        else if ((t1 - ti) == (ti - t2))
                                        {
                                            (yd as iyd).Llsz = d1;
                                        }
                                    }
                                }
                            }

                            yd.Lnqksz = rom["Fllqkxz"].Value.ToString() == "999" ? 0 : double.Parse(rom["Fllqkxz"].Value.ToString());
                            if (yd.Lnqksz > 1000)
                            {
                                if ((yd as iyd).Lnqksz.ToString().Substring(0, 3) == "888")
                                {
                                    (yd as iyd).Lnqksz = 0;
                                }
                            }

                            (yd as iyddj).Jtsz = rom["Fjtxz"].Value.ToString() == "999" ? 0 : double.Parse(rom["Fjtxz"].Value.ToString());
                            if ((yd as iyddj).Jtsz > 1000)
                            {
                                if ((yd as iyddj).Jtsz.ToString().Substring(0, 3) == "888")
                                {
                                    (yd as iyddj).Jtsz = 0;
                                }
                            }
                            #region rjl 特殊处理
                            (yd as iyddj).Rjlsz = rom["Rjlxz"].Value.ToString() == "999" ? 0 : double.Parse(rom["Rjlxz"].Value.ToString());
                            if ((yd as iyddj).Rjlsz > 1000)
                            {
                                if ((yd as iyddj).Rjlsz.ToString().Substring(0, 3) == "888")
                                {
                                    int i = int.Parse((yd as iyddj).Rjlsz.ToString().Substring(3, 1)) - 1; //当前所在点
                                    double d1 = 0d;
                                    double d2 = 0d;
                                    int t1 = 0;  //后面点
                                    int t2 = 0;  //前面点
                                    for (int t = i + 1; t < ydcol.Rjl.Count; t++)
                                    {
                                        if (ydcol.Rjl[t] < 1000)
                                        {
                                            d1 = ydcol.Rjl[t];
                                            t1 = t + 1;
                                            break;
                                        }
                                    }
                                    for (int j = i - 1; j >= 0; j--)
                                    {
                                        if (ydcol.Rjl[j] < 1000)
                                        {
                                            d2 = ydcol.Rjl[j];
                                            t2 = j + 1;
                                            break;
                                        }
                                    }

                                    if (t1 == 0 && t2 == 0)
                                    {
                                        (yd as iyddj).Rjlsz = 0;
                                    }
                                    else if (t1 != 0 && t2 == 0)
                                    {
                                        (yd as iyddj).Rjlsz = d1;
                                    }
                                    else if (t1 == 0 && t2 != 0)
                                    {
                                        (yd as iyddj).Rjlsz = d2;
                                    }
                                    else if (t1 != 0 && t2 != 0)
                                    {
                                        int ti = int.Parse((yd as iyddj).Rjlsz.ToString().Substring(3, 1));
                                        if ((t1 - ti) > (ti - t2))
                                        {
                                            (yd as iyddj).Rjlsz = d2;
                                        }
                                        else if ((t1 - ti) < (ti - t2))
                                        {
                                            (yd as iyddj).Rjlsz = d1;
                                        }
                                        else if ((t1 - ti) == (ti - t2))
                                        {
                                            (yd as iyddj).Rjlsz = d1;
                                        }
                                    }
                                }
                            }
                            #endregion

                        }
                        if (tmpyddata is ydfdjdata)
                        {
                            double dqpj;

                            if (yd != null)
                            {
                                yd = null;
                                yd = new ydfdj();
                            }
                            else
                            {
                                yd = new ydfdj();
                            }
                            if (bflag.Ss)
                            {
                                if (double.TryParse(this.txt房价区片价.Text.ToString(), out dqpj))
                                {
                                    yd.Fjqpj = dqpj;
                                }
                                else
                                {
                                    yd = null;
                                    SkyMap.Net.Gui.MessageHelper.ShowInfo("房价区片价不能为空");
                                    return;
                                }
                            }
                            else
                            {
                                if (double.TryParse(bflag.Dj.ToString(), out dqpj))
                                {
                                    yd.Fjqpj = dqpj;
                                }
                                else
                                {
                                    yd = null;
                                    SkyMap.Net.Gui.MessageHelper.ShowInfo("单价不能为空");
                                    return;
                                }
                            }
                            yd.Jglxsz = rom["Fjglxxz"].Value.ToString() == "999" ? 0 : double.Parse(rom["Fjglxxz"].Value.ToString());
                            if (yd.Jglxsz > 1000)
                            {
                                if ((yd as iyd).Jglxsz.ToString().Substring(0, 3) == "888")
                                {
                                    (yd as iyd).Jglxsz = 0;
                                }
                            }

                            yd.Cxsz = rom["Fcxxz"].Value.ToString() == "999" ? 0 : double.Parse(rom["Fcxxz"].Value.ToString());
                            if (yd.Cxsz > 1000)
                            {
                                if ((yd as iyd).Cxsz.ToString().Substring(0, 3) == "888")
                                {
                                    (yd as iyd).Cxsz = 0;
                                }
                            }

                            yd.Llsz = rom["Llxz"].Value.ToString() == "999" ? 0 : double.Parse(rom["Llxz"].Value.ToString());
                            if (yd.Llsz > 1000)
                            {
                                if ((yd as iyd).Llsz.ToString().Substring(0, 3) == "888")
                                {
                                    int i = int.Parse((yd as iyd).Llsz.ToString().Substring(3, 1)) - 1; //当前所在点
                                    double d1 = 0d;
                                    double d2 = 0d;
                                    int t1 = 0;  //后面点
                                    int t2 = 0;  //前面点
                                    for (int t = i + 1; t < ydcol.Ll.Count; t++)
                                    {
                                        if (ydcol.Ll[t] < 1000)
                                        {
                                            d1 = ydcol.Ll[t];
                                            t1 = t + 1;
                                            break;
                                        }
                                    }
                                    for (int j = i - 1; j >= 0; j--)
                                    {
                                        if (ydcol.Ll[j] < 1000)
                                        {
                                            d2 = ydcol.Ll[j];
                                            t2 = j + 1;
                                            break;
                                        }
                                    }
                                    if (t1 == 0 && t2 == 0)
                                    {
                                        (yd as iyd).Llsz = 0;
                                    }
                                    else if (t1 != 0 && t2 == 0)
                                    {
                                        (yd as iyd).Llsz = d1;
                                    }
                                    else if (t1 == 0 && t2 != 0)
                                    {
                                        (yd as iyd).Llsz = d2;
                                    }
                                    else if (t1 != 0 && t2 != 0)
                                    {
                                        int ti = int.Parse((yd as iyd).Llsz.ToString().Substring(3, 1));
                                        if ((t1 - ti) > (ti - t2))
                                        {
                                            (yd as iyd).Llsz = d2;
                                        }
                                        else if ((t1 - ti) < (ti - t2))
                                        {
                                            (yd as iyd).Llsz = d1;
                                        }
                                        else if ((t1 - ti) == (ti - t2))
                                        {
                                            (yd as iyd).Llsz = d1;
                                        }
                                    }
                                }
                            }

                            yd.Lnqksz = rom["Fllqkxz"].Value.ToString() == "999" ? 0 : double.Parse(rom["Fllqkxz"].Value.ToString());
                            if (yd.Lnqksz > 1000)
                            {
                                if ((yd as iyd).Lnqksz.ToString().Substring(0, 3) == "888")
                                {
                                    (yd as iyd).Lnqksz = 0;
                                }
                            }

                            #region fdj jzmj
                            (yd as iydfdj).Jzmjsz = rom["Jzmjxz"].Value.ToString() == "999" ? 0 : double.Parse(rom["Jzmjxz"].Value.ToString());
                            if ((yd as iydfdj).Jzmjsz > 1000)
                            {
                                if ((yd as iydfdj).Jzmjsz.ToString().Substring(0, 3) == "888")
                                {
                                    int i = int.Parse((yd as iydfdj).Jzmjsz.ToString().Substring(3, 1)) - 1; //当前所在点
                                    double d1 = 0d;
                                    double d2 = 0d;
                                    int t1 = 0;  //后面点
                                    int t2 = 0;  //前面点
                                    for (int t = i + 1; t < ydcol.Jzmj.Count; t++)
                                    {
                                        if (ydcol.Jzmj[t] < 1000)
                                        {
                                            d1 = ydcol.Jzmj[t];
                                            t1 = t + 1;
                                            break;
                                        }
                                    }
                                    for (int j = i - 1; j >= 0; j--)
                                    {
                                        if (ydcol.Jzmj[j] < 1000)
                                        {
                                            d2 = ydcol.Jzmj[j];
                                            t2 = j + 1;
                                            break;
                                        }
                                    }

                                    if (t1 == 0 && t2 == 0)
                                    {
                                        (yd as iydfdj).Jzmjsz = 0;

                                    }
                                    else if (t1 != 0 && t2 == 0)
                                    {
                                        (yd as iydfdj).Jzmjsz = d1;

                                    }
                                    else if (t1 == 0 && t2 != 0)
                                    {
                                        (yd as iydfdj).Jzmjsz = d2;

                                    }
                                    else if (t1 != 0 && t2 != 0)
                                    {
                                        int ti = int.Parse((yd as iydfdj).Jzmjsz.ToString().Substring(3, 1));
                                        if ((t1 - ti) > (ti - t2))
                                        {
                                            (yd as iydfdj).Jzmjsz = d2;
                                        }
                                        else if ((t1 - ti) < (ti - t2))
                                        {
                                            (yd as iydfdj).Jzmjsz = d1;
                                        }
                                        else if ((t1 - ti) == (ti - t2))
                                        {
                                            (yd as iydfdj).Jzmjsz = d1;
                                        }
                                    }
                                }
                            }
                            #endregion

                            (yd as iydfdj).Fssz = rom["Fsxz"].Value.ToString() == "999" ? 0 : double.Parse(rom["Fsxz"].Value.ToString());
                            if ((yd as iydfdj).Fssz > 1000)
                            {
                                (yd as iydfdj).Fssz = 0;
                            }

                            (yd as iydfdj).Gtsz = rom["Gtxz"].Value.ToString() == "999" ? 0 : double.Parse(rom["Gtxz"].Value.ToString());
                            if ((yd as iydfdj).Gtsz > 1000)
                            {
                                (yd as iydfdj).Gtsz = 0;
                            }

                            (yd as iydfdj).Lxsz = rom["Llxz"].Value.ToString() == "999" ? 0 : double.Parse(rom["Llxz"].Value.ToString());
                            if ((yd as iydfdj).Lxsz > 1000)
                            {
                                (yd as iydfdj).Lxsz = 0;
                            }

                            (yd as iydfdj).Wyglsz = rom["Wyxz"].Value.ToString() == "999" ? 0 : double.Parse(rom["Wyxz"].Value.ToString());
                            if ((yd as iydfdj).Wyglsz > 1000)
                            {
                                (yd as iydfdj).Wyglsz = 0;
                            }

                            if ((tmpyddata as ydfdjdata).Ywdt.Contains("有"))
                            {
                                (yd as iydfdj).Lcsz = rom["Dtxz"].Value.ToString() == "999" ? 0 : double.Parse(rom["Dtxz"].Value.ToString());
                                #region fdj dt
                                if ((yd as iydfdj).Lcsz > 1000)
                                {
                                    if ((yd as iydfdj).Lcsz.ToString().Substring(0, 3) == "888")
                                    {
                                        int i = int.Parse((yd as iydfdj).Lcsz.ToString().Substring(3, 1)) - 1; //当前所在点
                                        double d1 = 0d;
                                        double d2 = 0d;
                                        int t1 = 0;  //后面点
                                        int t2 = 0;  //前面点
                                        for (int t = i + 1; t < ydcol.Dt.Count; t++)
                                        {
                                            if (ydcol.Dt[t] < 1000)
                                            {
                                                d1 = ydcol.Dt[t];
                                                t1 = t + 1;
                                                break;
                                            }
                                        }
                                        for (int j = i - 1; j >= 0; j--)
                                        {
                                            if (ydcol.Dt[j] < 1000)
                                            {
                                                d2 = ydcol.Dt[j];
                                                t2 = j + 1;
                                                break;
                                            }
                                        }
                                        if (t1 == 0 && t2 == 0)
                                        {
                                            (yd as iydfdj).Lcsz = 0;
                                        }
                                        else if (t1 != 0 && t2 == 0)
                                        {
                                            (yd as iydfdj).Lcsz = d1;
                                        }
                                        else if (t1 == 0 && t2 != 0)
                                        {
                                            (yd as iydfdj).Lcsz = d2;
                                        }
                                        else if (t1 != 0 && t2 != 0)
                                        {
                                            int ti = int.Parse((yd as iydfdj).Lcsz.ToString().Substring(3, 1));
                                            if ((t1 - ti) > (ti - t2))
                                            {
                                                (yd as iydfdj).Lcsz = d2;
                                            }
                                            else if ((t1 - ti) < (ti - t2))
                                            {
                                                (yd as iydfdj).Lcsz = d1;
                                            }
                                            else if ((t1 - ti) == (ti - t2))
                                            {
                                                (yd as iydfdj).Lcsz = d1;
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                            else if ((tmpyddata as ydfdjdata).Ywdt.Contains("无"))
                            {
                                (yd as iydfdj).Lcsz = rom["Fdtxz"].Value.ToString() == "999" ? 0 : double.Parse(rom["Fdtxz"].Value.ToString());
                                #region fdj fdt
                                if ((yd as iydfdj).Lcsz > 1000)
                                {
                                    if ((yd as iydfdj).Lcsz.ToString().Substring(0, 3) == "888")
                                    {
                                        int i = int.Parse((yd as iydfdj).Lcsz.ToString().Substring(3, 1)) - 1; //当前所在点
                                        double d1 = 0d;
                                        double d2 = 0d;
                                        int t1 = 0;  //后面点
                                        int t2 = 0;  //前面点
                                        for (int t = i + 1; t < ydcol.Fdt.Count; t++)
                                        {
                                            if (ydcol.Fdt[t] < 1000)
                                            {
                                                d1 = ydcol.Fdt[t];
                                                t1 = t + 1;
                                                break;
                                            }
                                        }
                                        for (int j = i - 1; j >= 0; j--)
                                        {
                                            if (ydcol.Fdt[j] < 1000)
                                            {
                                                d2 = ydcol.Fdt[j];
                                                t2 = j + 1;
                                                break;
                                            }
                                        }

                                        if (t1 == 0 && t2 == 0)
                                        {
                                            (yd as iydfdj).Lcsz = 0;

                                        }
                                        else if (t1 != 0 && t2 == 0)
                                        {
                                            (yd as iydfdj).Lcsz = d1;

                                        }
                                        else if (t1 == 0 && t2 != 0)
                                        {
                                            (yd as iydfdj).Lcsz = d2;

                                        }
                                        else if (t1 != 0 && t2 != 0)
                                        {
                                            int ti = int.Parse((yd as iydfdj).Lcsz.ToString().Substring(3, 1));
                                            if ((t1 - ti) > (ti - t2))
                                            {
                                                (yd as iydfdj).Lcsz = d2;
                                            }
                                            else if ((t1 - ti) < (ti - t2))
                                            {
                                                (yd as iydfdj).Lcsz = d1;
                                            }
                                            else if ((t1 - ti) == (ti - t2))
                                            {
                                                (yd as iydfdj).Lcsz = d1;
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                (yd as iydfdj).Lcsz = 0;
                            }

                            (yd as iydfdj).Dtsz = rom["Dtszxz"].Value.ToString() == "999" ? 0 : double.Parse(rom["Dtszxz"].Value.ToString());
                            if ((yd as iydfdj).Dtsz > 1000)
                            {
                                (yd as iydfdj).Dtsz = 0;
                            }


                        }

                    }

                    else if (inTest && reader.Name == "test")
                    {
                        rom = null;
                        model = null;
                        comment = null;
                        ruleset = null;
                        inTest = false;
                        reader.Read();
                        Debug.WriteLine("END TEST");
                    }
                    else
                    {
                        reader.Read();
                    }

                }
            }

        }

        private void bt_exportyddata_Click(object sender, EventArgs e)
        {
            string temp = "";
            string strexecl = "";
            string[] sheetname = new string[1];
            string[] sql = new string[1];
            string strwdt = string.Empty;
            string strProjectId = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProjectId, "");

            string where;

            this.Save();

            if (cbeType.Text.ToString() == "单家独户")
            {
                temp = System.Windows.Forms.Application.StartupPath + string.Format("\\生成结果\\{0}单家独户样点.xls", txt区片号.Text.ToString());
                where = " where yw_yddj.PROJECT_ID = '" + strProjectId + "'";
                strexecl = System.Windows.Forms.Application.StartupPath + "\\fj模板\\单家独户模板.xls";
                sheetname[0] = "单家独户样点";
                sql[0] = @"SELECT 
  yw_yddj.[序号],
  yw_yddj.[区片号],
  yw_yddj.[样点编号],
  yw_yddj.[区镇],
  yw_yddj.[地址],
  yw_yddj.[建筑面积],
  yw_yddj.[套内面积],
  yw_yddj.[土地面积],
  yw_yddj.[结构],
  yw_yddj.[朝向],
  yw_yddj.[竣工时间],
  yw_yddj.[楼龄],
  yw_yddj.[临路情况],
  yw_yddj.[交通情况],
  yw_yddj.[容积率],
  yw_yddj.[总价],
  yw_yddj.[单价],
  yw_yddj.[交易时点],
  yw_yddj.[评估时点],
  yw_yddj.[样点来源],
  yw_yddj.[备注]
FROM
  yw_yddj" + where;
            }
            else if (cbeType.Text.ToString() == "非单家独户")
            {
                temp = System.Windows.Forms.Application.StartupPath + string.Format("\\生成结果\\{0}非单家独户样点.xls", txt区片号.Text.ToString());
                where = " where yw_ydfdj.[样点]=1 and  yw_ydfdj.PROJECT_ID = '" + strProjectId + "'";
                strexecl = System.Windows.Forms.Application.StartupPath + "\\fj模板\\非单家独户模板.xls";
                sheetname[0] = "非单家独户样点";
                sql[0] = @"SELECT 
  yw_ydfdj.[序号],
  yw_ydfdj.[区片号],
  yw_ydfdj.[样点编号],
  yw_ydfdj.[区镇],
  yw_ydfdj.[地址],
  yw_ydfdj.[建筑面积],
  yw_ydfdj.[套内面积],
  yw_ydfdj.[土地面积],
  yw_ydfdj.[结构],
  yw_ydfdj.[朝向],
  yw_ydfdj.[竣工时间],
  yw_ydfdj.[楼龄],
  yw_ydfdj.[临路情况],
  yw_ydfdj.[总楼层],
  yw_ydfdj.[楼型],
  yw_ydfdj.[所处楼层],
  yw_ydfdj.[物业],
  yw_ydfdj.[复式],
  yw_ydfdj.[公摊],
  yw_ydfdj.[有无电梯],
  yw_ydfdj.[总价],
  yw_ydfdj.[单价],
  yw_ydfdj.[交易时点],
  yw_ydfdj.[评估时点],
  yw_ydfdj.[样点来源],
  yw_ydfdj.[备注]
FROM
  yw_ydfdj" + where;
            }


            Microsoft.Office.Interop.Excel.Workbook wb;
            excelop = excelop == null ? new ExcelFromArrayList() : excelop;
            wb = excelop.GetWorkBook(strexecl);
            for (int i = 0; i < sheetname.Count(); i++)
            {
                excelop.setCellValue(SkyMap.Net.DAO.QueryHelper.ExecuteSql("", "", sql[i]), excelop.GetSheet(sheetname[i]), 2, 1);
            }
            excelop.SaveAs(temp);
            excelop.GetWorkBook(temp);
            excelop.showExcel();
        }

        private void gcydfdj_Click(object sender, EventArgs e)
        {
        }

        private void gvydfdj_DoubleClick(object sender, EventArgs e)
        {
            bflag.Ss = false;
            DataRow drfdj = this.gvydfdj.GetFocusedDataRow();
            if (drfdj != null)
            {
                string jglxsz;
                string cxsz;
                string lnqksz;
                double llsz;
                double jzmj;
                //string lxsz;
                double lcsz;
                double zlc;
                //string wyglsz;
                //string fssz;
                //string gtsz;
                //string ywdt;
                #region 非单家

                ydfdjdata tmpydfdjdata = new ydfdjdata();
                double dj;
                if (double.TryParse(drfdj["单价"].ToString(), out dj))
                {
                    bflag.Dj = dj;
                }
                else
                {
                    MessageBox.Show("请检查所选择的行对应的单价数据的正确性!", "提示:");
                    return;
                }
                if (drfdj["样点来源"].ToString().Trim() != "评估样点")
                {
                    return;
                }
                if (double.TryParse(drfdj["建筑面积"].ToString(), out jzmj))
                {
                }
                else
                {
                    if (double.TryParse(drfdj["套内面积"].ToString(), out jzmj))
                    {

                    }
                }
                if (jzmj <= 0)
                {
                    SkyMap.Net.Gui.MessageHelper.ShowInfo(String.Format("请输入样点{0}的建筑面积或套内面积！", drfdj["序号"].ToString()));
                    return;
                }

                jglxsz = drfdj["结构"].ToString();
                cxsz = drfdj["朝向"].ToString();
                lnqksz = drfdj["临路情况"].ToString();

                if (double.TryParse(drfdj["楼龄"].ToString(), out llsz))
                {

                }
                else
                {
                    //SkyMap.Net.Gui.MessageHelper.ShowInfo(String.Format("请输入样点{0}的楼龄！", drfdj["序号"].ToString()));
                    //return;
                }
                tmpydfdjdata.Cxsz = cxsz;
                tmpydfdjdata.Jglxsz = jglxsz;
                tmpydfdjdata.Jzmj = jzmj;
                tmpydfdjdata.Llsz = llsz;
                tmpydfdjdata.Lnqksz = lnqksz;
                //string lxsz;
                //double lcsz;
                //double zlc;
                //string wyglsz;
                //string fssz;
                //string gtsz;
                //string ywdt;
                tmpydfdjdata.Lxsz = drfdj["楼型"].ToString();
                if (double.TryParse(drfdj["所处楼层"].ToString(), out lcsz))
                {
                    tmpydfdjdata.Lcsz = lcsz;
                }
                if (double.TryParse(drfdj["总楼层"].ToString(), out zlc))
                {
                    tmpydfdjdata.Zlc = zlc;
                }
                tmpydfdjdata.Wyglsz = drfdj["物业"].ToString();
                tmpydfdjdata.Fssz = drfdj["复式"].ToString();
                tmpydfdjdata.Gtsz = drfdj["公摊"].ToString();
                tmpydfdjdata.Ywdt = drfdj["有无电梯"].ToString();
                EvaluateYd(tmpydfdjdata, yd);
                if (yd != null)
                {
                    drfdj["标准价格"] = yd.Calu(bflag).ToString("#");
                    try
                    {
                        drfdj["总价"] = (dj * double.Parse(drfdj["建筑面积"].ToString())).ToString("#");
                    }
                    catch
                    {
                        try
                        {
                            drfdj["总价"] = (dj * double.Parse(drfdj["套内面积"].ToString())).ToString("#");
                        }
                        catch
                        {
                        }
                    }
                }

                #endregion
            }
        }

        private void gvyddj_DoubleClick(object sender, EventArgs e)
        {
            bflag.Ss = false;
            DataRow drdj = this.gvyddj.GetFocusedDataRow();
            if (drdj != null)
            {
                string jglxsz;
                string cxsz;
                string lnqksz;
                double jzmj;
                double tdmj;
                double llsz;
                double rjlsz;
                string jtsz;
                #region 单家
                yddjdata tmpyddjdata = new yddjdata();
                double dj;
                if (double.TryParse(drdj["单价"].ToString(), out dj))
                {
                    bflag.Dj = dj;
                }
                else
                {
                    MessageBox.Show("请检查所选择的行对应的单价数据的正确性!", "提示:");
                    return;
                }
                if (drdj["样点来源"].ToString().Trim() != "评估样点")
                {
                    return;
                }
                if (double.TryParse(drdj["建筑面积"].ToString(), out jzmj))
                {
                }
                else
                {
                    if (double.TryParse(drdj["套内面积"].ToString(), out jzmj))
                    {

                    }
                }
                if (jzmj <= 0)
                {
                    SkyMap.Net.Gui.MessageHelper.ShowInfo(String.Format("请输入样点{0}的建筑面积或套内面积！", drdj["序号"].ToString()));
                    return;
                }

                if (double.TryParse(drdj["土地面积"].ToString(), out tdmj))
                {
                    drdj["容积率"] = (jzmj / tdmj).ToString("#.##");
                }

                jglxsz = drdj["结构"].ToString();
                cxsz = drdj["朝向"].ToString();
                lnqksz = drdj["临路情况"].ToString();
                jtsz = drdj["交通情况"].ToString();
                if (double.TryParse(drdj["容积率"].ToString(), out rjlsz))
                {
                }
                else
                {
                    SkyMap.Net.Gui.MessageHelper.ShowInfo(String.Format("请输入样点{0}的容积率！", drdj["序号"].ToString()));
                    return;
                }
                if (double.TryParse(drdj["楼龄"].ToString(), out llsz))
                {

                }
                else
                {
                    SkyMap.Net.Gui.MessageHelper.ShowInfo(String.Format("请输入样点{0}的楼龄！", drdj["序号"].ToString()));
                    return;
                }
                tmpyddjdata.Cxsz = cxsz;
                tmpyddjdata.Jglxsz = jglxsz;
                tmpyddjdata.Jtsz = jtsz;
                tmpyddjdata.Jzmj = jzmj;
                tmpyddjdata.Llsz = llsz;
                tmpyddjdata.Lnqksz = lnqksz;
                tmpyddjdata.Rjlsz = rjlsz;
                EvaluateYd(tmpyddjdata, yd);
                if (yd != null)
                {
                    drdj["标准价格"] = yd.Calu(bflag).ToString("#");
                    try
                    {
                        drdj["总价"] = (dj * double.Parse(drdj["建筑面积"].ToString())).ToString("#");
                    }
                    catch
                    {
                        try
                        {
                            drdj["总价"] = (dj * double.Parse(drdj["套内面积"].ToString())).ToString("#");
                        }
                        catch
                        {
                        }
                    }
                }
           

                #endregion
            }

        }

        private void bt生成标准状态_Click(object sender, EventArgs e)
        {
            string Jglxsz = "";
            int tmpjglx = 0;
            StringBuilder sb = new StringBuilder();
            DataSet ds = (this as IDataForm).DataFormConntroller.DataSource;

            DataTable dtjzfj = ds.Tables["yw_jzfj"].Copy();
            DataTable dtjglx = ds.Tables["yw_结构类型修正"].Copy();
            DataTable dtcx = ds.Tables["yw_朝向修正"].Copy();
            DataTable dtll = ds.Tables["yw_楼龄修正"].Copy();
            DataTable dtllqk = ds.Tables["yw_临路情况修正"].Copy();
            DataTable dtjt = ds.Tables["yw_交通修正"].Copy();
            DataTable dtjzmj = ds.Tables["yw_建筑面积修正"].Copy();
            DataTable dtlx = ds.Tables["yw_楼型修正"].Copy();
            DataTable dtgt = ds.Tables["yw_公摊修正"].Copy();
            DataTable dtwy = ds.Tables["yw_物业管理修正"].Copy();
            DataTable dtfs = ds.Tables["yw_复式修正"].Copy();
            DataTable dtdt = ds.Tables["yw_电梯房楼层修正"].Copy();
            DataTable dtfdt = ds.Tables["yw_无电梯房楼层修正"].Copy();
            DataTable dtrjl = ds.Tables["yw_容积率修正"].Copy();
            DataTable dtsz = ds.Tables["yw_电梯修正"].Copy();
            DataTable tmpdt;
            #region 
            string tmpjglxbz, tmpcxbz, tmpllbz, tmpllqkbz, tmpjzmjbz, tmpjtbz, tmprjlbz, tmplxbz, tmpwybz, tmpfsbz;
            string tmpname;
            int isearch=0;
            if (dtjzfj.Rows.Count == 1)
            {
                DataRow dr = dtjzfj.Rows[0];
                tmpjglxbz = dr["结构类型备注"] == DBNull.Value? "":dr["结构类型备注"].ToString();
                tmpcxbz = dr["朝向修正备注"].ToString() ;
                tmpllbz = dr["楼龄修正备注"].ToString();
                tmpllqkbz = dr["临路情况备注"].ToString();
                tmpjzmjbz = dr["建筑面积备注"].ToString();
                tmpjtbz = dr["交通修正备注"].ToString();
                tmprjlbz = "";//注:容积率没有备注
                tmplxbz = dr["楼型修正备注"].ToString();
                tmpwybz = dr["物业管理备注"].ToString();
                tmpfsbz = dr["复式修正备注"].ToString();

#region 结构类型
                tmpdt = dtjglx;
                isearch = 0;
                tmpname = GetBzzt(tmpdt, isearch);
                if (tmpname == "no")
                {
                    Jglxsz = GetJglx(tmpjglxbz);                    
                }
                else
                {
                    Jglxsz = GetJglx(tmpname); 
                }
                sb.Append(string.Format("结构类型:{0};", Jglxsz));
#endregion
#region 朝向
                tmpdt = dtcx;
                isearch = 0;
                tmpname = GetBzzt(tmpdt, isearch);
                if (tmpname == "no")
                {
                    sb.Append(string.Format("朝向:{0};", tmpcxbz));
                }
                else
                {
                    sb.Append(string.Format("朝向:{0};",tmpname));
                }
#endregion
#region 楼龄
                if (dtll.Rows.Count >= 1)
                {
                    if (Jglxsz.Contains("钢筋混凝土"))
                    {
                        tmpjglx = 1;
                    }
                    else if (Jglxsz.Contains("混合"))
                    {
                        tmpjglx = 2;
                    }
                    else if (Jglxsz.Contains("砖木"))
                    {
                        tmpjglx = 3;
                    }
                    else if (Jglxsz.Contains("其它"))
                    {
                        tmpjglx = 4;
                    }
                    foreach (DataRow drll in dtll.Rows)
                    {
                        try
                        {
                            if (int.Parse(drll["结构类型"].ToString()) != tmpjglx)
                            {
                                drll.Delete();
                            }
                        }
                        catch
                        {
                        }
                    }
                    dtll.AcceptChanges();

                    tmpdt = dtll;
                    isearch = 0;
                    tmpname = GetBzzt(tmpdt, isearch);
                    if (tmpname == "no")
                    {
                        sb.Append(string.Format("楼龄:{0};", tmpllbz));
                    }
                    else
                    {
                        sb.Append(string.Format("楼龄:{0};", tmpname));
                    }

                }
                else
                {
                    sb.Append(string.Format("楼龄:{0};", tmpllbz));
                }
#endregion
#region 临路情况
                tmpdt = dtllqk;
                isearch = 0;
                tmpname = GetBzzt(tmpdt, isearch);
                if (tmpname == "no")
                {
                    sb.Append(string.Format("临路情况:{0};", tmpllqkbz));
                }
                else
                {
                    sb.Append(string.Format("临路情况:{0};", tmpname));
                }
#endregion
#region 交通修正
                tmpdt = dtjt;
                isearch = 0;
                tmpname = GetBzzt(tmpdt, isearch);
                if (tmpname == "no")
                {
                    sb.Append(string.Format("交通:{0};", tmpjtbz));
                }
                else
                {
                    sb.Append(string.Format("交通:{0};", tmpname));
                }
#endregion
#region 容积率
                tmpdt = dtrjl;
                isearch = 100;
                tmpname = GetBzzt(tmpdt, isearch);
                if (tmpname == "no")
                {
                    sb.Append(string.Format("容积率:{0};",tmprjlbz));
                }
                else
                {
                    sb.Append(string.Format("容积率:{0};", tmpname.Replace("-",".")));
                }
#endregion
#region 建筑面积
                tmpdt = dtjzmj ;
                isearch = 0;
                tmpname = GetBzztAll(tmpdt, isearch);
                if (tmpname == "no")
                {
                    sb.Append(string.Format("建筑面积:{0};", tmpjzmjbz));
                }
                else
                {
                    sb.Append(string.Format("建筑面积:{0};", tmpname));
                }
#endregion
#region 有无电梯
                tmpdt = dtsz;
                isearch = 0;
                tmpname = GetBzzt(tmpdt, isearch);
                if (tmpname == "no")
                {
                    //sb.Append(string.Format("电梯:{0};",""));
                }
                else
                {
                    sb.Append(string.Format("{0};", tmpname));
                }
#endregion
#region 楼型
                tmpdt = dtlx;
                isearch = 0;
                tmpname = GetBzzt(tmpdt, isearch);
                if (tmpname == "no")
                {
                    sb.Append(string.Format("楼型:{0};",tmplxbz));
                }
                else
                {
                    sb.Append(string.Format("楼型:{0};", tmpname));
                }
#endregion
#region 公摊
                tmpdt = dtgt;
                isearch = 0;
                tmpname = GetBzzt(tmpdt, isearch);
                if (tmpname == "no")
                {
                    //sb.Append(string.Format("公摊:{0};",""));
                }
                else
                {
                    sb.Append(string.Format("{0};", tmpname));
                }
#endregion
#region 物业
                tmpdt = dtwy;
                isearch = 0;
                tmpname = GetBzzt(tmpdt, isearch);
                if (tmpname == "no")
                {
                    sb.Append(string.Format("{0};", tmpwybz));
                }
                else
                {
                    sb.Append(string.Format("{0};", tmpname));
                }
#endregion
#region 复式
                tmpdt = dtfs;
                isearch = 0;
                tmpname = GetBzzt(tmpdt, isearch);
                if (tmpname == "no")
                {
                    sb.Append(string.Format("{0};", tmpfsbz));
                }
                else
                {
                    sb.Append(string.Format("{0};", tmpname));
                }
#endregion
            }
            #endregion
          

         
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tmpjglxbz"></param>
        /// <returns></returns>
        private static string GetJglx( string tmpjglxbz)
        {
            string Jglxsz = "";
            if (tmpjglxbz.IndexOf("钢筋") >= 0)
            {
                Jglxsz = "钢筋混凝土";
            }
            else if (tmpjglxbz.IndexOf("混合") >= 0)
            {
                Jglxsz = "混合";
            }
            else if (tmpjglxbz.IndexOf("转木") >= 0)
            {
                Jglxsz = "转木";
            }
            else if (tmpjglxbz.IndexOf("其它") >= 0)
            {
                Jglxsz = "其它";
            }
            else
            {
                MessageBox.Show("请检查结构类型!", "提醒:");
            }
            return Jglxsz;
        }

        private  string GetBzzt(DataTable tmpdt, int isearch)
        {
            string tmpname;
            if (tmpdt.Rows.Count == 1)
            {
                foreach (DataColumn dc in tmpdt.Columns)
                {
                    if (tmpdt.Rows[0][dc].ToString() == isearch.ToString())
                    {
                        tmpname = dc.ColumnName.ToString();
                        return tmpname;
                    }
                }
            }
            else
            {
                tmpname = "no";
                return tmpname;
            }
            return null;
           
        }

        private string GetBzztAll(DataTable tmpdt, int isearch)
        {
            string tmpname="";
            if (tmpdt.Rows.Count == 1)
            {
                foreach (DataColumn dc in tmpdt.Columns)
                {
                    if (tmpdt.Rows[0][dc].ToString() == isearch.ToString())
                    {
                        tmpname = tmpname + dc.ColumnName.ToString() + ",";                        
                    }
                }
                return tmpname;
            }
            else
            {
                tmpname = "no";
                return tmpname;
            }
            return null;

        }
    }

}

public class BFlag
{
    private bool ss;

    private double dj;

    /// <summary>
    /// 单价
    /// </summary>
    public double Dj
    {
        get { return dj; }
        set { dj = value; }
    }
    /// <summary>
    /// true代表顺算，false代表逆算
    /// </summary>
    public bool Ss
    {
        get { return ss; }
        set { ss = value; }
    }
}
public class JZFJ
{
    private System.Data.DataTable dtCxxz = null;//yw_朝向修正
    private System.Data.DataTable dtCflx = null;//yw_车房类型修正
    private DataTable dtCfqp = null;//yw_车房区片价
    private DataTable dtJzfj = null;//yw_jzfj中某projectid指定的单条记录
    private DataTable dtData = null;//yw_jzfj所有记录
    private DataTable dtDt = null;//yw_电梯修正
    private DataTable dtFs = null;//yw_复式修正
    private DataTable dtGt = null;//yw_公摊修正
    private DataTable dtJzmd = null;//yw_建筑密度修正
    private DataTable dtJzmj = null;//yw_建筑面积修正
    private DataTable dtJt = null;//yw_交通修正
    private DataTable dtJglx = null;//yw_结构类型修正
    private DataTable dtLlxz = null;//yw_临路情况修正
    private DataTable dtLoulingxz = null;//yw_楼龄修正    钢筋混凝土
    private DataTable dtLoulingxz1 = null;//yw_楼龄修正   混合
    private DataTable dtLoulingxz2 = null;//yw_楼龄修正   砖木
    private DataTable dtLoulingxz3 = null;//yw_楼龄修正   其它
    private DataTable dtLx = null;//yw_楼型修正
    private DataTable dtQp = null;//yw_区片信息
    private DataTable dtRjl = null;//yw_容积率修正
    private DataTable dtWdt = null;//yw_无电梯房楼层修正
    private DataTable dtWygl = null;//yw_物业管理修正
    private DataTable dtJzmjxzlx = null;//建筑面积修正楼型
    private DataTable dtLlxzjglx = null;//楼龄修正结构类型
    private DataTable dtRjlxzlx = null;//容积率修正类型

    public DataTable Jzfj()
    {
        string sql = "SELECT * FROM [Yw_Jzfj]";
        string tablename = "Jzfj";
        return NewMethod(sql, tablename, this.dtData);
    }

    private DataTable NewMethod(string sql, string tablename, DataTable dt)
    {
        DataTable tmpdt = new DataTable();
        tmpdt = QueryHelper.ExecuteSql("Default", string.Empty, sql);
        tmpdt.TableName = tablename;
        return tmpdt;
    }

    public DataTable Cxxz(string projectid)
    {
        string sql = "SELECT * FROM [Yw_朝向修正] where project_id = '{0}'";
        sql = string.Format(sql, projectid);
        string tablename = "朝向修正";
        return NewMethod(sql, tablename, this.dtCxxz);
    }

    public DataTable Jzfj(string projectid)
    {
        string sql = "SELECT * FROM [Yw_Jzfj] where project_id = '{0}'";
        sql = string.Format(sql, projectid);
        string tablename = "Jzfj";
        return NewMethod(sql, tablename, this.dtJzfj);
    }

    public DataTable Cflx(string projectid)
    {
        string sql = "SELECT * FROM [yw_车房类型修正] where project_id = '{0}'";
        sql = string.Format(sql, projectid);
        string tablename = "车房类型修正";
        return NewMethod(sql, tablename, this.dtCflx);
    }

    public DataTable Cfqp(string projectid)
    {
        string sql = "SELECT * FROM [yw_车房区片价] where project_id = '{0}'";
        sql = string.Format(sql, projectid);
        string tablename = "车房区片价";
        return NewMethod(sql, tablename, this.dtCfqp);
    }



    public DataTable Dt(string projectid)
    {
        string sql = "SELECT * FROM [yw_电梯修正] where project_id = '{0}'";
        sql = string.Format(sql, projectid);
        string tablename = "电梯修正";
        return NewMethod(sql, tablename, this.dtDt);
    }

    public DataTable Fs(string projectid)
    {
        string sql = "SELECT * FROM [yw_复式修正] where project_id = '{0}'";
        sql = string.Format(sql, projectid);
        string tablename = "复式修正";
        return NewMethod(sql, tablename, this.dtFs);
    }

    public DataTable Gt(string projectid)
    {
        string sql = "SELECT * FROM [yw_公摊修正] where project_id = '{0}'";
        sql = string.Format(sql, projectid);
        string tablename = "公摊修正";
        return NewMethod(sql, tablename, this.dtGt);
    }

    public DataTable Jzmd(string projectid)
    {
        string sql = "SELECT * FROM [yw_建筑密度修正] where project_id = '{0}'";
        sql = string.Format(sql, projectid);
        string tablename = "建筑密度修正";
        return NewMethod(sql, tablename, this.dtJzmd);
    }

    public DataTable Jzmj(string projectid)
    {
        string sql = "SELECT * FROM [yw_建筑面积修正] where project_id = '{0}'";
        sql = string.Format(sql, projectid);
        string tablename = "建筑面积修正";
        return NewMethod(sql, tablename, this.dtJzmj);
    }

    public DataTable Jt(string projectid)
    {
        string sql = "SELECT * FROM [yw_交通修正] where project_id = '{0}'";
        sql = string.Format(sql, projectid);
        string tablename = "交通修正";
        return NewMethod(sql, tablename, this.dtJt);
    }

    public DataTable Jglx(string projectid)
    {
        string sql = "SELECT * FROM [yw_结构类型修正] where project_id = '{0}'";
        sql = string.Format(sql, projectid);
        string tablename = "结构类型修正";
        return NewMethod(sql, tablename, this.dtJglx);
    }

    public DataTable Llxz(string projectid)
    {
        string sql = "SELECT * FROM [yw_临路情况修正] where project_id = '{0}'";
        sql = string.Format(sql, projectid);
        string tablename = "临路情况修正";
        return NewMethod(sql, tablename, this.dtLlxz);
    }



    public DataTable LouLingXz(string projectid)
    {
        string sql = "SELECT * FROM [yw_楼龄修正] where project_id = '{0}' and  结构类型 in (select 结构类型 from [楼龄修正结构类型] where  结构说明 ='钢筋混凝土')";
        sql = string.Format(sql, projectid);
        string tablename = "钢筋混凝土楼龄修正";
        return NewMethod(sql, tablename, this.dtLoulingxz);
    }

    public DataTable LouLingXz1(string projectid)
    {
        string sql = "SELECT * FROM [yw_楼龄修正] where project_id = '{0}' and  结构类型 in (select 结构类型 from [楼龄修正结构类型] where  结构说明 ='混合')";
        sql = string.Format(sql, projectid);
        string tablename = "混合楼龄修正";
        return NewMethod(sql, tablename, this.dtLoulingxz1);
    }


    public DataTable LouLingXz2(string projectid)
    {
        string sql = "SELECT * FROM [yw_楼龄修正] where project_id = '{0}' and  结构类型 in (select 结构类型 from [楼龄修正结构类型] where  结构说明 ='砖木')";
        sql = string.Format(sql, projectid);
        string tablename = "砖木楼龄修正";
        return NewMethod(sql, tablename, this.dtLoulingxz2);
    }


    public DataTable LouLingXz3(string projectid)
    {
        string sql = "SELECT * FROM [yw_楼龄修正] where project_id = '{0}' and  结构类型 in (select 结构类型 from [楼龄修正结构类型] where  结构说明 ='其它')";
        sql = string.Format(sql, projectid);
        string tablename = "其它楼龄修正";
        return NewMethod(sql, tablename, this.dtLoulingxz3);
    }

    public DataTable Lxxz(string projectid)
    {
        string sql = "SELECT * FROM [yw_楼型修正] where project_id = '{0}'";
        sql = string.Format(sql, projectid);
        string tablename = "楼型修正";
        return NewMethod(sql, tablename, this.dtLx);

    }

    public DataTable Qp(string projectid)
    {
        string sql = "SELECT * FROM [yw_区片信息] where project_id = '{0}'";
        sql = string.Format(sql, projectid);
        string tablename = "区片信息";
        return NewMethod(sql, tablename, this.dtQp);
    }

    public DataTable Rjl(string projectid)
    {
        string sql = "SELECT * FROM [yw_容积率修正] where project_id = '{0}'";
        sql = string.Format(sql, projectid);
        string tablename = "容积率修正";
        return NewMethod(sql, tablename, this.dtRjl);
    }

    public DataTable Wdt(string projectid, int i)
    {
        string sql = "SELECT count(*) FROM [yw_无电梯房楼层修正] where project_id = '{0}'";
        sql = string.Format(sql, projectid);
        string tablename = "无电梯房楼层修正" + i.ToString();
        DataTable dt = NewMethod(sql, tablename, this.dtWdt);

        Debug.Assert(int.Parse(dt.Rows[0][0].ToString()) < 6, "只能处理小于6条记录的无电梯房楼层修正");

        if (int.Parse(dt.Rows[0][0].ToString()) == 0)
        {
            sql = "select * from [yw_无电梯房楼层修正] where 1<>1";
        }
        else if (int.Parse(dt.Rows[0][0].ToString()) >= 1 && i == 1)
        {
            sql = "SELECT top 1 *  FROM (select top 1 * from [yw_无电梯房楼层修正] where project_id = '{0}' order by [ID] desc) ttt order by [id] asc";
        }
        else if (int.Parse(dt.Rows[0][0].ToString()) >= 2 && i == 2)
        {
            sql = "SELECT top 1 *  FROM (select top 2 * from [yw_无电梯房楼层修正] where project_id = '{0}' order by [ID] desc) ttt order by [id] asc";
        }
        else if (int.Parse(dt.Rows[0][0].ToString()) >= 3 && i == 3)
        {
            sql = "SELECT top 1 *  FROM (select top 3 * from [yw_无电梯房楼层修正] where project_id = '{0}' order by [ID] desc) ttt order by [id] asc";
        }
        else if (int.Parse(dt.Rows[0][0].ToString()) >= 4 && i == 4)
        {
            sql = "SELECT top 1 *  FROM (select top 4 * from [yw_无电梯房楼层修正] where project_id = '{0}' order by [ID] desc) ttt order by [id] asc";
        }
        else if (int.Parse(dt.Rows[0][0].ToString()) >= 5 && i == 5)
        {
            sql = "SELECT top 1 *  FROM (select top 5 * from [yw_无电梯房楼层修正] where project_id = '{0}' order by [ID] desc) ttt order by [id] asc";
        }
        else
        {
            sql = "select * from [yw_无电梯房楼层修正] where 1<>1";
        }

        sql = string.Format(sql, projectid);
        tablename = "无电梯房楼层修正" + i.ToString();
        return NewMethod(sql, tablename, this.dtWdt);

    }


    public DataTable Dtflc(string projectid, int i)
    {
        string sql = "SELECT count(*) FROM [yw_电梯房楼层修正] where project_id = '{0}'";
        sql = string.Format(sql, projectid);
        string tablename = "电梯房楼层修正" + i.ToString();
        DataTable dt = NewMethod(sql, tablename, this.dtWdt);

        Debug.Assert(int.Parse(dt.Rows[0][0].ToString()) < 6, "只能处理小于6条记录的电梯房楼层修正");

        if (int.Parse(dt.Rows[0][0].ToString()) == 0)
        {
            sql = "select * from [yw_电梯房楼层修正] where 1<>1";
        }
        else if (int.Parse(dt.Rows[0][0].ToString()) >= 1 && i == 1)
        {
            sql = "SELECT top 1 *  FROM (select top 1 * from [yw_电梯房楼层修正] where project_id = '{0}' order by [ID] desc) ttt order by [id] asc";
        }
        else if (int.Parse(dt.Rows[0][0].ToString()) >= 2 && i == 2)
        {
            sql = "SELECT top 1 *  FROM (select top 2 * from [yw_电梯房楼层修正] where project_id = '{0}' order by [ID] desc) ttt order by [id] asc";
        }
        else if (int.Parse(dt.Rows[0][0].ToString()) >= 3 && i == 3)
        {
            sql = "SELECT top 1 *  FROM (select top 3 * from [yw_电梯房楼层修正] where project_id = '{0}' order by [ID] desc) ttt order by [id] asc";
        }
        else if (int.Parse(dt.Rows[0][0].ToString()) >= 4 && i == 4)
        {
            sql = "SELECT top 1 *  FROM (select top 4 * from [yw_电梯房楼层修正] where project_id = '{0}' order by [ID] desc) ttt order by [id] asc";
        }
        else if (int.Parse(dt.Rows[0][0].ToString()) >= 5 && i == 5)
        {
            sql = "SELECT top 1 *  FROM (select top 5 * from [yw_电梯房楼层修正] where project_id = '{0}' order by [ID] desc) ttt order by [id] asc";
        }
        else
        {
            sql = "select * from [yw_电梯房楼层修正] where 1<>1";
        }

        sql = string.Format(sql, projectid);
        tablename = "电梯房楼层修正" + i.ToString();
        return NewMethod(sql, tablename, this.dtWdt);
    }

    public System.Data.DataTable Wygl(string projectid)
    {
        string sql = "SELECT * FROM [yw_物业管理修正] where project_id = '{0}'";
        sql = string.Format(sql, projectid);
        string tablename = "物业管理修正";
        return NewMethod(sql, tablename, this.dtWygl);
    }

    public DataTable Jzmjxzlx()
    {
        string sql = "SELECT * FROM [建筑面积修正楼型]";
        string tablename = "建筑面积修正楼型";
        return NewMethod(sql, tablename, this.dtJzmjxzlx);
    }

    public DataTable Llxzjglx()
    {
        string sql = "SELECT * FROM [楼龄修正结构类型]";
        string tablename = "楼龄修正结构类型";
        return NewMethod(sql, tablename, this.dtLlxzjglx);
    }

    public DataTable Rjlxzlx()
    {
        string sql = "SELECT * FROM [容积率修正类型]";
        string tablename = "容积率修正类型";
        return NewMethod(sql, tablename, this.dtRjlxzlx);
    }
}

public class KillExcel
{
    /// <summary>
    /// 结束Excel进程
    /// </summary>
    public static void KillExcelProcess(DateTime beforetime, DateTime aftertime)
    {
        Process[] myProcesses;
        DateTime startTime;
        myProcesses = Process.GetProcessesByName("Excel");

        //得不到Excel进程ID，暂时只能判断进程启动时间
        foreach (Process myProcess in myProcesses)
        {
            //加入try,因为有可能myProcess已退出 20091203
            try
            {
                startTime = myProcess.StartTime;
                string title = myProcess.MainWindowTitle;// 返回标题,在这里可以考虑做些判断，那样的话，杀进程将更加精确;

                if (startTime > beforetime && startTime < aftertime)
                {
                    myProcess.Kill();
                }
            }
            catch
            { }
        }
    }

    public static void KillWordProcess(DateTime beforetime, DateTime aftertime)
    {
        Process[] myProcesses;
        DateTime startTime;
        myProcesses = Process.GetProcessesByName("WINWORD");

        //得不到Excel进程ID，暂时只能判断进程启动时间
        foreach (Process myProcess in myProcesses)
        {
            //加入try,因为有可能myProcess已退出 20091203
            try
            {
                startTime = myProcess.StartTime;
                string title = myProcess.MainWindowTitle;// 返回标题,在这里可以考虑做些判断，那样的话，杀进程将更加精确;

                if (startTime > beforetime && startTime < aftertime)
                {
                    myProcess.Kill();
                }
            }
            catch
            { }
        }
    }

    public static void KillExcelProcess()
    {
        Process[] myProcesses;

        myProcesses = Process.GetProcessesByName("Excel");

        foreach (Process myProcess in myProcesses)
        {
            //加入try,因为有可能myProcess已退出 20091203
            try
            {
                myProcess.Kill();
            }
            catch
            {
            }
        }
    }
}

