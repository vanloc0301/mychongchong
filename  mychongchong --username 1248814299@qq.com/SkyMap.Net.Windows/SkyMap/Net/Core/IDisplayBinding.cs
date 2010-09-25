namespace SkyMap.Net.Core
{
    using SkyMap.Net.Gui;
    using System;

    public interface IDisplayBinding
    {
        bool CanCreateContentForFile(string fileName);
        bool CanCreateContentForLanguage(string languageName);
        IViewContent CreateContentForFile(string fileName);
        IViewContent CreateContentForLanguage(string languageName, string content);
    }
}

