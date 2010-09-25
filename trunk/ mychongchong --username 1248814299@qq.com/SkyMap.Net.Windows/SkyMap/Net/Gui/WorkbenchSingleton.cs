namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public static class WorkbenchSingleton
    {
        private static STAThreadCaller caller;
        private const string uiIconStyle = "IconMenuItem.IconMenuStyle";
        private const string uiLanguageProperty = "CoreProperties.UILanguage";
        private static DefaultWorkbench workbench = null;
        private const string workbenchMemento = "WorkbenchMemento";

        public static  event EventHandler WorkbenchCreated;

        public static void InitializeWorkbench()
        {
            workbench = new DefaultWorkbench();
            MessageService.MainForm = workbench;
            PropertyService.PropertyChanged += new PropertyChangedEventHandler(WorkbenchSingleton.TrackPropertyChanges);
            ResourceService.LanguageChanged += delegate {
                workbench.RedrawAllComponents();
            };
            caller = new STAThreadCaller(workbench);
            workbench.InitializeWorkspace();
            workbench.SetMemento(PropertyService.Get<Properties>("WorkbenchMemento", new Properties()));
            workbench.WorkbenchLayout = new SdiWorkbenchLayout();
            OnWorkbenchCreated();
        }

        private static void OnWorkbenchCreated()
        {
            if (WorkbenchCreated != null)
            {
                WorkbenchCreated(null, EventArgs.Empty);
            }
        }

        public static void SafeThreadAsyncCall(Delegate method, params object[] arguments)
        {
            caller.BeginCall(method, arguments);
        }

        public static void SafeThreadAsyncCall(object target, string methodName, params object[] arguments)
        {
            caller.BeginCall(target, methodName, arguments);
        }

        public static object SafeThreadCall(Delegate method, params object[] arguments)
        {
            return caller.Call(method, arguments);
        }

        public static object SafeThreadCall(object target, string methodName, params object[] arguments)
        {
            return caller.Call(target, methodName, arguments);
        }

        private static void TrackPropertyChanges(object sender, PropertyChangedEventArgs e)
        {
            if ((e.OldValue != e.NewValue) && (workbench != null))
            {
                string key = e.Key;
                if (key != null)
                {
                    if ((!(key == "SkyMap.Net.Gui.StatusBarVisible") && !(key == "SkyMap.Net.Gui.VisualStyle")) && !(key == "SkyMap.Net.Gui.ToolBarVisible"))
                    {
                        if (key == "SkyMap.Net.Gui.UseProfessionalRenderer")
                        {
                            workbench.UpdateRenderer();
                        }
                    }
                    else
                    {
                        workbench.RedrawAllComponents();
                    }
                }
            }
        }

        public static Control ActiveControl
        {
            get
            {
                Control activeControl;
                ContainerControl mainForm = MainForm;
                do
                {
                    activeControl = mainForm.ActiveControl;
                    if (activeControl == null)
                    {
                        return mainForm;
                    }
                    mainForm = activeControl as ContainerControl;
                }
                while (mainForm != null);
                return activeControl;
            }
        }

        public static bool InvokeRequired
        {
            get
            {
                return workbench.InvokeRequired;
            }
        }

        public static Form MainForm
        {
            get
            {
                return workbench;
            }
        }

        public static IWorkbench Workbench
        {
            get
            {
                return workbench;
            }
        }

        private class STAThreadCaller
        {
            private Control ctl;
            private PerformCallDelegate performCallDelegate;

            public STAThreadCaller(Control ctl)
            {
                this.ctl = ctl;
                this.performCallDelegate = new PerformCallDelegate(this.DoPerformCall);
            }

            public void BeginCall(Delegate method, object[] arguments)
            {
                if (method == null)
                {
                    throw new ArgumentNullException("method");
                }
                this.ctl.BeginInvoke(method, arguments);
            }

            public void BeginCall(object target, string methodName, object[] arguments)
            {
                if (target == null)
                {
                    throw new ArgumentNullException("target");
                }
                this.ctl.BeginInvoke(this.performCallDelegate, new object[] { target, methodName, arguments });
            }

            public object Call(Delegate method, object[] arguments)
            {
                if (method == null)
                {
                    throw new ArgumentNullException("method");
                }
                return this.ctl.Invoke(method, arguments);
            }

            public object Call(object target, string methodName, object[] arguments)
            {
                if (target == null)
                {
                    throw new ArgumentNullException("target");
                }
                return this.ctl.Invoke(this.performCallDelegate, new object[] { target, methodName, arguments });
            }

            private object DoPerformCall(object target, string methodName, object[] arguments)
            {
                MethodInfo method = null;
                if (target is System.Type)
                {
                    method = ((System.Type) target).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
                }
                else
                {
                    method = target.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                }
                if (method == null)
                {
                    throw new ArgumentException("method not found : " + methodName);
                }
                try
                {
                    if (target is System.Type)
                    {
                        return method.Invoke(null, arguments);
                    }
                    return method.Invoke(target, arguments);
                }
                catch (Exception innerException)
                {
                    if ((innerException is TargetInvocationException) && (innerException.InnerException != null))
                    {
                        innerException = innerException.InnerException;
                    }
                    MessageService.ShowError(innerException, "Exception got.");
                }
                return null;
            }

            private delegate object PerformCallDelegate(object target, string methodName, object[] arguments);
        }
    }
}

