namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Workflow.Client.Box;
    using SkyMap.Net.Workflow.Client.Services;
    using SkyMap.Net.Workflow.Client.View;
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    public class CreateAndAcceptProinstCommand : AbstractBoxCommand
    {
        private string _firstProinstID;

        private void box_LoadDataCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IWfBox owner = this.Owner as IWfBox;
            DefaultOpenZbViewCommand command = new DefaultOpenZbViewCommand();
            command.Owner = owner;
            command.Run(this._firstProinstID);
            owner.LoadDataCompleted -= new RunWorkerCompletedEventHandler(this.box_LoadDataCompleted);
        }

        public override void Run()
        {
            EventHandler handler = null;
            IWfBox box = this.Owner as IWfBox;
            if ((this.IsEnabled && (box != null)) && (MessageHelper.ShowOkCancelInfo("需要新建业务吗？") != DialogResult.Cancel))
            {
                AbstractBoxCommand.WaitDialogHelper.Show();
                try
                {
                    string prodefId = null;
                    int num = 1;
                    if ((!base.Codon.Properties.Contains("prodef_id") && (DisplayMyFirstProdefsCommand.SelectProdef != null)) && (DisplayMyFirstAddNumEditCommand.Num > 0))
                    {
                        prodefId = DisplayMyFirstProdefsCommand.SelectProdef.Id;
                        num = DisplayMyFirstAddNumEditCommand.Num;
                    }
                    else
                    {
                        prodefId = base.Codon.Properties["prodef_id"];
                        if (!WorkflowService.IsCanCreateProdef(prodefId))
                        {
                            MessageHelper.ShowInfo("你没有权限新建该业务！");
                            return;
                        }
                    }
                    if (!string.IsNullOrEmpty(prodefId))
                    {
                        string[] strArray = WorkflowService.WfcInstance.CreateWfProcessAndAccept(prodefId, num);
                        if ((strArray.Length > 0) && (base.Codon.Properties.Contains("prodef_id") || ZbCreateAndOpenFirstCheckedCommand.IsChecked))
                        {
                            LoggingService.DebugFormatted("将打开业务：{0}", new object[] { strArray[0] });
                            this._firstProinstID = strArray[0];
                            WfView view = new WfView();
                            view.WfBox = box;
                            if (handler == null)
                            {
                                handler = delegate (object sender, EventArgs e) {
                                    box.RefreshData();
                                };
                            }
                            view.ViewClosed += handler;
                            WfViewHelper.OpenViewForCreateAndAcceptProinst(box, view, this._firstProinstID, true);
                        }
                    }
                }
                finally
                {
                    AbstractBoxCommand.WaitDialogHelper.Close();
                }
            }
        }
    }
}

