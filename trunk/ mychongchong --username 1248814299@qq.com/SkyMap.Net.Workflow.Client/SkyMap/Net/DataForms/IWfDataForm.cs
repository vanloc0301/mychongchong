namespace SkyMap.Net.DataForms
{
    using SkyMap.Net.Workflow.Client.View;
    using System;

    public interface IWfDataForm : IDataForm
    {
        IWfView WfView { set; }
    }
}

