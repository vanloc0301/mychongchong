namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Workflow.Client.Box;
    using System;

    public abstract class YwCriteriaCheckedCommand : AbstractCheckableMenuCommand
    {
        protected YwCriteriaCheckedCommand()
        {
        }

        public override void Run()
        {
            ToolBarCheckBox owner = this.Owner as ToolBarCheckBox;
            WfYwCriteria caller = owner.Caller as WfYwCriteria;
            if (caller != null)
            {
                IStatusUpdate toolStripItem = caller.GetToolStripItem(this.LinkCommandType);
                if ((toolStripItem != null) && (toolStripItem.Command != null))
                {
                    ((QueryEditCommand) toolStripItem.Command).IsEnabled = owner.Checked;
                    toolStripItem.UpdateStatus();
                }
            }
        }

        protected abstract string Key { get; }

        protected abstract Type LinkCommandType { get; }

        public override object Owner
        {
            get
            {
                return base.Owner;
            }
            set
            {
                base.Owner = value;
                (this.Owner as ToolBarCheckBox).Checked = false;
            }
        }
    }
}

