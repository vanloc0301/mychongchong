namespace SkyMap.Net.BrowserDisplayBinding
{
    using SkyMap.Net.Core;
    using System;

    public class GoBack : AbstractCommand
    {
        public override void Run()
        {
            ((HtmlViewPane) this.Owner).WebBrowser.GoBack();
        }
    }
}

