namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;
    using System.Windows.Forms;

    public class OptionsCommand : AbstractMenuCommand
    {
        public override void Run()
        {
            using (TreeViewOptions options = new TreeViewOptions(PropertyService.Get<Properties>("SkyMap.Net.TextEditor.Document.Document.DefaultDocumentAggregatorProperties", new Properties()), AddInTree.GetTreeNode("/Dialogs/OptionsDialog")))
            {
                options.FormBorderStyle = FormBorderStyle.FixedDialog;
                options.Owner = (Form) WorkbenchSingleton.Workbench;
                options.ShowDialog(WorkbenchSingleton.MainForm);
            }
        }

        public static void ShowTabbedOptions(string dialogTitle, AddInTreeNode node)
        {
            TabbedOptions options = new TabbedOptions(dialogTitle, PropertyService.Get<Properties>("SkyMap.Net.TextEditor.Document.Document.DefaultDocumentAggregatorProperties", new Properties()), node);
            options.Width = 450;
            options.Height = 0x1a9;
            options.FormBorderStyle = FormBorderStyle.FixedDialog;
            options.ShowDialog(WorkbenchSingleton.MainForm);
            options.Dispose();
        }
    }
}

