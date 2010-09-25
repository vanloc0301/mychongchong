namespace SkyMap.Net.Gui.Pads
{
    using SkyMap.Net.BrowserDisplayBinding;
    using System;
    using System.Windows.Forms;

    public class StartPageScheme : DefaultSchemeExtension
    {
        private ToolsCodePage page;

        public override void DocumentCompleted(HtmlViewPane pane, WebBrowserDocumentCompletedEventArgs e)
        {
        }

        public override void GoHome(HtmlViewPane pane)
        {
            pane.Navigate("startpage://start/");
        }

        public override void InterceptNavigate(HtmlViewPane pane, WebBrowserNavigatingEventArgs e)
        {
            e.Cancel = true;
            if (this.page == null)
            {
                this.page = new ToolsCodePage();
            }
            if (e.Url.Host == "command")
            {
            }
        }
    }
}

