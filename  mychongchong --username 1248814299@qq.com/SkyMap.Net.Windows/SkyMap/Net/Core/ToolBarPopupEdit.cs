namespace SkyMap.Net.Core
{
    using DevExpress.XtraEditors;
    using SkyMap.Net.Gui.Components;
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    public class ToolBarPopupEdit : ToolStripControlHost, IStatusUpdate
    {
        private object caller;
        private Codon codon;
        private SkyMap.Net.Gui.Components.IContainerControl containerControl;
        private IPopupEditCommand menuCommand;
        private PopupContainerEdit popupContainerEdit;

        public ToolBarPopupEdit(Codon codon, object caller) : base(new PopupContainerEdit())
        {
            this.menuCommand = null;
            this.RightToLeft = RightToLeft.Inherit;
            this.caller = caller;
            this.codon = codon;
            this.popupContainerEdit = base.Control as PopupContainerEdit;
            PopupContainerControl control = new PopupContainerControl();
            this.popupContainerEdit.Properties.PopupControl = control;
            this.popupContainerEdit.Properties.CloseOnLostFocus = false;
            this.popupContainerEdit.Properties.CloseOnOuterMouseClick = false;
            control.ControlAdded += new ControlEventHandler(this.popupControl_ControlAdded);
            this.popupContainerEdit.QueryPopUp += new CancelEventHandler(this.popupContainerEdit_QueryPopUp);
            this.popupContainerEdit.QueryCloseUp += new CancelEventHandler(this.popupContainerEdit_QueryCloseUp);
            try
            {
                this.CreateCommand(codon);
            }
            catch (Exception exception)
            {
                LoggingService.Error(exception);
            }
            this.UpdateStatus();
            this.UpdateText();
        }

        private void ContainerControlDblClick(object sender, EventArgs e)
        {
            this.popupContainerEdit.ClosePopup();
        }

        private void CreateCommand(Codon codon)
        {
            this.menuCommand = (IPopupEditCommand) codon.AddIn.CreateObject(codon.Properties["class"]);
            if (this.menuCommand != null)
            {
                this.menuCommand.ID = codon.Id;
                this.menuCommand.Owner = this;
                this.menuCommand.Codon = codon;
            }
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (this.menuCommand == null)
            {
                this.CreateCommand(this.codon);
            }
            if (this.menuCommand != null)
            {
                this.menuCommand.Run();
            }
        }

        private void popupContainerEdit_QueryCloseUp(object sender, CancelEventArgs e)
        {
            this.Visible = false;
            if (this.containerControl == null)
            {
                LoggingService.DebugFormatted("尚没有设置好下拉控件...", new object[0]);
                this.Visible = true;
            }
            else
            {
                this.popupContainerEdit.EditValue = this.containerControl.QueryValue();
                if (LoggingService.IsDebugEnabled)
                {
                    LoggingService.DebugFormatted("下拉弹出框设定的值是:{0}", new object[] { this.popupContainerEdit.EditValue });
                }
                this.Visible = true;
            }
        }

        private void popupContainerEdit_QueryPopUp(object sender, CancelEventArgs e)
        {
        }

        private void popupControl_ControlAdded(object sender, ControlEventArgs e)
        {
            this.containerControl = e.Control as SkyMap.Net.Gui.Components.IContainerControl;
            e.Control.DoubleClick += new EventHandler(this.ContainerControlDblClick);
            this.popupContainerEdit.EditValue = this.containerControl.QueryValue();
        }

        public virtual void UpdateStatus()
        {
            if (this.codon != null)
            {
                ConditionFailedAction failedAction = this.codon.GetFailedAction(this.caller, this.menuCommand);
                this.Visible = failedAction != ConditionFailedAction.Exclude;
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

        public object EditValue
        {
            get
            {
                return this.popupContainerEdit.EditValue;
            }
            set
            {
                this.popupContainerEdit.EditValue = value;
            }
        }

        public PopupContainerControl PopupControl
        {
            get
            {
                return (base.Control as PopupContainerEdit).Properties.PopupControl;
            }
        }

        public bool Visible
        {
            get
            {
                return this.popupContainerEdit.Visible;
            }
            set
            {
                base.Visible = value;
                base.Control.Visible = value;
                this.popupContainerEdit.Visible = value;
            }
        }
    }
}

