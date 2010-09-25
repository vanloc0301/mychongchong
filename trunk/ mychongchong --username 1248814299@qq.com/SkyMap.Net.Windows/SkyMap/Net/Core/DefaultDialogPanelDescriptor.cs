namespace SkyMap.Net.Core
{
    using System;
    using System.Collections.Generic;

    public class DefaultDialogPanelDescriptor : IDialogPanelDescriptor
    {
        private AddIn addin;
        private IDialogPanel dialogPanel;
        private List<IDialogPanelDescriptor> dialogPanelDescriptors;
        private string dialogPanelPath;
        private string id;
        private string label;

        public DefaultDialogPanelDescriptor(string id, string label)
        {
            this.id = string.Empty;
            this.label = string.Empty;
            this.dialogPanelDescriptors = null;
            this.dialogPanel = null;
            this.id = id;
            this.label = label;
        }

        public DefaultDialogPanelDescriptor(string id, string label, List<IDialogPanelDescriptor> dialogPanelDescriptors) : this(id, label)
        {
            this.dialogPanelDescriptors = dialogPanelDescriptors;
        }

        public DefaultDialogPanelDescriptor(string id, string label, AddIn addin, string dialogPanelPath) : this(id, label)
        {
            this.addin = addin;
            this.dialogPanelPath = dialogPanelPath;
        }

        public List<IDialogPanelDescriptor> ChildDialogPanelDescriptors
        {
            get
            {
                return this.dialogPanelDescriptors;
            }
            set
            {
                this.dialogPanelDescriptors = value;
            }
        }

        public IDialogPanel DialogPanel
        {
            get
            {
                if (this.dialogPanelPath != null)
                {
                    if (this.dialogPanel == null)
                    {
                        this.dialogPanel = (IDialogPanel) this.addin.CreateObject(this.dialogPanelPath);
                    }
                    this.dialogPanelPath = null;
                    this.addin = null;
                }
                return this.dialogPanel;
            }
            set
            {
                this.dialogPanel = value;
            }
        }

        public string ID
        {
            get
            {
                return this.id;
            }
        }

        public string Label
        {
            get
            {
                return this.label;
            }
            set
            {
                this.label = value;
            }
        }
    }
}

