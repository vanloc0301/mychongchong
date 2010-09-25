namespace SkyMap.Net.Gui
{
    using System;

    public interface IParseableContent
    {
        string ParseableContentName { get; }

        string ParseableText { get; }
    }
}

