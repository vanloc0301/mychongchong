namespace SkyMap.Net.BrowserDisplayBinding
{
    using SkyMap.Net.Core;
    using System;
    using System.Collections;

    public class SchemeExtensionDoozer : IDoozer
    {
        public object BuildItem(object caller, Codon codon, ArrayList subItems)
        {
            return new SchemeExtensionDescriptor(codon);
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

