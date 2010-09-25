namespace SkyMap.Net.Core
{
    using System;

    public interface ITextEditCommand : ICommand
    {
        bool IsEnabled { get; set; }
    }
}

