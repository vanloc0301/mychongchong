namespace SkyMap.Net.BrowserDisplayBinding
{
    using SkyMap.Net.Core;
    using System;

    public class SchemeExtensionDescriptor
    {
        private Codon codon;
        private ISchemeExtension extension;
        private string schemeName;

        public SchemeExtensionDescriptor(Codon codon)
        {
            this.codon = codon;
            this.schemeName = codon.Properties["scheme"];
            if ((this.schemeName == null) || (this.schemeName.Length == 0))
            {
                this.schemeName = codon.Id;
            }
        }

        public ISchemeExtension Extension
        {
            get
            {
                if (this.extension == null)
                {
                    this.extension = (ISchemeExtension) this.codon.AddIn.CreateObject(this.codon.Properties["class"]);
                }
                return this.extension;
            }
        }

        public string SchemeName
        {
            get
            {
                return this.schemeName;
            }
        }
    }
}

