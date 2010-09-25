namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui.XmlForms;
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public abstract class AbstractOptionPanel : BaseSharpDevelopUserControl, IDialogPanel
    {
        protected string baseDirectory;
        private object customizationObject = null;
        private bool isFinished = true;
        private bool wasActivated = false;

        public event EventHandler CustomizationObjectChanged;

        public event EventHandler EnableFinishChanged;

        protected void ConnectBrowseButton(string browseButton, string target)
        {
            this.ConnectBrowseButton(browseButton, target, "${res:FileFilter.AllFiles}|*.*");
        }

        protected void ConnectBrowseButton(string browseButton, string target, string fileFilter)
        {
            if (base.ControlDictionary[browseButton] == null)
            {
                MessageService.ShowError(browseButton + " not found!");
            }
            else if (base.ControlDictionary[target] == null)
            {
                MessageService.ShowError(target + " not found!");
            }
            else
            {
                base.ControlDictionary[browseButton].Click += new EventHandler(new BrowseButtonEvent(this, target, fileFilter).Event);
            }
        }

        protected void ConnectBrowseFolder(string browseButton, string target)
        {
            this.ConnectBrowseFolder(browseButton, target, "${res:Dialog.ProjectOptions.SelectFolderTitle}");
        }

        protected void ConnectBrowseFolder(string browseButton, string target, string description)
        {
            if (base.ControlDictionary[browseButton] == null)
            {
                MessageService.ShowError(browseButton + " not found!");
            }
            else if (base.ControlDictionary[target] == null)
            {
                MessageService.ShowError(target + " not found!");
            }
            else
            {
                base.ControlDictionary[browseButton].Click += new EventHandler(new BrowseFolderEvent(this, target, description).Event);
            }
        }

        public virtual void LoadPanelContents()
        {
        }

        protected virtual void OnCustomizationObjectChanged()
        {
            if (this.CustomizationObjectChanged != null)
            {
                this.CustomizationObjectChanged(this, null);
            }
        }

        protected virtual void OnEnableFinishChanged()
        {
            if (this.EnableFinishChanged != null)
            {
                this.EnableFinishChanged(this, null);
            }
        }

        public virtual bool ReceiveDialogMessage(DialogMessage message)
        {
            DialogMessage message2 = message;
            if (message2 != DialogMessage.OK)
            {
                if ((message2 == DialogMessage.Activated) && !this.wasActivated)
                {
                    this.LoadPanelContents();
                    this.wasActivated = true;
                }
            }
            else if (this.wasActivated)
            {
                return this.StorePanelContents();
            }
            return true;
        }

        public virtual bool StorePanelContents()
        {
            return true;
        }

        public System.Windows.Forms.Control Control
        {
            get
            {
                return this;
            }
        }

        public virtual object CustomizationObject
        {
            get
            {
                return this.customizationObject;
            }
            set
            {
                this.customizationObject = value;
                this.OnCustomizationObjectChanged();
            }
        }

        public virtual bool EnableFinish
        {
            get
            {
                return this.isFinished;
            }
            set
            {
                if (this.isFinished != value)
                {
                    this.isFinished = value;
                    this.OnEnableFinishChanged();
                }
            }
        }

        public bool WasActivated
        {
            get
            {
                return this.wasActivated;
            }
        }

        protected class BrowseButtonEvent
        {
            private string filter;
            private AbstractOptionPanel panel;
            private string target;

            public BrowseButtonEvent(AbstractOptionPanel panel, string target, string filter)
            {
                this.panel = panel;
                this.filter = filter;
                this.target = target;
            }

            public void Event(object sender, EventArgs e)
            {
                using (OpenFileDialog dialog = new OpenFileDialog())
                {
                    dialog.Filter = SkyMap.Net.Core.StringParser.Parse(this.filter);
                    dialog.Multiselect = false;
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        string fileName = dialog.FileName;
                        if (this.panel.baseDirectory != null)
                        {
                            fileName = FileUtility.GetRelativePath(this.panel.baseDirectory, fileName);
                        }
                        this.panel.ControlDictionary[this.target].Text = fileName;
                    }
                }
            }
        }

        private class BrowseFolderEvent
        {
            private string description;
            private AbstractOptionPanel panel;
            private string target;

            public BrowseFolderEvent(AbstractOptionPanel panel, string target, string description)
            {
                this.panel = panel;
                this.description = description;
                this.target = target;
            }

            public void Event(object sender, EventArgs e)
            {
                FolderDialog dialog = new FolderDialog();
                if (dialog.DisplayDialog(this.description) == DialogResult.OK)
                {
                    string path = dialog.Path;
                    if (this.panel.baseDirectory != null)
                    {
                        path = FileUtility.GetRelativePath(this.panel.baseDirectory, path);
                    }
                    if (!(path.EndsWith(@"\") || path.EndsWith("/")))
                    {
                        path = path + @"\";
                    }
                    this.panel.ControlDictionary[this.target].Text = path;
                }
            }
        }
    }
}

