namespace SkyMap.Net.BrowserDisplayBinding
{
    using SkyMap.Net.Core;
    using System;

    public class Stop : AbstractCommand
    {
        public override void Run()
        {
            ((HtmlViewPane) this.Owner).WebBrowser.Stop();
        }
    }
}

