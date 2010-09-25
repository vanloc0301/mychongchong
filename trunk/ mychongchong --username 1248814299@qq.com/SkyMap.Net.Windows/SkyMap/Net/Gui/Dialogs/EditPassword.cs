namespace SkyMap.Net.Gui.Dialogs
{
    using DevExpress.XtraEditors;
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Gui.Components;
    using SkyMap.Net.OGM;
    using SkyMap.Net.Security;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class EditPassword : SmForm
    {
        private SimpleButton btCancel;
        private SimpleButton btOk;
        private Container components = null;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextEdit txtConfirmPwd;
        private TextEdit txtNewPwd;
        private TextEdit txtOldPwd;

        public EditPassword()
        {
            this.InitializeComponent();
            this.InitMe();
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            CStaff staff = OGMService.GetStaff(SecurityUtil.GetSmIdentity().UserId);
            string name = string.Empty;
            if ((this.txtNewPwd.Text.Length == 0) || (this.txtConfirmPwd.Text.Length == 0))
            {
                name = "Dialog.EditPassword.Message.InputNotFull";
            }
            if (!OGMService.CheckPassword(staff, this.txtOldPwd.Text))
            {
                name = "Dialog.EditPassword.Message.OldPasswordError";
                this.txtOldPwd.SelectAll();
            }
            if (this.txtNewPwd.Text != this.txtConfirmPwd.Text)
            {
                name = "Dialog.EditPassword.Message.ConfirmPasswordError";
                this.txtConfirmPwd.SelectAll();
            }
            if (name != string.Empty)
            {
                base.DialogResult = DialogResult.None;
                MessageHelper.ShowInfo(ResourceService.GetString(name));
            }
            else
            {
                OGMService.SetNewPassword(staff, this.txtNewPwd.Text);
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

        private void InitializeComponent()
        {
            this.label2 = new Label();
            this.txtOldPwd = new TextEdit();
            this.label1 = new Label();
            this.txtNewPwd = new TextEdit();
            this.label3 = new Label();
            this.txtConfirmPwd = new TextEdit();
            this.btCancel = new SimpleButton();
            this.btOk = new SimpleButton();
            this.txtOldPwd.Properties.BeginInit();
            this.txtNewPwd.Properties.BeginInit();
            this.txtConfirmPwd.Properties.BeginInit();
            base.SuspendLayout();
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x18, 0x18);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x2d, 0x11);
            this.label2.TabIndex = 9;
            this.label2.Text = "旧密码";
            this.txtOldPwd.EditValue = "";
            this.txtOldPwd.Location = new Point(80, 20);
            this.txtOldPwd.Name = "txtOldPwd";
            this.txtOldPwd.Size = new Size(0xa8, 0x15);
            this.txtOldPwd.TabIndex = 0;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x18, 0x38);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x2d, 0x11);
            this.label1.TabIndex = 11;
            this.label1.Text = "新密码";
            this.txtNewPwd.EditValue = "";
            this.txtNewPwd.Location = new Point(80, 0x34);
            this.txtNewPwd.Name = "txtNewPwd";
            this.txtNewPwd.Size = new Size(0xa8, 0x15);
            this.txtNewPwd.TabIndex = 1;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(8, 0x58);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x3b, 0x11);
            this.label3.TabIndex = 13;
            this.label3.Text = "确认密码";
            this.txtConfirmPwd.EditValue = "";
            this.txtConfirmPwd.Location = new Point(80, 0x54);
            this.txtConfirmPwd.Name = "txtConfirmPwd";
            this.txtConfirmPwd.Size = new Size(0xa8, 0x15);
            this.txtConfirmPwd.TabIndex = 2;
            this.btCancel.DialogResult = DialogResult.Cancel;
            this.btCancel.Location = new Point(0x90, 120);
            this.btCancel.Name = "btCancel";
            this.btCancel.TabIndex = 4;
            this.btCancel.Text = "取消";
            this.btOk.Location = new Point(40, 120);
            this.btOk.Name = "btOk";
            this.btOk.TabIndex = 3;
            this.btOk.Text = "登录";
            base.AcceptButton = this.btOk;
            this.AutoScaleBaseSize = new Size(6, 15);
            base.CancelButton = this.btCancel;
            base.ClientSize = new Size(0x102, 0x9f);
            base.Controls.Add(this.btCancel);
            base.Controls.Add(this.btOk);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.txtConfirmPwd);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.txtNewPwd);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.txtOldPwd);
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.MaximizeBox = false;
            base.Name = "EditPassword";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "EditPassword";
            this.txtOldPwd.Properties.EndInit();
            this.txtNewPwd.Properties.EndInit();
            this.txtConfirmPwd.Properties.EndInit();
            base.ResumeLayout(false);
        }

        private void InitMe()
        {
            this.Text = ResourceService.GetString("Dialog.EditPassword.Text");
            base.Icon = ResourceService.GetIcon("Dialog.EditPassword.Icon");
            this.btOk.Text = ResourceService.GetString("Global.OKButtonText");
            this.btCancel.Text = ResourceService.GetString("Global.CancelButtonText");
            this.txtOldPwd.Properties.PasswordChar = '*';
            this.txtNewPwd.Properties.PasswordChar = '*';
            this.txtConfirmPwd.Properties.PasswordChar = '*';
            this.btOk.Image = ResourceService.GetBitmap("Global.OkButtonImage");
            this.btCancel.Image = ResourceService.GetBitmap("Global.CancelButtonImage");
            this.btOk.DialogResult = DialogResult.OK;
            this.btCancel.DialogResult = DialogResult.Cancel;
            this.btOk.Click += new EventHandler(this.btOk_Click);
        }
    }
}

