namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Security;
    using SkyMap.Net.Workflow.Client.Services;
    using SkyMap.Net.Workflow.XPDL;
    using System;
    using System.Collections.Generic;

    public class DisplayMyFirstProdefsCommand : AbstractBoxCommand, IComboBoxCommand, ICommand
    {
        private object caller;
        public static EventHandler ProdefChanged;
        private static ProdefRow selectProdef;

        public DisplayMyFirstProdefsCommand()
        {
            SecurityUtil.CurrentPrincipalChanged += new EventHandler(this.SecurityUtil_CurrentPrincipalChanged);
        }

        public override void Run()
        {
            selectProdef = (this.Owner as ToolBarComboBox).SelectedItem as ProdefRow;
            if (ProdefChanged != null)
            {
                ProdefChanged(null, null);
            }
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
                IList<ProdefRow> myFirstProdefs = WorkflowService.GetMyFirstProdefs();
                foreach (ProdefRow row in myFirstProdefs)
                {
                    owner.Items.Add(row);
                }
            }
        }

        public object Caller
        {
            get
            {
                return this.caller;
            }
            set
            {
                this.caller = value;
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

