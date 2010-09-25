namespace SkyMap.Net.Gui
{
    using System;
    using System.Windows.Forms;

    public interface IPadContent : IDisposable
    {
        void RedrawContent();

        System.Windows.Forms.Control Control { get; }
    }
}

