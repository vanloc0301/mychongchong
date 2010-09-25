namespace SkyMap.Net.Tools.DataForms
{
    using SkyMap.Net.DataForms;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DataFormPermissionSettingForm : Form
    {
        private Button btCancel;
        private Button btOk;
        private Container components = null;
        private Panel panel1;
        private Panel panel2;
        private TabPageForSetFormPermission tpForSetFormPerm;

        public DataFormPermissionSettingForm()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panel1 = new Panel();
            this.btCancel = new Button();
            this.btOk = new Button();
            this.panel2 = new Panel();
            this.panel1.SuspendLayout();
            base.SuspendLayout();
            this.panel1.BorderStyle = BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.btCancel);
            this.panel1.Controls.Add(this.btOk);
            this.panel1.Dock = DockStyle.Bottom;
            this.panel1.Location = new Point(2, 0x213);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x314, 40);
            this.panel1.TabIndex = 0;
            this.btCancel.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btCancel.DialogResult = DialogResult.Cancel;
            this.btCancel.Location = new Point(0x271, 10);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new Size(0x4b, 0x17);
            this.btCancel.TabIndex = 1;
            this.btCancel.Text = "取消";
            this.btOk.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btOk.DialogResult = DialogResult.OK;
            this.btOk.Location = new Point(0x2c2, 10);
            this.btOk.Name = "btOk";
            this.btOk.Size = new Size(0x4b, 0x17);
            this.btOk.TabIndex = 0;
            this.btOk.Text = "确定";
            this.panel2.Dock = DockStyle.Fill;
            this.panel2.Location = new Point(2, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(0x314, 0x211);
            this.panel2.TabIndex = 1;
            this.AutoScaleBaseSize = new Size(6, 14);
            base.ClientSize = new Size(0x318, 0x23d);
            base.ControlBox = false;
            base.Controls.Add(this.panel2);
            base.Controls.Add(this.panel1);
            base.Name = "DataFormPermissionSettingForm";
            base.Padding = new Padding(2);
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "设置表单权限";
            base.WindowState = FormWindowState.Maximized;
            this.panel1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        public SkyMap.Net.DataForms.FormPermission FormPermission
        {
            get
            {
                return this.tpForSetFormPerm.FormPermission;
            }
            set
            {
                if (value == null)
                {
                    throw new NullReferenceException("Form Permission cannot be null");
                }
                this.tpForSetFormPerm = new TabPageForSetFormPermission();
                this.tpForSetFormPerm.FormPermission = value;
                this.tpForSetFormPerm.Dock = DockStyle.Fill;
                this.panel2.Controls.Add(this.tpForSetFormPerm);
            }
        }
    }
}

