namespace SkyMap.Net.Core
{
    using System;
    using System.Windows.Forms;

    public class ToolBarDropDownButton : ToolStripDropDownButton, IStatusUpdate
    {
        private object caller;
        private Codon codon;
        private ICommand menuCommand = null;

        public ToolBarDropDownButton(Codon codon, object caller)
        {
            this.RightToLeft = RightToLeft.Inherit;
            this.caller = caller;
            this.codon = codon;
            if ((this.Image == null) && codon.Properties.Contains("icon"))
            {
                this.Image = ResourceService.GetBitmap(codon.Properties["icon"]);
            }
            this.CreateCommand(codon);
            this.UpdateStatus();
            this.UpdateText();
        }

        private void CreateCommand(Codon codon)
        {
            this.menuCommand = codon.AddIn.CreateObject(codon.Properties["class"]) as ICommand;
            this.menuCommand.ID = codon.Id;
            this.menuCommand.Owner = this;
            this.menuCommand.Codon = codon;
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this.menuCommand.Run();
        }

        public virtual void UpdateStatus()
        {
            if (this.codon != null)
            {
                ConditionFailedAction failedAction = this.codon.GetFailedAction(this.caller, this.menuCommand);
                base.Visible = failedAction != ConditionFailedAction.Exclude;
                this.Enabled = failedAction != ConditionFailedAction.Disable;
            }
        }

        public virtual void UpdateText()
        {
            if (this.codon != null)
            {
                base.ToolTipText = SkyMap.Net.Core.StringParser.Parse(this.codon.Properties["tooltip"]);
            }
        }

        public ICommand Command
        {
            get
            {
                return this.menuCommand;
            }
        }

        public override bool Enabled
        {
            get
            {
                if (this.codon == null)
                {
                    return base.Enabled;
                }
                bool flag = this.codon.GetFailedAction(this.caller) != ConditionFailedAction.Disable;
                if ((this.menuCommand != null) && (this.menuCommand is IMenuCommand))
                {
                    flag &= ((IMenuCommand) this.menuCommand).IsEnabled;
                }
                return flag;
            }
        }
    }
}

