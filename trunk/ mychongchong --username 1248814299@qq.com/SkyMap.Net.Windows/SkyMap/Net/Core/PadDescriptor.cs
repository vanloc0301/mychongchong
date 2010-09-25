namespace SkyMap.Net.Core
{
    using SkyMap.Net.Gui;
    using System;

    public class PadDescriptor : IDisposable
    {
        private Codon codon;
        private IPadContent padContent;
        private bool padContentCreated;

        public PadDescriptor(Codon codon)
        {
            this.codon = codon;
        }

        public void BringPadToFront()
        {
            this.CreatePad();
            if (this.padContent != null)
            {
                if (!WorkbenchSingleton.Workbench.WorkbenchLayout.IsVisible(this))
                {
                    WorkbenchSingleton.Workbench.WorkbenchLayout.ShowPad(this);
                }
                WorkbenchSingleton.Workbench.WorkbenchLayout.ActivatePad(this);
            }
        }

        public void CreatePad()
        {
            if (!this.padContentCreated)
            {
                this.padContentCreated = true;
                this.padContent = (IPadContent) this.codon.AddIn.CreateObject(this.Class);
            }
        }

        public void Dispose()
        {
            if (this.padContent != null)
            {
                this.padContent.Dispose();
                this.padContent = null;
            }
        }

        public void RedrawContent()
        {
            if (this.padContent != null)
            {
                this.padContent.RedrawContent();
            }
        }

        public string Category
        {
            get
            {
                return this.codon.Properties["category"];
            }
        }

        public string Class
        {
            get
            {
                return this.codon.Properties["class"];
            }
        }

        public bool HasFocus
        {
            get
            {
                return ((this.padContent != null) ? this.padContent.Control.ContainsFocus : false);
            }
        }

        public string Icon
        {
            get
            {
                return this.codon.Properties["icon"];
            }
        }

        public IPadContent PadContent
        {
            get
            {
                this.CreatePad();
                return this.padContent;
            }
        }

        public string Shortcut
        {
            get
            {
                return this.codon.Properties["shortcut"];
            }
        }

        public string Title
        {
            get
            {
                return this.codon.Properties["title"];
            }
        }
    }
}

