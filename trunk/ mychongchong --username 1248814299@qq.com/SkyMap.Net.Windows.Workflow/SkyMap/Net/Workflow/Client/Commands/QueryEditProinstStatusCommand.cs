namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Workflow.Engine;
    using System;
    using System.Collections.Generic;
    using SkyMap.Net.Workflow.Instance;

    public class QueryEditProinstStatusCommand : QueryEditCommand, IComboBoxCommand, ICommand
    {
        private object caller;

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

        public override string Key
        {
            get
            {
                return "p.PROINST_STATUS";
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
                IDictionary<WfStatusType, string> statuses = WfUtil.GetStatuses();
                string[] array = new string[statuses.Count];
                statuses.Values.CopyTo(array, 0);
                (this.Owner as ToolBarComboBox).Items.AddRange(array);
            }
        }

        public override string Value
        {
            get
            {
                string sName = (this.Owner as ToolBarComboBox).SelectedItem.ToString();
                if (sName.Length > 0)
                {
                    return ("'" + Convert.ToInt16(WfUtil.GetWfStatus(sName)).ToString() + "'");
                }
                return string.Empty;
            }
        }
    }
}

