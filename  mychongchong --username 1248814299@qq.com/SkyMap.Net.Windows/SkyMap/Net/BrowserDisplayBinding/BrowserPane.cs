namespace SkyMap.Net.BrowserDisplayBinding
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;
    using System.Windows.Forms;

    public class BrowserPane : AbstractViewContent
    {
        private SkyMap.Net.BrowserDisplayBinding.HtmlViewPane htmlViewPane;

        public BrowserPane() : this(true)
        {
        }

        protected BrowserPane(bool showNavigation)
        {
            this.htmlViewPane = new SkyMap.Net.BrowserDisplayBinding.HtmlViewPane(showNavigation);
            this.htmlViewPane.WebBrowser.DocumentTitleChanged += new EventHandler(this.TitleChange);
            this.htmlViewPane.Closed += new EventHandler(this.PaneClosed);
            this.TitleChange(null, null);
        }

        public BrowserPane(Uri uri) : this(true)
        {
            this.htmlViewPane.Navigate(uri);
        }

        public override void Dispose()
        {
            base.Dispose();
            this.htmlViewPane.Dispose();
        }

        public override void Load(string url)
        {
            this.htmlViewPane.Navigate(url);
        }

        private void PaneClosed(object sender, EventArgs e)
        {
            this.WorkbenchWindow.CloseWindow(true);
        }

        public override void Save(string url)
        {
            this.Load(url);
        }

        private void TitleChange(object sender, EventArgs e)
        {
            string documentTitle = this.htmlViewPane.WebBrowser.DocumentTitle;
            if (documentTitle != null)
            {
                documentTitle = documentTitle.Trim();
            }
            if ((documentTitle == null) || (documentTitle.Length == 0))
            {
                this.TitleName = ResourceService.GetString("BrowserDisplayBinding.Browser");
            }
            else
            {
                this.TitleName = documentTitle;
            }
        }

        public override System.Windows.Forms.Control Control
        {
            get
            {
                return this.htmlViewPane;
            }
        }

        public SkyMap.Net.BrowserDisplayBinding.HtmlViewPane HtmlViewPane
        {
            get
            {
                return this.htmlViewPane;
            }
        }

        public override bool IsDirty
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        public override bool IsViewOnly
        {
            get
            {
                return true;
            }
        }

        public Uri Url
        {
            get
            {
                return this.htmlViewPane.Url;
            }
        }
    }
}

