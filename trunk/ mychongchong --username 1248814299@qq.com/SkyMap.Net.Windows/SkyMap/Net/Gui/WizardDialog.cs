namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    public class WizardDialog : Form
    {
        private int activePanelNumber = 0;
        private Button backButton = new Button();
        private Button cancelButton = new Button();
        private CurrentPanelPanel curPanel = null;
        private Panel dialogPanel = new Panel();
        private EventHandler enableCancelChangedHandler;
        private EventHandler enableNextChangedHandler;
        private Button finishButton = new Button();
        private EventHandler finishPanelHandler;
        private Button helpButton = new Button();
        private Stack<int> idStack = new Stack<int>();
        private Label label1 = new Label();
        private Button nextButton = new Button();
        private EventHandler nextWizardPanelIDChangedHandler;
        private StatusPanel statusPanel = null;
        private List<IDialogPanelDescriptor> wizardPanels = new List<IDialogPanelDescriptor>();

        public WizardDialog(string title, object customizer, string treePath)
        {
            AddInTreeNode treeNode = AddInTree.GetTreeNode(treePath);
            this.Text = title;
            if (treeNode != null)
            {
                this.AddNodes(customizer, new List<IDialogPanelDescriptor>((IDialogPanelDescriptor[]) treeNode.BuildChildItems(this).ToArray(typeof(IDialogPanelDescriptor))));
            }
            this.InitializeComponents();
            this.enableNextChangedHandler = new EventHandler(this.EnableNextChanged);
            this.nextWizardPanelIDChangedHandler = new EventHandler(this.NextWizardPanelIDChanged);
            this.enableCancelChangedHandler = new EventHandler(this.EnableCancelChanged);
            this.finishPanelHandler = new EventHandler(this.FinishPanelEvent);
            this.ActivatePanel(0);
        }

        private void ActivatePanel(int number)
        {
            if (this.CurrentWizardPane != null)
            {
                this.CurrentWizardPane.EnableNextChanged -= this.enableNextChangedHandler;
                this.CurrentWizardPane.EnableCancelChanged -= this.enableCancelChangedHandler;
                this.CurrentWizardPane.EnablePreviousChanged -= this.enableNextChangedHandler;
                this.CurrentWizardPane.NextWizardPanelIDChanged -= this.nextWizardPanelIDChangedHandler;
                this.CurrentWizardPane.IsLastPanelChanged -= this.nextWizardPanelIDChangedHandler;
                this.CurrentWizardPane.FinishPanelRequested -= this.finishPanelHandler;
            }
            this.activePanelNumber = number;
            if (this.CurrentWizardPane != null)
            {
                this.CurrentWizardPane.EnableNextChanged += this.enableNextChangedHandler;
                this.CurrentWizardPane.EnableCancelChanged += this.enableCancelChangedHandler;
                this.CurrentWizardPane.EnablePreviousChanged += this.enableNextChangedHandler;
                this.CurrentWizardPane.NextWizardPanelIDChanged += this.nextWizardPanelIDChangedHandler;
                this.CurrentWizardPane.IsLastPanelChanged += this.nextWizardPanelIDChangedHandler;
                this.CurrentWizardPane.FinishPanelRequested += this.finishPanelHandler;
            }
            this.EnableNextChanged(null, null);
            this.NextWizardPanelIDChanged(null, null);
            this.EnableCancelChanged(null, null);
            this.statusPanel.Refresh();
            this.curPanel.Refresh();
            this.dialogPanel.Controls.Clear();
            Control control = this.CurrentWizardPane.Control;
            control.Dock = DockStyle.Fill;
            this.dialogPanel.Controls.Add(control);
        }

        private void AddNodes(object customizer, List<IDialogPanelDescriptor> dialogPanelDescriptors)
        {
            foreach (IDialogPanelDescriptor descriptor in dialogPanelDescriptors)
            {
                if (descriptor.DialogPanel != null)
                {
                    descriptor.DialogPanel.EnableFinishChanged += new EventHandler(this.CheckFinishedState);
                    descriptor.DialogPanel.CustomizationObject = customizer;
                    this.wizardPanels.Add(descriptor);
                }
                if (descriptor.ChildDialogPanelDescriptors != null)
                {
                    this.AddNodes(customizer, descriptor.ChildDialogPanelDescriptors);
                }
            }
        }

        private void CancelEvent(object sender, EventArgs e)
        {
            foreach (IDialogPanelDescriptor descriptor in this.wizardPanels)
            {
                if (!descriptor.DialogPanel.ReceiveDialogMessage(DialogMessage.Cancel))
                {
                    return;
                }
            }
            base.DialogResult = DialogResult.Cancel;
        }

        private void CheckFinishedState(object sender, EventArgs e)
        {
            this.finishButton.Enabled = this.CanFinish;
        }

        private void EnableCancelChanged(object sender, EventArgs e)
        {
            this.cancelButton.Enabled = this.CurrentWizardPane.EnableCancel;
        }

        private void EnableNextChanged(object sender, EventArgs e)
        {
            this.nextButton.Enabled = this.CurrentWizardPane.EnableNext && (this.GetSuccessorNumber(this.activePanelNumber) < this.wizardPanels.Count);
            this.backButton.Enabled = this.CurrentWizardPane.EnablePrevious && (this.idStack.Count > 0);
        }

        private void FinishEvent(object sender, EventArgs e)
        {
            foreach (IDialogPanelDescriptor descriptor in this.wizardPanels)
            {
                if (!descriptor.DialogPanel.ReceiveDialogMessage(DialogMessage.Finish))
                {
                    return;
                }
            }
            base.DialogResult = DialogResult.OK;
        }

        private void FinishPanelEvent(object sender, EventArgs e)
        {
            AbstractWizardPanel currentWizardPane = (AbstractWizardPanel) this.CurrentWizardPane;
            bool isLastPanel = currentWizardPane.IsLastPanel;
            currentWizardPane.IsLastPanel = false;
            this.ShowNextPanelEvent(sender, e);
            currentWizardPane.IsLastPanel = isLastPanel;
        }

        private int GetPanelNumber(string id)
        {
            for (int i = 0; i < this.wizardPanels.Count; i++)
            {
                IDialogPanelDescriptor descriptor = this.wizardPanels[i];
                if (descriptor.ID == id)
                {
                    return i;
                }
            }
            return -1;
        }

        public int GetSuccessorNumber(int curNr)
        {
            IWizardPanel dialogPanel = (IWizardPanel) this.wizardPanels[curNr].DialogPanel;
            if (dialogPanel.IsLastPanel)
            {
                return (this.wizardPanels.Count + 1);
            }
            int panelNumber = this.GetPanelNumber(dialogPanel.NextWizardPanelID);
            if (panelNumber < 0)
            {
                return (curNr + 1);
            }
            return panelNumber;
        }

        private void HelpEvent(object sender, EventArgs e)
        {
            this.CurrentWizardPane.ReceiveDialogMessage(DialogMessage.Help);
        }

        private void InitializeComponents()
        {
            base.SuspendLayout();
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MinimizeBox = base.MaximizeBox = false;
            base.Icon = null;
            base.ClientSize = new Size(640, 440);
            int width = 0x5c;
            int y = 0x19c;
            int x = (base.Width - ((width + 4) * 4)) - 4;
            this.label1.Size = new Size(base.Width - 4, 1);
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new Point(2, 0x192);
            this.label1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            base.Controls.Add(this.label1);
            this.backButton.Text = ResourceService.GetString("Global.BackButtonText");
            this.backButton.Location = new Point(x, y);
            this.backButton.ClientSize = new Size(width, 0x1a);
            this.backButton.Click += new EventHandler(this.ShowPrevPanelEvent);
            this.backButton.FlatStyle = FlatStyle.System;
            this.backButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            base.Controls.Add(this.backButton);
            this.nextButton.Text = ResourceService.GetString("Global.NextButtonText");
            this.nextButton.Location = new Point((x + width) + 4, y);
            this.nextButton.ClientSize = new Size(width, 0x1a);
            this.nextButton.Click += new EventHandler(this.ShowNextPanelEvent);
            this.nextButton.FlatStyle = FlatStyle.System;
            this.nextButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            base.Controls.Add(this.nextButton);
            this.finishButton.Text = ResourceService.GetString("Dialog.WizardDialog.FinishButton");
            this.finishButton.Location = new Point(x + (2 * (width + 4)), y);
            this.finishButton.ClientSize = new Size(width, 0x1a);
            this.finishButton.Click += new EventHandler(this.FinishEvent);
            this.finishButton.FlatStyle = FlatStyle.System;
            this.finishButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            base.Controls.Add(this.finishButton);
            this.cancelButton.Text = ResourceService.GetString("Global.CancelButtonText");
            this.cancelButton.Location = new Point(x + (3 * (width + 4)), y);
            this.cancelButton.ClientSize = new Size(width, 0x1a);
            this.cancelButton.Click += new EventHandler(this.CancelEvent);
            this.cancelButton.FlatStyle = FlatStyle.System;
            this.cancelButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            base.Controls.Add(this.cancelButton);
            this.statusPanel = new StatusPanel(this);
            this.statusPanel.Location = new Point(2, 2);
            this.statusPanel.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            base.Controls.Add(this.statusPanel);
            this.curPanel = new CurrentPanelPanel(this);
            this.curPanel.Location = new Point(200, 2);
            this.curPanel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            base.Controls.Add(this.curPanel);
            this.dialogPanel.Location = new Point(200, 0x1b);
            this.dialogPanel.Size = new Size((base.Width - 8) - this.statusPanel.Bounds.Right, this.label1.Location.Y - this.dialogPanel.Location.Y);
            this.dialogPanel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            base.Controls.Add(this.dialogPanel);
            base.ResumeLayout(true);
        }

        private void NextWizardPanelIDChanged(object sender, EventArgs e)
        {
            this.EnableNextChanged(null, null);
            this.finishButton.Enabled = this.CanFinish;
            this.statusPanel.Refresh();
        }

        private void ShowNextPanelEvent(object sender, EventArgs e)
        {
            int successorNumber = this.GetSuccessorNumber(this.ActivePanelNumber);
            if (this.CurrentWizardPane.ReceiveDialogMessage(DialogMessage.Next))
            {
                this.idStack.Push(this.activePanelNumber);
                this.ActivatePanel(successorNumber);
                this.CurrentWizardPane.ReceiveDialogMessage(DialogMessage.Activated);
            }
        }

        private void ShowPrevPanelEvent(object sender, EventArgs e)
        {
            if (this.CurrentWizardPane.ReceiveDialogMessage(DialogMessage.Prev))
            {
                this.ActivatePanel(this.idStack.Pop());
            }
        }

        public int ActivePanelNumber
        {
            get
            {
                return this.activePanelNumber;
            }
        }

        private bool CanFinish
        {
            get
            {
                for (int i = 0; i < this.wizardPanels.Count; i = this.GetSuccessorNumber(i))
                {
                    IDialogPanelDescriptor descriptor = this.wizardPanels[i];
                    if (!descriptor.DialogPanel.EnableFinish)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public IWizardPanel CurrentWizardPane
        {
            get
            {
                return (IWizardPanel) this.wizardPanels[this.activePanelNumber].DialogPanel;
            }
        }

        public List<IDialogPanelDescriptor> WizardPanels
        {
            get
            {
                return this.wizardPanels;
            }
        }
    }
}

