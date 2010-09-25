namespace SkyMap.Net.Core
{
    using SkyMap.Net.Project;
    using System;

    public class LanguageBindingDescriptor
    {
        private ILanguageBinding binding = null;
        private SkyMap.Net.Core.Codon codon;

        public LanguageBindingDescriptor(SkyMap.Net.Core.Codon codon)
        {
            this.codon = codon;
        }

        public ILanguageBinding Binding
        {
            get
            {
                if (this.binding == null)
                {
                    this.binding = (ILanguageBinding) this.codon.AddIn.CreateObject(this.codon.Properties["class"]);
                }
                return this.binding;
            }
        }

        public SkyMap.Net.Core.Codon Codon
        {
            get
            {
                return this.codon;
            }
        }

        public string Guid
        {
            get
            {
                return this.codon.Properties["guid"];
            }
        }

        public string Language
        {
            get
            {
                return this.codon.Id;
            }
        }

        public string ProjectFileExtension
        {
            get
            {
                return this.codon.Properties["projectfileextension"];
            }
        }

        public string[] Supportedextensions
        {
            get
            {
                return this.codon.Properties["supportedextensions"].Split(new char[] { ';' });
            }
        }
    }
}

