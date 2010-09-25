namespace SkyMap.Net.BrowserDisplayBinding
{
    using SkyMap.Net.Core;
    using System;

    public class GoHome : AbstractCommand
    {
        public override void Run()
        {
            ((HtmlViewPane) this.Owner).GoHome();
        }
    }
}

