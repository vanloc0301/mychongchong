namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using WeifenLuo.WinFormsUI.Docking;

    public class SdiWorkspaceWindow : DockContent, IWorkbenchWindow, IOwnerState
    {
        private IViewContent content;
        private TabControl viewTabControl = null;

        public event EventHandler CloseEvent;

        public event EventHandler TitleChanged;

        public event EventHandler WindowDeselected;

        public event EventHandler WindowSelected;

        public SdiWorkspaceWindow(IViewContent content)
        {
            this.content = content;
            content.WorkbenchWindow = this;
            content.TitleNameChanged += new EventHandler(this.SetTitleEvent);
            content.DirtyChanged += new EventHandler(this.SetTitleEvent);
            base.DockAreas = DockAreas.Document;
            base.DockPadding.All = 2;
            this.SetTitleEvent(this, EventArgs.Empty);
            this.InitControls();
        }

        private void AttachSecondaryViewContent(IBaseViewContent viewContent)
        {
            viewContent.WorkbenchWindow = this;
            TabPage page = new TabPage(SkyMap.Net.Core.StringParser.Parse(viewContent.TabPageText));
            page.Tag = viewContent;
            viewContent.Control.Dock = DockStyle.Fill;
            page.Controls.Add(viewContent.Control);
            this.viewTabControl.TabPages.Add(page);
        }

        public bool CloseWindow(bool force)
        {
            if ((!force && (this.ViewContent != null)) && this.ViewContent.IsDirty)
            {
                switch (MessageBox.Show(ResourceService.GetString("MainWindow.SaveChangesMessage"), ResourceService.GetString("MainWindow.SaveChangesMessageHeader") + " " + this.Title + " ?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, RightToLeftConverter.IsRightToLeft ? (MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign) : ((MessageBoxOptions) 0)))
                {
                    case DialogResult.Yes:
                        if (this.content.FileName != null)
                        {
                            IViewContent viewContent = this.ViewContent;
                            FileUtility.ObservedSave(new FileOperationDelegate(viewContent.Save), this.ViewContent.FileName, FileErrorPolicy.ProvideAlternative);
                            break;
                        }
                        do
                        {
                        }
                        while (this.ViewContent.IsDirty && !MessageService.AskQuestion("${res:MainWindow.DiscardChangesMessage}"));
                        break;

                    case DialogResult.Cancel:
                        return false;
                }
            }
            this.OnCloseEvent(null);
            base.Dispose();
            return true;
        }

        private void CreateViewTabControl()
        {
            this.viewTabControl = new TabControl();
            this.viewTabControl.GotFocus += delegate {
                TabPage page = this.viewTabControl.TabPages[this.viewTabControl.TabIndex];
                if (!((page.Controls.Count != 1) || page.ContainsFocus))
                {
                    page.Controls[0].Focus();
                }
            };
            this.viewTabControl.Alignment = TabAlignment.Bottom;
            this.viewTabControl.Dock = DockStyle.Fill;
            this.viewTabControl.Selected += new TabControlEventHandler(this.viewTabControlSelected);
            this.viewTabControl.Deselecting += new TabControlCancelEventHandler(this.viewTabControlDeselecting);
            this.viewTabControl.Deselected += new TabControlEventHandler(this.viewTabControlDeselected);
        }

        public void DetachContent()
        {
            this.content.TitleNameChanged -= new EventHandler(this.SetTitleEvent);
            this.content.DirtyChanged -= new EventHandler(this.SetTitleEvent);
            this.content = null;
            if (this.viewTabControl != null)
            {
                foreach (TabPage page in this.viewTabControl.TabPages)
                {
                    if ((this.viewTabControl.SelectedTab == page) && (page.Tag is IBaseViewContent))
                    {
                        ((IBaseViewContent) page.Tag).Deselecting();
                    }
                    page.Controls.Clear();
                    if ((this.viewTabControl.SelectedTab == page) && (page.Tag is IBaseViewContent))
                    {
                        ((IBaseViewContent) page.Tag).Deselected();
                    }
                }
                this.viewTabControl.Dispose();
                this.viewTabControl = null;
            }
            base.Controls.Clear();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.content != null)
                {
                    this.DetachContent();
                }
                if (base.TabPageContextMenu != null)
                {
                    base.TabPageContextMenu.Dispose();
                    base.TabPageContextMenu = null;
                }
            }
            base.Dispose(disposing);
        }

        private int GetSelectedIndex()
        {
            return this.viewTabControl.SelectedIndex;
        }

        private IBaseViewContent GetSubViewContent(int index)
        {
            if (index == 0)
            {
                return this.content;
            }
            return this.content.SecondaryViewContents[index - 1];
        }

        internal void InitControls()
        {
            if (this.content.SecondaryViewContents.Count > 0)
            {
                this.CreateViewTabControl();
                this.AttachSecondaryViewContent(this.content);
                foreach (ISecondaryViewContent content in this.content.SecondaryViewContents)
                {
                    this.AttachSecondaryViewContent(content);
                }
                base.Controls.Add(this.viewTabControl);
            }
            else
            {
                this.content.Control.Dock = DockStyle.Fill;
                base.Controls.Add(this.content.Control);
            }
        }

        private void LoadSolutionProjectsThreadEndedEvent(object sender, EventArgs e)
        {
            WorkbenchSingleton.SafeThreadAsyncCall(new MethodInvoker(this.RefreshSecondaryViewContents), new object[0]);
        }

        protected virtual void OnCloseEvent(EventArgs e)
        {
            this.OnWindowDeselected(e);
            if (this.CloseEvent != null)
            {
                this.CloseEvent(this, e);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = !this.CloseWindow(false);
        }

        protected virtual void OnTitleChanged(EventArgs e)
        {
            if (this.TitleChanged != null)
            {
                this.TitleChanged(this, e);
            }
            WorkbenchSingleton.Workbench.WorkbenchLayout.OnActiveWorkbenchWindowChanged(EventArgs.Empty);
        }

        public virtual void OnWindowDeselected(EventArgs e)
        {
            if (this.WindowDeselected != null)
            {
                this.WindowDeselected(this, e);
            }
        }

        public virtual void OnWindowSelected(EventArgs e)
        {
            if (this.WindowSelected != null)
            {
                this.WindowSelected(this, e);
            }
        }

        public virtual void RedrawContent()
        {
            if (this.viewTabControl != null)
            {
                for (int i = 0; i < this.viewTabControl.TabPages.Count; i++)
                {
                    TabPage page = this.viewTabControl.TabPages[i];
                    page.Text = SkyMap.Net.Core.StringParser.Parse(this.GetSubViewContent(i).TabPageText);
                }
            }
        }

        private void RefreshSecondaryViewContents()
        {
            if (this.content != null)
            {
                int count = this.content.SecondaryViewContents.Count;
                DisplayBindingService.AttachSubWindows(this.content, true);
                if (this.content.SecondaryViewContents.Count > count)
                {
                    LoggingService.Debug("Attaching new secondary view contents to '" + this.Title + "'");
                    if (this.viewTabControl == null)
                    {
                        base.Controls.Remove(this.content.Control);
                        this.CreateViewTabControl();
                        this.AttachSecondaryViewContent(this.content);
                        base.Controls.Add(this.viewTabControl);
                    }
                    foreach (ISecondaryViewContent content in this.content.SecondaryViewContents)
                    {
                        if (content.WorkbenchWindow == null)
                        {
                            this.AttachSecondaryViewContent(content);
                        }
                    }
                }
            }
        }

        public void SelectWindow()
        {
            base.Show();
        }

        public void SetTitleEvent(object sender, EventArgs e)
        {
            if (this.content != null)
            {
                string fileName;
                this.SetToolTipText();
                if (this.content.TitleName == null)
                {
                    fileName = Path.GetFileName(this.content.UntitledName);
                }
                else
                {
                    fileName = this.content.TitleName;
                }
                if (this.content.IsDirty)
                {
                    fileName = fileName + "*";
                }
                else if (this.content.IsReadOnly)
                {
                    fileName = fileName + "+";
                }
                if (fileName != this.Title)
                {
                    this.Title = fileName;
                }
            }
        }

        private void SetToolTipText()
        {
            if (this.content != null)
            {
                try
                {
                    if ((this.content.FileName != null) && (this.content.FileName.Length > 0))
                    {
                        base.ToolTipText = Path.GetFullPath(this.content.FileName);
                    }
                    else
                    {
                        base.ToolTipText = null;
                    }
                }
                catch (Exception)
                {
                    base.ToolTipText = this.content.FileName;
                }
            }
            else
            {
                base.ToolTipText = null;
            }
        }

        public void SwitchView(int viewNumber)
        {
            if (this.viewTabControl != null)
            {
                this.viewTabControl.SelectedIndex = viewNumber;
            }
        }

        private void viewTabControlDeselected(object sender, TabControlEventArgs e)
        {
            if ((e.Action == TabControlAction.Deselected) && (e.TabPageIndex >= 0))
            {
                IBaseViewContent subViewContent = this.GetSubViewContent(e.TabPageIndex);
                if (subViewContent != null)
                {
                    subViewContent.Deselected();
                }
            }
        }

        private void viewTabControlDeselecting(object sender, TabControlCancelEventArgs e)
        {
            if ((e.Action == TabControlAction.Deselecting) && (e.TabPageIndex >= 0))
            {
                IBaseViewContent subViewContent = this.GetSubViewContent(e.TabPageIndex);
                if (subViewContent != null)
                {
                    subViewContent.Deselecting();
                }
            }
        }

        private void viewTabControlSelected(object sender, TabControlEventArgs e)
        {
            if ((e.Action == TabControlAction.Selected) && (e.TabPageIndex >= 0))
            {
                IBaseViewContent subViewContent = this.GetSubViewContent(e.TabPageIndex);
                if (subViewContent != null)
                {
                    subViewContent.Deselected();
                    subViewContent.SwitchedTo();
                    subViewContent.Selected();
                }
            }
            WorkbenchSingleton.Workbench.WorkbenchLayout.OnActiveWorkbenchWindowChanged(EventArgs.Empty);
            this.ActiveViewContent.Control.Focus();
        }

        public IBaseViewContent ActiveViewContent
        {
            get
            {
                if (this.viewTabControl != null)
                {
                    int index = 0;
                    if (WorkbenchSingleton.InvokeRequired)
                    {
                        index = (int) WorkbenchSingleton.SafeThreadCall(this, "GetSelectedIndex", new object[0]);
                    }
                    else
                    {
                        index = this.GetSelectedIndex();
                    }
                    return this.GetSubViewContent(index);
                }
                return this.content;
            }
        }

        protected override Size DefaultSize
        {
            get
            {
                return Size.Empty;
            }
        }

        public Enum InternalState
        {
            get
            {
                OpenFileTabState nothing = OpenFileTabState.Nothing;
                if (this.content != null)
                {
                    if (this.content.IsDirty)
                    {
                        nothing |= OpenFileTabState.FileDirty;
                    }
                    if (this.content.IsReadOnly)
                    {
                        nothing |= OpenFileTabState.FileReadOnly;
                    }
                    if (this.content.IsUntitled)
                    {
                        nothing |= OpenFileTabState.FileUntitled;
                    }
                }
                return nothing;
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
                this.OnTitleChanged(EventArgs.Empty);
            }
        }

        public IViewContent ViewContent
        {
            get
            {
                return this.content;
            }
        }

        [Flags]
        public enum OpenFileTabState
        {
            FileDirty = 1,
            FileReadOnly = 2,
            FileUntitled = 4,
            Nothing = 0
        }
    }
}

