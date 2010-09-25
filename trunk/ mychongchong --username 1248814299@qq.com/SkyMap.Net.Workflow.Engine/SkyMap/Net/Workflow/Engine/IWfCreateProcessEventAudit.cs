namespace SkyMap.Net.Workflow.Engine
{
    using System;

    public interface IWfCreateProcessEventAudit : IWfEventAudit
    {
        string ParentActivityKey { get; }

        string ParentProcessKey { get; }

        string ParentProcessMgrName { get; }

        string ParentProcessMgrVersion { get; }
    }
}

