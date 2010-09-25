namespace SkyMap.Net.Workflow.Client.Box
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Runtime.CompilerServices;

    public interface IWfBox : IBox
    {
        event RunWorkerCompletedEventHandler LoadDataCompleted;

        void CancelSelect();
        void CancelTopSelectedRow();
        void DeleteRows(DataRow[] rows);
        DataRow[] GetSelectedRows();
        DataRow[] GetSelectedRows(string[] batchFields);
        void SelectAll();
        void SelectCurrent();
        void SelectElse();

        string DAONameSpace { get; }

        object DataSource { get; set; }

        int FocusedRowIndex { get; }

        string IdField { get; }

        bool IsSelected { get; set; }

        string QueryName { get; }

        string[] QueryParameters { get; }
    }
}

