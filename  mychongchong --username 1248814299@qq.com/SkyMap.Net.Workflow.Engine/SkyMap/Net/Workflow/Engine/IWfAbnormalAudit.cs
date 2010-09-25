namespace SkyMap.Net.Workflow.Engine
{
    using SkyMap.Net.Workflow.Instance;
    using System;

    public interface IWfAbnormalAudit : IWfEventAudit
    {
        void Create(WfAbnormalType type);
        void Decision(WfDecisionStatusType status);
        void Release(WfReleaseType releaseType);

        bool Decisioned { get; }

        bool Released { get; }
    }
}

