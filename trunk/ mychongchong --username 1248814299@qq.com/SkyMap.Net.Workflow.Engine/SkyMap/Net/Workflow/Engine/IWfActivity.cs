namespace SkyMap.Net.Workflow.Engine
{
    using SkyMap.Net.Workflow.XPDL;
    using System;

    public interface IWfActivity : IWfExecutionObject
    {
        void Activate();
        void Complete();
        void Start();

        string ActdefId { get; }

        IWfProcess Container { get; }

        ActdefType Type { get; }
    }
}

