namespace SkyMap.Net.Workflow.Client.Dialog
{
    using DevExpress.LookAndFeel;
    using DevExpress.XtraEditors;
    using DevExpress.XtraTab;
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Evaluant;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Gui.Components;
    using SkyMap.Net.OGM;
    using SkyMap.Net.Security;
    using SkyMap.Net.Security.Principal;
    using SkyMap.Net.Workflow.Client;
    using SkyMap.Net.Workflow.Client.Services;
    using SkyMap.Net.Workflow.Engine;
    using SkyMap.Net.Workflow.XPDL;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Runtime.Remoting.Messaging;
    using System.Windows.Forms;

    public class PassDialog : AbstractWorkflowDialog
    {
        public CheckBox chkReSel;
        public CheckBox chkStaff;
        public System.Windows.Forms.ComboBox cmbActdef;
        public System.Windows.Forms.ComboBox cmbPoscond;
        public System.Windows.Forms.ComboBox cmbStaff;
        private GroupControl groupBox1;
        public Label Label2;
        public Label Label4;
        public Label Lbl_TS;
        private Label lblEndInfo;
        public Label lblReSel;
        public ListBox List_PreStaff;
        private OGMTreeView ogmTv;
        private CStaff selectedStaff;
        private List<CStaff> selectedStaffs;
        private DevExpress.XtraTab.XtraTabControl tabControl1;
        private XtraTabPage tbpCommon;

        public PassDialog()
        {
            this.InitializeComponent();
            this.Text = "转下一步活动";
        }

        private void AddSelecctStaff(TreeNodeCollection nodes, List<CStaff> result)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Checked && (node.Tag is CStaff))
                {
                    result.Add((CStaff) node.Tag);
                }
                if (node.Nodes.Count > 0)
                {
                    this.AddSelecctStaff(node.Nodes, result);
                }
            }
        }

        private void cmbActdef_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoggingService.Debug("下一步活动改变了...");
            try
            {
                if ((this.cmbActdef.DataSource != null) && (this.cmbActdef.SelectedItem != null))
                {
                    Actdef selectedItem = this.cmbActdef.SelectedItem as Actdef;
                    if (!StringHelper.IsNull(selectedItem.ParticipantId))
                    {
                        CParticipant part = QueryHelper.Get<CParticipant>(selectedItem.ParticipantId);
                        if (part != null)
                        {
                            IList<CStaff> staffsOfParticipant = WorkflowService.GetStaffsOfParticipant(part);
                            if (staffsOfParticipant.Count <= 0)
                            {
                                throw new WfClientException("没有分配人员行使该活动！");
                            }
                            if (part.AssignRule == AssignRuleType.ANY)
                            {
                                if (this.ogmTv == null)
                                {
                                    this.ogmTv = new OGMTreeView();
                                    this.ogmTv.Size = new Size(this.cmbStaff.Width, 4 * this.cmbStaff.Height);
                                    this.groupBox1.Controls.Add(this.ogmTv);
                                    this.ogmTv.Location = this.cmbStaff.Location;
                                    this.ogmTv.CheckBoxes = true;
                                    this.ogmTv.AfterCheck += new TreeViewEventHandler(this.ogmTv_AfterCheck);
                                }
                                this.cmbStaff.Visible = false;
                                this.ogmTv.InitStaffsTree(part.Name, staffsOfParticipant);
                            }
                            else
                            {
                                int num2;
                                if (this.ogmTv != null)
                                {
                                    this.ogmTv.Visible = false;
                                    this.cmbStaff.Visible = true;
                                }
                                this.cmbStaff.DataSource = null;
                                Transition transition = this.cmbPoscond.SelectedItem as Transition;
                                foreach (SkyMap.Net.Workflow.XPDL.Condition condition in transition.Conditions)
                                {
                                    if (condition.Type == "AUTOSTAFFSQL")
                                    {
                                        LoggingService.InfoFormatted("将依据{0}自动选择转出人员。。。", new object[] { condition.Xpression });
                                        if (!string.IsNullOrEmpty(condition.Xpression))
                                        {
                                            DataTable table = QueryHelper.ExecuteSql("Default", string.Empty, this.ReplaceSystemParams(condition.Xpression));
                                            if ((staffsOfParticipant != null) && (table.Rows.Count > 0))
                                            {
                                                int count = table.Rows.Count;
                                                num2 = staffsOfParticipant.Count - 1;
                                                while (num2 > -1)
                                                {
                                                    if (table.Select("staff_id='" + staffsOfParticipant[num2].Id + "'").Length == 0)
                                                    {
                                                        staffsOfParticipant.RemoveAt(num2);
                                                    }
                                                    num2--;
                                                }
                                                if (staffsOfParticipant.Count > 0)
                                                {
                                                    this.cmbStaff.DataSource = staffsOfParticipant;
                                                    this.chkStaff.Checked = true;
                                                }
                                            }
                                        }
                                    }
                                }
                                if ((this.cmbStaff.DataSource == null) || (this.cmbStaff.Items.Count == 0))
                                {
                                    CStaff current;
                                    string str = SecurityUtil.GetSmPrincipal().DeptIds[0];
                                    LoggingService.Debug("转出人员太多，看是否需要赐除非本部门人员...");
                                    bool flag = false;
                                    bool flag2 = true;
                                    using (IEnumerator<CStaff> enumerator2 = staffsOfParticipant.GetEnumerator())
                                    {
                                        while (enumerator2.MoveNext())
                                        {
                                            current = enumerator2.Current;
                                            foreach (CStaffDept dept in current.StaffDepts)
                                            {
                                                if (str == dept.DeptID)
                                                {
                                                    flag = true;
                                                }
                                                else
                                                {
                                                    flag2 = true;
                                                }
                                                if (flag2 && flag)
                                                {
                                                    goto Label_0411;
                                                }
                                            }
                                        Label_0411:;
                                        }
                                    }
                                    if (flag && flag2)
                                    {
                                        //lhm 原来同部门的情况下默认会自动勾选上选中某人，现修改为false
                                        this.chkStaff.Checked = false;
                                        if (LoggingService.IsInfoEnabled)
                                        {
                                            LoggingService.Info("转出时移除非本部门的人员从列表。。。");
                                        }
                                        for (num2 = staffsOfParticipant.Count - 1; num2 > -1; num2--)
                                        {
                                            current = staffsOfParticipant[num2];
                                            foreach (CStaffDept dept in current.StaffDepts)
                                            {
                                                if (str != dept.DeptID)
                                                {
                                                    staffsOfParticipant.RemoveAt(num2);
                                                    goto Label_04EF;
                                                }
                                            }
                                        Label_04EF:;
                                        }
                                    }
                                    else
                                    {
                                        this.chkStaff.Checked = false;
                                    }
                                    this.cmbStaff.DataSource = staffsOfParticipant;
                                    string userId = SecurityUtil.GetSmIdentity().UserId;
                                    foreach (CStaff staff in staffsOfParticipant)
                                    {
                                        if (staff.Id == str)
                                        {
                                            this.cmbStaff.SelectedItem = staff;
                                            return;
                                        }
                                    }
                                    foreach (CStaff staff in staffsOfParticipant)
                                    {
                                        foreach (CStaffDept dept in staff.StaffDepts)
                                        {
                                            if (str == dept.DeptID)
                                            {
                                                this.cmbStaff.SelectedItem = staff;
                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (CoreException exception)
            {
                MessageHelper.ShowBaseExceptionInfo(exception);
            }
            catch (Exception exception2)
            {
                MessageHelper.ShowError("UnKnown", exception2);
            }
        }

        private void cmbPoscond_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoggingService.Debug("转下一步条件改变了。。。");
            if (this.cmbPoscond.SelectedItem != null)
            {
                Transition selectedItem = this.cmbPoscond.SelectedItem as Transition;
                IList<Actdef> toInteractionActdefs = WorkflowService.GetToInteractionActdefs(selectedItem);
                bool flag = toInteractionActdefs.Count == 0;
                this.lblEndInfo.Visible = flag;
                this.cmbStaff.Visible = flag;
                this.cmbStaff.Visible = !flag;
                this.chkStaff.Visible = !flag;
                if (this.ogmTv != null)
                {
                    this.ogmTv.Visible = !flag;
                }
                if (!flag)
                {
                    this.cmbActdef.DataSource = toInteractionActdefs;
                }
                else
                {
                    this.cmbActdef.DataSource = new Actdef[] { selectedItem.To };
                }
            }
        }

        protected override bool DoOk()
        {
            if (this.cmbPoscond.SelectedItem == null)
            {
                MessageHelper.ShowInfo(ResourceService.GetString("Workflow.Message.NoPoscond"));
                return false;
            }
            this.selectedStaffs = this.GetSelectedStaffs();
            if ((this.selectedStaffs != null) && (this.selectedStaffs.Count == 0))
            {
                MessageHelper.ShowInfo("你必须至少选择一个下一活动的经办人！");
                return false;
            }
            if (this.chkStaff.Visible && this.chkStaff.Checked)
            {
                this.selectedStaff = (CStaff) this.cmbStaff.SelectedItem;
            }
            else
            {
                this.selectedStaff = null;
            }
            Transition selectedItem = (Transition) this.cmbPoscond.SelectedItem;
            foreach (SkyMap.Net.Workflow.XPDL.Condition condition in selectedItem.Conditions)
            {
                string str;
                string type = condition.Type;
                if (((type != null) && (type == "SQL")) && !this.SQLConditionEval(condition.Xpression, out str))
                {
                    MessageHelper.ShowInfo(str);
                    return false;
                }
            }
            return true;
        }

        private List<CStaff> GetSelectedStaffs()
        {
            if ((this.ogmTv != null) && this.ogmTv.Visible)
            {
                List<CStaff> result = new List<CStaff>();
                this.AddSelecctStaff(this.ogmTv.Nodes, result);
                return result;
            }
            return null;
        }

        private void InitializeComponent()
        {
            this.tabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.tbpCommon = new DevExpress.XtraTab.XtraTabPage();
            this.groupBox1 = new DevExpress.XtraEditors.GroupControl();
            this.lblEndInfo = new System.Windows.Forms.Label();
            this.chkStaff = new System.Windows.Forms.CheckBox();
            this.chkReSel = new System.Windows.Forms.CheckBox();
            this.cmbPoscond = new System.Windows.Forms.ComboBox();
            this.cmbActdef = new System.Windows.Forms.ComboBox();
            this.List_PreStaff = new System.Windows.Forms.ListBox();
            this.cmbStaff = new System.Windows.Forms.ComboBox();
            this.lblReSel = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Lbl_TS = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tbpCommon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btCancel
            // 
            this.btCancel.Location = new System.Drawing.Point(200, 291);
            // 
            // btOk
            // 
            this.btOk.Location = new System.Drawing.Point(104, 291);
            // 
            // tabControl1
            // 
            this.tabControl1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl1.Location = new System.Drawing.Point(8, 8);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedTabPage = this.tbpCommon;
            this.tabControl1.Size = new System.Drawing.Size(376, 272);
            this.tabControl1.TabIndex = 2;
            this.tabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tbpCommon});
            // 
            // tbpCommon
            // 
            this.tbpCommon.Appearance.PageClient.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tbpCommon.Appearance.PageClient.Options.UseForeColor = true;
            this.tbpCommon.Controls.Add(this.groupBox1);
            this.tbpCommon.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbpCommon.Name = "tbpCommon";
            this.tbpCommon.Size = new System.Drawing.Size(367, 240);
            this.tbpCommon.Text = "一般转出";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblEndInfo);
            this.groupBox1.Controls.Add(this.chkStaff);
            this.groupBox1.Controls.Add(this.chkReSel);
            this.groupBox1.Controls.Add(this.cmbPoscond);
            this.groupBox1.Controls.Add(this.cmbActdef);
            this.groupBox1.Controls.Add(this.List_PreStaff);
            this.groupBox1.Controls.Add(this.cmbStaff);
            this.groupBox1.Controls.Add(this.lblReSel);
            this.groupBox1.Controls.Add(this.Label4);
            this.groupBox1.Controls.Add(this.Label2);
            this.groupBox1.Controls.Add(this.Lbl_TS);
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(352, 192);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.Text = "请选择下一步经办人";
            // 
            // lblEndInfo
            // 
            this.lblEndInfo.AutoSize = true;
            this.lblEndInfo.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblEndInfo.ForeColor = System.Drawing.Color.Magenta;
            this.lblEndInfo.Location = new System.Drawing.Point(118, 174);
            this.lblEndInfo.Name = "lblEndInfo";
            this.lblEndInfo.Size = new System.Drawing.Size(135, 12);
            this.lblEndInfo.TabIndex = 22;
            this.lblEndInfo.Text = "选择该条件业务将结案";
            // 
            // chkStaff
            // 
            this.chkStaff.BackColor = System.Drawing.Color.Transparent;
            this.chkStaff.Cursor = System.Windows.Forms.Cursors.Default;
            this.chkStaff.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkStaff.ForeColor = System.Drawing.SystemColors.WindowText;
            this.chkStaff.Location = new System.Drawing.Point(22, 72);
            this.chkStaff.Name = "chkStaff";
            this.chkStaff.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.chkStaff.Size = new System.Drawing.Size(112, 18);
            this.chkStaff.TabIndex = 17;
            this.chkStaff.Text = "下一步经办人：";
            this.chkStaff.UseVisualStyleBackColor = false;
            // 
            // chkReSel
            // 
            this.chkReSel.BackColor = System.Drawing.Color.Transparent;
            this.chkReSel.Cursor = System.Windows.Forms.Cursors.Default;
            this.chkReSel.Enabled = false;
            this.chkReSel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkReSel.Location = new System.Drawing.Point(310, 23);
            this.chkReSel.Name = "chkReSel";
            this.chkReSel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.chkReSel.Size = new System.Drawing.Size(16, 21);
            this.chkReSel.TabIndex = 16;
            this.chkReSel.UseVisualStyleBackColor = false;
            this.chkReSel.Visible = false;
            // 
            // cmbPoscond
            // 
            this.cmbPoscond.BackColor = System.Drawing.SystemColors.Window;
            this.cmbPoscond.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmbPoscond.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPoscond.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cmbPoscond.Location = new System.Drawing.Point(142, 23);
            this.cmbPoscond.Name = "cmbPoscond";
            this.cmbPoscond.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmbPoscond.Size = new System.Drawing.Size(155, 20);
            this.cmbPoscond.TabIndex = 15;
            // 
            // cmbActdef
            // 
            this.cmbActdef.BackColor = System.Drawing.SystemColors.Window;
            this.cmbActdef.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmbActdef.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbActdef.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cmbActdef.Location = new System.Drawing.Point(142, 47);
            this.cmbActdef.Name = "cmbActdef";
            this.cmbActdef.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmbActdef.Size = new System.Drawing.Size(155, 20);
            this.cmbActdef.TabIndex = 14;
            // 
            // List_PreStaff
            // 
            this.List_PreStaff.BackColor = System.Drawing.SystemColors.Menu;
            this.List_PreStaff.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.List_PreStaff.Cursor = System.Windows.Forms.Cursors.Default;
            this.List_PreStaff.ForeColor = System.Drawing.Color.Blue;
            this.List_PreStaff.ItemHeight = 12;
            this.List_PreStaff.Location = new System.Drawing.Point(142, 97);
            this.List_PreStaff.Name = "List_PreStaff";
            this.List_PreStaff.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.List_PreStaff.Size = new System.Drawing.Size(155, 62);
            this.List_PreStaff.Sorted = true;
            this.List_PreStaff.TabIndex = 13;
            this.List_PreStaff.Visible = false;
            // 
            // cmbStaff
            // 
            this.cmbStaff.BackColor = System.Drawing.SystemColors.Window;
            this.cmbStaff.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmbStaff.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStaff.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cmbStaff.Location = new System.Drawing.Point(142, 71);
            this.cmbStaff.Name = "cmbStaff";
            this.cmbStaff.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmbStaff.Size = new System.Drawing.Size(155, 20);
            this.cmbStaff.TabIndex = 12;
            // 
            // lblReSel
            // 
            this.lblReSel.BackColor = System.Drawing.Color.Transparent;
            this.lblReSel.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblReSel.Enabled = false;
            this.lblReSel.ForeColor = System.Drawing.Color.Blue;
            this.lblReSel.Location = new System.Drawing.Point(310, 47);
            this.lblReSel.Name = "lblReSel";
            this.lblReSel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblReSel.Size = new System.Drawing.Size(20, 96);
            this.lblReSel.TabIndex = 21;
            this.lblReSel.Text = "重新选择条件";
            this.lblReSel.Visible = false;
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.BackColor = System.Drawing.Color.Transparent;
            this.Label4.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label4.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label4.Location = new System.Drawing.Point(48, 27);
            this.Label4.Name = "Label4";
            this.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label4.Size = new System.Drawing.Size(77, 12);
            this.Label4.TabIndex = 20;
            this.Label4.Text = "转下步条件：";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.BackColor = System.Drawing.Color.Transparent;
            this.Label2.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label2.Location = new System.Drawing.Point(48, 52);
            this.Label2.Name = "Label2";
            this.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label2.Size = new System.Drawing.Size(77, 12);
            this.Label2.TabIndex = 19;
            this.Label2.Text = "下一步工作：";
            // 
            // Lbl_TS
            // 
            this.Lbl_TS.BackColor = System.Drawing.Color.Transparent;
            this.Lbl_TS.Cursor = System.Windows.Forms.Cursors.Default;
            this.Lbl_TS.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Lbl_TS.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Lbl_TS.Location = new System.Drawing.Point(54, 98);
            this.Lbl_TS.Name = "Lbl_TS";
            this.Lbl_TS.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Lbl_TS.Size = new System.Drawing.Size(78, 48);
            this.Lbl_TS.TabIndex = 18;
            this.Lbl_TS.Text = "提示：                上几步经办人见右列表";
            this.Lbl_TS.Visible = false;
            // 
            // PassDialog
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(392, 325);
            this.Controls.Add(this.tabControl1);
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Name = "PassDialog";
            this.Controls.SetChildIndex(this.btOk, 0);
            this.Controls.SetChildIndex(this.btCancel, 0);
            this.Controls.SetChildIndex(this.tabControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.tabControl1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tbpCommon.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        private void ogmTv_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Nodes.Count > 0)
            {
                foreach (TreeNode node in e.Node.Nodes)
                {
                    node.Checked = e.Node.Checked;
                }
            }
        }

        private string ReplaceSystemParams(string xpression)
        {
            SmIdentity smIdentity = SecurityUtil.GetSmIdentity();
            return xpression.Replace("{PROJECT_ID}", base.WorkItem.ProjectId).Replace("{PROINST_ID}", base.WorkItem.ProinstId).Replace("{ACTINST_ID}", base.WorkItem.ActinstId).Replace("{STAFF_ID}", smIdentity.UserId).Replace("{STAFF_NAME}", smIdentity.UserName);
        }

        public override void SetContextData(ILogicalThreadAffinative contextData)
        {
            if ((contextData is WfLogicalPassContextData) && (this.cmbPoscond.Items.Count != 0))
            {
                Transition selectedItem = this.cmbPoscond.SelectedItem as Transition;
                if (selectedItem != null)
                {
                    WfLogicalPassContextData data = contextData as WfLogicalPassContextData;
                    data.SelectedTranId = selectedItem.Id;
                    if (this.selectedStaffs == null)
                    {
                        if (this.selectedStaff != null)
                        {
                            data.ToStaffIds = new string[] { this.selectedStaff.Id };
                            data.ToStaffNames = new string[] { this.selectedStaff.Name };
                        }
                        else
                        {
                            data.ToStaffIds = new string[0];
                            data.ToStaffNames = new string[0];
                        }
                    }
                    else
                    {
                        int count = this.selectedStaffs.Count;
                        data.ToStaffIds = new string[count];
                        data.ToStaffNames = new string[count];
                        for (int i = 0; i < count; i++)
                        {
                            CStaff staff = this.selectedStaffs[i];
                            data.ToStaffIds[i] = staff.Id;
                            data.ToStaffNames[i] = staff.Name;
                        }
                    }
                }
            }
        }

        public override DialogResult ShowDialog()
        {
            string actdefId = base.WorkItem.ActdefId;
            IList<Transition> trans = WorkflowService.GetTrans(base.WorkItem.ProdefId, actdefId);
            if ((trans == null) || (trans.Count <= 0))
            {
                if (trans != null)
                {
                    if (MessageHelper.ShowOkCancelInfo(ResourceService.GetString("Workflow.Message.WillCompleted")) == DialogResult.OK)
                    {
                        return DialogResult.No;
                    }
                    return DialogResult.Cancel;
                }
                return DialogResult.No;
            }
            this.cmbPoscond.DataSource = trans;
            if (trans.Count > 0)
            {
                foreach (Transition transition in trans)
                {
                    foreach (SkyMap.Net.Workflow.XPDL.Condition condition in transition.Conditions)
                    {
                        if (condition.Type == "AUTOROUTE")
                        {
                            string str2;
                            LoggingService.InfoFormatted("将依据{0}自动选择转出路由。。。", new object[] { condition.Xpression });
                            if (this.SQLConditionEval(condition.Xpression, out str2))
                            {
                                this.cmbPoscond.SelectedItem = transition;
                                goto Label_013D;
                            }
                        }
                    }
                }
            }
        Label_013D:
            this.cmbPoscond.SelectedIndexChanged += new EventHandler(this.cmbPoscond_SelectedIndexChanged);
            this.cmbActdef.SelectedIndexChanged += new EventHandler(this.cmbActdef_SelectedIndexChanged);
            if (this.cmbPoscond.SelectedItem != null)
            {
                this.cmbPoscond_SelectedIndexChanged(this.cmbPoscond, new EventArgs());
            }
            return base.ShowDialog();
        }

        private bool SQLConditionEval(string xpression, out string message)
        {
            return XMLSQLConditionsHelper.ParserAndEval(this.ReplaceSystemParams(xpression), out message);
        }
    }
}

