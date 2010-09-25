namespace SkyMap.Net.Workflow.Client.Dialog
{
    using DevExpress.XtraEditors;
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Workflow.Engine;
    using System;
    using System.Drawing;
    using System.Runtime.Remoting.Messaging;
    using System.Windows.Forms;

    public class ReasonDialog : AbstractWorkflowDialog
    {
        public Label Label2;
        private MemoEdit txtReason;

        public ReasonDialog()
        {
            this.InitializeComponent();
            this.Text = "请输入原因：";
        }

        protected override bool DoOk()
        {
            string s = string.Empty;
            if (this.txtReason.Text.Trim().Length == 0)
            {
                s = "Workflow.Message.NoInputReason";
            }
            if (!StringHelper.IsNull(s))
            {
                MessageHelper.ShowInfo(ResourceService.GetString(s));
                return false;
            }
            return true;
        }

        private void InitializeComponent()
        {
            this.txtReason = new MemoEdit();
            this.Label2 = new Label();
            this.txtReason.Properties.BeginInit();
            base.SuspendLayout();
            base.btOk.Location = new Point(0x40, 0x8b);
            base.btOk.Name = "btOk";
            base.btCancel.Location = new Point(160, 0x8b);
            base.btCancel.Name = "btCancel";
            this.txtReason.EditValue = "";
            this.txtReason.Location = new Point(8, 0x18);
            this.txtReason.Name = "txtReason";
            this.txtReason.Size = new Size(0x110, 0x70);
            this.txtReason.TabIndex = 2;
            this.Label2.AutoSize = true;
            this.Label2.BackColor = Color.Transparent;
            this.Label2.Cursor = Cursors.Default;
            this.Label2.ForeColor = SystemColors.ControlText;
            this.Label2.Location = new Point(8, 2);
            this.Label2.Name = "Label2";
            this.Label2.RightToLeft = RightToLeft.No;
            this.Label2.Size = new Size(0x56, 0x11);
            this.Label2.TabIndex = 7;
            this.Label2.Text = "请输入原因：";
            this.AutoScaleBaseSize = new Size(6, 15);
            base.ClientSize = new Size(0x124, 0xad);
            base.Controls.Add(this.Label2);
            base.Controls.Add(this.txtReason);
            base.Name = "ReasonDialog";
            base.Controls.SetChildIndex(this.txtReason, 0);
            base.Controls.SetChildIndex(base.btOk, 0);
            base.Controls.SetChildIndex(base.btCancel, 0);
            base.Controls.SetChildIndex(this.Label2, 0);
            this.txtReason.Properties.EndInit();
            base.ResumeLayout(false);
        }

        public override void SetContextData(ILogicalThreadAffinative contextData)
        {
            if (contextData is WfLogicalAbnormalContextData)
            {
                ((WfLogicalAbnormalContextData) contextData).OpReason = this.txtReason.Text.Trim();
            }
        }
    }
}

