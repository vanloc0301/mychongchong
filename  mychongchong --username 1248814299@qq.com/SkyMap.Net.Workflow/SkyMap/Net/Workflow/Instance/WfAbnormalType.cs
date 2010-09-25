namespace SkyMap.Net.Workflow.Instance
{
    using System;

    [Serializable]
    public enum WfAbnormalType
    {
        ABROTED = 400,
        ALL_DELEGATED = 850,
        CALL_BACK = 100,
        CANCEL_ACCEPTED = 200,
        DELEGATED = 800,
        MONITOR = 900,
        NO_ABNORMAL = 0,
        PRESS = 0x3e8,
        SEND_BACKED = 600,
        SEND_BACKING = 650,
        SUSPENDED = 500,
        TERMINATED = 700
    }
}

