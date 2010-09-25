namespace SkyMap.Net.Gui.Dialogs
{
    using DevExpress.XtraEditors;
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Gui.Components;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class AbstractDialog : SmForm
    {
        protected SimpleButton btCancel;
        protected SimpleButton btOk;
        private bool textFromResource = true;

        public AbstractDialog()
        {
            this.InitializeComponent();
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            if (!this.DoOk())
            {
                base.DialogResult = DialogResult.None;
            }
        }

        protected virtual bool DoOk()
        {
            throw new NotImplementedException("Must implement the DoOk method in dialog");
        }

        private void InitializeComponent()
        {
            this.btOk = new SimpleButton();
            this.btCancel = new SimpleButton();
            base.SuspendLayout();
            this.btOk.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.btOk.Location = new Point(0x40, 0xd3);
            this.btOk.Name = "btOk";
            this.btOk.TabIndex = 0;
            this.btOk.Text = "simpleButton1";
            this.btCancel.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.btCancel.DialogResult = DialogResult.Cancel;
            this.btCancel.Location = new Point(160, 0xd3);
            this.btCancel.Name = "btCancel";
            this.btCancel.TabIndex = 1;
            this.btCancel.Text = "simpleButton2";
            base.AcceptButton = this.btOk;
            this.AutoScaleBaseSize = new Size(6, 15);
            base.CancelButton = this.btCancel;
            base.ClientSize = new Size(0x124, 0xf5);
            base.Controls.Add(this.btCancel);
            base.Controls.Add(this.btOk);
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "AbstractDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            base.ResumeLayout(false);
        }

        public virtual DialogResult ShowDialog()
        {
            return this.ShowDialog(null);
        }

        public virtual DialogResult ShowDialog(IWin32Window owner)
        {
            try
            {
                base.Icon = MessageHelper.GetSystemIcon();
            }
            catch
            {
                LoggingService.WarnFormatted("获取ICON:‘{0}’失败，请检查资源中是否存在相应文件!", new object[] { "Dialog." + base.GetType().Name + ".Icon" });
            }
            this.btOk.Text = ResourceService.GetString("Global.OKButtonText");
            this.btCancel.Text = ResourceService.GetString("Global.CancelButtonText");
            this.btOk.DialogResult = DialogResult.OK;
            this.btCancel.DialogResult = DialogResult.Cancel;
            this.btOk.Click += new EventHandler(this.btOk_Click);
            return base.ShowDialog(owner);
        }

        public override string Text
        {
            get
            {
                if (this.textFromResource)
                {
                    string str="";
                    try
                    {
                        str = ResourceService.GetString("Dialog." + base.GetType().Name + ".Text");
                    }
                    catch
                    {
                    }
                    return str;
                }
                return base.Text;
            }
            set
            {
                base.Text = value;
                this.textFromResource = false;
            }
        }
    }
}

