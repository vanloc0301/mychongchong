namespace SkyMap.Net.Core
{
    using System;

    public interface IStringTagProvider
    {
        string Convert(string tag);

        string[] Tags { get; }
    }
}

