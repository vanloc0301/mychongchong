namespace SkyMap.Net.Core
{
    using System;
    using System.Text.RegularExpressions;

    public class DisplayBindingDescriptor
    {
        private object binding = null;
        private SkyMap.Net.Core.Codon codon;
        private bool isSecondary;

        public DisplayBindingDescriptor(SkyMap.Net.Core.Codon codon)
        {
            this.isSecondary = codon.Properties["type"] == "Secondary";
            if ((!this.isSecondary && (codon.Properties["type"] != "")) && (codon.Properties["type"] != "Primary"))
            {
                MessageService.ShowWarning("Unknown display binding type: " + codon.Properties["type"]);
            }
            this.codon = codon;
        }

        public bool CanAttachToFile(string fileName)
        {
            string pattern = this.codon.Properties["fileNamePattern"];
            return (((pattern == null) || (pattern.Length == 0)) || Regex.IsMatch(fileName, pattern, RegexOptions.IgnoreCase));
        }

        public bool CanAttachToLanguage(string language)
        {
            string pattern = this.codon.Properties["languagePattern"];
            return (((pattern == null) || (pattern.Length == 0)) || Regex.IsMatch(language, pattern, RegexOptions.IgnoreCase));
        }

        public IDisplayBinding Binding
        {
            get
            {
                if (this.binding == null)
                {
                    this.binding = this.codon.AddIn.CreateObject(this.codon.Properties["class"]);
                }
                return (this.binding as IDisplayBinding);
            }
        }

        public SkyMap.Net.Core.Codon Codon
        {
            get
            {
                return this.codon;
            }
        }

        public bool IsSecondary
        {
            get
            {
                return this.isSecondary;
            }
        }

        public ISecondaryDisplayBinding SecondaryBinding
        {
            get
            {
                if (this.binding == null)
                {
                    this.binding = this.codon.AddIn.CreateObject(this.codon.Properties["class"]);
                }
                return (this.binding as ISecondaryDisplayBinding);
            }
        }
    }
}

