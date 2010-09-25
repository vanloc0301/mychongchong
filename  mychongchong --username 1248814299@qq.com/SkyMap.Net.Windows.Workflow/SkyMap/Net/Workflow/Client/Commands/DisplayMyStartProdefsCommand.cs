namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Security;
    using SkyMap.Net.Workflow.Client.Services;
    using SkyMap.Net.Workflow.XPDL;
    using System;
    using System.Collections.Generic;

    public class DisplayMyStartProdefsCommand : AbstractBoxCommand, IComboBoxCommand, ICommand
    {
        public static ProdefRow selectProdef;

        public DisplayMyStartProdefsCommand()
        {
            SecurityUtil.CurrentPrincipalChanged += new EventHandler(this.SecurityUtil_CurrentPrincipalChanged);
        }

        public override void Run()
        {
            selectProdef = (this.Owner as ToolBarComboBox).SelectedItem as ProdefRow;
        }

        private void SecurityUtil_CurrentPrincipalChanged(object sender, EventArgs e)
        {
            selectProdef = null;
            this.SetComboxItems();
        }

        private void SetComboxItems()
        {
            ToolBarComboBox owner = this.Owner as ToolBarComboBox;
            if (owner != null)
            {
                owner.Items.Clear();
                IList<ProdefRow> myProdefs = WorkflowService.GetMyProdefs("FLW");
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

        public static ProdefRow SelectProdef
        {
            get
            {
                return selectProdef;
            }
        }
    }
}

