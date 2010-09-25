namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using System;
    using System.Runtime.CompilerServices;

    public abstract class AbstractWizardPanel : AbstractOptionPanel, IWizardPanel, IDialogPanel
    {
        private bool enableCancel = true;
        private bool enableNext = true;
        private bool enablePrevious = true;
        private bool isLastPanel = false;
        private string nextWizardPanelID = string.Empty;

        public event EventHandler EnableCancelChanged;

        public event EventHandler EnableNextChanged;

        public event EventHandler EnablePreviousChanged;

        public event EventHandler FinishPanelRequested;

        public event EventHandler IsLastPanelChanged;

        public event EventHandler NextWizardPanelIDChanged;

        protected virtual void FinishPanel()
        {
            if (this.FinishPanelRequested != null)
            {
                this.FinishPanelRequested(this, EventArgs.Empty);
            }
        }

        protected virtual void OnEnableCancelChanged(EventArgs e)
        {
            if (this.EnableCancelChanged != null)
            {
                this.EnableCancelChanged(this, e);
            }
        }

        protected virtual void OnEnableNextChanged(EventArgs e)
        {
            if (this.EnableNextChanged != null)
            {
                this.EnableNextChanged(this, e);
            }
        }

        protected virtual void OnEnablePreviousChanged(EventArgs e)
        {
            if (this.EnablePreviousChanged != null)
            {
                this.EnablePreviousChanged(this, e);
            }
        }

        protected virtual void OnIsLastPanelChanged(EventArgs e)
        {
            if (this.IsLastPanelChanged != null)
            {
                this.IsLastPanelChanged(this, e);
            }
        }

        protected virtual void OnNextWizardPanelIDChanged(EventArgs e)
        {
            if (this.NextWizardPanelIDChanged != null)
            {
                this.NextWizardPanelIDChanged(this, e);
            }
        }

        public bool EnableCancel
        {
            get
            {
                return this.enableCancel;
            }
            set
            {
                if (this.enableCancel != value)
                {
                    this.enableCancel = value;
                    this.OnEnableCancelChanged(EventArgs.Empty);
                }
            }
        }

        public bool EnableNext
        {
            get
            {
                return this.enableNext;
            }
            set
            {
                if (this.enableNext != value)
                {
                    this.enableNext = value;
                    this.OnEnableNextChanged(EventArgs.Empty);
                }
            }
        }

        public bool EnablePrevious
        {
            get
            {
                return this.enablePrevious;
            }
            set
            {
                if (this.enablePrevious != value)
                {
                    this.enablePrevious = value;
                    this.OnEnablePreviousChanged(EventArgs.Empty);
                }
            }
        }

        public bool IsLastPanel
        {
            get
            {
                return this.isLastPanel;
            }
            set
            {
                if (this.isLastPanel != value)
                {
                    this.isLastPanel = value;
                    this.OnIsLastPanelChanged(EventArgs.Empty);
                }
            }
        }

        public string NextWizardPanelID
        {
            get
            {
                return this.nextWizardPanelID;
            }
            set
            {
                if (this.nextWizardPanelID != value)
                {
                    this.nextWizardPanelID = value;
                    this.OnNextWizardPanelIDChanged(EventArgs.Empty);
                }
            }
        }
    }
}

