namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Workflow.Client;
    using SkyMap.Net.Workflow.Client.Box;
    using SkyMap.Net.Workflow.Client.View;
    using System;
    using System.Data;

    public class DefaultOpenViewCommand : AbstractBoxCommand
    {
        protected virtual void OpenView(IWfBox box, IWfView view, DataRowView row, int index)
        {
            WfViewHelper.OpenView(box, view, row, index, this.CanEdit);
        }

        public override void Run()
        {
            IWfBox owner = this.Owner as IWfBox;
            if (owner != null)
            {
                this.Run(owner, this.Index);
            }
        }

        public virtual void Run(string proinstId)
        {
            IWfBox owner = this.Owner as IWfBox;
            if (owner != null)
            {
                DataView dataSource = owner.DataSource as DataView;
                int index = 0;
                foreach (DataRowView view2 in dataSource)
                {
                    if (view2["proinst_id"].Equals(proinstId))
                    {
                        view2["sel"] = true;
                        this.Run(owner, index);
                        return;
                    }
                    index++;
                }
            }
            throw new WfClientException(string.Format("找不到业务编号为{0}的业务!", proinstId));
        }

        protected void Run(IWfBox box, int index)
        {
            AbstractBoxCommand.WaitDialogHelper.Show();
            try
            {
                DataRowView dataRowView = BoxHelper.GetDataRowView(box, index);
                WfView view = new WfView();
                view.WfBox = box;
                view.Navigate += new WfViewNavigationHandle(this.OpenView);
                this.OpenView(box, view, dataRowView, this.Index);
            }
            finally
            {
                AbstractBoxCommand.WaitDialogHelper.Close();
            }
        }

        protected virtual bool CanEdit
        {
            get
            {
                return false;
            }
        }

        protected virtual int Index
        {
            get
            {
                return BoxHelper.GetFirstSelectedIndex(this.Owner as IWfBox);
            }
        }
    }
}

