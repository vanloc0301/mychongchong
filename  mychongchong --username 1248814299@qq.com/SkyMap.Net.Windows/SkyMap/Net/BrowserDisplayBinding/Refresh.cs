namespace SkyMap.Net.BrowserDisplayBinding
{
    using SkyMap.Net.Core;
    using System;
    using System.Windows.Forms;

    public class Refresh : AbstractCommand
    {
        public override void Run()
        {
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                ((HtmlViewPane) this.Owner).WebBrowser.Refresh(WebBrowserRefreshOption.Completely);
            }
            else
            {
                ((HtmlViewPane) this.Owner).WebBrowser.Refresh();
            }
        }
    }
}

