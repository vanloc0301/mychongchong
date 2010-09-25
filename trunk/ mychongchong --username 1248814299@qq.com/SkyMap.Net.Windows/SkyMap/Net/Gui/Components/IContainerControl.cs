namespace SkyMap.Net.Gui.Components
{
    using System;

    public interface IContainerControl
    {
        bool QueryPopup(object obj);
        object QueryValue();
    }
}

