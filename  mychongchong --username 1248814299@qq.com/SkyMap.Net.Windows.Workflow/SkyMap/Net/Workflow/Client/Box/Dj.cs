namespace SkyMap.Net.Workflow.Client.Box
{
    using DevExpress.XtraGrid.Columns;
    using SkyMap.Net.DataForms;
    using SkyMap.Net.Workflow.Client;
    using SkyMap.Net.Workflow.Client.Commands;
    using SkyMap.Net.Workflow.Client.Config;
    using SkyMap.Net.Workflow.Client.Services;
    using SkyMap.Net.Workflow.XPDL;
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.ComponentModel;

    public class Dj : WfBox
    {
        public static int AddProinstNum = 1;
        private IContainer components = null;
        public static ProdefRow CurrentProdef = null;
        private int FixedColumnNum = 0;
        private OpenViewForQueryFocusCommand openViewForQueryFocusCommand;
        private static HybridDictionary prodefColumns = new HybridDictionary(2);
        private static HybridDictionary prodefQueryVals = new HybridDictionary(2);

        public Dj()
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

        private void GetColumnsAndQueryVals(string prodefKey, int startVisibleIndex, ref GridColumn[] columns, ref string[] queryVals)
        {
            if ((columns == null) || (queryVals == null))
            {
                if (!WorkflowService.Prodefs.ContainsKey(prodefKey))
                {
                    throw new WfClientException(string.Format("不能找到ID为：【{0}】的业务定义！", prodefKey));
                }
                Prodef prodef = WorkflowService.Prodefs[prodefKey];
                string str = string.Empty;
                string str2 = string.Empty;
                GridColumn column = null;
                ArrayList list = new ArrayList(5);
                int num = startVisibleIndex;
                DAODataSet daoDataSet = WorkflowService.GetDaoDataSet(prodef.DaoDataSetId);
                if (daoDataSet == null)
                {
                    throw new WfClientException("Prodef Config data have errors : Data Set is not setted!");
                }
                foreach (DAODataTable table in daoDataSet.DAODataTables)
                {
                    if (!table.Level)
                    {
                        string str3 = str2;
                        str2 = str3 + "\n left join " + table.Name + " on p.PROJECT_ID=" + table.Name + "." + table.Key;
                        foreach (DAODataColumn column2 in table.DataColumns)
                        {
                            if (column2.IsDisplay)
                            {
                                str3 = str;
                                str = str3 + "," + table.Name + "." + column2.Name;
                                column = new GridColumn();
                                column.FieldName = column2.Name;
                                column.Caption = column2.Description;
                                column.VisibleIndex = num;
                                column.OptionsColumn.AllowEdit = false;
                                column.ColumnEdit = base.GetDefaultColumnEdit();
                                list.Add(column);
                                num++;
                            }
                        }
                    }
                    if (list.Count > 0)
                    {
                        columns = (GridColumn[]) list.ToArray(typeof(GridColumn));
                    }
                    if (str.Length > 0)
                    {
                        queryVals = new string[] { str, str2, prodefKey };
                    }
                }
            }
        }

        protected override void InitializeAsync(object sender, DoWorkEventArgs e)
        {
            CBoxConfig argument = e.Argument as CBoxConfig;
            this.FixedColumnNum = argument.ColList.Count;
            base.InitializeAsync(sender, e);
        }

        private void InitializeComponent()
        {
            base.SuspendLayout();
            base.Name = "Dj";
            base.ResumeLayout(false);
        }

        protected override void LoadMainData()
        {
            if (CurrentProdef != null)
            {
                GridColumn[] columns = null;
                string[] queryVals = null;
                this.GetColumnsAndQueryVals(CurrentProdef.Id, this.FixedColumnNum, ref columns, ref queryVals);
                if ((columns == null) || (queryVals == null))
                {
                    throw new WfClientException("The dataset of the prodef '" + CurrentProdef.Id + "' have error");
                }
                if ((base.Tag == null) || ((base.Tag != null) && !CurrentProdef.Equals(base.Tag)))
                {
                    base.Tag = CurrentProdef;
                    for (int i = base.view.Columns.Count - 1; i > this.FixedColumnNum; i--)
                    {
                        base.view.Columns.RemoveAt(i);
                    }
                    base.view.Columns.AddRange(columns);
                }
                base.bgLoadDataWorker.RunWorkerAsync(queryVals);
            }
            else
            {
                base.grid.DataSource = null;
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

