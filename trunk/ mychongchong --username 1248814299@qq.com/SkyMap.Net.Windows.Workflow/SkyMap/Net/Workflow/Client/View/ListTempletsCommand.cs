namespace SkyMap.Net.Workflow.Client.View
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Workflow.Client.Services;
    using SkyMap.Net.Workflow.Instance;
    using SkyMap.Net.Workflow.XPDL;
    using SkyMap.Net.Workflow.XPDL.ExtendElement;
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public class ListTempletsCommand : AbstractComboBoxCommand
    {
        private void barItem_Click(object sender, EventArgs e)
        {
            ToolBarComboBox owner = this.Owner as ToolBarComboBox;
            if (owner.Items.Count == 0)
            {
                MatersEdit caller = owner.Caller as MatersEdit;
                Prodef prodef = WorkflowService.Prodefs[caller.ProdefId];
                if ((prodef != null) && !StringHelper.IsNull(prodef.TempletAppendixsId))
                {
                    WfTempletAppendixs appendixs = QueryHelper.Get<WfTempletAppendixs>("WfTempletAppendixs_" + prodef.TempletAppendixsId, prodef.TempletAppendixsId);
                    if ((appendixs != null) && (appendixs.TempletAppendixs.Count > 0))
                    {
                        owner.Items.AddRange(new List<WfTempletAppendix>(appendixs.TempletAppendixs).ToArray());
                    }
                }
            }
        }

        public override void Run()
        {
            ToolBarComboBox owner = this.Owner as ToolBarComboBox;
            MatersEdit caller = owner.Caller as MatersEdit;
            WfTempletAppendix selectedItem = owner.SelectedItem as WfTempletAppendix;
            for (int i = 0; i < selectedItem.WfAppendixs.Count; i++)
            {
                WfAppendix appendix = selectedItem.WfAppendixs[i];
                if (!caller.CurrentMatersLocate(appendix))
                {
                    WfProinstMater mater = AddMaterFromTempletCommand.AddMaterFormTemplet(appendix, caller.ProinstId);
                    caller.CurrentUnitOfWork.RegisterNew(mater);
                    caller.AddMater(mater);
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
                ToolBarComboBox owner = this.Owner as ToolBarComboBox;
                owner.ComboBox.DropDownHeight = 100;
                owner.Click += new EventHandler(this.barItem_Click);
                MatersEdit caller = owner.Caller as MatersEdit;
                caller.ProdefIDChange = (EventHandler)Delegate.Combine(caller.ProdefIDChange, (EventHandler)delegate
                {
                    (this.Owner as ToolBarComboBox).Items.Clear();
                });
            }
        }
    }
}

