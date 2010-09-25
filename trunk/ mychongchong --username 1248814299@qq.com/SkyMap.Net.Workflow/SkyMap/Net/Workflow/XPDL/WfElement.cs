namespace SkyMap.Net.Workflow.XPDL
{
    using System;

    public interface WfElement
    {
        string Description { get; set; }

        string Id { get; set; }

        string Name { get; set; }

        int ReplicationVersion { get; set; }
    }
}

