namespace SkyMap.Net.Core
{
    using System;

    public interface IStatusUpdate
    {
        void UpdateStatus();
        void UpdateText();

        ICommand Command { get; }
    }
}

