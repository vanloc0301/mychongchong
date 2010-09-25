namespace SkyMap.Net.Workflow.Engine
{
    using System;
    using System.Collections.Generic;

    public interface IWfProcessMgr
    {
        IWfProcess CreateProcess(IWfRequester requester);
        IList<IWfProcess> GetListProcess();
        int HowManyProcess();

        string Description { get; }

        string Name { get; }

        ProcessMgrStateType ProcessMgrState { get; }

        string Version { get; }
    }
}

