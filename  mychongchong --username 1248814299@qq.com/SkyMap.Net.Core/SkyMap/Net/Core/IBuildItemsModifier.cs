namespace SkyMap.Net.Core
{
    using System;
    using System.Collections;

    public interface IBuildItemsModifier
    {
        void Apply(IList items);
    }
}

