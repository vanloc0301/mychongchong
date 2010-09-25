namespace SkyMap.Net.Workflow.Instance
{
    using System;

    [Serializable]
    public enum WfReleaseType
    {
        NotReleased,
        Resumed,
        AbortDeleted,
        TerminateCompleted
    }
}

