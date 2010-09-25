namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui.XmlForms;
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public class TabbedOptions : BaseSharpDevelopForm
    {
        private List<IDialogPanel> OptionPanels = new List<IDialogPanel>();
        private Properties properties = null;

        public TabbedOptions(string dialogName, Properties properties, AddInTreeNode node)
        {
            this.properties = properties;
            base.SetupFromXmlStream(base.GetType().Assembly.GetManifestResourceStream("Resources.TabbedOptionsDialog.xfrm"));
            this.Text = dialogName;
            base.ControlDictionary["okButton"].Click += new EventHandler(this.AcceptEvent);
            base.Icon = null;
            base.Owner = (Form) WorkbenchSingleton.Workbench;
            this.AddOptionPanels(new List<IDialogPanelDescriptor>((IDialogPanelDescriptor[]) node.BuildChildItems(this).ToArray(typeof(IDialogPanelDescriptor))));
        }

        private void AcceptEvent(object sender, EventArgs e)
        {
            foreach (AbstractOptionPanel panel in this.OptionPanels)
            {
                if (!panel.ReceiveDialogMessage(DialogMessage.OK))
                {
                    return;
                }
            }
            base.DialogResult = DialogResult.OK;
        }

        private void AddOptionPanels(List<IDialogPanelDescriptor> dialogPanelDescriptors)
        {
            foreach (IDialogPanelDescriptor descriptor in dialogPanelDescriptors)
            {
                if (((descriptor != null) && (descriptor.DialogPanel != null)) && (descriptor.DialogPanel.Control != null))
                {
                    descriptor.DialogPanel.CustomizationObject = this.properties;
                    descriptor.DialogPanel.Control.Dock = DockStyle.Fill;
                    descriptor.DialogPanel.ReceiveDialogMessage(DialogMessage.Activated);
                    this.OptionPanels.Add(descriptor.DialogPanel);
                    TabPage page = new TabPage(descriptor.Label);
                    page.UseVisualStyleBackColor = true;
                    page.Controls.Add(descriptor.DialogPanel.Control);
                    ((TabControl) base.ControlDictionary["optionPanelTabControl"]).TabPages.Add(page);
                }
                if (descriptor.ChildDialogPanelDescriptors != null)
                {
                    this.AddOptionPanels(descriptor.ChildDialogPanelDescriptors);
                }
            }
        }
    }
}

