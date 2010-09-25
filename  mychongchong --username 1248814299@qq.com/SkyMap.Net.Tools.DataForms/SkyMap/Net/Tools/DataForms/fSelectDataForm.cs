namespace SkyMap.Net.Tools.DataForms
{
    using SkyMap.Net.DAO;
    using SkyMap.Net.DataForms;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;

    public class fSelectDataForm : Form
    {
        private Button btnCancel;
        private Button btnOK;
        private IContainer components;
        private ImageList imageList1;
        private ListView lvw1;
        private string mID;
        private string mName;
        private Panel panel1;
        private ToolBar toolBar1;
        private ToolBarButton toolBarButton1;
        private ToolBarButton toolBarButton2;
        private ToolBarButton toolBarButton3;

        public fSelectDataForm()
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
            this.mID = this.lvw1.SelectedItems[0].Tag.ToString();
            this.mName = this.lvw1.SelectedItems[0].SubItems[1].Text;
            base.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void fSelectForm_Load(object sender, EventArgs e)
        {
            this.InitColumns();
            this.RefreshList();
            if (this.mID != null)
            {
                foreach (ListViewItem item in this.lvw1.Items)
                {
                    if (item.Tag.ToString().ToUpper() == this.mID.ToUpper())
                    {
                        item.Selected = true;
                    }
                }
            }
        }

        private void InitColumns()
        {
            this.lvw1.Columns.Clear();
            this.lvw1.Columns.Add("ID", 50, HorizontalAlignment.Left);
            this.lvw1.Columns.Add("名称", 200, HorizontalAlignment.Left);
            this.lvw1.Columns.Add("说明", 200, HorizontalAlignment.Left);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ResourceManager manager = new ResourceManager(typeof(fSelectDataForm));
            this.lvw1 = new ListView();
            this.toolBarButton1 = new ToolBarButton();
            this.toolBarButton2 = new ToolBarButton();
            this.toolBarButton3 = new ToolBarButton();
            this.imageList1 = new ImageList(this.components);
            this.panel1 = new Panel();
            this.toolBar1 = new ToolBar();
            this.btnCancel = new Button();
            this.btnOK = new Button();
            this.panel1.SuspendLayout();
            base.SuspendLayout();
            this.lvw1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.lvw1.FullRowSelect = true;
            this.lvw1.Location = new Point(8, 0x2a);
            this.lvw1.MultiSelect = false;
            this.lvw1.Name = "lvw1";
            this.lvw1.Size = new Size(0x20e, 0x184);
            this.lvw1.TabIndex = 4;
            this.lvw1.View = View.Details;
            this.toolBarButton1.ImageIndex = 0;
            this.toolBarButton1.Text = "新增";
            this.toolBarButton2.ImageIndex = 1;
            this.toolBarButton2.Text = "编辑";
            this.toolBarButton3.ImageIndex = 2;
            this.toolBarButton3.Text = "删除";
            this.imageList1.ColorDepth = ColorDepth.Depth32Bit;
            this.imageList1.ImageSize = new Size(0x10, 0x10);
            this.imageList1.ImageStream = (ImageListStreamer)manager.GetObject("imageList1.ImageStream");
            this.imageList1.TransparentColor = Color.Fuchsia;
            this.panel1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.panel1.Controls.Add(this.toolBar1);
            this.panel1.Location = new Point(8, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x20e, 0x20);
            this.panel1.TabIndex = 7;
            this.toolBar1.Appearance = ToolBarAppearance.Flat;
            this.toolBar1.Buttons.AddRange(new ToolBarButton[] { this.toolBarButton1, this.toolBarButton2, this.toolBarButton3 });
            this.toolBar1.DropDownArrows = true;
            this.toolBar1.ImageList = this.imageList1;
            this.toolBar1.Location = new Point(0, 0);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.ShowToolTips = true;
            this.toolBar1.Size = new Size(0x20e, 0x1c);
            this.toolBar1.TabIndex = 0;
            this.toolBar1.TextAlign = ToolBarTextAlign.Right;
            this.btnCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnCancel.Location = new Point(0x1ce, 0x1b6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x48, 0x18);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.btnOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnOK.Location = new Point(0x176, 0x1b6);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(80, 0x18);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.AutoScaleBaseSize = new Size(6, 14);
            base.ClientSize = new Size(0x21e, 0x1d9);
            base.Controls.Add(this.panel1);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.lvw1);
            base.Name = "fSelectDataForm";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "请选择";
            base.Load += new EventHandler(this.fSelectForm_Load);
            this.panel1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        public void RefreshList()
        {
            ListViewItem current;
            IEnumerator enumerator = this.lvw1.Items.GetEnumerator();
            while (enumerator.MoveNext())
            {
                current = (ListViewItem)enumerator.Current;
                current.SubItems.Clear();
            }
            this.lvw1.Items.Clear();
            IList<DAODataForm> list = QueryHelper.List<DAODataForm>("ALL_DAODataForm_DAO");
            foreach (DAODataForm form in list)
            {
                current = new ListViewItem(form.Id);
                current.SubItems.Add(form.Name);
                current.SubItems.Add(form.Description);
                current.Tag = form.Id;
                this.lvw1.Items.Add(current);
            }
        }

        public string DataFormID
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

        public string DataFormName
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
    }
}

