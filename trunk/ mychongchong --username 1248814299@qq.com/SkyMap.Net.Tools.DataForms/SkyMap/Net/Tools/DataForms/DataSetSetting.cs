namespace SkyMap.Net.Tools.DataForms
{
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraGrid;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Base;
    using DevExpress.XtraGrid.Views.Grid;
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.DataForms;
    using SkyMap.Net.DataForms.DataEngine;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Gui.Components;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;

    public class DataSetSetting : SmUserControl
    {
        private ContextMenu cmnuDataColumn;
        private Container components = null;
        private UnitOfWork currentUnitOfWork;
        private DAODataTable daoDataTable;
        private GridControl gcDataColumn;
        private GridView gvDataColumn;
        private MenuItem mnuDelDataColumn;
        private MenuItem mnuRefreshDataColumn;

        public DataSetSetting()
        {
            this.InitializeComponent();
            this.InitGridControl();
        }

        private DAODataColumn CreateNewDAODataColumn(DAODataTable ddt, DataRow row)
        {
            DAODataColumn item = new DAODataColumn();
            item.Id = StringHelper.GetNewGuid();
            item.Name = row["ColumnName"].ToString();
            item.Description = item.Name;
            item.DataType = (row["DataType"] as System.Type).Name;
            item.Length = (int)row["ColumnSize"];
            item.DataTable = ddt;
            item.DisplayIndex = row.Table.Rows.IndexOf(row);
            ddt.DataColumns.Add(item);
            this.currentUnitOfWork.RegisterNew(item);
            return item;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private DataTable GetSchemaTable()
        {
            DataTable schemaTable;
            DBConnection connection = new DBConnection(this.daoDataTable.TempletDataSet.DataSource);
            connection.Open();
            try
            {
                IDbCommand command = connection.Connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "select * from " + this.daoDataTable.Name + " where 1<>1";
                IDataReader reader = command.ExecuteReader();
                try
                {
                    schemaTable = reader.GetSchemaTable();
                }
                finally
                {
                    reader.Close();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                connection.Close();
            }
            return schemaTable;
        }

        private void gvDataColumn_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                DAODataColumn row = this.gvDataColumn.GetRow(e.RowHandle) as DAODataColumn;
                this.currentUnitOfWork.RegisterDirty(row);
            }
        }

        private void InitGridControl()
        {
            this.gcDataColumn.BeginUpdate();
            this.gvDataColumn.OptionsView.ShowGroupPanel = true;
            this.gvDataColumn.OptionsView.ShowGroupedColumns = true;
            string[] strArray = new string[] { "Name", "DataType", "Length", "IsDisplay", "IsNeedInitial", "InitialValue", "IsQuery", "DisplayIndex", "Description" };
            string[] strArray2 = new string[] { "名称", "类型", "长度", "是否显示", "是否初始化", "初始化值", "是否查询", "显示顺序", "备注" };
            int length = strArray.Length;
            GridColumn[] columns = new GridColumn[length];
            for (int i = 0; i < length; i++)
            {
                columns[i] = new GridColumn();
                columns[i].FieldName = strArray[i];
                columns[i].Caption = strArray2[i];
                columns[i].VisibleIndex = i;
            }
            RepositoryItemComboBox box = new RepositoryItemComboBox();
            box.TextEditStyle = TextEditStyles.Standard;
            box.Items.AddRange(new string[] { "", "{SYS:NOW}", "{SYS:STAFFID}", "{SYS:STAFFNAME}", "{SYS:DEPTID}", "{SYS:DEPTNAME}" });
            columns[5].ColumnEdit = box;
            this.gvDataColumn.Columns.AddRange(columns);
            this.gvDataColumn.OptionsView.ShowAutoFilterRow = true;
            this.gcDataColumn.EndUpdate();
        }

        private void InitializeComponent()
        {
            this.cmnuDataColumn = new ContextMenu();
            this.mnuRefreshDataColumn = new MenuItem();
            this.mnuDelDataColumn = new MenuItem();
            this.gcDataColumn = new GridControl();
            this.gvDataColumn = new GridView();
            this.gcDataColumn.BeginInit();
            this.gvDataColumn.BeginInit();
            base.SuspendLayout();
            this.cmnuDataColumn.MenuItems.AddRange(new MenuItem[] { this.mnuRefreshDataColumn, this.mnuDelDataColumn });
            this.mnuRefreshDataColumn.Index = 0;
            this.mnuRefreshDataColumn.Text = "更新";
            this.mnuRefreshDataColumn.Click += new EventHandler(this.mnuRefreshDataColumn_Click);
            this.mnuDelDataColumn.Index = 1;
            this.mnuDelDataColumn.Text = "删除";
            this.mnuDelDataColumn.Click += new EventHandler(this.mnuDelDataColumn_Click);
            this.gcDataColumn.ContextMenu = this.cmnuDataColumn;
            this.gcDataColumn.Dock = DockStyle.Fill;
            this.gcDataColumn.EmbeddedNavigator.Name = "";
            this.gcDataColumn.Location = new Point(0, 0);
            this.gcDataColumn.MainView = this.gvDataColumn;
            this.gcDataColumn.Name = "gcDataColumn";
            this.gcDataColumn.Size = new Size(760, 0x158);
            this.gcDataColumn.TabIndex = 3;
            this.gcDataColumn.ViewCollection.AddRange(new BaseView[] { this.gvDataColumn });
            this.gvDataColumn.GridControl = this.gcDataColumn;
            this.gvDataColumn.Name = "gvDataColumn";
            this.gvDataColumn.OptionsSelection.MultiSelect = true;
            this.gvDataColumn.CellValueChanged += new CellValueChangedEventHandler(this.gvDataColumn_CellValueChanged);
            base.Controls.Add(this.gcDataColumn);
            base.Name = "DataSetSetting";
            base.Size = new Size(760, 0x158);
            this.gcDataColumn.EndInit();
            this.gvDataColumn.EndInit();
            base.ResumeLayout(false);
        }

        public void LoadMe(DAODataTable ddt, UnitOfWork unitOfWork)
        {
            if (ddt == null)
            {
                throw new ArgumentNullException("ddt");
            }
            if (unitOfWork == null)
            {
                throw new ArgumentNullException("unitOfWork");
            }
            this.daoDataTable = ddt;
            this.currentUnitOfWork = unitOfWork;
            this.gcDataColumn.DataSource = null;
            this.gcDataColumn.DataSource = ddt.DataColumns;
        }

        private void mnuDelDataColumn_Click(object sender, EventArgs e)
        {
            if (this.daoDataTable != null)
            {
                int[] selectedRows = this.gvDataColumn.GetSelectedRows();
                for (int i = selectedRows.Length - 1; i > -1; i--)
                {
                    DAODataColumn row = this.gvDataColumn.GetRow(selectedRows[i]) as DAODataColumn;
                    this.currentUnitOfWork.RegisterRemoved(row);
                    this.gvDataColumn.DeleteRow(selectedRows[i]);
                }
            }
        }

        private void mnuRefreshDataColumn_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    if (this.daoDataTable != null)
                    {
                        DataRow current;
                        DataTable schemaTable = this.GetSchemaTable();
                        Dictionary<string, DataRow> dictionary = new Dictionary<string, DataRow>(schemaTable.Rows.Count);
                        IEnumerator enumerator = schemaTable.Rows.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            current = (DataRow)enumerator.Current;
                            dictionary.Add(current["ColumnName"].ToString(), current);
                        }
                        if (this.daoDataTable.DataColumns == null)
                        {
                            this.daoDataTable.DataColumns = new List<DAODataColumn>();
                        }
                        DialogResult none = DialogResult.None;
                        DialogResult result2 = DialogResult.None;
                        DAODataColumn column = null;
                        this.gvDataColumn.BeginUpdate();
                        try
                        {
                            for (int i = this.gvDataColumn.RowCount - 1; i >= 0; i--)
                            {
                                column = this.gvDataColumn.GetRow(i) as DAODataColumn;
                                string name = column.Name;
                                if (dictionary.ContainsKey(name))
                                {
                                    current = dictionary[name];
                                    int num = (int)current["ColumnSize"];
                                    string str2 = (current["DataType"] as System.Type).Name;
                                    int index = current.Table.Rows.IndexOf(current);
                                    if ((!num.Equals(column.Length) || !str2.Equals(column.DataType)) || (column.DisplayIndex != index))
                                    {
                                        switch (none)
                                        {
                                            case DialogResult.None:
                                                none = MessageHelper.ShowYesNoCancelInfo("列：" + name + "的数据类型或长度或者显示顺序已经改变，是否更新它们(其它相同)？");
                                                break;

                                            case DialogResult.Yes:
                                                column.Length = num;
                                                column.DataType = str2;
                                                column.DisplayIndex = index;
                                                this.currentUnitOfWork.RegisterDirty(column);
                                                break;

                                            case DialogResult.Cancel:
                                                return;
                                        }
                                    }
                                    dictionary.Remove(name);
                                }
                                else
                                {
                                    switch (result2)
                                    {
                                        case DialogResult.None:
                                            result2 = MessageHelper.ShowYesNoCancelInfo("列：" + name + "已经不存在，是否删除它？(其它相同)");
                                            break;

                                        case DialogResult.Yes:
                                            this.currentUnitOfWork.RegisterRemoved(column);
                                            this.gvDataColumn.DeleteRow(i);
                                            goto Label_027C;

                                        case DialogResult.Cancel:
                                            return;
                                    }
                                Label_027C: ;
                                }
                            }
                        }
                        finally
                        {
                            this.gvDataColumn.EndUpdate();
                        }
                        if (dictionary.Count > 0)
                        {
                            foreach (DataRow row in dictionary.Values)
                            {
                                column = this.CreateNewDAODataColumn(this.daoDataTable, row);
                            }
                            this.gcDataColumn.DataSource = null;
                            this.gcDataColumn.DataSource = this.daoDataTable.DataColumns;
                        }
                    }
                }
                catch (Exception exception)
                {
                    MessageHelper.ShowError("更新数据表配置时发生错误", exception);
                }
            }
            finally
            {
            }
        }
    }
}

