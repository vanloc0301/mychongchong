namespace SkyMap.Net.DAO
{
    using System;

    [Serializable, AttributeUsage(AttributeTargets.Method)]
    public sealed class NoDaoAttribute : Attribute
    {
    }
}

