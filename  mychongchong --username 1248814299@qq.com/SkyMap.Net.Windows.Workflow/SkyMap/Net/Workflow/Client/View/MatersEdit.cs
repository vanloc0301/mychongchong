namespace SkyMap.Net.Workflow.Client.View
{
    using DevExpress.Data;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Base;
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Gui.Components;
    using SkyMap.Net.Workflow.Client.Services;
    using SkyMap.Net.Workflow.Instance;
    using SkyMap.Net.Workflow.XPDL;
    using SkyMap.Net.Workflow.XPDL.ExtendElement;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class MatersEdit : SmBarUserControl, IEditView
    {
        private IContainer components;
        private const string contextMenuPath = "/Workflow/MaterEdit/ContextMenu";
        private IList<WfProinstMater> currentMaters;
        private UnitOfWork currentUnitOfWork;
        private SmGridControl grid;
        private SmGridView mainView;
        private string prodefId;
        public EventHandler ProdefIDChange;
        private string proinstId;

        public MatersEdit()
        {
            this.InitializeComponent();
            if (!((this.Site != null) && this.Site.DesignMode))
            {
                this.InitGrid();
                base.CreateToolbar();
                this.grid.ContextMenuStrip = base.CreateContextMenu("/Workflow/MaterEdit/ContextMenu");
            }
        }

        public void AddMater(WfProinstMater mater)
        {
            if (mater.DisplayOrder != this.currentMaters.Count)
            {
                mater.DisplayOrder = this.currentMaters.Count;
            }
            this.currentMaters.Add(mater);
            this.mainView.RefreshData();
            this.mainView.FocusedRowHandle = this.mainView.GetRowHandle(mater);
        }

        internal bool CurrentMatersLocate(WfAppendix appendix)
        {
            foreach (WfProinstMater mater in this.currentMaters)
            {
                if (mater.Name.ToString().Trim() == appendix.Name.ToString().Trim())
                {
                    return true;
                }
            }
            return false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public List<WfAppendix> GetSelectedOrFocusAppendixs()
        {
            throw new NotImplementedException();
        }

        public List<WfProinstMater> GetSelectedOrFocusMaters()
        {
            return this.mainView.GetSelectedOrFocusedRows<WfProinstMater>();
        }

        private void InitGrid()
        {
            this.mainView.BeginUpdate();
            this.mainView.OptionsView.ShowGroupPanel = false;
            this.mainView.OptionsView.ShowHorzLines = true;
            this.mainView.OptionsView.ShowVertLines = true;
            this.mainView.OptionsSelection.MultiSelect = true;
            RepositoryItemTextEdit edit = new RepositoryItemTextEdit();
            RepositoryItemSpinEdit edit2 = new RepositoryItemSpinEdit();
            RepositoryItemCheckEdit edit3 = new RepositoryItemCheckEdit();
            this.mainView.CellValueChanged += new CellValueChangedEventHandler(this.MaterCellValueChanged);
            string[] strArray = new string[] { "Selected", "Name", "OldNum", "DupliNum", "Description" };
            string[] strArray2 = new string[] { "", "资料名称", "原件份数", "复印件份数", "备注" };
            GridColumn[] columns = new GridColumn[5];
            GridColumn[] columnArray2 = new GridColumn[4];
            for (int i = 0; i < 5; i++)
            {
                columns[i] = new GridColumn();
                columns[i].FieldName = strArray[i];
                columns[i].Caption = strArray2[i];
                columns[i].VisibleIndex = i;
            }
            this.mainView.Columns.AddRange(columns);
            columns[0].ColumnEdit = edit3;
            columns[1].ColumnEdit = edit;
            columns[4].ColumnEdit = edit;
            columns[2].ColumnEdit = edit2;
            columns[3].ColumnEdit = edit2;
            this.mainView.EndUpdate();
        }

        private void InitializeComponent()
        {
            this.grid = new SmGridControl();
            this.mainView = new SmGridView();
            this.grid.BeginInit();
            this.mainView.BeginInit();
            base.SuspendLayout();
            this.grid.Dock = DockStyle.Fill;
            this.grid.EmbeddedNavigator.Name = "";
            this.grid.Location = new Point(2, 2);
            this.grid.MainView = this.mainView;
            this.grid.Name = "grid";
            this.grid.Size = new Size(0x380, 0x1b4);
            this.grid.TabIndex = 0;
            this.grid.ViewCollection.AddRange(new BaseView[] { this.mainView });
            this.mainView.Appearance.EvenRow.BackColor = Color.LightSkyBlue;
            this.mainView.Appearance.EvenRow.BackColor2 = Color.GhostWhite;
            this.mainView.Appearance.EvenRow.Font = new Font("SimSun", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.mainView.Appearance.EvenRow.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.mainView.Appearance.EvenRow.Options.UseBackColor = true;
            this.mainView.Appearance.EvenRow.Options.UseFont = true;
            this.mainView.Appearance.OddRow.BackColor = Color.NavajoWhite;
            this.mainView.Appearance.OddRow.BackColor2 = Color.White;
            this.mainView.Appearance.OddRow.Font = new Font("SimSun", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.mainView.Appearance.OddRow.GradientMode = LinearGradientMode.BackwardDiagonal;
            this.mainView.Appearance.OddRow.Options.UseBackColor = true;
            this.mainView.Appearance.OddRow.Options.UseFont = true;
            this.mainView.GridControl = this.grid;
            this.mainView.HorzScrollVisibility = ScrollVisibility.Always;
            this.mainView.Name = "mainView";
            this.mainView.OptionsMenu.EnableColumnMenu = false;
            this.mainView.OptionsMenu.EnableFooterMenu = false;
            this.mainView.OptionsMenu.EnableGroupPanelMenu = false;
            this.mainView.OptionsSelection.MultiSelect = true;
            this.mainView.OptionsView.EnableAppearanceEvenRow = true;
            this.mainView.OptionsView.EnableAppearanceOddRow = true;
            this.mainView.PaintStyleName = "Skin";
            this.mainView.VertScrollVisibility = ScrollVisibility.Always;
            this.mainView.SelectionChanged += new SelectionChangedEventHandler(this.mainView_SelectionChanged);
            base.Controls.Add(this.grid);
            base.Name = "MatersEdit";
            base.Padding = new Padding(2);
            base.Size = new Size(900, 440);
            this.grid.EndInit();
            this.mainView.EndInit();
            base.ResumeLayout(false);
        }

        public void LoadData(UnitOfWork currentUnitOfWork, string proinstID, string prodefID, string assignId, string[] projectSubTypes)
        {
            if (this.currentUnitOfWork != currentUnitOfWork)
            {
                this.currentUnitOfWork = currentUnitOfWork;
            }
            if (this.proinstId != proinstID)
            {
                this.grid.DataSource = null;
                this.proinstId = proinstID;
                this.currentMaters = QueryHelper.List<WfProinstMater>(string.Empty, new string[] { "ProinstId" }, new string[] { this.proinstId }, new string[] { "DisplayOrder" });
                this.ResumeBinding();
            }
            if (this.prodefId != prodefID)
            {
                this.prodefId = prodefID;
                if (this.ProdefIDChange != null)
                {
                    this.ProdefIDChange(this, null);
                }
            }
            if (((projectSubTypes != null) && (projectSubTypes.Length > 0)) && ((this.currentMaters == null) || (this.currentMaters.Count == 0)))
            {
                Prodef prodef = WorkflowService.Prodefs[this.ProdefId];
                if (prodef != null)
                {
                    if (string.IsNullOrEmpty(prodef.TempletAppendixsId))
                    {
                        LoggingService.WarnFormatted("没有配置流程ID：{0} 名称：{1} 的收件资料模块集合.", new object[] { prodef.Id, prodef.Name });
                    }
                    else
                    {
                        WfTempletAppendixs appendixs = QueryHelper.Get<WfTempletAppendixs>("WfTempletAppendixs_" + prodef.TempletAppendixsId, prodef.TempletAppendixsId);
                        if (appendixs != null)
                        {
                            int length = projectSubTypes.Length;
                            int num2 = 0;
                            foreach (WfTempletAppendix appendix in appendixs.TempletAppendixs)
                            {
                                foreach (string str in projectSubTypes)
                                {
                                    if (appendix.Name == str)
                                    {
                                        if (LoggingService.IsDebugEnabled)
                                        {
                                            LoggingService.DebugFormatted("将自动添加类型：{0} 的收件资料", new object[] { str });
                                        }
                                        num2++;
                                        for (int i = 0; i < appendix.WfAppendixs.Count; i++)
                                        {
                                            WfAppendix appendix2 = appendix.WfAppendixs[i];
                                            if (!this.CurrentMatersLocate(appendix2))
                                            {
                                                WfProinstMater mater = AddMaterFromTempletCommand.AddMaterFormTemplet(appendix2, this.ProinstId);
                                                this.CurrentUnitOfWork.RegisterNew(mater);
                                                this.AddMater(mater);
                                            }
                                        }
                                        if (num2 == length)
                                        {
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void mainView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            base.BarStatusUpdate();
        }

        private void MaterCellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                WfProinstMater row = this.mainView.GetRow(e.RowHandle) as WfProinstMater;
                this.CurrentUnitOfWork.RegisterDirty(row);
            }
        }

        public void PostEditor()
        {
            this.mainView.PostEditor();
        }

        public void RemoveMaters(List<WfProinstMater> maters)
        {
            foreach (WfProinstMater mater in maters)
            {
                this.currentMaters.Remove(mater);
            }
            this.mainView.RefreshData();
        }

        private void ResumeBinding()
        {
            if (this.grid.DataSource == null)
            {
                if (this.currentMaters.Count == 0)
                {
                    this.currentMaters = new List<WfProinstMater>(1);
                    this.currentMaters.Add(new WfProinstMater());
                    this.grid.DataSource = this.currentMaters;
                    this.currentMaters.Clear();
                    this.mainView.RefreshData();
                }
                else
                {
                    this.grid.DataSource = this.currentMaters;
                }
            }
            else
            {
                this.mainView.RefreshData();
            }
        }

        void IEditView.BarStatusUpdate()
        {
            base.BarStatusUpdate();
        }

        public UnitOfWork CurrentUnitOfWork
        {
            get
            {
                if (this.currentUnitOfWork == null)
                {
                    throw new NullReferenceException("UnitOfWork cannot be null");
                }
                return this.currentUnitOfWork;
            }
        }

        public string ProdefId
        {
            get
            {
                return this.prodefId;
            }
        }

        public string ProinstId
        {
            get
            {
                return this.proinstId;
            }
        }

        protected override string ToolbarPath
        {
            get
            {
                return "/Workflow/MatersEdit/Toolbar";
            }
        }
    }
}

