namespace SkyMap.Net.DataForms.DataEngine
{
    using SkyMap.Net.DAO;
    using System;
    using System.Data;

    public interface ITraceDb
    {
        string GetTraceHistory(string table, string field, string keyValue);
        void SaveTraceToDb();
        void TraceDsAfterSave(DataSet ds);
        void TraceDsChange(DataSet ds);
        void TraceOuterAdd(string table, string keyValue, string description);
        void TraceOuterAdd(string table, string keyValue, string field, string fldValue, string description);
        void TraceOuterRemove(string[] tables, string[] keyValues, string description);

        SkyMap.Net.DAO.SMDataSource SMDataSource { get; set; }
    }
}

