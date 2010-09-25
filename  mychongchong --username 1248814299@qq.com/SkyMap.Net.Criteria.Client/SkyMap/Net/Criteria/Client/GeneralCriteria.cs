namespace SkyMap.Net.Criteria.Client
{
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraTreeList;
    using DevExpress.XtraTreeList.Columns;
    using DevExpress.XtraTreeList.Nodes;
    using SkyMap.Net.Core;
    using SkyMap.Net.Criteria;
    using SkyMap.Net.Gui.Components;
    using SkyMap.Net.Security;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class GeneralCriteria : SmUserControl
    {
        private SkyMap.Net.Criteria.Client.CriteriaBusiness _criteriaBusiness;
        protected const string _nodetagsimple = "simple";
        protected internal Button bt_DelFilter;
        protected internal Button bt_DelRow;
        protected internal Button bt_Ok;
        protected internal Button bt_SaveFilter;
        private RepositoryItemComboBox cmbrs_cnd;
        private RepositoryItemComboBox cmbrs_fld;
        private RepositoryItemComboBox cmbrs_tableName;
        private RepositoryItemComboBox cmbrs_type;
        private TreeListColumn col_condition;
        private TreeListColumn col_fieldid;
        private TreeListColumn col_op;
        private TreeListColumn col_Rel;
        private TreeListColumn col_tableName;
        private TreeListColumn col_type;
        private TreeListColumn col_value;
        private IContainer components;
        private RepositoryItemDateEdit dtrs_val;
        private GroupBox gbFilter;
        private ImageList imageList1;
        protected internal TreeList lv_condition;
        protected internal Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Panel panel4;
        private Panel pnlMain;
        private Splitter splitter1;
        protected internal TreeView tv_List;
        protected internal TextBox txtFilterName;
        private RepositoryItemTextEdit txtrs_val;
        private RepositoryItemComboBox txtrs_cbx;
        public event CriteriaEventHandle Criteria;

        public GeneralCriteria()
        {
            this.InitializeComponent();
            this.GridEditorsAdjust();
            SecurityUtil.CurrentPrincipalChanged += new EventHandler(this.SecurityUtil_CurrentPrincipalChanged);
        }

        protected TreeNode AddChildNode(TreeNode pnode, TyQuery query)
        {
            TreeNode node = pnode.Nodes.Add(query.Name);
            node.Tag = query;
            return node;
        }

        protected TreeNode AddChildNode(TreeNode pnode, TrFilter query)
        {
            TreeNode node = pnode.Nodes.Add(query.Name);
            node.Tag = query;
            return node;
        }

        protected TreeNode AddChildNode(TreeNode pnode, string TextFld, DataRow dr)
        {
            TreeNode node = pnode.Nodes.Add(dr[TextFld].ToString());
            node.Tag = dr;
            return node;
        }

        protected virtual void AddConditionFieldToCmb(TyQuery query)
        {
            this.cmbrs_fld.Items.Clear();
            this.cmbrs_cnd.Items.Clear();
            this.cmbrs_type.Items.Clear();
            this.cmbrs_tableName.Items.Clear();
            foreach (TyQueryWhere where in query.QueryWheres)
            {
                this.cmbrs_cnd.Items.Add(where.Description);
                this.cmbrs_fld.Items.Add(where.Name);
                this.cmbrs_type.Items.Add(where.ColumnType);
                this.cmbrs_tableName.Items.Add(where.TableName);
            }
        }

        protected virtual void AddConditionFieldToCmb(DataRow drsearchx)
        {
            this.cmbrs_fld.Items.Clear();
            this.cmbrs_cnd.Items.Clear();
            this.cmbrs_type.Items.Clear();
            foreach (DataRow row in this._criteriaBusiness.GetSelectChildRows(drsearchx))
            {
                if (row["is_condtion"].ToString() == "是")
                {
                    this.cmbrs_cnd.Items.Add(row["Condition_Name"]);
                    this.cmbrs_fld.Items.Add(row["ID"]);
                    this.cmbrs_type.Items.Add(row["Field_Type"]);
                    this.cmbrs_tableName.Items.Add("");
                }
            }
        }

        protected virtual void AfterSelectCxNode(object sender, TreeViewEventArgs e)
        {
            TrFilter trfilter;
            TreeNode node = e.Node;
            this._criteriaBusiness.DtCondition.Rows.Clear();
            this.bt_SaveFilter.Enabled = false;
            this.bt_DelFilter.Enabled = false;
            if (!this.IsQueryNode(node) && !this.IsTrFilterNode(node))
            {
                if (node.Parent != null)
                {
                    DataRow row;
                    object tag = node.Parent.Tag;
                    if (tag.Equals("simple"))
                    {
                        row = node.Tag as DataRow;
                        this.AddConditionFieldToCmb(row);
                        if (this.cmbrs_cnd.Items.Count > 0)
                        {
                            this._criteriaBusiness.AddNewCondtion(this.cmbrs_cnd.Items[0].ToString(), this.cmbrs_fld.Items[0].ToString(), this.cmbrs_type.Items[0].ToString(), null);
                        }
                        else
                        {
                            MessageBox.Show("没有设置任何可作为条件的字段");
                        }
                        this.txtFilterName.Text = "新过滤X";
                        this.bt_SaveFilter.Enabled = true;
                        this.gbFilter.Visible = true;
                    }
                    else if ((node.Parent.Parent != null) && node.Parent.Parent.Tag.Equals("simple"))
                    {
                        row = tag as DataRow;
                        if (row != null)
                        {
                            this.AddConditionFieldToCmb(row);
                            DataRow filterX = node.Tag as DataRow;
                            this._criteriaBusiness.AddNewCondtion(filterX);
                        }                      
                        this.txtFilterName.Text = node.Text;
                        this.bt_SaveFilter.Enabled = true;
                        this.bt_DelFilter.Enabled = true;
                        this.gbFilter.Visible = true;
                    }
                }
            }
            else
            {
                TyQuery query = node.Tag as TyQuery;
                if (query != null)
                {
                    this.AddConditionFieldToCmb(query);
                    if (query.QueryWheres.Count > 0)
                    {
                        this._criteriaBusiness.AddNewCondtion(this.cmbrs_cnd.Items[0].ToString(), this.cmbrs_fld.Items[0].ToString(), this.cmbrs_type.Items[0].ToString(), this.cmbrs_tableName.Items[0].ToString());
                    }
                    else
                    {
                        MessageBox.Show("没有设置任何可作为条件的字段");
                    }
                }
                else
                {
                    trfilter = node.Tag as TrFilter;
                    if (trfilter != null)
                    {
                        this.AddConditionFieldToCmb(trfilter.TyQuery);
                        if (trfilter.TrFilterConditions.Count > 0)
                        {
                            this._criteriaBusiness.AddNewCondition(trfilter);
                        }
                        this.txtFilterName.Text = node.Text;
                    }
                }             
            }
            if (SkyMap.Net.Security.SecurityUtil.GetSmPrincipal().IsInRole(this.GetType().FullName))
            {
                this.gbFilter.Visible = true;
                this.bt_SaveFilter.Enabled = true;
                this.bt_DelFilter.Enabled = true;
            }
            else
            {
                this.gbFilter.Visible = false;
                this.bt_SaveFilter.Enabled = false;
                this.bt_DelFilter.Enabled =  false;
            }
        }

        private void bt_DelFilter_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                TreeNode selectedNode = this.tv_List.SelectedNode;
                if (this.IsFilterXNode(selectedNode))
                {
                    if (selectedNode.Tag is TrFilter)
                    {
                        this._criteriaBusiness.DeleteFilterX(selectedNode.Tag as TrFilter);
                        selectedNode.Remove();
                    }
                    else if (selectedNode.Tag is DataRow)
                    {
                        this._criteriaBusiness.DeleteFilterX(selectedNode.Tag as DataRow);
                        selectedNode.Remove();
                    }
                    else
                    {
                        throw new Exception("未知类型！");
                    }
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void bt_SaveFilter_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                this.lv_condition.PostEditor();
                this.BindingContext[this.lv_condition.DataSource].EndCurrentEdit();
                this._criteriaBusiness.CheckDtCondition();
                TreeNode selectedNode = this.tv_List.SelectedNode;
                DataRow searchX = null;
                DataRow filterX = null;

                TyQuery tyquery = null;
                TrFilter trfilter = null;

                searchX = this.GetSearchXRow();
                if (this.IsFilterXNode(selectedNode))
                {
                    filterX = selectedNode.Tag as DataRow;
                }
                if (searchX != null)
                {
                    this._criteriaBusiness.SaveFilterX(searchX, ref filterX, this.txtFilterName.Text);
                    if (this.IsFilterXNode(selectedNode))
                    {
                        selectedNode.Text = this.txtFilterName.Text;
                    }
                    else
                    {
                        this.AddChildNode(selectedNode, "NAME", filterX);
                    }
                }
                //=====2010 0802 Add
                else
                {
                    tyquery = this.GetTyQuery();
                    trfilter = this.GetTrFilter();
                    if (tyquery != null)
                    {
                        this._criteriaBusiness.SaveFilterX(tyquery, ref trfilter, this.txtFilterName.Text);
                        if (this.IsFilterXNode(selectedNode))
                        {
                            selectedNode.Text = this.txtFilterName.Text;
                        }
                        else
                        {
                            this.AddChildNode(selectedNode, trfilter);
                        }
                    }
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void BtDelRowClick(object sender, EventArgs e)
        {
            if (this.lv_condition.Selection.Count > 0)
            {
                try
                {
                    TreeListNode node = this.lv_condition.Selection[0];
                    this.lv_condition.PostEditor();
                    this.BindingContext[this.lv_condition.DataSource].EndCurrentEdit();
                    this.lv_condition.DataSource = null;
                    this.lv_condition.CustomNodeCellEdit -= new DevExpress.XtraTreeList.GetCustomNodeCellEditEventHandler(this.lv_condition_GetCustomNodeCellEdit);
                    this._criteriaBusiness.DtCondition.Rows.Remove(this._criteriaBusiness.DtCondition.Rows[node.Id]);
                    this.lv_condition.DataSource = this._criteriaBusiness.DtCondition;
                    this.lv_condition.CustomNodeCellEdit += new DevExpress.XtraTreeList.GetCustomNodeCellEditEventHandler(this.lv_condition_GetCustomNodeCellEdit);
                }
                catch (ApplicationException exception)
                {
                    throw exception;
                }
            }
        }

        protected virtual void BtOkClick(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                DataSet dataSet;
                CriteriaEventArgs args;
                this.lv_condition.PostEditor();
                this.BindingContext[this.lv_condition.DataSource].EndCurrentEdit();
                this._criteriaBusiness.CheckDtCondition();
                if (!this.IsQueryNode(this.tv_List.SelectedNode) && GetTyQuery() == null )
                {
                    DataRow searchXRow = this.GetSearchXRow();
                    if (searchXRow != null)
                    {
                        dataSet = this._criteriaBusiness.GetDataSet(searchXRow);
                        args = new CriteriaEventArgs();
                        args.DataSet = dataSet;
                        this.Criteria(this, args);
                    }
                }
                else
                {
                    TyQuery tag = GetTyQuery();// this.tv_List.SelectedNode.Tag as TyQuery; 
                    string str = string.Format(this._criteriaBusiness.ParsePreCondition(tag.Sql), QueryClientHelper.GetWhereByConditions(this._criteriaBusiness.DtCondition));
                    dataSet = RemotingSingletonProvider<CriteriaDAOService>.Instance.Execute(tag.DataSource.Id, new string[] { str }, new string[] { tag.Name });
                    dataSet.DataSetName = tag.Name;
                    args = new CriteriaEventArgs();
                    args.DataSet = dataSet;
                    this.Criteria(this, args);
                }
            }
            catch (ConditionInputException exception)
            {
                MessageService.ShowMessage(exception.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void ConditionNameChanged(object sender, EventArgs e)
        {
            TreeListNode focusedNode = this.lv_condition.FocusedNode;
            if (focusedNode != null)
            {
                this.lv_condition.PostEditor();
                try
                {
                    int index = this.cmbrs_cnd.Items.IndexOf(focusedNode.GetValue(this.col_condition));
                    focusedNode.SetValue(this.col_fieldid, this.cmbrs_fld.Items[index]);
                    focusedNode.SetValue(this.col_type, this.cmbrs_type.Items[index]);
                    if (this.IsQueryNode(this.tv_List.SelectedNode))
                    {
                        LoggingService.DebugFormatted("所选字段：{0} 表为：{1}", new object[] { focusedNode.GetValue(this.col_condition), this.cmbrs_tableName.Items[index] });
                        focusedNode.SetValue(this.col_tableName, this.cmbrs_tableName.Items[index]);
                    }
                }
                catch (Exception exception)
                {
                    LoggingService.Error(exception);
                }
                this.lv_condition.PostEditor();
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

        private ICommand GetCommandByPath(string typeName, object owner)
        {
            if (!string.IsNullOrEmpty(typeName))
            {
                return (Activator.CreateInstance(System.Type.GetType(typeName)) as ICommand);
            }
            return null;
        }

        private DataRow GetSearchXRow()
        {
            TreeNode selectedNode = this.tv_List.SelectedNode;
            if (this.IsSearchXNode(selectedNode))
            {
                return (selectedNode.Tag as DataRow);
            }
            if (this.IsFilterXNode(selectedNode))
            {
                return (selectedNode.Parent.Tag as DataRow);
            }
            return null;
        }

        private TyQuery GetTyQuery()
        {
            TreeNode selectedNode = this.tv_List.SelectedNode;
            if (this.IsQueryNode(selectedNode))
            {
                return (selectedNode.Tag as TyQuery);
            }
            if (this.IsTrFilterNode(selectedNode))
            {
                return (selectedNode.Parent.Tag as TyQuery);
            }
            return null;
        }

        //private TyQuery GetTyQuery()
        //{
        //    TreeNode selectedNode = this.tv_List.SelectedNode;
        //    if (selectedNode.Tag is TyQuery)
        //    {
        //        return (selectedNode.Tag as TyQuery);
        //    }
        //    return null;
        //}

        private TrFilter GetTrFilter()
        {
            TreeNode selectedNode = this.tv_List.SelectedNode;
            if (selectedNode.Tag is TrFilter)
            {
                return (selectedNode.Tag as TrFilter);
            }
            return null;
        }

        private void GridEditorsAdjust()
        {
            this.lv_condition.BeginUpdate();
            RepositoryItemComboBox box = new RepositoryItemComboBox();
            this.cmbrs_cnd = new RepositoryItemComboBox();
            this.cmbrs_fld = new RepositoryItemComboBox();
            this.cmbrs_type = new RepositoryItemComboBox();
            RepositoryItemComboBox box2 = new RepositoryItemComboBox();
            this.cmbrs_tableName = new RepositoryItemComboBox();
            this.txtrs_val = new RepositoryItemTextEdit();
            this.txtrs_cbx = new RepositoryItemComboBox();
            this.dtrs_val = new RepositoryItemDateEdit();
            box.Items.AddRange(new object[] { "且", "或" });
            this.cmbrs_fld.AutoHeight = false;
            box2.AutoHeight = false;
            box2.Items.AddRange(new object[] { "包含", "等于", "大于", "小于", "不等于", "大于或等于", "小于或等于" });
            this.col_op.FieldName = "Operation";
            this.col_Rel.FieldName = "Relation";
            this.col_value.FieldName = "Compare_Value";
            this.col_condition.FieldName = "Condition_Name";
            this.col_fieldid.FieldName = "Field_ID";
            this.col_type.FieldName = "Field_Type";
            this.col_tableName.FieldName = "TABLE_NAME";
            this.col_Rel.ColumnEdit = box;
            this.col_condition.ColumnEdit = this.cmbrs_cnd;
            this.col_fieldid.ColumnEdit = this.cmbrs_fld;
            this.col_type.ColumnEdit = this.cmbrs_type;
            this.col_op.ColumnEdit = box2;
            this.txtrs_val.Validating += new CancelEventHandler(this.ValueValidating);
            this.cmbrs_cnd.SelectedIndexChanged += new EventHandler(this.ConditionNameChanged);
            this.lv_condition.EndUpdate();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GeneralCriteria));
            this.pnlMain = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lv_condition = new DevExpress.XtraTreeList.TreeList();
            this.col_Rel = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.col_condition = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.col_op = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.col_value = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.col_fieldid = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.col_type = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.col_tableName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.bt_DelFilter = new System.Windows.Forms.Button();
            this.bt_SaveFilter = new System.Windows.Forms.Button();
            this.txtFilterName = new System.Windows.Forms.TextBox();
            this.bt_DelRow = new System.Windows.Forms.Button();
            this.bt_Ok = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tv_List = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.pnlMain.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lv_condition)).BeginInit();
            this.panel1.SuspendLayout();
            this.gbFilter.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.Transparent;
            this.pnlMain.Controls.Add(this.panel2);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(696, 167);
            this.pnlMain.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.splitter1);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(696, 167);
            this.panel2.TabIndex = 10;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.Control;
            this.panel4.Controls.Add(this.lv_condition);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(155, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(405, 167);
            this.panel4.TabIndex = 10;
            // 
            // lv_condition
            // 
            this.lv_condition.Appearance.EvenRow.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lv_condition.Appearance.EvenRow.Options.UseFont = true;
            this.lv_condition.Appearance.OddRow.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lv_condition.Appearance.OddRow.Options.UseFont = true;
            this.lv_condition.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            this.lv_condition.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.col_Rel,
            this.col_condition,
            this.col_op,
            this.col_value,
            this.col_fieldid,
            this.col_type,
            this.col_tableName});
            this.lv_condition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lv_condition.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lv_condition.IndicatorWidth = 4;
            this.lv_condition.Location = new System.Drawing.Point(0, 0);
            this.lv_condition.LookAndFeel.UseDefaultLookAndFeel = false;
            this.lv_condition.Name = "lv_condition";
            this.lv_condition.RootValue = "0";
            this.lv_condition.Size = new System.Drawing.Size(405, 167);
            this.lv_condition.TabIndex = 2;
            this.lv_condition.TreeLineStyle = DevExpress.XtraTreeList.LineStyle.None;
            this.lv_condition.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvMouseDown);
            this.lv_condition.CustomNodeCellEdit += new DevExpress.XtraTreeList.GetCustomNodeCellEditEventHandler(this.lv_condition_GetCustomNodeCellEdit);
            this.lv_condition.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lv_condition_KeyUp);
            // 
            // col_Rel
            // 
            this.col_Rel.Caption = "关系";
            this.col_Rel.FieldName = "关系";
            this.col_Rel.Name = "col_Rel";
            this.col_Rel.OptionsColumn.AllowSort = false;
            this.col_Rel.Visible = true;
            this.col_Rel.VisibleIndex = 0;
            // 
            // col_condition
            // 
            this.col_condition.Caption = "字段";
            this.col_condition.FieldName = "字段";
            this.col_condition.Name = "col_condition";
            this.col_condition.OptionsColumn.AllowSort = false;
            this.col_condition.Visible = true;
            this.col_condition.VisibleIndex = 1;
            this.col_condition.Width = 148;
            // 
            // col_op
            // 
            this.col_op.Caption = "比较";
            this.col_op.FieldName = "比较";
            this.col_op.Name = "col_op";
            this.col_op.OptionsColumn.AllowSort = false;
            this.col_op.Visible = true;
            this.col_op.VisibleIndex = 2;
            this.col_op.Width = 119;
            // 
            // col_value
            // 
            this.col_value.Caption = "比较值";
            this.col_value.FieldName = "比较值";
            this.col_value.Name = "col_value";
            this.col_value.OptionsColumn.AllowSort = false;
            this.col_value.Visible = true;
            this.col_value.VisibleIndex = 3;
            this.col_value.Width = 120;
            // 
            // col_fieldid
            // 
            this.col_fieldid.Name = "col_fieldid";
            this.col_fieldid.OptionsColumn.AllowSort = false;
            // 
            // col_type
            // 
            this.col_type.Name = "col_type";
            this.col_type.OptionsColumn.AllowSort = false;
            // 
            // col_tableName
            // 
            this.col_tableName.Name = "col_tableName";
            this.col_tableName.OptionsColumn.AllowSort = false;
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.SystemColors.Control;
            this.splitter1.Location = new System.Drawing.Point(152, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 167);
            this.splitter1.TabIndex = 11;
            this.splitter1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.gbFilter);
            this.panel1.Controls.Add(this.bt_DelRow);
            this.panel1.Controls.Add(this.bt_Ok);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(560, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(136, 167);
            this.panel1.TabIndex = 7;
            // 
            // gbFilter
            // 
            this.gbFilter.BackColor = System.Drawing.Color.Transparent;
            this.gbFilter.Controls.Add(this.bt_DelFilter);
            this.gbFilter.Controls.Add(this.bt_SaveFilter);
            this.gbFilter.Controls.Add(this.txtFilterName);
            this.gbFilter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbFilter.Location = new System.Drawing.Point(0, 67);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(136, 100);
            this.gbFilter.TabIndex = 2;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "保存当明查询条件";
            this.gbFilter.Visible = false;
            // 
            // bt_DelFilter
            // 
            this.bt_DelFilter.Enabled = false;
            this.bt_DelFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_DelFilter.Location = new System.Drawing.Point(64, 64);
            this.bt_DelFilter.Name = "bt_DelFilter";
            this.bt_DelFilter.Size = new System.Drawing.Size(48, 24);
            this.bt_DelFilter.TabIndex = 2;
            this.bt_DelFilter.Text = "删除";
            this.bt_DelFilter.Click += new System.EventHandler(this.bt_DelFilter_Click);
            // 
            // bt_SaveFilter
            // 
            this.bt_SaveFilter.Enabled = false;
            this.bt_SaveFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_SaveFilter.Location = new System.Drawing.Point(8, 64);
            this.bt_SaveFilter.Name = "bt_SaveFilter";
            this.bt_SaveFilter.Size = new System.Drawing.Size(48, 24);
            this.bt_SaveFilter.TabIndex = 1;
            this.bt_SaveFilter.Text = "保存";
            this.bt_SaveFilter.Click += new System.EventHandler(this.bt_SaveFilter_Click);
            // 
            // txtFilterName
            // 
            this.txtFilterName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFilterName.Location = new System.Drawing.Point(8, 32);
            this.txtFilterName.Name = "txtFilterName";
            this.txtFilterName.Size = new System.Drawing.Size(100, 21);
            this.txtFilterName.TabIndex = 0;
            // 
            // bt_DelRow
            // 
            this.bt_DelRow.BackColor = System.Drawing.Color.Transparent;
            this.bt_DelRow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_DelRow.Location = new System.Drawing.Point(8, 35);
            this.bt_DelRow.Name = "bt_DelRow";
            this.bt_DelRow.Size = new System.Drawing.Size(67, 24);
            this.bt_DelRow.TabIndex = 1;
            this.bt_DelRow.Text = "删除行";
            this.bt_DelRow.UseVisualStyleBackColor = false;
            this.bt_DelRow.Click += new System.EventHandler(this.BtDelRowClick);
            // 
            // bt_Ok
            // 
            this.bt_Ok.BackColor = System.Drawing.Color.Transparent;
            this.bt_Ok.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bt_Ok.Location = new System.Drawing.Point(8, 3);
            this.bt_Ok.Name = "bt_Ok";
            this.bt_Ok.Size = new System.Drawing.Size(67, 24);
            this.bt_Ok.TabIndex = 0;
            this.bt_Ok.Text = "确定";
            this.bt_Ok.UseVisualStyleBackColor = false;
            this.bt_Ok.Click += new System.EventHandler(this.BtOkClick);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.Control;
            this.panel3.Controls.Add(this.tv_List);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(152, 167);
            this.panel3.TabIndex = 9;
            // 
            // tv_List
            // 
            this.tv_List.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tv_List.Font = new System.Drawing.Font("SimSun", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tv_List.HideSelection = false;
            this.tv_List.ImageIndex = 0;
            this.tv_List.ImageList = this.imageList1;
            this.tv_List.Location = new System.Drawing.Point(0, 0);
            this.tv_List.Name = "tv_List";
            this.tv_List.SelectedImageIndex = 0;
            this.tv_List.Size = new System.Drawing.Size(152, 167);
            this.tv_List.TabIndex = 1;
            this.tv_List.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.AfterSelectCxNode);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "");
            this.imageList1.Images.SetKeyName(2, "");
            // 
            // GeneralCriteria
            // 
            this.Controls.Add(this.pnlMain);
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Name = "GeneralCriteria";
            this.Size = new System.Drawing.Size(696, 167);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GeneralCriteria_KeyUp);
            this.pnlMain.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lv_condition)).EndInit();
            this.panel1.ResumeLayout(false);
            this.gbFilter.ResumeLayout(false);
            this.gbFilter.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private bool IsFilterXNode(TreeNode node)
        {
            return (((node.Parent != null) && (node.Parent.Parent != null)) && node.Parent.Parent.Tag.Equals("simple"));
        }

        private bool IsQueryNode(TreeNode node)
        {
            return ((node != null) && (node.Tag is TyQuery));
        }

        private bool IsTrFilterNode(TreeNode node)
        {
            return (((node.Parent != null) && (node.Parent.Parent != null)) && node.Parent.Parent.Tag.Equals("simple")) && (node.Tag is TrFilter);
        }

        private bool IsSearchXNode(TreeNode node)
        {
            return ((node.Parent != null) && node.Parent.Tag.Equals("simple"));
        }

        protected virtual void LoadCxTree()
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.Info("开始载入通用查询树");
            }
            this.LoadSimpleCxTree();
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.Info("载入查询树完成");
            }
        }

        private void LoadNewQuerys(TreeNode pnode)
        {
            try
            {
                bool isCanAccessAll = QueryClientHelper.IsCanAccessAll;
                IList<string> authQuerys = null;
                if (!isCanAccessAll)
                {
                    authQuerys = QueryClientHelper.GetAuthQuerys();
                }
                foreach (TyQuery query in QueryClientHelper.TyQuerys)
                {
                    if (isCanAccessAll || ((authQuerys != null) && authQuerys.Contains(query.Id)))
                    {
                        TreeNode node = this.AddChildNode(pnode, query);
                        node.ImageIndex = 1;
                        node.SelectedImageIndex = 1;

                        foreach (TrFilter trfilter in query.TrFilters)
                        {
                                TreeNode node1 = this.AddChildNode(node, trfilter);
                                node1.ImageIndex = 2;
                                node1.SelectedImageIndex = 2;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
            }
        }

        protected void LoadSimpleCxTree()
        {
            try
            {
                TreeNode pnode = this.tv_List.Nodes.Add("简单查询");
                pnode.ImageIndex = 0;
                pnode.Tag = "simple";
                bool isCanAccessAll = this._criteriaBusiness.IsCanAccessAll;
                IList<string> authQuerys = null;
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.Info("开始获取通用查询权限...");
                }
                if (!isCanAccessAll)
                {
                    authQuerys = this._criteriaBusiness.GetAuthQuerys();
                }
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.Info("已获取权限...");
                }
                if (LoggingService.IsInfoEnabled)
                {
                    LoggingService.Info("开始遍历查询树,结合查询权限加载查询树...");
                }
                foreach (DataRow row in this._criteriaBusiness.DtSearchX.Rows)
                {
                    if (isCanAccessAll || authQuerys.Contains(row["id"].ToString()))
                    {
                        TreeNode node2 = this.AddChildNode(pnode, "name", row);
                        node2.ImageIndex = 1;
                        node2.SelectedImageIndex = 1;
                        foreach (DataRow row2 in this._criteriaBusiness.GetFilterChildRows(row))
                        {
                            TreeNode node3 = this.AddChildNode(node2, "NAME", row2);
                            node3.ImageIndex = 2;
                            node3.SelectedImageIndex = 2;
                        }
                    }
                }
                this.LoadNewQuerys(pnode);
                pnode.Expand();
                if (pnode.Nodes.Count > 0)
                {
                    this.tv_List.SelectedNode = pnode.Nodes[0];
                }
            }
            catch (ApplicationException exception)
            {
                throw exception;
            }
        }

        private void lv_condition_GetCustomNodeCellEdit(object sender, GetCustomNodeCellEditEventArgs e)
        {
            if(e.Column.Caption=="比较值") //(e.Column == this.col_value)
            {
                string s = (string)e.Node.GetValue(this.col_type);
                if (s == null)
                {
                    s = string.Empty;
                }
                s = s.ToLower();

                if (!(StringHelper.IsNull(s) || ((s.IndexOf("date") <= -1) && !(s == ColumnType.日期.ToString()))))
                {
                    e.RepositoryItem = this.dtrs_val;
                    this.dtrs_val.Mask.EditMask = "d";
                    this.dtrs_val.Mask.UseMaskAsDisplayFormat = true;
                }
                else if (s == "值列表")
                {
                    string strlist = "";
                    this.col_value.ColumnEdit = null;
                    this.txtrs_cbx.Items.Clear();
                    foreach (TyQuery query in QueryClientHelper.TyQuerys)
                    {
                        foreach (TyQueryWhere tw in query.QueryWheres)
                        {
                            if (tw.Description == (string)e.Node.GetValue(1) && tw.TableName == (string)e.Node.GetValue(6))
                            {
                                strlist = tw.DefaultValue;
                                break;
                            }
                        }
                        break;
                    }
                    this.txtrs_cbx.Items.AddRange(strlist.Split(new char[] { ',' }));
                    this.txtrs_cbx.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                    e.RepositoryItem = this.txtrs_cbx;
                }
                else
                {
                    e.RepositoryItem = this.txtrs_val;
                }
                
            }
        }

        protected virtual void lvMouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                this.lv_condition.PostEditor();
                Point pt = new Point(e.X, e.Y);
                if ((this.lv_condition.CalcHitInfo(pt).Node == null) && ((this.cmbrs_fld != null) && (this.cmbrs_fld.Items.Count != 0)))
                {
                    TreeListNode node = null;
                    if (this.lv_condition.Nodes.Count > 0)
                    {
                        node = this.lv_condition.Nodes[this.lv_condition.Nodes.Count - 1];
                    }
                    if ((node == null) || ((((node.GetValue(1).ToString().Trim() != "") && (node.GetValue(2).ToString().Trim() != "")) || ((node.GetValue(1).ToString().Trim() != "") && (node.GetValue(3).ToString().Trim() != ""))) || ((node.GetValue(2).ToString().Trim() != "") && (node.GetValue(3).ToString().Trim() != ""))))
                    {
                        this._criteriaBusiness.AddNewCondtion(this.cmbrs_cnd.Items[0].ToString(), this.cmbrs_fld.Items[0].ToString(), this.cmbrs_type.Items[0].ToString(), this.cmbrs_tableName.Items[0].ToString());

                    }
                }
                else
                {
                   
                    this.lv_condition.BeginUpdate();                   
                    this.lv_condition.EndUpdate();                      
                }
            }
            catch (ApplicationException exception)
            {
                throw exception;
            }
        }

        public ICommand OpenDetailCommand(object owner)
        {
            if (this.IsQueryNode(this.tv_List.SelectedNode))
            {
                TyQuery tag = this.tv_List.SelectedNode.Tag as TyQuery;
                return this.GetCommandByPath(tag.DetailPage, owner);
            }
            return null;
        }

        public ICommand OpenFormCommand(object owner)
        {
            if (this.IsQueryNode(this.tv_List.SelectedNode))
            {
                TyQuery tag = this.tv_List.SelectedNode.Tag as TyQuery;
                return this.GetCommandByPath(tag.FormPage, owner);
            }
            return null;
        }

        protected void SecurityUtil_CurrentPrincipalChanged(object sender, EventArgs e)
        {
            this.tv_List.Nodes.Clear();
            this.LoadSimpleCxTree();
        }

        private void ValueValidating(object sender, CancelEventArgs e)
        {
            TreeListNode focusedNode = this.lv_condition.FocusedNode;
            if (focusedNode != null)
            {
                string str = (string) focusedNode.GetValue(this.col_type);
                if (str != null)
                {
                    str = str.ToLower();
                    TextEdit edit = sender as TextEdit;
                    string editValue = (string) edit.EditValue;
                    if ((editValue != null) && (editValue.Length != 0))
                    {
                        if (str.Equals("double") || (str == ColumnType.数字.ToString()))
                        {
                            try
                            {
                                double.Parse(editValue);
                            }
                            catch
                            {
                                edit.ErrorText = "输入了无效的数字！";
                                edit.EditValue = "";
                                e.Cancel = true;
                            }
                        }
                        else if (str.IndexOf("int") > -1)
                        {
                            try
                            {
                                int.Parse(editValue);
                            }
                            catch
                            {
                                edit.ErrorText = "输入了效的整数！";
                                edit.EditValue = "";
                                e.Cancel = true;
                            }
                        }
                        else if (str.Equals("long"))
                        {
                            try
                            {
                                long.Parse(editValue);
                            }
                            catch
                            {
                                edit.ErrorText = "输入了无效的整数！";
                                edit.EditValue = "";
                                e.Cancel = true;
                            }
                        }
                    }
                }
            }
        }

        public SkyMap.Net.Criteria.Client.CriteriaBusiness CriteriaBusiness
        {
            set
            {
                this._criteriaBusiness = value;
                this.lv_condition.DataSource = this._criteriaBusiness.DtCondition;
                this.LoadCxTree();
            }
        }

        private void lv_condition_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtOkClick(this, new EventArgs());
            }
        }

        private void GeneralCriteria_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtOkClick(this, new EventArgs());
            }
        }
    }
}

