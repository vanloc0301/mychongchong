namespace SkyMap.Net.Core
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml;

    public static class LanguageService
    {
        private static ImageList languageImageList = null;
        private static string languagePath = FileUtility.Combine(new string[] { PropertyService.DataDirectory, "resources", "languages" });
        private static List<Language> languages = null;

        static LanguageService()
        {
            languageImageList = new ImageList();
            languageImageList.ColorDepth = ColorDepth.Depth32Bit;
            languages = new List<Language>();
            LanguageImageList.ImageSize = new Size(0x2e, 0x26);
            XmlDocument document = new XmlDocument();
            document.Load(Path.Combine(languagePath, "LanguageDefinition.xml"));
            XmlNodeList childNodes = document.DocumentElement.ChildNodes;
            foreach (XmlNode node in childNodes)
            {
                XmlElement element = node as XmlElement;
                if (element != null)
                {
                    languages.Add(new Language(element.Attributes["name"].InnerText, element.Attributes["code"].InnerText, LanguageImageList.Images.Count));
                    LanguageImageList.Images.Add(new Bitmap(Path.Combine(languagePath, element.Attributes["icon"].InnerText)));
                }
            }
        }

        public static ImageList LanguageImageList
        {
            get
            {
                return languageImageList;
            }
        }

        public static List<Language> Languages
        {
            get
            {
                return languages;
            }
        }
    }
}

