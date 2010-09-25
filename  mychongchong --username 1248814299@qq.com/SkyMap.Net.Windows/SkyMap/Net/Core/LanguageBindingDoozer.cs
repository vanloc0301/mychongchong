namespace SkyMap.Net.Core
{
    using System;
    using System.Collections;

    public class LanguageBindingDoozer : IDoozer
    {
        public object BuildItem(object caller, Codon codon, ArrayList subItems)
        {
            return new LanguageBindingDescriptor(codon);
        }

        public bool HandleConditions
        {
            get
            {
                return false;
            }
        }
    }
}

