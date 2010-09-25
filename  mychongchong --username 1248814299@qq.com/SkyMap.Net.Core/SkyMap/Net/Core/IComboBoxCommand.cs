namespace SkyMap.Net.Core
{
    using System;

    public interface IComboBoxCommand : ICommand
    {
        bool IsEnabled { get; set; }
    }
}

