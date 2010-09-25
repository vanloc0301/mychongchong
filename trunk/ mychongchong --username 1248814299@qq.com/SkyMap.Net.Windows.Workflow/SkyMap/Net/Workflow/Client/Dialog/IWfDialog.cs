namespace SkyMap.Net.Workflow.Client.Dialog
{
    using SkyMap.Net.Workflow.Engine;
    using System;
    using System.Runtime.Remoting.Messaging;
    using System.Windows.Forms;

    public interface IWfDialog
    {
        void Close();
        void SetContextData(ILogicalThreadAffinative contextData);
        DialogResult ShowDialog();

        SkyMap.Net.Workflow.Engine.WorkItem WorkItem { get; set; }
    }
}

