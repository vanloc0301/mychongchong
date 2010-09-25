namespace SkyMap.Net.Workflow.Client.View
{
    using SkyMap.Net.Workflow.Client.Box;
    using System;
    using System.Data;
    using System.Runtime.CompilerServices;

    public delegate void WfViewNavigationHandle(IWfBox box, IWfView view, DataRowView row, int index);
}

