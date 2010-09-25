namespace SkyMap.Net.Server
{
    using Quartz.Server.Core;
    using SkyMap.Net.Core;
    using SkyMap.Net.Criteria;
    using SkyMap.Net.DAO;
    using SkyMap.Net.DataForms;
    using SkyMap.Net.DataForms.DataEngine;
    using SkyMap.Net.Workflow.Engine;
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Remoting;
    using System.Runtime.Remoting.Channels;
    using System.Runtime.Remoting.Channels.Http;
    using System.Runtime.Serialization.Formatters;
    using System.ServiceProcess;
    using System.Threading;

    internal static class Program
    {
        public static ServiceBase[] ServicesToRun;

        private static void ControlService(string status)
        {
            try
            {
                ServiceController controller = new ServiceController(ZXService.LookupServiceName());
                string str = status;
                if (str != null)
                {
                    if (!(str == "start"))
                    {
                        if (str == "stop")
                        {
                            goto Label_005A;
                        }
                        if (str == "restart")
                        {
                            goto Label_00A2;
                        }
                    }
                    else if (controller.Status != ServiceControllerStatus.Running)
                    {
                        controller.Start();
                    }
                }
                return;
            Label_005A:
                if (controller.Status == ServiceControllerStatus.Running)
                {
                    controller.Stop();
                }
                while (controller.Status != ServiceControllerStatus.Stopped)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1.0));
                    controller.Refresh();
                }
                return;
            Label_00A2:
                if (controller.Status == ServiceControllerStatus.Running)
                {
                    controller.Stop();
                    while (controller.Status != ServiceControllerStatus.Stopped)
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(1.0));
                        controller.Refresh();
                    }
                    Console.WriteLine("服务已停止");
                }
                if (controller.Status != ServiceControllerStatus.Running)
                {
                    controller.Start();
                    Console.Write("服务正在重新启动");
                }
            }
            catch (Exception exception)
            {
                MessageService.ShowError(exception);
            }
        }

        private static void Main(string[] args)
        {
            FileUtility.ApplicationRootPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..");
            Environment.CurrentDirectory = FileUtility.ApplicationRootPath;
            if (args.Length > 0)
            {
                string str = args[0].ToLower();
                if ((str != null) && (((str == "start") || (str == "stop")) || (str == "restart")))
                {
                    ControlService(args[0].ToLower());
                    return;
                }
            }
            LoggingService.Info("Start application....");
            CoreStartup startup = new CoreStartup("置信远程应用服务");
            startup.RunInitialization();
            startup.StartCoreServices();
            if (ConfigurationSettings.AppSettings["remoting"] != "off")
            {
                RegisterRemoting();
                DAOCacheService.LoadCaches();
            }
            if (args.Length == 0)
            {
                StartService();
            }
            else
            {
                RunAsConsole();
            }
        }

        private static void RegisterRemoting()
        {
            Hashtable hashtable = new Hashtable();
            BinaryServerFormatterSinkProvider sinkProvider = new BinaryServerFormatterSinkProvider {
                TypeFilterLevel = TypeFilterLevel.Full
            };
            HttpServerChannel chnl = new HttpServerChannel("HttpBinary", HttpPort, sinkProvider);
            ChannelServices.RegisterChannel(chnl, false);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(HibernateImpl), "DBDAO", WellKnownObjectMode.Singleton);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(WorkflowClient), "WorkflowClient", WellKnownObjectMode.Singleton);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(SQLDataEngine), "DataEngine", WellKnownObjectMode.Singleton);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(RemoteUpdate), "RemoteUpdate", WellKnownObjectMode.Singleton);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(DataWordService), "DataWordDAO", WellKnownObjectMode.Singleton);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(DataFormDAOService), "DataFormDAOService", WellKnownObjectMode.Singleton);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(CriteriaDAOService), "CriteriaDAOService", WellKnownObjectMode.Singleton);
            //RemotingConfiguration.RegisterWellKnownServiceType(typeof(ZSTaxService), "ZSTaxService", WellKnownObjectMode.Singleton);
            RemotingConfiguration.CustomErrorsMode = CustomErrorsModes.Off;
            RemotingConfiguration.CustomErrorsEnabled(false);
        }

        private static void RunAsConsole()
        {
            IQuartzServer server;
            try
            {
                server = QuartzServerFactory.CreateServer();
                server.Initialize();
                server.Start();
            }
            catch (Exception exception)
            {
                Console.Write("Error starting server: " + exception.Message);
                Console.WriteLine(exception.ToString());
                Console.WriteLine("Hit any key to close");
                Console.Read();
                return;
            }
            string strA = "";
            while (string.Compare(strA, "0", true, CultureInfo.InvariantCulture) != 0)
            {
                Console.WriteLine("Press a key and ENTER: G=GC.Collect, 0=Exit");
                strA = Console.ReadLine();
                Console.WriteLine("Pressed: " + strA);
                if (string.Compare(strA, "G", true, CultureInfo.InvariantCulture) == 0)
                {
                    Console.WriteLine("GC Collect - start");
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    Console.WriteLine("GC Collect - done");
                }
            }
            if (server != null)
            {
                server.Stop();
            }
        }

        private static void StartService()
        {
            ServicesToRun = new ServiceBase[] { new ZXService() };
            ServiceBase.Run(ServicesToRun);
        }

        private static int HttpPort
        {
            get
            {
                try
                {
                    if (!string.IsNullOrEmpty(ConfigurationSettings.AppSettings["HttpPort"]))
                    {
                        return Convert.ToInt32(ConfigurationSettings.AppSettings["HttpPort"]);
                    }
                }
                catch
                {
                }
                return 0x1d4e;
            }
        }
    }
}

