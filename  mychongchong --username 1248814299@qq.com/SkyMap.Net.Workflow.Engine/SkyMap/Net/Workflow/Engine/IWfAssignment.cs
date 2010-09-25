namespace SkyMap.Net.Workflow.Engine
{
    using SkyMap.Net.Workflow.Instance;
    using System;

    public interface IWfAssignment
    {
        void Accept();
        void CancelAccept();
        void ChangeStatus(AssignStatusType Status);
        void ChangeStatus(WfAbnormalType abnormal);
        void Complete();
        object GetInstanceObject();
        void Resume();
        void SendBack(IWfAssignment sendBackAssign);
        void SetAbnormal(WfAbnormalType abnormal);
        void SetDelegate();
        void Store();

        WfAbnormalType AbnormalStatus { get; }

        IWfActivity Activity { get; }

        IWfResource Assignee { get; }

        DateTime? FromDate { get; }

        string Key { get; }

        IWfProcess Process { get; }

        string StaffId { get; }

        string StaffName { get; }

        AssignStatusType Status { get; }
    }
}

