namespace SkyMap.Net.Gui.TreeGrid
{
    using System;
    using System.Runtime.CompilerServices;

    public interface IActivatable
    {
        event EventHandler Activated;

        event EventHandler Deactivate;

        bool IsActivated { get; }
    }
}

