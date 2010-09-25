namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class DefaultWorkbench : Form, IWorkbench, IMementoCapable
    {
        private bool closeAll = false;
        private FormWindowState defaultWindowState = FormWindowState.Normal;
        private bool fullscreen;
        private IWorkbenchLayout layout = null;
        private static readonly string mainMenuPath = "/Workbench/MainMenu";
        private Rectangle normalBounds = new Rectangle(0, 0, 640, 480);
        public ToolStrip[] ToolBars = null;
        private Timer toolbarUpdateTimer;
        public MenuStrip TopMenu = null;
        private List<PadDescriptor> viewContentCollection = new List<PadDescriptor>();
        private static readonly string viewContentPath = "/Workbench/Pads";
        private const int VK_RMENU = 0xa5;
        private List<IViewContent> workbenchContentCollection = new List<IViewContent>();

        public event EventHandler ActiveWorkbenchWindowChanged;

        public event ViewContentEventHandler ViewClosed;

        public event ViewContentEventHandler ViewOpened;

        public DefaultWorkbench()
        {
            this.Text = MessageHelper.GetCaption("还没有设置好应用程序标题");
            base.Icon = MessageHelper.GetSystemIcon();
            base.StartPosition = FormStartPosition.Manual;
            this.AllowDrop = true;
            try
            {
                base.ImeMode = ImeMode.OnHalf;
            }
            catch
            {
            }
        }

        private void CheckRemovedOrReplacedFile(object sender, FileEventArgs e)
        {
            int num = 0;
            while (num < this.ViewContentCollection.Count)
            {
                if (FileUtility.IsBaseDirectory(e.FileName, this.ViewContentCollection[num].FileName))
                {
                    this.ViewContentCollection[num].WorkbenchWindow.CloseWindow(true);
                }
                else
                {
                    num++;
                }
            }
        }

        private void CheckRenamedFile(object sender, FileRenameEventArgs e)
        {
            if (e.IsDirectory)
            {
                foreach (IViewContent content in this.ViewContentCollection)
                {
                    if ((content.FileName != null) && FileUtility.IsBaseDirectory(e.SourceFile, content.FileName))
                    {
                        content.FileName = FileUtility.RenameBaseDirectory(content.FileName, e.SourceFile, e.TargetFile);
                    }
                }
            }
            else
            {
                foreach (IViewContent content in this.ViewContentCollection)
                {
                    if ((content.FileName != null) && FileUtility.IsEqualFileName(content.FileName, e.SourceFile))
                    {
                        content.FileName = e.TargetFile;
                        content.TitleName = Path.GetFileName(e.TargetFile);
                        break;
                    }
                }
            }
        }

        public void CloseAllViews()
        {
            try
            {
                this.closeAll = true;
                foreach (IViewContent content in this.workbenchContentCollection)
                {
                    content.WorkbenchWindow.CloseWindow(false);
                }
            }
            finally
            {
                this.closeAll = false;
                this.OnActiveWindowChanged(this, EventArgs.Empty);
            }
        }

        public void CloseContent(IViewContent content)
        {
            if (PropertyService.Get<bool>("SharpDevelop.LoadDocumentProperties", true) && (content is IMementoCapable))
            {
                this.StoreMemento(content);
            }
            if (this.ViewContentCollection.Contains(content))
            {
                this.ViewContentCollection.Remove(content);
            }
            this.OnViewClosed(new ViewContentEventArgs(content));
            content.Dispose();
            if (content.WorkbenchWindow != null)
            {
                content.WorkbenchWindow.CloseWindow(false);
            }
            content = null;
        }

        private void CreateMainMenu()
        {
            this.TopMenu = new MenuStrip();
            this.TopMenu.Items.Clear();
            try
            {
                ToolStripItem[] toolStripItems = (ToolStripItem[]) AddInTree.GetTreeNode(mainMenuPath).BuildChildItems(this).ToArray(typeof(ToolStripItem));
                this.TopMenu.Items.AddRange(toolStripItems);
                this.UpdateMenus();
            }
            catch (TreePathNotFoundException)
            {
            }
        }

        public Properties CreateMemento()
        {
            Properties properties = new Properties();
            properties["bounds"] = this.normalBounds.X.ToString(NumberFormatInfo.InvariantInfo) + "," + this.normalBounds.Y.ToString(NumberFormatInfo.InvariantInfo) + "," + this.normalBounds.Width.ToString(NumberFormatInfo.InvariantInfo) + "," + this.normalBounds.Height.ToString(NumberFormatInfo.InvariantInfo);
            if (this.FullScreen || (base.WindowState == FormWindowState.Minimized))
            {
                properties["windowstate"] = this.defaultWindowState.ToString();
            }
            else
            {
                properties["windowstate"] = base.WindowState.ToString();
            }
            properties["defaultstate"] = this.defaultWindowState.ToString();
            return properties;
        }

        private void CreateToolBars()
        {
            if (this.ToolBars == null)
            {
                this.ToolBars = ToolbarService.CreateToolbars(this, "/Workbench/ToolBar");
            }
        }

        [DllImport("user32.dll", ExactSpelling=true)]
        private static extern short GetAsyncKeyState(int vKey);
        private string GetMementoFileName(string contentName)
        {
            return Path.Combine(Path.Combine(PropertyService.ConfigDirectory, "temp"), Path.GetFileName(contentName) + "." + contentName.ToLowerInvariant().GetHashCode().ToString("x") + ".xml");
        }

        public PadDescriptor GetPad(System.Type type)
        {
            foreach (PadDescriptor descriptor in this.PadContentCollection)
            {
                if (descriptor.Class == type.FullName)
                {
                    return descriptor;
                }
            }
            return null;
        }

        public Properties GetStoredMemento(IViewContent content)
        {
            if ((content != null) && (content.FileName != null))
            {
                string mementoFileName = this.GetMementoFileName(content.FileName);
                if (FileUtility.IsValidFileName(mementoFileName) && File.Exists(mementoFileName))
                {
                    return Properties.Load(mementoFileName);
                }
            }
            return null;
        }

        public void InitializeWorkspace()
        {
            this.UpdateRenderer();
            base.MenuComplete += new EventHandler(this.SetStandardStatusBar);
            this.SetStandardStatusBar(null, null);
            FileService.FileRemoved += new EventHandler<FileEventArgs>(this.CheckRemovedOrReplacedFile);
            FileService.FileReplaced += new EventHandler<FileEventArgs>(this.CheckRemovedOrReplacedFile);
            FileService.FileRenamed += new EventHandler<FileRenameEventArgs>(this.CheckRenamedFile);
            FileService.FileRemoved += new EventHandler<FileEventArgs>(FileService.RecentOpen.FileRemoved);
            FileService.FileRenamed += new EventHandler<FileRenameEventArgs>(FileService.RecentOpen.FileRenamed);
            try
            {
                ArrayList list = AddInTree.GetTreeNode(viewContentPath).BuildChildItems(this);
                foreach (PadDescriptor descriptor in list)
                {
                    if (descriptor != null)
                    {
                        this.ShowPad(descriptor);
                    }
                }
            }
            catch (TreePathNotFoundException)
            {
            }
            this.CreateMainMenu();
            this.CreateToolBars();
            this.toolbarUpdateTimer = new Timer();
            this.toolbarUpdateTimer.Tick += new EventHandler(this.UpdateMenu);
            this.toolbarUpdateTimer.Interval = 500;
            this.toolbarUpdateTimer.Start();
            RightToLeftConverter.Convert(this);
        }

        private void OnActiveWindowChanged(object sender, EventArgs e)
        {
            if (!(this.closeAll || (this.ActiveWorkbenchWindowChanged == null)))
            {
                this.ActiveWorkbenchWindowChanged(this, e);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            while (WorkbenchSingleton.Workbench.ViewContentCollection.Count > 0)
            {
                IViewContent item = WorkbenchSingleton.Workbench.ViewContentCollection[0];
                if (item.WorkbenchWindow == null)
                {
                    LoggingService.Warn("Content with empty WorkbenchWindow found");
                    WorkbenchSingleton.Workbench.ViewContentCollection.RemoveAt(0);
                }
                else
                {
                    item.WorkbenchWindow.CloseWindow(false);
                    if (WorkbenchSingleton.Workbench.ViewContentCollection.IndexOf(item) >= 0)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            }
            this.closeAll = true;
            this.layout.Detach();
            foreach (PadDescriptor descriptor in this.PadContentCollection)
            {
                descriptor.Dispose();
            }
        }

        protected override void OnDragDrop(DragEventArgs e)
        {
            base.OnDragDrop(e);
            if ((e.Data != null) && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] data = (string[]) e.Data.GetData(DataFormats.FileDrop);
                foreach (string str in data)
                {
                    if (File.Exists(str))
                    {
                        FileService.OpenFile(str);
                    }
                }
            }
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);
            if ((e.Data != null) && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] data = (string[]) e.Data.GetData(DataFormats.FileDrop);
                foreach (string str in data)
                {
                    if (File.Exists(str))
                    {
                        e.Effect = DragDropEffects.Copy;
                        return;
                    }
                }
            }
            e.Effect = DragDropEffects.None;
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
            if (base.WindowState == FormWindowState.Normal)
            {
                this.normalBounds = base.Bounds;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (!this.FullScreen && (base.WindowState != FormWindowState.Minimized))
            {
                this.defaultWindowState = base.WindowState;
                if (base.WindowState == FormWindowState.Normal)
                {
                    this.normalBounds = base.Bounds;
                }
            }
        }

        protected virtual void OnViewClosed(ViewContentEventArgs e)
        {
            if (this.ViewClosed != null)
            {
                this.ViewClosed(this, e);
            }
        }

        protected virtual void OnViewOpened(ViewContentEventArgs e)
        {
            if (this.ViewOpened != null)
            {
                this.ViewOpened(this, e);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (this.IsAltGRPressed)
            {
                return false;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void RedrawAllComponents()
        {
            RightToLeftConverter.ConvertRecursive(this);
            foreach (ToolStripItem item in this.TopMenu.Items)
            {
                if (item is IStatusUpdate)
                {
                    ((IStatusUpdate) item).UpdateText();
                }
            }
            foreach (IViewContent content in this.workbenchContentCollection)
            {
                content.RedrawContent();
                if (content.WorkbenchWindow != null)
                {
                    content.WorkbenchWindow.RedrawContent();
                }
            }
            foreach (PadDescriptor descriptor in this.viewContentCollection)
            {
                descriptor.RedrawContent();
            }
            if (this.layout != null)
            {
                this.layout.RedrawAllComponents();
            }
            StatusBarService.RedrawStatusbar();
        }

        public void SetMemento(Properties properties)
        {
            if ((properties != null) && properties.Contains("bounds"))
            {
                string[] strArray = properties["bounds"].Split(new char[] { ',' });
                if (strArray.Length == 4)
                {
                    base.Bounds = this.normalBounds = new Rectangle(int.Parse(strArray[0], NumberFormatInfo.InvariantInfo), int.Parse(strArray[1], NumberFormatInfo.InvariantInfo), int.Parse(strArray[2], NumberFormatInfo.InvariantInfo), int.Parse(strArray[3], NumberFormatInfo.InvariantInfo));
                }
                this.defaultWindowState = (FormWindowState) Enum.Parse(typeof(FormWindowState), properties["defaultstate"]);
                this.FullScreen = properties.Get<bool>("fullscreen", false);
                base.WindowState = (FormWindowState) Enum.Parse(typeof(FormWindowState), properties["windowstate"]);
            }
        }

        private void SetStandardStatusBar(object sender, EventArgs e)
        {
            StatusBarService.SetMessage("${res:MainWindow.StatusBar.ReadyMessage}");
        }

        public virtual void ShowPad(PadDescriptor content)
        {
            this.PadContentCollection.Add(content);
            if (this.layout != null)
            {
                this.layout.ShowPad(content);
            }
        }

        public virtual void ShowView(IViewContent content)
        {
            this.ViewContentCollection.Add(content);
            if (PropertyService.Get<bool>("SharpDevelop.LoadDocumentProperties", true) && (content is IMementoCapable))
            {
                try
                {
                    Properties storedMemento = this.GetStoredMemento(content);
                    if (storedMemento != null)
                    {
                        ((IMementoCapable) content).SetMemento(storedMemento);
                    }
                }
                catch (Exception exception)
                {
                    MessageService.ShowError(exception, "Can't get/set memento");
                }
            }
            this.layout.ShowView(content);
            content.WorkbenchWindow.SelectWindow();
            this.OnViewOpened(new ViewContentEventArgs(content));
        }

        public void StoreMemento(IViewContent content)
        {
            if (content.FileName != null)
            {
                string path = PropertyService.ConfigDirectory + "temp";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                Properties properties = ((IMementoCapable) content).CreateMemento();
                string mementoFileName = this.GetMementoFileName(content.FileName);
                if (FileUtility.IsValidFileName(mementoFileName))
                {
                    FileUtility.ObservedSave(new NamedFileOperationDelegate(properties.Save), mementoFileName, FileErrorPolicy.Inform);
                }
            }
        }

        private void UpdateMenu(object sender, EventArgs e)
        {
            this.UpdateMenus();
            this.UpdateToolbars();
        }

        private void UpdateMenus()
        {
            foreach (object obj2 in this.TopMenu.Items)
            {
                if (obj2 is IStatusUpdate)
                {
                    ((IStatusUpdate) obj2).UpdateStatus();
                }
            }
        }

        public void UpdateRenderer()
        {
            if (PropertyService.Get<bool>("SkyMap.Net.Gui.UseProfessionalRenderer", true))
            {
                ToolStripManager.Renderer = new ToolStripProfessionalRenderer();
            }
            else
            {
                ProfessionalColorTable professionalColorTable = new ProfessionalColorTable();
                professionalColorTable.UseSystemColors = true;
                ToolStripManager.Renderer = new ToolStripProfessionalRenderer(professionalColorTable);
            }
        }

        private void UpdateToolbars()
        {
            if (this.ToolBars != null)
            {
                foreach (ToolStrip strip in this.ToolBars)
                {
                    ToolbarService.UpdateToolbar(strip);
                }
            }
        }

        public object ActiveContent
        {
            get
            {
                if (this.layout == null)
                {
                    return null;
                }
                return this.layout.ActiveContent;
            }
        }

        public IWorkbenchWindow ActiveWorkbenchWindow
        {
            get
            {
                if (this.layout == null)
                {
                    return null;
                }
                return this.layout.ActiveWorkbenchwindow;
            }
        }

        public bool FullScreen
        {
            get
            {
                return this.fullscreen;
            }
            set
            {
                if (this.fullscreen != value)
                {
                    this.fullscreen = value;
                    if (this.fullscreen)
                    {
                        this.defaultWindowState = base.WindowState;
                        base.Visible = false;
                        base.FormBorderStyle = FormBorderStyle.None;
                        base.WindowState = FormWindowState.Maximized;
                        base.Visible = true;
                    }
                    else
                    {
                        base.FormBorderStyle = FormBorderStyle.Sizable;
                        base.Bounds = this.normalBounds;
                        base.WindowState = this.defaultWindowState;
                    }
                    this.RedrawAllComponents();
                }
            }
        }

        public bool IsAltGRPressed
        {
            get
            {
                return ((GetAsyncKeyState(0xa5) < 0) && ((Control.ModifierKeys & Keys.Control) == Keys.Control));
            }
        }

        public List<PadDescriptor> PadContentCollection
        {
            get
            {
                return this.viewContentCollection;
            }
        }

        public string Title
        {
            get
            {
                return this.Text;
            }
            set
            {
                this.Text = value;
            }
        }

        public List<IViewContent> ViewContentCollection
        {
            get
            {
                return this.workbenchContentCollection;
            }
        }

        public IWorkbenchLayout WorkbenchLayout
        {
            get
            {
                return this.layout;
            }
            set
            {
                if (this.layout != null)
                {
                    this.layout.ActiveWorkbenchWindowChanged -= new EventHandler(this.OnActiveWindowChanged);
                    this.layout.Detach();
                }
                value.Attach(this);
                this.layout = value;
                this.layout.ActiveWorkbenchWindowChanged += new EventHandler(this.OnActiveWindowChanged);
            }
        }
    }
}

