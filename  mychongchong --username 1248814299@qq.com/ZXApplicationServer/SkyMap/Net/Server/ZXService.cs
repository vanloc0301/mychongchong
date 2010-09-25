namespace SkyMap.Net.Server
{
    using Quartz.Server.Core;
    using SkyMap.Net.Core;
    using System;
    using System.ComponentModel;
    using System.Reflection;
    using System.ServiceProcess;
    using System.Xml.XPath;

    internal class ZXService : ServiceBase
    {
        private IContainer components = null;
        private readonly string DefaultDirectory = AppDomain.CurrentDomain.BaseDirectory;
        public const string DefaultServiceName = "置信远程应用程序服务";
        private readonly IQuartzServer server;

        public ZXService()
        {
            base.ServiceName = LookupServiceName();
            try
            {
                LoggingService.Debug("Obtaining instance of an IQuartzServer");
                this.server = QuartzServerFactory.CreateServer();
                LoggingService.Debug("Initializing server");
                this.server.Initialize();
                LoggingService.Debug("Server initialized");
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.components != null)
                {
                    this.components.Dispose();
                }
                if (this.server != null)
                {
                    this.server.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            base.ServiceName = "行政在线网上数据交换服务";
        }

        internal static string LookupServiceName()
        {
            string str = string.Empty;
            XPathNavigator navigator = new XPathDocument(Assembly.GetExecutingAssembly().Location + ".config").CreateNavigator();
            XPathExpression expr = navigator.Compile("string(/configuration/appSettings/add[@key='service.name']/@value)");
            str = (string) navigator.Evaluate(expr);
            return (string.IsNullOrEmpty(str) ? "置信远程应用程序服务" : str);
        }

        protected override void OnStart(string[] args)
        {
            LoggingService.InfoFormatted("{0}启动...", new object[] { base.ServiceName });
            try
            {
                LoggingService.Info("启动任务调度服务支持...");
                this.server.Start();
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
            }
        }

        protected override void OnStop()
        {
            LoggingService.InfoFormatted("{0}停止...", new object[] { base.ServiceName });
            try
            {
                LoggingService.Info("停止任务调度服务支持...");
                this.server.Stop();
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
            }
        }
    }
}

