namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Workflow.Client.Services;
    using System;
    using System.Configuration;

    public class OpenDefaultBox : AbstractCommand
    {
        public override void Run()
        {
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.Info("开始打开缺省工作箱");
            }
            string str = PropertyService.Get<string>("DefaultBox", null);
            if (string.IsNullOrEmpty(str))
            {
                str = ConfigurationSettings.AppSettings["DefaultBox"];
            }
            if (!string.IsNullOrEmpty(str))
            {
                BoxService.OpenBox(str);
            }
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.Info("打开缺省工作箱完成");
            }
        }
    }
}

