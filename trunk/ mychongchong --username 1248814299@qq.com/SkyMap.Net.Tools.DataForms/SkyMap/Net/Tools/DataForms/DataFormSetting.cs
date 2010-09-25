namespace SkyMap.Net.Tools.DataForms
{
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraGrid;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Base;
    using DevExpress.XtraGrid.Views.Grid;
    using DevExpress.XtraTab;
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.DataForms;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Gui.Components;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;

    public class DataFormSetting : SmUserControl
    {
        private ContextMenu cmnuDataControl;
        private Container components = null;
        private DAODataForm currentDataForm;
        private UnitOfWork currentUnitOfWork;
        private GridControl gcDataControls;
        private GridView gvDataControls;
        private MenuItem menuItem1;
        private MenuItem miAuto;
        private MenuItem mnuDelDataControl;
        private MenuItem mnuRefreshControls;
        private XtraTabPage pMapColumn;
        private XtraTabPage pMapTable;
        private RepositoryItemComboBox rscmbColumns = new RepositoryItemComboBox();
        private RepositoryItemComboBox rscmbTables = new RepositoryItemComboBox();
        private XtraTabControl tabProperty;
        private TreeView tvDataSets;

        public DataFormSetting()
        {
            this.InitializeComponent();
            SkyMap.Net.Tools.DataForms.TreeViewHelper.SetImageList(this.tvDataSets);
            this.InitDataSets();
            this.InitGridControl();
        }

        private void CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                DataControl row = this.gvDataControls.GetRow(e.RowHandle) as DataControl;
                this.currentUnitOfWork.RegisterDirty(row);
                LoggingService.DebugFormatted("修改了列:{0}的设置", new object[] { row.Name });
            }
        }

        private void DisplayMapColumns(DAODataForm df)
        {
            if ((df.DataControls != null) && (df.DataControls.Count == 0))
            {
                df.DataControls.Add(new DataControl());
                this.gcDataControls.DataSource = df.DataControls;
                df.DataControls.Clear();
                this.gvDataControls.RefreshData();
            }
            else
            {
                this.gcDataControls.DataSource = df.DataControls;
            }
            this.gvDataControls.BestFitColumns();
        }

        private void DisplayMapTables(DAODataForm df)
        {
            this.tvDataSets.AfterCheck -= new TreeViewEventHandler(this.tvDataSets_AfterCheck);
            foreach (TreeNode node in this.tvDataSets.Nodes)
            {
                node.Checked = false;
                foreach (TreeNode node2 in node.Nodes)
                {
                    node2.Checked = false;
                }
            }
            if ((df.BindTables == null) || (df.BindTables.Count == 0))
            {
                this.tvDataSets.AfterCheck += new TreeViewEventHandler(this.tvDataSets_AfterCheck);
            }
            else
            {
                DAODataSet templetDataSet = df.BindTables[0].TempletDataSet;
                foreach (TreeNode node3 in this.tvDataSets.Nodes)
                {
                    if (templetDataSet.Equals(node3.Tag))
                    {
                        node3.Checked = true;
                        foreach (TreeNode node4 in node3.Nodes)
                        {
                            foreach (DAODataTable table in df.BindTables)
                            {
                                if (table.Equals(node4.Tag))
                                {
                                    node4.Checked = true;
                                }
                            }
                        }
                    }
                }
                this.tvDataSets.AfterCheck += new TreeViewEventHandler(this.tvDataSets_AfterCheck);
                this.RefreshRscmbTables(df.BindTables);
                this.tvDataSets.Refresh();
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

        private List<DAODataTable> GetSelectedMapTables()
        {
            List<DAODataTable> list = new List<DAODataTable>();
            foreach (TreeNode node in this.tvDataSets.Nodes)
            {
                if (node.Checked)
                {
                    foreach (TreeNode node2 in node.Nodes)
                    {
                        if (node2.Checked)
                        {
                            list.Add((DAODataTable) node2.Tag);
                        }
                    }
                }
            }
            if (((this.currentDataForm.BindTables != null) && (this.currentDataForm.BindTables.Count != 0)) && (list.Count != 0))
            {
                for (int i = list.Count - 1; i > -1; i--)
                {
                    DAODataTable table = list[i];
                    foreach (DAODataTable table2 in this.currentDataForm.BindTables)
                    {
                        if (table2.Equals(table))
                        {
                            list.Remove(table);
                            list.Add(table2);
                        }
                    }
                }
            }
            return list;
        }

        private void gvDataControls_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            if (e.Column.FieldName == "MapColumn")
            {
                DataControl row = this.gvDataControls.GetRow(e.RowHandle) as DataControl;
                if (row != null)
                {
                    DAODataTable mapTable = row.MapTable;
                    if (mapTable == null)
                    {
                        row.MapColumn = null;
                        this.rscmbColumns.Items.Clear();
                    }
                    else
                    {
                        if (!((row.MapColumn == null) || row.MapColumn.DataTable.Equals(mapTable)))
                        {
                            row.MapColumn = null;
                        }
                        this.rscmbColumns.Items.Clear();
                        foreach (DAODataColumn column2 in mapTable.DataColumns)
                        {
                            this.rscmbColumns.Items.Add(column2);
                        }
                        e.RepositoryItem = this.rscmbColumns;
                    }
                }
            }
        }

        private void InitDataSets()
        {
            this.tvDataSets.CheckBoxes = true;
            IList<DAODataSet> list = QueryHelper.List<DAODataSet>("ALL_DAODataSet");
            foreach (DAODataSet set in list)
            {
                TreeNode node = SkyMap.Net.Tools.DataForms.TreeViewHelper.AddTreeNode(this.tvDataSets.Nodes, set.Name + "(" + set.Description + ")", set);
                foreach (DAODataTable table in set.DAODataTables)
                {
                    SkyMap.Net.Tools.DataForms.TreeViewHelper.AddTreeNode(node.Nodes, table.Name + "(" + table.Name + ")", table);
                }
            }
        }

        private void InitGridControl()
        {
            this.gcDataControls.BeginUpdate();
            this.gvDataControls.OptionsView.ShowGroupPanel = true;
            string[] strArray = new string[] { "Name", "MapTable", "MapColumn", "Type", "ValueList", "ValueDataSource", "Description", "DisplayOrder" };
            string[] strArray2 = new string[] { "名称", "关联表", "关联字段", "类型", "值列表", "同步SQL", "备注", "显示顺序" };
            int length = strArray.Length;
            GridColumn[] columns = new GridColumn[length];
            RepositoryItemMemoEdit edit = new RepositoryItemMemoEdit();
            for (int i = 0; i < length; i++)
            {
                columns[i] = new GridColumn();
                columns[i].FieldName = strArray[i];
                columns[i].Caption = strArray2[i];
                columns[i].VisibleIndex = i;
                string str = strArray[i];
                if (str != null)
                {
                    if (!(str == "MapTable"))
                    {
                        if (str == "MapColumn")
                        {
                            goto Label_016A;
                        }
                        if ((str == "ValueList") || (str == "ValueDataSource"))
                        {
                            goto Label_017C;
                        }
                    }
                    else
                    {
                        columns[i].ColumnEdit = this.rscmbTables;
                    }
                }
                continue;
            Label_016A:
                columns[i].ColumnEdit = this.rscmbColumns;
                continue;
            Label_017C:
                columns[i].ColumnEdit = edit;
                columns[i].OptionsColumn.FixedWidth = true;
                columns[i].Width = 250;
            }
            this.gvDataControls.Columns.AddRange(columns);
            this.gvDataControls.OptionsView.RowAutoHeight = true;
            this.gvDataControls.OptionsView.ColumnAutoWidth = false;
            this.gvDataControls.OptionsView.ShowAutoFilterRow = true;
            this.gcDataControls.EndUpdate();
        }

        private void InitializeComponent()
        {
            this.cmnuDataControl = new ContextMenu();
            this.mnuRefreshControls = new MenuItem();
            this.mnuDelDataControl = new MenuItem();
            this.menuItem1 = new MenuItem();
            this.miAuto = new MenuItem();
            this.tabProperty = new XtraTabControl();
            this.pMapTable = new XtraTabPage();
            this.tvDataSets = new TreeView();
            this.pMapColumn = new XtraTabPage();
            this.gcDataControls = new GridControl();
            this.gvDataControls = new GridView();
            this.tabProperty.BeginInit();
            this.tabProperty.SuspendLayout();
            this.pMapTable.SuspendLayout();
            this.pMapColumn.SuspendLayout();
            this.gcDataControls.BeginInit();
            this.gvDataControls.BeginInit();
            base.SuspendLayout();
            this.cmnuDataControl.MenuItems.AddRange(new MenuItem[] { this.mnuRefreshControls, this.mnuDelDataControl, this.menuItem1, this.miAuto });
            this.mnuRefreshControls.Index = 0;
            this.mnuRefreshControls.Text = "更新表单控件";
            this.mnuRefreshControls.Click += new EventHandler(this.mnuRefreshControls_Click);
            this.mnuDelDataControl.Index = 1;
            this.mnuDelDataControl.Text = "删除";
            this.mnuDelDataControl.Click += new EventHandler(this.mnuDelDataControl_Click);
            this.menuItem1.Index = 2;
            this.menuItem1.Text = "-";
            this.miAuto.Index = 3;
            this.miAuto.Text = "控件与字段自动关联";
            this.miAuto.Click += new EventHandler(this.miAuto_Click);
            this.tabProperty.Dock = DockStyle.Fill;
            this.tabProperty.Location = new Point(3, 3);
            this.tabProperty.Name = "tabProperty";
            this.tabProperty.SelectedTabPage = this.pMapTable;
            this.tabProperty.Size = new Size(0x206, 0x143);
            this.tabProperty.TabIndex = 1;
            this.tabProperty.TabPages.AddRange(new XtraTabPage[] { this.pMapTable, this.pMapColumn });
            this.pMapTable.Controls.Add(this.tvDataSets);
            this.pMapTable.Name = "pMapTable";
            this.pMapTable.Padding = new Padding(2);
            this.pMapTable.Size = new Size(0x1fd, 0x123);
            this.pMapTable.Text = "关联表";
            this.tvDataSets.Dock = DockStyle.Fill;
            this.tvDataSets.Location = new Point(2, 2);
            this.tvDataSets.Name = "tvDataSets";
            this.tvDataSets.Size = new Size(0x1f9, 0x11f);
            this.tvDataSets.TabIndex = 1;
            this.pMapColumn.Controls.Add(this.gcDataControls);
            this.pMapColumn.Name = "pMapColumn";
            this.pMapColumn.Padding = new Padding(2);
            this.pMapColumn.Size = new Size(0x1fd, 0x123);
            this.pMapColumn.Text = "数据关联";
            this.gcDataControls.ContextMenu = this.cmnuDataControl;
            this.gcDataControls.Dock = DockStyle.Fill;
            this.gcDataControls.EmbeddedNavigator.Name = "";
            this.gcDataControls.Location = new Point(2, 2);
            this.gcDataControls.MainView = this.gvDataControls;
            this.gcDataControls.Name = "gcDataControls";
            this.gcDataControls.Size = new Size(0x1f9, 0x11f);
            this.gcDataControls.TabIndex = 2;
            this.gcDataControls.ViewCollection.AddRange(new BaseView[] { this.gvDataControls });
            this.gvDataControls.GridControl = this.gcDataControls;
            this.gvDataControls.Name = "gvDataControls";
            this.gvDataControls.OptionsSelection.MultiSelect = true;
            this.gvDataControls.OptionsView.ShowGroupedColumns = true;
            this.gvDataControls.CellValueChanged += new CellValueChangedEventHandler(this.CellValueChanged);
            this.gvDataControls.CustomRowCellEdit += new CustomRowCellEditEventHandler(this.gvDataControls_CustomRowCellEdit);
            base.Controls.Add(this.tabProperty);
            base.Name = "DataFormSetting";
            base.Padding = new Padding(3);
            base.Size = new Size(0x20c, 0x149);
            this.tabProperty.EndInit();
            this.tabProperty.ResumeLayout(false);
            this.pMapTable.ResumeLayout(false);
            this.pMapColumn.ResumeLayout(false);
            this.gcDataControls.EndInit();
            this.gvDataControls.EndInit();
            base.ResumeLayout(false);
        }

        private void LoadDataControlsFromAssembly(string file)
        {
            Assembly assembly = Assembly.LoadFile(file);
            if (assembly == null)
            {
                MessageHelper.ShowInfo("不能载入表单程序集,请检查程序集是否正确!");
            }
            else
            {
                System.Type type = assembly.GetType(this.currentDataForm.Name);
                if (type == null)
                {
                    MessageHelper.ShowInfo(string.Format("不能得到表单类型:{0}!", this.currentDataForm.Name));
                }
                else
                {
                    this.gvDataControls.PostEditor();
                    IDataForm form = Activator.CreateInstance(type) as IDataForm;
                    if (form != null)
                    {
                        IDictionary dataControls = form.DataControls;
                        if (dataControls.Count != 0)
                        {
                            DataControl row;
                            if (this.currentDataForm.DataControls == null)
                            {
                                this.currentDataForm.DataControls = new List<DataControl>();
                            }
                            IList<DataControl> list = this.currentDataForm.DataControls;
                            Dictionary<string, DataControl> dictionary2 = new Dictionary<string, DataControl>();
                            new StringCollection();
                            this.gvDataControls.BeginUpdate();
                            try
                            {
                                DialogResult none = DialogResult.None;
                                DialogResult result2 = DialogResult.None;
                                for (int i = this.gvDataControls.RowCount - 1; i > -1; i--)
                                {
                                    row = this.gvDataControls.GetRow(i) as DataControl;
                                    if (!dataControls.Contains(row.Name))
                                    {
                                        switch (none)
                                        {
                                            case DialogResult.None:
                                                none = MessageHelper.ShowYesNoCancelInfo("控件：{0}已经不再存在，是否删除它？(其余都执行相同操作)", row.Name);
                                                break;

                                            case DialogResult.Yes:
                                                this.currentUnitOfWork.RegisterRemoved(row);
                                                this.gvDataControls.DeleteRow(i);
                                                break;

                                            case DialogResult.Cancel:
                                                return;
                                        }
                                    }
                                    else if (!dictionary2.ContainsKey(row.Name))
                                    {
                                        dictionary2.Add(row.Name, row);
                                        Control control = dataControls[row.Name] as Control;
                                        string fullName = control.GetType().FullName;
                                        if (fullName != row.Type)
                                        {
                                            switch (result2)
                                            {
                                                case DialogResult.None:
                                                    result2 = MessageHelper.ShowYesNoCancelInfo("控件{0}类型已经改变是否更新它？(其余都执行相同操作)", row.Name);
                                                    break;

                                                case DialogResult.Yes:
                                                    row.Type = fullName;
                                                    this.currentUnitOfWork.RegisterDirty(row);
                                                    break;

                                                case DialogResult.Cancel:
                                                    return;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        this.currentUnitOfWork.RegisterRemoved(row);
                                        this.gvDataControls.DeleteRow(i);
                                    }
                                }
                            }
                            finally
                            {
                                this.gvDataControls.EndUpdate();
                            }
                            int num2 = 0;
                            foreach (Control control3 in dataControls.Values)
                            {
                                if (!dictionary2.ContainsKey(control3.Name))
                                {
                                    row = new DataControl();
                                    row.Id = StringHelper.GetNewGuid();
                                    row.Name = control3.Name;
                                    row.DisplayOrder = num2;
                                    try
                                    {
                                        row.Description = control3.Text;
                                    }
                                    catch
                                    {
                                    }
                                    if (row.Name.StartsWith("tp_"))
                                    {
                                        row.ValueList = "/Workflow/WfView/ToolBar/ProjectCaptureCommand2";
                                    }
                                    else if (row.Name.StartsWith("qm_"))
                                    {
                                        row.ValueList = string.Format("txt_{0},dt_{0},1", row.Name.Substring(3));
                                    }
                                    else if (row.Name.StartsWith("dk_"))
                                    {
                                        row.ValueList = string.Format("txt_{0},txt_{0}身份证号", row.Name.Substring(3));
                                    }
                                    else if (row.Name.StartsWith("imgwb_"))
                                    {
                                        row.ValueList = string.Format("txt_{0}", row.Name.Substring(6));
                                    }
                                    else if (row.Name.StartsWith("print_"))
                                    {
                                        row.ValueList = "打印报表的ID";
                                    }
                                    row.Type = control3.GetType().FullName;
                                    row.Owner = this.currentDataForm;
                                    this.currentDataForm.DataControls.Add(row);
                                    this.currentUnitOfWork.RegisterNew(row);
                                }
                                else
                                {
                                    row = dictionary2[control3.Name];
                                    if (row.DisplayOrder != num2)
                                    {
                                        row.DisplayOrder = num2;
                                        this.currentUnitOfWork.RegisterDirty(row);
                                    }
                                }
                                num2++;
                            }
                            this.gvDataControls.RefreshData();
                        }
                    }
                }
            }
        }

        private void LoadDataControlsFromHTML(string file)
        {
            if (File.Exists(file))
            {
                WebBrowser wb = new WebBrowser();
                wb.Url = new Uri(file);
                wb.DocumentCompleted += delegate (object sender, WebBrowserDocumentCompletedEventArgs e) {
                    DataControl row;
                    HtmlDocument document = wb.Document;
                    if (this.currentDataForm.DataControls == null)
                    {
                        this.currentDataForm.DataControls = new List<DataControl>();
                    }
                    IList<DataControl> dataControls = this.currentDataForm.DataControls;
                    StringCollection strings = new StringCollection();
                    this.gvDataControls.BeginUpdate();
                    try
                    {
                        DialogResult none = DialogResult.None;
                        DialogResult result2 = DialogResult.None;
                        for (int j = this.gvDataControls.RowCount - 1; j > -1; j--)
                        {
                            row = this.gvDataControls.GetRow(j) as DataControl;
                            HtmlElement elementById = document.GetElementById(row.Name);
                            if (elementById == null)
                            {
                                switch (none)
                                {
                                    case DialogResult.None:
                                        none = MessageHelper.ShowYesNoCancelInfo("控件：{0}已经不再存在，是否删除它？(其余都执行相同操作)", row.Name);
                                        break;

                                    case DialogResult.Yes:
                                        this.currentUnitOfWork.RegisterRemoved(row);
                                        this.gvDataControls.DeleteRow(j);
                                        break;

                                    case DialogResult.Cancel:
                                        return;
                                }
                            }
                            else
                            {
                                strings.Add(row.Name);
                                string tagName = elementById.TagName;
                                if (tagName != row.Type)
                                {
                                    switch (result2)
                                    {
                                        case DialogResult.None:
                                            result2 = MessageHelper.ShowYesNoCancelInfo("控件{0}类型已经改变是否更新它？(其余都执行相同操作)", row.Name);
                                            break;

                                        case DialogResult.Yes:
                                            row.Type = tagName;
                                            this.currentUnitOfWork.RegisterDirty(row);
                                            break;

                                        case DialogResult.Cancel:
                                            return;
                                    }
                                }
                            }
                        }
                    }
                    finally
                    {
                        this.gvDataControls.EndUpdate();
                    }
                    string[] strArray = new string[] { "input", "select" };
                    foreach (string str2 in strArray)
                    {
                        HtmlElementCollection elementsByTagName = document.GetElementsByTagName(str2);
                        foreach (HtmlElement element in elementsByTagName)
                        {
                            if (!strings.Contains(element.Id))
                            {
                                row = new DataControl {
                                    Name = element.Id
                                };
                                try
                                {
                                    row.Description = element.GetAttribute("title");
                                }
                                catch
                                {
                                }
                                row.Type = element.TagName;
                                row.Owner = this.currentDataForm;
                                this.currentDataForm.DataControls.Add(row);
                                this.currentUnitOfWork.RegisterNew(row);
                            }
                            this.gvDataControls.RefreshData();
                        }
                    }
                };
            }
        }

        public void LoadMe(DomainObject domainObject, UnitOfWork unitOfWork)
        {
            DAODataForm df = (DAODataForm) domainObject;
            if (df == null)
            {
                throw new ArgumentOutOfRangeException("domainObject");
            }
            this.currentDataForm = df;
            this.DisplayMapTables(df);
            this.DisplayMapColumns(df);
            this.currentUnitOfWork = unitOfWork;
        }

        private void miAuto_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            if (this.rscmbTables.Items.Count == 0)
            {
                text = "请先选择要关联的数据表";
            }
            if ((text == string.Empty) && ((this.currentDataForm.DataControls == null) || (this.currentDataForm.DataControls.Count == 0)))
            {
                text = "没有找到可关联的控件列表！\r你可能需要先更新控件列表";
            }
            if (text != string.Empty)
            {
                MessageHelper.ShowInfo(text);
            }
            else
            {
                int num = 0;
                this.gvDataControls.BeginUpdate();
                foreach (DataControl control in this.currentDataForm.DataControls)
                {
                    foreach (DAODataTable table in this.rscmbTables.Items)
                    {
                        foreach (DAODataColumn column in table.DataColumns)
                        {
                            if ((control.Name.ToLower().EndsWith("_" + column.Name.ToLower()) || control.Name.ToLower().EndsWith(column.Name.ToLower())) && !column.Equals(control.MapColumn))
                            {
                                control.MapTable = table;
                                control.MapColumn = column;
                                this.currentUnitOfWork.RegisterDirty(control);
                                text = text + control.Name + ",";
                                num++;
                                if (num == 5)
                                {
                                    text = text + "\r";
                                    num = 0;
                                }
                            }
                        }
                    }
                }
                this.gvDataControls.EndUpdate();
                if (text != string.Empty)
                {
                    MessageHelper.ShowInfo("成功地更新以下控件的数据关联关系：\r" + text + "\r你需要保存它们才能更新数据库配置");
                }
            }
        }

        private void mnuDelDataControl_Click(object sender, EventArgs e)
        {
            int[] selectedRows = this.gvDataControls.GetSelectedRows();
            for (int i = selectedRows.Length - 1; i >= 0; i--)
            {
                this.currentUnitOfWork.RegisterRemoved(this.gvDataControls.GetRow(selectedRows[i]) as DataControl);
                this.gvDataControls.DeleteRow(selectedRows[i]);
            }
        }

        private void mnuRefreshControls_Click(object sender, EventArgs e)
        {
            WaitDialogHelper.Show();
            string str = this.currentDataForm.Name + "," + this.currentDataForm.AssemblyName;
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "dll files (*.dll)|*.dll|html files(*.html)|*.html|htm files(*.htm)|*.htm";
                dialog.FilterIndex = 1;
                dialog.InitialDirectory = Environment.CurrentDirectory;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string fileName = dialog.FileName;
                    if (fileName.EndsWith(".dll"))
                    {
                        this.LoadDataControlsFromAssembly(fileName);
                    }
                    else if (fileName.EndsWith(".html") || fileName.EndsWith(".htm"))
                    {
                        this.LoadDataControlsFromHTML(fileName);
                    }
                }
            }
            catch (Exception exception)
            {
                MessageHelper.ShowError("更新表单时有错误：" + exception.Message, exception);
            }
            finally
            {
                WaitDialogHelper.Close();
            }
        }

        private void RefreshRscmbTables(IList<DAODataTable> ddts)
        {
            this.rscmbTables.Items.Clear();
            this.rscmbTables.Items.AddRange(new List<DAODataTable>(ddts).ToArray());
        }

        private void tvDataSets_AfterCheck(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            TreeNode parent = node.Parent;
            this.tvDataSets.AfterCheck -= new TreeViewEventHandler(this.tvDataSets_AfterCheck);
            if (parent != null)
            {
                if (node.Checked && !parent.Checked)
                {
                    parent.Checked = true;
                }
            }
            else
            {
                if (node.Checked)
                {
                    foreach (TreeNode node3 in this.tvDataSets.Nodes)
                    {
                        if (!node3.Equals(node) && node3.Checked)
                        {
                            node3.Checked = false;
                            foreach (TreeNode node4 in node3.Nodes)
                            {
                                node4.Checked = false;
                            }
                        }
                    }
                }
                foreach (TreeNode node5 in node.Nodes)
                {
                    node5.Checked = node.Checked;
                }
            }
            this.tvDataSets.Refresh();
            this.tvDataSets.AfterCheck += new TreeViewEventHandler(this.tvDataSets_AfterCheck);
            this.currentDataForm.BindTables = this.GetSelectedMapTables();
            this.currentUnitOfWork.RegisterDirty(this.currentDataForm);
            this.RefreshRscmbTables(this.currentDataForm.BindTables);
        }
    }
}

