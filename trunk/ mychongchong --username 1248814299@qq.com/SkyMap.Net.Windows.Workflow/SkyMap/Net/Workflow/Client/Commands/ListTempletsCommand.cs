namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.DAO;
    using SkyMap.Net.Workflow.Client.Services;
    using SkyMap.Net.Workflow.XPDL;
    using SkyMap.Net.Workflow.XPDL.ExtendElement;
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public class ListTempletsCommand : AbstractBoxCommand, IComboBoxCommand, ICommand
    {
        private object caller;
        internal static WfTempletAppendix TempletAppendix;

        public override void Run()
        {
            ToolBarComboBox owner = this.Owner as ToolBarComboBox;
            TempletAppendix = owner.SelectedItem as WfTempletAppendix;
        }

        private void SetComboxItems()
        {
            ToolBarComboBox owner = this.Owner as ToolBarComboBox;
            if (owner.Items.Count == 0)
            {
                ProdefRow selectProdef = DisplayMyFirstProdefsCommand.SelectProdef;
                if (selectProdef != null)
                {
                    Prodef prodef = WorkflowService.Prodefs[selectProdef.Id];
                    if (!StringHelper.IsNull(prodef.TempletAppendixsId))
                    {
                        WfTempletAppendixs appendixs = QueryHelper.Get<WfTempletAppendixs>("WfTempletAppendixs_" + prodef.TempletAppendixsId, prodef.TempletAppendixsId);
                        if ((appendixs != null) && (appendixs.TempletAppendixs.Count > 0))
                        {
                            owner.Items.AddRange(new List<WfTempletAppendix>(appendixs.TempletAppendixs).ToArray());
                        }
                    }
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
                ToolBarComboBox owner = this.Owner as ToolBarComboBox;
                owner.ComboBox.DropDownHeight = 100;
                DisplayMyFirstProdefsCommand.ProdefChanged = (EventHandler)Delegate.Combine(DisplayMyFirstProdefsCommand.ProdefChanged, (MethodInvoker)delegate
                {
                    (this.Owner as ToolBarComboBox).Items.Clear();
                    this.SetComboxItems();
                });
                this.SetComboxItems();
            }
        }
    }
}

