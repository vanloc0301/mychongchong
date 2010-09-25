namespace SkyMap.Net.Core
{
    using System;

    public interface ISpinEditCommand : ICommand
    {
        bool IsEnabled { get; set; }
    }
}

