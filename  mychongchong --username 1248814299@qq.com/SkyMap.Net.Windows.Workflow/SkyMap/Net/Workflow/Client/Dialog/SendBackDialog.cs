namespace SkyMap.Net.Workflow.Client.Dialog
{
    using DevExpress.XtraEditors;
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Workflow.Client.Services;
    using SkyMap.Net.Workflow.Engine;
    using System;
    using System.Data;
    using System.Drawing;
    using System.Runtime.Remoting.Messaging;
    using System.Windows.Forms;

    public class SendBackDialog : AbstractWorkflowDialog
    {
        private System.Windows.Forms.ComboBox cmbSendBacks;
        public Label label1;
        public Label Label2;
        private MemoEdit txtReason;

        public SendBackDialog()
        {
            this.InitializeComponent();
            this.Text = "退回";
        }

        protected override bool DoOk()
        {
            string s = string.Empty;
            if (this.txtReason.Text.Trim().Length == 0)
            {
                s = "Workflow.Message.NoInputReason";
            }
            else if (this.cmbSendBacks.SelectedItem == null)
            {
                s = "Workflow.Message.NoSelectSendBack";
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
            this.cmbSendBacks = new System.Windows.Forms.ComboBox();
            this.label1 = new Label();
            this.txtReason.Properties.BeginInit();
            base.SuspendLayout();
            base.btOk.Location = new Point(0x40, 0xbb);
            base.btOk.Name = "btOk";
            base.btCancel.Location = new Point(160, 0xbb);
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
            this.Label2.Size = new Size(0x71, 0x11);
            this.Label2.TabIndex = 7;
            this.Label2.Text = "请输入退回原因：";
            this.cmbSendBacks.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbSendBacks.Location = new Point(8, 160);
            this.cmbSendBacks.Name = "cmbSendBacks";
            this.cmbSendBacks.Size = new Size(0x110, 20);
            this.cmbSendBacks.TabIndex = 8;
            this.label1.AutoSize = true;
            this.label1.BackColor = Color.Transparent;
            this.label1.Cursor = Cursors.Default;
            this.label1.ForeColor = SystemColors.ControlText;
            this.label1.Location = new Point(0x10, 0x90);
            this.label1.Name = "label1";
            this.label1.RightToLeft = RightToLeft.No;
            this.label1.Size = new Size(140, 0x11);
            this.label1.TabIndex = 9;
            this.label1.Text = "请选择退回的活动环节";
            this.AutoScaleBaseSize = new Size(6, 15);
            base.ClientSize = new Size(0x124, 0xdd);
            base.Controls.Add(this.cmbSendBacks);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.Label2);
            base.Controls.Add(this.txtReason);
            base.Name = "ReasonDialog";
            base.Controls.SetChildIndex(this.txtReason, 0);
            base.Controls.SetChildIndex(base.btOk, 0);
            base.Controls.SetChildIndex(base.btCancel, 0);
            base.Controls.SetChildIndex(this.Label2, 0);
            base.Controls.SetChildIndex(this.label1, 0);
            base.Controls.SetChildIndex(this.cmbSendBacks, 0);
            this.txtReason.Properties.EndInit();
            base.ResumeLayout(false);
        }

        public override void SetContextData(ILogicalThreadAffinative contextData)
        {
            if (contextData is WfLogicalAbnormalContextData)
            {
                WfLogicalAbnormalContextData data = (WfLogicalAbnormalContextData) contextData;
                data.OpReason = this.txtReason.Text.Trim();
                data.ReceiveAssignId = this.cmbSendBacks.SelectedValue.ToString();
            }
        }

        public override DialogResult ShowDialog()
        {
            string[] vals = new string[] { base.WorkItem.Id, base.WorkItem.ProinstId };
            DataTable rs = WorkflowService.GetRs("GetSendBackAssigns", vals);
            if (rs.Rows.Count == 0)
            {
                throw new CannotSendBackException("Cannot find the completed assignments to send back");
            }
            this.cmbSendBacks.DataSource = rs;
            this.cmbSendBacks.DisplayMember = "ACTINST_NAME";
            this.cmbSendBacks.ValueMember = "ASSIGN_ID";
            return base.ShowDialog();
        }
    }
}

