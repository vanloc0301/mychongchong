namespace SkyMap.Net.Tools.Criteria
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Size=1)]
    public struct Field
    {
        public const string f_fldFieldId = "FIELD_ID";
        public const string f_fldTableName = "TABLE_NAME";
        public const string f_fldFieldName = "FIELD_NAME";
        public const string f_fldFieldType = "FIELD_TYPE";
        public const string f_fldIsDisp = "FIELD_ISDISP";
        public const string f_fldDispName = "FIELD_DISPNAME";
        public const string f_fldIsCondition = "FIELD_ISCONDITION";
        public const string f_fldConditionName = "FIELD_CONDITIONNAME";
        public const string f_relId = "RELATIONTABLE_ID";
        public const string f_relPTbl = "RELATIONTABLE_PTABLE";
        public const string f_relPFld = "RELATIONTABLE_PFIELD";
        public const string f_relCTbl = "RELATIONTABLE_CTABLE";
        public const string f_relCFld = "RELATIONTABLE_CFIELD";
        public const string f_relOrder = "RELATIONTABLE_ORDER";
    }
}

