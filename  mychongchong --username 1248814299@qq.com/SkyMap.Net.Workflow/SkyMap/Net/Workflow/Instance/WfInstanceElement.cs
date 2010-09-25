namespace SkyMap.Net.Workflow.Instance
{
    using System;
    using System.Collections.Generic;

    public interface WfInstanceElement
    {
        double CostTime { get; set; }

        DateTime? CreateDate { get; set; }

        string Description { get; set; }

        DateTime? DueDate { get; set; }

        double DueTime { get; set; }

        DateTime? EndDate { get; set; }

        string Id { get; set; }

        DateTime? LastStateDate { get; set; }

        string Name { get; set; }

        string PackageId { get; set; }

        string PackageName { get; set; }

        short Priority { get; set; }

        string ProdefId { get; set; }

        string ProdefName { get; set; }

        string ProdefVersion { get; set; }

        int ReplicationVersion { get; set; }

        DateTime? StartDate { get; set; }

        IList<WfStateEventAuditInst> StateEvents { get; set; }

        WfStatusType Status { get; set; }

        IList<WfAbnormalAuditInst> WfAbnormalAudits { get; set; }
    }
}

