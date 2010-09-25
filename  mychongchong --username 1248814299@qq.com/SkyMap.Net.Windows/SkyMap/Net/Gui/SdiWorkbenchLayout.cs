namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using WeifenLuo.WinFormsUI.Docking;

    public class SdiWorkbenchLayout : IWorkbenchLayout
    {
        private Dictionary<string, PadContentWrapper> contentHash = new Dictionary<string, PadContentWrapper>();
        private DockPanel dockPanel;
        private DockContent lastActiveContent;
        private AutoHideMenuStripContainer mainMenuContainer;
        private IWorkbenchWindow oldSelectedWindow = null;
        private AutoHideStatusStripContainer statusStripContainer;
        private ToolStripPanel toolBarPanel;
        private DefaultWorkbench wbForm;

        public event EventHandler ActiveWorkbenchWindowChanged;

        public void ActivatePad(PadDescriptor padContent)
        {
            if ((padContent != null) && this.contentHash.ContainsKey(padContent.Class))
            {
                this.contentHash[padContent.Class].Show();
            }
        }

        public void ActivatePad(string fullyQualifiedTypeName)
        {
            this.contentHash[fullyQualifiedTypeName].Show();
        }

        private void ActiveContentChanged(object sender, EventArgs e)
        {
            this.OnActiveWorkbenchWindowChanged(e);
        }

        private void ActiveMdiChanged(object sender, EventArgs e)
        {
            this.OnActiveWorkbenchWindowChanged(e);
        }

        public void Attach(IWorkbench workbench)
        {
            this.wbForm = (DefaultWorkbench) workbench;
            this.wbForm.SuspendLayout();
            this.wbForm.Controls.Clear();
            this.dockPanel = new DockPanel();
            this.dockPanel.DocumentStyle = DocumentStyle.DockingWindow;
            this.dockPanel.ActiveAutoHideContent = null;
            this.dockPanel.Dock = DockStyle.Fill;
            this.mainMenuContainer = new AutoHideMenuStripContainer(this.wbForm.TopMenu);
            this.mainMenuContainer.Dock = DockStyle.Top;
            this.toolBarPanel = new ToolStripPanel();
            if (this.wbForm.ToolBars != null)
            {
                this.toolBarPanel.Controls.AddRange(this.wbForm.ToolBars);
            }
            this.toolBarPanel.Dock = DockStyle.Top;
            this.statusStripContainer = new AutoHideStatusStripContainer((StatusStrip) StatusBarService.Control);
            this.statusStripContainer.Dock = DockStyle.Bottom;
            this.wbForm.Controls.Add(this.dockPanel);
            this.wbForm.Controls.Add(this.toolBarPanel);
            this.wbForm.Controls.Add(this.mainMenuContainer);
            this.wbForm.Controls.Add(this.statusStripContainer);
            this.wbForm.MainMenuStrip = this.wbForm.TopMenu;
            this.LoadLayoutConfiguration();
            this.ShowPads();
            this.ShowViewContents();
            this.RedrawAllComponents();
            this.dockPanel.ActiveDocumentChanged += new EventHandler(this.ActiveMdiChanged);
            this.dockPanel.ActiveContentChanged += new EventHandler(this.ActiveContentChanged);
            this.ActiveMdiChanged(this, EventArgs.Empty);
            this.wbForm.ResumeLayout(false);
            PropertyService.Get<Properties>("SkyMap.Net.Gui.FullscreenOptions", new Properties()).PropertyChanged += new PropertyChangedEventHandler(this.TrackFullscreenPropertyChanges);
        }

        public void CloseWindowEvent(object sender, EventArgs e)
        {
            SdiWorkspaceWindow window = (SdiWorkspaceWindow) sender;
            window.CloseEvent -= new EventHandler(this.CloseWindowEvent);
            if (window.ViewContent != null)
            {
                this.wbForm.CloseContent(window.ViewContent);
                if (window == this.oldSelectedWindow)
                {
                    this.oldSelectedWindow = null;
                }
                this.ActiveMdiChanged(this, null);
            }
        }

        private PadContentWrapper CreateContent(PadDescriptor content)
        {
            if (this.contentHash.ContainsKey(content.Class))
            {
                return this.contentHash[content.Class];
            }
            Properties properties = PropertyService.Get<Properties>("Workspace.ViewMementos", new Properties());
            PadContentWrapper wrapper = new PadContentWrapper(content);
            if (!string.IsNullOrEmpty(content.Icon))
            {
                wrapper.Icon = IconService.GetIcon(content.Icon);
            }
            wrapper.Text = SkyMap.Net.Core.StringParser.Parse(content.Title);
            this.contentHash[content.Class] = wrapper;
            return wrapper;
        }

        public void Detach()
        {
            this.StoreConfiguration();
            this.dockPanel.ActiveDocumentChanged -= new EventHandler(this.ActiveMdiChanged);
            this.DetachPadContents(true);
            this.DetachViewContents(true);
            try
            {
                if (this.dockPanel != null)
                {
                    this.dockPanel.Dispose();
                    this.dockPanel = null;
                }
            }
            catch (Exception exception)
            {
                MessageService.ShowError(exception);
            }
            if (this.contentHash != null)
            {
                this.contentHash.Clear();
            }
            this.wbForm.Controls.Clear();
        }

        private void DetachPadContents(bool dispose)
        {
            PadContentWrapper current;
            using (Dictionary<string, PadContentWrapper>.ValueCollection.Enumerator enumerator = this.contentHash.Values.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    current.allowInitialize = false;
                }
            }
            foreach (PadDescriptor descriptor in this.wbForm.PadContentCollection)
            {
                try
                {
                    current = this.contentHash[descriptor.Class];
                    current.DockPanel = null;
                    if (dispose)
                    {
                        current.DetachContent();
                        current.Dispose();
                    }
                }
                catch (Exception exception)
                {
                    MessageService.ShowError(exception);
                }
            }
            if (dispose)
            {
                this.contentHash.Clear();
            }
        }

        private void DetachViewContents(bool dispose)
        {
            foreach (IViewContent content in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                try
                {
                    SdiWorkspaceWindow workbenchWindow = (SdiWorkspaceWindow) content.WorkbenchWindow;
                    workbenchWindow.DockPanel = null;
                    if (dispose)
                    {
                        content.WorkbenchWindow = null;
                        workbenchWindow.CloseEvent -= new EventHandler(this.CloseWindowEvent);
                        workbenchWindow.DetachContent();
                        workbenchWindow.Dispose();
                    }
                }
                catch (Exception exception)
                {
                    MessageService.ShowError(exception);
                }
            }
        }

        private static IViewContent GetActiveView()
        {
            IWorkbenchWindow activeWorkbenchWindow = WorkbenchSingleton.Workbench.ActiveWorkbenchWindow;
            if (activeWorkbenchWindow != null)
            {
                return activeWorkbenchWindow.ViewContent;
            }
            return null;
        }

        private DockContent GetContent(string padTypeName)
        {
            foreach (PadDescriptor descriptor in WorkbenchSingleton.Workbench.PadContentCollection)
            {
                if (descriptor.Class == padTypeName)
                {
                    return this.CreateContent(descriptor);
                }
            }
            return null;
        }

        public void HidePad(PadDescriptor padContent)
        {
            if ((padContent != null) && this.contentHash.ContainsKey(padContent.Class))
            {
                this.contentHash[padContent.Class].Hide();
            }
        }

        private void HideToolBars()
        {
            if (this.wbForm.ToolBars != null)
            {
                foreach (ToolStrip strip in this.wbForm.ToolBars)
                {
                    if (this.toolBarPanel.Controls.Contains(strip))
                    {
                        this.toolBarPanel.Controls.Remove(strip);
                    }
                }
            }
        }

        public bool IsVisible(PadDescriptor padContent)
        {
            return (((padContent != null) && this.contentHash.ContainsKey(padContent.Class)) && !this.contentHash[padContent.Class].IsHidden);
        }

        public void LoadConfiguration()
        {
            if (this.dockPanel != null)
            {
                LockWindowUpdate(this.wbForm.Handle);
                try
                {
                    IViewContent activeView = GetActiveView();
                    this.dockPanel.ActiveDocumentChanged -= new EventHandler(this.ActiveMdiChanged);
                    this.DetachPadContents(true);
                    this.DetachViewContents(true);
                    this.dockPanel.ActiveDocumentChanged += new EventHandler(this.ActiveMdiChanged);
                    this.LoadLayoutConfiguration();
                    this.ShowPads();
                    this.ShowViewContents();
                    if ((activeView != null) && (activeView.WorkbenchWindow != null))
                    {
                        activeView.WorkbenchWindow.SelectWindow();
                    }
                }
                finally
                {
                    LockWindowUpdate(IntPtr.Zero);
                }
            }
        }

        private void LoadDefaultLayoutConfiguration()
        {
            if (File.Exists(LayoutConfiguration.CurrentLayoutTemplateFileName))
            {
                this.LoadDockPanelLayout(LayoutConfiguration.CurrentLayoutTemplateFileName);
            }
        }

        private void LoadDockPanelLayout(string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                this.dockPanel.LoadFromXml(stream, new DeserializeDockContent(this.GetContent));
            }
        }

        private void LoadLayoutConfiguration()
        {
            try
            {
                if (File.Exists(LayoutConfiguration.CurrentLayoutFileName))
                {
                    this.LoadDockPanelLayout(LayoutConfiguration.CurrentLayoutFileName);
                }
                else
                {
                    this.LoadDefaultLayoutConfiguration();
                }
            }
            catch
            {
            }
        }

        [DllImport("User32.dll")]
        public static extern bool LockWindowUpdate(IntPtr hWnd);
        public virtual void OnActiveWorkbenchWindowChanged(EventArgs e)
        {
            IWorkbenchWindow activeWorkbenchwindow = this.ActiveWorkbenchwindow;
            if (((activeWorkbenchwindow == null) || (activeWorkbenchwindow.ViewContent != null)) && (this.ActiveWorkbenchWindowChanged != null))
            {
                this.ActiveWorkbenchWindowChanged(this, e);
            }
            if (this.oldSelectedWindow != null)
            {
                this.oldSelectedWindow.OnWindowDeselected(EventArgs.Empty);
            }
            this.oldSelectedWindow = activeWorkbenchwindow;
            if (((this.oldSelectedWindow != null) && (this.oldSelectedWindow.ActiveViewContent != null)) && (this.oldSelectedWindow.ActiveViewContent.Control != null))
            {
                this.oldSelectedWindow.OnWindowSelected(EventArgs.Empty);
                this.oldSelectedWindow.ActiveViewContent.SwitchedTo();
            }
        }

        public void RedrawAllComponents()
        {
            foreach (PadDescriptor descriptor in this.wbForm.PadContentCollection)
            {
                DockContent content = this.contentHash[descriptor.Class];
                if (content != null)
                {
                    content.Text = SkyMap.Net.Core.StringParser.Parse(descriptor.Title);
                }
            }
            this.RedrawMainMenu();
            this.RedrawToolbars();
            this.RedrawStatusBar();
        }

        private void RedrawMainMenu()
        {
            Properties properties = PropertyService.Get<Properties>("SkyMap.Net.Gui.FullscreenOptions", new Properties());
            bool flag = properties.Get<bool>("HideMainMenu", false);
            bool flag2 = properties.Get<bool>("ShowMainMenuOnMouseMove", true);
            this.mainMenuContainer.AutoHide = this.wbForm.FullScreen && flag;
            this.mainMenuContainer.ShowOnMouseDown = true;
            this.mainMenuContainer.ShowOnMouseMove = flag2;
        }

        private void RedrawStatusBar()
        {
            Properties properties = PropertyService.Get<Properties>("SkyMap.Net.Gui.FullscreenOptions", new Properties());
            bool flag = properties.Get<bool>("HideStatusBar", true);
            bool flag2 = properties.Get<bool>("ShowStatusBarOnMouseMove", true);
            bool flag3 = PropertyService.Get<bool>("SkyMap.Net.Gui.StatusBarVisible", true);
            this.statusStripContainer.AutoHide = this.wbForm.FullScreen && flag;
            this.statusStripContainer.ShowOnMouseDown = true;
            this.statusStripContainer.ShowOnMouseMove = flag2;
            this.statusStripContainer.Visible = flag3;
        }

        private void RedrawToolbars()
        {
            bool flag = PropertyService.Get<Properties>("SkyMap.Net.Gui.FullscreenOptions", new Properties()).Get<bool>("HideToolbars", true);
            if (PropertyService.Get<bool>("SkyMap.Net.Gui.ToolBarVisible", true))
            {
                if (this.wbForm.FullScreen && flag)
                {
                    this.HideToolBars();
                }
                else
                {
                    this.ShowToolBars();
                }
            }
            else
            {
                this.HideToolBars();
            }
        }

        public void ShowPad(PadDescriptor content)
        {
            if (content != null)
            {
                if (!this.contentHash.ContainsKey(content.Class))
                {
                    this.CreateContent(content).Show(this.dockPanel);
                }
                else
                {
                    this.contentHash[content.Class].Show();
                }
            }
        }

        private void ShowPads()
        {
            foreach (PadDescriptor descriptor in WorkbenchSingleton.Workbench.PadContentCollection)
            {
                if (!this.contentHash.ContainsKey(descriptor.Class))
                {
                    this.ShowPad(descriptor);
                }
            }
            foreach (PadContentWrapper wrapper in this.contentHash.Values)
            {
                wrapper.AllowInitialize();
            }
        }

        private void ShowToolBars()
        {
            if (this.wbForm.ToolBars != null)
            {
                List<Control> list = new List<Control>();
                foreach (Control control in this.toolBarPanel.Controls)
                {
                    list.Add(control);
                }
                this.toolBarPanel.Controls.Clear();
                this.toolBarPanel.Controls.Add(list[0]);
                foreach (ToolStrip strip in this.wbForm.ToolBars)
                {
                    if (!this.toolBarPanel.Controls.Contains(strip))
                    {
                        this.toolBarPanel.Controls.Add(strip);
                    }
                }
                for (int i = 1; i < list.Count; i++)
                {
                    this.toolBarPanel.Controls.Add(list[i]);
                }
            }
        }

        public IWorkbenchWindow ShowView(IViewContent content)
        {
            if (content.WorkbenchWindow is SdiWorkspaceWindow)
            {
                SdiWorkspaceWindow workbenchWindow = (SdiWorkspaceWindow) content.WorkbenchWindow;
                if (!workbenchWindow.IsDisposed)
                {
                    workbenchWindow.Show(this.dockPanel);
                    return workbenchWindow;
                }
            }
            if (!content.Control.Visible)
            {
                content.Control.Visible = true;
            }
            content.Control.Dock = DockStyle.Fill;
            SdiWorkspaceWindow window2 = new SdiWorkspaceWindow(content);
            window2.CloseEvent += new EventHandler(this.CloseWindowEvent);
            if (this.dockPanel != null)
            {
                window2.Show(this.dockPanel);
            }
            return window2;
        }

        private void ShowViewContents()
        {
            foreach (IViewContent content in WorkbenchSingleton.Workbench.ViewContentCollection)
            {
                this.ShowView(content);
            }
        }

        public void StoreConfiguration()
        {
            try
            {
                if (this.dockPanel != null)
                {
                    LayoutConfiguration currentLayout = LayoutConfiguration.CurrentLayout;
                    if ((currentLayout != null) && !currentLayout.ReadOnly)
                    {
                        string path = Path.Combine(PropertyService.ConfigDirectory, "layouts");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        this.dockPanel.SaveAsXml(Path.Combine(path, currentLayout.FileName));
                    }
                }
            }
            catch (Exception exception)
            {
                MessageService.ShowError(exception);
            }
        }

        private void TrackFullscreenPropertyChanges(object sender, PropertyChangedEventArgs e)
        {
            if (!object.Equals(e.OldValue, e.NewValue) && this.wbForm.FullScreen)
            {
                string key = e.Key;
                if (key != null)
                {
                    if (!(key == "HideMainMenu") && !(key == "ShowMainMenuOnMouseMove"))
                    {
                        if (key == "HideToolbars")
                        {
                            this.RedrawToolbars();
                        }
                        else if ((key == "HideStatusBar") || (key == "ShowStatusBarOnMouseMove"))
                        {
                            this.RedrawStatusBar();
                        }
                    }
                    else
                    {
                        this.RedrawMainMenu();
                    }
                }
            }
        }

        public object ActiveContent
        {
            get
            {
                DockContent lastActiveContent;
                if (this.dockPanel == null)
                {
                    lastActiveContent = this.lastActiveContent;
                }
                else
                {
                    lastActiveContent = ((DockContent) this.dockPanel.ActiveContent) ?? this.lastActiveContent;
                }
                this.lastActiveContent = lastActiveContent;
                if ((lastActiveContent == null) || lastActiveContent.IsDisposed)
                {
                    return null;
                }
                if (lastActiveContent is IWorkbenchWindow)
                {
                    return ((IWorkbenchWindow) lastActiveContent).ActiveViewContent;
                }
                if (lastActiveContent is PadContentWrapper)
                {
                    return ((PadContentWrapper) lastActiveContent).PadContent;
                }
                return lastActiveContent;
            }
        }

        public IWorkbenchWindow ActiveWorkbenchwindow
        {
            get
            {
                if (((this.dockPanel == null) || (this.dockPanel.ActiveDocument == null)) || this.dockPanel.ActiveDocument.IsDisposed)
                {
                    return null;
                }
                return (this.dockPanel.ActiveDocument as IWorkbenchWindow);
            }
        }

        private class PadContentWrapper : DockContent
        {
            internal bool allowInitialize = false;
            private bool isInitialized = false;
            private PadDescriptor padDescriptor;

            public PadContentWrapper(PadDescriptor padDescriptor)
            {
                if (padDescriptor == null)
                {
                    throw new ArgumentNullException("padDescriptor");
                }
                this.padDescriptor = padDescriptor;
                base.DockAreas = DockAreas.DockBottom | DockAreas.DockTop | DockAreas.DockRight | DockAreas.DockLeft | DockAreas.Float;
                base.HideOnClose = true;
            }

            private void ActivateContent()
            {
                if (this.allowInitialize && !this.isInitialized)
                {
                    this.isInitialized = true;
                    IPadContent padContent = this.padDescriptor.PadContent;
                    if (padContent != null)
                    {
                        Control control = padContent.Control;
                        control.Dock = DockStyle.Fill;
                        base.Controls.Add(control);
                    }
                }
            }

            public void AllowInitialize()
            {
                this.allowInitialize = true;
                if (base.Visible && (base.Width > 0))
                {
                    this.ActivateContent();
                }
            }

            public void DetachContent()
            {
                base.Controls.Clear();
                this.padDescriptor = null;
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
                if (disposing && (this.padDescriptor != null))
                {
                    this.padDescriptor.Dispose();
                    this.padDescriptor = null;
                }
            }

            protected override string GetPersistString()
            {
                return this.padDescriptor.Class;
            }

            protected override void OnSizeChanged(EventArgs e)
            {
                base.OnSizeChanged(e);
                if (base.Visible && (base.Width > 0))
                {
                    this.ActivateContent();
                }
            }

            protected override void OnVisibleChanged(EventArgs e)
            {
                base.OnVisibleChanged(e);
                if (base.Visible && (base.Width > 0))
                {
                    this.ActivateContent();
                }
            }

            public IPadContent PadContent
            {
                get
                {
                    return this.padDescriptor.PadContent;
                }
            }
        }
    }
}

