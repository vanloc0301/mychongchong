namespace SkyMap.Net.Workflow.Engine
{
    using System;

    public interface IWfRequester
    {
        object GetInstanceObject();
        void ReceiveEvent(IWfEventAudit audit);
        void RegisterProcess(IWfProcess process);
        void Store();
    }
}

