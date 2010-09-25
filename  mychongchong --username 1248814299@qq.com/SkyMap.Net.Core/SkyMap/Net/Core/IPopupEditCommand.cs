namespace SkyMap.Net.Core
{
    using System;

    public interface IPopupEditCommand : ICommand
    {
        bool IsEnabled { get; set; }
    }
}

