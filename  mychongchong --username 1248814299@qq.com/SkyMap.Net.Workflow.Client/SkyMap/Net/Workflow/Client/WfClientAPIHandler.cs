namespace SkyMap.Net.Workflow.Client
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using SkyMap.Net.Workflow.Engine;

    public delegate void WfClientAPIHandler(IDictionary<string, WorkItem> workItems);
}

