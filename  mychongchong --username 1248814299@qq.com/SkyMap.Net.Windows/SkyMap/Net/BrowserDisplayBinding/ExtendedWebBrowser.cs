namespace SkyMap.Net.BrowserDisplayBinding
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class ExtendedWebBrowser : WebBrowser
    {
        private AxHost.ConnectionPointCookie cookie;
        private WebBrowserExtendedEvents wevents;

        public event NewWindowExtendedEventHandler NewWindowExtended;

        protected override void CreateSink()
        {
            base.CreateSink();
            this.wevents = new WebBrowserExtendedEvents(this);
            this.cookie = new AxHost.ConnectionPointCookie(base.ActiveXInstance, this.wevents, typeof(DWebBrowserEvents2));
        }

        protected override void DetachSink()
        {
            if (this.cookie != null)
            {
                this.cookie.Disconnect();
                this.cookie = null;
            }
            base.DetachSink();
        }

        protected virtual void OnNewWindowExtended(NewWindowExtendedEventArgs e)
        {
            if (this.NewWindowExtended != null)
            {
                this.NewWindowExtended(this, e);
            }
        }

        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIDispatch), Guid("34A715A0-6587-11D0-924A-0020AFC7AC4D"), TypeLibType(TypeLibTypeFlags.FHidden)]
        private interface DWebBrowserEvents2
        {
            [DispId(0x111)]
            void NewWindow3([In, MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In, Out] ref bool cancel, [In] ref object flags, [In, MarshalAs(UnmanagedType.BStr)] ref string urlContext, [In, MarshalAs(UnmanagedType.BStr)] ref string url);
        }

        private class WebBrowserExtendedEvents : StandardOleMarshalObject, ExtendedWebBrowser.DWebBrowserEvents2
        {
            private ExtendedWebBrowser browser;

            public WebBrowserExtendedEvents(ExtendedWebBrowser browser)
            {
                this.browser = browser;
            }

            public void NewWindow3(object pDisp, ref bool cancel, ref object flags, ref string urlContext, ref string url)
            {
                NewWindowExtendedEventArgs e = new NewWindowExtendedEventArgs(new Uri(url));
                this.browser.OnNewWindowExtended(e);
                cancel = e.Cancel;
            }
        }
    }
}

