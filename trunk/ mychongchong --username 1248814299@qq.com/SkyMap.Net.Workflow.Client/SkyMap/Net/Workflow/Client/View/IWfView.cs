namespace SkyMap.Net.Workflow.Client.View
{
    using SkyMap.Net.DAO;
    using SkyMap.Net.DataForms;
    using SkyMap.Net.Workflow.Engine;
    using System;
    using System.Data;
    using System.Runtime.CompilerServices;

    public interface IWfView
    {
        event WfViewNavigationHandle Navigate;

        event EventHandler ViewClosed;

        void Close();
        void Pass();
        void Print();
        bool Save();
        void SetCurrentDataRowView(DataRowView row);
        void Show(string viewContent, IWfDataForm dataForm, SkyMap.Net.Workflow.Engine.WorkItem workItem);

        bool CanPass { get; set; }

        bool CanRemove { get; set; }

        UnitOfWork CurrentUnitOfWork { get; }

        IWfDataForm DataForm { get; }

        DataRowView[] NavigationDataRowViews { get; set; }

        int[] NavigationIndexs { get; set; }

        SkyMap.Net.DataForms.PrintSet PrintSet { get; }

        string PrintSetId { set; }

        SkyMap.Net.Workflow.Engine.WorkItem WorkItem { get; }
    }
}

