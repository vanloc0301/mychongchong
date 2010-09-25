namespace SkyMap.Net.Gui.Dialogs
{
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using SkyMap.Net.Core;
    using System;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ServerDialog : AbstractDialog
    {
        private CheckEdit chkDefault;
        private ComboBoxEdit cmbServers;
        private Container components = null;
        private Label label1;
        private HybridDictionary servers = new HybridDictionary(1);

        public ServerDialog()
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

        protected override bool DoOk()
        {
            if (this.cmbServers.EditValue == null)
            {
                return false;
            }
            return true;
        }

        private void InitializeComponent()
        {
            this.cmbServers = new ComboBoxEdit();
            this.chkDefault = new CheckEdit();
            this.label1 = new Label();
            this.cmbServers.Properties.BeginInit();
            this.chkDefault.Properties.BeginInit();
            base.SuspendLayout();
            base.btOk.Location = new Point(0x40, 0x53);
            base.btOk.Name = "btOk";
            base.btCancel.Location = new Point(160, 0x53);
            base.btCancel.Name = "btCancel";
            this.cmbServers.EditValue = "";
            this.cmbServers.Location = new Point(80, 0x10);
            this.cmbServers.Name = "cmbServers";
            this.cmbServers.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.cmbServers.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            this.cmbServers.Size = new Size(0xb0, 0x15);
            this.cmbServers.TabIndex = 2;
            this.chkDefault.EditValue = true;
            this.chkDefault.Location = new Point(0x48, 0x30);
            this.chkDefault.Name = "chkDefault";
            this.chkDefault.Properties.Caption = "下次登录默认为此服务器";
            this.chkDefault.Size = new Size(0xb8, 0x13);
            this.chkDefault.TabIndex = 3;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x19, 0x13);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x2d, 0x11);
            this.label1.TabIndex = 4;
            this.label1.Text = "服务器";
            this.AutoScaleBaseSize = new Size(6, 15);
            base.ClientSize = new Size(0x124, 0x75);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.chkDefault);
            base.Controls.Add(this.cmbServers);
            base.Name = "ServerDialog";
            this.Text = "选择使用的远程服务器";
            base.Controls.SetChildIndex(this.cmbServers, 0);
            base.Controls.SetChildIndex(this.chkDefault, 0);
            base.Controls.SetChildIndex(this.label1, 0);
            base.Controls.SetChildIndex(base.btOk, 0);
            base.Controls.SetChildIndex(base.btCancel, 0);
            this.cmbServers.Properties.EndInit();
            this.chkDefault.Properties.EndInit();
            base.ResumeLayout(false);
        }

        public void InitServers(HybridDictionary servers)
        {
            this.cmbServers.Properties.Items.Clear();
            foreach (string str in servers.Keys)
            {
                this.cmbServers.Properties.Items.Add(str);
            }
            string str2 = PropertyService.Get<string>("DefaultServer", string.Empty);
            this.cmbServers.EditValue = (!string.IsNullOrEmpty(str2) && servers.Contains(str2)) ? str2 : this.cmbServers.Properties.Items[0];
            this.chkDefault.Checked = PropertyService.Get<bool>("IsHideServerDialogOnNext", true);
        }

        public bool IsHideServerDialogOnNext
        {
            get
            {
                return this.chkDefault.Checked;
            }
        }

        public string ServerName
        {
            get
            {
                return this.cmbServers.Text;
            }
        }
    }
}

