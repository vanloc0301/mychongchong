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

namespace ZBPM
{

    public partial class GujiaDataForm : WfAbstractDataForm
    {
        /// <summary>
        /// 土地证编号相同时候的特殊处理
        /// </summary>
        public class Sh
        {
            private string _tdzh;
            private double _bzsf;
            private string _pglx;
            private double _tdzj;
            private double _fdczj;

            public string Pglx
            {
                get { return _pglx; }
                set { _pglx = value; }
            }


            public double Tdzj
            {
                get { return _tdzj; }
                set { _tdzj = value; }
            }


            public double Fdczj
            {
                get { return _fdczj; }
                set { _fdczj = value; }
            }

            public double Bzsf
            {
                get { return _bzsf; }
                set { _bzsf = value; }
            }


            public string Tdzh
            {
                get
                {
                    return _tdzh;
                }
                set
                {
                    _tdzh = value;
                }
            }
        }

        #region 全局变量
        private DataSet m_dstAll;
        private DataTable m_dtbB;
        private string m_strYwNo;
        private PrintSet m_printSet;
        private string bglx = "doc";
        private int iweburl = 0; //标记是否是第一次运行webgujia;
        #endregion
        private WindowManager _windowManager;

        public WindowManager WindowManager
        {
            get { return _windowManager; }
        }

        public GujiaDataForm()
        {
            InitializeComponent();
            Init();
            //==========嵌入webbrowser
            _windowManager = new WindowManager(this.tabControl);
            _windowManager.CommandStateChanged += new EventHandler<CommandStateEventArgs>(_windowManager_CommandStateChanged);
            _windowManager.StatusTextChanged += new EventHandler<TextChangedEventArgs>(_windowManager_StatusTextChanged);
            _windowManager.New();
        }

        #region   webbrowser 相关处理
        #region Tools menu
        // Executed when the user clicks on Tools -> Options
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        // Tools -> Show script errors
        private void scriptErrorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScriptErrorManager.Instance.ShowWindow();
        }

        #endregion

        #region File Menu

        // File -> Print
        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Print();
        }

        // File -> Print Preview
        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintPreview();
        }

        // File -> Exit
        //private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}

        // File -> Open URL
        private void openUrlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenUrlForm ouf = new OpenUrlForm())
            {
                if (ouf.ShowDialog() == DialogResult.OK)
                {
                    ExtendedWebBrowser brw = _windowManager.New(false);
                    brw.Navigate(ouf.Url);
                }
            }
        }

        // File -> Open File
        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = Properties.Resources.OpenFileDialogFilter;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Uri url = new Uri(ofd.FileName);
                    WindowManager.Open(url);
                }
            }
        }
        #endregion

        #region Help Menu

        //// Executed when the user clicks on Help -> About
        //private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    About();
        //}

        ///// <summary>
        ///// Shows the AboutForm
        ///// </summary>
        //private void About()
        //{
        //    using (AboutForm af = new AboutForm())
        //    {
        //        af.ShowDialog(this);
        //    }
        //}

        #endregion
        // Update the status text
        void _windowManager_StatusTextChanged(object sender, TextChangedEventArgs e)
        {
            this.toolStripStatusLabel.Text = e.Text;
        }

        // Enable / disable buttons
        void _windowManager_CommandStateChanged(object sender, CommandStateEventArgs e)
        {
            this.forwardToolStripButton.Enabled = ((e.BrowserCommands & BrowserCommands.Forward) == BrowserCommands.Forward);
            this.backToolStripButton.Enabled = ((e.BrowserCommands & BrowserCommands.Back) == BrowserCommands.Back);
            this.printPreviewToolStripButton.Enabled = ((e.BrowserCommands & BrowserCommands.PrintPreview) == BrowserCommands.PrintPreview);
            //  this.printPreviewToolStripMenuItem.Enabled = ((e.BrowserCommands & BrowserCommands.PrintPreview) == BrowserCommands.PrintPreview);
            this.printToolStripButton.Enabled = ((e.BrowserCommands & BrowserCommands.Print) == BrowserCommands.Print);
            //   this.printToolStripMenuItem.Enabled = ((e.BrowserCommands & BrowserCommands.Print) == BrowserCommands.Print);
            this.homeToolStripButton.Enabled = ((e.BrowserCommands & BrowserCommands.Home) == BrowserCommands.Home);
            this.searchToolStripButton.Enabled = ((e.BrowserCommands & BrowserCommands.Search) == BrowserCommands.Search);
            this.refreshToolStripButton.Enabled = ((e.BrowserCommands & BrowserCommands.Reload) == BrowserCommands.Reload);
            this.stopToolStripButton.Enabled = ((e.BrowserCommands & BrowserCommands.Stop) == BrowserCommands.Stop);
        }
        private void tabControl_VisibleChanged(object sender, EventArgs e)
        {
            if (tabControl.Visible)
            {
                this.panelControl3.BackColor = SystemColors.Control;
            }
            else
                this.panelControl3.BackColor = SystemColors.AppWorkspace;
        }

        #region Printing & Print Preview
        private void Print()
        {
            ExtendedWebBrowser brw = _windowManager.ActiveBrowser;
            if (brw != null)
                brw.ShowPrintDialog();
        }

        private void PrintPreview()
        {
            ExtendedWebBrowser brw = _windowManager.ActiveBrowser;
            if (brw != null)
                brw.ShowPrintPreviewDialog();
        }
        #endregion

        #region Toolstrip buttons
        private void closeWindowToolStripButton_Click(object sender, EventArgs e)
        {
            this._windowManager.New();
        }

        private void closeToolStripButton_Click(object sender, EventArgs e)
        {
            if (int.Parse(this.tabControl.TabPages.Count.ToString()) > 1)
            {
                this._windowManager.Close();
            }
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            Print();
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            PrintPreview();
        }

        private void backToolStripButton_Click(object sender, EventArgs e)
        {
            if (_windowManager.ActiveBrowser != null && _windowManager.ActiveBrowser.CanGoBack)
                _windowManager.ActiveBrowser.GoBack();
        }

        private void forwardToolStripButton_Click(object sender, EventArgs e)
        {
            if (_windowManager.ActiveBrowser != null && _windowManager.ActiveBrowser.CanGoForward)
                _windowManager.ActiveBrowser.GoForward();
        }

        private void stopToolStripButton_Click(object sender, EventArgs e)
        {
            if (_windowManager.ActiveBrowser != null)
            {
                _windowManager.ActiveBrowser.Stop();
            }
            stopToolStripButton.Enabled = false;
        }

        private void refreshToolStripButton_Click(object sender, EventArgs e)
        {
            if (_windowManager.ActiveBrowser != null)
            {
                _windowManager.ActiveBrowser.Refresh(WebBrowserRefreshOption.Normal);
            }
        }

        private void homeToolStripButton_Click(object sender, EventArgs e)
        {
            if (_windowManager.ActiveBrowser != null)
            //_windowManager.ActiveBrowser.GoHome();
            {
                string strdz = "";
                XElement xe = XElement.Load(Application.StartupPath + "\\webgujia.xml");
                XNamespace ns = "http://www.zgfdc.net/gujia";
                var xx1 = from el in xe.Descendants(ns + "webgujia") select el;

                foreach (XElement x in xx1)
                {
                    if (HasElement(x, ns, "address"))
                    {
                        strdz = x.Element(ns + "address").Value;
                    }
                }

                string url = strdz;

                Object o = null;

                //fetch the page to your web browser.
                this._windowManager.ActiveBrowser.Navigate(url);
                if (iweburl == 0)
                {
                    iweburl++;
                }
                else
                {
                    this._windowManager.ActiveBrowser.Refresh();
                }

            }
        }

        private void searchToolStripButton_Click(object sender, EventArgs e)
        {
            if (_windowManager.ActiveBrowser != null)
                _windowManager.ActiveBrowser.GoSearch();
        }

        #endregion

        #endregion

        private void Init()
        {
            //if (lue_估价目的.Properties.DataSource == null)
            //{
            //    public static void Init(DevExpress.XtraEditors.LookUpEdit lookupEdit, 
            //    string dataWordTypeCode,
            //    string displayMember,string valueMember)
            //{
            //    lookupEdit.Properties.DisplayMember = displayMember;
            //    lookupEdit.Properties.ValueMember = valueMember;
            //    lookupEdit.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Code", 40, "代码"));
            //    lookupEdit.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Name", 80, "名称"));
            //    lookupEdit.Properties.DataSource = SkyMap.Net.DAO.DataWordService.FindDataWordsByTypeCode(dataWordTypeCode);
            //}
            //public static void Init(DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit,
            //   string dataWordTypeCode)
            //{
            //    comboBoxEdit.Properties.Items.Clear();
            //    IList<SkyMap.Net.DAO.DataWord> ds = SkyMap.Net.DAO.DataWordService.FindDataWordsByTypeCode(dataWordTypeCode);
            //    foreach (SkyMap.Net.DAO.DataWord dw in ds)
            //    {
            //            comboBoxEdit.Properties.Items.Add(dw.Name);
            //    }
            //}
            this.statusStrip1.Visible = true;
            this.toolStrip.Visible = true;
            this.repositoryItemLookUpEdit3 = DataWordLookUpEditHelper.Create("PURPOSE", "Name", "Name");
            this.repositoryItemLookUpEdit3.NullText = "";
            this.col估价目的.ColumnEdit = this.repositoryItemLookUpEdit3;
            this.repositoryItemLookUpEdit5 = DataWordLookUpEditHelper.Create("JZLX", "Name", "Name");
            this.repositoryItemLookUpEdit5.NullText = "";
            this.col价值类型.ColumnEdit = this.repositoryItemLookUpEdit5;
            //this.repositoryItemLookUpEdit6 = DataWordLookUpEditHelper.Create("GJS", "Name", "Name");
            //this.repositoryItemLookUpEdit6.NullText = "";
            //this.col估价师.ColumnEdit = this.repositoryItemLookUpEdit6;
            //DataWordLookUpEditHelper.Init(, "GJS", "Name", "Name");
            this.repositoryItemCheckedComboBoxEdit3.Items.Clear();
            IList<SkyMap.Net.DAO.DataWord> ds = SkyMap.Net.DAO.DataWordService.FindDataWordsByTypeCode("GJS");
            foreach (SkyMap.Net.DAO.DataWord dw in ds)
            {
                this.repositoryItemCheckedComboBoxEdit3.Items.Add(dw.Name);
            }


            this.col估价师.ColumnEdit = this.repositoryItemCheckedComboBoxEdit3;
            this.repositoryItemLookUpEdit7 = DataWordLookUpEditHelper.Create("TDKFCD", "Name", "Name");
            this.repositoryItemLookUpEdit7.NullText = "";
            this.col土地开发程度1.ColumnEdit = this.repositoryItemLookUpEdit7;
            this.repositoryItemLookUpEdit8 = DataWordLookUpEditHelper.Create("TDPZCD", "Name", "Name");
            this.repositoryItemLookUpEdit8.NullText = "";
            this.col土地平整条件1.ColumnEdit = this.repositoryItemLookUpEdit8;
            // DataWordLookUpEditHelper.Init(lue_估价目的, "PURPOSE", "Name", "Name");
            DataWordLookUpEditHelper.Init(lue_业务来源, "YWLY", "Name", "Name");
            DataWordLookUpEditHelper.Init(lue_业务联系人, "YWLXR", "Name", "Name");
            DataWordLookUpEditHelper.Init(lue_财务业务来源, "YWLY", "Name", "Name");
            DataWordLookUpEditHelper.Init(lue_财务业务联系人, "YWLXR", "Name", "Name");
            DataWordLookUpEditHelper.Init(lue_现场调查人员, "XCDCRY", "Name", "Name");
            DataWordLookUpEditHelper.Init(cbe收款状态, "SKZT");
            DataWordLookUpEditHelper.Init(txt财务正式报告档案报告领取人员, "YWLXR");
            DataWordLookUpEditHelper.Init(txt正式报告档案报告领取人员, "YWLXR");
            DataWordLookUpEditHelper.Init(txt初评档案报告领取人员, "YWLXR");
            // DataWordLookUpEditHelper.Init(lue_价值类型, "JZLX", "Name", "Name");
            // DataWordLookUpEditHelper.Init(lue_估价师, "GJS", "Name", "Name");
            //}
        }


        #region 对表单基类数据绑定事件的重写
        protected override void BeforeBindData()
        {
            base.BeforeBindData();
        }

        protected override void AfterBindData()
        {
            base.AfterBindData();
            m_dstAll = GetAllData();
            BBindData();
            this.txt_ProjectId.Text = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProjectId, "");
            this.lbl_委托方.Text = base.GetControlBindValue(this.txt委托方).ToString();
            this.lbl_业务来源.Text = base.GetControlBindValue(this.lue_业务来源).ToString();
            SetControlState();
            //for (int i = gridview_sf.RowCount; i >= 0; i--)
            //{
                gridview_sf.ExpandAllGroups();
            //}
        }

        protected override void BeforeEndEdit()
        {
            base.BeforeEndEdit();
        }

        protected override void BeforeSave()
        {
            base.BeforeSave();
        }

        protected override void AfterSave()
        {
            base.AfterSave();
            SaveAllData();
        }

        protected override void SetControlDisable(Control ctr)
        {
            base.SetControlDisable(ctr);
        }

        public override void SetFormPermission(FormPermission formPermission, DAODataForm ddf, bool saveEnable)
        {
            this.tblData.SelectedPageChanged -= new DevExpress.XtraTab.TabPageChangedEventHandler(this.tblData_SelectedPageChanged);
            base.SetFormPermission(formPermission, ddf, saveEnable);
            this.tblData.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.tblData_SelectedPageChanged);

        }

        protected override bool SelfBind(Control control, DataTable dataSource, string memberName, string[] valueCollection)
        {
            return base.SelfBind(control, dataSource, memberName, valueCollection);
        }
        #endregion

        #region 打印控制
        private void UpdateReport()
        {
            try
            {
                string strProjectID = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProjectId, "");
                string strSQLDelete = @"delete from dbo.Yw_Gujia_Report where 
                                    Yw_Gujia_Report.PROJECT_ID = '{0}' ";
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

        private void InitReport()
        {
            string strActdefId = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PActdefId, "");
            string strProjectID = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProjectId, "");
            string strSQLInsert = @"insert into dbo.Yw_Gujia_Report(ReportID,PROJECT_ID,TEMPLETPRINT_ID,ReportName,ReportData,REPLICATION_VERSION)
                                    select newid(),'{0}',DF_TEMPLETPRINT.TEMPLETPRINT_ID,DF_TEMPLETPRINT.TEMPLETPRINT_NAME,DF_TEMPLETPRINT.TEMPLETPRINT_DATA,DF_TEMPLETPRINT.REPLICATION_VERSION
                                    from dbo.WF_ACTDEFFORMPERMISSION  inner join dbo.DF_TEMPLETPRINT_PRINTSET 
                                    on WF_ACTDEFFORMPERMISSION.PRINTSET_ID = dbo.DF_TEMPLETPRINT_PRINTSET.PRINTSET_ID
                                    inner join dbo.DF_TEMPLETPRINT on dbo.DF_TEMPLETPRINT_PRINTSET.TEMPLETPRINT_ID = dbo.DF_TEMPLETPRINT.TEMPLETPRINT_ID
                                    where  ACTDEF_ID ='{1}'  and not exists(select * from Yw_Gujia_Report where Yw_Gujia_Report.TEMPLETPRINT_ID = DF_TEMPLETPRINT.TEMPLETPRINT_ID and
                                    Yw_Gujia_Report.PROJECT_ID = '{0}' )";
            strSQLInsert = string.Format(strSQLInsert, strProjectID, strActdefId);
            SkyMap.Net.DAO.QueryHelper.ExecuteSql("Default", string.Empty, strSQLInsert);
        }

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

        private void AwokeUpdateReport()
        {
            string strActdefId = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PActdefId, "");
            string strProjectID = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProjectId, "");


            //========================
            string tmpsql = "select count(1) as tcount from Yw_Gujia_Report d where project_id='{0}'";
            tmpsql = string.Format(tmpsql, strProjectID);
            DataTable tmpTable = SkyMap.Net.DAO.QueryHelper.ExecuteSql("Default", string.Empty, tmpsql);
            if (tmpTable.Rows[0]["tcount"].Equals(0))
            {
                UpdateReport();
            }
            //=========================
            string sql = @"select  count(1) as rcount from  dbo.WF_ACTDEFFORMPERMISSION 
                         inner join dbo.DF_TEMPLETPRINT_PRINTSET 
                        on WF_ACTDEFFORMPERMISSION.PRINTSET_ID = dbo.DF_TEMPLETPRINT_PRINTSET.PRINTSET_ID
                        inner join dbo.DF_TEMPLETPRINT f on dbo.DF_TEMPLETPRINT_PRINTSET.TEMPLETPRINT_ID = f.TEMPLETPRINT_ID
                        where  ACTDEF_ID ='{1}'   and  f.templetprint_id in(select templetprint_id from Yw_Gujia_Report d where project_id='{0}' and 

f.templetprint_id=d.templetprint_id and f.REPLICATION_VERSION!=d.REPLICATION_VERSION)";
            sql = string.Format(sql, strProjectID, strActdefId);
            DataTable oldTable = SkyMap.Net.DAO.QueryHelper.ExecuteSql("Default", string.Empty, sql);

            if (!oldTable.Rows[0]["rcount"].Equals(0))
            {
                if (MessageBox.Show("有新版本报表，是否更新?", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    UpdateReport();

            }
        }

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
                //if (tab交件信息.PageVisible == true && tab地块信息.PageVisible == false && tab镇区审核.PageVisible == false && tab招标办审核.PageVisible == false)
                //{
                //    if (strDescription.IndexOf("receipt") != -1)
                //    {
                PrintSet.TempletPrints.Add(printSet.TempletPrints[i]);
                //    }
                //}

                //else
                //{
                //    //过滤规则1：交易情况，2：业务类型，3：业务版本  备注：*代表不作区分
                //    if ((strDescription.IndexOf("receipt") != -1) || (strDescription.IndexOf("detail") != -1) || ((strDescription.Substring(0, 1) == m_strChange || strDescription.Substring(0, 1) == "*") && (strDescription.Substring(1, 1) == m_strType || strDescription.Substring(1, 1) == "*") && (strDescription.Substring(2, 1) == m_strEdition || strDescription.Substring(2, 1) == "*") && ((strDescription.Substring(3, 1) == "Y" ? true : false) == m_blnWatch || strDescription.Substring(3, 1) == "*") && ((strDescription.Substring(4, 1) == "Y" ? true : false) == m_blnConsign || strDescription.Substring(4, 1) == "*")))//strDescription.IndexOf(cmb_exchange1.Text) != -1&& 
                //    {
                //PrintSet.TempletPrints.Add(printSet.TempletPrints[i]);
                //}
                //}
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


            //if (tab交件信息.PageVisible == true && (tab地块信息.PageVisible == true || tab镇区审核.PageVisible == true) && tab招标办审核.PageVisible == false)
            //{
            //    RemovePrint(ref PrintSet, "04");
            //    RemovePrint(ref PrintSet, "05");
            //    RemovePrint(ref PrintSet, "07");
            //    RemovePrint(ref PrintSet, "09");
            //    RemovePrint(ref PrintSet, "12");
            //    RemovePrint(ref PrintSet, "15");
            //    RemovePrint(ref PrintSet, "22");
            //    RemovePrint(ref PrintSet, "25");
            //    RemovePrint(ref PrintSet, "29");
            //    RemovePrint(ref PrintSet, "31");
            //    RemovePrint(ref printSet, "32");
            //}

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

            //            string strYW_tdzbpm_td = @"SELECT dkid AS Expr1, PROJECT_ID AS Expr2, ISNULL(地块编号, N'') AS 地块编号, 
            //                                      ISNULL(权属单位, N'') AS 权属单位, ISNULL(土地位置坐落, N'') AS 土地位置坐落, 
            //                                      ISNULL(建设用地批文号, N'') AS 建设用地批文号, ISNULL(用地现状, N'') 
            //                                      AS 用地现状, ISNULL(土地使用证编号, N'') AS 土地使用证编号, 
            //                                      CASE WHEN YW_tdzbpm_td.原产权单位提供的底价 IS NOT NULL 
            //                                      THEN CONVERT(varchar(20), YW_tdzbpm_td.原产权单位提供的底价) 
            //                                      ELSE '' END AS 原产权单位提供的底价, ISNULL(原土地用途, N'') AS 原土地用途, 
            //                                      ISNULL(原土地性质, N'') AS 原土地性质, 
            //                                      CASE WHEN YW_tdzbpm_td.用地面积 IS NOT NULL THEN CONVERT(varchar(20), 
            //                                      YW_tdzbpm_td.用地面积) ELSE '' END AS 用地面积, ISNULL(图纸编号, N'') 
            //                                      AS 图纸编号, ISNULL(勘察情况, N'') AS 勘察情况, ISNULL(税费完善情况, N'') 
            //                                      AS 税费完善情况, ISNULL(权属争议, N'') AS 权属争议, ISNULL(清拆补偿负责人, N'') 
            //                                      AS 清拆补偿负责人, 
            //                                      CASE WHEN YW_tdzbpm_td.保证金冲减地价款期数 IS NOT NULL 
            //                                      THEN CONVERT(varchar(20), YW_tdzbpm_td.保证金冲减地价款期数) 
            //                                      ELSE '' END AS 保证金冲减地价款期数, ISNULL(过户税费, N'') AS 过户税费, 
            //                                      ISNULL(查封情况, N'') AS 查封情况, ISNULL(抵押情况, N'') AS 抵押情况, 
            //                                      ISNULL(是否有保留价, N'') AS 是否有保留价, 宗地图, 万分一图, 现状图, 规划图, 
            //                                      地价图,'wordml://'+PROJECT_ID+'Z.jpg' as 宗地图号,'wordml://'+PROJECT_ID+'W.jpg' as  万分一图号,'wordml://'+PROJECT_ID+'X.jpg' as  现状图号,'wordml://'+PROJECT_ID+'G.jpg' as  规划图号, 
            //                                      'wordml://'+PROJECT_ID+'D.jpg' as  地价图号, 图纸编号
            //                                      FROM dbo.YW_tdzbpm_td where YW_tdzbpm_td.PROJECT_ID = '" + strProjectId + @"'
            //                                      FOR XML AUTO,ELEMENTS,BINARY BASE64";


            //            string strMATER = @"SELECT PROINSTMATER_ID, isnull(PROINSTMATER_NAME,'') as PROINSTMATER_NAME, PROINST_ID, isnull(OLD_NUM,'') as OLD_NUM, 
            //                                isnull(DUPL_NUM,'') as DUPL_NUM, isnull(PROINSTMATER_MEM,'') as PROINSTMATER_MEM, REPLICATION_VERSION
            //                                FROM WF_PROINSTMATER where  PROINST_ID = '" + strPROINST_ID + @"' and SELECTED = 1 
            //                                FOR XML AUTO,ELEMENTS";
            //            dtsYW_tdzbpm = SkyMap.Net.DAO.QueryHelper.ExecuteSqls("Default", string.Empty, new string[] { strYW_tdzbpm_td, strMATER }, new string[] { "YW_tdzbpm_td", "WF_PROINSTMATER" });

            IDA0 dao = DAOFactory.GetInstanceByNameSpace("SkyMap.Net.DAO");
            IDbConnection dbConn = dao.Connection;
            IDbCommand dbCmd;
            try
            {
                dbCmd = dbConn.CreateCommand();
                dbCmd.CommandType = CommandType.StoredProcedure;
                dbCmd.CommandText = "ProcGujiaxml";

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

            strXmlData = "<Root xmlns=\"http://www.zgfdc.net/gujia\">" + strXmlData + "</Root>";
            doc.LoadXml(strXmlData);

            //strXmlData = "";
            //foreach (DataRow dr in dtsYW_tdzbpm.Tables["YW_tdzbpm_td"].Rows)
            //{
            //    strXmlData += dr[0].ToString();
            //}

            //strXmlData = strXmlData.Replace("dbo.", "");
            //strXmlData = strXmlData.Replace("xmlns=\"\"", "");
            //XmlDocument docYW_tdzbpm_td = new XmlDocument();
            //strXmlData = "<Root xmlns=\"http://www.zgfdc.net/gujia\">" + strXmlData + "</Root>";
            //docYW_tdzbpm_td.LoadXml(strXmlData);

            XmlNode node;
            //foreach (XmlNode nodeTD in docYW_tdzbpm_td.FirstChild.ChildNodes)
            //{
            //    node = doc.ImportNode(nodeTD, true);
            //    doc.FirstChild.FirstChild.AppendChild(node);
            //}

            strXmlData = "";

            dbCmd = dbConn.CreateCommand();
            dbCmd.CommandType = CommandType.StoredProcedure;
            dbCmd.CommandText = "ProcGujiaBxml";
            dbCmd.Parameters.Add(new SqlParameter("@bid", SqlDbType.Int));
            dbCmd.Parameters.Add(new SqlParameter("@Pglx", SqlDbType.NVarChar, 50));
            if (strReportName.IndexOf("土地") >= 0)
            {
                ((SqlParameter)dbCmd.Parameters["@Pglx"]).Value = "土地";
            }
            else if (strReportName.IndexOf("房地产") >= 0)
            {
                ((SqlParameter)dbCmd.Parameters["@Pglx"]).Value = "房地产";
            }
            else
            {
                MessageHelper.ShowInfo("只允许评估房地产和土地类型");
                return false;
            }

            if (strDescription.EndsWith("B"))
            {
                if (typeof(DevExpress.XtraGrid.Views.BandedGrid.BandedGridView) != gridB1.FocusedView.GetType())
                {
                    MessageHelper.ShowInfo("请选择报告");
                    return false;
                }
                DataRow dr = ((DevExpress.XtraGrid.Views.BandedGrid.BandedGridView)gridB1.FocusedView).GetDataRow(((DevExpress.XtraGrid.Views.BandedGrid.BandedGridView)gridB1.FocusedView).FocusedRowHandle);
                if (dr == null)
                {
                    MessageHelper.ShowInfo("请选择报告");
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
                foreach (DataRow dr in m_dstAll.Tables["yw_gujia_b"].Rows)
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
                            LoggingService.Debug("出错报表" + e.Message);
                            MessageHelper.ShowError("出错", e);
                        }
                    }
                }
            }
            strXmlData = strXmlData.Replace("dbo.", "");
            strXmlData = strXmlData.Replace("xmlns=\"\"", "");
            XmlDocument docyw_gujia_b = new XmlDocument();
            strXmlData = "<Root xmlns=\"http://www.zgfdc.net/gujia\">" + strXmlData + "</Root>";
            docyw_gujia_b.LoadXml(strXmlData);
            foreach (XmlNode nodeB in docyw_gujia_b.FirstChild.ChildNodes)
            {
                node = doc.ImportNode(nodeB, true);
                doc.FirstChild.FirstChild.AppendChild(node);
            }

            //===============================对xml进行处理；
            string tmperror = "";
            string tmpstring = doc.InnerXml.ToString();
            string wtf = "";
            XElement xe = XElement.Parse(tmpstring);
            XNamespace ns = "http://www.zgfdc.net/gujia";
            var ss = from el in xe.Descendants(ns + "yw_gujia") select el;
            foreach (XElement x in ss)
            {
                wtf = x.Element(ns + "委托方").Value.ToString();
            }
            string[] tmpzj = { "sss", "yyy", "wwww" };
            string[] tmpzq = { };
            var yy = from el in xe.Descendants(ns + "yw_gujia_b") select el;
            try
            {
                foreach (XElement x in yy)
                {
                    string stryxzj = "";
                    string strtdz = "";
                    string strfcz = "";
                    string strfcgyz = "";
                    System.Collections.ArrayList arraylistzj;
                    System.Collections.ArrayList arraylistfj;
                    tmperror = "有效证件不能为空";
                    stryxzj = x.Element(ns + "有效证件").Value != null ? x.Element(ns + "有效证件").Value.ToString() : "";//有效证件
                    if (stryxzj.IndexOf("土地") >= 0)
                    {
                        tmperror = "有效证件包含： " + stryxzj + ",土地证号不能为空";
                        strtdz = x.Element(ns + "土地证号").Value != null ? x.Element(ns + "土地证号").Value.ToString() : "";//土地证
                    }
                    if (stryxzj.IndexOf("房地产权证") >= 0 || stryxzj.IndexOf("广东省房地产权证") >= 0)
                    {
                        tmperror = "有效证件包含： " + stryxzj + ",房地产权证不能为空";
                        strfcz = x.Element(ns + "房地产权证").Value != null ? x.Element(ns + "房地产权证").Value.ToString() : "";//房产证
                    }
                    if (stryxzj.IndexOf("房地产权共有（用）证") >= 0)
                    {
                        tmperror = "有效证件包含： " + stryxzj + ",房地产共有用证不能为空";
                        strfcgyz = x.Element(ns + "房地产共有用证").Value != null ? x.Element(ns + "房地产共有用证").Value.ToString() : "";//房产共有证
                    }
                    tmperror = "1.正在准备附件描述";
                    arraylistfj = AppraiseReport.Fjms(stryxzj, strtdz, strfcz, strfcgyz);
                    x.Add(new XElement(ns + "yw_fj", from att in arraylistfj.ToArray()
                                                     select new XElement(ns + "附件", att.ToString())
                    ));
                    tmperror = "1.正在准备证件描述";
                    arraylistzj = AppraiseReport.Zjms(stryxzj, strtdz, strfcz, strfcgyz);
                    x.Add(new XElement(ns + "yw_zj", from att in arraylistzj.ToArray()
                                                     select new XElement(ns + "证件", att.ToString())
                    ));
                    x.Add(new XElement(ns + "yw_zj1", from att in arraylistzj.ToArray()
                                                      select new XElement(ns + "证件1", att.ToString())
                   ));
                    x.Add(new XElement(ns + "yw_zj2", from att in arraylistzj.ToArray()
                                                      select new XElement(ns + "证件2", att.ToString())
                   ));
                    tmperror = "2.委托方添加";
                    x.Add(new XElement(ns + "委托方", wtf));
                    tmperror = "3.评估确定房地产单价为空";
                    x.Add(new XElement(ns + "评估确定房地产单价大写", AppraiseReport.MoneyDx(x.Element(ns + "评估确定房地产单价").Value.ToString())));
                    x.SetElementValue(ns + "评估确定房地产单价", AppraiseReport.MoneyXx(x.Element(ns + "评估确定房地产单价").Value.ToString()));
                    tmperror = "4.评估确定房地产总价为空";
                    x.Add(new XElement(ns + "评估确定房地产总价大写", AppraiseReport.MoneyDx(x.Element(ns + "评估确定房地产总价").Value.ToString())));
                    x.SetElementValue(ns + "评估确定房地产总价", AppraiseReport.MoneyXx(x.Element(ns + "评估确定房地产总价").Value.ToString()));
                    tmperror = "5.评估确定土地总价为空";
                    x.Add(new XElement(ns + "评估确定土地总价大写", AppraiseReport.MoneyDx(x.Element(ns + "评估确定土地总价").Value.ToString())));
                    x.SetElementValue(ns + "评估确定土地总价", AppraiseReport.MoneyXx(x.Element(ns + "评估确定土地总价").Value.ToString()));
                    tmperror = "6.评估确定土地单价为空";
                    x.Add(new XElement(ns + "评估确定土地单价大写", AppraiseReport.MoneyDx(x.Element(ns + "评估确定土地单价").Value.ToString())));
                    x.SetElementValue(ns + "评估确定土地单价", AppraiseReport.MoneyXx(x.Element(ns + "评估确定土地单价").Value.ToString()));
                    tmperror = "7.评估确定房产价值为空";
                    x.Add(new XElement(ns + "评估确定房产价值大写", AppraiseReport.MoneyDx(x.Element(ns + "评估确定房产价值").Value.ToString())));
                    x.SetElementValue(ns + "评估确定房产价值", AppraiseReport.MoneyXx(x.Element(ns + "评估确定房产价值").Value.ToString()));
                    tmperror = "8.评估确定房产单价为空";
                    x.Add(new XElement(ns + "评估确定房产单价大写", AppraiseReport.MoneyDx(x.Element(ns + "评估确定房产单价").Value.ToString())));
                    x.SetElementValue(ns + "评估确定房产单价", AppraiseReport.MoneyXx(x.Element(ns + "评估确定房产单价").Value.ToString()));
                    tmperror = "9.评估确定楼面地价为空";
                    x.Add(new XElement(ns + "评估确定楼面地价大写", AppraiseReport.MoneyDx(x.Element(ns + "评估确定楼面地价").Value.ToString())));
                    x.SetElementValue(ns + "评估确定楼面地价", AppraiseReport.MoneyXx(x.Element(ns + "评估确定楼面地价").Value.ToString()));
                    tmperror = "10.土地使用者或座落或评估类型或价值类型或估价目的为空";
                    x.Add(new XElement(ns + "长估价目的", AppraiseReport.ReportPurpose(x.Element(ns + "土地使用者").Value.ToString(), x.Element(ns + "座落").Value.ToString(), x.Element(ns + "评估类型").Value.ToString(), x.Element(ns + "价值类型").Value.ToString(), x.Element(ns + "估价目的").Value.ToString())));
                    tmperror = "11.估价目的或价值类型为空";
                    x.Add(new XElement(ns + "价值定义", "999"));
                    tmperror = "12.项目编号生成出错";
                    x.Add(new XElement(ns + "项目编号", AppraiseReport.ProjectBh(x.Element(ns + "报告名").Value.ToString())));
                    tmperror = "13.初报告编号生成出错";
                    x.Add(new XElement(ns + "初报告编号", AppraiseReport.ReportBh(x.Element(ns + "报告名").Value.ToString())));
                    tmperror = "14.正报告编号出错";
                    x.Add(new XElement(ns + "正报告编号", AppraiseReport.ReportBh(x.Element(ns + "报告名").Value.ToString())));
                    tmperror = "15.技报告编号生成出错";
                    x.Add(new XElement(ns + "技报告编号", AppraiseReport.ReportBh(x.Element(ns + "报告名").Value.ToString())));
                    tmperror = "15.报告开始日期或报告结束日期为空";
                    x.Add(new XElement(ns + "开始日期短", AppraiseReport.SetValue("开始日期(短)", x.Element(ns + "报告开始时间").Value.ToString())));
                    x.Add(new XElement(ns + "开始日期长", AppraiseReport.SetValue("开始日期(长)", x.Element(ns + "报告开始时间").Value.ToString())));
                    x.Add(new XElement(ns + "结束日期长", AppraiseReport.SetValue("结束日期(长)", x.Element(ns + "报告结束时间").Value.ToString())));
                    x.Add(new XElement(ns + "结束日期短", AppraiseReport.SetValue("结束日期(短)", x.Element(ns + "报告结束时间").Value.ToString())));
                    x.Add(new XElement(ns + "开始日期长数字", AppraiseReport.SetValue("开始日期(长数字)", x.Element(ns + "报告开始时间").Value.ToString())));
                    x.Add(new XElement(ns + "结束日期长数字", AppraiseReport.SetValue("结束日期(长数字)", x.Element(ns + "报告结束时间").Value.ToString())));

                    tmperror = "16.估价期日为空";
                    x.Add(new XElement(ns + "估价期日长", AppraiseReport.SetValue("估价期日(长)", x.Element(ns + "估价基准日").Value.ToString())));
                    x.Add(new XElement(ns + "估价期日长数字", AppraiseReport.SetValue("估价期日(长数字)", x.Element(ns + "估价基准日").Value.ToString())));
                    tmperror = "17.有效年限长";
                    x.Add(new XElement(ns + "有效年期长", AppraiseReport.SetValue("有效年期(长)", x.Element(ns + "报告有效期限").Value.ToString())));
                    //==================

                    //==================
                    tmperror = "19. 价格精度";
                    x.SetElementValue(ns + "价格精度大写", x.Element(ns + "总价精度").Value.ToString());

                    tmperror = "19.镇区或评估类型为空";
                    System.Collections.ArrayList arraylist;
                    arraylist = AppraiseReport.Zqms(x.Element(ns + "镇区").Value.ToString(), x.Element(ns + "评估类型").Value.ToString());
                    //==================
                    x.Add(new XElement(ns + "yw_zq", from att in arraylist.ToArray()
                                                     select new XElement(ns + "描述", att.ToString())

                   ));
                    x.Add(new XElement(ns + "yw_zq1", from att in arraylist.ToArray()
                                                      select new XElement(ns + "描述1", att.ToString())

                    ));
                    x.Add(new XElement(ns + "yw_zq2", from att in arraylist.ToArray()
                                                      select new XElement(ns + "描述2", att.ToString())

                    ));
                    tmperror = "20.报告到期时间长为空";
                    x.Add(new XElement(ns + "报告到期时间长", AppraiseReport.SetValue("报告到期时间(长)", x.Element(ns + "报告到期时间").Value.ToString())));
                    x.Add(new XElement(ns + "报告到期时间长数字", AppraiseReport.SetValue("报告到期时间(长数字)", x.Element(ns + "报告到期时间").Value.ToString())));
                    tmperror = "21.土地终止日期为空";
                    if (HasElementReturnString(x, ns, "土地终止日期") == "")
                    {
                        x.Add(new XElement(ns + "土地终止日期长", "-"));
                        x.SetElementValue(ns + "土地终止日期", "-");
                        x.Add(new XElement(ns + "土地终止日期长数字", "-"));
                    }
                    else
                    {
                        x.Add(new XElement(ns + "土地终止日期长", AppraiseReport.SetValue("土地终止日期(长)", x.Element(ns + "土地终止日期").Value.ToString())));
                        x.SetElementValue(ns + "土地终止日期", AppraiseReport.SetValue("土地终止日期(长数字)", x.Element(ns + "土地终止日期").Value.ToString()));
                        x.Add(new XElement(ns + "土地终止日期长数字", AppraiseReport.SetValue("土地终止日期(长数字)", x.Element(ns + "土地终止日期").Value.ToString())));
                    }
                    x.Add(new XElement(ns + "评估方法描述", ""));
                    x.Add(new XElement(ns + "本估评师", (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PStaffName, "")));
                    tmperror = "18.位置区域和环境为空";
                    // x.SetElementValue(ns + "位置区域和环境", Convert.ToBase64String(Encoding.Default.GetBytes((x.Element(ns + "位置区域和环境").Value.ToString()))));
                    x.SetElementValue(ns + "位置区域和环境", x.Element(ns + "位置区域和环境").Value.ToString());

                    //==============================start生成xml后需要再复制生成相同的节点eg:<root><gujia><gujia_b><name>text</name><name_1>text</name_1><name_2>text</name2></gujia_b></gujia></root>
                    var hh = from el in x.Elements() select el;
                    XElement tmpxe = XElement.Parse("<Root xmlns=\"http://www.zgfdc.net/gujia\"></Root>");
                    foreach (XElement xx in hh)
                    {
                        if (!xx.HasElements)
                        {
                            tmpxe.SetElementValue(ns + xx.Name.LocalName.ToString() + "_1", xx.Value.ToString());
                            tmpxe.SetElementValue(ns + xx.Name.LocalName.ToString() + "_2", xx.Value.ToString());
                            tmpxe.SetElementValue(ns + xx.Name.LocalName.ToString() + "_3", xx.Value.ToString());
                            tmpxe.SetElementValue(ns + xx.Name.LocalName.ToString() + "_4", xx.Value.ToString());
                            tmpxe.SetElementValue(ns + xx.Name.LocalName.ToString() + "_5", xx.Value.ToString());
                            tmpxe.SetElementValue(ns + xx.Name.LocalName.ToString() + "_6", xx.Value.ToString());
                            tmpxe.SetElementValue(ns + xx.Name.LocalName.ToString() + "_7", xx.Value.ToString());
                        }
                    }
                    foreach (XElement xx in tmpxe.Elements())
                    {
                        x.SetElementValue(ns + xx.Name.LocalName.ToString(), xx.Value.ToString());
                    }
                    //==============================end
                }

                //XmlDocument xcopyto = new XmlDocument();
                //strXmlData = "<Root xmlns=\"http://www.zgfdc.net/gujia\">" + "</Root>";
                //xcopyto.LoadXml(strXmlData);
                //XmlDocument xcopyfrom = new XmlDocument();
                //xcopyfrom.LoadXml(xe.ToString());
                //foreach (XmlNode nodeB in xcopyfrom.FirstChild.ChildNodes)
                //{
                //    node = xcopyto.ImportNode(nodeB, true);
                //    xcopyto.FirstChild.AppendChild(node);
                //}
                //string xcopytostring = xcopyto.InnerXml.ToString();
                //XElement xcopytoxe = XElement.Parse(tmpstring);
                //var xcopytoss = from el in xcopytoxe.Descendants(ns + "yw_gujia_b") select el;
                //foreach (XElement x in xcopytoss)
                //{
                //    xe.SetElementValue(ns + x.Name.LocalName.ToString(), "111");
                //}
                //#region==============================2009.9.4 收费通知书
                ////HasElementReturnString(x, ns, "座落");
                ////评估类型,评估总值,评估对象,评估地址,评估日期,评估编号,标准收费,折扣率,折扣后价值大写,折扣后价值小写,开户帐号,开户行,开户名称
                //string straddress="";//评估地址
                //string strpgbh="";
                //string strpglx="";
                //var varywgujiab = from el in xe.Descendants(ns + "yw_gujia_b") select el;
                //foreach (XElement x in varywgujiab)
                //{
                //    if (HasElement(x, ns, "收费标准"))
                //    {
                //        if (HasElementReturnString(x, ns, "收费标准").IndexOf("房地产") >= 0 || HasElementReturnString(x, ns, "收费标准").IndexOf("土地") >= 0)
                //        {
                //            if (strpglx != "房地产")
                //            {
                //                if (HasElementReturnString(x, ns, "收费标准").IndexOf("房地产") >= 0) strpglx = "房地产";
                //                if (HasElementReturnString(x, ns, "收费标准").IndexOf("土地") >= 0) strpglx = "土地";
                //            }
                //            if (double.Parse(HasElementReturnString(x, ns, "标准收费")) > 0)
                //            {
                //                straddress = straddress + HasElementReturnString(x,ns, "座落") + "/";
                //                strpgbh = strpgbh + HasElementReturnString(x, ns, "正报告编号") + "/";
                //            }                            
                //        }
                //        straddress = straddress.Substring(0, straddress.Length - 1);
                //        strpgbh = strpgbh.Substring(0, strpgbh.Length - 1);
                //    }
                //}
                //var varywgujia = from el in xe.Descendants(ns + "yw_gujia") select el;
                //foreach (XElement x in varywgujia)
                //{
                //    if (HasElement(x, ns, "委托方"))
                //    {
                //        x.SetElementValue(ns + "评估类型", strpglx);
                //        x.SetElementValue(ns + "评估地址", straddress);
                //        x.SetElementValue(ns + "评估编号", strpgbh);
                //        x.SetElementValue(ns + "评估总值", AppraiseClass.S4J5(HasElementReturnDouble(x, ns, "评估总值"),0).ToString("#,#"));
                //        x.SetElementValue(ns + "标准收费", AppraiseClass.S4J5(HasElementReturnDouble(x, ns, "标准收费"),0).ToString("#,#"));
                //        x.SetElementValue(ns + "折扣率", AppraiseClass.S4J5(HasElementReturnDouble(x, ns, "折扣率") * 10, 1));
                //        x.SetElementValue(ns + "实际收费", AppraiseClass.S4J5(HasElementReturnDouble(x, ns, "实际收费"), 0).ToString("#,#"));
                //        x.SetElementValue(ns + "实际收费大写",AppraiseReport.MoneyDx(AppraiseClass.S4J5(HasElementReturnDouble(x, ns, "实际收费"), 0).ToString("#,#")));
                //        x.SetElementValue(ns + "评估日期", string.Format("{0}年{1}月{2}日", DateTime.Parse(HasElementReturnString(x, ns, "报告开始时间")).Year, DateTime.Parse(HasElementReturnString(x, ns, "报告开始时间")).Month, DateTime.Parse(HasElementReturnString(x, ns, "报告开始时间")).Day));
                //        x.SetElementValue(ns + "收费通知书日期", string.Format("{0}年{1}月{2}日", DateTime.Parse(DateTime.Now.ToString()).Year, DateTime.Parse(DateTime.Now.ToString()).Month, DateTime.Parse(DateTime.Now.ToString()).Day));
                //        x.SetElementValue(ns + "开户帐号", "ssss");
                //        x.SetElementValue(ns + "开户行", "name");
                //        x.SetElementValue(ns + "开户名称", strpglx);
                //    }
                //}
                //#endregion==============================2009.9.4 收费通知书
            }
            catch (Exception ex)
            {
                MessageHelper.ShowInfo(tmperror);
            }


            xe.Save(strFilePath);
            //XElement xe = XElement.Load("test.xml");
            //var ss = from el in xe.Elements("yw_gujia").Elements("yw_gujia_b") select el;
            //foreach (XElement x in ss)
            //{
            //    MessageBox.Show(x.ToString());
            //}
            //===============================
            //strXmlData = "";
            //foreach (DataRow dr in dtsYW_tdzbpm.Tables["WF_PROINSTMATER"].Rows)
            //{
            //    strXmlData += dr[0].ToString();
            //}
            //strXmlData = strXmlData.Replace("dbo.", "");
            //strXmlData = strXmlData.Replace("xmlns=\"\"", "");
            //XmlDocument docWF_PROINSTMATER = new XmlDocument();
            //strXmlData = "<Root xmlns=\"http://www.zgfdc.net/gujia\">" + strXmlData + "</Root>";
            //docWF_PROINSTMATER.LoadXml(strXmlData);
            //foreach (XmlNode nodeB in docWF_PROINSTMATER.FirstChild.ChildNodes)
            //{
            //    node = doc.ImportNode(nodeB, true);
            //    doc.FirstChild.FirstChild.AppendChild(node);
            //}

            return true;
        }

        #region 生成明收费通知书/明细表的xml
        private bool CreateMxb(string strFilePath)
        {
            string strProjectId;
            string strPROINST_ID;
            DataSet dtsYW_tdzbpm;
            string strXmlData = "";

            strProjectId = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProjectId, "");
            strPROINST_ID = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProinstId, "");

            IDA0 dao = DAOFactory.GetInstanceByNameSpace("SkyMap.Net.DAO");
            IDbConnection dbConn = dao.Connection;
            IDbCommand dbCmd;
            try
            {
                dbCmd = dbConn.CreateCommand();
                dbCmd.CommandType = CommandType.StoredProcedure;
                dbCmd.CommandText = "ProcGujiaxml";

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

            strXmlData = "<Root xmlns=\"http://www.zgfdc.net/gujia\">" + strXmlData + "</Root>";
            doc.LoadXml(strXmlData);

            //strXmlData = "";
            //foreach (DataRow dr in dtsYW_tdzbpm.Tables["YW_tdzbpm_td"].Rows)
            //{
            //    strXmlData += dr[0].ToString();
            //}

            //strXmlData = strXmlData.Replace("dbo.", "");
            //strXmlData = strXmlData.Replace("xmlns=\"\"", "");
            //XmlDocument docYW_tdzbpm_td = new XmlDocument();
            //strXmlData = "<Root xmlns=\"http://www.zgfdc.net/gujia\">" + strXmlData + "</Root>";
            //docYW_tdzbpm_td.LoadXml(strXmlData);

            XmlNode node;
            //foreach (XmlNode nodeTD in docYW_tdzbpm_td.FirstChild.ChildNodes)
            //{
            //    node = doc.ImportNode(nodeTD, true);
            //    doc.FirstChild.FirstChild.AppendChild(node);
            //}

            strXmlData = "";

            dbCmd = dbConn.CreateCommand();
            dbCmd.CommandType = CommandType.StoredProcedure;
            dbCmd.CommandText = "ProcGujiaBMxxml";
            dbCmd.Parameters.Add(new SqlParameter("@bid", SqlDbType.Int));
            dbCmd.Parameters.Add(new SqlParameter("@Pglx", SqlDbType.NVarChar, 50));
            ((SqlParameter)dbCmd.Parameters["@Pglx"]).Value = DBNull.Value;

            foreach (DataRow dr in m_dstAll.Tables["yw_gujia_b"].Rows)
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
                        LoggingService.Debug("出错报表" + e.Message);
                        MessageHelper.ShowError("出错", e);
                    }
                }
            }

            strXmlData = strXmlData.Replace("dbo.", "");
            strXmlData = strXmlData.Replace("xmlns=\"\"", "");
            XmlDocument docyw_gujia_b = new XmlDocument();
            strXmlData = "<Root xmlns=\"http://www.zgfdc.net/gujia\">" + strXmlData + "</Root>";
            docyw_gujia_b.LoadXml(strXmlData);
            foreach (XmlNode nodeB in docyw_gujia_b.FirstChild.ChildNodes)
            {
                node = doc.ImportNode(nodeB, true);
                doc.FirstChild.FirstChild.AppendChild(node);
            }

            //===============================对xml进行处理；
            string tmperror = "";
            string tmpstring = doc.InnerXml.ToString();
            string wtf = "";
            XElement xe = XElement.Parse(tmpstring);
            XNamespace ns = "http://www.zgfdc.net/gujia";
            var ss = from el in xe.Descendants(ns + "yw_gujia") select el;
            foreach (XElement x in ss)
            {
                wtf = x.Element(ns + "委托方").Value.ToString();
            }
            string[] tmpzj = { "sss", "yyy", "wwww" };
            string[] tmpzq = { };
            var yy = from el in xe.Descendants(ns + "yw_gujia_b") select el;
            try
            {
                foreach (XElement x in yy)
                {
                    string stryxzj = "";
                    string strtdz = "";
                    string strfcz = "";
                    string strfcgyz = "";
                    System.Collections.ArrayList arraylistzj;
                    System.Collections.ArrayList arraylistfj;
                    tmperror = "有效证件不能为空";
                    stryxzj = x.Element(ns + "有效证件").Value != null ? x.Element(ns + "有效证件").Value.ToString() : "";//有效证件
                    if (stryxzj.IndexOf("土地") >= 0)
                    {
                        tmperror = "有效证件包含： " + stryxzj + ",土地证号不能为空";
                        strtdz = x.Element(ns + "土地证号").Value != null ? x.Element(ns + "土地证号").Value.ToString() : "";//土地证
                    }
                    if (stryxzj.IndexOf("房地产权证") >= 0 || stryxzj.IndexOf("广东省房地产权证") >= 0)
                    {
                        tmperror = "有效证件包含： " + stryxzj + ",房地产权证不能为空";
                        strfcz = x.Element(ns + "房地产权证").Value != null ? x.Element(ns + "房地产权证").Value.ToString() : "";//房产证
                    }
                    if (stryxzj.IndexOf("房地产权共有（用）证") >= 0)
                    {
                        tmperror = "有效证件包含： " + stryxzj + ",房地产共有用证不能为空";
                        strfcgyz = x.Element(ns + "房地产共有用证").Value != null ? x.Element(ns + "房地产共有用证").Value.ToString() : "";//房产共有证
                    }
                    tmperror = "1.正在准备附件描述";
                    arraylistfj = AppraiseReport.Fjms(stryxzj, strtdz, strfcz, strfcgyz);
                    x.Add(new XElement(ns + "yw_fj", from att in arraylistfj.ToArray()
                                                     select new XElement(ns + "附件", att.ToString())
                    ));
                    tmperror = "1.正在准备证件描述";
                    arraylistzj = AppraiseReport.Zjms(stryxzj, strtdz, strfcz, strfcgyz);
                    x.Add(new XElement(ns + "yw_zj", from att in arraylistzj.ToArray()
                                                     select new XElement(ns + "证件", att.ToString())
                    ));
                    x.Add(new XElement(ns + "yw_zj1", from att in arraylistzj.ToArray()
                                                      select new XElement(ns + "证件1", att.ToString())
                   ));
                    x.Add(new XElement(ns + "yw_zj2", from att in arraylistzj.ToArray()
                                                      select new XElement(ns + "证件2", att.ToString())
                   ));
                    tmperror = "2.委托方添加";
                    x.Add(new XElement(ns + "委托方", wtf));
                    tmperror = "3.评估确定房地产单价为空";
                    x.Add(new XElement(ns + "评估确定房地产单价大写", AppraiseReport.MoneyDx(x.Element(ns + "评估确定房地产单价").Value.ToString())));
                    x.SetElementValue(ns + "评估确定房地产单价", AppraiseReport.MoneyXx(x.Element(ns + "评估确定房地产单价").Value.ToString()));
                    tmperror = "4.评估确定房地产总价为空";
                    x.Add(new XElement(ns + "评估确定房地产总价大写", AppraiseReport.MoneyDx(x.Element(ns + "评估确定房地产总价").Value.ToString())));
                    x.SetElementValue(ns + "评估确定房地产总价", AppraiseReport.MoneyXx(x.Element(ns + "评估确定房地产总价").Value.ToString()));
                    tmperror = "5.评估确定土地总价为空";
                    x.Add(new XElement(ns + "评估确定土地总价大写", AppraiseReport.MoneyDx(x.Element(ns + "评估确定土地总价").Value.ToString())));
                    x.SetElementValue(ns + "评估确定土地总价", AppraiseReport.MoneyXx(x.Element(ns + "评估确定土地总价").Value.ToString()));
                    tmperror = "6.评估确定土地单价为空";
                    x.Add(new XElement(ns + "评估确定土地单价大写", AppraiseReport.MoneyDx(x.Element(ns + "评估确定土地单价").Value.ToString())));
                    x.SetElementValue(ns + "评估确定土地单价", AppraiseReport.MoneyXx(x.Element(ns + "评估确定土地单价").Value.ToString()));
                    tmperror = "7.评估确定房产价值为空";
                    x.Add(new XElement(ns + "评估确定房产价值大写", AppraiseReport.MoneyDx(x.Element(ns + "评估确定房产价值").Value.ToString())));
                    x.SetElementValue(ns + "评估确定房产价值", AppraiseReport.MoneyXx(x.Element(ns + "评估确定房产价值").Value.ToString()));
                    tmperror = "8.评估确定房产单价为空";
                    x.Add(new XElement(ns + "评估确定房产单价大写", AppraiseReport.MoneyDx(x.Element(ns + "评估确定房产单价").Value.ToString())));
                    x.SetElementValue(ns + "评估确定房产单价", AppraiseReport.MoneyXx(x.Element(ns + "评估确定房产单价").Value.ToString()));
                    tmperror = "9.评估确定楼面地价为空";
                    x.Add(new XElement(ns + "评估确定楼面地价大写", AppraiseReport.MoneyDx(x.Element(ns + "评估确定楼面地价").Value.ToString())));
                    x.SetElementValue(ns + "评估确定楼面地价", AppraiseReport.MoneyXx(x.Element(ns + "评估确定楼面地价").Value.ToString()));
                    tmperror = "10.土地使用者或座落或评估类型或价值类型或估价目的为空";
                    x.Add(new XElement(ns + "长估价目的", AppraiseReport.ReportPurpose(x.Element(ns + "土地使用者").Value.ToString(), x.Element(ns + "座落").Value.ToString(), x.Element(ns + "评估类型").Value.ToString(), x.Element(ns + "价值类型").Value.ToString(), x.Element(ns + "估价目的").Value.ToString())));
                    tmperror = "11.估价目的或价值类型为空";
                    x.Add(new XElement(ns + "价值定义", "999"));
                    tmperror = "12.项目编号生成出错";
                    x.Add(new XElement(ns + "项目编号", AppraiseReport.ProjectBh(x.Element(ns + "报告名").Value.ToString())));
                    tmperror = "13.初报告编号生成出错";
                    x.Add(new XElement(ns + "初报告编号", AppraiseReport.ReportBh(x.Element(ns + "报告名").Value.ToString())));
                    tmperror = "14.正报告编号出错";
                    x.Add(new XElement(ns + "正报告编号", AppraiseReport.ReportBh(x.Element(ns + "报告名").Value.ToString())));
                    tmperror = "15.技报告编号生成出错";
                    x.Add(new XElement(ns + "技报告编号", AppraiseReport.ReportBh(x.Element(ns + "报告名").Value.ToString())));
                    tmperror = "15.报告开始日期或报告结束日期为空";
                    x.Add(new XElement(ns + "开始日期短", AppraiseReport.SetValue("开始日期(短)", x.Element(ns + "报告开始时间").Value.ToString())));
                    x.Add(new XElement(ns + "开始日期长", AppraiseReport.SetValue("开始日期(长)", x.Element(ns + "报告开始时间").Value.ToString())));
                    x.Add(new XElement(ns + "结束日期长", AppraiseReport.SetValue("结束日期(长)", x.Element(ns + "报告结束时间").Value.ToString())));
                    x.Add(new XElement(ns + "结束日期短", AppraiseReport.SetValue("结束日期(短)", x.Element(ns + "报告结束时间").Value.ToString())));
                    x.Add(new XElement(ns + "开始日期长数字", AppraiseReport.SetValue("开始日期(长数字)", x.Element(ns + "报告开始时间").Value.ToString())));
                    x.Add(new XElement(ns + "结束日期长数字", AppraiseReport.SetValue("结束日期(长数字)", x.Element(ns + "报告结束时间").Value.ToString())));

                    tmperror = "16.估价期日为空";
                    x.Add(new XElement(ns + "估价期日长", AppraiseReport.SetValue("估价期日(长)", x.Element(ns + "估价基准日").Value.ToString())));
                    x.Add(new XElement(ns + "估价期日长数字", AppraiseReport.SetValue("估价期日(长数字)", x.Element(ns + "估价基准日").Value.ToString())));
                    tmperror = "17.有效年限长";
                    x.Add(new XElement(ns + "有效年期长", AppraiseReport.SetValue("有效年期(长)", x.Element(ns + "报告有效期限").Value.ToString())));
                    //==================

                    //==================
                    tmperror = "19. 价格精度";
                    x.SetElementValue(ns + "价格精度大写", x.Element(ns + "总价精度").Value.ToString());

                    tmperror = "19.镇区或评估类型为空";
                    System.Collections.ArrayList arraylist;
                    arraylist = AppraiseReport.Zqms(x.Element(ns + "镇区").Value.ToString(), x.Element(ns + "评估类型").Value.ToString());
                    //==================
                    x.Add(new XElement(ns + "yw_zq", from att in arraylist.ToArray()
                                                     select new XElement(ns + "描述", att.ToString())

                   ));
                    x.Add(new XElement(ns + "yw_zq1", from att in arraylist.ToArray()
                                                      select new XElement(ns + "描述1", att.ToString())

                    ));
                    x.Add(new XElement(ns + "yw_zq2", from att in arraylist.ToArray()
                                                      select new XElement(ns + "描述2", att.ToString())

                    ));
                    tmperror = "20.报告到期时间长为空";
                    x.Add(new XElement(ns + "报告到期时间长", AppraiseReport.SetValue("报告到期时间(长)", x.Element(ns + "报告到期时间").Value.ToString())));
                    x.Add(new XElement(ns + "报告到期时间长数字", AppraiseReport.SetValue("报告到期时间(长数字)", x.Element(ns + "报告到期时间").Value.ToString())));
                    tmperror = "21.土地终止日期为空";
                    if (HasElementReturnString(x, ns, "土地终止日期") == "")
                    {
                        x.Add(new XElement(ns + "土地终止日期长", "-"));
                        x.SetElementValue(ns + "土地终止日期", "-");
                        x.Add(new XElement(ns + "土地终止日期长数字", "-"));
                    }
                    else
                    {
                        x.Add(new XElement(ns + "土地终止日期长", AppraiseReport.SetValue("土地终止日期(长)", x.Element(ns + "土地终止日期").Value.ToString())));
                        x.SetElementValue(ns + "土地终止日期", AppraiseReport.SetValue("土地终止日期(长数字)", x.Element(ns + "土地终止日期").Value.ToString()));
                        x.Add(new XElement(ns + "土地终止日期长数字", AppraiseReport.SetValue("土地终止日期(长数字)", x.Element(ns + "土地终止日期").Value.ToString())));
                    }
                    x.Add(new XElement(ns + "评估方法描述", ""));
                    x.Add(new XElement(ns + "本估评师", (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PStaffName, "")));
                    tmperror = "18.位置区域和环境为空";
                    // x.SetElementValue(ns + "位置区域和环境", Convert.ToBase64String(Encoding.Default.GetBytes((x.Element(ns + "位置区域和环境").Value.ToString()))));
                    x.SetElementValue(ns + "位置区域和环境", x.Element(ns + "位置区域和环境").Value.ToString());

                }
                #region==============================2009.9.4 收费通知书
                //HasElementReturnString(x, ns, "座落");
                //评估类型,评估总值,评估对象,评估地址,评估日期,评估编号,标准收费,折扣率,折扣后价值大写,折扣后价值小写,开户帐号,开户行,开户名称
                string straddress = "";//评估地址

                tmperror = "业务收费通知书";
                string strpgbh = "";
                string strpglx = "";
                var varywgujiab = from el in xe.Descendants(ns + "yw_gujia_b") select el;
                foreach (XElement x in varywgujiab)
                {
                    if (HasElement(x, ns, "收费标准"))
                    {
                        if (HasElementReturnString(x, ns, "收费标准").IndexOf("房地产") >= 0 || HasElementReturnString(x, ns, "收费标准").IndexOf("土地") >= 0)
                        {
                            if (strpglx != "房地产")
                            {
                                if (HasElementReturnString(x, ns, "收费标准").IndexOf("房地产") >= 0) strpglx = "房地产";
                                if (HasElementReturnString(x, ns, "收费标准").IndexOf("土地") >= 0) strpglx = "土地";
                            }
                            if (double.Parse(HasElementReturnString(x, ns, "标准收费")) > 0)
                            {
                                straddress = straddress + HasElementReturnString(x, ns, "座落") + "/";
                                strpgbh = strpgbh + HasElementReturnString(x, ns, "正报告编号") + "/";
                            }
                        }
                        straddress = straddress.Substring(0, straddress.Length - 1);
                        strpgbh = strpgbh.Substring(0, strpgbh.Length - 1);
                    }
                }
                var varywgujia = from el in xe.Descendants(ns + "yw_gujia") select el;
                foreach (XElement x in varywgujia)
                {
                    if (HasElement(x, ns, "委托方"))
                    {
                        string tmpyh = HasElementReturnString(x, ns, "收费银行");
                        x.SetElementValue(ns + "评估类型", strpglx);
                        x.SetElementValue(ns + "评估地址", straddress);
                        x.SetElementValue(ns + "评估编号", strpgbh);
                        x.SetElementValue(ns + "评估总值", AppraiseClass.S4J5(HasElementReturnDouble(x, ns, "评估总值"), 0).ToString("#,#"));
                        x.SetElementValue(ns + "标准收费", AppraiseClass.S4J5(HasElementReturnDouble(x, ns, "标准收费"), 0).ToString("#,#"));
                        double tmpdouble = AppraiseClass.S4J5(HasElementReturnDouble(x, ns, "折扣") * 10, 1);
                        string strzk = "";
                        if (tmpdouble.ToString().EndsWith("0"))
                        {
                            strzk = int.Parse(tmpdouble.ToString()).ToString() == "10" ? "" : string.Format("，现按{0}折收费如下", int.Parse(tmpdouble.ToString()).ToString());
                        }
                        else
                        {
                            strzk = string.Format("，现按{0}折收费如下", tmpdouble.ToString());
                        }
                        x.SetElementValue(ns + "折扣", strzk);
                        //20100126 收费通知书，实际收费数据继承改为开票收费字段;
                        x.SetElementValue(ns + "实际收费", AppraiseClass.S4J5(HasElementReturnDouble(x, ns, "开票收费"), 0).ToString("#,#"));
                        x.SetElementValue(ns + "实际收费大写", AppraiseReport.MoneyDx(AppraiseClass.S4J5(HasElementReturnDouble(x, ns, "开票收费"), 0).ToString("#,#")));
                        x.SetElementValue(ns + "评估日期", string.Format("{0}年{1}月{2}日", DateTime.Parse(HasElementReturnString(x, ns, "窗口收件日期")).Year, DateTime.Parse(HasElementReturnString(x, ns, "窗口收件日期")).Month, DateTime.Parse(HasElementReturnString(x, ns, "窗口收件日期")).Day));
                        x.SetElementValue(ns + "收费通知书日期", string.Format("{0}年{1}月{2}日", DateTime.Parse(DateTime.Now.ToString()).Year, DateTime.Parse(DateTime.Now.ToString()).Month, DateTime.Parse(DateTime.Now.ToString()).Day));
                        string tmpsql = "select 开户名称 ,帐号 ,开户行 from [收费帐号] where 开户行='{0}'";
                        tmpsql = string.Format(tmpsql, tmpyh);
                        DataTable tmpTable = SkyMap.Net.DAO.QueryHelper.ExecuteSql("Default", string.Empty, tmpsql);

                        x.SetElementValue(ns + "开户帐号", tmpTable.Rows[0]["帐号"].ToString());
                        x.SetElementValue(ns + "开户行", tmpTable.Rows[0]["开户行"].ToString());
                        x.SetElementValue(ns + "开户名称", tmpTable.Rows[0]["开户名称"].ToString());
                    }
                }
                #endregion==============================2009.9.4 收费通知书
            }
            catch (Exception ex)
            {
                if (tmperror == "业务收费通知书")
                {
                   //MessageHelper.ShowInfo("出错,请与系统维护人员联系!");     
                    LoggingService.Debug("财务环节的话，业务收费通知书出错;");
                }
                else
                {
                    MessageHelper.ShowInfo(tmperror);
                    
                }
            }
            xe.Save(strFilePath);
            //XElement xe = XElement.Load("test.xml");
            //var ss = from el in xe.Elements("yw_gujia").Elements("yw_gujia_b") select el;
            //foreach (XElement x in ss)
            //{
            //    MessageBox.Show(x.ToString());
            //}
            //===============================
            //strXmlData = "";
            //foreach (DataRow dr in dtsYW_tdzbpm.Tables["WF_PROINSTMATER"].Rows)
            //{
            //    strXmlData += dr[0].ToString();
            //}
            //strXmlData = strXmlData.Replace("dbo.", "");
            //strXmlData = strXmlData.Replace("xmlns=\"\"", "");
            //XmlDocument docWF_PROINSTMATER = new XmlDocument();
            //strXmlData = "<Root xmlns=\"http://www.zgfdc.net/gujia\">" + strXmlData + "</Root>";
            //docWF_PROINSTMATER.LoadXml(strXmlData);
            //foreach (XmlNode nodeB in docWF_PROINSTMATER.FirstChild.ChildNodes)
            //{
            //    node = doc.ImportNode(nodeB, true);
            //    doc.FirstChild.FirstChild.AppendChild(node);
            //}

            return true;
        }
        #endregion

        #region 检查正式报告建档是否能通过
        private bool CreateData(string strFilePath)
        {
            string strProjectId;
            string strPROINST_ID;
            DataSet dtsYW_tdzbpm;
            string strXmlData = "";

            strProjectId = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProjectId, "");
            strPROINST_ID = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProinstId, "");

            IDA0 dao = DAOFactory.GetInstanceByNameSpace("SkyMap.Net.DAO");
            IDbConnection dbConn = dao.Connection;
            IDbCommand dbCmd;
            try
            {
                dbCmd = dbConn.CreateCommand();
                dbCmd.CommandType = CommandType.StoredProcedure;
                dbCmd.CommandText = "ProcGujiaxml";

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

            strXmlData = "<Root xmlns=\"http://www.zgfdc.net/gujia\">" + strXmlData + "</Root>";
            doc.LoadXml(strXmlData);

            //strXmlData = "";
            //foreach (DataRow dr in dtsYW_tdzbpm.Tables["YW_tdzbpm_td"].Rows)
            //{
            //    strXmlData += dr[0].ToString();
            //}

            //strXmlData = strXmlData.Replace("dbo.", "");
            //strXmlData = strXmlData.Replace("xmlns=\"\"", "");
            //XmlDocument docYW_tdzbpm_td = new XmlDocument();
            //strXmlData = "<Root xmlns=\"http://www.zgfdc.net/gujia\">" + strXmlData + "</Root>";
            //docYW_tdzbpm_td.LoadXml(strXmlData);

            XmlNode node;
            //foreach (XmlNode nodeTD in docYW_tdzbpm_td.FirstChild.ChildNodes)
            //{
            //    node = doc.ImportNode(nodeTD, true);
            //    doc.FirstChild.FirstChild.AppendChild(node);
            //}

            strXmlData = "";

            dbCmd = dbConn.CreateCommand();
            dbCmd.CommandType = CommandType.StoredProcedure;
            dbCmd.CommandText = "ProcGujiaBMxxml";
            dbCmd.Parameters.Add(new SqlParameter("@bid", SqlDbType.Int));
            dbCmd.Parameters.Add(new SqlParameter("@Pglx", SqlDbType.NVarChar, 50));
            ((SqlParameter)dbCmd.Parameters["@Pglx"]).Value = DBNull.Value;

            foreach (DataRow dr in m_dstAll.Tables["yw_gujia_b"].Rows)
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
                        LoggingService.Debug("出错报表" + e.Message);
                        MessageHelper.ShowError("出错", e);
                    }
                }
            }

            strXmlData = strXmlData.Replace("dbo.", "");
            strXmlData = strXmlData.Replace("xmlns=\"\"", "");
            XmlDocument docyw_gujia_b = new XmlDocument();
            strXmlData = "<Root xmlns=\"http://www.zgfdc.net/gujia\">" + strXmlData + "</Root>";
            docyw_gujia_b.LoadXml(strXmlData);
            foreach (XmlNode nodeB in docyw_gujia_b.FirstChild.ChildNodes)
            {
                node = doc.ImportNode(nodeB, true);
                doc.FirstChild.FirstChild.AppendChild(node);
            }

            //===============================对xml进行处理；
            string tmperror = "";
            string tmpstring = doc.InnerXml.ToString();
            string wtf = "";
            XElement xe = XElement.Parse(tmpstring);
            XNamespace ns = "http://www.zgfdc.net/gujia";
            var ss = from el in xe.Descendants(ns + "yw_gujia") select el;
            foreach (XElement x in ss)
            {
                wtf = x.Element(ns + "委托方").Value.ToString();
            }
            string[] tmpzj = { "sss", "yyy", "wwww" };
            string[] tmpzq = { };
            var yy = from el in xe.Descendants(ns + "yw_gujia_b") select el;
            try
            {
                foreach (XElement x in yy)
                {
                    string stryxzj = "";
                    string strtdz = "";
                    string strfcz = "";
                    string strfcgyz = "";
                    System.Collections.ArrayList arraylistzj;
                    System.Collections.ArrayList arraylistfj;
                    tmperror = "有效证件不能为空";
                    stryxzj = x.Element(ns + "有效证件").Value != null ? x.Element(ns + "有效证件").Value.ToString() : "";//有效证件
                    if (stryxzj.IndexOf("土地") >= 0)
                    {
                        tmperror = "有效证件包含： " + stryxzj + ",土地证号不能为空";
                        strtdz = x.Element(ns + "土地证号").Value != null ? x.Element(ns + "土地证号").Value.ToString() : "";//土地证
                    }
                    if (stryxzj.IndexOf("房地产权证") >= 0 || stryxzj.IndexOf("广东省房地产权证") >= 0)
                    {
                        tmperror = "有效证件包含： " + stryxzj + ",房地产权证不能为空";
                        strfcz = x.Element(ns + "房地产权证").Value != null ? x.Element(ns + "房地产权证").Value.ToString() : "";//房产证
                    }
                    if (stryxzj.IndexOf("房地产权共有（用）证") >= 0)
                    {
                        tmperror = "有效证件包含： " + stryxzj + ",房地产共有用证不能为空";
                        strfcgyz = x.Element(ns + "房地产共有用证").Value != null ? x.Element(ns + "房地产共有用证").Value.ToString() : "";//房产共有证
                    }
                    tmperror = "1.正在准备附件描述";
                    arraylistfj = AppraiseReport.Fjms(stryxzj, strtdz, strfcz, strfcgyz);
                    x.Add(new XElement(ns + "yw_fj", from att in arraylistfj.ToArray()
                                                     select new XElement(ns + "附件", att.ToString())
                    ));
                    tmperror = "1.正在准备证件描述";
                    arraylistzj = AppraiseReport.Zjms(stryxzj, strtdz, strfcz, strfcgyz);
                    x.Add(new XElement(ns + "yw_zj", from att in arraylistzj.ToArray()
                                                     select new XElement(ns + "证件", att.ToString())
                    ));
                    x.Add(new XElement(ns + "yw_zj1", from att in arraylistzj.ToArray()
                                                      select new XElement(ns + "证件1", att.ToString())
                   ));
                    x.Add(new XElement(ns + "yw_zj2", from att in arraylistzj.ToArray()
                                                      select new XElement(ns + "证件2", att.ToString())
                   ));
                    tmperror = "2.委托方添加";
                    x.Add(new XElement(ns + "委托方", wtf));
                    tmperror = "3.评估确定房地产单价为空";
                    x.Add(new XElement(ns + "评估确定房地产单价大写", AppraiseReport.MoneyDx(x.Element(ns + "评估确定房地产单价").Value.ToString())));
                    x.SetElementValue(ns + "评估确定房地产单价", AppraiseReport.MoneyXx(x.Element(ns + "评估确定房地产单价").Value.ToString()));
                    tmperror = "4.评估确定房地产总价为空";
                    x.Add(new XElement(ns + "评估确定房地产总价大写", AppraiseReport.MoneyDx(x.Element(ns + "评估确定房地产总价").Value.ToString())));
                    x.SetElementValue(ns + "评估确定房地产总价", AppraiseReport.MoneyXx(x.Element(ns + "评估确定房地产总价").Value.ToString()));
                    tmperror = "5.评估确定土地总价为空";
                    x.Add(new XElement(ns + "评估确定土地总价大写", AppraiseReport.MoneyDx(x.Element(ns + "评估确定土地总价").Value.ToString())));
                    x.SetElementValue(ns + "评估确定土地总价", AppraiseReport.MoneyXx(x.Element(ns + "评估确定土地总价").Value.ToString()));
                    tmperror = "6.评估确定土地单价为空";
                    x.Add(new XElement(ns + "评估确定土地单价大写", AppraiseReport.MoneyDx(x.Element(ns + "评估确定土地单价").Value.ToString())));
                    x.SetElementValue(ns + "评估确定土地单价", AppraiseReport.MoneyXx(x.Element(ns + "评估确定土地单价").Value.ToString()));
                    tmperror = "7.评估确定房产价值为空";
                    x.Add(new XElement(ns + "评估确定房产价值大写", AppraiseReport.MoneyDx(x.Element(ns + "评估确定房产价值").Value.ToString())));
                    x.SetElementValue(ns + "评估确定房产价值", AppraiseReport.MoneyXx(x.Element(ns + "评估确定房产价值").Value.ToString()));
                    tmperror = "8.评估确定房产单价为空";
                    x.Add(new XElement(ns + "评估确定房产单价大写", AppraiseReport.MoneyDx(x.Element(ns + "评估确定房产单价").Value.ToString())));
                    x.SetElementValue(ns + "评估确定房产单价", AppraiseReport.MoneyXx(x.Element(ns + "评估确定房产单价").Value.ToString()));
                    tmperror = "9.评估确定楼面地价为空";
                    x.Add(new XElement(ns + "评估确定楼面地价大写", AppraiseReport.MoneyDx(x.Element(ns + "评估确定楼面地价").Value.ToString())));
                    x.SetElementValue(ns + "评估确定楼面地价", AppraiseReport.MoneyXx(x.Element(ns + "评估确定楼面地价").Value.ToString()));
                    tmperror = "10.土地使用者或座落或评估类型或价值类型或估价目的为空";
                    x.Add(new XElement(ns + "长估价目的", AppraiseReport.ReportPurpose(x.Element(ns + "土地使用者").Value.ToString(), x.Element(ns + "座落").Value.ToString(), x.Element(ns + "评估类型").Value.ToString(), x.Element(ns + "价值类型").Value.ToString(), x.Element(ns + "估价目的").Value.ToString())));
                    tmperror = "11.估价目的或价值类型为空";
                    x.Add(new XElement(ns + "价值定义", "999"));
                    tmperror = "12.项目编号生成出错";
                    x.Add(new XElement(ns + "项目编号", AppraiseReport.ProjectBh(x.Element(ns + "报告名").Value.ToString())));
                    tmperror = "13.初报告编号生成出错";
                    x.Add(new XElement(ns + "初报告编号", AppraiseReport.ReportBh(x.Element(ns + "报告名").Value.ToString())));
                    tmperror = "14.正报告编号出错";
                    x.Add(new XElement(ns + "正报告编号", AppraiseReport.ReportBh(x.Element(ns + "报告名").Value.ToString())));
                    tmperror = "15.技报告编号生成出错";
                    x.Add(new XElement(ns + "技报告编号", AppraiseReport.ReportBh(x.Element(ns + "报告名").Value.ToString())));
                    tmperror = "15.报告开始日期或报告结束日期为空";
                    x.Add(new XElement(ns + "开始日期短", AppraiseReport.SetValue("开始日期(短)", x.Element(ns + "报告开始时间").Value.ToString())));
                    x.Add(new XElement(ns + "开始日期长", AppraiseReport.SetValue("开始日期(长)", x.Element(ns + "报告开始时间").Value.ToString())));
                    x.Add(new XElement(ns + "结束日期长", AppraiseReport.SetValue("结束日期(长)", x.Element(ns + "报告结束时间").Value.ToString())));
                    x.Add(new XElement(ns + "结束日期短", AppraiseReport.SetValue("结束日期(短)", x.Element(ns + "报告结束时间").Value.ToString())));
                    x.Add(new XElement(ns + "开始日期长数字", AppraiseReport.SetValue("开始日期(长数字)", x.Element(ns + "报告开始时间").Value.ToString())));
                    x.Add(new XElement(ns + "结束日期长数字", AppraiseReport.SetValue("结束日期(长数字)", x.Element(ns + "报告结束时间").Value.ToString())));

                    tmperror = "16.估价期日为空";
                    x.Add(new XElement(ns + "估价期日长", AppraiseReport.SetValue("估价期日(长)", x.Element(ns + "估价基准日").Value.ToString())));
                    x.Add(new XElement(ns + "估价期日长数字", AppraiseReport.SetValue("估价期日(长数字)", x.Element(ns + "估价基准日").Value.ToString())));
                    tmperror = "17.有效年限长";
                    x.Add(new XElement(ns + "有效年期长", AppraiseReport.SetValue("有效年期(长)", x.Element(ns + "报告有效期限").Value.ToString())));
                    //==================

                    //==================
                    tmperror = "19. 价格精度";
                    x.SetElementValue(ns + "价格精度大写", x.Element(ns + "总价精度").Value.ToString());

                    tmperror = "19.镇区或评估类型为空";
                    System.Collections.ArrayList arraylist;
                    arraylist = AppraiseReport.Zqms(x.Element(ns + "镇区").Value.ToString(), x.Element(ns + "评估类型").Value.ToString());
                    //==================
                    x.Add(new XElement(ns + "yw_zq", from att in arraylist.ToArray()
                                                     select new XElement(ns + "描述", att.ToString())

                   ));
                    x.Add(new XElement(ns + "yw_zq1", from att in arraylist.ToArray()
                                                      select new XElement(ns + "描述1", att.ToString())

                    ));
                    x.Add(new XElement(ns + "yw_zq2", from att in arraylist.ToArray()
                                                      select new XElement(ns + "描述2", att.ToString())

                    ));
                    tmperror = "20.报告到期时间长为空";
                    x.Add(new XElement(ns + "报告到期时间长", AppraiseReport.SetValue("报告到期时间(长)", x.Element(ns + "报告到期时间").Value.ToString())));
                    x.Add(new XElement(ns + "报告到期时间长数字", AppraiseReport.SetValue("报告到期时间(长数字)", x.Element(ns + "报告到期时间").Value.ToString())));
                    tmperror = "21.土地终止日期为空";
                    if (HasElementReturnString(x, ns, "土地终止日期") == "")
                    {
                        x.Add(new XElement(ns + "土地终止日期长", "-"));
                        x.SetElementValue(ns + "土地终止日期", "-");
                        x.Add(new XElement(ns + "土地终止日期长数字", "-"));
                    }
                    else
                    {
                        x.Add(new XElement(ns + "土地终止日期长", AppraiseReport.SetValue("土地终止日期(长)", x.Element(ns + "土地终止日期").Value.ToString())));
                        x.SetElementValue(ns + "土地终止日期", AppraiseReport.SetValue("土地终止日期(长数字)", x.Element(ns + "土地终止日期").Value.ToString()));
                        x.Add(new XElement(ns + "土地终止日期长数字", AppraiseReport.SetValue("土地终止日期(长数字)", x.Element(ns + "土地终止日期").Value.ToString())));
                    }
                    x.Add(new XElement(ns + "评估方法描述", ""));
                    x.Add(new XElement(ns + "本估评师", (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PStaffName, "")));
                    tmperror = "18.位置区域和环境为空";
                    // x.SetElementValue(ns + "位置区域和环境", Convert.ToBase64String(Encoding.Default.GetBytes((x.Element(ns + "位置区域和环境").Value.ToString()))));
                    x.SetElementValue(ns + "位置区域和环境", x.Element(ns + "位置区域和环境").Value.ToString());

                }
                //#region==============================2009.9.4 收费通知书
                ////HasElementReturnString(x, ns, "座落");
                ////评估类型,评估总值,评估对象,评估地址,评估日期,评估编号,标准收费,折扣率,折扣后价值大写,折扣后价值小写,开户帐号,开户行,开户名称
                //string straddress = "";//评估地址

                //tmperror = "业务收费通知书";
                //string strpgbh = "";
                //string strpglx = "";
                //var varywgujiab = from el in xe.Descendants(ns + "yw_gujia_b") select el;
                //foreach (XElement x in varywgujiab)
                //{
                //    if (HasElement(x, ns, "收费标准"))
                //    {
                //        if (HasElementReturnString(x, ns, "收费标准").IndexOf("房地产") >= 0 || HasElementReturnString(x, ns, "收费标准").IndexOf("土地") >= 0)
                //        {
                //            if (strpglx != "房地产")
                //            {
                //                if (HasElementReturnString(x, ns, "收费标准").IndexOf("房地产") >= 0) strpglx = "房地产";
                //                if (HasElementReturnString(x, ns, "收费标准").IndexOf("土地") >= 0) strpglx = "土地";
                //            }
                //            if (double.Parse(HasElementReturnString(x, ns, "标准收费")) > 0)
                //            {
                //                straddress = straddress + HasElementReturnString(x, ns, "座落") + "/";
                //                strpgbh = strpgbh + HasElementReturnString(x, ns, "正报告编号") + "/";
                //            }
                //        }
                //        straddress = straddress.Substring(0, straddress.Length - 1);
                //        strpgbh = strpgbh.Substring(0, strpgbh.Length - 1);
                //    }
                //}
                //var varywgujia = from el in xe.Descendants(ns + "yw_gujia") select el;
                //foreach (XElement x in varywgujia)
                //{
                //    if (HasElement(x, ns, "委托方"))
                //    {
                //        string tmpyh = HasElementReturnString(x, ns, "收费银行");
                //        x.SetElementValue(ns + "评估类型", strpglx);
                //        x.SetElementValue(ns + "评估地址", straddress);
                //        x.SetElementValue(ns + "评估编号", strpgbh);
                //        x.SetElementValue(ns + "评估总值", AppraiseClass.S4J5(HasElementReturnDouble(x, ns, "评估总值"), 0).ToString("#,#"));
                //        x.SetElementValue(ns + "标准收费", AppraiseClass.S4J5(HasElementReturnDouble(x, ns, "标准收费"), 0).ToString("#,#"));
                //        double tmpdouble = AppraiseClass.S4J5(HasElementReturnDouble(x, ns, "折扣") * 10, 1);
                //        string strzk = "";
                //        if (tmpdouble.ToString().EndsWith("0"))
                //        {
                //            strzk = int.Parse(tmpdouble.ToString()).ToString() == "10" ? "" : string.Format("，现按{0}折收费如下", int.Parse(tmpdouble.ToString()).ToString());
                //        }
                //        else
                //        {
                //            strzk = string.Format("，现按{0}折收费如下", tmpdouble.ToString());
                //        }
                //        x.SetElementValue(ns + "折扣", strzk);
                //        //20100126 收费通知书，实际收费数据继承改为开票收费字段;
                //        x.SetElementValue(ns + "实际收费", AppraiseClass.S4J5(HasElementReturnDouble(x, ns, "开票收费"), 0).ToString("#,#"));
                //        x.SetElementValue(ns + "实际收费大写", AppraiseReport.MoneyDx(AppraiseClass.S4J5(HasElementReturnDouble(x, ns, "开票收费"), 0).ToString("#,#")));
                //        x.SetElementValue(ns + "评估日期", string.Format("{0}年{1}月{2}日", DateTime.Parse(HasElementReturnString(x, ns, "报告开始时间")).Year, DateTime.Parse(HasElementReturnString(x, ns, "报告开始时间")).Month, DateTime.Parse(HasElementReturnString(x, ns, "报告开始时间")).Day));
                //        x.SetElementValue(ns + "收费通知书日期", string.Format("{0}年{1}月{2}日", DateTime.Parse(DateTime.Now.ToString()).Year, DateTime.Parse(DateTime.Now.ToString()).Month, DateTime.Parse(DateTime.Now.ToString()).Day));
                //        string tmpsql = "select 开户名称 ,帐号 ,开户行 from [收费帐号] where 开户行='{0}'";
                //        tmpsql = string.Format(tmpsql, tmpyh);
                //        DataTable tmpTable = SkyMap.Net.DAO.QueryHelper.ExecuteSql("Default", string.Empty, tmpsql);

                //        x.SetElementValue(ns + "开户帐号", tmpTable.Rows[0]["帐号"].ToString());
                //        x.SetElementValue(ns + "开户行", tmpTable.Rows[0]["开户行"].ToString());
                //        x.SetElementValue(ns + "开户名称", tmpTable.Rows[0]["开户名称"].ToString());
                //    }
                //}
                //#endregion==============================2009.9.4 收费通知书
            }
            catch (Exception ex)
            {
                MessageHelper.ShowInfo(tmperror);
                return false;
            }
            //xe.Save(strFilePath);
            //XElement xe = XElement.Load("test.xml");
            //var ss = from el in xe.Elements("yw_gujia").Elements("yw_gujia_b") select el;
            //foreach (XElement x in ss)
            //{
            //    MessageBox.Show(x.ToString());
            //}
            //===============================
            //strXmlData = "";
            //foreach (DataRow dr in dtsYW_tdzbpm.Tables["WF_PROINSTMATER"].Rows)
            //{
            //    strXmlData += dr[0].ToString();
            //}
            //strXmlData = strXmlData.Replace("dbo.", "");
            //strXmlData = strXmlData.Replace("xmlns=\"\"", "");
            //XmlDocument docWF_PROINSTMATER = new XmlDocument();
            //strXmlData = "<Root xmlns=\"http://www.zgfdc.net/gujia\">" + strXmlData + "</Root>";
            //docWF_PROINSTMATER.LoadXml(strXmlData);
            //foreach (XmlNode nodeB in docWF_PROINSTMATER.FirstChild.ChildNodes)
            //{
            //    node = doc.ImportNode(nodeB, true);
            //    doc.FirstChild.FirstChild.AppendChild(node);
            //}

            return true;
        }
        #endregion
        protected override bool PrintBySelf(TempletPrint templetPrint)
        {
            AwokeUpdateReport();
            if (templetPrint.Type.EndsWith("xslt"))
            {
                string strProjectId = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProjectId, "");
                string inputfile = string.Format("{0}.xml", Path.GetTempFileName());
                try
                {
                    if (templetPrint.Description.IndexOf("收费") >= 0)
                    {
                        CreateMxb(inputfile);
                    }
                    else if (templetPrint.Description.IndexOf("检查数据") >= 0)
                    {
                       return CreateData(inputfile); //正式报告 建档的时候判断数据是否补全了，防止财务收费通知书的时候退回;
                    }
                    else
                    {
                        CreateReportDataFile(inputfile, templetPrint.Name, templetPrint.Description);
                    }

                    File.Copy(inputfile, Application.StartupPath + "\\data.xml", true);
                }
                catch (Exception ex)
                {
                    MessageHelper.ShowError("生成报表数据有误！", ex);
                }
                DataTable dtbReport = SkyMap.Net.DAO.QueryHelper.ExecuteSql("Default", string.Empty, @"SELECT ReportID, PROJECT_ID, TEMPLETPRINT_ID, ReportName, ReportData
                                        FROM Yw_Gujia_Report where PROJECT_ID = '" + strProjectId + "' and TEMPLETPRINT_ID = '" + templetPrint.Id + "'");

                dtbReport.ExtendedProperties.Add("selectsql", @"SELECT ReportID, PROJECT_ID, TEMPLETPRINT_ID, ReportName, ReportData
                                        FROM Yw_Gujia_Report where PROJECT_ID = '" + strProjectId + "' and TEMPLETPRINT_ID = '" + templetPrint.Id + "'");

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

                if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + Path.DirectorySeparatorChar + m_strYwNo + "评估报告"))
                {
                    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + Path.DirectorySeparatorChar + m_strYwNo + "评估报告");
                }
                object outputfile = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + Path.DirectorySeparatorChar + m_strYwNo + "评估报告" + Path.DirectorySeparatorChar + m_strYwNo + "-" + templetPrint.Name;
                outputfile = string.Format("{0}.xml", outputfile);
                try
                {
                    xslt.Transform(inputfile, outputfile as string);
                    ////======================start 特殊处理方式
                    //string sok;
                    //string strstart = "<ns0:位置区域和环境>";
                    //string strend = "</ns0:位置区域和环境>";
                    //StringBuilder ss = new StringBuilder();
                    //string s1;
                    //using (StreamReader sr = new StreamReader(outputfile as string, System.Text.Encoding.GetEncoding("utf-8")))
                    //{
                    //    string s = "";
                    //    while ((s = sr.ReadLine()) != null)
                    //    {
                    //        ss.Append(s);
                    //    }
                    //    s1 = ss.ToString();

                    //    sok = AppraiseReport.TF(s1, strstart, strend);

                    //}
                    //using (StreamWriter sw = new StreamWriter(outputfile as string))
                    //{
                    //    sw.Write(sok);
                    //}
                    ////=====================end  特殊处理方式

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
                    if (MessageHelper.ShowYesNoInfo("是否将报告上传到服务器？") == DialogResult.Yes)
                    {
                        try
                        {
                            //-------------------------
                            SaveToWebGujia(outputfile.ToString(), base.GetControlBindValue(this.txt委托方).ToString(), this.bglx, base.GetControlBindValue(this.txt_ProjectId).ToString().Substring(base.GetControlBindValue(this.txt_ProjectId).ToString().IndexOf("-") + 1).Replace("-", ""), templetPrint.Description.ToString());
                            //-------------------------
                            //System.Diagnostics.Process process1 = new Process();
                            //process1.StartInfo.CreateNoWindow = true;
                            //process1.StartInfo.UseShellExecute = false;
                            //process1.StartInfo.FileName = Application.StartupPath + "\\WML2XSLT";
                            //process1.StartInfo.Arguments = "\"" + outputfile + "\" -o \"" + xsltFile + "\" -ns http://www.zgfdc.net/gujia";
                            //process1.Start();
                            //process1.WaitForExit();

                            //byte[] bytefile;
                            //FileStream file = File.Open(xsltFile, FileMode.Open);
                            //bytefile = new byte[(int)file.Length];
                            //file.Read(bytefile, 0, (int)file.Length);
                            //dtbReport.Rows[0].BeginEdit();
                            //dtbReport.Rows[0]["ReportData"] = bytefile;
                            //dtbReport.Rows[0].EndEdit();
                            //SkyMap.Net.DataForms.DataEngine.SQLDataEngine sqlDataEngine = new SkyMap.Net.DataForms.DataEngine.SQLDataEngine();
                            //sqlDataEngine.SaveData(this.dataFormController.DAODataForm.DataSource, dtbReport);
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

        private DataSet GetAllData()
        {
            string strProjectId = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProjectId, "");
            m_dstAll = SkyMap.Net.DAO.QueryHelper.ExecuteSqls("Default", string.Empty, new string[]{@"SELECT * 
FROM YW_gujia_b where PROJECT_ID ='"+strProjectId+"' order by Bid asc","SELECT * FROM YW_tdzbpm_td where PROJECT_ID ='"+strProjectId+"'","SELECT * FROM YW_tdzbpm_djksq where PROJECT_ID ='"+strProjectId+"'","SELECT * FROM YW_tdzbpm_gpbj where PROJECT_ID ='"+strProjectId+"'","SELECT * FROM YW_tdzbpm_phdj where PROJECT_ID ='"+strProjectId+"'","SELECT * FROM YW_tdzbpm_xcbj where PROJECT_ID ='"+strProjectId+"'"}, new string[] { "YW_gujia_b", "YW_tdzbpm_td", "YW_tdzbpm_djksq", "YW_tdzbpm_gpbj", "YW_tdzbpm_phdj", "YW_tdzbpm_xcbj" });
            if (m_dstAll != null && m_dstAll.Tables.Count != 0)
            {
                m_dstAll.Tables["YW_gujia_b"].ExtendedProperties.Add("selectsql", @"SELECT  * FROM YW_gujia_b where PROJECT_ID ='" + strProjectId + "' order by bid asc");

                m_dstAll.Tables["YW_tdzbpm_td"].ExtendedProperties.Add("selectsql", @"SELECT  *  FROM YW_tdzbpm_td where PROJECT_ID ='" + strProjectId + "'");
                m_dstAll.Tables["YW_tdzbpm_djksq"].ExtendedProperties.Add("selectsql", @"SELECT * FROM YW_tdzbpm_djksq where PROJECT_ID ='" + strProjectId + "' order by  djksqid ");
                m_dstAll.Tables["YW_tdzbpm_gpbj"].ExtendedProperties.Add("selectsql", @"SELECT * FROM YW_tdzbpm_gpbj where PROJECT_ID ='" + strProjectId + "'");
                m_dstAll.Tables["YW_tdzbpm_phdj"].ExtendedProperties.Add("selectsql", @"SELECT * FROM YW_tdzbpm_phdj where PROJECT_ID ='" + strProjectId + "'");
                m_dstAll.Tables["YW_tdzbpm_xcbj"].ExtendedProperties.Add("selectsql", @"SELECT * FROM YW_tdzbpm_xcbj where PROJECT_ID ='" + strProjectId + "'");
                //地块
                DataRelation dtr = new DataRelation("Btd", m_dstAll.Tables["YW_gujia_b"].Columns["Bid"], m_dstAll.Tables["YW_tdzbpm_td"].Columns["标"], false);
                m_dstAll.Relations.Add(dtr);
                //地价款
                dtr = new DataRelation("Bdjksq", m_dstAll.Tables["YW_gujia_b"].Columns["Bid"], m_dstAll.Tables["YW_tdzbpm_djksq"].Columns["B_id"], false);
                m_dstAll.Relations.Add(dtr);

                dtr = new DataRelation("Bxcbj", m_dstAll.Tables["YW_gujia_b"].Columns["Bid"], m_dstAll.Tables["YW_tdzbpm_xcbj"].Columns["B_id"], false);
                m_dstAll.Relations.Add(dtr);

                m_dstAll.Tables["YW_gujia_b"].TableNewRow += new DataTableNewRowEventHandler(ZBPMDataForms_BNewRow);
                m_dstAll.Tables["YW_gujia_b"].RowDeleted += new DataRowChangeEventHandler(ZBPMDataForms_RowDeleted);
                // m_dstAll.Tables["YW_gujia_b"].RowDeleting += new DataRowChangeEventHandler(ZBPMDataForms_RowDeleting);
            }
            return m_dstAll;
        }

        void ZBPMDataForms_BNewRow(object sender, DataTableNewRowEventArgs e)
        {
            if ((txt_ProjectId.EditValue != null && txt_ProjectId.EditValue.ToString() != ""))
            {
                int intCount = 1;
                if (base.GetControlBindValue(this.cbx_换证).ToString() == "是")
                {
                    foreach (DataRow dr in e.Row.Table.Rows)
                    {
                        try
                        {
                            if (dr.RowState != DataRowState.Deleted && (Convert.ToInt32(dr["报告号"].ToString())) >= 65)
                            {
                                intCount++;
                            }
                        }
                        catch
                        {

                            if (dr.RowState != DataRowState.Deleted && (Convert.ToInt32(dr["报告号"].ToString())) >= 65)
                            {
                                intCount++;
                            }
                        }
                    }
                }
                else
                {
                    foreach (DataRow dr in e.Row.Table.Rows)
                    {
                        if (dr.RowState != DataRowState.Deleted && (Convert.ToInt32(dr["报告号"].ToString())) < 65)
                        {
                            intCount++;
                        }
                    }
                }
                //e.Row["标名"] = "第" + convertchinese(intCount.ToString()) + "标";                
                if (intCount <= 1)
                {
                    e.Row["报告号"] = base.GetControlBindValue(this.cbx_换证).ToString() == "是" ? (intCount + 64).ToString() : intCount.ToString();
                    e.Row["报告名"] = txt_ProjectId.EditValue.ToString();
                }
                else
                {
                    int i = 0;
                    if (base.GetControlBindValue(this.cbx_换证).ToString() == "是")
                    {
                        foreach (DataRow dr in e.Row.Table.Rows)
                        {
                            if (dr.RowState != DataRowState.Deleted && (Convert.ToInt32(dr["报告号"].ToString())) >= 65)
                            {
                                dr["报告号"] = (i + 65).ToString();
                                dr["报告名"] = txt_ProjectId.EditValue.ToString() + "-" + Convert.ToChar((i + 65)).ToString();
                                i++;
                            }
                        }
                        e.Row["报告号"] = (i + 65).ToString();
                        e.Row["报告名"] = txt_ProjectId.EditValue.ToString() + "-" + Convert.ToChar((i + 65)).ToString();
                    }
                    else
                    {
                        foreach (DataRow dr in e.Row.Table.Rows)
                        {
                            if (dr.RowState != DataRowState.Deleted && (Convert.ToInt32((dr["报告号"].ToString()))) < 65)
                            {
                                dr["报告号"] = (i + 1).ToString();
                                dr["报告名"] = txt_ProjectId.EditValue.ToString() + "-" + (i + 1).ToString();
                                i++;
                            }
                        }
                        e.Row["报告号"] = (i + 1).ToString();
                        e.Row["报告名"] = txt_ProjectId.EditValue.ToString() + "-" + (i + 1).ToString();

                    }
                }
                //e.Row["容积率标准值标"] = "1.5";
                e.Row["PROJECT_ID"] = (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PProjectId, "");
                e.Row["报告有效期限"] = 1;
                e.Row["总价精度"] = "个";
            }
            else
            {
                MessageHelper.ShowInfo("");
            }

        }

        void ZBPMDataForms_RowDeleted(object sender, DataRowChangeEventArgs e)
        {
            if (base.GetControlBindValue(this.cbx_换证).ToString() != "是")
            {
                #region 没换证

                if ((txt_ProjectId.EditValue != null && txt_ProjectId.EditValue.ToString() != ""))
                {
                    int intCount = 0;
                    foreach (DataRow dr in e.Row.Table.Rows)
                    {
                        if (dr.RowState != DataRowState.Deleted && int.Parse(dr["报告号"].ToString()) < 65)
                        {
                            intCount++;
                        }
                    }
                    if (intCount == 1)
                    {
                        foreach (DataRow dr in e.Row.Table.Rows)
                        {
                            if (dr.RowState != DataRowState.Deleted && int.Parse(dr["报告号"].ToString()) < 65)
                            {
                                dr["报告号"] = intCount.ToString();
                                dr["报告名"] = txt_ProjectId.EditValue.ToString();
                            }
                        }
                    }
                    else
                    {
                        int i = 0;
                        foreach (DataRow dr in e.Row.Table.Rows)
                        {
                            if (dr.RowState != DataRowState.Deleted && int.Parse(dr["报告号"].ToString()) < 65)
                            {
                                dr["报告号"] = (i + 1).ToString();
                                dr["报告名"] = txt_ProjectId.EditValue.ToString() + "-" + (i + 1).ToString();
                                i++;
                            }
                        }
                    }
                }
                #endregion
            }
            else
            {
                #region 换证
                if ((txt_ProjectId.EditValue != null && txt_ProjectId.EditValue.ToString() != ""))
                {
                    int intCount = 0;
                    foreach (DataRow dr in e.Row.Table.Rows)
                    {
                        if (dr.RowState != DataRowState.Deleted && int.Parse(dr["报告号"].ToString()) >= 65)
                        {
                            intCount++;
                        }
                    }
                    if (intCount == 1)
                    {
                        foreach (DataRow dr in e.Row.Table.Rows)
                        {
                            if (dr.RowState != DataRowState.Deleted && int.Parse(dr["报告号"].ToString()) >= 65)
                            {
                                dr["报告号"] = (intCount + 64).ToString();
                                dr["报告名"] = txt_ProjectId.EditValue.ToString();
                            }
                        }
                    }
                    else
                    {
                        int i = 0;
                        foreach (DataRow dr in e.Row.Table.Rows)
                        {
                            if (dr.RowState != DataRowState.Deleted && int.Parse(dr["报告号"].ToString()) >= 65)
                            {
                                dr["报告号"] = (i + 65).ToString();
                                dr["报告名"] = txt_ProjectId.EditValue.ToString() + "-" + Convert.ToChar(i + 65).ToString();
                                i++;
                            }
                        }
                    }
                }
                #endregion
            }
        }

        void ZBPMDataForms_RowDeleting(object sender, DataRowChangeEventArgs e)
        {
            if (base.GetControlBindValue(this.cbx_换证).ToString() != "是")
            {
                #region 没换证
                #endregion
            }
            else
            {
                #region 换证

                #endregion
            }
        }

        private void BBindData()
        {
            gridB.DataSource = m_dstAll;
            gridB.DataMember = "yw_gujia_b";
            gridB1.DataSource = m_dstAll;
            gridB1.DataMember = "yw_gujia_b";
            gridB2.DataSource = m_dstAll;
            gridB2.DataMember = "yw_gujia_b";
        }

        private void SetControlState()
        {
            ////if (!cbx_换证.Properties.ReadOnly)
            ////{
            ////    cbx_换证.Text = "是";
            ////    cbx_换证.Enabled = false;
            ////}
            ////else
            ////{

            ////}
            //try
            //{
            //    if (base.GetControlBindValue(this.cbx_换证).ToString() == "是")
            //    {
            //        panelControl_换证.Visible = true;
            //        panelControl_不换证.Visible = false;
            //    }
            //    else
            //    {
            //        panelControl_换证.Visible = false;
            //        panelControl_不换证.Visible = true;

            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message.ToString());
            //}
            panelControl_换证.Visible = false;
            panelControl_不换证.Visible = true;
        }

        private void SaveAllData()
        {
            SMDataSource smDs = this.dataFormController.DAODataForm.DataSource;
            SkyMap.Net.DataForms.DataEngine.SQLDataEngine sqlDataEngine = new SkyMap.Net.DataForms.DataEngine.SQLDataEngine();
            foreach (DataRow dr in m_dstAll.Tables["YW_gujia_b"].Rows)
            {
                #region 如果换证，不允许修改以前的证载信息
                if (dr.RowState == DataRowState.Modified)
                {
                    #region 保护证载信息
                    if (base.GetControlBindValue(this.cbx_换证).ToString() == "是")
                    {
                        if (Int32.Parse(dr["报告号", DataRowVersion.Original].ToString()) < 65)
                        {
                            dr.RejectChanges();
                            MessageHelper.ShowInfo("请不要修改以前的证载信息!");
                        }
                    }
                    else
                    {
                        if (Int32.Parse(dr["报告号", DataRowVersion.Original].ToString()) >= 65)
                        {
                            dr.RejectChanges();
                            MessageHelper.ShowInfo("请不要修改换证后的证载信息!");
                        }
                    }
                    #endregion
                }
                #endregion
                if (dr.RowState == DataRowState.Deleted)
                {
                    //如果换证，则不能删除以前输入的证载信息；
                    #region 保护证载信息
                    if (base.GetControlBindValue(this.cbx_换证).ToString() == "是")
                    {
                        if (Int32.Parse(dr["报告号", DataRowVersion.Original].ToString()) < 65)
                        {
                            dr.RejectChanges();
                            MessageHelper.ShowInfo("请不要删除以前的证载信息!");
                        }
                    }
                    else
                    {
                        if (Int32.Parse(dr["报告号", DataRowVersion.Original].ToString()) >= 65)
                        {
                            dr.RejectChanges();
                            MessageHelper.ShowInfo("请不要删除换证后的证载信息!");
                        }
                    }
                    #endregion
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
            sqlDataEngine.SaveData(smDs, m_dstAll.Tables["YW_gujia_b"]);
            sqlDataEngine.SaveData(smDs, m_dstAll.Tables["YW_tdzbpm_td"]);
            sqlDataEngine.SaveData(smDs, m_dstAll.Tables["YW_tdzbpm_djksq"]);
            sqlDataEngine.SaveData(smDs, m_dstAll.Tables["YW_tdzbpm_gpbj"]);
            sqlDataEngine.SaveData(smDs, m_dstAll.Tables["YW_tdzbpm_phdj"]);
            sqlDataEngine.SaveData(smDs, m_dstAll.Tables["YW_tdzbpm_xcbj"]);
            sqlDataEngine.RefreshDataset(smDs, m_dstAll);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.cardViewB.ClearColumnsFilter();
            DevExpress.XtraGrid.Views.Base.ColumnView view = this.cardViewB;
            DevExpress.XtraGrid.Views.Base.ViewColumnFilterInfo viewFilterInfo = new ViewColumnFilterInfo(view.Columns["报告号"],
              new DevExpress.XtraGrid.Columns.ColumnFilterInfo("[报告号] >= 65", ""));
            view.ActiveFilter.Add(viewFilterInfo);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.cardViewB.ClearColumnsFilter();
            DevExpress.XtraGrid.Views.Base.ColumnView view = this.cardViewB as DevExpress.XtraGrid.Views.Base.ColumnView;
            DevExpress.XtraGrid.Views.Base.ViewColumnFilterInfo viewFilterInfo = new ViewColumnFilterInfo(view.Columns["报告号"],
              new DevExpress.XtraGrid.Columns.ColumnFilterInfo("[报告号] < 65", ""));
            view.ActiveFilter.Add(viewFilterInfo);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.cardViewB.ClearColumnsFilter();
            DevExpress.XtraGrid.Views.Base.ColumnView view = this.cardViewB;
            DevExpress.XtraGrid.Views.Base.ViewColumnFilterInfo viewFilterInfo = new ViewColumnFilterInfo(view.Columns["报告号"],
              new DevExpress.XtraGrid.Columns.ColumnFilterInfo("[报告号] >= 1", ""));
            view.ActiveFilter.Add(viewFilterInfo);

        }

        private void cbx_换证_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbx_换证.Text.ToString() == "是")
            {
                simpleButton1_Click(null, null);
            }
            else
            {
                simpleButton2_Click(null, null);
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            this.repositoryItemLookUpEdit3 = DataWordLookUpEditHelper.Create("PURPOSE", "Name", "Name");
        }

        private void QM_窗口收件人_Click(object sender, EventArgs e)
        {
            Qm(this.txt_窗口收件人, this.dt_窗口收件日期);
        }

        private void xtb_评估报告_Paint(object sender, PaintEventArgs e)
        {

        }

        private void qm_估价人员_Click(object sender, EventArgs e)
        {
            Qm(this.txt_估价人员, this.dt_估价日期);
        }

        private void qm_一审_Click(object sender, EventArgs e)
        {
            Qm(this.txt_一审, this.dt_一审日期);
        }

        private void qm_二审_Click(object sender, EventArgs e)
        {
            Qm(this.txt_二审, this.dt_二审日期);
        }

        private void qm_估价人员1_Click(object sender, EventArgs e)
        {
            Qm(this.txt_估价人员1, this.dt_估价日期1);
        }

        private void qm_一审1_Click(object sender, EventArgs e)
        {
            Qm(this.txt_一审1, this.dt_一审日期1);
        }

        private void qm_二审1_Click(object sender, EventArgs e)
        {
            Qm(this.txt_二审, this.dt_二审日期);
        }

        private void tblData_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            //========================
            if (m_dstAll != null)
            {

                //-------------------------------------------
                if (this.tblData.SelectedTabPage.Text.ToString() == "审核报告")
                {
                    //this._windowManager.New();
                    //get the registrations page URL
                    string strdz = "";
                    XElement xe = XElement.Load(Application.StartupPath + "\\webgujia.xml");
                    XNamespace ns = "http://www.zgfdc.net/gujia";
                    var xx1 = from el in xe.Descendants(ns + "webgujia") select el;

                    foreach (XElement x in xx1)
                    {
                        if (HasElement(x, ns, "address"))
                        {
                            strdz = x.Element(ns + "address").Value;
                        }
                    }

                    string url = strdz + "/weboffice/default.aspx?ReportId=" + base.GetControlBindValue(this.txt_ProjectId).ToString().Substring(base.GetControlBindValue(this.txt_ProjectId).ToString().IndexOf("-") + 1).Replace("-", "");

                    Object o = null;

                    //fetch the page to your web browser.
                    this._windowManager.ActiveBrowser.Navigate(url);
                    if (iweburl == 0)
                    {
                        iweburl++;
                    }
                    else
                    {
                        this._windowManager.ActiveBrowser.Refresh();
                    }
                    this._windowManager.ActiveBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(LoginWeb);

                }
            }
            SetControlState();
        }

        public override bool PreProcessMessage(ref Message msg)
        {
            return base.PreProcessMessage(ref msg);
        }

        private void qm_初评_Click(object sender, EventArgs e)
        {
            base.Save();
            this.bglx = "doc";
            DataFormPrint("房地产初评");

        }

        private void qm_UpdateReport_Click(object sender, EventArgs e)
        {
            UpdateReport();
        }


        #region 评估房屋面积选定，评估土地面积选定
        private void repositoryItemComboBox35_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString() == "" || ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString() == string.Empty) return;
            try
            {
                DataRow drB = ((DevExpress.XtraGrid.Views.Card.CardView)gridB1.FocusedView).GetDataRow(((DevExpress.XtraGrid.Views.Card.CardView)gridB1.FocusedView).FocusedRowHandle);
                if (((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString() == "建筑面积")
                {
                    try
                    {
                        // MessageHelper.ShowInfo(drB["建筑面积"].ToString());
                        if (double.Parse(drB["建筑面积"].ToString()) >= 0)
                        {
                            drB.BeginEdit();
                            drB["评估最后采用建筑面积"] = drB["建筑面积"];
                            drB.EndEdit();
                        }
                        else
                        {
                            MessageHelper.ShowInfo(String.Format("{0}输入有误或为空！", ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString()));
                            ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue = "";
                            drB.BeginEdit();
                            drB["评估最后采用建筑面积"] = 0;
                            drB.EndEdit();
                        }
                    }
                    catch
                    {
                        MessageHelper.ShowInfo(String.Format("{0}输入有误或为空！", ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString()));
                        ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue = "";
                        drB.BeginEdit();
                        drB["评估最后采用建筑面积"] = 0;
                        drB.EndEdit();
                    }
                }
                else if (((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString() == "套内建筑面积")
                {
                    try
                    {
                        // MessageHelper.ShowInfo(drB["套内建筑面积"].ToString());
                        if (double.Parse(drB["套内建筑面积"].ToString()) >= 0)
                        {
                            drB.BeginEdit();
                            drB["评估最后采用建筑面积"] = drB["套内建筑面积"];
                            drB.EndEdit();
                        }
                        else
                        {
                            MessageHelper.ShowInfo(String.Format("{0}输入有误或为空！", ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString()));
                            ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue = "";
                            drB.BeginEdit();
                            drB["评估最后采用建筑面积"] = 0;
                            drB.EndEdit();
                        }
                    }
                    catch
                    {
                        MessageHelper.ShowInfo(String.Format("{0}输入有误或为空！", ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString()));
                        ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue = "";
                        drB.BeginEdit();
                        drB["评估最后采用建筑面积"] = 0;
                        drB.EndEdit();
                    }
                }
                else if (((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString() == "新测实评房屋面积")
                {
                    try
                    {
                        //MessageHelper.ShowInfo(drB["新测实评房屋面积"].ToString());
                        if (double.Parse(drB["新测实评房屋面积"].ToString()) >= 0)
                        {
                            drB.BeginEdit();
                            drB["评估最后采用建筑面积"] = drB["新测实评房屋面积"];
                            drB.EndEdit();
                        }
                        else
                        {
                            MessageHelper.ShowInfo(String.Format("{0}输入有误或为空！", ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString()));
                            ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue = "";
                            drB.BeginEdit();
                            drB["评估最后采用建筑面积"] = 0;
                            drB.EndEdit();
                        }
                    }
                    catch
                    {
                        MessageHelper.ShowInfo(String.Format("{0}输入有误或为空！", ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString()));
                        ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue = "";
                        drB.BeginEdit();
                        drB["评估最后采用建筑面积"] = 0;
                        drB.EndEdit();
                    }
                }
                else
                {

                }
            }
            catch
            {
                MessageHelper.ShowInfo("有错误发生");
            }
        }
        private void repositoryItemComboBox36_EditValueChanged(object sender, EventArgs e)
        {
            if (((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString() == "" || ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString() == string.Empty) return;
            try
            {
                DataRow drB = ((DevExpress.XtraGrid.Views.Card.CardView)gridB1.FocusedView).GetDataRow(((DevExpress.XtraGrid.Views.Card.CardView)gridB1.FocusedView).FocusedRowHandle);
                if (((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString() == "土地使用权面积")
                {
                    try
                    {
                        if (double.Parse(drB["土地使用权面积"].ToString()) >= 0)
                        {
                            drB.BeginEdit();
                            drB["评估最后采用土地面积"] = drB["土地使用权面积"];
                            drB.EndEdit();
                        }
                        else
                        {
                            drB.BeginEdit();
                            drB["评估最后采用土地面积"] = 0;
                            drB.EndEdit();
                            MessageHelper.ShowInfo(String.Format("{0}输入有误或为空！", ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString()));
                            ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue = "";

                        }
                    }
                    catch
                    {
                        drB.BeginEdit();
                        drB["评估最后采用土地面积"] = 0;
                        drB.EndEdit();
                        MessageHelper.ShowInfo(String.Format("{0}输入有误或为空！", ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString()));
                        ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue = "";
                    }
                }
                else if (((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString() == "新测实评土地面积")
                {
                    try
                    {
                        if (double.Parse(drB["新测实评土地面积"].ToString()) >= 0)
                        {
                            drB.BeginEdit();
                            drB["评估最后采用土地面积"] = drB["新测实评土地面积"];
                            drB.EndEdit();
                        }
                        else
                        {
                            MessageHelper.ShowInfo(String.Format("{0}输入有误或为空！", ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString()));
                            drB.BeginEdit();
                            drB["评估最后采用土地面积"] = 0;
                            drB.EndEdit();
                            ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue = "";
                        }
                    }
                    catch
                    {
                        MessageHelper.ShowInfo(String.Format("{0}输入有误或为空！", ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString()));
                        ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue = "";
                        drB.BeginEdit();
                        drB["评估最后采用土地面积"] = 0;
                        drB.EndEdit();
                    }
                }
                else
                {
                    MessageHelper.ShowInfo("...");
                }
            }
            catch
            {
                MessageHelper.ShowInfo("有错误发生");
            }
        }
        #endregion

        #region 面积更改
        //新测实评房屋面积更改
        private void repositoryItemTextEdit2_EditValueChanged(object sender, EventArgs e)
        {
            string tmpstring = ((DevExpress.XtraEditors.TextEdit)sender).Text.ToString();
            if (tmpstring == string.Empty || tmpstring == "") return;
            DataRow drB = ((DevExpress.XtraGrid.Views.Card.CardView)gridB1.FocusedView).GetDataRow(((DevExpress.XtraGrid.Views.Card.CardView)gridB1.FocusedView).FocusedRowHandle);

            try
            {
                if (double.Parse(tmpstring) >= 0)
                {
                    try
                    {
                        if (drB["评估房屋面积"].ToString() == "新测实评房屋面积")
                        {
                            drB.BeginEdit();
                            drB["评估最后采用建筑面积"] = tmpstring;
                            drB.EndEdit();
                        }

                    }
                    catch
                    {

                    }
                }
                else
                {
                    MessageHelper.ShowInfo("面积输入有误");
                    ((DevExpress.XtraEditors.TextEdit)sender).Text = "";
                }
            }
            catch
            {
                MessageHelper.ShowInfo("面积输入有误");
                ((DevExpress.XtraEditors.TextEdit)sender).Text = "";
            }
        }
        private void repositoryItemTextEdit3_EditValueChanged(object sender, EventArgs e)
        {
            string tmpstring = ((DevExpress.XtraEditors.TextEdit)sender).Text.ToString();
            if (tmpstring == string.Empty || tmpstring == "") return;
            DataRow drB = ((DevExpress.XtraGrid.Views.Card.CardView)gridB.FocusedView).GetDataRow(((DevExpress.XtraGrid.Views.Card.CardView)gridB.FocusedView).FocusedRowHandle);

            try
            {
                if (double.Parse(tmpstring) >= 0)
                {
                    try
                    {
                        if (drB["评估房屋面积"].ToString() == "建筑面积")
                        {
                            drB.BeginEdit();
                            drB["评估最后采用建筑面积"] = tmpstring;
                            drB.EndEdit();
                        }

                    }
                    catch
                    {

                    }
                }
                else
                {
                    MessageHelper.ShowInfo("面积输入有误");
                    ((DevExpress.XtraEditors.TextEdit)sender).Text = "";
                }
            }
            catch
            {
                MessageHelper.ShowInfo("面积输入有误");
                ((DevExpress.XtraEditors.TextEdit)sender).Text = "";
            }
        }
        private void repositoryItemTextEdit4_EditValueChanged(object sender, EventArgs e)
        {
            string tmpstring = ((DevExpress.XtraEditors.TextEdit)sender).Text.ToString();
            if (tmpstring == string.Empty || tmpstring == "") return;
            DataRow drB = ((DevExpress.XtraGrid.Views.Card.CardView)gridB.FocusedView).GetDataRow(((DevExpress.XtraGrid.Views.Card.CardView)gridB.FocusedView).FocusedRowHandle);

            try
            {
                if (double.Parse(tmpstring) >= 0)
                {
                    try
                    {
                        if (drB["评估房屋面积"].ToString() == "套内建筑面积")
                        {
                            drB.BeginEdit();
                            drB["评估最后采用建筑面积"] = tmpstring;
                            drB.EndEdit();
                        }

                    }
                    catch
                    {

                    }
                }
                else
                {
                    MessageHelper.ShowInfo("面积输入有误");
                    ((DevExpress.XtraEditors.TextEdit)sender).Text = "";
                }
            }
            catch
            {
                MessageHelper.ShowInfo("面积输入有误");
                ((DevExpress.XtraEditors.TextEdit)sender).Text = "";
            }
        }


        //土地面积更改
        private void repositoryItemTextEdit14_EditValueChanged(object sender, EventArgs e)
        {
            string tmpstring = ((DevExpress.XtraEditors.TextEdit)sender).Text.ToString();
            if (tmpstring == string.Empty || tmpstring == "") return;
            DataRow drB = ((DevExpress.XtraGrid.Views.Card.CardView)gridB1.FocusedView).GetDataRow(((DevExpress.XtraGrid.Views.Card.CardView)gridB1.FocusedView).FocusedRowHandle);

            try
            {
                if (double.Parse(tmpstring) >= 0)
                {
                    try
                    {
                        if (drB["评估土地面积"].ToString() == "新测实评土地面积")
                        {
                            drB.BeginEdit();
                            drB["评估最后采用土地面积"] = tmpstring;
                            drB.EndEdit();
                        }

                    }
                    catch
                    {

                    }
                }
                else
                {
                    MessageHelper.ShowInfo("面积输入有误");
                    ((DevExpress.XtraEditors.TextEdit)sender).Text = "";
                }
            }
            catch
            {
                MessageHelper.ShowInfo("面积输入有误");
                ((DevExpress.XtraEditors.TextEdit)sender).Text = "";
            }
        }
        private void repositoryItemTextEdit15_EditValueChanged(object sender, EventArgs e)
        {
            string tmpstring = ((DevExpress.XtraEditors.TextEdit)sender).Text.ToString();
            if (tmpstring == string.Empty || tmpstring == "") return;
            DataRow drB = ((DevExpress.XtraGrid.Views.Card.CardView)gridB.FocusedView).GetDataRow(((DevExpress.XtraGrid.Views.Card.CardView)gridB.FocusedView).FocusedRowHandle);

            try
            {
                if (double.Parse(tmpstring) >= 0)
                {
                    try
                    {
                        if (drB["评估土地面积"].ToString() == "土地使用权面积")
                        {
                            drB.BeginEdit();
                            drB["评估最后采用土地面积"] = tmpstring;
                            drB.EndEdit();
                        }

                    }
                    catch
                    {

                    }
                }
                else
                {
                    MessageHelper.ShowInfo("面积输入有误");
                    ((DevExpress.XtraEditors.TextEdit)sender).Text = "";
                }
            }
            catch
            {
                MessageHelper.ShowInfo("面积输入有误");
                ((DevExpress.XtraEditors.TextEdit)sender).Text = "";
            }

        }
        #endregion

        private void repositoryItemDateEdit3_EditValueChanged(object sender, EventArgs e)
        {

        }

        #region 自动计算评估结果
        private void repositoryItemTextEdit17_EditValueChanged(object sender, EventArgs e)
        {
            if (((DevExpress.XtraEditors.TextEdit)sender).Text.ToString() == "" || ((DevExpress.XtraEditors.TextEdit)sender).Text.ToString() == String.Empty) return;
            DataRow drB = ((DevExpress.XtraGrid.Views.Card.CardView)gridB1.FocusedView).GetDataRow(((DevExpress.XtraGrid.Views.Card.CardView)gridB1.FocusedView).FocusedRowHandle);
            try
            {
                if (double.Parse(((DevExpress.XtraEditors.TextEdit)sender).Text.ToString()) >= 0)
                {
                    try
                    {
                        // MessageHelper.ShowInfo(drB["建筑面积"].ToString());
                        if (double.Parse(drB["评估最后采用土地面积"].ToString()) >= 0)
                        {
                            int intjd = 0;
                            drB.BeginEdit();
                            if (drB["总价精度"].ToString() == "个")
                            {
                                intjd = 0;
                            }
                            else if (drB["总价精度"].ToString() == "十" || drB["总价精度"].ToString() == "拾")
                            {
                                intjd = 1;
                            }
                            else if (drB["总价精度"].ToString() == "百" || drB["总价精度"].ToString() == "佰")
                            {
                                intjd = 2;
                            }
                            else if (drB["总价精度"].ToString() == "千" || drB["总价精度"].ToString() == "仟")
                            {
                                intjd = 3;
                            }
                            else if (drB["总价精度"].ToString() == "万")
                            {
                                intjd = 4;
                            }

                            if (drB["评估类型"].ToString() == "土地")
                            {
                                drB["评估确定土地总价"] = AppraiseClass.QuZhen(AppraiseClass.S4J5(((DevExpress.XtraEditors.TextEdit)sender).Text.ToString()), intjd);
                                drB["评估确定土地单价"] = AppraiseClass.S4J5(double.Parse(((DevExpress.XtraEditors.TextEdit)sender).Text.ToString()) / double.Parse(drB["评估最后采用土地面积"].ToString()), 0);
                                drB["评估确定房地产总价"] = 0;
                                drB["评估确定房地产单价"] = 0;
                                drB["评估确定房产价值"] = 0;
                                drB["评估确定房产单价"] = 0;
                                drB["评估确定楼面地价"] = 0;
                            }
                            else
                            {
                                drB["评估确定土地总价"] = AppraiseClass.QuZhen(AppraiseClass.S4J5(((DevExpress.XtraEditors.TextEdit)sender).Text.ToString()), intjd);
                                drB["评估确定土地单价"] = AppraiseClass.S4J5(double.Parse(((DevExpress.XtraEditors.TextEdit)sender).Text.ToString()) / double.Parse(drB["评估最后采用土地面积"].ToString()), 0);
                                drB["评估确定房地产单价"] = AppraiseClass.S4J5(double.Parse(drB["计算房地产总价"].ToString()) / double.Parse(drB["评估最后采用建筑面积"].ToString()), 0);
                                drB["评估确定房产价值"] = AppraiseClass.QuZhen(AppraiseClass.S4J5(double.Parse(drB["计算房地产总价"].ToString()) - double.Parse(((DevExpress.XtraEditors.TextEdit)sender).Text.ToString()), 0), intjd);
                                drB["评估确定房产单价"] = AppraiseClass.S4J5(((double.Parse(drB["计算房地产总价"].ToString()) - double.Parse(((DevExpress.XtraEditors.TextEdit)sender).Text.ToString()))) / double.Parse(drB["评估最后采用建筑面积"].ToString()), 0);
                                drB["评估确定楼面地价"] = AppraiseClass.S4J5(double.Parse(((DevExpress.XtraEditors.TextEdit)sender).Text.ToString()) / double.Parse(drB["评估最后采用建筑面积"].ToString()), 0);
                                //drB["评估确定房地产总价"] = AppraiseClass.QuZhen(AppraiseClass.S4J5(drB["计算房地产总价"].ToString()), intjd);
                                drB["评估确定房地产总价"] = AppraiseClass.QuZhen(AppraiseClass.S4J5(((DevExpress.XtraEditors.TextEdit)sender).Text.ToString()), intjd) + AppraiseClass.QuZhen(AppraiseClass.S4J5(double.Parse(drB["计算房地产总价"].ToString()) - double.Parse(((DevExpress.XtraEditors.TextEdit)sender).Text.ToString()), 0), intjd);
                            }
                            drB.EndEdit();
                        }
                        else
                        {
                            MessageHelper.ShowInfo(String.Format("{0}输入有误或为空！", ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString()));
                            ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue = "";
                            drB.BeginEdit();
                            //if (drB["评估类型"].ToString() == "土地")
                            //{
                            //    drB["评估确定土地总价"] = 0;
                            //    drB["评估确定土地单价"] = 0;
                            //}
                            //else
                            //{
                            drB["评估确定土地总价"] = 0;
                            drB["评估确定土地单价"] = 0;
                            drB["评估确定房地产总价"] = 0;
                            drB["评估确定房地产单价"] = 0;
                            drB["评估确定房产价值"] = 0;
                            drB["评估确定房产单价"] = 0;
                            drB["评估确定楼面地价"] = 0;
                            //}
                            drB.EndEdit();
                        }
                    }
                    catch
                    {

                        drB.BeginEdit();
                        //if (drB["评估类型"].ToString() == "土地")
                        //{
                        //    drB["评估确定土地总价"] = 0;
                        //    drB["评估确定土地单价"] = 0;
                        //}
                        //else
                        //{
                        drB["评估确定土地总价"] = 0;
                        drB["评估确定土地单价"] = 0;
                        drB["评估确定房地产总价"] = 0;
                        drB["评估确定房地产单价"] = 0;
                        drB["评估确定房产价值"] = 0;
                        drB["评估确定房产单价"] = 0;
                        drB["评估确定楼面地价"] = 0;
                        //}
                        drB.EndEdit();
                        //MessageHelper.ShowInfo("请检查数据");
                        //  ((DevExpress.XtraEditors.TextEdit)sender).Text = "";
                    }
                }
            }
            catch
            {
                //  MessageHelper.ShowInfo("请检查数据");
                // ((DevExpress.XtraEditors.TextEdit)sender).Text = "";
                drB.BeginEdit();
                //if (drB["评估类型"].ToString() == "土地")
                //{
                //    drB["评估确定土地总价"] = 0;
                //    drB["评估确定土地单价"] = 0;
                //}
                //else
                //{
                drB["评估确定土地总价"] = 0;
                drB["评估确定土地单价"] = 0;
                drB["评估确定房地产总价"] = 0;
                drB["评估确定房地产单价"] = 0;
                drB["评估确定房产价值"] = 0;
                drB["评估确定房产单价"] = 0;
                drB["评估确定楼面地价"] = 0;
                //}
                drB.EndEdit();
            }
        }

        #endregion

        private void repositoryItemComboBox32_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRow drB = ((DevExpress.XtraGrid.Views.Card.CardView)gridB1.FocusedView).GetDataRow(((DevExpress.XtraGrid.Views.Card.CardView)gridB1.FocusedView).FocusedRowHandle);
            try
            {
                if (double.Parse(drB["计算土地总价"].ToString()) >= 0)
                {
                    try
                    {
                        // MessageHelper.ShowInfo(drB["建筑面积"].ToString());
                        if (double.Parse(drB["评估最后采用土地面积"].ToString()) >= 0)
                        {
                            int intjd = 0;
                            drB.BeginEdit();
                            if (((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString() == "个")
                            {
                                intjd = 0;
                            }
                            else if (((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString() == "十" || ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString() == "拾")
                            {
                                intjd = 1;
                            }
                            else if (((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString() == "百" || ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString() == "佰")
                            {
                                intjd = 2;
                            }
                            else if (((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString() == "千" || ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString() == "仟")
                            {
                                intjd = 3;
                            }
                            else if (((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString() == "万")
                            {
                                intjd = 4;
                            }

                            if (drB["评估类型"].ToString() == "土地")
                            {
                                drB["评估确定土地总价"] = AppraiseClass.QuZhen(AppraiseClass.S4J5(drB["计算土地总价"].ToString()), intjd);
                                drB["评估确定土地单价"] = AppraiseClass.S4J5(double.Parse(drB["计算土地总价"].ToString()) / double.Parse(drB["评估最后采用土地面积"].ToString()), 0);
                                drB["评估确定房地产总价"] = 0;
                                drB["评估确定房地产单价"] = 0;
                                drB["评估确定房产价值"] = 0;
                                drB["评估确定房产单价"] = 0;
                                drB["评估确定楼面地价"] = 0;
                            }
                            else
                            {
                                drB["评估确定土地总价"] = AppraiseClass.QuZhen(AppraiseClass.S4J5(drB["计算土地总价"].ToString()), intjd);
                                drB["评估确定土地单价"] = AppraiseClass.S4J5(double.Parse(drB["计算土地总价"].ToString()) / double.Parse(drB["评估最后采用土地面积"].ToString()), 0);
                               // drB["评估确定房地产总价"] = AppraiseClass.QuZhen(AppraiseClass.S4J5(drB["计算房地产总价"].ToString()), intjd);
                                drB["评估确定房地产单价"] = AppraiseClass.S4J5(double.Parse(drB["计算房地产总价"].ToString()) / double.Parse(drB["评估最后采用建筑面积"].ToString()), 0);
                                drB["评估确定房产价值"] = AppraiseClass.QuZhen(AppraiseClass.S4J5(double.Parse(drB["计算房地产总价"].ToString()) - double.Parse(drB["计算土地总价"].ToString()), 0), intjd);
                                drB["评估确定房产单价"] = AppraiseClass.S4J5(((double.Parse(drB["计算房地产总价"].ToString()) - double.Parse(drB["计算土地总价"].ToString()))) / double.Parse(drB["评估最后采用建筑面积"].ToString()), 0);
                                drB["评估确定楼面地价"] = AppraiseClass.S4J5(double.Parse(drB["计算土地总价"].ToString()) / double.Parse(drB["评估最后采用建筑面积"].ToString()), 0);
                                drB["评估确定房地产总价"] = AppraiseClass.QuZhen(AppraiseClass.S4J5(drB["计算土地总价"].ToString()), intjd) + AppraiseClass.QuZhen(AppraiseClass.S4J5(double.Parse(drB["计算房地产总价"].ToString()) - double.Parse(drB["计算土地总价"].ToString()), 0), intjd);
                            }
                            drB.EndEdit();
                        }
                        else
                        {
                            //  MessageHelper.ShowInfo("请检查数据");
                            drB.BeginEdit();
                            drB["评估确定土地总价"] = 0;
                            drB["评估确定土地单价"] = 0;
                            drB["评估确定房地产总价"] = 0;
                            drB["评估确定房地产单价"] = 0;
                            drB["评估确定房产价值"] = 0;
                            drB["评估确定房产单价"] = 0;
                            drB["评估确定楼面地价"] = 0;
                            drB.EndEdit();
                        }
                    }
                    catch
                    {

                        drB.BeginEdit();
                        //if (drB["评估类型"].ToString() == "土地")
                        //{
                        //    drB["评估确定土地总价"] = 0;
                        //    drB["评估确定土地单价"] = 0;
                        //}
                        //else
                        //{
                        drB["评估确定土地总价"] = 0;
                        drB["评估确定土地单价"] = 0;
                        drB["评估确定房地产总价"] = 0;
                        drB["评估确定房地产单价"] = 0;
                        drB["评估确定房产价值"] = 0;
                        drB["评估确定房产单价"] = 0;
                        drB["评估确定楼面地价"] = 0;
                        //}
                        drB.EndEdit();
                        MessageHelper.ShowInfo("请检查数据");
                        //  ((DevExpress.XtraEditors.TextEdit)sender).Text = "";
                    }
                }
            }
            catch
            {
                // MessageHelper.ShowInfo("请检查数据");
                // ((DevExpress.XtraEditors.TextEdit)sender).Text = "";
                drB.BeginEdit();
                //if (drB["评估类型"].ToString() == "土地")
                //{
                //    drB["评估确定土地总价"] = 0;
                //    drB["评估确定土地单价"] = 0;
                //}
                //else
                //{
                drB["评估确定土地总价"] = 0;
                drB["评估确定土地单价"] = 0;
                drB["评估确定房地产总价"] = 0;
                drB["评估确定房地产单价"] = 0;
                drB["评估确定房产价值"] = 0;
                drB["评估确定房产单价"] = 0;
                drB["评估确定楼面地价"] = 0;
                //}
                drB.EndEdit();
            }
        }

        #region 根据评估总值，计算标准收费
        private void repositoryItemComboBox34_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string tmpstring = ((DevExpress.XtraEditors.TextEdit)sender).Text.ToString();
            string tmpstring = ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString();
            string type = "";
            DataRow drB = ((DevExpress.XtraGrid.Views.Grid.GridView)gridB2.FocusedView).GetDataRow(((DevExpress.XtraGrid.Views.Grid.GridView)gridB2.FocusedView).FocusedRowHandle);

            try
            {
                if (tmpstring == string.Empty || tmpstring == "")
                {
                    drB.BeginEdit();
                    drB["标准收费"] = 0;
                    drB.EndEdit();
                }
                else
                {
                    if (tmpstring.IndexOf("房地产") >= 0)
                    {
                        type = "房地产";
                    }
                    else if (tmpstring.IndexOf("土地") >= 0)
                    {
                        type = "土地";
                    }
                    else
                    {
                        MessageHelper.ShowInfo("评估类型无法确定");
                        return;
                    }

                    drB.BeginEdit();
                    if (drB["评估类型"].ToString() == "房地产")
                    {
                        drB["标准收费"] = GetMoneyStand(double.Parse(drB["评估确定房地产总价"].ToString()), type);
                    }
                    else if (drB["评估类型"].ToString() == "土地")
                    {
                        drB["标准收费"] = GetMoneyStand(double.Parse(drB["评估确定土地总价"].ToString()), type);
                    }
                    else
                    {
                        drB["标准收费"] = 0;
                    }
                    drB.EndEdit();
                }
                double dprice = 0, dtotal = 0;

                #region 统计最后的标准收费，此处需特别处理
                System.Collections.ArrayList al = new System.Collections.ArrayList();//包含所有需要统计的数据;
                foreach (DataRow dr in m_dstAll.Tables["YW_gujia_b"].Rows)
                {
                    if (dr["标准收费"] != DBNull.Value)
                    {
                        if (double.Parse(dr["标准收费"].ToString()) != 0)
                        {
                            al.Add(new Sh { Tdzh = dr["土地证号"].ToString(), Bzsf = double.Parse(dr["标准收费"].ToString()), Pglx = dr["评估类型"].ToString(), Tdzj = double.Parse(dr["评估确定土地总价"].ToString()), Fdczj = double.Parse(dr["评估确定房地产总价"].ToString()) });
                        }
                    }
                }
                var vtj = from Sh aa in al group aa by aa.Tdzh into g where g.Count() > 1 select new { g.Key, NumProducts = g.Count() };
                List<double> tmpal = new List<double>(); //土地证号包含的土地总价
                List<string> tmptdz = new List<string>();//----包含多了房产的土地的证号;
                bool btdzj = true; //判断土地证号一样的情况下，土地总价是否一样; 一样为true,不同为false;

                #region
                //=============遍历土地证号是一样的评估项目 start
                foreach (var s in vtj)
                {
                    // string aa = string.Format("{0}-{1}", s.Key, s.NumProducts);
                    double dfcjz = 0;
                    double dtdjz = 0;
                    var v = from Sh aa in al where aa.Tdzh == s.Key select new { tdzj = aa.Tdzj,fdczj=aa.Fdczj,pglx= aa.Pglx };
                    foreach (var t in v)
                    {
                        tmpal.Add(double.Parse(t.tdzj.ToString()));
                        dtdjz = t.tdzj;
                        dfcjz = dfcjz + t.fdczj - t.tdzj;
                        if (!tmptdz.Contains(s.Key))
                        {
                            tmptdz.Add(s.Key);
                        }
                        
                    }
                    double tmptdzj = tmpal[0];
                    for (int i = 1; i < tmpal.Count;i++ )
                    {
                        if (tmptdzj != tmpal[i])
                        {
                            btdzj = false;
                            break;
                        }
                    }
                    if (!btdzj)
                    {
                        dprice = 0; dtotal = 0;
                        txt标准价格.Text = dprice.ToString("#,#");
                        txt评估总值.Text = dtotal.ToString("#,#");
                        MessageHelper.ShowInfo(String.Format("请检查数据是否正确!土地证号为【{0}】的评估项目土地总价输入不适合业务规则!",s.Key));
                        return;
                    }
                    //=============
                    if (dfcjz > 0 && dtdjz > 0)
                    {
                        dprice = dprice + GetMoneyStand(dfcjz+dtdjz,"房地产");
                        MessageHelper.ShowInfo(string.Format("【{0}】包含{1}宗房产，评估最后确定土地总价为{2}，房地产总价为{3}，评估标准收费为{4}。", s.Key, s.NumProducts, dtdjz, dtdjz + dfcjz, GetMoneyStand(dfcjz + dtdjz, "房地产").ToString("#,#")));
                        dtotal = dtotal + dtdjz + dfcjz;
                    }
                }
                //=============遍历土地证号是一样的评估项目 End
                #endregion
                //================
                foreach (Sh sh in al)
                {
                    if (!tmptdz.Contains(sh.Tdzh))
                    {
                        dprice = dprice + sh.Bzsf;
                        if (sh.Pglx.ToString()=="房地产")
                        {
                            dtotal = dtotal + sh.Fdczj;
                        }
                        else if (sh.Pglx.ToString() == "土地")
                        {
                            dtotal = dtotal + sh.Tdzj;
                        }
                        else
                        {
                            throw new Exception(string.Format("[{0}]评估类型不明！",sh.Tdzh.ToString()));
                        }
                    }
                }

                //foreach (DataRow dr in m_dstAll.Tables["YW_gujia_b"].Rows)
                //{
                //    if (dr["标准收费"] == DBNull.Value)
                //    {

                //    }
                //    else
                //    {
                //        if (dr["标准收费"].ToString() != string.Empty && dr["标准收费"].ToString() != "" && dr["标准收费"].ToString() != "0")
                //        {
                //            dprice = dprice + double.Parse(dr["标准收费"].ToString());
                //            //-----------------
                //            if (dr["评估类型"].ToString() == "房地产")
                //            {
                //                dtotal = dtotal + double.Parse(dr["评估确定房地产总价"].ToString());
                //            }
                //            else if (dr["评估类型"].ToString() == "土地")
                //            {
                //                dtotal = dtotal + double.Parse(dr["评估确定土地总价"].ToString());
                //            }
                //            else
                //            {
                //                dr["标准收费"] = 0;
                //            }
                //        }
                //    }
                //}
                #endregion

                txt标准价格.Text = dprice.ToString("#,#");
                txt评估总值.Text = dtotal.ToString("#,#");

            }
            catch
            {
                MessageHelper.ShowInfo("请检查数据是否正确");
                ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue = "";
            }
        }
        #endregion
        //评估基准日发生改变
        private void repositoryItemDateEdit5_EditValueChanged(object sender, EventArgs e)
        {
            //if (((DevExpress.XtraEditors.DateEdit)sender).EditValue.ToString() == string.Empty || ((DevExpress.XtraEditors.DateEdit)sender).EditValue.ToString() == null || ((DevExpress.XtraEditors.DateEdit)sender).EditValue.ToString() == string.Empty) return;
            DataRow drB = ((DevExpress.XtraGrid.Views.Card.CardView)gridB1.FocusedView).GetDataRow(((DevExpress.XtraGrid.Views.Card.CardView)gridB1.FocusedView).FocusedRowHandle);
            try
            {
                int flag = 0;
                //  MessageHelper.ShowInfo(drB["估价基准日"].ToString());
                if (drB["土地终止日期"].ToString() == "" || drB["土地终止日期"].ToString() == string.Empty || drB["土地终止日期"].ToString() == null)
                {
                    flag = 1;
                }
                drB.BeginEdit();
                if (flag == 1)
                {
                    drB["土地剩余使用年限"] = 0;
                    drB["报告到期时间"] = AppraiseClass.BiaoGaoYouXiaoQi(DateTime.Parse(((DevExpress.XtraEditors.DateEdit)sender).EditValue.ToString()), 1);
                }
                else
                {
                    double synx = AppraiseClass.ShengYuNianXian(DateTime.Parse(((DevExpress.XtraEditors.DateEdit)sender).EditValue.ToString()), DateTime.Parse(drB["土地终止日期"].ToString()));
                    drB["土地剩余使用年限"] = synx;
                    drB["报告到期时间"] = AppraiseClass.BiaoGaoYouXiaoQi(DateTime.Parse(((DevExpress.XtraEditors.DateEdit)sender).EditValue.ToString()), 1);
                }
                drB.EndEdit();
            }
            catch
            {
                drB.BeginEdit();
                drB["土地剩余使用年限"] = 0;
                drB["报告到期时间"] = System.DBNull.Value;
                drB.EndEdit();
            }
        }

        private void qm_正式报告_Click(object sender, EventArgs e)
        {
            base.Save();
            this.bglx = "doc";
            DataFormPrint("房地产正式报告");
        }

        private void qm_土地初评_Click(object sender, EventArgs e)
        {
            base.Save();
            this.bglx = "doc";
            DataFormPrint("土地初评");
        }

        private void qm_土地正式报告_Click(object sender, EventArgs e)
        {
            base.Save();
            base.Save();
            this.bglx = "doc";
            DataFormPrint("土地正式报告");
        }

        private void qm_评估明细表_Click(object sender, EventArgs e)
        {
            base.Save();
            this.bglx = "xls";
            string inputfile = string.Format("{0}.xml", Path.GetTempFileName());
            try
            {
                CreateMxb(inputfile);

                File.Copy(inputfile, Application.StartupPath + "\\data.xml", true);
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError("生成报表数据有误！", ex);
            }


            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + Path.DirectorySeparatorChar + m_strYwNo + "评估报告"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + Path.DirectorySeparatorChar + m_strYwNo + "评估报告");
            }
            object outputfile = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + Path.DirectorySeparatorChar + m_strYwNo + "评估报告" + Path.DirectorySeparatorChar + m_strYwNo + "-" + "评估明细表";
            outputfile = string.Format("{0}.xls", outputfile);
            try
            {
                string strwtf = "", strrq1 = "", strrq2 = "", strjd = ""; double strzj = 0.0; //委托方，评估基准日，评估结束日期，评估精度，评估房地产总值
                // Create a instance...
                ExcelXmlWorkbook book = new ExcelXmlWorkbook();

                // Many such properties exist. Details can be found in the documentation
                // The author of the document
                book.Properties.Author = "lhm";


                // This returns the first worksheet.
                // Note that we have not declared a instance of a new worksheet
                // All the dirty work is done by the library.
                Worksheet sheet = book[0];

                // Name is the name of the sheet. If not set, the default name
                // style is "sheet" + sheet number, like sheet1, sheet2
                sheet.Name = "评估明细表";

                // More on this in documentation
                sheet.FreezeTopRows = 3;
                sheet.PrintOptions.Orientation = PageOrientation.Landscape;
                // and this too...
                //sheet.PrintOptions.Orientation = PageOrientation.Landscape;
                //sheet.PrintOptions.SetMargins(0.5, 0.4, 0.5, 0.4);

                // This is the actual code which sets out the cell values
                // Note again, that we don't declare any instance at all.
                // All the work is done by the library.
                // Index operator takes first value as column and second as row.
                //----------------------第一行
                new Range(sheet[0, 0], sheet[15, 0]).Merge();
                sheet[0, 0].Value = "评估明细表 ";
                sheet[0, 0].Font.Size = 16;
                sheet[0, 0].Font.Bold = true;
                AlignmentOptionsBase ss = sheet[0, 0].Alignment as AlignmentOptionsBase;
                ss.Vertical = VerticalAlignment.Center;
                ss.Horizontal = Yogesh.ExcelXml.HorizontalAlignment.Center;
                //--------------------第二行
                new Range(sheet[0, 1], sheet[3, 1]).Merge();
                new Range(sheet[1, 1], sheet[4, 1]).Merge();
                sheet[0, 1].Font.Size = 12;
                sheet[1, 1].Font.Size = 12;
                //--------------------第三行
                sheet[0, 2].Value = "序号";
                sheet[1, 2].Value = "地址";
                sheet[2, 2].Value = "产权人";
                sheet[3, 2].Value = "土地证号";
                sheet[4, 2].Value = "房产证号";
                sheet[5, 2].Value = "土地用途";
                sheet[6, 2].Value = "土地面积（㎡）";
                sheet[7, 2].Value = "建筑面积（㎡）";
                sheet[8, 2].Value = "套内面积（㎡）";
                sheet[9, 2].Value = "土地单价（元/㎡）";
                sheet[10, 2].Value = "楼面地价（元/㎡）";
                sheet[11, 2].Value = "房产单价（元/㎡）";
                sheet[12, 2].Value = "土地总值（元）";
                sheet[13, 2].Value = "房产总值（元）";
                sheet[14, 2].Value = "房地产价值（元）";
                sheet[15, 2].Value = "备注";
                //sheet[15, 2].Value = "合计房地产总值";
                //sheet[16, 2].Value = "精确到佰位";
                sheet.Columns(0).Width = 30;
                sheet.Columns(1).Width = 60;
                sheet.Columns(2).Width = 60;
                sheet.Columns(3).Width = 80;
                sheet.Columns(4).Width = 80;
                sheet.Columns(5).Width = 60;
                sheet.Columns(6).Width = 60;
                sheet.Columns(7).Width = 60;
                sheet.Columns(8).Width = 60;
                sheet.Columns(9).Width = 60;
                sheet.Columns(10).Width = 60;
                sheet.Columns(11).Width = 60;
                sheet.Columns(12).Width = 60;
                sheet.Columns(13).Width = 60;
                sheet.Columns(14).Width = 60;
                sheet.Columns(15).Width = 50;
                //------------------------------表格字段的定义
                sheet[0, 2].Border.LineStyle = Borderline.Continuous;
                sheet[1, 2].Border.LineStyle = Borderline.Continuous;
                sheet[2, 2].Border.LineStyle = Borderline.Continuous;
                sheet[3, 2].Border.LineStyle = Borderline.Continuous;
                sheet[4, 2].Border.LineStyle = Borderline.Continuous;
                sheet[5, 2].Border.LineStyle = Borderline.Continuous;
                sheet[6, 2].Border.LineStyle = Borderline.Continuous;
                sheet[7, 2].Border.LineStyle = Borderline.Continuous;
                sheet[8, 2].Border.LineStyle = Borderline.Continuous;
                sheet[9, 2].Border.LineStyle = Borderline.Continuous;
                sheet[10, 2].Border.LineStyle = Borderline.Continuous;
                sheet[11, 2].Border.LineStyle = Borderline.Continuous;
                sheet[12, 2].Border.LineStyle = Borderline.Continuous;
                sheet[13, 2].Border.LineStyle = Borderline.Continuous;
                sheet[14, 2].Border.LineStyle = Borderline.Continuous;
                sheet[15, 2].Border.LineStyle = Borderline.Continuous;
                sheet[0, 2].Border.Sides = BorderSides.All;
                sheet[1, 2].Border.Sides = BorderSides.All;
                sheet[2, 2].Border.Sides = BorderSides.All;
                sheet[3, 2].Border.Sides = BorderSides.All;
                sheet[4, 2].Border.Sides = BorderSides.All;
                sheet[5, 2].Border.Sides = BorderSides.All;
                sheet[6, 2].Border.Sides = BorderSides.All;
                sheet[7, 2].Border.Sides = BorderSides.All;
                sheet[8, 2].Border.Sides = BorderSides.All;
                sheet[9, 2].Border.Sides = BorderSides.All;
                sheet[10, 2].Border.Sides = BorderSides.All;
                sheet[11, 2].Border.Sides = BorderSides.All;
                sheet[12, 2].Border.Sides = BorderSides.All;
                sheet[13, 2].Border.Sides = BorderSides.All;
                sheet[14, 2].Border.Sides = BorderSides.All;
                sheet[15, 2].Border.Sides = BorderSides.All;

                ss = new Range(sheet[0, 2], sheet[15, 2]).Alignment as AlignmentOptionsBase;
                ss.Vertical = VerticalAlignment.Center;
                ss.Horizontal = Yogesh.ExcelXml.HorizontalAlignment.Center;
                sheet[2].Height = 30;
                //-----------------------------------------向表格中插入内容
                XElement xe = XElement.Load(Application.StartupPath + "\\data.xml");
                XNamespace ns = "http://www.zgfdc.net/gujia";
                var xx1 = from el in xe.Descendants(ns + "yw_gujia_b") select el;
                int inum;
                inum = 3;
                foreach (XElement x in xx1)
                {
                    if (HasElement(x, ns, "委托方"))
                    {
                        strwtf = x.Element(ns + "委托方").Value;
                        strrq1 = HasElementReturnString(x, ns, "估价期日长数字");
                        strrq2 = HasElementReturnString(x, ns, "结束日期长数字");
                        strjd = HasElementReturnString(x, ns, "总价精度");
                    }
                    else
                    {
                        strwtf = "";
                    }
                    sheet[0, inum].Value = (inum - 2).ToString();
                    sheet[1, inum].Value = HasElementReturnString(x, ns, "座落");
                    sheet[2, inum].Value = HasElementReturnString(x, ns, "土地使用者");
                    sheet[3, inum].Value = HasElementReturnString(x, ns, "土地证号");
                    sheet[4, inum].Value = HasElementReturnString(x, ns, "房地产权证") + "、" + HasElementReturnString(x, ns, "房地产共有用证");
                    sheet[5, inum].Value = HasElementReturnString(x, ns, "土地用途");
                    sheet[6, inum].Value = HasElementReturnString(x, ns, "土地使用权面积");
                    sheet[7, inum].Value = HasElementReturnString(x, ns, "建筑面积");
                    sheet[8, inum].Value = HasElementReturnString(x, ns, "套内建筑面积");
                    sheet[9, inum].Value = HasElementReturnString(x, ns, "评估确定土地单价");
                    sheet[10, inum].Value = HasElementReturnString(x, ns, "评估确定楼面地价");
                    sheet[11, inum].Value = HasElementReturnString(x, ns, "评估确定房产单价");
                    sheet[12, inum].Value = HasElementReturnString(x, ns, "评估确定土地总价");
                    sheet[13, inum].Value = HasElementReturnString(x, ns, "评估确定房产价值");
                    sheet[14, inum].Value = HasElementReturnString(x, ns, "评估确定房地产总价");
                    strzj = double.Parse(HasElementReturnString(x, ns, "评估确定房地产总价") == "" ? "0" : HasElementReturnString(x, ns, "评估确定房地产总价")) + strzj;

                    #region 设置样式
                    sheet[inum].Height = 60;
                    for (int i = 0; i <= 15; i++)
                    {
                        sheet[i, inum].Border.LineStyle = Borderline.Continuous;
                        sheet[i, inum].Border.Sides = BorderSides.All;
                        sheet[i, inum].Font.Size = 10;
                        sheet[i, inum].Font.Bold = false;
                        AlignmentOptionsBase ao = sheet[i, inum].Alignment as AlignmentOptionsBase;
                        ao.Vertical = VerticalAlignment.Center;
                        ao.Horizontal = Yogesh.ExcelXml.HorizontalAlignment.Center;
                        ao.WrapText = true;
                    }
                    #endregion
                    inum++;
                }

                //----------------------------------------表格末尾的定义
                sheet[inum].Height = 25;
                for (int i = 0; i <= 15; i++)
                {
                    sheet[i, inum].Border.LineStyle = Borderline.Continuous;
                    sheet[i, inum].Border.Sides = BorderSides.All;
                    sheet[i, inum].Font.Size = 10;
                    sheet[i, inum].Font.Bold = false;
                    AlignmentOptionsBase ao = sheet[i, inum].Alignment as AlignmentOptionsBase;
                    ao.Vertical = VerticalAlignment.Center;
                    ao.Horizontal = Yogesh.ExcelXml.HorizontalAlignment.Center;
                    ao.WrapText = true;
                }
                sheet[0, inum].Value = "合计";
                XmlStyle style = new XmlStyle();
                style.Font.Bold = false;
                sheet[14, inum].Value = strzj.ToString("#,#");
                sheet[15, inum].Value = "精确到" + strjd + "位";
                sheet[inum + 1].Height = 10;
                //-----------------------------------------
                // sheet[inum+2].Height = 20;
                for (int i = 0; i <= 15; i++)
                {
                    sheet[i, inum + 2].Font.Size = 12;
                    sheet[i, inum + 2].Font.Bold = false;
                    AlignmentOptionsBase ao = sheet[i, inum + 2].Alignment as AlignmentOptionsBase;
                    ao.Vertical = VerticalAlignment.Center;
                    ao.Horizontal = Yogesh.ExcelXml.HorizontalAlignment.Right;
                    ao.WrapText = true;
                }
                new Range(sheet[7, inum + 2], sheet[15, inum + 2]).Merge();
                sheet[7, inum + 2].Value = "估价方：中山市置信土地房地产估价有限公司";
                //-----------------------------------------
                for (int i = 0; i <= 15; i++)
                {
                    sheet[i, inum + 3].Font.Size = 12;
                    sheet[i, inum + 3].Font.Bold = false;
                    AlignmentOptionsBase ao = sheet[i, inum + 3].Alignment as AlignmentOptionsBase;
                    ao.Vertical = VerticalAlignment.Center;
                    ao.Horizontal = Yogesh.ExcelXml.HorizontalAlignment.Right;
                    ao.WrapText = true;
                }
                new Range(sheet[7, inum + 3], sheet[15, inum + 3]).Merge();
                sheet[7, inum + 3].Value = strrq2;
                sheet[0, 1].Value = "委托方：" + strwtf;
                sheet[1, 1].Value = "评估基准日：" + strrq1;
                //-----------------------------------------
                book.Export(outputfile.ToString());

                System.Diagnostics.Process process = new Process();
                process.StartInfo.FileName = outputfile as string;
                process.Start();
                process.WaitForExit();
                if (MessageHelper.ShowYesNoInfo("是否将报告上传到服务器？") == DialogResult.Yes)
                {
                    try
                    {
                        //-------------------------
                        SaveToWebGujia(outputfile.ToString(), base.GetControlBindValue(this.txt委托方).ToString(), this.bglx, base.GetControlBindValue(this.txt_ProjectId).ToString().Substring(base.GetControlBindValue(this.txt_ProjectId).ToString().IndexOf("-") + 1).Replace("-", ""), "明细表");
                        //-------------------------
                        //System.Diagnostics.Process process1 = new Process();
                        //process1.StartInfo.CreateNoWindow = true;
                        //process1.StartInfo.UseShellExecute = false;
                        //process1.StartInfo.FileName = Application.StartupPath + "\\WML2XSLT";
                        //process1.StartInfo.Arguments = "\"" + outputfile + "\" -o \"" + xsltFile + "\" -ns http://www.zgfdc.net/gujia";
                        //process1.Start();
                        //process1.WaitForExit();

                        //byte[] bytefile;
                        //FileStream file = File.Open(xsltFile, FileMode.Open);
                        //bytefile = new byte[(int)file.Length];
                        //file.Read(bytefile, 0, (int)file.Length);
                        //dtbReport.Rows[0].BeginEdit();
                        //dtbReport.Rows[0]["ReportData"] = bytefile;
                        //dtbReport.Rows[0].EndEdit();
                        //SkyMap.Net.DataForms.DataEngine.SQLDataEngine sqlDataEngine = new SkyMap.Net.DataForms.DataEngine.SQLDataEngine();
                        //sqlDataEngine.SaveData(this.dataFormController.DAODataForm.DataSource, dtbReport);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageHelper.ShowInfo("该报表已经打开了！");
            }

        }


        #region xml的特殊处理
        public static bool HasElement(XElement x, XNamespace ns, string e)
        {
            try
            {
                string tmp = x.Element(ns + e).Value.ToString();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static string HasElementReturnString(XElement x, XNamespace ns, string e)
        {
            try
            {
                if (HasElement(x, ns, e))
                {
                    string tmp = x.Element(ns + e).Value.ToString();
                    return tmp;
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }

        public static double HasElementReturnDouble(XElement x, XNamespace ns, string e)
        {
            try
            {
                if (HasElement(x, ns, e))
                {
                    double tmp = double.Parse(x.Element(ns + e).Value.ToString());
                    return tmp;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        private double GetMoneyStand(double totalPrice, string type)
        {
            double num = 0.0;
            string str = type;
            if (str != null)
            {
                str = string.IsInterned(str);
                if (str == "房地产")
                {
                    if (totalPrice <= 1000000.0)
                    {
                        return (totalPrice * 0.005);
                    }
                    if ((totalPrice > 1000000.0) && (totalPrice <= 10000000.0))
                    {
                        return (5000.0 + ((totalPrice - 1000000.0) * 0.0025));
                    }
                    if ((totalPrice > 10000000.0) && (totalPrice <= 20000000.0))
                    {
                        return (27500.0 + ((totalPrice - 10000000.0) * 0.0015));
                    }
                    if ((totalPrice <= 50000000.0) && (totalPrice > 20000000.0))
                    {
                        return (42500.0 + ((totalPrice - 20000000.0) * 0.0008));
                    }
                    if ((totalPrice <= 80000000.0) && (totalPrice > 50000000.0))
                    {
                        return (66500.0 + ((totalPrice - 50000000.0) * 0.0004));
                    }
                    if ((totalPrice <= 100000000.0) && (totalPrice > 80000000.0))
                    {
                        return (78500.0 + ((totalPrice - 80000000.0) * 0.0002));
                    }
                    if (totalPrice > 100000000.0)
                    {
                        num = 82500.0 + ((totalPrice - 100000000.0) * 0.0001);
                    }
                    return num;

                }
                if (str == "土地")
                {
                    if (totalPrice <= 1000000.0)
                    {
                        return (totalPrice * 0.004);
                    }
                    if ((totalPrice <= 2000000.0) && (totalPrice > 1000000.0))
                    {
                        return (4000.0 + ((totalPrice - 1000000.0) * 0.003));
                    }
                    if ((totalPrice <= 10000000.0) && (totalPrice > 2000000.0))
                    {
                        return (7000.0 + ((totalPrice - 2000000.0) * 0.002));
                    }
                    if ((totalPrice <= 20000000.0) && (totalPrice > 10000000.0))
                    {
                        return (23000.0 + ((totalPrice - 10000000.0) * 0.0015));
                    }
                    if ((totalPrice <= 50000000.0) && (totalPrice > 20000000.0))
                    {
                        return (38000.0 + ((totalPrice - 20000000.0) * 0.0008));
                    }
                    if ((totalPrice <= 100000000.0) && (totalPrice > 50000000.0))
                    {
                        return (62000.0 + ((totalPrice - 50000000.0) * 0.0004));
                    }
                    if (totalPrice > 100000000.0)
                    {
                        num = 82000.0 + ((totalPrice - 100000000.0) * 0.0001);
                    }
                    return num;
                }
                if (str != "资产")
                {
                    return num;
                }
                if (totalPrice < 1000000.0)
                {
                    return (totalPrice * 0.006);
                }
                if ((totalPrice < 10000000.0) && (totalPrice >= 1000000.0))
                {
                    return (6000.0 + ((totalPrice - 1000000.0) * 0.0025));
                }
                if ((totalPrice < 50000000.0) && (totalPrice >= 10000000.0))
                {
                    return (28500.0 + ((totalPrice - 10000000.0) * 0.0008));
                }
                if ((totalPrice < 100000000.0) && (totalPrice >= 50000000.0))
                {
                    return (60500.0 + ((totalPrice - 50000000.0) * 0.0005));
                }
                if (totalPrice >= 100000000.0)
                {
                    num = 85500.0 + ((totalPrice - 100000000.0) * 0.0001);
                }
            }
            return num;
        }

        private void txt备注_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt折扣_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                double b;
                if (double.TryParse(txt标准价格.Text.ToString(), out b) && double.TryParse(txt折扣.Text.ToString(), out b))
                {
                    txt开票收费.Text = AppraiseClass.QuZhen(AppraiseClass.S4J5(double.Parse(txt标准价格.Text.ToString()) * double.Parse(txt折扣.Text.ToString()), 0), 1).ToString();
                }
            }
            catch
            {
            }
        }

        private void qm_收费通知书_Click(object sender, EventArgs e)
        {
            base.Save();
            string tmp = "";
            tmp = this.txt备注.Text.ToString();
            if (tmp.IndexOf("收费通知书") < 0)
            {
                this.txt备注.Text = tmp == "" ? "收费通知书" : (tmp + "，收费通知书");
            }
            this.bglx = "doc";
            DataFormPrint("收费通知书");

            //SkyMap.Net.DAO.SMDataSource smds = SkyMap.Net.DAO.QueryHelper.Get<SMDataSource>("SMDataSource_1c9905db15c1465f825867d269ea0a9b", "1c9905db15c1465f825867d269ea0a9b");
            //smds.CreateConnection();
            //SkyMap.Net.DAO.DAOCacheService.Get();
            //if(SkyMap.Net.DAO.DAOCacheService.Contains(""))

        }

        private void LoginWeb(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string ss = e.Url.ToString();
            if (ss.IndexOf("login.aspx") >= 0 || ss.IndexOf("Login.aspx") >= 0)
            {
                string tmpsql = "select * from [user] where Name='{0}'";
                tmpsql = string.Format(tmpsql, (string)this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PStaffName, ""));
                DataTable tmpTable = SkyMap.Net.DAO.QueryHelper.ExecuteSql("gujia", string.Empty, tmpsql);
                try
                {
                    if (tmpTable != null)
                    {
                        this._windowManager.ActiveBrowser.Document.All["txtUserPass"].InnerText = "gujia2009";
                        this._windowManager.ActiveBrowser.Document.All["txtUserName"].InnerText = tmpTable.Rows[0]["LoginName"].ToString();
                        this._windowManager.ActiveBrowser.Document.All["btLogin"].InvokeMember("Click");
                    }
                }
                catch
                {
                    MessageHelper.ShowInfo("请联系管理员添加此用户");
                }
            }
        }



        //位置区域和环境改变
        //private void repositoryItemMemoExEdit3_EditValueChanged(object sender, EventArgs e)
        //{
        //    //DataRow drB = ((DevExpress.XtraGrid.Views.Card.CardView)gridB1.FocusedView).GetDataRow(((DevExpress.XtraGrid.Views.Card.CardView)gridB1.FocusedView).FocusedRowHandle);
        //    //try
        //    //{
        //    //    drB.BeginEdit();
        //    //    drB["位置区域和环境"] = XMLEncodeHelper.Encode("<div>sss</div><p>yyyyy</p>");
        //    //    drB.EndEdit();
        //    //}
        //    //catch
        //    //{

        //    //}
        //}

        /// <summary>
        /// 将生成的报告上传到webgujia服务器上
        /// </summary>
        /// <param name="filename">本地存储的文件名</param>
        /// <param name="DocID">上传文件的用户名</param>
        /// <param name="DocTitle">委托方</param>
        /// <param name="DocType">doc或xls</param>
        /// <param name="ReportId">项目编号</param>
        /// <param name="ReportAbout">项目备注</param>
        private void SaveToWebGujia(string filename, string DocTitle, string DocType, string ReportId, string ReportAbout)
        {
            System.Data.IDbConnection dbcn = DBSessionFactory.Instance.GetConnection(typeof(GujiaDataForm));//as System.Data.Common.DbConnection;

            string strSql = "Insert into Doc(DocID,DocTitle,DocType,ReportId,ReportAbout,DocContent,DocDate) values(@DocId,@DocTitle,@DocType,@ReportId,@ReportAbout,@FImage,@DocDate)";
            // OleDbCommand comd = new OleDbCommand(strSql, objConnection); //执行sql语句
            SqlCommand comd = new SqlCommand(strSql, (SqlConnection)dbcn as SqlConnection);
            comd.Parameters.Add("@DocId", SqlDbType.VarChar, 20).Value = this.DataFormConntroller.GetParamValue(SkyMap.Net.DataForms.ParamNames.PStaffName, "");  //定义参数同时给它赋值
            if (DocTitle != "")
                comd.Parameters.Add("@DocTitle", SqlDbType.VarChar, 50).Value = DocTitle;
            comd.Parameters.Add("@DocType", SqlDbType.VarChar, 10).Value = DocType;
            if (ReportId != "")
                comd.Parameters.Add("@ReportId", SqlDbType.VarChar, 50).Value = ReportId;
            if (ReportAbout != "")
                comd.Parameters.Add("@ReportAbout", SqlDbType.VarChar, 50).Value = ReportAbout;
            comd.Parameters.Add("@FImage", SqlDbType.Binary);
            comd.Parameters["@FImage"].Value = SetFileToByteArray(filename);
            comd.Parameters.Add("@DocDate", SqlDbType.DateTime, 50);
            comd.Parameters["@DocDate"].Value = System.DateTime.Now.ToString();
            comd.ExecuteNonQuery(); //执行查询
        }
        public byte[] SetFileToByteArray(string fileName)
        {

            FileStream fs = new FileStream(fileName, FileMode.Open);

            int streamLength = (int)fs.Length;

            byte[] image = new byte[streamLength];

            fs.Read(image, 0, streamLength);

            fs.Close();

            return image;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string strsearch = txtSearchAddress.Text.ToString();

            DataSet m_dst = SkyMap.Net.DAO.QueryHelper.ExecuteSqls("Default", string.Empty, new string[] { @"select 报告名 as 编号,座落,位置区域和环境,对象状况 from yw_gujia_b where  对象状况  is not null and 位置区域和环境   is not null and 座落 like '%" + strsearch + "%' order by Bid asc" }, new string[] { "YW_gujia_b" });
            if (m_dst != null && m_dst.Tables.Count != 0)
            {
                //m_dst.Tables["YW_gujia_b"].ExtendedProperties.Add("selectsql", @"select bid,报告名,座落 from yw_gujia_b order by bid asc");
                gdSearch.DataSource = m_dst;
                gdSearch.DataMember = "yw_gujia_b";
            }



        }

        private void gridView2_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {

        }

        private void gridView2_DoubleClick(object sender, EventArgs e)
        {
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle);
            string tmp = dr["位置区域和环境"].ToString();
            string tmp1 = dr["对象状况"] as string ?? "";
            if (XtraTabSearch.SelectedTabPageIndex == 0)
            {
                txtSearch1.Text = tmp;
            }
            else
            {
                txtSearch2.Text = tmp1;
            }

        }

        private void panelShow_DoubleClick(object sender, EventArgs e)
        {
            seachclose();
        }

        private void seachclose()
        {
            panelShow.Visible = false;
            if (this.comboxSearch.EditValue.ToString().IndexOf("不") != 0)
            {
                DataRow dr = ((DataRowView)cardView1.GetRow(cardView1.FocusedRowHandle)).Row;
                dr["位置区域和环境"] = txtSearch1.Text.ToString();
                dr["对象状况"] = txtSearch2.Text.ToString();
            }
        }

        private void cardView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this.comboxSearch.EditValue.ToString().IndexOf("不") != 0)
                {
                    DataRow dr = ((DataRowView)cardView1.GetRow(cardView1.FocusedRowHandle)).Row;
                    panelShow.Visible = true;
                    txtSearch1.Text = dr["位置区域和环境"].ToString();
                    txtSearch2.Text = dr["对象状况"] as string ?? "";
                }
            }
            catch
            {
            }
        }

        private void btnSearchClose_Click(object sender, EventArgs e)
        {
            seachclose();
        }

        private void cbeAppraiseMoney_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string tmpstring = ((DevExpress.XtraEditors.ComboBoxEdit)sender).EditValue.ToString();
                if (tmpstring == "房地产" || tmpstring == "土地")
                {
                    txt标准价格.Text = GetMoneyStand(double.Parse(txt评估总值.Text.Replace(",","").ToString()), tmpstring).ToString("#,#");
                }
                else
                {
                  
                }
            }
            catch
            {
            }
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            txt实际收费.Text = txt开票收费.Text;
        }



        private void simpleButton5_Click(object sender, EventArgs e)
        {
            List<string> lstdsyz = new List<string>();//土地使用者
            List<string> lsbgm = new List<string>(); //报告名
            List<string> lspglx = new List<string>();//评估类型
            List<string> lszl = new List<string>();//座落
            List<string> lsgjmd = new List<string>();//估价目的;
            double tdmj = 0.0d;  //土地面积
            double jzmj = 0.0d;  //建筑面积
            double pgzz = 0.0d;  //评估总值
            DataView dv = m_dstAll.Tables["yw_gujia_b"].DefaultView;
            DataTable dt = m_dstAll.Tables["yw_gujia_b"];
            dv.RowFilter = "[报告号] < 65";
            if (dv.Count > 0)
            {
                #region 获取相关数据
                foreach (DataRow dr in dt.Rows)
                {
                    if (int.Parse(dr["报告号"].ToString()) < 65)
                    {
                        string tmp = dr["土地使用者"].ToString();
                        if (!lstdsyz.Contains(tmp))
                        {
                            lstdsyz.Add(tmp);
                        }

                        tmp = dr["报告名"].ToString();
                        if (!lsbgm.Contains(tmp))
                        {
                            lsbgm.Add(tmp);
                        }

                        tmp = dr["评估类型"].ToString();
                        if (!lspglx.Contains(tmp))
                        {
                            lspglx.Add(tmp);
                        }

                        tmp = dr["座落"].ToString();
                        if (!lszl.Contains(tmp))
                        {
                            lszl.Add(tmp);
                        }

                        tmp = dr["估价目的"].ToString();
                        if (!lsgjmd.Contains(tmp))
                        {
                            lsgjmd.Add(tmp);
                        }

                        tmp = dr["评估最后采用建筑面积"].ToString();
                        if(!string.IsNullOrEmpty(tmp))
                        {
                            jzmj = jzmj + double.Parse(tmp);
                        }

                        tmp = dr["评估最后采用土地面积"].ToString();
                        if (!string.IsNullOrEmpty(tmp))
                        {
                            tdmj = tdmj + double.Parse(tmp);
                        }

                        if (dr["评估类型"].ToString() == "房地产")
                        {
                            tmp = dr["评估确定房地产总价"].ToString();
                            if (!string.IsNullOrEmpty(tmp))
                            {
                                pgzz = pgzz + double.Parse(tmp);
                            }
                            
                        }
                        else if (dr["评估类型"].ToString() == "土地")
                        {
                            tmp = dr["评估确定土地总价"].ToString();
                            if (!string.IsNullOrEmpty(tmp))
                            {
                                pgzz = pgzz + double.Parse(tmp);
                            }
                        }
                        else
                        {
                            MessageBox.Show("评估类型请选择房地产或土地!", "提示：");
                        }

                    }
                }
                #endregion
                string strsplit = "/-";

                //string result = string.Join("/-", studentNames.ToArray());
                //List<string> newStudentNames = new List<string>(result.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries));


            }
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (int.Parse(dr["报告号"].ToString()) < 65)
                    {

                    }
                }

            }
        }

        private void txt初评档案_Click(object sender, EventArgs e)
        {
           
        }

        private void groupControl初评档案归档_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textEdit11_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void groupControl8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dateEdit18_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void labelControl62_Click(object sender, EventArgs e)
        {

        }

        private void textEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void textEdit27_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void textEdit26_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void textEdit20_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void label25_Click(object sender, EventArgs e)
        {

        }

        private void groupControl6_Paint(object sender, PaintEventArgs e)
        {

        }

        private bool checkdata()
        {
           return CreateData("");
        }

        private void bt初评建档_Click(object sender, EventArgs e)
        {
            List<string> lstdsyz = new List<string>();//土地使用者
            List<string> lsbgm = new List<string>(); //报告名
            List<string> lspglx = new List<string>();//评估类型
            List<string> lszl = new List<string>();//座落
            List<string> lsgjmd = new List<string>();//估价目的;
            List<string> lstdzh = new List<string>();//土地证号;
            int itd = 0; //计算土地类型评估报告份数
            int ifc = 0; //计算房地产类型评估报告份数
            double tdmj = 0.0d;  //土地面积
            double jzmj = 0.0d;  //建筑面积
            double pgzz = 0.0d;  //评估总值
            DataView dv = m_dstAll.Tables["yw_gujia_b"].DefaultView;
            DataTable dt = m_dstAll.Tables["yw_gujia_b"];
            dv.RowFilter = "[报告号] < 65";
            if (dv.Count > 0)
            {
                #region 获取相关数据
                foreach (DataRow dr in dt.Rows)
                {
                    if (int.Parse(dr["报告号"].ToString()) < 65)
                    {
                        string tmp = dr["土地使用者"].ToString();
                        if (!lstdsyz.Contains(tmp))
                        {
                            lstdsyz.Add(tmp);
                        }

                       

                        tmp = dr["报告名"].ToString();
                        if (!lsbgm.Contains(tmp))
                        {
                            lsbgm.Add(tmp);
                        }

                        tmp = dr["评估类型"].ToString();
                        if (!lspglx.Contains(tmp))
                        {
                            lspglx.Add(tmp);
                        }

                        tmp = dr["座落"].ToString();
                        if (!lszl.Contains(tmp))
                        {
                            lszl.Add(tmp);
                        }

                        tmp = dr["估价目的"].ToString();
                        if (!lsgjmd.Contains(tmp))
                        {
                            lsgjmd.Add(tmp);
                        }

                        tmp = dr["评估最后采用建筑面积"].ToString();
                        if (!string.IsNullOrEmpty(tmp))
                        {
                            jzmj = jzmj + double.Parse(tmp);
                        }

                        tmp = dr["土地使用权面积"].ToString();
                        if (!string.IsNullOrEmpty(tmp))
                        {
                            if (!lstdzh.Contains(dr["土地证号"].ToString()))
                            {
                                tdmj = tdmj + double.Parse(tmp);
                            }
                        }

                        if (dr["评估类型"].ToString() == "房地产")
                        {
                            ifc++;
                            tmp = dr["评估确定房地产总价"].ToString();
                            if (!string.IsNullOrEmpty(tmp))
                            {
                                pgzz = pgzz + double.Parse(tmp);
                            }

                        }
                        else if (dr["评估类型"].ToString() == "土地")
                        {
                            itd++;
                            tmp = dr["评估确定土地总价"].ToString();

                            if (!string.IsNullOrEmpty(tmp))
                            {
                                if (!lstdzh.Contains(dr["土地证号"].ToString()))
                                {
                                    pgzz = pgzz + double.Parse(tmp);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("评估类型请选择房地产或土地!", "提示：");
                            return;
                        }

                        tmp = dr["土地证号"].ToString();
                        if (!lstdzh.Contains(tmp))
                        {
                            lstdzh.Add(tmp);
                        }

                    }
                }
                #endregion
                string strsplit = "、";

                //string result = string.Join("/-", studentNames.ToArray());
                //List<string> newStudentNames = new List<string>(result.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries));
                txt初评档案估价报告编号.Text = string.Join(strsplit, lsbgm.ToArray());
                txt初评档案土地使用者.Text = string.Join(strsplit, lstdsyz.ToArray());
                txt初评档案评估类型.Text = string.Join(strsplit, lspglx.ToArray());
                txt初评档案座落.Text = string.Join(strsplit, lszl.ToArray());
                txt初评档案评估目的.Text = string.Join(strsplit, lsgjmd.ToArray());
                txt初评档案评估总值.Text = pgzz.ToString();
                txt初评档案土地面积.Text = tdmj.ToString();
                txt初评档案建筑面积.Text = jzmj.ToString();
                txt初评档案初评.Checked = true;
                txt初评档案计算房地产报告份数.Text = ifc.ToString();
                txt初评档案计算土地报告份数.Text = itd.ToString();
            }
            else
            {
                MessageBox.Show("初评没有找到记录!", "提示：");
            }
        }

        private void bt正式报告建档_Click(object sender, EventArgs e)
        {
            
            List<string> lstdsyz = new List<string>();//土地使用者
            List<string> lsbgm = new List<string>(); //报告名
            List<string> lspglx = new List<string>();//评估类型
            List<string> lszl = new List<string>();//座落
            List<string> lsgjmd = new List<string>();//估价目的;
            List<string> lstdzh = new List<string>();//土地证号;
            if (checkdata() == false) return;
            double tdmj = 0.0d;  //土地面积
            double jzmj = 0.0d;  //建筑面积
            double pgzz = 0.0d;  //评估总值
            int itd = 0;
            int ifc = 0;
            DataView dv = m_dstAll.Tables["yw_gujia_b"].DefaultView;
            DataTable dt = m_dstAll.Tables["yw_gujia_b"];
            dv.RowFilter = "[报告号] >= 65";
            if (dv.Count > 0)
            {
                #region 获取相关数据
                foreach (DataRow dr in dt.Rows)
                {
                    if (int.Parse(dr["报告号"].ToString()) >= 65)
                    {
                        string tmp = dr["土地使用者"].ToString();
                        if (!lstdsyz.Contains(tmp))
                        {
                            lstdsyz.Add(tmp);
                        }
                       

                        tmp = dr["报告名"].ToString();
                        if (!lsbgm.Contains(tmp))
                        {
                            lsbgm.Add(tmp);
                        }

                        tmp = dr["评估类型"].ToString();
                        if (!lspglx.Contains(tmp))
                        {
                            lspglx.Add(tmp);
                        }

                        tmp = dr["座落"].ToString();
                        if (!lszl.Contains(tmp))
                        {
                            lszl.Add(tmp);
                        }

                        tmp = dr["估价目的"].ToString();
                        if (!lsgjmd.Contains(tmp))
                        {
                            lsgjmd.Add(tmp);
                        }

                        tmp = dr["评估最后采用建筑面积"].ToString();
                        if (!string.IsNullOrEmpty(tmp))
                        {
                            jzmj = jzmj + double.Parse(tmp);
                        }

                        tmp = dr["土地使用权面积"].ToString();
                        if (!string.IsNullOrEmpty(tmp))
                        {
                            if (!lstdzh.Contains(dr["土地证号"].ToString()))
                            {
                                tdmj = tdmj + double.Parse(tmp);
                            }
                        }

                        if (dr["评估类型"].ToString() == "房地产")
                        {
                            ifc++;
                            tmp = dr["评估确定房地产总价"].ToString();
                            if (!string.IsNullOrEmpty(tmp))
                            {
                                pgzz = pgzz + double.Parse(tmp);
                            }

                        }
                        else if (dr["评估类型"].ToString() == "土地")
                        {
                            itd++;
                            tmp = dr["评估确定土地总价"].ToString();

                            if (!string.IsNullOrEmpty(tmp))
                            {
                                if (!lstdzh.Contains(dr["土地证号"].ToString()))
                                {
                                    pgzz = pgzz + double.Parse(tmp);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("评估类型请选择房地产或土地!", "提示：");
                            return;
                        }

                        tmp = dr["土地证号"].ToString();
                        if (!lstdzh.Contains(tmp))
                        {
                            lstdzh.Add(tmp);
                        }

                    }
                }
                #endregion              

            }
            else
            {
                if (dt.Rows.Count > 0)
                {
                    #region 获取相关数据
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (int.Parse(dr["报告号"].ToString()) < 65)
                        {
                            string tmp = dr["土地使用者"].ToString();
                            if (!lstdsyz.Contains(tmp))
                            {
                                lstdsyz.Add(tmp);
                            }

                            tmp = dr["报告名"].ToString();
                            if (!lsbgm.Contains(tmp))
                            {
                                lsbgm.Add(tmp);
                            }

                            tmp = dr["评估类型"].ToString();
                            if (!lspglx.Contains(tmp))
                            {
                                lspglx.Add(tmp);
                            }

                            tmp = dr["座落"].ToString();
                            if (!lszl.Contains(tmp))
                            {
                                lszl.Add(tmp);
                            }

                            tmp = dr["估价目的"].ToString();
                            if (!lsgjmd.Contains(tmp))
                            {
                                lsgjmd.Add(tmp);
                            }

                            tmp = dr["评估最后采用建筑面积"].ToString();
                            if (!string.IsNullOrEmpty(tmp))
                            {
                                jzmj = jzmj + double.Parse(tmp);
                            }

                            tmp = dr["土地使用权面积"].ToString();
                            if (!string.IsNullOrEmpty(tmp))
                            {
                                if (!lstdzh.Contains(dr["土地证号"].ToString()))
                                {
                                    tdmj = tdmj + double.Parse(tmp);
                                }
                            }

                            if (dr["评估类型"].ToString() == "房地产")
                            {
                                ifc++;
                                tmp = dr["评估确定房地产总价"].ToString();
                                if (!string.IsNullOrEmpty(tmp))
                                {
                                    pgzz = pgzz + double.Parse(tmp);
                                }

                            }
                            else if (dr["评估类型"].ToString() == "土地")
                            {
                                itd++;
                                tmp = dr["评估确定土地总价"].ToString();

                                if (!string.IsNullOrEmpty(tmp))
                                {
                                    if (!lstdzh.Contains(dr["土地证号"].ToString()))
                                    {
                                        pgzz = pgzz + double.Parse(tmp);
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("评估类型请选择房地产或土地!", "提示：");
                                return;
                            }


                            tmp = dr["土地证号"].ToString();
                            if (!lstdzh.Contains(tmp))
                            {
                                lstdzh.Add(tmp);
                            }

                        }
                    }
                    #endregion              
                }
                else
                {
                    MessageBox.Show("初评没有找到记录!", "提示：");
                    return;
                }
            }
            string strsplit = "、";
            //string result = string.Join("/-", studentNames.ToArray());
            //List<string> newStudentNames = new List<string>(result.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries));
            txt正式报告档案估价报告编号.Text = string.Join(strsplit, lsbgm.ToArray());
            txt正式报告档案土地使用者.Text = string.Join(strsplit, lstdsyz.ToArray());
            txt正式报告档案座落.Text = string.Join(strsplit, lszl.ToArray());
            txt正式报告档案评估类型.Text = string.Join(strsplit, lspglx.ToArray());
            txt正式报告档案评估目的.Text = string.Join(strsplit, lsgjmd.ToArray());
            txt正式报告档案评估总值.Text = pgzz.ToString();
            txt正式报告档案土地面积.Text = tdmj.ToString();
            txt正式报告档案建筑面积.Text = jzmj.ToString();
            txt正式报告档案正式报告.Checked = true;
            txt正式报告档案计算房地产报告份数.Text = ifc.ToString();
            txt正式报告档案计算土地报告份数.Text = itd.ToString();
            return;
        }

        #region 签名
        private void qm_初评档案录入人员签名_Click(object sender, EventArgs e)
        {
            Qm(this.txt初评档案录入人员, this.txt初评档案录入日期);
        }

        private void qm_初评档案估价人员签名_Click(object sender, EventArgs e)
        {
            Qm(this.txt初评档案估价人员, this.txt初评档案估价日期);
        }

        private void qm_初评档案一审估价师签名_Click(object sender, EventArgs e)
        {
            Qm(this.txt初评档案一审估价师, this.txt初评档案一审日期);
        }

        private void qm_初评档案一审修改估价师签名_Click(object sender, EventArgs e)
        {
            Qm(this.txt初评档案一审修改估价师, this.txt初评档案一审修改日期);
        }

        private void qm_初评档案二审估价师签名_Click(object sender, EventArgs e)
        {
            Qm(this.txt初评档案二审估价师, this.txt初评档案二审日期);
        }

        private void qm_初评档案二审修改估价师签名_Click(object sender, EventArgs e)
        {
            Qm(this.txt初评档案二审修改估价师, this.txt初评档案二审修改日期);
        }

        private void qm_初评档案装订人员签名_Click(object sender, EventArgs e)
        {
            Qm(this.txt初评档案装订人员, this.txt初评档案装订日期);
        }

        private void qm_初评档案归档人员签名_Click(object sender, EventArgs e)
        {
            Qm(this.txt初评档案归档人员, this.txt初评档案归档日期);
        }

        private void qm_正式报告档案录入人员签名_Click(object sender, EventArgs e)
        {
            Qm(this.txt正式报告档案录入人员, this.txt正式报告档案录入日期);
        }

        private void qm_正式报告档案估价人员签名_Click(object sender, EventArgs e)
        {
            Qm(this.txt正式报告档案估价人员, this.txt正式报告档案估价日期);
        }

        private void qm_正式报告档案一审估价师签名_Click(object sender, EventArgs e)
        {
            Qm(this.txt正式报告档案一审估价师, this.txt正式报告档案一审日期);
        }

        private void qm_正式报告档案一审修改估价师签名_Click(object sender, EventArgs e)
        {
            Qm(this.txt正式报告档案一审修改估价师, this.txt正式报告档案一审修改日期);
        }

        private void qm_正式报告档案二审估价师签名_Click(object sender, EventArgs e)
        {
            Qm(this.txt正式报告档案二审估价师, this.txt正式报告档案二审日期);
        }

        private void qm_正式报告档案二审修改估价师签名_Click(object sender, EventArgs e)
        {
            Qm(this.txt正式报告档案二审修改估价师, this.txt正式报告档案二审修改日期);
        }

        private void qm_正式报告档案装订人员签名_Click(object sender, EventArgs e)
        {
            Qm(this.txt正式报告档案装订人员, this.txt正式报告档案装订日期);
        }

        private void qm_正式报告档案归档人员签名_Click(object sender, EventArgs e)
        {
            Qm(this.txt正式报告档案归档人员, this.txt正式报告档案归档日期);
        }

        private void qm_正式报告档案财务经办人员签名_Click(object sender, EventArgs e)
        {
            Qm(this.txt正式报告档案财务经办人员, this.txt正式报告档案财务经办日期);
        }

        private void qm_正式报告档案财务收费人员签名_Click(object sender, EventArgs e)
        {
            Qm(this.txt正式报告档案财务收费人员, this.txt正式报告档案财务收费日期);
        }
        #endregion

        private void lue_财务业务来源_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void lue_财务业务联系人_EditValueChanged(object sender, EventArgs e)
        {

        }




    }
}
