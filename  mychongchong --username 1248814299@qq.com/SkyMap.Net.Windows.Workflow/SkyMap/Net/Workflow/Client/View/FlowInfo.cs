namespace SkyMap.Net.Workflow.Client.View
{
    using DevExpress.Utils;
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraGrid;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Base;
    using DevExpress.XtraGrid.Views.Grid;
    using DevExpress.XtraTab;
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui.Components;
    using SkyMap.Net.Workflow.Client.Services;
    using SkyMap.Net.Workflow.Engine;
    using SkyMap.Net.Workflow.Instance;
    using SkyMap.Net.Workflow.XPDL;
    using System;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    public class FlowInfo : SmBarUserControl
    {
        private DataView ablView = null;
        private Container components = null;
        private DataSet ds = new DataSet();
        private const string fldAblMem = "特殊操作";
        private const string fldDueDate = "规定结束日期";
        private const string fldDueTime = "规定天数";
        private const string fldEndDate = "实际结束日期";
        private const string fldId = "ID";
        private const string fldOverTime = "超期天数";
        private const string fldPID = "PID";
        private const string fldStaffName = "经办人";
        private const string fldStartDate = "开始日期";
        private const string fldStatus = "状态";
        private const string fldStepName = "步骤";
        private SkyMap.Net.Gui.Components.SmGridControl grid;
        private bool hasLoadedDetailFlowInfo;
        private Proinst p;
        private ProinstFlowInfo proinstFlowInfo1;
        private string proinstId;
        private string relStepAbl = "特殊操作";
        private SmGridView stepGrid;
        private System.Data.DataView stepView = null;
        private DevExpress.XtraEditors.MemoEdit txtProInfo;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;

        public FlowInfo()
        {
            this.InitializeComponent();
            base.CreateToolbar();
            this.InitialDataSet();
            this.InitGrid();
        }

        private void AddAblRow(WfAbnormalAuditInst abl)
        {
            if (LoggingService.IsWarnEnabled)
            {
                LoggingService.Warn("Add abl : '" + abl.AssignId + "'");
            }
            DataRowView view = this.ablView.AddNew();
            view.BeginEdit();
            view["PID"] = abl.AssignId;
            StringBuilder builder = new StringBuilder(100);
            if (abl.TimeStamp.HasValue)
            {
                builder.Append(abl.TimeStamp.Value.ToString("yyyy年MM月dd日"));
            }
            builder.Append("被" + abl.OpStaffName);
            builder.Append(WfUtil.GetSMNAbnormal(abl.Type));
            if (!StringHelper.IsNull(abl.ReceiveStaffName))
            {
                builder.Append("给" + abl.ReceiveStaffName);
            }
            if (!StringHelper.IsNull(abl.OpReason))
            {
                builder.Append(",原因：" + abl.OpReason);
            }
            if (abl.ReleaseType != WfReleaseType.NotReleased)
            {
                if (abl.ReleaseTime.HasValue)
                {
                    builder.Append(";" + abl.ReleaseTime.Value.ToString("yyyy年MM月dd日"));
                }
                builder.Append("被" + abl.ReleaseStaffName);
                switch (abl.ReleaseType)
                {
                    case WfReleaseType.Resumed:
                        builder.Append("重新开始办理");
                        break;

                    case WfReleaseType.AbortDeleted:
                        builder.Append("回收删除");
                        break;

                    case WfReleaseType.TerminateCompleted:
                        builder.Append("退件结案");
                        break;
                }
            }
            if (LoggingService.IsWarnEnabled)
            {
                LoggingService.Warn("Add abl : " + builder.ToString());
            }
            view["特殊操作"] = builder.ToString();
            view.EndEdit();
        }

        private void AddStepRow(Actinst a, WfAssigninst s)
        {
            DataRowView view = this.stepView.AddNew();
            view.BeginEdit();
            view["ID"] = s.Id;
            view["步骤"] = a.Name;
            view["规定天数"] = a.DueTime;
            if (a.StartDate.HasValue)
            {
                view["开始日期"] = a.StartDate.Value.ToString();
                if (a.DueTime > 0.0)
                {
                    view["规定结束日期"] = a.DueDate.ToString();
                    double num = WfUtil.GetCostTime(a) - a.DueTime;
                    view["超期天数"] = num;
                }
            }
            if ((WfUtil.GetWfState(a.Status) == WfStateType.Closed) && a.EndDate.HasValue)
            {
                view["实际结束日期"] = a.EndDate.Value.ToString();
            }
            view["经办人"] = s.StaffName;
            view["状态"] = WfUtil.GetSMNAssignStatus(s);
            view.EndEdit();
        }

        private void AddStepRow(string prodefId, string actdefId, StringCollection sc)
        {
            Prodef prodef = WorkflowService.Prodefs[prodefId];
            if (prodef != null)
            {
                try
                {
                    Actdef actdef = prodef.Actdefs[actdefId];
                    if ((actdef.Type != ActdefType.COMPLETION) && (actdef.Froms.Count > 0))
                    {
                        Actdef to = null;
                        foreach (Transition transition in actdef.Froms)
                        {
                            if (!sc.Contains(transition.Id))
                            {
                                to = transition.To;
                                sc.Add(transition.Id);
                                break;
                            }
                        }
                        if (to != null)
                        {
                            if (to.Type == ActdefType.INTERACTION)
                            {
                                DataRowView view = this.stepView.AddNew();
                                view.BeginEdit();
                                view["ID"] = -this.stepView.Count;
                                view["步骤"] = to.Name;
                                view.EndEdit();
                            }
                            this.AddStepRow(prodefId, to.Id, sc);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message.ToString());
                }
            }
        }

        private void AddStylesForConditions()
        {
            StyleFormatCondition[] conditions = new StyleFormatCondition[3];
            conditions[0] = new StyleFormatCondition(FormatConditionEnum.Greater, this.stepGrid.Columns["超期天数"], null, 0.1);
            conditions[0].ApplyToRow = true;
            conditions[0].Appearance.ForeColor = Color.Red;
            conditions[1] = new StyleFormatCondition(FormatConditionEnum.Less, this.stepGrid.Columns["超期天数"], null, -1.1);
            conditions[1].ApplyToRow = true;
            conditions[1].Appearance.ForeColor = Color.Green;
            conditions[2] = new StyleFormatCondition(FormatConditionEnum.Between, this.stepGrid.Columns["超期天数"], null, -1.1, 0.1);
            conditions[2].ApplyToRow = true;
            conditions[2].Appearance.ForeColor = Color.YellowGreen;
            this.stepGrid.FormatConditions.AddRange(conditions);
        }

        private void AdjustGrid(SmGridView grid)
        {
            foreach (GridColumn column in grid.Columns)
            {
                column.OptionsColumn.AllowSize = true;
                if (column.Caption.Substring(0, 1).IndexOfAny(StringHelper.ABC) >= 0)
                {
                    column.VisibleIndex = -1;
                }
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

        private string GetProinstInfoString(Proinst p)
        {
            double costTime;
            double num2;
            StringBuilder builder = new StringBuilder(100);
            builder.Append("业务名称：");
            builder.Append(p.Name);
            if (((p.WfAbnormalAudits != null) && (p.Status == WfStatusType.WF_RUNNING)) && (((p.Priority & 1) == 1) || ((p.Priority & 2) == 2)))
            {
                foreach (WfAbnormalAuditInst inst in p.WfAbnormalAudits)
                {
                    if ((inst.Type == WfAbnormalType.MONITOR) && (inst.ReleaseType == WfReleaseType.NotReleased))
                    {
                        builder.AppendFormat("，{0}正在督办该业务", inst.OpStaffName);
                    }
                    if ((inst.Type == WfAbnormalType.PRESS) && (inst.ReleaseType == WfReleaseType.NotReleased))
                    {
                        builder.AppendFormat("，{0}正在催办该业务", inst.OpStaffName);
                    }
                }
            }
            builder.Append("\r\n");
            builder.Append("业 务 号：");
            builder.Append(p.ProjectId);
            builder.Append("\r\n");
            builder.Append("开始时间：");
            if (p.StartDate.HasValue)
            {
                builder.Append(p.StartDate.Value.ToString("yyyy年MM月dd日HH时mm分"));
            }
            else
            {
                builder.Append("尚未开始办理");
            }
            builder.Append("；");
            if (p.DueTime > 0.0)
            {
                if (p.DueTime > 0.0)
                {
                    builder.Append("规定办理天数：");
                    builder.Append(p.DueTime);
                }
                builder.Append("；");
                if (p.DueDate.HasValue)
                {
                    builder.Append("规定办结时间：");
                    builder.Append(p.DueDate.Value.ToShortDateString());
                }
            }
            builder.Append("\r\n");
            builder.Append("实际办结时间：");
            if (WfUtil.GetWfState(p.Status) == WfStateType.Closed)
            {
                builder.Append(p.EndDate.Value.ToString("yyyy年MM月dd日HH时mm分") + "；");
                builder.Append("办理用时");
                costTime = WfUtil.GetCostTime(p);
                builder.Append(costTime.ToString());
                builder.Append("天；");
                if (p.DueTime > 0.0)
                {
                    num2 = costTime - p.DueTime;
                    if (num2 > 0.0)
                    {
                        builder.Append("超期");
                        builder.Append(num2.ToString());
                        builder.Append("天；");
                    }
                    else
                    {
                        builder.Append("未超期；");
                    }
                }
            }
            else if (WfUtil.GetWhileOpen(p.Status) == WhileOpenType.NotRunning)
            {
                builder.Append("该业务尚在待办中");
            }
            else
            {
                builder.Append("已办理：");
                costTime = WfUtil.GetCostTime(p);
                builder.Append(costTime.ToString());
                builder.Append("天；");
                if (p.DueTime > 0.0)
                {
                    num2 = costTime - p.DueTime;
                    if (num2 > 0.0)
                    {
                        builder.Append("已超期");
                        builder.Append(num2.ToString());
                        builder.Append("天；");
                    }
                    else
                    {
                        builder.Append("尚未超期；");
                    }
                }
            }
            builder.Append("业务状态：");
            builder.Append(WfUtil.GetSMNState(p.Status));
            return builder.ToString();
        }

        private void InitGrid()
        {
            this.grid.BeginUpdate();
            this.stepGrid.OptionsView.ShowGroupPanel = false;
            this.stepGrid.OptionsBehavior.Editable = false;
            this.grid.DataSource = this.stepView;
            this.grid.MainView.PopulateColumns();
            SmGridView template = new SmGridView(this.grid);
            template.OptionsView.ShowGroupPanel = false;
            template.OptionsDetail.ShowDetailTabs = true;
            template.OptionsDetail.EnableDetailToolTip = true;
            template.OptionsView.ShowColumnHeaders = false;
            template.DetailHeight = 80;
            this.grid.LevelTree.Nodes.Add(this.relStepAbl, template);
            foreach (DataColumn column in this.ablView.Table.Columns)
            {
                GridColumn column2 = template.Columns.AddField(column.ColumnName);
                column2.Caption = column.ColumnName;
                column2.OptionsColumn.AllowIncrementalSearch = false;
                column2.OptionsColumn.AllowSort = DefaultBoolean.False;
                column2.OptionsColumn.AllowMove = false;
                column2.OptionsFilter.AllowFilter = false;
                column2.VisibleIndex = template.Columns.Count - 1;
            }
            template.OptionsBehavior.Editable = false;
            template.OptionsView.RowAutoHeight = true;
            this.AdjustGrid(this.stepGrid);
            this.AdjustGrid(template);
            this.grid.EndUpdate();
        }

        private void InitialDataSet()
        {
            int num2;
            DataTable table = new DataTable("step");
            string[] strArray = new string[] { "ID", "步骤", "规定天数", "开始日期", "规定结束日期", "实际结束日期", "超期天数", "经办人", "状态" };
            System.Type[] typeArray = new System.Type[] { typeof(string), typeof(string), typeof(double), typeof(string), typeof(string), typeof(string), typeof(double), typeof(string), typeof(string) };
            int length = strArray.Length;
            DataColumn[] columns = new DataColumn[length];
            for (num2 = 0; num2 < length; num2++)
            {
                columns[num2] = new DataColumn(strArray[num2], typeArray[num2]);
            }
            table.Columns.AddRange(columns);
            DataTable table2 = new DataTable("abnormal");
            strArray = new string[] { "PID", "特殊操作" };
            typeArray = new System.Type[] { typeof(string), typeof(string) };
            length = strArray.Length;
            columns = new DataColumn[length];
            for (num2 = 0; num2 < length; num2++)
            {
                columns[num2] = new DataColumn(strArray[num2], typeArray[num2]);
            }
            table2.Columns.AddRange(columns);
            this.ds.Tables.Add(table);
            this.ds.Tables.Add(table2);
            this.ds.Relations.Add(this.relStepAbl, table.Columns["ID"], table2.Columns["PID"], false);
            DataViewManager manager = new DataViewManager(this.ds);
            this.stepView = manager.CreateDataView(table);
            this.ablView = manager.CreateDataView(table2);
        }

        private void InitializeComponent()
        {
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.grid = new SkyMap.Net.Gui.Components.SmGridControl();
            this.stepGrid = new SkyMap.Net.Gui.Components.SmGridView();
            this.txtProInfo = new DevExpress.XtraEditors.MemoEdit();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.proinstFlowInfo1 = new SkyMap.Net.Workflow.Client.View.ProinstFlowInfo();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stepGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProInfo.Properties)).BeginInit();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.grid);
            this.xtraTabPage2.Controls.Add(this.txtProInfo);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Padding = new System.Windows.Forms.Padding(1);
            this.xtraTabPage2.Size = new System.Drawing.Size(609, 316);
            this.xtraTabPage2.Text = "详细流程";
            // 
            // grid
            // 
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.EmbeddedNavigator.Name = "";
            this.grid.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.grid.Location = new System.Drawing.Point(1, 81);
            this.grid.MainView = this.stepGrid;
            this.grid.Name = "grid";
            this.grid.Size = new System.Drawing.Size(607, 234);
            this.grid.TabIndex = 3;
            this.grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.stepGrid});
            // 
            // stepGrid
            // 
            this.stepGrid.Appearance.FocusedRow.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.stepGrid.Appearance.FocusedRow.Options.UseFont = true;
            this.stepGrid.Appearance.SelectedRow.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.stepGrid.Appearance.SelectedRow.Options.UseFont = true;
            this.stepGrid.GridControl = this.grid;
            this.stepGrid.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
            this.stepGrid.Name = "stepGrid";
            this.stepGrid.OptionsMenu.EnableColumnMenu = false;
            this.stepGrid.OptionsMenu.EnableFooterMenu = false;
            this.stepGrid.OptionsMenu.EnableGroupPanelMenu = false;
            this.stepGrid.OptionsView.EnableAppearanceEvenRow = true;
            this.stepGrid.OptionsView.EnableAppearanceOddRow = true;
            this.stepGrid.PaintStyleName = "Skin";
            this.stepGrid.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
            // 
            // txtProInfo
            // 
            this.txtProInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtProInfo.EditValue = "textBox1";
            this.txtProInfo.Location = new System.Drawing.Point(1, 1);
            this.txtProInfo.Name = "txtProInfo";
            this.txtProInfo.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.txtProInfo.Properties.Appearance.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtProInfo.Properties.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.txtProInfo.Properties.Appearance.Options.UseBackColor = true;
            this.txtProInfo.Properties.Appearance.Options.UseFont = true;
            this.txtProInfo.Properties.Appearance.Options.UseForeColor = true;
            this.txtProInfo.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.txtProInfo.Properties.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtProInfo.Size = new System.Drawing.Size(607, 80);
            this.txtProInfo.TabIndex = 2;
            this.txtProInfo.TabStop = false;
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.AutoScroll = true;
            this.xtraTabPage1.Controls.Add(this.proinstFlowInfo1);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Padding = new System.Windows.Forms.Padding(1);
            this.xtraTabPage1.Size = new System.Drawing.Size(609, 316);
            this.xtraTabPage1.Text = "办理流程";
            // 
            // proinstFlowInfo1
            // 
            this.proinstFlowInfo1.Location = new System.Drawing.Point(1, 1);
            this.proinstFlowInfo1.Name = "proinstFlowInfo1";
            this.proinstFlowInfo1.Padding = new System.Windows.Forms.Padding(1);
            this.proinstFlowInfo1.Size = new System.Drawing.Size(2000, 1500);
            this.proinstFlowInfo1.TabIndex = 0;
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(2, 2);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(618, 348);
            this.xtraTabControl1.TabIndex = 2;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            this.xtraTabControl1.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.xtraTabControl1_SelectedPageChanged);
            // 
            // FlowInfo
            // 
            this.Controls.Add(this.xtraTabControl1);
            this.Name = "FlowInfo";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Size = new System.Drawing.Size(622, 352);
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stepGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProInfo.Properties)).EndInit();
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void ListStep(Proinst p)
        {
            Actinst a = null;
            StringCollection sc = new StringCollection();
            for (int i = 0; i < p.Actinsts.Count; i++)
            {
                a = p.Actinsts[i];
                if (LoggingService.IsWarnEnabled)
                {
                    LoggingService.Warn(a.Name);
                }
                foreach (WfAssigninst assigninst in a.Assigns)
                {
                    this.AddStepRow(a, assigninst);
                    foreach (WfAbnormalAuditInst inst in assigninst.WfAbnormalAudits)
                    {
                        this.AddAblRow(inst);
                    }
                }
            }
            if (a.Type != ActdefType.COMPLETION)
            {
               //this.AddStepRow(a.ProdefId, a.ActdefId, sc);
            }
        }

        private void LoadFlowInfo()
        {
            this.txtProInfo.Text = this.GetProinstInfoString(this.p);
            this.txtProInfo.Properties.ReadOnly = true;
            this.ablView.Table.Rows.Clear();
            this.stepView.Table.Rows.Clear();
            this.ListStep(this.p);
            for (int i = 0; i < this.stepGrid.RowCount; i++)
            {
                this.stepGrid.ExpandMasterRow(i);
            }
            this.AddStylesForConditions();
            base.Height = (this.grid.Top + (this.stepGrid.RowCount * 15)) + 50;
            base.Width = 800;
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {
            if (!(this.hasLoadedDetailFlowInfo || (e.Page != this.xtraTabPage2)))
            {
                this.LoadFlowInfo();
                this.stepGrid.ExpandAllGroups();
                this.hasLoadedDetailFlowInfo = true;
            }
        }

        public string ProinstId
        {
            set
            {
                if (this.proinstId != value)
                {
                    this.hasLoadedDetailFlowInfo = false;
                    this.proinstId = value;
                    this.p = WorkflowService.WfcInstance.GetProinst(this.proinstId);
                    if (this.p != null)
                    {
                        this.Text = string.Format("{0}:{1}", this.p.Name, this.p.ProjectId);
                        this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
                        if (WorkflowService.Prodefs.ContainsKey(this.p.ProdefId))
                        {
                            Prodef prodef = WorkflowService.Prodefs[this.p.ProdefId];
                            if (LoggingService.IsDebugEnabled)
                            {
                                LoggingService.DebugFormatted("将显示{0}的流程定义图...", new object[] { value });
                            }
                            this.proinstFlowInfo1.ShowMe(prodef, this.p);
                        }   
                    }                  
                }
            }
        }

        public string ProjectID
        {
            get
            {
                return this.p.ProjectId;
            }
        }

        protected override string ToolbarPath
        {
            get
            {
                return "/Workflow/FlowInfo/Toolbar";
            }
        }
    }
}

