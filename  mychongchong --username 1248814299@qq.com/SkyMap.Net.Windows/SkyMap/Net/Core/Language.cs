namespace SkyMap.Net.Core
{
    using System;

    public class Language
    {
        private string code;
        private int imageIndex;
        private string name;

        public Language(string name, string code, int imageIndex)
        {
            this.name = name;
            this.code = code;
            this.imageIndex = imageIndex;
        }

        public string Code
        {
            get
            {
                return this.code;
            }
        }

        public int ImageIndex
        {
            get
            {
                return this.imageIndex;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }
    }
}

