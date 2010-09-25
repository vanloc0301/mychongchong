namespace SkyMap.Net.Core
{
    using DevExpress.XtraEditors;
    using System;
    using System.Windows.Forms;

    public class ToolBarSpinEdit : ToolStripControlHost, IStatusUpdate
    {
        private object caller;
        private Codon codon;
        private ISpinEditCommand menuCommand;
        private SpinEdit spinEdit;

        public ToolBarSpinEdit(Codon codon, object caller) : base(new PopupContainerControl())
        {
            this.menuCommand = null;
            this.RightToLeft = RightToLeft.Inherit;
            this.caller = caller;
            this.codon = codon;
            this.spinEdit = new SpinEdit();
            if (!codon.Properties.Contains("width"))
            {
                this.spinEdit.Width = 40;
            }
            else
            {
                this.spinEdit.Width = Convert.ToInt32(codon.Properties["width"]);
            }
            this.spinEdit.EditValue = 1;
            this.spinEdit.EditValueChanged += new EventHandler(this.EditValueChanged);
            base.Control.Controls.Add(this.spinEdit);
            try
            {
                this.CreateCommand();
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
            }
            this.UpdateStatus();
            this.UpdateText();
        }

        private void CreateCommand()
        {
            this.menuCommand = (ISpinEditCommand) this.codon.AddIn.CreateObject(this.codon.Properties["class"]);
            this.menuCommand.ID = this.codon.Id;
            this.menuCommand.Owner = this;
            this.menuCommand.Codon = this.codon;
        }

        private void EditValueChanged(object sender, EventArgs e)
        {
            base.OnClick(e);
            if (this.menuCommand == null)
            {
                this.CreateCommand();
            }
            if (this.menuCommand != null)
            {
                this.menuCommand.Run();
            }
        }

        protected override void OnClick(EventArgs e)
        {
        }

        public void SetBound(int from, int to)
        {
            if (this.spinEdit != null)
            {
                this.spinEdit.Properties.MinValue = from;
                this.spinEdit.Properties.MaxValue = to;
                this.spinEdit.EditValue = from;
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
            if (this.codon != null)
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

        public decimal EditValue
        {
            get
            {
                return this.spinEdit.Value;
            }
        }
    }
}

