namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Workflow.Client.Box;
    using SkyMap.Net.Workflow.Client.Services;
    using SkyMap.Net.Workflow.Client.View;
    using SkyMap.Net.Workflow.Engine;
    using System;
    using System.Data;

    public class DefaultAcceptAndOpenCommand : DefaultOpenViewCommand
    {
        protected bool accept = true;

        private void AcceptAndOpenView(IWfBox box, IWfView view, DataRowView row, int index)
        {
            int[] numArray;
            WorkItem workItem = BoxHelper.GetWorkItem(row, box);
            if (this.accept)
            {
                WorkflowService.DelegateEvent(workItem, new WfClientAPIHandler(WorkflowService.WfcInstance.Accept));
            }
            DataRowView[] navigationDataRowViews = BoxHelper.GetNavigationDataRowViews(row, index, out numArray);
            WfViewHelper.OpenView(view, workItem, row, navigationDataRowViews, numArray, this.CanEdit);
        }

        public override void Run()
        {
            IWfBox owner = this.Owner as IWfBox;
            if (owner != null)
            {
                AbstractBoxCommand.WaitDialogHelper.Show();
                try
                {
                    DataRowView dataRowView = BoxHelper.GetDataRowView(owner, this.Index);
                    IWfView view = new WfView();
                    view.Navigate += new WfViewNavigationHandle(this.AcceptAndOpenView);
                    this.AcceptAndOpenView(owner, view, dataRowView, this.Index);
                }
                finally
                {
                    AbstractBoxCommand.WaitDialogHelper.Close();
                }
            }
        }

        protected override bool CanEdit
        {
            get
            {
                return true;
            }
        }
    }
}

