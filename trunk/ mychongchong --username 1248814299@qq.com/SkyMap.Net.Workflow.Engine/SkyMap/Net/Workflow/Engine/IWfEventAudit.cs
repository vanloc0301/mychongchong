namespace SkyMap.Net.Workflow.Engine
{
    using System;

    public interface IWfEventAudit
    {
        string ActivityKey { get; }

        string ActivityName { get; }

        string EventType { get; }

        string ProcessKey { get; }

        string ProcessName { get; }

        IWfExecutionObject Source { get; }

        DateTime? TimeStamp { get; }
    }
}

