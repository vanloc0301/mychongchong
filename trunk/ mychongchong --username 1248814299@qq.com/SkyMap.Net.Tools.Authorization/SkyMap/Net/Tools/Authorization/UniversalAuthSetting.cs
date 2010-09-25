namespace SkyMap.Net.Tools.Authorization
{
    using DevExpress.Data;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraGrid;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Base;
    using DevExpress.XtraGrid.Views.Grid;
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Gui.Components;
    using SkyMap.Net.OGM;
    using SkyMap.Net.Security;
    using SkyMap.Net.Tools.Organize;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;

    public class UniversalAuthSetting : SmUserControl, IAuthSetting
    {
        private CAuthType authType;
        private ToolBarButton btDel;
        private ToolBarButton btNew;
        private IContainer components;
        private UnitOfWork currentUnitOfWork;
        private GridControl grid;
        private ImageList imageList1;
        private Panel panel1;
        private ToolBar toolBar;
        private GridView view;

        public UniversalAuthSetting()
        {
            this.InitializeComponent();
            this.InitGrid();
        }

        private void AddNewUniversalAuth()
        {
            CUniversalAuth item = new CUniversalAuth();
            item.Type = this.authType;
            this.authType.UniversalAuths.Add(item);
            this.CurrentUnitOfWork.RegisterNew(item);
        }

        private void DelClick()
        {
            int[] selectedRows = this.view.GetSelectedRows();
            for (int i = selectedRows.Length - 1; i >= 0; i--)
            {
                CUniversalAuth row = this.view.GetRow(selectedRows[i]) as CUniversalAuth;
                this.CurrentUnitOfWork.RegisterRemoved(row);
                this.view.DeleteRow(selectedRows[i]);
            }
            this.btDel.Pushed = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitGrid()
        {
            this.view.BeginUpdate();
            this.view.OptionsView.ShowHorzLines = true;
            this.view.OptionsView.ShowVertLines = true;
            this.view.OptionsSelection.MultiSelect = true;
            string[] strArray = new string[] { "ParticipantName", "ParticipantType", "ResourceName", "Description" };
            string[] strArray2 = new string[] { "参与者名称", "参与者类型", "资源名称", "备注" };
            GridColumn[] columns = new GridColumn[strArray.Length];
            for (int i = 0; i < columns.Length; i++)
            {
                columns[i] = new GridColumn();
                columns[i].FieldName = strArray[i];
                columns[i].Caption = strArray2[i];
                columns[i].VisibleIndex = i;
            }
            this.view.Columns.AddRange(columns);
            RepositoryItemButtonEdit edit = new RepositoryItemButtonEdit();
            RepositoryItemTextEdit edit2 = new RepositoryItemTextEdit();
            RepositoryItemButtonEdit edit3 = new RepositoryItemButtonEdit();
            edit.ReadOnly = true;
            edit2.ReadOnly = true;
            edit3.ReadOnly = true;
            edit.ButtonClick += new ButtonPressedEventHandler(this.ParticipantButtonClick);
            edit3.ButtonClick += new ButtonPressedEventHandler(this.ResouceButtonClick);
            columns[0].ColumnEdit = edit;
            columns[1].ColumnEdit = edit2;
            columns[2].ColumnEdit = edit3;
            this.view.EndUpdate();
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ResourceManager manager = new ResourceManager(typeof(UniversalAuthSetting));
            this.grid = new GridControl();
            this.view = new GridView();
            this.imageList1 = new ImageList(this.components);
            this.panel1 = new Panel();
            this.toolBar = new ToolBar();
            this.btNew = new ToolBarButton();
            this.btDel = new ToolBarButton();
            this.grid.BeginInit();
            this.view.BeginInit();
            this.panel1.SuspendLayout();
            base.SuspendLayout();
            this.grid.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.grid.EmbeddedNavigator.Name = "";
            this.grid.Location = new Point(5, 0x20);
            this.grid.MainView = this.view;
            this.grid.Name = "grid";
            this.grid.Size = new Size(0x13e, 0x110);
            this.grid.TabIndex = 0;
            this.grid.Text = "gridControl1";
            this.view.GridControl = this.grid;
            this.view.Name = "view";
            this.view.OptionsView.ShowGroupedColumns = true;
            this.view.SelectionChanged += new SelectionChangedEventHandler(this.view_SelectionChanged);
            this.view.CellValueChanged += new CellValueChangedEventHandler(this.view_CellValueChanged);
            this.imageList1.ColorDepth = ColorDepth.Depth32Bit;
            this.imageList1.ImageSize = new Size(0x10, 0x10);
            this.imageList1.ImageStream = (ImageListStreamer) manager.GetObject("imageList1.ImageStream");
            this.imageList1.TransparentColor = Color.Fuchsia;
            this.panel1.Controls.Add(this.toolBar);
            this.panel1.Dock = DockStyle.Top;
            this.panel1.Location = new Point(5, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x13e, 0x20);
            this.panel1.TabIndex = 12;
            this.toolBar.Appearance = ToolBarAppearance.Flat;
            this.toolBar.Buttons.AddRange(new ToolBarButton[] { this.btNew, this.btDel });
            this.toolBar.DropDownArrows = true;
            this.toolBar.ImageList = this.imageList1;
            this.toolBar.Location = new Point(0, 0);
            this.toolBar.Name = "toolBar";
            this.toolBar.ShowToolTips = true;
            this.toolBar.Size = new Size(0x13e, 0x1c);
            this.toolBar.TabIndex = 0;
            this.toolBar.TextAlign = ToolBarTextAlign.Right;
            this.toolBar.ButtonClick += new ToolBarButtonClickEventHandler(this.toolBar_ButtonClick);
            this.btNew.ImageIndex = 0;
            this.btNew.Text = "新增";
            this.btDel.ImageIndex = 1;
            this.btDel.Pushed = true;
            this.btDel.Text = "删除";
            base.Controls.Add(this.panel1);
            base.Controls.Add(this.grid);
            base.DockPadding.Bottom = 5;
            base.DockPadding.Left = 5;
            base.DockPadding.Right = 5;
            base.Name = "UniversalAuthSetting";
            base.Size = new Size(0x148, 0x138);
            this.grid.EndInit();
            this.view.EndInit();
            this.panel1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void NewClick()
        {
            this.AddNewUniversalAuth();
            this.ResumeBinding();
            this.view.SelectRow(this.view.RowCount - 1);
        }

        private void ParticipantButtonClick(object sender, ButtonPressedEventArgs e)
        {
            int focusedRowHandle = this.view.FocusedRowHandle;
            if (focusedRowHandle >= 0)
            {
                CUniversalAuth row = this.view.GetRow(focusedRowHandle) as CUniversalAuth;
                fSelectParticipant participant = new fSelectParticipant();
                if (!StringHelper.IsNull(row.ParticipantId))
                {
                    participant.ParticipantID = row.ParticipantId;
                }
                participant.ShowDialog();
                if (!StringHelper.IsNull(participant.ParticipantID))
                {
                    this.view.BeginUpdate();
                    row.ParticipantId = participant.ParticipantID;
                    row.ParticipantName = participant.ParticipantName;
                    row.ParticipantType = participant.ParticipantType;
                    row.ParticipantIdValue = participant.ParticipantIdValue;
                    this.view.EndUpdate();
                    this.CurrentUnitOfWork.RegisterDirty(row);
                }
                participant.Close();
            }
        }

        public void PostEditor()
        {
            this.view.PostEditor();
        }

        private void ResouceButtonClick(object sender, ButtonPressedEventArgs e)
        {
            int focusedRowHandle = this.view.FocusedRowHandle;
            if (focusedRowHandle >= 0)
            {
                CUniversalAuth row = this.view.GetRow(focusedRowHandle) as CUniversalAuth;
                SkyMap.Net.Tools.Authorization.ResourceSelect select = new SkyMap.Net.Tools.Authorization.ResourceSelect();
                select.AuthType = this.authType;
                if (select.ShowDialog() == DialogResult.OK)
                {
                    this.view.BeginUpdate();
                    row.ResourceId = select.ResourceId;
                    row.ResourceName = select.ResourceName;
                    this.view.EndUpdate();
                    this.CurrentUnitOfWork.RegisterDirty(row);
                }
                select.Close();
            }
        }

        private void ResumeBinding()
        {
            this.grid.DataSource = null;
            this.grid.DataSource = this.authType.UniversalAuths;
        }

        private void toolBar_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            ToolBarButton button = e.Button;
            if (!button.Pushed)
            {
                if (button.Equals(this.btNew))
                {
                    this.NewClick();
                }
                else if (button.Equals(this.btDel))
                {
                    this.DelClick();
                }
            }
        }

        private void view_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            CUniversalAuth row = this.view.GetRow(e.RowHandle) as CUniversalAuth;
            this.CurrentUnitOfWork.RegisterDirty(row);
        }

        private void view_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.btDel.Pushed = this.view.SelectedRowsCount == 0;
        }

        public CAuthType AuthType
        {
            get
            {
                return this.authType;
            }
            set
            {
                this.authType = value;
                this.grid.DataSource = this.authType.UniversalAuths;
            }
        }

        public UnitOfWork CurrentUnitOfWork
        {
            get
            {
                if (this.currentUnitOfWork == null)
                {
                    throw new NullReferenceException("Current unit of work cannot be null");
                }
                return this.currentUnitOfWork;
            }
            set
            {
                this.currentUnitOfWork = value;
            }
        }
    }
}

