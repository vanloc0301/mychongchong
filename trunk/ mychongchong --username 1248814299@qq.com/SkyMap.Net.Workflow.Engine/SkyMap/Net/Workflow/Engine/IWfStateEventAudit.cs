namespace SkyMap.Net.Workflow.Engine
{
    using SkyMap.Net.Workflow.Instance;

    public interface IWfStateEventAudit : IWfEventAudit
    {
        WfStatusType NewState { get; }

        WfStatusType OldState { get; }
    }
}

