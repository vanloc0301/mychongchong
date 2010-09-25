namespace SkyMap.Net.BrowserDisplayBinding
{
    using System;
    using System.Windows.Forms;

    public interface ISchemeExtension
    {
        void DocumentCompleted(HtmlViewPane pane, WebBrowserDocumentCompletedEventArgs e);
        void GoHome(HtmlViewPane pane);
        void GoSearch(HtmlViewPane pane);
        void InterceptNavigate(HtmlViewPane pane, WebBrowserNavigatingEventArgs e);
    }
}

