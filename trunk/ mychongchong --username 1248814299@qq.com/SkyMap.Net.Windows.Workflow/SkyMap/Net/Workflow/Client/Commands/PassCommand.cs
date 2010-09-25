namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Workflow.Client;
    using SkyMap.Net.Workflow.Client.Box;
    using SkyMap.Net.Workflow.Client.Dialog;
    using System;
    using System.Data;

    public class PassCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            IWfBox owner = this.Owner as IWfBox;
            if (owner != null)
            {
                WaitDialogHelper.Show();
                try
                {
                    if (LoggingService.IsInfoEnabled)
                    {
                        LoggingService.Info("准备转出业务...");
                    }
                    BoxHelper.PassToNext(owner, new PassDialog());
                }
                catch (CannotBatchExecuteException)
                {
                    MessageService.ShowMessage("你选择的业务不能进行批处理!");
                }
                catch (Exception exception)
                {
                    LoggingService.Error("转出时发生异常", exception);
                    owner.RefreshData();
                }
                finally
                {
                    WaitDialogHelper.Close();
                }
            }
        }

        public override bool IsEnabled
        {
            get
            {
                IWfBox owner = this.Owner as IWfBox;
                try
                {
                    DataRowView[] selectedRows = BoxHelper.GetSelectedRows(owner.DataSource as DataView);
                    return ((selectedRows != null) && (selectedRows.Length > 0));
                }
                catch
                {
                    return false;
                }
            }
            set
            {
            }
        }
    }
}

