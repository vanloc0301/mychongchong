namespace SkyMap.Net.Tools.Criteria
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Size=1)]
    public struct Table
    {
        public const string tblField = "TY_FIELD";
        public const string tblSearchTable = "TY_RELATIONTABLE";
    }
}

