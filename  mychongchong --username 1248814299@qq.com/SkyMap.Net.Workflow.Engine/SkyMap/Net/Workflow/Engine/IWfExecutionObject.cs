namespace SkyMap.Net.Workflow.Engine
{
    using SkyMap.Net.Workflow.Instance;
    using System;
    using System.Collections;

    public interface IWfExecutionObject
    {
        void Abort();
        void ChangeStatus(WfStatusType newstatus);
        object GetDefineOject();
        IList GetHistory();
        object GetInstanceObject();
        void Resume();
        void Suspend();
        void Terminate();

        string Description { get; set; }

        HowClosedType HowClosed { get; }

        string Key { get; }

        DateTime? LastStateTime { get; }

        string Name { get; set; }

        string PackageId { get; }

        string PackageName { get; }

        short Priority { get; set; }

        string ProdefId { get; }

        string ProdefName { get; }

        string ProdefVersion { get; }

        string State { get; }

        WfStateType WfState { get; }

        WhileOpenType WhileOpen { get; }

        WhyNotRunningType WhyNotRunning { get; }
    }
}

