namespace SkyMap.Net.Workflow.Engine
{
    using SkyMap.Net.Workflow.Instance;
    using System;
    using System.Collections;

    public interface IWfResource
    {
        void Assign();
        void CallBack();
        IList GetListWorkItem();
        WfResinst GetWfResinst();
        int HowManyWorkItem();
        void Store();
        void WorkItemCompleted();

        bool IsAssigned { get; }

        string Key { get; }

        string Name { get; }

        IWfActivity WfActivity { get; }
    }
}

