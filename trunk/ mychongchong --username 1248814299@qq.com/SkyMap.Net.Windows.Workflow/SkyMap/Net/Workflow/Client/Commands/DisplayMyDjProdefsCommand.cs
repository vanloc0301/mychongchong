namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Security;
    using SkyMap.Net.Workflow.Client.Box;
    using SkyMap.Net.Workflow.Client.Services;
    using SkyMap.Net.Workflow.XPDL;
    using System;
    using System.Collections.Generic;

    public class DisplayMyDjProdefsCommand : AbstractBoxCommand, IComboBoxCommand, ICommand
    {
        public DisplayMyDjProdefsCommand()
        {
            SecurityUtil.CurrentPrincipalChanged += new EventHandler(this.SecurityUtil_CurrentPrincipalChanged);
        }

        public override void Run()
        {
            ToolBarComboBox owner = this.Owner as ToolBarComboBox;
            Dj.CurrentProdef = owner.SelectedItem as ProdefRow;
            Dj caller = owner.Caller as Dj;
            if (caller != null)
            {
                caller.RefreshData();
            }
        }

        private void SecurityUtil_CurrentPrincipalChanged(object sender, EventArgs e)
        {
            Dj.CurrentProdef = null;
            this.SetComboxItems();
        }

        private void SetComboxItems()
        {
            ToolBarComboBox owner = this.Owner as ToolBarComboBox;
            if (owner != null)
            {
                owner.Items.Clear();
                IList<ProdefRow> myProdefs = WorkflowService.GetMyProdefs("REG");
                foreach (ProdefRow row in myProdefs)
                {
                    owner.Items.Add(row);
                }
            }
        }

        public override object Owner
        {
            get
            {
                return base.Owner;
            }
            set
            {
                base.Owner = value;
                this.SetComboxItems();
            }
        }
    }
}

