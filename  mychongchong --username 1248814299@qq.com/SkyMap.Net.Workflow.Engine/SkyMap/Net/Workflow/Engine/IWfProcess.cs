namespace SkyMap.Net.Workflow.Engine
{
    using System;
    using System.Collections.Generic;

    public interface IWfProcess : IWfExecutionObject
    {
        void ActivityComplete(IWfActivity activity);
        IList<IWfActivity> GetActivitiesInState(string State);
        IList<IWfActivity> GetListStep();
        int HowManyStep();
        void Start();
        void Start(string actdefId);

        IWfProcessMgr Manager { get; }

        string ProjectID { get; }

        IWfRequester Requester { get; set; }
    }
}

