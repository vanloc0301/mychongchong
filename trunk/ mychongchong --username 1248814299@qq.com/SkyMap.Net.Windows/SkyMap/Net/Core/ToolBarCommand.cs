namespace SkyMap.Net.Core
{
    using SkyMap.Net.Gui;
    using System;
    using System.Windows.Forms;

    public class ToolBarCommand : ToolStripButton, IStatusUpdate
    {
        private object caller;
        private Codon codon;
        private bool isRunning = false;
        private ICommand menuCommand = null;

        public ToolBarCommand(Codon codon, object caller, bool createCommand)
        {
            this.RightToLeft = RightToLeft.Inherit;
            this.caller = caller;
            this.codon = codon;
            if (createCommand)
            {
                this.CreateCommand(codon, caller);
            }
            if ((this.Image == null) && codon.Properties.Contains("icon"))
            {
                this.Image = ResourceService.GetBitmap(codon.Properties["icon"]);
            }
            this.UpdateStatus();
            this.UpdateText();
        }

        public void AutoClick()
        {
            this.OnClick(new EventArgs());
        }

        private void CreateCommand(Codon codon, object caller)
        {
            this.menuCommand = (ICommand) codon.AddIn.CreateObject(codon.Properties["class"]);
            this.menuCommand.ID = codon.Id;
            this.menuCommand.Owner = caller;
            this.menuCommand.Codon = codon;
        }

        protected override void OnClick(EventArgs e)
        {
            if (!this.isRunning)
            {
                this.isRunning = true;
                try
                {
                    base.OnClick(e);
                    if (this.menuCommand == null)
                    {
                        this.CreateCommand(this.codon, this.caller);
                    }
                    if (this.menuCommand != null)
                    {
                        WaitDialogHelper.Show();
                        try
                        {
                            this.menuCommand.Run();
                        }
                        finally
                        {
                            WaitDialogHelper.Close();
                        }
                    }
                }
                finally
                {
                    this.UpdateStatus();
                    this.isRunning = false;
                }
            }
        }

        public virtual void UpdateStatus()
        {
            if (this.codon != null)
            {
                ConditionFailedAction failedAction = this.codon.GetFailedAction(this.caller, this.menuCommand);
                base.Visible = failedAction != ConditionFailedAction.Exclude;
                this.Enabled = failedAction != ConditionFailedAction.Disable;
                if (this.Enabled)
                {
                    try
                    {
                        if ((this.Enabled && (this.menuCommand != null)) && (this.menuCommand is ICheckableMenuCommand))
                        {
                            bool isChecked = ((ICheckableMenuCommand) this.menuCommand).IsChecked;
                            if (isChecked != base.Checked)
                            {
                                base.Checked = isChecked;
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        LoggingService.Error(exception);
                    }
                }
            }
        }

        public virtual void UpdateText()
        {
            if (this.codon != null)
            {
                this.Text = SkyMap.Net.Core.StringParser.Parse(this.codon.Properties["label"]);
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
    }
}

