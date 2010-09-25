namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Security;
    using System;
    using System.Windows.Forms;

    public class UpdateDAOCacheVersionCommand : AbstractRoleSecurityCommand
    {
        public override void Run()
        {
            if (this.IsEnabled && (MessageHelper.ShowYesNoInfo("你真的要更新数据库缓存版本吗，这将会使客户端重新更新整个缓存数据") == DialogResult.Yes))
            {
                InputBox box = new InputBox("请输入使用的远程DAO对象地址,\r\n如果取消将更新当前服务器。", "提示：", "http://127.0.0.1:7502/DBDAO");
                string result = string.Empty;
                if (box.ShowDialog(WorkbenchSingleton.MainForm) == DialogResult.OK)
                {
                    result = box.Result;
                    if (string.IsNullOrEmpty(result))
                    {
                        MessageHelper.ShowInfo("没有输入远程DAO对象地址");
                        return;
                    }
                }
                try
                {
                    DBSessionFactory.GetInstanceByUrl(result).UpdateVersion();
                    MessageHelper.ShowInfo("更新数据库缓存版本成功!");
                }
                catch (Exception exception)
                {
                    MessageHelper.ShowError("更新数据库缓存时发生错误", exception);
                    LoggingService.Error(exception);
                }
            }
        }
    }
}

