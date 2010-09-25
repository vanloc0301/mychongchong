namespace SkyMap.Net.BrowserDisplayBinding
{
    using SkyMap.Net.Core;
    using System;

    public class GoForward : AbstractCommand
    {
        public override void Run()
        {
            ((HtmlViewPane) this.Owner).WebBrowser.GoForward();
        }
    }
}

