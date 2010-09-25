namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Workflow.Client.Box;
    using SkyMap.Net.Workflow.Client.View;
    using System;
    using System.Data;

    public class DefaultOpenZbViewCommand : DefaultOpenViewCommand
    {
        private bool _canPass = true;

        protected override void OpenView(IWfBox box, IWfView view, DataRowView row, int index)
        {
            base.OpenView(box, view, row, index);
        }

        protected override bool CanEdit
        {
            get
            {
                return true;
            }
        }

        public bool CanPass
        {
            get
            {
                return this._canPass;
            }
            set
            {
                this._canPass = value;
            }
        }
    }
}

