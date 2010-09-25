namespace SkyMap.Net.Workflow.Client.Box
{
    using SkyMap.Net.Workflow.Client.Config;
    using System;

    public interface IBox
    {
        void Init(CBoxConfig boxConfig);
        void RefreshData();

        string BoxName { get; set; }

        bool Initialized { get; }
    }
}

