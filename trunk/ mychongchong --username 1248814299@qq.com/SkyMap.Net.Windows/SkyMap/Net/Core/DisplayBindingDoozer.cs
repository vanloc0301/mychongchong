namespace SkyMap.Net.Core
{
    using System;
    using System.Collections;

    public class DisplayBindingDoozer : IDoozer
    {
        public object BuildItem(object caller, Codon codon, ArrayList subItems)
        {
            return new DisplayBindingDescriptor(codon);
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

