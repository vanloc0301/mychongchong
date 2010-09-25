namespace SkyMap.Net.BrowserDisplayBinding
{
    using System;
    using System.Windows.Forms;

    public class DefaultSchemeExtension : ISchemeExtension
    {
        public virtual void DocumentCompleted(HtmlViewPane pane, WebBrowserDocumentCompletedEventArgs e)
        {
        }

        public virtual void GoHome(HtmlViewPane pane)
        {
            pane.Navigate("http://www.skymapsoft.com/");
        }

        public virtual void GoSearch(HtmlViewPane pane)
        {
            pane.Navigate("http://www.google.com/");
        }

        public virtual void InterceptNavigate(HtmlViewPane pane, WebBrowserNavigatingEventArgs e)
        {
        }
    }
}

