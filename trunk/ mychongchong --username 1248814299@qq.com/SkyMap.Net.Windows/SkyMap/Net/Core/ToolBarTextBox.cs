namespace SkyMap.Net.Core
{
    using System;
    using System.Windows.Forms;

    public class ToolBarTextBox : ToolStripTextBox, IStatusUpdate
    {
        private object caller;
        private Codon codon;
        private string description = string.Empty;
        private ITextEditCommand menuCommand = null;

        public ToolBarTextBox(Codon codon, object caller)
        {
            this.RightToLeft = RightToLeft.Inherit;
            base.TextBox.LostFocus += new EventHandler(this.TextBox_LostFocus);
            this.caller = caller;
            this.codon = codon;
            this.CreateCommand(codon);
            this.UpdateText();
            this.UpdateStatus();
        }

        private void CreateCommand(Codon codon)
        {
            this.menuCommand = (ITextEditCommand) codon.AddIn.CreateObject(codon.Properties["class"]);
            if (this.menuCommand == null)
            {
                throw new NullReferenceException("Can't create textbox command");
            }
            this.menuCommand.ID = codon.Id;
            this.menuCommand.Owner = this;
            this.menuCommand.Codon = codon;
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
        }

        private void TextBox_LostFocus(object sender, EventArgs e)
        {
            this.MenuCommand.Run();
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
            if (this.codon.Properties.Contains("label"))
            {
                this.Text = SkyMap.Net.Core.StringParser.Parse(this.codon.Properties["label"]);
            }
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
                bool flag = this.codon.GetFailedAction(this.caller) != ConditionFailedAction.Disable;
                if (this.menuCommand != null)
                {
                    flag &= this.menuCommand.IsEnabled;
                }
                return flag;
            }
        }

        public ITextEditCommand MenuCommand
        {
            get
            {
                return this.menuCommand;
            }
        }
    }
}

