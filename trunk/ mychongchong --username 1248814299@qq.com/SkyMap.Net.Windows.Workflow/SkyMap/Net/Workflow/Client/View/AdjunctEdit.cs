namespace SkyMap.Net.Workflow.Client.View
{
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Base;
    using DevExpress.XtraGrid.Views.Grid;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Gui.Components;
    using SkyMap.Net.Workflow.Instance;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.IO;
    using System.Windows.Forms;

    public class AdjunctEdit : SmBarUserControl, IEditView
    {
        private SkyMap.Net.Workflow.Client.View.AdjunctEditCommand adjunctEditCommand;
        private IContainer components;
        internal IList<WfAdjunct> currentAdjuncts;
        private UnitOfWork currentUnitOfWork;
        private SmGridControl grid;
        private string proinstId;
        private SmGridView view;

        public AdjunctEdit()
        {
            this.InitializeComponent();
            if (!((this.Site != null) && this.Site.DesignMode))
            {
                this.InitGrid();
                base.CreateToolbar();
                this.grid.ContextMenuStrip = base.CreateContextMenu(this.ToolbarPath);
            }
        }

        public void AddAdjunct(WfAdjunct adj)
        {
            this.currentAdjuncts.Add(adj);
            this.view.RefreshData();
            this.view.FocusedRowHandle = this.view.GetRowHandle(adj);
        }

        private void BtEditClick(object sender, ButtonPressedEventArgs e)
        {
            WfAdjunct focusAdjunct = this.GetFocusAdjunct();
            if ((focusAdjunct != null) && this.AdjunctEditCommand.IsHaveEditAdjuctPermission(focusAdjunct))
            {
                this.view.BeginUpdate();
                try
                {
                    if (this.AdjunctEditCommand.SelectFileToAdjuct(focusAdjunct))
                    {
                        this.CurrentUnitOfWork.RegisterDirty(focusAdjunct);
                        this.CurrentUnitOfWork.RegisterDirty(focusAdjunct.File);
                    }
                }
                finally
                {
                    this.view.EndUpdate();
                }
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

        private void FileChanged(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine(e.Name);
        }

        public WfAdjunct GetAdjunct(int index)
        {
            return (this.view.GetRow(index) as WfAdjunct);
        }

        public WfAdjunct GetFocusAdjunct()
        {
            if (this.view.FocusedRowHandle > -1)
            {
                return this.GetAdjunct(this.view.FocusedRowHandle);
            }
            return null;
        }

        public List<WfAdjunct> GetSelectedAdjuncts()
        {
            return this.view.GetSelectedOrFocusedRows<WfAdjunct>();
        }

        private void InitAdjuncts()
        {
            this.currentAdjuncts = QueryHelper.List<WfAdjunct>(string.Empty, new string[] { "ProinstId" }, new string[] { this.proinstId });
            this.ResumeBinding();
        }

        private void InitGrid()
        {
            this.view.BeginUpdate();
            this.view.OptionsView.ShowGroupPanel = false;
            this.view.OptionsView.ShowHorzLines = true;
            this.view.OptionsView.ShowVertLines = true;
            this.view.OptionsSelection.MultiSelect = true;
            RepositoryItemButtonEdit edit = new RepositoryItemButtonEdit();
            RepositoryItemTextEdit edit2 = new RepositoryItemTextEdit();
            RepositoryItemMemoExEdit edit3 = new RepositoryItemMemoExEdit();
            string[] strArray = new string[] { "Name", "CreateStaffName", "CreateDate", "Description" };
            string[] strArray2 = new string[] { "附件名称", "创建人", "创建时间", "备注" };
            GridColumn[] columns = new GridColumn[4];
            for (int i = 0; i < 4; i++)
            {
                columns[i] = new GridColumn();
                columns[i].FieldName = strArray[i];
                columns[i].Caption = strArray2[i];
                columns[i].VisibleIndex = i;
            }
            this.view.Columns.AddRange(columns);
            columns[0].ColumnEdit = edit;
            columns[3].ColumnEdit = edit3;
            columns[1].ColumnEdit = edit2;
            columns[2].ColumnEdit = edit2;
            columns[1].ColumnEdit.ReadOnly = true;
            columns[2].ColumnEdit.ReadOnly = true;
            edit.ButtonClick += new ButtonPressedEventHandler(this.BtEditClick);
            edit.EditValueChanged += new EventHandler(this.ValueChanged);
            edit3.EditValueChanged += new EventHandler(this.ValueChanged);
            edit2.EditValueChanged += new EventHandler(this.ValueChanged);
            this.view.EndUpdate();
        }

        private void InitializeComponent()
        {
            this.grid = new SmGridControl();
            this.view = new SmGridView();
            this.grid.BeginInit();
            this.view.BeginInit();
            base.SuspendLayout();
            this.grid.Dock = DockStyle.Fill;
            this.grid.EmbeddedNavigator.Name = "";
            this.grid.ImeMode = ImeMode.On;
            this.grid.Location = new Point(2, 2);
            this.grid.MainView = this.view;
            this.grid.Name = "grid";
            this.grid.Size = new Size(0x18c, 340);
            this.grid.TabIndex = 0;
            this.grid.ViewCollection.AddRange(new BaseView[] { this.view });
            this.view.Appearance.EvenRow.BackColor = Color.LightSkyBlue;
            this.view.Appearance.EvenRow.BackColor2 = Color.GhostWhite;
            this.view.Appearance.EvenRow.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.view.Appearance.EvenRow.Options.UseBackColor = true;
            this.view.Appearance.EvenRow.Options.UseFont = true;
            this.view.Appearance.OddRow.BackColor = Color.NavajoWhite;
            this.view.Appearance.OddRow.BackColor2 = Color.White;
            this.view.Appearance.OddRow.GradientMode = LinearGradientMode.BackwardDiagonal;
            this.view.Appearance.OddRow.Options.UseBackColor = true;
            this.view.Appearance.OddRow.Options.UseFont = true;
            this.view.GridControl = this.grid;
            this.view.HorzScrollVisibility = ScrollVisibility.Always;
            this.view.Name = "view";
            this.view.OptionsMenu.EnableColumnMenu = false;
            this.view.OptionsMenu.EnableFooterMenu = false;
            this.view.OptionsMenu.EnableGroupPanelMenu = false;
            this.view.OptionsView.EnableAppearanceEvenRow = true;
            this.view.OptionsView.EnableAppearanceOddRow = true;
            this.view.PaintStyleName = "Skin";
            this.view.VertScrollVisibility = ScrollVisibility.Always;
            this.view.FocusedRowChanged += new FocusedRowChangedEventHandler(this.view_FocusedRowChanged);
            this.view.DoubleClick += new EventHandler(this.view_DoubleClick);
            this.view.CustomRowCellEdit += new CustomRowCellEditEventHandler(this.view_CustomRowCellEdit);
            base.Controls.Add(this.grid);
            base.Name = "AdjunctEdit";
            base.Padding = new Padding(2);
            base.Size = new Size(400, 0x158);
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
                this.InitAdjuncts();
            }
        }

        public void PostEditor()
        {
            this.view.PostEditor();
        }

        public void RemoveAdjunct(WfAdjunct adj)
        {
            this.currentAdjuncts.Remove(adj);
            this.view.RefreshData();
        }

        private void ResumeBinding()
        {
            if (this.grid.DataSource == null)
            {
                if (this.currentAdjuncts.Count == 0)
                {
                    this.currentAdjuncts = new List<WfAdjunct>(1);
                    this.currentAdjuncts.Add(new WfAdjunct());
                    this.grid.DataSource = this.currentAdjuncts;
                    this.currentAdjuncts.Clear();
                    this.view.RefreshData();
                }
                else
                {
                    this.grid.DataSource = this.currentAdjuncts;
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

        private void ValueChanged(object sender, EventArgs e)
        {
            if (this.view.FocusedRowHandle > -1)
            {
                WfAdjunct focusAdjunct = this.GetFocusAdjunct();
                this.CurrentUnitOfWork.RegisterDirty(focusAdjunct);
            }
        }

        private void view_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            WfAdjunct adj = this.GetAdjunct(e.RowHandle);
            if (((adj != null) && (e.Column.FieldName == "Name")) || (e.Column.FieldName == "Description"))
            {
                e.RepositoryItem.ReadOnly = !this.AdjunctEditCommand.IsHaveEditAdjuctPermission(adj);
            }
        }

        private void view_DoubleClick(object sender, EventArgs e)
        {
            this.AdjunctEditCommand.OpenAdjunct();
        }

        private void view_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            base.BarStatusUpdate();
        }

        private SkyMap.Net.Workflow.Client.View.AdjunctEditCommand AdjunctEditCommand
        {
            get
            {
                if (this.adjunctEditCommand == null)
                {
                    this.adjunctEditCommand = new SkyMap.Net.Workflow.Client.View.AdjunctEditCommand();
                    this.adjunctEditCommand.Owner = this;
                }
                return this.adjunctEditCommand;
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
                return "/Workflow/AdjunctEdit/Toolbar";
            }
        }
    }
}

