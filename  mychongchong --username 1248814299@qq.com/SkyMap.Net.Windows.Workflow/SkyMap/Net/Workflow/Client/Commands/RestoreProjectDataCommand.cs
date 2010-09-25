namespace SkyMap.Net.Workflow.Client.Commands
{
    using System;

    public class RestoreProjectDataCommand : AbstractWfViewCommand
    {
        public override void Run()
        {
            if (base.view != null)
            {
                base.view.DataForm.RestoreFailSavedDataSet();
            }
        }
    }
}

