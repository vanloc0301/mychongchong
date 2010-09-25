namespace SkyMap.Net.Tools.Organize
{
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Base;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Gui.Components;
    using SkyMap.Net.OGM;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;

    public class fSelectParticipant : SmForm
    {
        private Button btnCancel;
        private Button btnOK;
        private IContainer components;
        private SmGridControl grid;
        private ImageList imageList1;
        private string mID;
        private string mName;
        private Panel panel1;
        private string participantIdValue;
        private string participantType;
        private SmGridView view;

        public fSelectParticipant()
        {
            this.InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.mID = null;
            this.mName = null;
            base.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            List<CParticipant> selectedOrFocusedRows = this.view.GetSelectedOrFocusedRows<CParticipant>();
            if ((selectedOrFocusedRows != null) && (selectedOrFocusedRows.Count == 1))
            {
                CParticipant participant = selectedOrFocusedRows[0];
                this.mID = participant.Id;
                this.mName = participant.Name;
                this.participantType = participant.ParticipantEntity.Type;
                this.participantIdValue = participant.ParticipantEntity.IdValue;
                base.Close();
            }
            else
            {
                MessageHelper.ShowInfo("你没有选择参与者！");
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

        private void fSelectParticipant_Load(object sender, EventArgs e)
        {
            this.InitGridControl();
            this.RefreshList();
        }

        private void fSelectParticipant_Shown(object sender, EventArgs e)
        {
            if (this.mID != null)
            {
                for (int i = 0; i < this.view.RowCount; i++)
                {
                    CParticipant row = this.view.GetRow<CParticipant>(i);
                    if ((row != null) && (row.Id == this.mID))
                    {
                        this.view.SelectRow(i);
                        this.view.FocusedRowHandle = i;
                        this.view.MakeRowVisible(i, true);
                        break;
                    }
                }
            }
        }

        private void InitGridControl()
        {
            this.view.BeginUpdate();
            this.view.OptionsView.ShowGroupPanel = true;
            string[] strArray = new string[] { "Id", "Name", "ParticipantEntity", "Description" };
            string[] strArray2 = new string[] { "Id", "名称", "实体类型", "备注" };
            int length = strArray.Length;
            GridColumn[] columns = new GridColumn[length];
            for (int i = 0; i < length; i++)
            {
                columns[i] = new GridColumn();
                columns[i].FieldName = strArray[i];
                columns[i].Caption = strArray2[i];
                columns[i].VisibleIndex = i;
            }
            this.view.Columns.AddRange(columns);
            this.view.OptionsBehavior.Editable = false;
            this.view.OptionsView.ColumnAutoWidth = true;
            this.view.OptionsView.ShowAutoFilterRow = true;
            this.view.EndUpdate();
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(fSelectParticipant));
            this.imageList1 = new ImageList(this.components);
            this.panel1 = new Panel();
            this.btnCancel = new Button();
            this.btnOK = new Button();
            this.grid = new SmGridControl();
            this.view = new SmGridView();
            this.panel1.SuspendLayout();
            this.grid.BeginInit();
            this.view.BeginInit();
            base.SuspendLayout();
            this.imageList1.ImageStream = (ImageListStreamer) manager.GetObject("imageList1.ImageStream");
            this.imageList1.TransparentColor = Color.Fuchsia;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "");
            this.imageList1.Images.SetKeyName(2, "");
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Dock = DockStyle.Bottom;
            this.panel1.Location = new Point(0, 0x1a6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x21e, 0x33);
            this.panel1.TabIndex = 11;
            this.btnCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnCancel.Location = new Point(460, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x48, 0x1a);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.btnOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnOK.Location = new Point(0x174, 12);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(80, 0x1a);
            this.btnOK.TabIndex = 11;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.grid.Dock = DockStyle.Fill;
            this.grid.EmbeddedNavigator.Name = "";
            this.grid.ImeMode = ImeMode.NoControl;
            this.grid.Location = new Point(0, 0);
            this.grid.MainView = this.view;
            this.grid.Name = "grid";
            this.grid.Size = new Size(0x21e, 0x1a6);
            this.grid.TabIndex = 12;
            this.grid.ViewCollection.AddRange(new BaseView[] { this.view });
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
            this.AutoScaleBaseSize = new Size(6, 15);
            base.ClientSize = new Size(0x21e, 0x1d9);
            base.ControlBox = false;
            base.Controls.Add(this.grid);
            base.Controls.Add(this.panel1);
            base.LookAndFeel.UseDefaultLookAndFeel = false;
            base.Name = "fSelectParticipant";
            this.Text = "请选择";
            base.Load += new EventHandler(this.fSelectParticipant_Load);
            base.Shown += new EventHandler(this.fSelectParticipant_Shown);
            this.panel1.ResumeLayout(false);
            this.grid.EndInit();
            this.view.EndInit();
            base.ResumeLayout(false);
        }

        public void RefreshList()
        {
            this.grid.DataSource = QueryHelper.List<CParticipant>("ALL_CParticipant").OrderBy<CParticipant, string>(delegate (CParticipant p) {
                return p.Name;
            }).ToList<CParticipant>();
        }

        public string ParticipantID
        {
            get
            {
                return this.mID;
            }
            set
            {
                this.mID = value;
            }
        }

        public string ParticipantIdValue
        {
            get
            {
                return this.participantIdValue;
            }
        }

        public string ParticipantName
        {
            get
            {
                return this.mName;
            }
            set
            {
                this.mName = value;
            }
        }

        public string ParticipantType
        {
            get
            {
                return this.participantType;
            }
        }
    }
}

