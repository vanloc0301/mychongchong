namespace SkyMap.Net.Core
{
    using System;
    using System.Collections;

    public class FileFilterDoozer : IDoozer
    {
        public object BuildItem(object caller, Codon codon, ArrayList subItems)
        {
            return (SkyMap.Net.Core.StringParser.Parse(codon.Properties["name"]) + "|" + codon.Properties["extensions"]);
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

