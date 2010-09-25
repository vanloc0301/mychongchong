namespace SkyMap.Net.Workflow.Client.Box
{
    using DevExpress.Data;
    using DevExpress.Utils;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Grid;
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Gui.Components;
    using SkyMap.Net.Workflow.Client;
    using SkyMap.Net.Workflow.Client.Commands;
    using SkyMap.Net.Workflow.Client.Config;
    using SkyMap.Net.Workflow.Client.Dialog;
    using SkyMap.Net.Workflow.Client.Services;
    using SkyMap.Net.Workflow.Engine;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using DevExpress.XtraGrid.Views.Base;

    public class WfBox : AbstractBox, IWfBox, IBox
    {
        private string _daoNameSpace;
        protected string _idField;
        private string _openViewCommandClass;
        protected string _queryName;
        protected string[] _queryParameters;
        protected BackgroundWorker bgLoadDataWorker;
        private Container components = null;
        protected SmGridControl grid;
        private bool isSelected = false;
        private DefaultOpenViewCommand openViewCommand;
        protected RepositoryItemCheckEdit rschkSel;
        protected RepositoryItemTextEdit txtrs_edit;
        protected internal SmGridView view;

        public event RunWorkerCompletedEventHandler LoadDataCompleted;

        public WfBox()
        {
            this.InitializeComponent();
            this.view.OptionsBehavior.UseNewCustomFilterDialog = true;
            this.view.CustomFilterDialog += new CustomFilterDialogEventHandler(this.ViewCutomDialog);
            this.grid.DataSourceChanged += new EventHandler(this.DataSourceChanged);
            this.InitGrid();
        }

        protected void AddStylesForConditions()
        {
        }

        public void CancelSelect()
        {
            for (int i = 0; i < this.view.DataRowCount; i++)
            {
                if ((!this.view.IsMasterRow(i) || !this.view.IsGroupRow(i)) && this.view.GetDataRow(i)["sel"].Equals(true))
                {
                    this.view.SetRowCellValue(i, "sel", false);
                }
            }
        }

        public void CancelTopSelectedRow()
        {
            for (int i = 0; i < this.view.DataRowCount; i++)
            {
                if (this.view.GetDataRow(i)["sel"].Equals(true))
                {
                    this.view.SetRowCellValue(i, "sel", false);
                    break;
                }
            }
        }

        internal void CompleteDeleteOneByOne()
        {
            int dataRowCount = this.view.DataRowCount;
            Dictionary<string, WorkItem> sl = new Dictionary<string, WorkItem>(10);
            int num2 = 1;
            List<DataRow> list = new List<DataRow>();
            for (int i = dataRowCount - 1; i >= 0; i--)
            {
                if (!(this.view.IsMasterRow(i) && this.view.IsGroupRow(i)))
                {
                    DataRow dataRow = this.view.GetDataRow(i);
                    WorkItem workItem = WorkflowService.GetWorkItem(dataRow, this.IdField);
                    sl.Add(workItem.ProinstId, workItem);
                    list.Add(dataRow);
                }
                if ((sl.Count == 10) || (i == 0))
                {
                    WaitDialogHelper.SetText(string.Format("共有{0}/10批业务将被彻底删除,正在删除第{1}批业务...", dataRowCount, num2));
                    WorkflowService.DelegateEvent(sl, new WfClientAPIHandler(WorkflowService.WfcInstance.ComleteDelete));
                    this.DeleteRows(list.ToArray());
                    sl.Clear();
                    num2++;
                }
            }
        }

        private void DataSourceChanged(object sender, EventArgs e)
        {
            if (this.grid.DataSource != null)
            {
                DataView dataSource = this.grid.DataSource as DataView;
                if (dataSource != null)
                {
                    this.ListChanged(dataSource, null);
                    dataSource.ListChanged += new ListChangedEventHandler(this.ListChanged);
                }
            }
        }

        public void DeleteRows(DataRow[] rows)
        {
            foreach (DataRow row in rows)
            {
                if (row.RowState != DataRowState.Deleted)
                {
                    row.Delete();
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

        protected void EnterKeyDown(object sender, KeyEventArgs e)
        {
            if ((e != null) && (e.KeyCode == Keys.Return))
            {
                this.ViewDoubleClick(null, null);
            }
        }

        protected DataRowView GetCurrentDataRowView()
        {
            int focusedRowIndex = this.FocusedRowIndex;
            if (focusedRowIndex > -1)
            {
                return (this.grid.DataSource as DataView)[focusedRowIndex];
            }
            return null;
        }

        protected RepositoryItem GetDefaultColumnEdit()
        {
            if (this.txtrs_edit == null)
            {
                this.txtrs_edit = new RepositoryItemTextEdit();
                this.txtrs_edit.DoubleClick += new EventHandler(this.ViewDoubleClick);
                this.txtrs_edit.KeyDown += new KeyEventHandler(this.EnterKeyDown);
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.Info("初始化编缉器,它承载双击与键盘事件");
                }
            }
            return this.txtrs_edit;
        }

        public DataRow[] GetSelectedRows()
        {
            List<DataRow> list = new List<DataRow>();
            if (this.view.Columns["sel"] != null)
            {
                for (int i = 0; i < this.view.DataRowCount; i++)
                {
                    if (!this.view.IsMasterRow(i) || !this.view.IsGroupRow(i))
                    {
                        DataRow dataRow = this.view.GetDataRow(i);
                        if (dataRow["sel"].Equals(true))
                        {
                            list.Add(dataRow);
                        }
                    }
                }
            }
            else
            {
                int[] selectedOrFocusedRowHandles = this.view.GetSelectedOrFocusedRowHandles();
                foreach (int num2 in selectedOrFocusedRowHandles)
                {
                    list.Add(this.view.GetDataRow(num2));
                }
            }
            return list.ToArray();
        }

        public DataRow[] GetSelectedRows(string[] batchField)
        {
            DataRow[] selectedRows = this.GetSelectedRows();
            if ((batchField != null) && (batchField.Length != 0))
            {
                object[] objArray = new object[batchField.Length];
                int index = 0;
                for (index = 0; index < batchField.Length; index++)
                {
                    objArray[index] = selectedRows[0][batchField[index]];
                }
                for (index = 1; index < selectedRows.Length; index++)
                {
                    for (int i = 0; i < batchField.Length; i++)
                    {
                        if (!selectedRows[index][batchField[i]].Equals(objArray[i]))
                        {
                            throw new CannotBatchExecuteException();
                        }
                    }
                }
            }
            return selectedRows;
        }

        private void InitGrid()
        {
            this.view.GroupPanelText = "请拖动要分组的列到这里";
            this.view.OptionsView.EnableAppearanceEvenRow = this.view.OptionsView.EnableAppearanceOddRow = true;
            this.view.ScrollStyle = ScrollStyleFlags.LiveHorzScroll | ScrollStyleFlags.LiveVertScroll;
            this.view.ColumnFilterChanged += new EventHandler(this.view_ColumnFilterChanged);
        }

        protected override void InitializeAsync(object sender, DoWorkEventArgs e)
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.Info("开始InitializeAsync");
            }
            base.InitializeAsync(sender, e);
            CBoxConfig argument = e.Argument as CBoxConfig;
            this._daoNameSpace = argument.DAONameSpace;
            this._queryName = argument.QueryName;
            this._queryParameters = argument.QueryParameters;
            this._idField = argument.IdField;
            this._openViewCommandClass = argument.OpenViewCommand;
            if (argument.ColList.Count > 0)
            {
                this.InittlResult(argument.ColList);
            }
            this.grid.ContextMenuStrip = base.CreateContextMenu(this.ContextMenuPath);
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.Info("完成InitializeAsync");
            }
        }

        private void InitializeComponent()
        {
            this.grid = new SmGridControl();
            this.view = new SmGridView();
            this.rschkSel = new RepositoryItemCheckEdit();
            this.bgLoadDataWorker = new BackgroundWorker();
            this.grid.BeginInit();
            this.view.BeginInit();
            this.rschkSel.BeginInit();
            base.SuspendLayout();
            this.grid.Dock = DockStyle.Fill;
            this.grid.EmbeddedNavigator.Name = "";
            this.grid.Location = new Point(0, 0);
            this.grid.MainView = this.view;
            this.grid.Name = "grid";
            this.grid.RepositoryItems.AddRange(new RepositoryItem[] { this.rschkSel });
            this.grid.Size = new Size(480, 0x1b0);
            this.grid.TabIndex = 4;
            this.grid.ViewCollection.AddRange(new BaseView[] { this.view });
            this.view.GridControl = this.grid;
            this.view.Name = "view";
            this.view.OptionsMenu.EnableColumnMenu = false;
            this.view.OptionsMenu.EnableFooterMenu = false;
            this.view.OptionsMenu.EnableGroupPanelMenu = false;
            this.view.OptionsView.EnableAppearanceEvenRow = true;
            this.view.OptionsView.EnableAppearanceOddRow = true;
            this.view.PaintStyleName = "Skin";
            this.view.DoubleClick += new EventHandler(this.ViewDoubleClick);
            this.rschkSel.AutoHeight = false;
            this.rschkSel.Name = "rschkSel";
            this.rschkSel.NullStyle = StyleIndeterminate.Unchecked;
            this.rschkSel.EditValueChanged += new EventHandler(this.rschkSel_EditValueChanged);
            this.bgLoadDataWorker.DoWork += new DoWorkEventHandler(this.LoadDataAsync);
            this.bgLoadDataWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.OnLoadDataCompleted);
            base.Controls.Add(this.grid);
            base.Name = "WfAbstractBox";
            base.Size = new Size(480, 0x1b0);
            this.grid.EndInit();
            this.view.EndInit();
            this.rschkSel.EndInit();
            base.ResumeLayout(false);
        }

        protected virtual void InittlResult(IList<CColConfig> cols)
        {
            this.RefreshColumnAutoWidth();
            GridColumn[] columns = new GridColumn[cols.Count + 1];
            columns[0] = new GridColumn();
            columns[0].FieldName = "sel";
            columns[0].VisibleIndex = 0;
            columns[0].Caption = string.Empty;
            columns[0].OptionsColumn.AllowSort = DefaultBoolean.False;
            columns[0].OptionsColumn.AllowGroup = DefaultBoolean.False;
            columns[0].OptionsColumn.AllowSize = false;
            columns[0].OptionsColumn.AllowIncrementalSearch = false;
            columns[0].OptionsColumn.FixedWidth = true;
            columns[0].Width = 15;
            columns[0].ColumnEdit = this.rschkSel;
            for (int i = 0; i < cols.Count; i++)
            {
                CColConfig config = cols[i];
                columns[i + 1] = new GridColumn();
                columns[i + 1].Caption = config.Caption;
                columns[i + 1].FieldName = config.FieldName;
                columns[i + 1].VisibleIndex = config.VisibleIndex;
                columns[i + 1].OptionsColumn.AllowEdit = false;
                if (config.Width > 0)
                {
                    columns[i + 1].Width = config.Width;
                }
                if (!StringHelper.IsNull(config.FormatType))
                {
                    columns[i + 1].DisplayFormat.FormatType = (FormatType) Enum.Parse(typeof(FormatType), config.FormatType);
                    columns[i + 1].DisplayFormat.FormatString = config.FormatString;
                    if (LoggingService.IsInfoEnabled)
                    {
                        LoggingService.Info("get col format:" + columns[i + 1].DisplayFormat.FormatType + columns[i + 1].DisplayFormat.FormatString);
                    }
                }
                columns[i + 1].ColumnEdit = this.GetDefaultColumnEdit();
            }
            this.view.BeginUpdate();
            this.view.Columns.AddRange(columns);
            this.view.EndUpdate();
        }

        private void ListChanged(object sender, ListChangedEventArgs e)
        {
            try
            {
                if (this.view.Columns["sel"] == null)
                {
                    return;
                }
            }
            catch
            {
                return;
            }
            if ((e != null) && (((e.ListChangedType == ListChangedType.ItemChanged) || (e.ListChangedType == ListChangedType.ItemDeleted)) || (e.ListChangedType == ListChangedType.Reset)))
            {
                this.IsSelected = BoxHelper.IsSelected(this);
            }
            DataView dv = sender as DataView;
            this.RefreshViewInfo(dv);
        }

        protected virtual void LoadDataAsync(object sender, DoWorkEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.QueryName))
            {
                if (e.Argument == null)
                {
                    e.Result = BoxHelper.GetData(this);
                }
                else
                {
                    e.Result = BoxHelper.GetData(this, (string[]) e.Argument);
                }
            }
        }

        protected virtual void LoadMainData()
        {
            if (!WorkflowService.IsMyAllDelegated)
            {
                if (!this.bgLoadDataWorker.IsBusy)
                {
                    StatusBarService.ProgressMonitor.BeginTask("正在获取数据...", 0);
                    this.bgLoadDataWorker.RunWorkerAsync(null);
                }
            }
            else
            {
                SystemHintHelper.Show(ResourceService.GetString("Workflow.Box.PromptMyAllDelegated"));
                this.grid.DataSource = null;
            }
        }

        private void OnLoadDataCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            StatusBarService.ProgressMonitor.Done();
            if (e.Error != null)
            {
                LoggingService.ErrorFormatted("载入{0}的数据有错误发生：{1}\r\n{2}", new object[] { base.BoxName, e.Error.Message, e.Error.StackTrace });
            }
            else
            {
                this.DataSource = e.Result;
                if (this.LoadDataCompleted != null)
                {
                    this.LoadDataCompleted(this, e);
                }
            }
        }

        internal virtual void RefreshColumnAutoWidth()
        {
            this.view.OptionsView.ColumnAutoWidth = PropertyService.Get<bool>("ColumnAutoWidth_" + base.BoxName, false);
        }

        public override void RefreshData()
        {
            if (base.Initialized)
            {
                this.LoadMainData();
                //lhm 暂未解决，加上以上语句后会重复刷新，出现假死现象
                //base.BeginInvoke(new Action(this.RefreshData));
            }
            else
            {
                base.waitInitializeAsyncCompletedToRefreshData = true;
            }
        }

        private void RefreshViewInfo(DataView dv)
        {
            if (dv != null)
            {
                string msg = string.Concat(new object[] { "共有", dv.Table.Rows.Count, "项,当前显示", this.view.RowCount, "项。" });
                try
                {
                    if (dv.Count > 0)
                    {
                        msg = "你选择了" + BoxHelper.GetSelectedCount(dv).ToString() + "项," + msg;
                    }
                }
                catch
                {
                }
                StatusBarService.SetOpMsg(msg);
            }
        }

        protected void rschkSel_EditValueChanged(object sender, EventArgs e)
        {
            this.view.PostEditor();
            this.BindingContext[this.view.DataSource].EndCurrentEdit();
        }

        internal void SaveAsProinsts(string url)
        {
            int dataRowCount = this.view.DataRowCount;
            int num2 = 1;
            List<string> list = new List<string>(10);
            for (int i = dataRowCount - 1; i >= 0; i--)
            {
                if (!(this.view.IsMasterRow(i) && this.view.IsGroupRow(i)))
                {
                    DataRow dataRow = this.view.GetDataRow(i);
                    list.Add(dataRow[this.IdField].ToString());
                }
                if ((list.Count == 10) || (i == 0))
                {
                    WaitDialogHelper.SetText(string.Format("共有{0}/10批业务将被另存,正在另存第{1}批业务...", dataRowCount, num2));
                    WorkflowService.WfcInstance.SaveAsProinst(list.ToArray(), url);
                    list.Clear();
                    num2++;
                }
            }
        }

        public void SelectAll()
        {
            for (int i = 0; i < this.view.DataRowCount; i++)
            {
                if ((!this.view.IsMasterRow(i) || !this.view.IsGroupRow(i)) && !this.view.GetDataRow(i)["sel"].Equals(true))
                {
                    this.view.SetRowCellValue(i, "sel", true);
                }
            }
        }

        public void SelectCurrent()
        {
            if (!(this.view.IsMasterRow(this.FocusedRowIndex) && this.view.IsGroupRow(this.FocusedRowIndex)))
            {
                this.view.SetRowCellValue(this.FocusedRowIndex, "sel", true);
            }
        }

        public void SelectElse()
        {
            for (int i = 0; i < this.view.DataRowCount; i++)
            {
                if (!(this.view.IsMasterRow(i) && this.view.IsGroupRow(i)))
                {
                    DataRow dataRow = this.view.GetDataRow(i);
                    bool flag2 = true;
                    this.view.SetRowCellValue(i, "sel", !flag2.Equals(dataRow["sel"]));
                }
            }
        }

        internal void SetGridFooter(string[] sumFields, string countField)
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

        internal void SetGridGroupFields(string[] groupFields)
        {
            this.view.SortInfo.Clear();
            if ((groupFields != null) && (groupFields.Length > 0))
            {
                this.view.GroupCount = groupFields.Length;
                foreach (string str in groupFields)
                {
                    try
                    {
                        if (this.view.Columns[str] != null)
                        {
                            this.view.SortInfo.Add(this.view.Columns[str], ColumnSortOrder.Ascending);
                        }
                        else
                        {
                            LoggingService.WarnFormatted("没有找到分组列：{0}", new object[] { str });
                        }
                    }
                    catch (Exception exception)
                    {
                        LoggingService.Error(exception);
                    }
                }
            }
        }

        private void view_ColumnFilterChanged(object sender, EventArgs e)
        {
            this.RefreshViewInfo((DataView) this.grid.DataSource);
        }

        protected void ViewCutomDialog(object sender, CustomFilterDialogEventArgs e)
        {
            e.Handled = true;
            e.FilterInfo = null;
            new FilterCustomDialog(e.Column, false).ShowDialog();
        }

        protected virtual void ViewDoubleClick(object sender, EventArgs e)
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.Info("双击准备打开一个业务");
            }
            if (this.OpenViewCommand == null)
            {
                LoggingService.WarnFormatted("{0}没有双击打开业务命令，所以无法打开业务", new object[] { base.BoxName });
            }
            else if ((!(sender is SmGridControl) || (this.view.CalcHitInfo((sender as Control).PointToClient(Control.MousePosition)).RowHandle >= 0)) && (this.FocusedRowIndex > -1))
            {
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.Info("找到了可以打开的业务,准备运行打开命令!");
                }
                this.OpenViewCommand.Run();
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.Info("打开命令运行完成");
                }
            }
        }

        protected string ContextMenuPath
        {
            get
            {
                return "/Workflow/WfBoxGrid/ContextMenu";
            }
        }

        public string DAONameSpace
        {
            get
            {
                return this._daoNameSpace;
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

        public int FocusedRowIndex
        {
            get
            {
                if (this.grid.DataSource != null)
                {
                    return this.view.GetDataSourceRowIndex(this.view.FocusedRowHandle);
                }
                return -1;
            }
        }

        public string IdField
        {
            get
            {
                return this._idField;
            }
        }

        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }
            set
            {
                if (this.isSelected != value)
                {
                    this.isSelected = value;
                }
                base.BarStatusUpdate();
            }
        }

        public virtual DefaultOpenViewCommand OpenViewCommand
        {
            get
            {
                if ((this.openViewCommand == null) && !string.IsNullOrEmpty(this._openViewCommandClass))
                {
                    System.Type type = System.Type.GetType(this._openViewCommandClass);
                    if (type != null)
                    {
                        this.openViewCommand = Activator.CreateInstance(type) as DefaultOpenViewCommand;
                        this.openViewCommand.Owner = this;
                    }
                    else
                    {
                        LoggingService.WarnFormatted("不能获取类型 ：{0}", new object[] { this._openViewCommandClass });
                    }
                }
                return this.openViewCommand;
            }
        }

        public string QueryName
        {
            get
            {
                return this._queryName;
            }
        }

        public string[] QueryParameters
        {
            get
            {
                return this._queryParameters;
            }
        }
    }
}

