namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Gui;
    using System;

    public class WfViewSaveCommand : AbstractWfViewCommand
    {
        public override void Run()
        {
            try
            {
                if (!base.view.Save())
                {
                    MessageHelper.ShowInfo("数据保存不成功!\r\n请关闭本窗口，重新打开业务后使用【载入最近未保存成功数据】尝试再次保存本次编辑的数据");
                }
            }
            catch (Exception exception)
            {
                MessageHelper.ShowError("数据保存不成功!\r\n请关闭本窗口，重新打开业务后使用【载入最近未保存成功数据】尝试再次保存本次编辑的数据", exception);
            }
        }

        public override bool IsEnabled
        {
            get
            {
                return base.view.Changed;
            }
            set
            {
                base.IsEnabled = value;
            }
        }
    }
}

