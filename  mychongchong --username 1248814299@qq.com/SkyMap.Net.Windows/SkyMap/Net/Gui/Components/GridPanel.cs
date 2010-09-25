namespace SkyMap.Net.Gui.Components
{
    using DevExpress.Data;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Grid;
    using SkyMap.Net.Components;
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using DevExpress.XtraGrid.Views.Base;

    public class GridPanel : UserControl
    {
        private ContextMenuStrip contextMenuStrip;
        private SmGridControl grid;
        private bool isUseCheckBox;
        private int pageIndex;
        public Navigate PageNavigation;
        private Dictionary<string, object> properties;
        private List<ToolStrip> toolStrips;
        private SmGridView view;

        public GridPanel(string toolbarPath)
        {
            this.InitializeComponent();
            this.InitGrid();
            this.CreateToolbars(toolbarPath);
            this.CreateContextMenu(null);
        }

        public GridPanel(GridAttribute ga, object datasource)
        {
            this.InitializeComponent();
            try
            {
                base.ImeMode = ImeMode.OnHalf;
            }
            catch
            {
            }
            this.InitGrid();
            this.grid.BeginUpdate();
            this.view.Columns.Clear();
            this.isUseCheckBox = ga.IsUseCheckBox;
            for (int i = 0; i < ga.Fields.Length; i++)
            {
                GridColumn column = new GridColumn();
                column.FieldName = ga.Fields[i];
                column.Caption = ga.Captions[i];
                column.VisibleIndex = i;
                if ((i == 0) && ga.IsUseCheckBox)
                {
                    column.OptionsColumn.AllowEdit = true;
                }
                else
                {
                    column.OptionsColumn.AllowEdit = ga.Editable;
                }
                this.view.Columns.Add(column);
            }
            this.SetGroupFields(ga.GroupFields);
            this.SetFooter(ga.SumFields, ga.CountField);
            if (ga.PageNavigation != null)
            {
                this.PageNavigation = ga.PageNavigation;
            }
            this.grid.DataSource = datasource;
            if (this.view.RowCount > 0)
            {
                this.view.ExpandGroupRow(0);
            }
            this.grid.EndUpdate();
            if (!string.IsNullOrEmpty(ga.ToolbarPath))
            {
                this.CreateToolbars(ga.ToolbarPath);
            }
            this.CreateContextMenu(ga.MenuPath);
        }

        private GridColumn CheckColumn()
        {
            GridColumn column = null;
            if (this.IsUseCheckBox)
            {
                column = this.view.Columns[0];
            }
            return column;
        }

        public void ClearSelection()
        {
            if (!this.IsUseCheckBox)
            {
                this.view.ClearSelection();
            }
            else
            {
                GridColumn column = this.CheckColumn();
                for (int i = 0; i < this.view.DataRowCount; i++)
                {
                    if (!(this.view.IsGroupRow(i) && this.view.IsMasterRow(i)))
                    {
                        this.view.SetRowCellValue(i, column, false);
                    }
                }
            }
        }

        protected virtual void CreateContextMenu(string contextMenuPath)
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.Debug("将创建网格控件的上下文菜单!");
            }
            this.contextMenuStrip = MenuService.CreateContextMenu(this, "/GridPanel/ContextMenu/CommonItems");
            if (!string.IsNullOrEmpty(contextMenuPath))
            {
                try
                {
                    MenuService.AddItemsToMenu(this.contextMenuStrip.Items, this, contextMenuPath);
                }
                catch
                {
                }
            }
            this.grid.ContextMenuStrip = this.contextMenuStrip;
        }

        protected virtual void CreateToolbars(string toolbarPath)
        {
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("将创建网格控件的工具栏:{0}", new object[] { toolbarPath });
            }
            if (!string.IsNullOrEmpty(toolbarPath))
            {
                try
                {
                    this.toolStrips = new List<ToolStrip>(ToolbarService.CreateToolbars(this, toolbarPath));
                }
                catch (Exception exception)
                {
                    LoggingService.Error(exception);
                }
            }
            if (this.PageNavigation != null)
            {
                if (this.toolStrips == null)
                {
                    this.toolStrips = new List<ToolStrip>();
                }
                this.toolStrips.Add(ToolbarService.CreateToolStrip(this, AddInTree.GetTreeNode("/GridPanel/Toolbar/PageNavigation")));
            }
            for (int i = this.toolStrips.Count - 1; i > -1; i--)
            {
                ToolStrip strip = this.toolStrips[i];
                strip.ShowItemToolTips = true;
                strip.Dock = DockStyle.Top;
                strip.Stretch = true;
                base.Controls.Add(strip);
            }
        }

        public string ExportToExcel()
        {
            string filePath = Path.GetTempFileName() + ".xls";
            this.view.ExportToXls(filePath);
            return filePath;
        }

        public object GetFocusRow()
        {
            if (!(this.isUseCheckBox || (this.view.FocusedRowHandle < 0)))
            {
                return this.view.GetRow(this.view.FocusedRowHandle);
            }
            if (this.isUseCheckBox)
            {
                throw new ApplicationException("在使用CheckBox的Grid中不能调用此方法");
            }
            return null;
        }

        public List<T> GetSelectedObjects<T>()
        {
            if (!this.isUseCheckBox)
            {
                return this.view.GetSelectedOrFocusedRows<T>();
            }
            this.view.PostEditor();
            GridColumn column = this.CheckColumn();
            List<T> list = new List<T>();
            for (int i = 0; i < this.view.DataRowCount; i++)
            {
                if (!this.view.IsGroupRow(i) || !this.view.IsMasterRow(i))
                {
                    bool flag2 = true;
                    if (flag2.Equals(this.view.GetRowCellValue(i, column)))
                    {
                        list.Add((T) this.view.GetRow(i));
                    }
                }
            }
            return list;
        }

        private void InitGrid()
        {
            this.grid.BeginUpdate();
            this.grid.ShowOnlyPredefinedDetails = true;
            this.view.OptionsSelection.MultiSelect = true;
            this.grid.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grid.LookAndFeel.UseWindowsXPTheme = false;
            this.grid.LookAndFeel.SetSkinStyle(LookAndFeelHelper.SkinStyle);
            this.grid.EndUpdate();
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
            this.grid.Location = new Point(2, 2);
            this.grid.MainView = this.view;
            this.grid.Name = "grid";
            this.grid.Size = new Size(0x1e1, 0x16d);
            this.grid.TabIndex = 2;
            this.grid.ViewCollection.AddRange(new BaseView[] { this.view });
            this.view.GridControl = this.grid;
            this.view.Name = "view";
            this.view.OptionsMenu.EnableColumnMenu = false;
            this.view.OptionsMenu.EnableFooterMenu = false;
            this.view.OptionsMenu.EnableGroupPanelMenu = false;
            this.view.OptionsView.EnableAppearanceEvenRow = true;
            this.view.OptionsView.EnableAppearanceOddRow = true;
            base.ClientSize = new Size(0x1e5, 0x171);
            base.Controls.Add(this.grid);
            base.Name = "GridPanel";
            base.Padding = new Padding(2);
            this.grid.EndInit();
            this.view.EndInit();
            base.ResumeLayout(false);
        }

        public void InvertSelection()
        {
            int num;
            if (!this.IsUseCheckBox)
            {
                for (num = 0; num < this.view.DataRowCount; num++)
                {
                    if (!(this.view.IsGroupRow(num) && this.view.IsMasterRow(num)))
                    {
                        this.view.InvertRowSelection(num);
                    }
                }
            }
            else
            {
                GridColumn column = this.CheckColumn();
                for (num = 0; num < this.view.DataRowCount; num++)
                {
                    if (!(this.view.IsGroupRow(num) && this.view.IsMasterRow(num)))
                    {
                        this.view.SetRowCellValue(num, column, !((bool) this.view.GetRowCellValue(num, column)));
                    }
                }
            }
        }

        public void PopulateColumns(object datasource)
        {
            this.PopulateColumns(datasource, null, null, null);
        }

        public void PopulateColumns(object datasource, string countField, string[] sumFields, string[] groupFields)
        {
            this.view.Columns.Clear();
            this.view.PopulateColumns(datasource);
            this.SetFooter(sumFields, countField);
            this.SetGroupFields(groupFields);
            this.grid.DataSource = datasource;
            this.view.BestFitColumns();
        }

        public void RemoveFocusRow()
        {
            if (!(this.isUseCheckBox || (this.view.FocusedRowHandle < 0)))
            {
                this.view.DeleteRow(this.view.FocusedRowHandle);
            }
            else if (this.isUseCheckBox)
            {
                throw new ApplicationException("在使用CheckBox的Grid中不能调用此方法");
            }
        }

        public void RemoveSelectedObject()
        {
            if (!this.isUseCheckBox)
            {
                this.view.DeleteSelectedOrFocusedRows();
            }
            else
            {
                GridColumn column = this.view.Columns[0];
                for (int i = this.view.DataRowCount - 1; i > -1; i--)
                {
                    if (!this.view.IsMasterRow(i) || !this.view.IsGroupRow(i))
                    {
                        bool flag2 = true;
                        if (flag2.Equals(this.view.GetRowCellValue(i, column)))
                        {
                            this.view.DeleteRow(i);
                        }
                    }
                }
            }
        }

        public void SelectAll()
        {
            if (!this.IsUseCheckBox)
            {
                this.view.SelectAll();
            }
            else
            {
                GridColumn column = this.CheckColumn();
                for (int i = 0; i < this.view.DataRowCount; i++)
                {
                    if (!(this.view.IsGroupRow(i) && this.view.IsMasterRow(i)))
                    {
                        this.view.SetRowCellValue(i, column, true);
                    }
                }
            }
        }

        private void SetFooter(string[] sumFields, string countField)
        {
            this.view.GroupSummary.Clear();
            if (((sumFields != null) && (sumFields.Length > 0)) || !string.IsNullOrEmpty(countField))
            {
                this.view.OptionsView.ShowFooter = true;
                this.view.GroupFooterShowMode = GroupFooterShowMode.VisibleIfExpanded;
                if (sumFields != null)
                {
                    foreach (string str in sumFields)
                    {
                        this.view.Columns[str].SummaryItem.SummaryType = SummaryItemType.Sum;
                        this.view.GroupSummary.Add(SummaryItemType.Sum, str);
                    }
                }
                if (!string.IsNullOrEmpty(countField))
                {
                    this.view.Columns[countField].SummaryItem.SummaryType = SummaryItemType.Count;
                    this.view.GroupSummary.Add(SummaryItemType.Count, countField);
                }
            }
        }

        private void SetGroupFields(string[] groupFields)
        {
            this.view.SortInfo.Clear();
            if (groupFields != null)
            {
                this.view.GroupCount = groupFields.Length;
                foreach (string str in groupFields)
                {
                    this.view.SortInfo.Add(this.view.Columns[str], ColumnSortOrder.Ascending);
                }
            }
        }

        public object DataSource
        {
            get
            {
                return this.grid.DataSource;
            }
            set
            {
                this.grid.DataSource = value;
                this.view.BestFitColumns();
            }
        }

        public bool IsUseCheckBox
        {
            get
            {
                return this.isUseCheckBox;
            }
        }

        public int PageIndex
        {
            get
            {
                return this.pageIndex;
            }
            set
            {
                this.pageIndex = value;
            }
        }

        public Dictionary<string, object> Properties
        {
            get
            {
                if (this.properties == null)
                {
                    this.properties = new Dictionary<string, object>();
                }
                return this.properties;
            }
            set
            {
                this.properties = value;
            }
        }
    }
}

