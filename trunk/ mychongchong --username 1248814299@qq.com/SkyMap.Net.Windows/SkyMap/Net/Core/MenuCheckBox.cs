namespace SkyMap.Net.Core
{
    using System;
    using System.Windows.Forms;

    public class MenuCheckBox : ToolStripMenuItem, IStatusUpdate
    {
        private object caller;
        private Codon codon;
        private string description;
        private ICheckableMenuCommand menuCommand;

        public MenuCheckBox(string text)
        {
            this.description = string.Empty;
            this.menuCommand = null;
            this.RightToLeft = RightToLeft.Inherit;
            this.Text = text;
        }

        public MenuCheckBox(Codon codon, object caller)
        {
            this.description = string.Empty;
            this.menuCommand = null;
            this.RightToLeft = RightToLeft.Inherit;
            this.caller = caller;
            this.codon = codon;
            this.UpdateText();
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (this.codon != null)
            {
                this.MenuCommand.Run();
                base.Checked = this.MenuCommand.IsChecked;
            }
        }

        public virtual void UpdateStatus()
        {
            if (this.codon != null)
            {
                ConditionFailedAction failedAction = this.codon.GetFailedAction(this.caller, this.menuCommand);
                base.Visible = failedAction != ConditionFailedAction.Exclude;
                this.Enabled = failedAction != ConditionFailedAction.Disable;
                if (this.MenuCommand != null)
                {
                    base.Checked = this.MenuCommand.IsChecked;
                }
            }
        }

        public virtual void UpdateText()
        {
            if (this.codon != null)
            {
                this.Text = SkyMap.Net.Core.StringParser.Parse(this.codon.Properties["label"]);
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
                if (this.menuCommand == null)
                {
                    try
                    {
                        this.menuCommand = (ICheckableMenuCommand) this.codon.AddIn.CreateObject(this.codon.Properties["class"]);
                        this.menuCommand.ID = this.codon.Id;
                        this.menuCommand.Owner = this.caller;
                        this.menuCommand.Codon = this.codon;
                        this.UpdateStatus();
                    }
                    catch (Exception exception)
                    {
                        MessageService.ShowError(exception, "Can't create menu command : " + this.codon.Id);
                    }
                }
                return this.menuCommand;
            }
        }
    }
}

