namespace SkyMap.Net.Core
{
    using System;
    using System.Windows.Forms;

    public class ToolBarCheckBox : ToolStripControlHost, IStatusUpdate
    {
        private object caller;
        private Codon codon;
        private string description;
        private ICheckableMenuCommand menuCommand;

        public ToolBarCheckBox(string text) : base(new CheckBox())
        {
            this.description = string.Empty;
            this.menuCommand = null;
            this.RightToLeft = RightToLeft.Inherit;
            this.Text = text;
        }

        public ToolBarCheckBox(Codon codon, object caller) : base(new CheckBox())
        {
            this.description = string.Empty;
            this.menuCommand = null;
            this.RightToLeft = RightToLeft.Inherit;
            this.caller = caller;
            this.codon = codon;
            try
            {
                this.menuCommand = (ICheckableMenuCommand) codon.AddIn.CreateObject(codon.Properties["class"]);
                this.menuCommand.ID = codon.Id;
                this.menuCommand.Owner = this;
                this.menuCommand.Codon = codon;
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
            }
            if (this.menuCommand == null)
            {
                MessageService.ShowError("Can't create toolbar checkbox : " + codon.Id);
            }
            this.UpdateText();
            this.UpdateStatus();
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (this.menuCommand != null)
            {
                this.menuCommand.Run();
            }
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
            base.Control.Text = SkyMap.Net.Core.StringParser.Parse(this.codon.Properties["label"]);
            if (this.codon.Properties.Contains("tooltip"))
            {
                base.ToolTipText = SkyMap.Net.Core.StringParser.Parse(this.codon.Properties["tooltip"]);
            }
        }

        public object Caller
        {
            get
            {
                return this.caller;
            }
        }

        public bool Checked
        {
            get
            {
                return ((CheckBox) base.Control).Checked;
            }
            set
            {
                ((CheckBox) base.Control).Checked = value;
            }
        }

        public ICommand Command
        {
            get
            {
                return this.menuCommand;
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
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
                return (this.codon.GetFailedAction(this.caller) != ConditionFailedAction.Disable);
            }
        }

        public ICheckableMenuCommand MenuCommand
        {
            get
            {
                return this.menuCommand;
            }
        }
    }
}

