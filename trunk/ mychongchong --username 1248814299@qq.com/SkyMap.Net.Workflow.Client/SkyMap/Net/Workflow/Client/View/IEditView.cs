namespace SkyMap.Net.Workflow.Client.View
{
    using SkyMap.Net.DAO;
    using System;

    public interface IEditView
    {
        void BarStatusUpdate();
        void LoadData(UnitOfWork currentUnitOfWork, string proinstID, string prodefID, string assignId, string[] projectSubTypes);
        void PostEditor();
    }
}

