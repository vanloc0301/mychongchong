namespace SkyMap.Net.Criteria.Client
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Size=1)]
    public struct TableField
    {
        public const string fld_SearchId = "id";
        public const string fld_relSearchId = "Searchx_id";
        public const string fld_PreCondition = "PreCondition";
        public const string fld_searchname = "name";
        public const string fld_searchmany = "ISMANY";
        public const string fld_searchtbl = "TableName";
        public const string fld_filterid = "ID";
        public const string fld_relFilterId = "Filter_Id";
        public const string fld_filtername = "NAME";
        public const string fld_selcondtionId = "ID";
        public const string fld_relSelcondtionId = "Field_ID";
        public const string fld_selcondtion = "Condition_Name";
        public const string fld_isconditin = "is_condtion";
        public const string fld_type = "Field_Type";
        public const string fld_fldname = "Name";
        public const string fld_tblname = "TABLE_NAME";
        public const string fld_issel = "Is_display";
        public const string fld_dispname = "Display_Name";
        public const string fld_dispindex = "Display_Index";
        public const string fld_tableid = "ID";
        public const string fld_tablename = "TABLE_NAME";
        public const string fld_reltablename = "Relation_TABLE_NAME";
        public const string fld_tablekey = "TABLE_KEY";
        public const string fld_tablerelkey = "Relation_TABLE_KEY";
        public const string fld_tblorder = "TABLE_ORDER";
        public const string fld_FilterConditonId = "ID";
        public const string fld_rel = "Relation";
        public const string fld_op = "Operation";
        public const string fld_val = "Compare_Value";
    }
}

