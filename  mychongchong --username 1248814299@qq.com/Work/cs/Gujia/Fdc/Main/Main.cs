using System;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Resources;
using System.Xml;
using System.Threading;
using System.Runtime.Remoting;
using System.Security.Policy;

using SkyMap.Net.Core;
using SkyMap.Net.Commands;
using SkyMap.Net.Gui;
using SkyMap.Net.Gui.Dialogs;
namespace SkyMap.Net.Workflow.Client
{
    /// <summary>
    /// This Class is the Core main class, it starts the program.
    /// </summary>
    public class AppEntry
    {
        static string[] commandLineArgs = null;

        public static string[] CommandLineArgs
        {
            get
            {
                return commandLineArgs;
            }
        }

        static void ShowErrorMessage(object sender, ThreadExceptionEventArgs eargs)
        {
            ShowErrorMessage(eargs.Exception);

        }

        static void ShowErrorMessage(Exception e)
        {
            if (e is CoreException)
                MessageHelper.ShowBaseExceptionInfo(e as CoreException);
            else
                MessageHelper.ShowError(e.Message, e);
        }

        /// <summary>
        /// Starts the core of SharpDevelop.
        /// </summary>
        [STAThread()]
        public static void Main(string[] args)
        {

#if DEBUG
            if (Debugger.IsAttached)
            {
                Run(args);
                return;
            }
#endif
            // Do not use LoggingService here (see comment in Run(string[]))
            try
            {
                //×Ô¶¯Éý¼¶
                //SkyMap.Net.Gui.AutoUpdateHepler.TryUpdate();
                Run(args);
            }
            catch (Exception ex)
            {
                try
                {
                    HandleMainException(ex);
                }
                catch (Exception loadError)
                {
                    // HandleMainException can throw error when log4net is not found
                    MessageBox.Show(loadError.ToString(), "Critical error (Logging service defect?)");
                }
            }


        }

        static void HandleMainException(Exception ex)
        {
            LoggingService.Fatal(ex.Message, ex);
            try
            {
                //Application.Run(new ExceptionBox(ex, "Unhandled exception terminated SharpDevelop", true));
            }
            catch
            {
                MessageBox.Show(ex.ToString(), "Critical error (cannot use ExceptionBox)");
            }
        }

        #region show error box
        static void ShowErrorBox(object sender, ThreadExceptionEventArgs e)
        {
            LoggingService.Error("ThreadException caught", e.Exception);
            ShowErrorBox(e.Exception, null);
        }

        static void ShowErrorBox(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            LoggingService.Fatal("UnhandledException caught", ex);
            if (e.IsTerminating)
                LoggingService.Fatal("Runtime is terminating because of unhandled exception.");
            ShowErrorBox(ex, "Unhandled exception", e.IsTerminating);
        }

        static void ShowErrorBox(Exception exception, string message)
        {
            ShowErrorBox(exception, message, false);
        }

        static void ShowErrorBox(Exception exception, string message, bool mustTerminate)
        {
            if (exception is System.InvalidOperationException)
                return;
            if (exception is System.InvalidProgramException)
                return;
            try
            {
                using (ExceptionBox box = new ExceptionBox(exception, message, mustTerminate))
                {
                    try
                    {
                        box.ShowDialog(SkyMap.Net.Gui.WorkbenchSingleton.MainForm);
                    }
                    catch (InvalidOperationException)
                    {
                        box.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingService.Warn("Error showing ExceptionBox", ex);
                MessageBox.Show(exception.ToString());
            }
        }
        #endregion


        static void Run(string[] args)
        {
            // DO NOT USE LoggingService HERE!
            // LoggingService requires ICSharpCode.Core.dll and log4net.dll
            // When a method containing a call to LoggingService is JITted, the
            // libraries are loaded.
            // We want to show the SplashScreen while those libraries are loaded, so
            // don't call LoggingService.

#if DEBUG
            Control.CheckForIllegalCrossThreadCalls = true;
#endif
            commandLineArgs = args;
            bool noLogo = false;

            Application.SetCompatibleTextRenderingDefault(false);
            SplashScreenForm.SetCommandLineArgs(args);

            foreach (string parameter in SplashScreenForm.GetParameterList())
            {
                if ("nologo".Equals(parameter, StringComparison.OrdinalIgnoreCase))
                    noLogo = true;
            }

            if (!noLogo)
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

        static void RunApplication()
        {
            LoggingService.Info("Starting Application...");
            try
            {
#if DEBUG
                if (!Debugger.IsAttached)
                {
                    Application.ThreadException += ShowErrorBox;
                    AppDomain.CurrentDomain.UnhandledException += ShowErrorBox;
                }
#else
				Application.ThreadException += ShowErrorBox;
				AppDomain.CurrentDomain.UnhandledException += ShowErrorBox;
				MessageService.CustomErrorReporter = ShowErrorBox;
#endif
                // disable RTL: translations for the RTL languages are inactive
                RightToLeftConverter.RightToLeftLanguages = new string[0];

                Assembly exe = typeof(AppEntry).Assembly;

                FileUtility.ApplicationRootPath = Path.Combine(Path.GetDirectoryName(exe.Location), "..");

                CoreStartup c = new CoreStartup("SMOA");
                c.ConfigDirectory = FileUtility.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".SkyMapSoft", ((AssemblyProductAttribute)exe.GetCustomAttributes(typeof(AssemblyProductAttribute), true)[0]).Product) + Path.DirectorySeparatorChar;
                LoggingService.Info("Starting core services...");
                c.StartCoreServices();

                ResourceService.RegisterNeutralStrings(new ResourceManager("Resources.StringResources", exe));
                ResourceService.RegisterNeutralImages(new ResourceManager("Resources.BitmapResources", exe));

                RegisterDoozers();

                LoggingService.Info("Looking for AddIns...");
                c.AddAddInsFromDirectory(Path.Combine(FileUtility.ApplicationRootPath, "AddIns"));

                c.ConfigureExternalAddIns(Path.Combine(PropertyService.ConfigDirectory, "AddIns.xml"));
                c.ConfigureUserAddIns(Path.Combine(PropertyService.ConfigDirectory, "AddInInstallTemp"),
                                      Path.Combine(PropertyService.ConfigDirectory, "AddIns"));

                LoggingService.Info("Loading AddInTree...");
                c.RunInitialization();

                LoggingService.Info("Initializing workbench...");
                // .NET base autostarts
                // taken out of the add-in tree for performance reasons (every tick in startup counts)
                WorkbenchSingleton.InitializeWorkbench();

                if (SplashScreenForm.SplashScreen != null)
                {
                    SplashScreenForm.SplashScreen.Dispose();
                }

                bool exception = true;
                // finally start the workbench.
                try
                {
                    LoggingService.Info("Starting workbench...");
                    new StartWorkbenchCommand().Run(SplashScreenForm.GetRequestedFileList());
                    exception = false;
                }
                finally
                {
                    LoggingService.Info("Unloading services...");
                    try
                    {
                        FileService.Unload();
                        PropertyService.Save();
                        //CacheService.Unload();
                    }
                    catch (Exception ex)
                    {
                        if (exception)
                            LoggingService.Warn("Exception during unloading after exception", ex);
                        else
                            MessageService.ShowError(ex);
                    }
                }
            }
            finally
            {
                LoggingService.Info("Leaving RunApplication()");
            }
        }

        static void ShowErrorMessage(object sender, UnhandledExceptionEventArgs e)
        {

            Exception ex = e.ExceptionObject as Exception;
            LoggingService.Fatal("UnhandledException caught", ex);
            if (e.IsTerminating)
                LoggingService.Fatal("Runtime is terminating because of unhandled exception.");
            ShowErrorMessage(ex);

        }

        static void RegisterDoozers()
        {
            AddInTree.Doozers.Add("Pad", new PadDoozer());
            AddInTree.Doozers.Add("MenuItem", new MenuItemDoozer());
            AddInTree.Doozers.Add("ToolbarItem", new ToolbarItemDoozer());
            AddInTree.Doozers.Add("BrowserSchemeExtension", new BrowserDisplayBinding.SchemeExtensionDoozer());
            AddInTree.Doozers.Add("DisplayBinding", new DisplayBindingDoozer());
            AddInTree.Doozers.Add("DialogPanel", new DialogPanelDoozer());

            MenuCommand.LinkCommandCreator = delegate(string link) { return new LinkCommand(link); };
        }
    }
}
