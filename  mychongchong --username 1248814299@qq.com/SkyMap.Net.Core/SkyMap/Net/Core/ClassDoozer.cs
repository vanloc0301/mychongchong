namespace SkyMap.Net.Core
{
    using System;
    using System.Collections;

    public class ClassDoozer : IDoozer
    {
        public object BuildItem(object caller, Codon codon, ArrayList subItems)
        {
            return codon.AddIn.CreateObject(codon.Properties["class"]);
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

