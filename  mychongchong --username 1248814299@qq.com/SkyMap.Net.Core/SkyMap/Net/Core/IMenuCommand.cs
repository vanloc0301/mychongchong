namespace SkyMap.Net.Core
{
    using System;

    public interface IMenuCommand : ICommand
    {
        bool IsEnabled { get; set; }

        bool Visible { get; set; }
    }
}

