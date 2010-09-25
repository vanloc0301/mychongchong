namespace SkyMap.Net.Core
{
    using System;
    using System.Collections;

    public class PadDoozer : IDoozer
    {
        public object BuildItem(object caller, Codon codon, ArrayList subItems)
        {
            return new PadDescriptor(codon);
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

