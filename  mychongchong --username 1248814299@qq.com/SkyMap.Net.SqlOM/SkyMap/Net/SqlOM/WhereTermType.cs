namespace SkyMap.Net.SqlOM
{
    using System;

    internal enum WhereTermType
    {
        Compare,
        Between,
        In,
        NotIn,
        InSubQuery,
        NotInSubQuery,
        IsNull,
        IsNotNull,
        Exists,
        NotExists
    }
}

