namespace SkyMap.Net.Core
{
    using DevExpress.XtraEditors;
    using System;
    using System.Windows.Forms;

    public class ToolBarCalcEdit : ToolStripControlHost, IStatusUpdate
    {
        private CalcEdit calcEdit;
        private object caller;
        private Codon codon;
        private ISpinEditCommand menuCommand;

        public ToolBarCalcEdit(Codon codon, object caller) : base(new PopupContainerControl())
        {
            this.menuCommand = null;
            this.RightToLeft = RightToLeft.Inherit;
            this.caller = caller;
            this.codon = codon;
            try
            {
                if (codon.Properties.Contains("class"))
                {
                    this.menuCommand = (ISpinEditCommand) codon.AddIn.CreateObject(codon.Properties["class"]);
                    if (this.menuCommand != null)
                    {
                        this.menuCommand.ID = codon.Id;
                        this.menuCommand.Owner = this;
                        this.menuCommand.Codon = codon;
                    }
                }
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
            }
            this.calcEdit = new CalcEdit();
            this.calcEdit.Width = 200;
            this.calcEdit.EditValue = 1;
            this.calcEdit.Text = "计算器";
            base.Control.Controls.Add(this.calcEdit);
            this.UpdateStatus();
            this.UpdateText();
        }

        public void UpdateStatus()
        {
            if (this.codon != null)
            {
                ConditionFailedAction failedAction = this.codon.GetFailedAction(this.caller, this.menuCommand);
                base.Visible = failedAction != ConditionFailedAction.Exclude;
                bool flag = failedAction != ConditionFailedAction.Disable;
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
    }
}

