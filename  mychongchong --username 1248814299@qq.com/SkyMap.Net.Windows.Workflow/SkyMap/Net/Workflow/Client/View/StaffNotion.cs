namespace SkyMap.Net.Workflow.Client.View
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Base;
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Gui.Components;
    using SkyMap.Net.Workflow.Instance;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class StaffNotion : SmBarUserControl, IEditView
    {
        private string assignId = string.Empty;
        private IContainer components;
        private UnitOfWork currentUnitOfWork;
        private SmGridControl grid;
        private string proinstId = string.Empty;
        private IList<WfStaffNotion> staffNotions;
        private SmCardView view;

        public StaffNotion()
        {
            this.InitializeComponent();
            if (!((this.Site != null) && this.Site.DesignMode))
            {
                this.InitGrid();
                base.CreateToolbar();
                this.grid.ContextMenuStrip = base.CreateContextMenu(this.ToolbarPath);
            }
        }

        public void AddStaffNotion(WfStaffNotion wsn)
        {
            this.staffNotions.Add(wsn);
            this.view.RefreshData();
            this.view.FocusedRowHandle = this.view.GetRowHandle(wsn);
        }

        public bool CanAddMyNotion()
        {
            if (!StringHelper.IsNull(this.AssignId))
            {
                WfStaffNotion row = null;
                for (int i = 0; i < this.view.RowCount; i++)
                {
                    row = this.view.GetRow(i) as WfStaffNotion;
                    if (row.AssignId == this.assignId)
                    {
                        return false;
                    }
                }
                return true;
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

        public WfStaffNotion GetMyNotion()
        {
            if (this.staffNotions != null)
            {
                foreach (WfStaffNotion notion in this.staffNotions)
                {
                    if (notion.AssignId == this.AssignId)
                    {
                        return notion;
                    }
                }
            }
            return null;
        }

        public List<WfStaffNotion> GetSelectedOrFocusStaffNotions()
        {
            return this.view.GetSelectedOrFocusedRows<WfStaffNotion>();
        }

        private void InitGrid()
        {
            this.grid.BeginUpdate();
            this.view.CardCaptionFormat = "“{2}”的经办意见";
            RepositoryItemMemoEdit edit = new RepositoryItemMemoEdit();
            RepositoryItemTextEdit edit2 = new RepositoryItemTextEdit();
            string[] strArray = new string[] { "Content", "StaffName", "Date" };
            string[] strArray2 = new string[] { "意见", "经办人", "时间" };
            GridColumn[] columns = new GridColumn[3];
            for (int i = 0; i < columns.Length; i++)
            {
                columns[i] = new GridColumn();
                columns[i].FieldName = strArray[i];
                columns[i].Caption = strArray2[i];
                columns[i].VisibleIndex = i;
            }
            this.view.Columns.AddRange(columns);
            columns[0].ColumnEdit = edit;
            columns[1].ColumnEdit = edit2;
            columns[2].ColumnEdit = edit2;
            columns[0].ColumnEdit.ReadOnly = true;
            columns[1].ColumnEdit.ReadOnly = true;
            columns[2].ColumnEdit.ReadOnly = true;
            edit.Enter += new EventHandler(this.memoEdit_Enter);
            this.grid.EndUpdate();
        }

        private void InitializeComponent()
        {
            this.grid = new SmGridControl();
            this.view = new SmCardView();
            this.grid.BeginInit();
            this.view.BeginInit();
            base.SuspendLayout();
            this.grid.Dock = DockStyle.Fill;
            this.grid.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grid.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grid.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grid.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grid.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grid.EmbeddedNavigator.Name = "";
            this.grid.Location = new Point(2, 2);
            this.grid.MainView = this.view;
            this.grid.Name = "grid";
            this.grid.Size = new Size(0x18c, 420);
            this.grid.TabIndex = 0;
            this.grid.UseEmbeddedNavigator = true;
            this.grid.ViewCollection.AddRange(new BaseView[] { this.view });
            this.view.Appearance.FieldCaption.BackColor = Color.GhostWhite;
            this.view.Appearance.FieldCaption.Font = new Font("宋体", 9f, FontStyle.Bold);
            this.view.Appearance.FieldCaption.ForeColor = Color.BlueViolet;
            this.view.Appearance.FieldCaption.Options.UseFont = true;
            this.view.Appearance.FieldCaption.TextOptions.VAlignment = VertAlignment.Center;
            this.view.Appearance.FieldValue.TextOptions.WordWrap = WordWrap.Wrap;
            this.view.CardInterval = 4;
            this.view.FocusedCardTopFieldIndex = 0;
            this.view.GridControl = this.grid;
            this.view.MaximumCardColumns = 1;
            this.view.Name = "view";
            this.view.OptionsBehavior.AutoFocusNewCard = true;
            this.view.OptionsBehavior.AutoHorzWidth = true;
            this.view.OptionsBehavior.FieldAutoHeight = true;
            this.view.FocusedRowChanged += new FocusedRowChangedEventHandler(this.view_FocusedRowChanged);
            this.view.CellValueChanged += new CellValueChangedEventHandler(this.view_CellValueChanged);
            base.Controls.Add(this.grid);
            base.Name = "StaffNotion";
            base.Padding = new Padding(2, 2, 2, 2);
            base.Size = new Size(400, 0x1a8);
            this.grid.EndInit();
            this.view.EndInit();
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
                this.staffNotions = QueryHelper.List<WfStaffNotion>(string.Empty, new string[] { "ProinstId" }, new string[] { this.proinstId });
                this.ResumeBinding();
                this.AssignId = assignId;
                base.BarStatusUpdate();
            }
        }

        private void memoEdit_Enter(object sender, EventArgs e)
        {
            int focusedRowHandle = this.view.FocusedRowHandle;
            if (focusedRowHandle >= 0)
            {
                WfStaffNotion row = this.view.GetRow(focusedRowHandle) as WfStaffNotion;
                MemoEdit edit = sender as MemoEdit;
                edit.Properties.ReadOnly = !StaffNotionCommand.IsCanEditOrDel(row);
                if (!edit.Properties.ReadOnly && (edit.Height < 100))
                {
                    edit.Height = 100;
                    edit.BorderStyle = BorderStyles.Flat;
                }
            }
        }

        public void PostEditor()
        {
            this.view.PostEditor();
        }

        public void RemoveSelectedOrFocusedWSN()
        {
            this.view.DeleteSelectedOrFocusedRows();
        }

        public void ResumeBinding()
        {
            if (this.grid.DataSource == null)
            {
                if (this.staffNotions.Count == 0)
                {
                    this.staffNotions = new List<WfStaffNotion>(1);
                    this.staffNotions.Add(new WfStaffNotion());
                    this.grid.DataSource = this.staffNotions;
                    this.staffNotions.Clear();
                    this.view.RefreshData();
                }
                else
                {
                    this.grid.DataSource = this.staffNotions;
                }
            }
            else
            {
                this.view.RefreshData();
            }
        }

        void IEditView.BarStatusUpdate()
        {
            base.BarStatusUpdate();
        }

        private void view_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            WfStaffNotion row = this.view.GetRow(e.RowHandle) as WfStaffNotion;
            this.CurrentUnitOfWork.RegisterDirty(row);
        }

        private void view_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            base.BarStatusUpdate();
        }

        public string AssignId
        {
            get
            {
                return this.assignId;
            }
            private set
            {
                this.assignId = value;
                WfStaffNotion row = null;
                for (int i = 0; i < this.view.RowCount; i++)
                {
                    row = this.view.GetRow(i) as WfStaffNotion;
                    if (row.AssignId == this.assignId)
                    {
                        this.view.FocusedRowHandle = i;
                    }
                }
            }
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
                return "/Workflow/StaffNotion/Toolbar";
            }
        }
    }
}

