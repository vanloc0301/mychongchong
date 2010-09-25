namespace SkyMap.Net.Workflow.Instance
{
    using System;

    [Serializable]
    public enum WfStatusType
    {
        WF_ABORTED = 4,
        WF_COMPLETED = 2,
        WF_NOT_STARTED = 3,
        WF_RUNNING = 1,
        WF_SUSPENDED = 5,
        WF_TERMINATED = 7
    }
}

