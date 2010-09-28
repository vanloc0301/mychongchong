namespace SkyMap.Net.Workflow.Client.View
{
    using DevExpress.XtraTab;
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.DataForms;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Gui.Components;
    using SkyMap.Net.Workflow.Client;
    using SkyMap.Net.Workflow.Client.Box;
    using SkyMap.Net.Workflow.Client.Dialog;
    using SkyMap.Net.Workflow.Client.Services;
    using SkyMap.Net.Workflow.Engine;
    using SkyMap.Net.Workflow.Instance;
    using SkyMap.Net.Workflow.XPDL;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class WfView : SmBarForm, IWfView, IOwnerState
    {
        private bool canPass = false;
        private bool canRemove;
        private bool changed = false;
        private Container components = null;
        private DataRowView currentDataRowView;
        private UnitOfWork currentUnitOfWork;
        private IWfDataForm dataForm;
        private List<IEditView> editViews = new List<IEditView>(2);
        private bool forceCloseWithoutSaveChanged = false;
        private DataRowView[] navigationDataRowViews;
        private int[] navigationIndexs;
        private XtraTabPage pAdjunct = null;
        private XtraTabPage pData = null;
        private XtraTabPage pFlowInfo = null;
        private XtraTabPage pMaters = null;
        private XtraTabPage pNotion = null;
        private SkyMap.Net.DataForms.PrintSet printSet;
        private string printSetId;
        private bool reBuildPrint = false;
        private XtraTabControl tblClient;
        private TempletPrint templatePrint;
        private IWfBox wfBox;
        private WfViewState wfViewState = WfViewState.None;
        private SkyMap.Net.Workflow.Engine.WorkItem workItem;

        public event WfViewNavigationHandle Navigate;

        public event EventHandler ViewClosed;

        public WfView()
        {
            this.InitializeComponent();
            base.TopLevel = true;
            base.ResizeRedraw = false;
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        public bool CanNavigation(int item)
        {
            return (this.navigationDataRowViews[item] != null);
        }

        private XtraTabPage CreateTabPage(string name, string imageResourceKey, string text)
        {
            XtraTabPage page = new XtraTabPage();
            page.Name = name;
            page.Image = ResourceService.GetBitmap(imageResourceKey);
            page.Text = text;
            return page;
        }

        private void currentUnitOfWork_Changed(object sender, EventArgs e)
        {
            this.Changed = true;
        }

        private void currentUnitOfWork_Cleared(object sender, EventArgs e)
        {
            if (!((this.dataForm != null) && this.dataForm.IsChanged))
            {
                this.Changed = false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WfView));
            this.tblClient = new XtraTabControl();
            base.toolStripContainer.ContentPanel.SuspendLayout();
            base.toolStripContainer.SuspendLayout();
            this.tblClient.BeginInit();
            base.SuspendLayout();
            base.toolStripContainer.BottomToolStripPanelVisible = true;
            base.toolStripContainer.ContentPanel.Controls.Add(this.tblClient);
            base.toolStripContainer.ContentPanel.Size = new Size(0x282, 0x1e6);
            base.toolStripContainer.LeftToolStripPanelVisible = true;
            base.toolStripContainer.RightToolStripPanelVisible = true;
            base.toolStripContainer.Size = new Size(0x282, 0x1ff);
            base.toolStripContainer.TopToolStripPanelVisible = true;
            this.tblClient.Dock = DockStyle.Fill;
            this.tblClient.Location = new Point(0, 0);
            this.tblClient.Name = "tblClient";
            this.tblClient.Size = new Size(0x282, 0x1e6);
            this.tblClient.TabIndex = 0;
            this.tblClient.SelectedPageChanged += new TabPageChangedEventHandler(this.tblClient_SelectedIndexChanged);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            base.ClientSize = new Size(0x282, 0x1ff);
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.LookAndFeel.UseDefaultLookAndFeel = false;
            base.Name = "WfView";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "WfForm";
            base.WindowState = FormWindowState.Maximized;
            base.Closed += new EventHandler(this.WfView_Closed);
            base.Closing += new CancelEventHandler(this.WfView_Closing);
            base.toolStripContainer.ContentPanel.ResumeLayout(false);
            base.toolStripContainer.ResumeLayout(false);
            base.toolStripContainer.PerformLayout();
            this.tblClient.EndInit();
            base.ResumeLayout(false);
        }

        private void LoadFlowInfo()
        {
            WaitDialogHelper.Show();
            try
            {
                FlowInfo info;
                if (!this.pFlowInfo.HasChildren)
                {
                    info = new FlowInfo();
                    info.Dock = DockStyle.Fill;
                    this.pFlowInfo.Controls.Add(info);
                }
                else
                {
                    info = this.pFlowInfo.Controls[0] as FlowInfo;
                }
                if (LoggingService.IsDebugEnabled)
                {
                    LoggingService.DebugFormatted("将显示业务编号：{0} 的流程信息", new object[] { this.workItem.ProjectId });
                }
                info.ProinstId = this.workItem.ProinstId;
            }
            finally
            {
                WaitDialogHelper.Close();
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            base.CreateToolbar();
        }

        public void Pass()
        {
            if (this.CanPass)
            {
                this.Save();
                BoxHelper.PassToNext(this.workItem, this.currentDataRowView, new PassDialog());
                base.Close();
            }
            else
            {
                MessageHelper.ShowInfo("该业务不能转出，可能是该业务已结案或者你不是在在办环节操作该业务！");
            }
        }

        private void PostEditor()
        {
            if (this.dataForm != null)
            {
                this.dataForm.PostEditor();
            }
            foreach (IEditView view in this.editViews)
            {
                view.PostEditor();
            }
        }

        public void Print()
        {
            if (this.templatePrint != null)
            {
                if (this.Changed)
                {
                    this.Save();
                }
                this.dataForm.Print(this.templatePrint, this.reBuildPrint);
            }
        }

        public bool Save()
        {
            this.PostEditor();
            if ((this.dataForm != null) && !this.dataForm.Save())
            {
                return false;
            }
            if (this.currentUnitOfWork != null)
            {
                this.currentUnitOfWork.Commit();
            }
            this.Changed = false;
            return true;
        }

        public void SetCurrentDataRowView(DataRowView row)
        {
            this.currentDataRowView = row;
        }

        public void Show(string viewContent, IWfDataForm dataForm, SkyMap.Net.Workflow.Engine.WorkItem workItem)
        {
            this.workItem = workItem;
            XtraTabPage[] pageArray = new XtraTabPage[] { this.pData, this.pNotion, this.pMaters, this.pAdjunct, this.pFlowInfo };
            string[] strArray = new string[] { "业务数据", "经办人意见", "收件资料", "业务附件", "流程信息" };
            string[] strArray2 = new string[] { "Workflow.WfView.DataTabPageImage", "Workflow.WfView.StaffNotionTabPageImage", "Workflow.WfView.MaterEditTabPageImage", "Workflow.WfView.MaterEditTabPageImage", "Workflow.WfView.AdjunctEditTabPageImage", "Workflow.WfView.FlowInfoTabPageImage" };
            string[] strArray3 = new string[] { "pData", "pNotion", "pMaters", "pNotion", "pFlowInfo" };
            for (int i = 0; i < strArray.Length; i++)
            {
                if (string.IsNullOrEmpty(viewContent) || (viewContent.IndexOf(strArray[i]) >= 0))
                {
                    if (pageArray[i] == null)
                    {
                        pageArray[i] = this.CreateTabPage(strArray3[i], strArray2[i], strArray[i]);
                        switch (i)
                        {
                            case 0:
                                this.pData = pageArray[i];
                                break;

                            case 1:
                                this.pNotion = pageArray[i];
                                break;

                            case 2:
                                this.pMaters = pageArray[i];
                                break;

                            case 3:
                                this.pAdjunct = pageArray[i];
                                break;

                            case 4:
                                this.pFlowInfo = pageArray[i];
                                break;
                        }
                    }
                    if (!((pageArray[i] == null) || this.tblClient.TabPages.Contains(pageArray[i])))
                    {
                        this.tblClient.TabPages.Add(pageArray[i]);
                    }
                    if (LoggingService.IsDebugEnabled)
                    {
                        LoggingService.DebugFormatted("将显示业务表单：{0}", new object[] { strArray[i] });
                    }
                    continue;
                }
                if ((pageArray[i] != null) && this.tblClient.TabPages.Contains(pageArray[i]))
                {
                    this.tblClient.TabPages.Remove(pageArray[i]);
                    if (LoggingService.IsDebugEnabled)
                    {
                        LoggingService.DebugFormatted("将不显示业务表单：{0}", new object[] { strArray[i] });
                    }
                }
            }
            this.dataForm = dataForm;
            if (((this.pData != null) && this.tblClient.TabPages.Contains(this.pData)) && (dataForm is Control))
            {
                Control control = dataForm as Control;
                if (control != null)
                {
                    control.Dock = DockStyle.Fill;
                }
                this.pData.Controls.Clear();
                this.pData.Controls.Add(control);
                dataForm.Changed += new EventHandler(this.currentUnitOfWork_Changed);
            }
            if (workItem != null)
            {
                this.Text = workItem.ProinstName + "-" + workItem.ProjectId;
                if (LoggingService.IsDebugEnabled)
                {
                    LoggingService.DebugFormatted("正在显示业务ID:{0}, 业务编号:{1}, 业务名称:{2} 的业务表单", new object[] { workItem.ProinstId, workItem.ProinstName, workItem.ProjectId });
                }
                Prodef prodef = WorkflowService.Prodefs[workItem.ProdefId];
                if (((prodef == null) || (prodef.Type != "FLW")) && (((this.pFlowInfo != null) && this.tblClient.TabPages.Contains(this.pFlowInfo)) && (string.IsNullOrEmpty(viewContent) || (viewContent.IndexOf("流程信息") < 0))))
                {
                    this.tblClient.TabPages.Remove(this.pFlowInfo);
                }
            }
            if (this.tblClient.TabPages.Count > 0)
            {
                this.tblClient.SelectedTabPageIndex = 0;
            }
            if (base.toolStripContainer.TopToolStripPanel.Controls.Count > 0)
            {
                base.BarStatusUpdate();
            }
            if (WorkbenchSingleton.MainForm != null)
            {
                base.Show(WorkbenchSingleton.MainForm);
            }
            else
            {
                base.Show();
            }
        }

        void IWfView.Close()
        {
            base.Close();
        }

        private void tblClient_SelectedIndexChanged(object sender, TabPageChangedEventArgs e)
        {
            XtraTabPage selectedTabPage = this.tblClient.SelectedTabPage;
            if (selectedTabPage != this.pData)
            {
                WaitDialogHelper.Show();
                try
                {
                    if (selectedTabPage == this.pFlowInfo)
                    {
                        this.LoadFlowInfo();
                    }
                    else
                    {
                        IEditView view = null;
                        if (!selectedTabPage.HasChildren)
                        {
                            if (selectedTabPage == this.pMaters)
                            {
                                view = new MatersEdit();
                            }
                            else if (selectedTabPage == this.pAdjunct)
                            {
                                view = new AdjunctEdit();
                            }
                            else if (selectedTabPage == this.pNotion)
                            {
                                view = new StaffNotion();
                            }
                            if (view != null)
                            {
                                Control control = view as Control;
                                control.Dock = DockStyle.Fill;
                                selectedTabPage.Controls.Add(control);
                            }
                        }
                        else if (((selectedTabPage == this.pMaters) || (selectedTabPage == this.pAdjunct)) || (selectedTabPage == this.pNotion))
                        {
                            view = selectedTabPage.Controls[0] as IEditView;
                        }
                        if (view != null)
                        {
                            if (LoggingService.IsDebugEnabled)
                            {
                                LoggingService.DebugFormatted("将显示业务：{0} - {1} 的数据...", new object[] { this.workItem.ProjectId, selectedTabPage.Text });
                            }
                            view.LoadData(this.CurrentUnitOfWork, this.workItem.ProinstId, this.workItem.ProdefId, this.workItem.AssignId, this.DataForm.ProjectSubTypes);
                        }
                    }
                }
                finally
                {
                    WaitDialogHelper.Close();
                }
            }
        }

        private void WfView_Closed(object sender, EventArgs e)
        {
            if (this.ViewClosed != null)
            {
                this.ViewClosed(this, e);
            }
        }

        private void WfView_Closing(object sender, CancelEventArgs e)
        {
            if (!this.ForceCloseWithoutSaveChanged && this.Changed)
            {
                this.PostEditor();
                DialogResult result = MessageHelper.ShowYesNoCancelInfo("数据已经改变，是否保存？");
                if (result == DialogResult.Yes)
                {
                    this.Save();
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
                else if (this.currentUnitOfWork != null)
                {
                    this.currentUnitOfWork.Clear();
                }
            }
            else if (this.currentUnitOfWork != null)
            {
                this.currentUnitOfWork.Clear();
            }
        }

        public void WfViewNavigation(int item)
        {
            if (this.navigationDataRowViews != null)
            {
                base.Visible = false;
                System.Windows.Forms.Application.DoEvents();
                this.WfView_Closed(this, null);
                DataRowView row = this.navigationDataRowViews[item];
                int index = this.navigationIndexs[item];
                if ((row == null) || (index <= -1))
                {
                    throw new WfClientException("Cannot navigation the item: item have changed");
                }
                this.Navigate(this.WfBox, this, row, index);
                base.BarStatusUpdate();
            }
        }

        public void WfViewNavigation(string item)
        {
            if (this.navigationDataRowViews != null)
            {
                base.Visible = false;
                System.Windows.Forms.Application.DoEvents();
                this.WfView_Closed(this, null);
                DataRowView row=null ;
                string[] tmpstr = item.Split(new char[] { '@' });
                DataView dt = ((System.Data.DataView)(this.WfBox.DataSource)).Table.DefaultView;
                int index = 0;
                foreach (DataRowView dr in dt)
                {
                    if (dr["PROJECT_ID"].ToString() == tmpstr[1].ToString())
                    {
                        row = (DataRowView)dr;
                        break;
                    }
                    index++;
                }
                if ((row == null) || (index <= -1))
                {
                    throw new WfClientException("Cannot navigation the item: item have changed");
                }
                this.Navigate(this.WfBox, this, row, index);
                base.BarStatusUpdate();
            }
        }

        public bool CanPass
        {
            get
            {
                return this.canPass;
            }
            set
            {
                if (value != this.canPass)
                {
                    this.canPass = value;
                    this.wfViewState = WfViewState.None;
                }
            }
        }

        public bool CanRemove
        {
            get
            {
                return this.canRemove;
            }
            set
            {
                this.canRemove = value;
                this.wfViewState = WfViewState.None;
            }
        }

        public bool Changed
        {
            get
            {
                return this.changed;
            }
            set
            {
                if (this.changed != value)
                {
                    this.wfViewState = WfViewState.None;
                    if (LoggingService.IsInfoEnabled)
                    {
                        LoggingService.InfoFormatted("业务表单修改状态从{0}到{1}！", new object[] { this.changed, value });
                    }
                    this.changed = value;
                    base.BarStatusUpdate();
                }
            }
        }

        public UnitOfWork CurrentUnitOfWork
        {
            get
            {
                if (this.currentUnitOfWork == null)
                {
                    this.currentUnitOfWork = new UnitOfWork(typeof(Proinst));
                    this.currentUnitOfWork.Changed += new EventHandler(this.currentUnitOfWork_Changed);
                    this.currentUnitOfWork.Cleared += new EventHandler(this.currentUnitOfWork_Cleared);
                }
                return this.currentUnitOfWork;
            }
        }

        public IWfDataForm DataForm
        {
            get
            {
                return this.dataForm;
            }
        }

        public bool ForceCloseWithoutSaveChanged
        {
            get
            {
                return this.forceCloseWithoutSaveChanged;
            }
            set
            {
                this.forceCloseWithoutSaveChanged = value;
            }
        }

        public Enum InternalState
        {
            get
            {
                if (this.wfViewState == WfViewState.None)
                {
                    if (this.Changed)
                    {
                        this.wfViewState |= WfViewState.CanSave;
                    }
                    if (this.CanPass)
                    {
                        this.wfViewState |= WfViewState.CanPass;
                    }
                    if (this.TemplatePrint != null)
                    {
                        this.wfViewState |= WfViewState.CanPrint;
                    }
                    if (this.CanRemove)
                    {
                        this.wfViewState |= WfViewState.CanRemove;
                    }
                    if (this.navigationDataRowViews != null)
                    {
                        if (this.navigationDataRowViews[0] != null)
                        {
                            this.wfViewState |= WfViewState.CanNavFirst;
                        }
                        if (this.navigationDataRowViews[1] != null)
                        {
                            this.wfViewState |= WfViewState.CanNavPrevious;
                        }
                        if (this.navigationDataRowViews[2] != null)
                        {
                            this.wfViewState |= WfViewState.CanNavNext;
                        }
                        if (this.navigationDataRowViews[3] != null)
                        {
                            this.wfViewState |= WfViewState.CanNavLast;
                        }
                    }
                    if (LoggingService.IsDebugEnabled)
                    {
                        LoggingService.DebugFormatted("WfView当前状态：{0} [{1},{2},{3},{4},{5},{6},{7},{8}]", new object[] { this.wfViewState, this.Changed, this.CanPass, this.TemplatePrint != null, this.CanRemove, (this.navigationDataRowViews != null) && (this.navigationDataRowViews[0] != null), (this.navigationDataRowViews != null) && (this.navigationDataRowViews[1] != null), (this.navigationDataRowViews != null) && (this.navigationDataRowViews[2] != null), (this.navigationDataRowViews != null) && (this.navigationDataRowViews[3] != null) });
                    }
                }
                return this.wfViewState;
            }
        }

        public DataRowView[] NavigationDataRowViews
        {
            get
            {
                return this.navigationDataRowViews;
            }
            set
            {
                if (value != this.navigationDataRowViews)
                {
                    this.navigationDataRowViews = value;
                    this.wfViewState = WfViewState.None;
                }
            }
        }

        public int[] NavigationIndexs
        {
            get
            {
                return this.navigationIndexs;
            }
            set
            {
                this.navigationIndexs = value;
            }
        }

        public SkyMap.Net.DataForms.PrintSet PrintSet
        {
            get
            {
                if (!((this.printSet != null) || string.IsNullOrEmpty(this.printSetId)))
                {
                    this.printSet = WorkflowService.GetPrintSet(this.printSetId);
                }
                if ((this.dataForm != null) && (this.printSet != null))
                {
                    return this.dataForm.FilterTempletPrints(this.printSet);
                }
                return this.printSet;
            }
        }

        public string PrintSetId
        {
            set
            {
                this.printSetId = value;
            }
        }

        public bool RebuildPrint
        {
            get
            {
                return this.reBuildPrint;
            }
            set
            {
                this.reBuildPrint = value;
            }
        }

        public TempletPrint TemplatePrint
        {
            get
            {
                return this.templatePrint;
            }
            set
            {
                if (this.templatePrint != value)
                {
                    this.templatePrint = value;
                    this.wfViewState = WfViewState.None;
                }
            }
        }

        protected override string ToolbarPath
        {
            get
            {
                return "/Workflow/WfView/ToolBar";
            }
        }

        public IWfBox WfBox
        {
            get
            {
                return this.wfBox;
            }
            set
            {
                this.wfBox = value;
            }
        }

        public SkyMap.Net.Workflow.Engine.WorkItem WorkItem
        {
            get
            {
                return this.workItem;
            }
        }
    }
}

