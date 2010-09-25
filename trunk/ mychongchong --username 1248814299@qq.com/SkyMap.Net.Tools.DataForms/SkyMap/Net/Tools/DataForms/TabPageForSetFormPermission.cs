namespace SkyMap.Net.Tools.DataForms
{
    using DevExpress.Data;
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraGrid;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Base;
    using DevExpress.XtraGrid.Views.Grid;
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.DataForms;
    using SkyMap.Net.Gui.Components;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class TabPageForSetFormPermission : SmUserControl
    {
        private CheckEdit chkDel;
        private ComboBoxEdit cmbDataForms;
        private ComboBoxEdit cmbPrintSets;
        private Container components = null;
        private DAODataForm daoDataForm;
        private SkyMap.Net.DataForms.FormPermission formPermission;
        private GridControl gcDataControls;
        private GridView gvDataControls;
        private Label label1;
        private Label label2;
        private Label label5;
        private Label lblPL;
        private List<string> lstDisables;
        private List<string> lstInvisibles;
        private SpinEdit numPageIndex;

        public TabPageForSetFormPermission()
        {
            this.InitializeComponent();
            this.InitGridControl();
            this.cmbDataForms.SelectedIndexChanged += new EventHandler(this.cmbDataForms_SelectedIndexChanged);
            this.InitDataForms();
            this.InitPrintSets();
        }

        private void cmbDataForms_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.daoDataForm = (DAODataForm) this.cmbDataForms.SelectedItem;
            if (this.daoDataForm != null)
            {
                this.gcDataControls.DataSource = null;
                this.gcDataControls.DataSource = this.daoDataForm.DataControls;
                this.gvDataControls.BestFitColumns();
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

        private void gvDataControls_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            DataControl row = (DataControl) this.gvDataControls.GetRow(e.RowHandle);
            if (e.IsGetData)
            {
                if ((e.Column.FieldName == "disable") && this.lstDisables.Contains(row.Name))
                {
                    e.Value = true;
                }
                else if ((e.Column.FieldName == "invisible") && this.lstInvisibles.Contains(row.Name))
                {
                    e.Value = true;
                }
                else
                {
                    e.Value = false;
                }
            }
            else if (e.IsSetData)
            {
                bool flag2;
                if (e.Column.FieldName == "disable")
                {
                    flag2 = true;
                    if (!(!flag2.Equals(e.Value) || this.lstDisables.Contains(row.Name)))
                    {
                        this.lstDisables.Add(row.Name);
                    }
                    flag2 = false;
                    if (flag2.Equals(e.Value) && this.lstDisables.Contains(row.Name))
                    {
                        this.lstDisables.Remove(row.Name);
                    }
                }
                else if (e.Column.FieldName == "invisible")
                {
                    flag2 = true;
                    if (!(!flag2.Equals(e.Value) || this.lstInvisibles.Contains(row.Name)))
                    {
                        this.lstInvisibles.Add(row.Name);
                    }
                    flag2 = false;
                    if (flag2.Equals(e.Value) && this.lstInvisibles.Contains(row.Name))
                    {
                        this.lstInvisibles.Remove(row.Name);
                    }
                }
            }
        }

        private void InitDataForms()
        {
            IList<DAODataForm> collection = QueryHelper.List<DAODataForm>("ALL_DAODataForm");
            this.cmbDataForms.Properties.Items.Clear();
            this.cmbDataForms.Properties.Items.AddRange(new List<DAODataForm>(collection).ToArray());
        }

        private void InitGridControl()
        {
            this.gcDataControls.BeginUpdate();
            this.gvDataControls.OptionsView.ShowGroupPanel = true;
            GridColumn column = this.gvDataControls.Columns.AddField("disable");
            column.VisibleIndex = 0;
            column.Caption = "不可控制";
            column.UnboundType = UnboundColumnType.Boolean;
            column.OptionsColumn.AllowEdit = true;
            GridColumn column2 = this.gvDataControls.Columns.AddField("invisible");
            column2.VisibleIndex = 1;
            column2.Caption = "不可见";
            column2.UnboundType = UnboundColumnType.Boolean;
            column2.OptionsColumn.AllowEdit = true;
            string[] strArray = new string[] { "Name", "MapTable", "MapColumn", "Type", "Description", "DisplayOrder" };
            string[] strArray2 = new string[] { "名称", "关联表", "关联字段", "类型", "备注", "显示顺序" };
            int length = strArray.Length;
            GridColumn[] columns = new GridColumn[length];
            for (int i = 0; i < length; i++)
            {
                columns[i] = new GridColumn();
                columns[i].FieldName = strArray[i];
                columns[i].Caption = strArray2[i];
                columns[i].VisibleIndex = i + 2;
                columns[i].OptionsColumn.AllowEdit = false;
            }
            this.gvDataControls.Columns.AddRange(columns);
            this.gvDataControls.OptionsView.ColumnAutoWidth = false;
            this.gvDataControls.CustomUnboundColumnData += new CustomColumnDataEventHandler(this.gvDataControls_CustomUnboundColumnData);
            this.gcDataControls.EndUpdate();
        }

        private void InitializeComponent()
        {
            this.label5 = new Label();
            this.label1 = new Label();
            this.lblPL = new Label();
            this.cmbPrintSets = new ComboBoxEdit();
            this.numPageIndex = new SpinEdit();
            this.label2 = new Label();
            this.cmbDataForms = new ComboBoxEdit();
            this.gcDataControls = new GridControl();
            this.gvDataControls = new GridView();
            this.chkDel = new CheckEdit();
            this.cmbPrintSets.Properties.BeginInit();
            this.numPageIndex.Properties.BeginInit();
            this.cmbDataForms.Properties.BeginInit();
            this.gcDataControls.BeginInit();
            this.gvDataControls.BeginInit();
            this.chkDel.Properties.BeginInit();
            base.SuspendLayout();
            this.label5.Location = new Point(13, 0x3b);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x26, 80);
            this.label5.TabIndex = 4;
            this.label5.Text = "表单控件权限";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x1d2, 8);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x1f, 14);
            this.label1.TabIndex = 13;
            this.label1.Text = "页面";
            this.lblPL.AutoSize = true;
            this.lblPL.Location = new Point(13, 0x23);
            this.lblPL.Name = "lblPL";
            this.lblPL.Size = new Size(0x1f, 14);
            this.lblPL.TabIndex = 15;
            this.lblPL.Text = "报表";
            this.cmbPrintSets.Location = new Point(50, 0x20);
            this.cmbPrintSets.Name = "cmbPrintSets";
            this.cmbPrintSets.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.cmbPrintSets.Size = new Size(0x191, 0x15);
            this.cmbPrintSets.TabIndex = 0x10;
            int[] bits = new int[4];
            this.numPageIndex.EditValue = new decimal(bits);
            this.numPageIndex.Location = new Point(0x1f7, 5);
            this.numPageIndex.Name = "numPageIndex";
            this.numPageIndex.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton() });
            this.numPageIndex.Size = new Size(0x5e, 0x15);
            this.numPageIndex.TabIndex = 0x11;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(13, 8);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x1f, 14);
            this.label2.TabIndex = 0x12;
            this.label2.Text = "表单";
            this.cmbDataForms.Location = new Point(50, 5);
            this.cmbDataForms.Name = "cmbDataForms";
            this.cmbDataForms.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.cmbDataForms.Size = new Size(0x191, 0x15);
            this.cmbDataForms.TabIndex = 0x13;
            this.gcDataControls.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gcDataControls.EmbeddedNavigator.Name = "";
            this.gcDataControls.Location = new Point(50, 0x3b);
            this.gcDataControls.MainView = this.gvDataControls;
            this.gcDataControls.Name = "gcDataControls";
            this.gcDataControls.Size = new Size(0x243, 0x15d);
            this.gcDataControls.TabIndex = 0x16;
            this.gcDataControls.ViewCollection.AddRange(new BaseView[] { this.gvDataControls });
            this.gvDataControls.GridControl = this.gcDataControls;
            this.gvDataControls.Name = "gvDataControls";
            this.gvDataControls.OptionsSelection.MultiSelect = true;
            this.gvDataControls.OptionsView.ShowGroupedColumns = true;
            this.chkDel.Location = new Point(0x1d3, 0x20);
            this.chkDel.Name = "chkDel";
            this.chkDel.Properties.Caption = "能否删除";
            this.chkDel.RightToLeft = RightToLeft.No;
            this.chkDel.Size = new Size(0x4b, 0x13);
            this.chkDel.TabIndex = 0x17;
            base.Controls.Add(this.chkDel);
            base.Controls.Add(this.gcDataControls);
            base.Controls.Add(this.cmbDataForms);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.numPageIndex);
            base.Controls.Add(this.cmbPrintSets);
            base.Controls.Add(this.lblPL);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.label5);
            base.Location = new Point(4, 0x15);
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            base.Name = "TabPageForSetFormPermission";
            base.Size = new Size(0x278, 0x1a0);
            this.cmbPrintSets.Properties.EndInit();
            this.numPageIndex.Properties.EndInit();
            this.cmbDataForms.Properties.EndInit();
            this.gcDataControls.EndInit();
            this.gvDataControls.EndInit();
            this.chkDel.Properties.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void InitPrintSets()
        {
            IList<PrintSet> collection = QueryHelper.List<PrintSet>("ALL_PrintSet");
            this.cmbPrintSets.Properties.Items.Clear();
            this.cmbPrintSets.Properties.Items.AddRange(new List<PrintSet>(collection).ToArray());
        }

        private void JoinDisableInvisibleValues(out string disables, out string invisibles)
        {
            disables = string.Join(",", this.lstDisables.ToArray());
            invisibles = string.Join(",", this.lstInvisibles.ToArray());
        }

        public SkyMap.Net.DataForms.FormPermission FormPermission
        {
            get
            {
                if (this.daoDataForm != null)
                {
                    string str;
                    string str2;
                    this.formPermission.DaoDataFormId = this.daoDataForm.Id;
                    this.formPermission.DaoDataFormName = this.daoDataForm.Name;
                    if (!StringHelper.IsNull(this.daoDataForm.Description))
                    {
                        this.formPermission.DaoDataFormName = this.formPermission.DaoDataFormName + "(" + this.daoDataForm.Description + ")";
                    }
                    this.JoinDisableInvisibleValues(out str, out str2);
                    this.formPermission.UnableFrame = str;
                    this.formPermission.InVisibleFrame = str2;
                    this.formPermission.PageIndex = (int) this.numPageIndex.Value;
                    if (this.cmbPrintSets.SelectedItem != null)
                    {
                        this.formPermission.PrintSetId = (this.cmbPrintSets.SelectedItem as PrintSet).Id;
                        this.formPermission.PrintSetName = (this.cmbPrintSets.SelectedItem as PrintSet).Name;
                    }
                    else
                    {
                        this.formPermission.PrintSetId = string.Empty;
                        this.formPermission.PrintSetName = string.Empty;
                    }
                    this.formPermission.EnableDelete = this.chkDel.Checked;
                    return this.formPermission;
                }
                return null;
            }
            set
            {
                this.formPermission = value;
                this.lstDisables = new List<string>(StringHelper.Split(this.formPermission.UnableFrame));
                this.lstInvisibles = new List<string>(StringHelper.Split(this.formPermission.InVisibleFrame));
                this.cmbPrintSets.SelectedItem = null;
                if (!string.IsNullOrEmpty(this.formPermission.PrintSetId))
                {
                    foreach (PrintSet set in this.cmbPrintSets.Properties.Items)
                    {
                        if ((set != null) && (set.Id == this.formPermission.PrintSetId))
                        {
                            this.cmbPrintSets.SelectedItem = set;
                            break;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(this.formPermission.DaoDataFormId))
                {
                    foreach (DAODataForm form in this.cmbDataForms.Properties.Items)
                    {
                        if ((form != null) && (form.Id == this.formPermission.DaoDataFormId))
                        {
                            this.daoDataForm = form;
                            this.cmbDataForms.SelectedItem = form;
                            break;
                        }
                    }
                }
                this.numPageIndex.EditValue = this.formPermission.PageIndex;
                this.chkDel.Checked = this.formPermission.EnableDelete;
            }
        }
    }
}

