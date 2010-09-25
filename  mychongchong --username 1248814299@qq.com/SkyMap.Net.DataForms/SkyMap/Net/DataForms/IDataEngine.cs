namespace SkyMap.Net.DataForms
{
    using System;
    using System.Data;

    public interface IDataEngine
    {
        DataSet GetDs(string dataFormID, string keyValue);
        string GetTraceHistory(string smDSOID, string table, string field, string keyValue);
        DataSet SaveData(string dataFormID, DataSet ds);
    }
}

