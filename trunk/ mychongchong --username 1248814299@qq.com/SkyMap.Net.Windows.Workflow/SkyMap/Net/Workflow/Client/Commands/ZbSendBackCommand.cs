namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Workflow.Client;
    using SkyMap.Net.Workflow.Client.Box;
    using System;
    using System.Data;

    public class ZbSendBackCommand : SendBackCommand
    {
        public override bool IsEnabled
        {
            get
            {
                try
                {
                    DataRowView[] selectedRows = BoxHelper.GetSelectedRows((this.Owner as IWfBox).DataSource as DataView);
                    foreach (DataRowView view in selectedRows)
                    {
                        if (view["FROMACTINST_ID"].ToString().Length == 0)
                        {
                            return false;
                        }
                    }
                }
                catch (NotSelectException)
                {
                    return false;
                }
                return true;
            }
            set
            {
                base.IsEnabled = value;
            }
        }
    }
}

