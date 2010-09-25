namespace SkyMap.Net.DataForms
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Size=1)]
    public struct ParamNames
    {
        public const string PStaffId = "StaffId";
        public const string PStaffName = "StaffName";
        public const string PDeptId = "DeptId";
        public const string PDeptName = "DeptName";
        public const string PProjectId = "ProjectId";
        public const string PProinstId = "ProinsId";
        public const string PActinstId = "ActinsId";
        public const string PActdefId = "ActdefId";
        public const string PPageIndex = "PageIndex";
    }
}

