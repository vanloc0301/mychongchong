namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Security;
    using SkyMap.Net.Workflow.Client.Box;
    using System;
    using System.Windows.Forms;

    public class SaveAsProinstCommand : AbstractRoleSecurityCommand
    {
        public override void Run()
        {
            IWfBox owner = this.Owner as IWfBox;
            if (owner != null)
            {
                InputBox box2 = new InputBox("请输入使用的远程DAO对象地址：", "提示", "http://127.0.0.1:7502/DBDAO");
                if (box2.ShowDialog(WorkbenchSingleton.MainForm) == DialogResult.OK)
                {
                    string result = box2.Result;
                    if (!string.IsNullOrEmpty(result))
                    {
                        WaitDialogHelper.Show();
                        try
                        {
                            (owner as WfBox).SaveAsProinsts(result);
                        }
                        catch (Exception exception)
                        {
                            MessageHelper.ShowInfo("发生错误:{0}", exception.Message);
                            LoggingService.Error(exception);
                        }
                        finally
                        {
                            WaitDialogHelper.Close();
                        }
                    }
                }
            }
        }
    }
}

