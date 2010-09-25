namespace SkyMap.Net.Core
{
    using System;
    using System.Collections;
    using System.IO;

    public class DirectoryDoozer : IDoozer
    {
        public object BuildItem(object caller, Codon codon, ArrayList subItems)
        {
            return Path.Combine(Path.GetDirectoryName(codon.AddIn.FileName), codon.Properties["path"]);
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

