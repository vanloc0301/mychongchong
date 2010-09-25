namespace SkyMap.Net.Workflow.Client
{
    using SkyMap.Net.BrowserDisplayBinding;
    using SkyMap.Net.Commands;
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Gui.Dialogs;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Resources;
    using System.Threading;
    using System.Windows.Forms;

    public class AppEntry
    {
        private static string[] commandLineArgs;

        private static void HandleMainException(Exception ex)
        {
            LoggingService.Fatal(ex.Message, ex);
        }

        [STAThread]
        public static void Main(string[] args)
        {
            FileUtility.ApplicationRootPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "..");
            Environment.CurrentDirectory = FileUtility.ApplicationRootPath;
            if (Debugger.IsAttached)
            {
                Run(args);
            }
            else
            {
                try
                {
                    Run(args);
                }
                catch (Exception exception)
                {
                    try
                    {
                        HandleMainException(exception);
                    }
                    catch (Exception exception2)
                    {
                        MessageBox.Show(exception2.ToString(), "Critical error (Logging service defect?)");
                    }
                }
            }
        }

        private static void RegisterDoozers()
        {
            AddInTree.Doozers.Add("Pad", new PadDoozer());
            AddInTree.Doozers.Add("MenuItem", new MenuItemDoozer());
            AddInTree.Doozers.Add("ToolbarItem", new ToolbarItemDoozer());
            AddInTree.Doozers.Add("BrowserSchemeExtension", new SchemeExtensionDoozer());
            AddInTree.Doozers.Add("DisplayBinding", new DisplayBindingDoozer());
            AddInTree.Doozers.Add("DialogPanel", new DialogPanelDoozer());
            MenuCommand.LinkCommandCreator = delegate (string link) {
                return new LinkCommand(link);
            };
        }

        private static void Run(string[] args)
        {
            Control.CheckForIllegalCrossThreadCalls = true;
            commandLineArgs = args;
            bool flag = false;
            Application.SetCompatibleTextRenderingDefault(false);
            SplashScreenForm.SetCommandLineArgs(args);
            foreach (string str in SplashScreenForm.GetParameterList())
            {
                if ("nologo".Equals(str, StringComparison.OrdinalIgnoreCase))
                {
                    flag = true;
                }
            }
            if (!flag)
            {
                SplashScreenForm.SplashScreen.Show();
            }
            try
            {
                RunApplication();
            }
            finally
            {
                if (SplashScreenForm.SplashScreen != null)
                {
                    SplashScreenForm.SplashScreen.Dispose();
                }
            }
        }

        private static void RunApplication()
        {
            LoggingService.Info("Starting Application...");
            try
            {
                if (!Debugger.IsAttached)
                {
                    Application.ThreadException += new ThreadExceptionEventHandler(AppEntry.ShowErrorBox);
                    AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(AppEntry.ShowErrorBox);
                }
                RightToLeftConverter.RightToLeftLanguages = new string[0];
                Assembly assembly = typeof(AppEntry).Assembly;
                CoreStartup startup = new CoreStartup("评估管理系统");
                LoggingService.Info("Starting core services...");
                startup.StartCoreServices();
                ResourceService.RegisterNeutralStrings(new ResourceManager("Resources.StringResources", assembly));
                ResourceService.RegisterNeutralImages(new ResourceManager("Resources.BitmapResources", assembly));
                RegisterDoozers();
                LoggingService.Info("Looking for AddIns...");
                startup.AddAddInsFromDirectory(Path.Combine(FileUtility.ApplicationRootPath, "AddIns"));
                startup.ConfigureExternalAddIns(Path.Combine(PropertyService.ConfigDirectory, "AddIns.xml"));
                startup.ConfigureUserAddIns(Path.Combine(PropertyService.ConfigDirectory, "AddInInstallTemp"), Path.Combine(PropertyService.ConfigDirectory, "AddIns"));
                LoggingService.Info("Loading AddInTree...");
                startup.RunInitialization();
                LoggingService.Info("Initializing workbench...");
                WorkbenchSingleton.InitializeWorkbench();
                if (SplashScreenForm.SplashScreen != null)
                {
                    SplashScreenForm.SplashScreen.Dispose();
                }
                bool flag = true;
                try
                {
                    LoggingService.Info("Starting workbench...");
                    new StartWorkbenchCommand().Run(SplashScreenForm.GetRequestedFileList());
                    flag = false;
                }
                finally
                {
                    LoggingService.Info("Unloading services...");
                    try
                    {
                        FileService.Unload();
                        PropertyService.Save();
                    }
                    catch (Exception exception)
                    {
                        if (flag)
                        {
                            LoggingService.Warn("Exception during unloading after exception", exception);
                        }
                        else
                        {
                            MessageService.ShowError(exception);
                        }
                    }
                }
            }
            finally
            {
                LoggingService.Info("Leaving RunApplication()");
            }
        }

        private static void ShowErrorBox(Exception exception, string message)
        {
            ShowErrorBox(exception, message, false);
        }

        private static void ShowErrorBox(object sender, ThreadExceptionEventArgs e)
        {
            LoggingService.Error("ThreadException caught", e.Exception);
            ShowErrorBox(e.Exception, null);
        }

        private static void ShowErrorBox(object sender, UnhandledExceptionEventArgs e)
        {
            Exception exceptionObject = e.ExceptionObject as Exception;
            LoggingService.Fatal("UnhandledException caught", exceptionObject);
            if (e.IsTerminating)
            {
                LoggingService.Fatal("Runtime is terminating because of unhandled exception.");
            }
            ShowErrorBox(exceptionObject, "Unhandled exception", e.IsTerminating);
        }

        private static void ShowErrorBox(Exception exception, string message, bool mustTerminate)
        {
            if (!(exception is InvalidOperationException) && !(exception is InvalidProgramException))
            {
                try
                {
                    using (ExceptionBox box = new ExceptionBox(exception, message, mustTerminate))
                    {
                        try
                        {
                            box.ShowDialog(WorkbenchSingleton.MainForm);
                        }
                        catch (InvalidOperationException)
                        {
                            box.ShowDialog();
                        }
                    }
                }
                catch (Exception exception2)
                {
                    LoggingService.Warn("Error showing ExceptionBox", exception2);
                    MessageBox.Show(exception.ToString());
                }
            }
        }

        private static void ShowErrorMessage(Exception e)
        {
            if (e is CoreException)
            {
                MessageHelper.ShowBaseExceptionInfo(e as CoreException);
            }
            else
            {
                MessageHelper.ShowError(e.Message, e);
            }
        }

        private static void ShowErrorMessage(object sender, ThreadExceptionEventArgs eargs)
        {
            ShowErrorMessage(eargs.Exception);
        }

        private static void ShowErrorMessage(object sender, UnhandledExceptionEventArgs e)
        {
            Exception exceptionObject = e.ExceptionObject as Exception;
            LoggingService.Fatal("UnhandledException caught", exceptionObject);
            if (e.IsTerminating)
            {
                LoggingService.Fatal("Runtime is terminating because of unhandled exception.");
            }
            ShowErrorMessage(exceptionObject);
        }

        public static string[] CommandLineArgs
        {
            get
            {
                return commandLineArgs;
            }
        }
    }
}

