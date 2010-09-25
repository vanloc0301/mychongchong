namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Gui;
    using SkyMap.Net.Workflow.Client.Box;
    using SkyMap.Net.Workflow.Client.Services;
    using System;
    using System.ComponentModel;

    public class CreateMyDjProinstCommand : AbstractBoxCommand
    {
        private string _firstProinstID;

        private void box_LoadDataCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IWfBox owner = this.Owner as IWfBox;
            OpenViewForQueryCommand command = new OpenViewForQueryCommand();
            command.Owner = owner;
            command.Run(this._firstProinstID);
            owner.LoadDataCompleted -= new RunWorkerCompletedEventHandler(this.box_LoadDataCompleted);
        }

        public override void Run()
        {
            IWfBox owner = this.Owner as IWfBox;
            if (this.IsEnabled && (owner != null))
            {
                AbstractBoxCommand.WaitDialogHelper.Show();
                try
                {
                    string prodefId = null;
                    int addProinstNum = 1;
                    if ((!base.Codon.Properties.Contains("prodef_id") && (Dj.CurrentProdef != null)) && (Dj.AddProinstNum > 0))
                    {
                        prodefId = Dj.CurrentProdef.Id;
                        addProinstNum = Dj.AddProinstNum;
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
                    string[] strArray = WorkflowService.WfcInstance.CreateWfProcess(prodefId, addProinstNum);
                    if ((strArray.Length > 0) && (base.Codon.Properties.Contains("prodef_id") || DjCreateAndOpenFirstCheckedCommand.IsChecked))
                    {
                        this._firstProinstID = strArray[0];
                        owner.LoadDataCompleted += new RunWorkerCompletedEventHandler(this.box_LoadDataCompleted);
                    }
                    owner.RefreshData();
                }
                finally
                {
                    AbstractBoxCommand.WaitDialogHelper.Close();
                }
            }
        }

        public override bool IsEnabled
        {
            get
            {
                return (base.Codon.Properties.Contains("prodef_id") || ((Dj.CurrentProdef != null) && (Dj.AddProinstNum > 0)));
            }
            set
            {
                base.IsEnabled = value;
            }
        }
    }
}

