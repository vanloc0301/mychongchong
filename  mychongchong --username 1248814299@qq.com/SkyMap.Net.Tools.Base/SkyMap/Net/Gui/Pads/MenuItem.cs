namespace SkyMap.Net.Gui.Pads
{
    using System;

    public class MenuItem
    {
        public string Caption;
        public string URL;

        public MenuItem(string strCaption, string strUrl)
        {
            this.Caption = strCaption;
            this.URL = strUrl;
        }
    }
}

