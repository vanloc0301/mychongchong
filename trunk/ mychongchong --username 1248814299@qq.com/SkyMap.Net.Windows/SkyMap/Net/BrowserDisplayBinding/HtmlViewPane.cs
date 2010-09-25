namespace SkyMap.Net.BrowserDisplayBinding
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class HtmlViewPane : UserControl
    {
        public const string DefaultHomepage = "http://www.skymapsoft.com/";
        public const string DefaultSearchUrl = "http://www.google.com/";
        private static ArrayList descriptors;
        private string dummyUrl;
        private ToolStrip toolStrip;
        private Control urlBox;
        private ExtendedWebBrowser webBrowser = null;

        public event EventHandler Closed;

        public HtmlViewPane(bool showNavigation)
        {
            this.Dock = DockStyle.Fill;
            base.Size = new Size(500, 500);
            this.webBrowser = new ExtendedWebBrowser();
            this.webBrowser.Dock = DockStyle.Fill;
            this.webBrowser.Navigating += new WebBrowserNavigatingEventHandler(this.WebBrowserNavigating);
            this.webBrowser.NewWindowExtended += new NewWindowExtendedEventHandler(this.NewWindow);
            this.webBrowser.Navigated += new WebBrowserNavigatedEventHandler(this.WebBrowserNavigated);
            this.webBrowser.StatusTextChanged += new EventHandler(this.WebBrowserStatusTextChanged);
            this.webBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(this.WebBrowserDocumentCompleted);
            base.Controls.Add(this.webBrowser);
            if (showNavigation)
            {
                this.toolStrip = ToolbarService.CreateToolStrip(this, "/ViewContent/Browser/Toolbar");
                this.toolStrip.GripStyle = ToolStripGripStyle.Hidden;
                base.Controls.Add(this.toolStrip);
            }
        }

        public void Close()
        {
            if (this.Closed != null)
            {
                this.Closed(this, EventArgs.Empty);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                this.webBrowser.Dispose();
            }
        }

        public static ISchemeExtension GetScheme(string name)
        {
            if (descriptors == null)
            {
                descriptors = AddInTree.BuildItems("/Views/Browser/SchemeExtensions", null, false);
            }
            foreach (SchemeExtensionDescriptor descriptor in descriptors)
            {
                if (string.Equals(name, descriptor.SchemeName, StringComparison.OrdinalIgnoreCase))
                {
                    return descriptor.Extension;
                }
            }
            return null;
        }

        public void GoHome()
        {
            ISchemeExtension scheme = GetScheme(this.Url.Scheme);
            if (scheme != null)
            {
                scheme.GoHome(this);
            }
            else
            {
                this.Navigate("http://www.skymapsoft.com/");
            }
        }

        public void GoSearch()
        {
            ISchemeExtension scheme = GetScheme(this.Url.Scheme);
            if (scheme != null)
            {
                scheme.GoSearch(this);
            }
            else
            {
                this.Navigate("http://www.google.com/");
            }
        }

        public void Navigate(string url)
        {
            this.webBrowser.Navigate(new Uri(url));
        }

        public void Navigate(Uri url)
        {
            this.webBrowser.Navigate(url);
        }

        private void NewWindow(object sender, NewWindowExtendedEventArgs e)
        {
            e.Cancel = true;
            WorkbenchSingleton.Workbench.ShowView(new BrowserPane(e.Url));
        }

        public void SetUrlBox(Control urlBox)
        {
            this.urlBox = urlBox;
            urlBox.KeyUp += new KeyEventHandler(this.UrlBoxKeyUp);
        }

        public void SetUrlComboBox(ComboBox comboBox)
        {
            this.SetUrlBox(comboBox);
            comboBox.DropDownStyle = ComboBoxStyle.DropDown;
            comboBox.Items.Clear();
            comboBox.Items.AddRange(PropertyService.Get<string[]>("Browser.URLBoxHistory", new string[0]));
            comboBox.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox.AutoCompleteSource = AutoCompleteSource.HistoryList;
        }

        private void UrlBoxKeyUp(object sender, KeyEventArgs e)
        {
            Control ctl = (Control) sender;
            if (e.KeyData == Keys.Return)
            {
                e.Handled = true;
                this.UrlBoxNavigate(ctl);
            }
        }

        private void UrlBoxNavigate(Control ctl)
        {
            string url = ctl.Text.Trim();
            if (url.IndexOf(':') < 0)
            {
                url = "http://" + url;
            }
            this.Navigate(url);
            ComboBox box = ctl as ComboBox;
            if (box != null)
            {
                box.Items.Remove(url);
                box.Items.Insert(0, url);
                string[] array = PropertyService.Get<string[]>("Browser.URLBoxHistory", new string[0]);
                int index = Array.IndexOf<string>(array, url);
                if ((index < 0) && (array.Length >= 20))
                {
                    index = array.Length - 1;
                }
                if (index < 0)
                {
                    string[] strArray2 = new string[array.Length + 1];
                    array.CopyTo(strArray2, 1);
                    array = strArray2;
                }
                else
                {
                    for (int i = index; i > 0; i--)
                    {
                        array[i] = array[i - 1];
                    }
                }
                array[0] = url;
                PropertyService.Set<string[]>("Browser.URLBoxHistory", array);
            }
        }

        private void WebBrowserDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                if ((this.dummyUrl != null) && (e.Url.ToString() == "about:blank"))
                {
                    e = new WebBrowserDocumentCompletedEventArgs(new Uri(this.dummyUrl));
                }
                ISchemeExtension scheme = GetScheme(e.Url.Scheme);
                if (scheme != null)
                {
                    scheme.DocumentCompleted(this, e);
                }
            }
            catch (Exception exception)
            {
                MessageService.ShowError(exception);
            }
        }

        private void WebBrowserNavigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            string str = this.webBrowser.Url.ToString();
            if ((this.dummyUrl != null) && (str == "about:blank"))
            {
                this.urlBox.Text = this.dummyUrl;
            }
            else
            {
                this.urlBox.Text = str;
            }
            foreach (object obj2 in this.toolStrip.Items)
            {
                IStatusUpdate update = obj2 as IStatusUpdate;
                if (update != null)
                {
                    update.UpdateStatus();
                }
            }
        }

        private void WebBrowserNavigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            try
            {
                ISchemeExtension scheme = GetScheme(e.Url.Scheme);
                if (scheme != null)
                {
                    scheme.InterceptNavigate(this, e);
                    if (e.TargetFrameName.Length == 0)
                    {
                        if (e.Cancel)
                        {
                            this.dummyUrl = e.Url.ToString();
                        }
                        else if (e.Url.ToString() != "about:blank")
                        {
                            this.dummyUrl = null;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                MessageService.ShowError(exception);
            }
        }

        private void WebBrowserStatusTextChanged(object sender, EventArgs e)
        {
            IWorkbenchWindow activeWorkbenchWindow = WorkbenchSingleton.Workbench.ActiveWorkbenchWindow;
            if (activeWorkbenchWindow != null)
            {
                BrowserPane activeViewContent = activeWorkbenchWindow.ActiveViewContent as BrowserPane;
                if ((activeViewContent != null) && (activeViewContent.HtmlViewPane == this))
                {
                    StatusBarService.SetMessage(this.webBrowser.StatusText);
                }
            }
        }

        public Uri Url
        {
            get
            {
                if (this.webBrowser.Url == null)
                {
                    return new Uri("about:blank");
                }
                if ((this.dummyUrl != null) && (this.webBrowser.Url.ToString() == "about:blank"))
                {
                    return new Uri(this.dummyUrl);
                }
                return this.webBrowser.Url;
            }
        }

        public ExtendedWebBrowser WebBrowser
        {
            get
            {
                return this.webBrowser;
            }
        }
    }
}

