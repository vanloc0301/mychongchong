namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class PropertyView : AbstractViewContent, IContextHelpProvider
    {
        private PropertyViewContainer activeContainer;
        private bool inUpdate = false;
        private bool isDirty = false;
        private Panel panel = new Panel();
        private IPropertyEditPanel propertyEditPanel;

        public static  event PropertyValueChangedEventHandler PropertyValueChanged;

        public void CancelEdit()
        {
            this.propertyEditPanel.CancelEdit();
        }

        public override void Dispose()
        {
            base.Dispose();
            if (this.propertyEditPanel != null)
            {
                try
                {
                    this.propertyEditPanel.SelectedObject = null;
                }
                catch
                {
                }
                this.propertyEditPanel.Dispose();
                this.propertyEditPanel = null;
            }
        }

        private static void OnPropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            if (PropertyValueChanged != null)
            {
                PropertyValueChanged(sender, e);
            }
        }

        public override void RedrawContent()
        {
            this.propertyEditPanel.RedrawContent();
        }

        public void Save()
        {
            this.propertyEditPanel.Save();
            this.isDirty = false;
        }

        public override void Save(string fileName)
        {
            this.Save();
        }

        private void SetActiveContainer(PropertyViewContainer pc)
        {
            if ((this.activeContainer != pc) && (pc != null))
            {
                this.activeContainer = pc;
                UpdateSelectedObjectIfActive(pc);
                UpdateSelectableIfActive(pc);
            }
        }

        private void SetDesignableObject(object obj)
        {
            this.inUpdate = true;
            this.propertyEditPanel.SelectedObject = obj;
            this.inUpdate = false;
        }

        public void ShowHelp()
        {
            LoggingService.Info("Show help on property pad");
        }

        internal static void UpdateSelectableIfActive(PropertyViewContainer container)
        {
            PropertyView instance = Instance;
            if ((instance != null) && ((container != null) && (instance.activeContainer == container)))
            {
                LoggingService.Debug("UpdateSelectableIfActive");
                instance.SetDesignableObject(container.SelectedObject);
            }
        }

        internal static void UpdateSelectedObjectIfActive(PropertyViewContainer container)
        {
            LoggingService.Debug("will UpdateSelectedObjectIfActive");
            PropertyView instance = Instance;
            if ((instance != null) && ((container != null) && (instance.activeContainer == container)))
            {
                LoggingService.Debug("UpdateSelectedObjectIfActive");
                instance.SetDesignableObject(container.SelectedObject);
            }
        }

        private void WorkbenchWindowChanged(object sender, EventArgs e)
        {
            IHasPropertyViewContainer activeContent = WorkbenchSingleton.Workbench.ActiveContent as IHasPropertyViewContainer;
            if (activeContent == null)
            {
                IWorkbenchWindow activeWorkbenchWindow = WorkbenchSingleton.Workbench.ActiveWorkbenchWindow;
                if (activeWorkbenchWindow != null)
                {
                    activeContent = activeWorkbenchWindow.ActiveViewContent as IHasPropertyViewContainer;
                }
            }
            if (activeContent != null)
            {
                this.SetActiveContainer(activeContent.PropertyViewContainer);
            }
        }

        public override System.Windows.Forms.Control Control
        {
            get
            {
                return this.panel;
            }
        }

        public static PropertyView Instance
        {
            get
            {
                PropertyView view = null;
                foreach (IViewContent content in WorkbenchSingleton.Workbench.ViewContentCollection)
                {
                    if (content is PropertyView)
                    {
                        view = (PropertyView) content;
                        break;
                    }
                }
                if (view == null)
                {
                    view = new PropertyView();
                    WorkbenchSingleton.Workbench.ShowView(view);
                }
                return view;
            }
        }

        public override bool IsDirty
        {
            get
            {
                return this.isDirty;
            }
            set
            {
                LoggingService.InfoFormatted("设置IsDirty值为：{0}", new object[] { value });
                this.isDirty = value;
                OnPropertyValueChanged(null, null);
            }
        }

        internal IPropertyEditPanel PropertyEditPanel
        {
            get
            {
                return this.propertyEditPanel;
            }
            set
            {
                if ((this.propertyEditPanel == null) || (((this.propertyEditPanel != null) && (this.propertyEditPanel is System.Windows.Forms.Control)) && (this.propertyEditPanel != value)))
                {
                    if (this.propertyEditPanel != null)
                    {
                        this.panel.Controls.Remove(this.propertyEditPanel as System.Windows.Forms.Control);
                    }
                    this.propertyEditPanel = value;
                    if (value is System.Windows.Forms.Control)
                    {
                        System.Windows.Forms.Control control = value as System.Windows.Forms.Control;
                        control.Dock = DockStyle.Fill;
                        this.panel.Controls.Add(control);
                    }
                    if (LoggingService.IsInfoEnabled)
                    {
                        LoggingService.Info("加载了属性编辑器");
                    }
                }
            }
        }

        public override string TitleName
        {
            get
            {
                return "属性";
            }
            set
            {
                base.TitleName = value;
            }
        }
    }
}

