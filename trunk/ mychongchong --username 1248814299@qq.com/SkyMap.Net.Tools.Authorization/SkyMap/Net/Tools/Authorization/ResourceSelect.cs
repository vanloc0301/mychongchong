namespace SkyMap.Net.Tools.Authorization
{
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraGrid;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Grid;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Gui;
    using SkyMap.Net.OGM;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;

    public class ResourceSelect : Form
    {
        private CAuthType authType;
        private Button btnCancel;
        private Button btnOK;
        private Container components = null;
        private GridControl grid;
        private Panel panel1;
        private string resourceId;
        private string resourceName;
        private GridView view;

        public ResourceSelect()
        {
            this.InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            int[] selectedRows = this.view.GetSelectedRows();
            if (selectedRows.Length == 0)
            {
                MessageHelper.ShowInfo("没有选择资源！");
                base.DialogResult = DialogResult.None;
            }
            else
            {
                DataRow dataRow = this.view.GetDataRow(selectedRows[0]);
                this.resourceId = dataRow[this.authType.ResourceIdField].ToString();
                this.resourceName = dataRow[this.authType.ResourceNameField].ToString();
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

        private void GetResource()
        {
            string str = "select " + this.authType.ResourceIdField + "," + this.authType.ResourceNameField + " from " + this.authType.ResourceTable;
            this.grid.DataSource = QueryHelper.ExecuteSql(typeof(CAuthType).Namespace,"", str);
        }

        private void InitGrid()
        {
            this.view.BeginUpdate();
            this.view.OptionsView.ShowGroupPanel = false;
            this.view.OptionsView.ShowHorzLines = true;
            this.view.OptionsView.ShowVertLines = true;
            string[] strArray = new string[] { this.authType.ResourceIdField, this.authType.ResourceNameField };
            string[] strArray2 = new string[] { "ID", "名称" };
            GridColumn[] columns = new GridColumn[strArray.Length];
            for (int i = 0; i < columns.Length; i++)
            {
                columns[i] = new GridColumn();
                columns[i].FieldName = strArray[i];
                columns[i].Caption = strArray2[i];
                columns[i].VisibleIndex = i;
            }
            this.view.Columns.AddRange(columns);
            RepositoryItemTextEdit edit = new RepositoryItemTextEdit();
            edit.ReadOnly = true;
            columns[0].ColumnEdit = edit;
            columns[1].ColumnEdit = edit;
            this.view.EndUpdate();
        }

        private void InitializeComponent()
        {
            this.grid = new GridControl();
            this.view = new GridView();
            this.panel1 = new Panel();
            this.btnCancel = new Button();
            this.btnOK = new Button();
            this.grid.BeginInit();
            this.view.BeginInit();
            this.panel1.SuspendLayout();
            base.SuspendLayout();
            this.grid.Dock = DockStyle.Fill;
            this.grid.EmbeddedNavigator.Name = "";
            this.grid.Location = new Point(0, 0);
            this.grid.MainView = this.view;
            this.grid.Name = "grid";
            this.grid.Size = new Size(0x124, 0x111);
            this.grid.TabIndex = 0;
            this.grid.Text = "gridControl1";
            this.view.GridControl = this.grid;
            this.view.Name = "view";
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Dock = DockStyle.Bottom;
            this.panel1.Location = new Point(0, 0xe1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x124, 0x30);
            this.panel1.TabIndex = 1;
            this.btnCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0x98, 10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x48, 0x18);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "取消";
            this.btnOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.Location = new Point(0x38, 10);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(80, 0x18);
            this.btnOK.TabIndex = 11;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            base.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new Size(6, 14);
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x124, 0x111);
            base.Controls.Add(this.panel1);
            base.Controls.Add(this.grid);
            base.Name = "ResourceSelect";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "ResourceSelect";
            this.grid.EndInit();
            this.view.EndInit();
            this.panel1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        public CAuthType AuthType
        {
            set
            {
                this.authType = value;
                this.InitGrid();
                this.GetResource();
                this.Text = "选择" + value.ResourceName;
            }
        }

        public string ResourceId
        {
            get
            {
                return this.resourceId;
            }
        }

        public string ResourceName
        {
            get
            {
                return this.resourceName;
            }
        }
    }
}

