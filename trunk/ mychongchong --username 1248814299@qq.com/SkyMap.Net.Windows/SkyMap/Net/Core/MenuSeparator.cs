namespace SkyMap.Net.Core
{
    using System;
    using System.Windows.Forms;

    public class MenuSeparator : ToolStripSeparator, IStatusUpdate
    {
        private object caller;
        private Codon codon;

        public MenuSeparator()
        {
            this.RightToLeft = RightToLeft.Inherit;
        }

        public MenuSeparator(Codon codon, object caller)
        {
            this.RightToLeft = RightToLeft.Inherit;
            this.caller = caller;
            this.codon = codon;
        }

        public virtual void UpdateStatus()
        {
            if (this.codon != null)
            {
                ConditionFailedAction failedAction = this.codon.GetFailedAction(this.caller);
                this.Enabled = failedAction != ConditionFailedAction.Disable;
                base.Visible = failedAction != ConditionFailedAction.Exclude;
            }
        }

        public virtual void UpdateText()
        {
        }

        public ICommand Command
        {
            get
            {
                return null;
            }
        }
    }
}

