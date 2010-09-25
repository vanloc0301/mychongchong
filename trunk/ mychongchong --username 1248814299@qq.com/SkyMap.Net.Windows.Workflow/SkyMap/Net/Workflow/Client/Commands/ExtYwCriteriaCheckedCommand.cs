namespace SkyMap.Net.Workflow.Client.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Workflow.Client.Box;
    using System;
    using System.Collections.Generic;

    public class ExtYwCriteriaCheckedCommand : YwCriteriaCheckedCommand
    {
        public override void Run()
        {
            ToolBarCheckBox owner = this.Owner as ToolBarCheckBox;
            WfYwCriteria caller = owner.Caller as WfYwCriteria;
            if (caller != null)
            {
                List<IStatusUpdate> toolStripItems = caller.GetToolStripItems(this.LinkCommandType);
                foreach (IStatusUpdate update in toolStripItems)
                {
                    if (update.Command != null)
                    {
                        ExtQueryEditCommand command = (ExtQueryEditCommand) update.Command;
                        if (command.Key == this.Key)
                        {
                            command.IsEnabled = owner.Checked;
                            update.UpdateStatus();
                        }
                    }
                }
            }
        }

        private string Field
        {
            get
            {
                if (base.Codon.Properties.Contains("field"))
                {
                    return base.Codon.Properties["field"];
                }
                return string.Empty;
            }
        }

        protected override string Key
        {
            get
            {
                return this.Field;
            }
        }

        protected override Type LinkCommandType
        {
            get
            {
                return typeof(ExtQueryEditCommand);
            }
        }
    }
}

