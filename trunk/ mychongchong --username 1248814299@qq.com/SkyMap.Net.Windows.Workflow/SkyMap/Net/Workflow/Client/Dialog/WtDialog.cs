namespace SkyMap.Net.Workflow.Client.Dialog
{
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.OGM;
    using SkyMap.Net.Security;
    using SkyMap.Net.Workflow.Client;
    using SkyMap.Net.Workflow.Engine;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.Remoting.Messaging;
    using System.Windows.Forms;

    public class WtDialog : AbstractWorkflowDialog
    {
        private ComboBoxEdit cmbStaffs;
        private Label label1;
        private Label lblAllWt;

        public WtDialog()
        {
            this.InitializeComponent();
            this.Text = "委托";
        }

        protected override bool DoOk()
        {
            if (this.cmbStaffs.EditValue == null)
            {
                MessageHelper.ShowInfo(ResourceService.GetString("Workflow.Message.NoAcceptStaff"));
                return false;
            }
            return true;
        }

        private void InitializeComponent()
        {
            this.cmbStaffs = new ComboBoxEdit();
            this.lblAllWt = new Label();
            this.label1 = new Label();
            this.cmbStaffs.Properties.BeginInit();
            base.SuspendLayout();
            base.btOk.Location = new Point(0x40, 0x83);
            base.btOk.Name = "btOk";
            base.btCancel.Location = new Point(160, 0x83);
            base.btCancel.Name = "btCancel";
            this.cmbStaffs.EditValue = "comboBoxEdit1";
            this.cmbStaffs.Location = new Point(0x58, 0x20);
            this.cmbStaffs.Name = "cmbStaffs";
            this.cmbStaffs.Properties.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.cmbStaffs.Size = new Size(160, 0x15);
            this.cmbStaffs.TabIndex = 2;
            this.lblAllWt.ForeColor = Color.Red;
            this.lblAllWt.Location = new Point(30, 0x40);
            this.lblAllWt.Name = "lblAllWt";
            this.lblAllWt.Size = new Size(0xe8, 0x38);
            this.lblAllWt.TabIndex = 12;
            this.lblAllWt.Text = "你没有选择任何业务，所以这将会将所有办理业务的权限（包括收件与在办权限）委托给被委托人，直到你取消委托";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x20, 0x24);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x2d, 0x11);
            this.label1.TabIndex = 11;
            this.label1.Text = "委托给";
            this.AutoScaleBaseSize = new Size(6, 15);
            base.ClientSize = new Size(0x124, 0xa5);
            base.Controls.Add(this.lblAllWt);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.cmbStaffs);
            base.Name = "WtDialog";
            base.Controls.SetChildIndex(this.cmbStaffs, 0);
            base.Controls.SetChildIndex(base.btOk, 0);
            base.Controls.SetChildIndex(base.btCancel, 0);
            base.Controls.SetChildIndex(this.label1, 0);
            base.Controls.SetChildIndex(this.lblAllWt, 0);
            this.cmbStaffs.Properties.EndInit();
            base.ResumeLayout(false);
        }

        public override void SetContextData(ILogicalThreadAffinative contextData)
        {
            CStaff selectedItem = this.cmbStaffs.SelectedItem as CStaff;
            if ((selectedItem != null) && (contextData is WfLogicalAbnormalContextData))
            {
                WfLogicalAbnormalContextData data = contextData as WfLogicalAbnormalContextData;
                data.ReceiveStaffId = selectedItem.Id;
                data.ReceiveStaffName = selectedItem.Name;
            }
        }

        public override DialogResult ShowDialog()
        {
            this.lblAllWt.Visible = base.WorkItem == null;
            IList<CStaff> staffsOfCurrentStaffDepts = OGMService.GetStaffsOfCurrentStaffDepts();
            if (staffsOfCurrentStaffDepts.Count == 0)
            {
                throw new WfClientException(ResourceService.GetString("Workflow.Message.NoWtStaffs"));
            }
            this.cmbStaffs.Properties.Items.Clear();
            foreach (CStaff staff in staffsOfCurrentStaffDepts)
            {
                this.cmbStaffs.Properties.Items.Add(staff);
            }
            this.cmbStaffs.EditValue = staffsOfCurrentStaffDepts[0];
            this.cmbStaffs.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            return base.ShowDialog();
        }
    }
}

