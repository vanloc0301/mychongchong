namespace SkyMap.Net.Core
{
    using System;
    using System.Windows.Forms;

    public class ToolBarDateTimeEdit : ToolStripControlHost, IStatusUpdate, IEditValue
    {
        private object caller;
        private Codon codon;
        private DateTimePicker dtEdit;
        private AbstractDateTimeEditCommand menuCommand;

        public ToolBarDateTimeEdit(Codon codon, object caller) : this(codon, caller, new DateTimePicker())
        {
        }

        private ToolBarDateTimeEdit(Codon codon, object caller, DateTimePicker dtEdit) : base(dtEdit)
        {
            this.menuCommand = null;
            this.RightToLeft = RightToLeft.Inherit;
            this.caller = caller;
            this.codon = codon;
            this.dtEdit = dtEdit;
            if (LoggingService.IsDebugEnabled)
            {
                LoggingService.DebugFormatted("时间编辑器的时间是:{0}", new object[] { dtEdit.Value });
            }
            this.dtEdit.Width = Convert.ToInt32(codon.Properties["width"]);
            try
            {
                this.CreateCommand();
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
            }
            if (!this.menuCommand.EditValue.HasValue)
            {
                this.dtEdit.Value = DateTime.Now;
            }
            else
            {
                this.dtEdit.Value = this.menuCommand.EditValue.Value;
            }
            this.dtEdit.ValueChanged += new EventHandler(this.EditValueChanged);
            this.UpdateStatus();
            this.UpdateText();
        }

        private void CreateCommand()
        {
            this.menuCommand = (AbstractDateTimeEditCommand) this.codon.AddIn.CreateObject(this.codon.Properties["class"]);
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
                this.menuCommand.EditValue = new DateTime?(this.dtEdit.Value);
                this.menuCommand.Run();
            }
        }

        protected override void OnClick(EventArgs e)
        {
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

        public object EditValue
        {
            get
            {
                return this.dtEdit.Value;
            }
            set
            {
                if (value is DateTime)
                {
                    this.dtEdit.Value = Convert.ToDateTime(value);
                }
            }
        }
    }
}

