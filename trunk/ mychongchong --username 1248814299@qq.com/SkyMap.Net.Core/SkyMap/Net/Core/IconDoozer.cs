namespace SkyMap.Net.Core
{
    using System;
    using System.Collections;

    public class IconDoozer : IDoozer
    {
        public object BuildItem(object caller, Codon codon, ArrayList subItems)
        {
            return new IconDescriptor(codon);
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

