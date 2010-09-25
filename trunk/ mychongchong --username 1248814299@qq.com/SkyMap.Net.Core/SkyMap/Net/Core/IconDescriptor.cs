namespace SkyMap.Net.Core
{
    using System;

    public class IconDescriptor
    {
        private Codon codon;

        public IconDescriptor(Codon codon)
        {
            this.codon = codon;
        }

        public string[] Extensions
        {
            get
            {
                return this.codon.Properties["extensions"].Split(new char[] { ';' });
            }
        }

        public string Id
        {
            get
            {
                return this.codon.Id;
            }
        }

        public string Language
        {
            get
            {
                return this.codon.Properties["language"];
            }
        }

        public string Resource
        {
            get
            {
                return this.codon.Properties["resource"];
            }
        }
    }
}

