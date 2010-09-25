namespace SkyMap.Net.Core
{
    using System;
    using System.Collections;

    public interface IDoozer
    {
        object BuildItem(object caller, Codon codon, ArrayList subItems);

        bool HandleConditions { get; }
    }
}

