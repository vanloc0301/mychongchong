namespace SkyMap.Net.BrowserDisplayBinding
{
    using System;
    using System.ComponentModel;

    public class NewWindowExtendedEventArgs : CancelEventArgs
    {
        private Uri url;

        public NewWindowExtendedEventArgs(Uri url)
        {
            this.url = url;
        }

        public Uri Url
        {
            get
            {
                return this.url;
            }
        }
    }
}

