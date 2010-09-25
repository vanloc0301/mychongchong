namespace SkyMap.Net.SqlOM
{
    using System;

    internal enum SqlExpressionType
    {
        Function,
        Field,
        Constant,
        SubQueryText,
        SubQueryObject,
        PseudoField,
        Parameter,
        Raw,
        Case,
        IfNull,
        Null
    }
}

