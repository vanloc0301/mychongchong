namespace SkyMap.Net.Workflow.Client.Box
{
    using DevExpress.XtraGrid.Columns;
    using SkyMap.Net.Core;
    using SkyMap.Net.Criteria.Client;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Gui.Components;
    using SkyMap.Net.Workflow.Client;
    using SkyMap.Net.Workflow.Client.Commands;
    using SkyMap.Net.Workflow.Client.Config;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Windows.Forms;

    public class WfGeneralCriteria : WfBox
    {
        private IContainer components = null;
        private GeneralCriteria genCriteria;
        private OpenViewForQueryFocusCommand openViewForQueryFocusCommand;
        private Panel pnlTop;
        private Splitter splitter1;

        public WfGeneralCriteria()
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

        private DataTable GetMainTable(DataSet ds)
        {
            int count = ds.Tables.Count;
            for (int i = 0; i < count; i++)
            {
                DataTable rs = ds.Tables[i];
                if (rs.ParentRelations.Count == 0)
                {
                    if (rs.Columns.Contains("PROJECT_ID"))
                    {
                        BoxHelper.AddSelColumn(rs);
                    }
                    return rs;
                }
            }
            throw new WfClientException("General criteria dataset is null");
        }

        private void GridAdjustment(DataSet ds)
        {
            int num3;
            int count = ds.Tables.Count;
            SmGridView[] viewArray = new SmGridView[count];
            viewArray[0] = base.view;
            int index = 1;
            for (num3 = 0; num3 < count; num3++)
            {
                string relationName = "";
                if (ds.Tables[num3].ParentRelations.Count > 0)
                {
                    viewArray[index] = new SmGridView(base.grid);
                    viewArray[index].OptionsDetail.ShowDetailTabs = true;
                    viewArray[index].OptionsDetail.EnableDetailToolTip = true;
                    relationName = ds.Tables[num3].ParentRelations[0].RelationName;
                }
                if (relationName != "")
                {
                    base.grid.LevelTree.Nodes.Add(relationName, viewArray[index]);
                }
            }
            for (num3 = 0; num3 < count; num3++)
            {
                viewArray[num3].OptionsView.ColumnAutoWidth = ds.Tables[num3].Columns.Count < 8;
                if (num3 > 0)
                {
                    viewArray[num3].OptionsView.ShowGroupPanel = false;
                }
                viewArray[num3].DetailHeight = 500;
            }
        }

        private void GridEditorsAdjustment()
        {
            base.grid.BeginUpdate();
            SmGridView view = null;
            view = base.view;
            foreach (GridColumn column in view.Columns)
            {
                if (column.FieldName.Equals("sel"))
                {
                    column.Caption = " ";                    
                    column.VisibleIndex = 0;
                    column.Width = 20;
                    column.OptionsColumn.AllowFocus = true;
                    column.ColumnEdit = base.rschkSel;
                }
                //else if (column.Caption.Substring(0, 1).ToUpper().LastIndexOfAny(StringHelper.ABC) >= 0) 2010.11.12 因dev升级此代码出现问题，改为如下
                else if (!String.IsNullOrEmpty(column.FieldName) && column.FieldName.Substring(0, 1).ToUpper().LastIndexOfAny(StringHelper.ABC) >= 0)
                {
                    column.VisibleIndex = -1;
                }
                else
                {
                    column.ColumnEdit = base.GetDefaultColumnEdit();
                }
            }
            base.grid.EndUpdate();
            //base.grid.Refresh();
        }

        public override void Init(CBoxConfig boxConfig)
        {
            CriteriaBusiness business = new CriteriaBusiness("TY_SEARCHX", "TY_FIELDSSELECT", "TY_FILTER", "TY_SEARCHTABLE", "TR_FILTER_CONDITION", "T_STAFFCX");
            this.genCriteria = new GeneralCriteria();
            this.genCriteria.CriteriaBusiness = business;
            this.pnlTop.ClientSize = this.genCriteria.Size;
            this.pnlTop.Controls.Add(this.genCriteria);
            this.genCriteria.Dock = DockStyle.Fill;
            this.genCriteria.BringToFront();
            this.genCriteria.Criteria += new CriteriaEventHandle(this.OnCriteria);
            base.Init(boxConfig);
        }

        private void InitializeComponent()
        {
            this.pnlTop = new Panel();
            this.splitter1 = new Splitter();
            base.SuspendLayout();
            this.pnlTop.Dock = DockStyle.Top;
            this.pnlTop.Location = new Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new Size(480, 0x90);
            this.pnlTop.TabIndex = 9;
            this.splitter1.Dock = DockStyle.Top;
            this.splitter1.Location = new Point(0, 0x90);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new Size(480, 3);
            this.splitter1.TabIndex = 10;
            this.splitter1.TabStop = false;
            base.Controls.Add(this.splitter1);
            base.Controls.Add(this.pnlTop);
            base.Name = "WfGeneralCriteria";
            base.Controls.SetChildIndex(this.pnlTop, 0);
            base.Controls.SetChildIndex(base.grid, 0);
            base.Controls.SetChildIndex(this.splitter1, 0);
            base.ResumeLayout(false);
        }

        private void OnCriteria(object sender, CriteriaEventArgs e)
        {
            WaitDialogHelper.Show();
            try
            {
                base.view.OptionsView.ShowFooter = false;
                base.view.GroupSummary.Clear();
                DataSet dataSet = e.DataSet;
                if ((dataSet != null) && (dataSet.Tables.Count > 0))
                {
                    DataTable rs = null;
                    for (int i = 0; i < dataSet.Tables.Count; i++)
                    {
                        rs = dataSet.Tables[i];
                        BoxHelper.SetRsReadOnly(rs);
                    }
                    DataView defaultView = this.GetMainTable(dataSet).DefaultView;
                    base.grid.DataSource = defaultView;
                    base.grid.MainView.PopulateColumns();
                    this.GridAdjustment(dataSet);
                    this.GridEditorsAdjustment();
                    base.view.BestFitColumns();
                }
                base.BarStatusUpdate();
            }
            catch (ConditionInputException exception)
            {
                MessageHelper.ShowInfo(exception.Message);
            }
            finally
            {
                WaitDialogHelper.Close();
            }
        }

        public override void RefreshData()
        {
        }

        protected override void SecurityUtil_CurrentPrincipalChanged(object sender, EventArgs e)
        {
            base.grid.DataSource = null;
        }

        protected override void ViewDoubleClick(object sender, EventArgs e)
        {
            ICommand command = this.genCriteria.OpenFormCommand(this);
            if (command != null)
            {
                command.Run();
            }
            else
            {
                base.ViewDoubleClick(sender, e);
            }
        }

        public override DefaultOpenViewCommand OpenViewCommand
        {
            get
            {
                if (this.openViewForQueryFocusCommand == null)
                {
                    this.openViewForQueryFocusCommand = new OpenViewForQueryFocusCommand();
                    this.openViewForQueryFocusCommand.Owner = this;
                }
                return this.openViewForQueryFocusCommand;
            }
        }
    }
}

