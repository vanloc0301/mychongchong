namespace SkyMap.Net.BrowserDisplayBinding
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;

    public class BrowserDisplayBinding : IDisplayBinding
    {
        public bool CanCreateContentForFile(string fileName)
        {
            return (((fileName.StartsWith("http:") || fileName.StartsWith("https:")) || fileName.StartsWith("ftp:")) || fileName.StartsWith("browser://"));
        }

        public bool CanCreateContentForLanguage(string language)
        {
            return false;
        }

        public IViewContent CreateContentForFile(string fileName)
        {
            BrowserPane pane = new BrowserPane();
            if (fileName.StartsWith("browser://"))
            {
                pane.Load(fileName.Substring("browser://".Length));
                return pane;
            }
            if (LoggingService.IsInfoEnabled)
            {
                LoggingService.InfoFormatted("将使用浏览器打开:{0}", new object[] { fileName });
            }
            pane.Load(fileName);
            return pane;
        }

        public IViewContent CreateContentForLanguage(string language, string content)
        {
            return null;
        }
    }
}

