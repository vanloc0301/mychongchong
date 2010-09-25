namespace SkyMap.Net.BrowserDisplayBinding
{
    using SkyMap.Net.Core;
    using System;

    public class GoSearch : AbstractCommand
    {
        public override void Run()
        {
            ((HtmlViewPane) this.Owner).GoSearch();
        }
    }
}

