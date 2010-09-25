namespace SkyMap.Net.Core
{
    using System;
    using System.Collections;

    public class StringDoozer : IDoozer
    {
        public object BuildItem(object caller, Codon codon, ArrayList subItems)
        {
            return SkyMap.Net.Core.StringParser.Parse(codon.Properties["text"]);
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

