namespace SkyMap.Net.Gui.OptionPanels
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using SkyMap.Net.Internal.ExternalTool;
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    public class ExternalToolPane : AbstractOptionPanel
    {
        private static string[,] argumentQuickInsertMenu = new string[,] { { "${res:Dialog.Options.ExternalTool.QuickInsertMenu.SharpDevelopStartupPath}", "${StartupPath}" } };
        private static string[] dependendControlNames = new string[] { "titleTextBox", "commandTextBox", "argumentTextBox", "workingDirTextBox", "promptArgsCheckBox", "useOutputPadCheckBox", "titleLabel", "argumentLabel", "commandLabel", "workingDirLabel", "browseButton", "argumentQuickInsertButton", "workingDirQuickInsertButton", "moveUpButton", "moveDownButton" };
        private static string[,] workingDirInsertMenu = new string[,] { { "${res:Dialog.Options.ExternalTool.QuickInsertMenu.SharpDevelopStartupPath}", "${StartupPath}" } };

        private void addEvent(object sender, EventArgs e)
        {
            ((ListBox) base.ControlDictionary["toolListBox"]).BeginUpdate();
            try
            {
                ((ListBox) base.ControlDictionary["toolListBox"]).Items.Add(new SkyMap.Net.Internal.ExternalTool.ExternalTool());
                ((ListBox) base.ControlDictionary["toolListBox"]).SelectedIndexChanged -= new EventHandler(this.selectEvent);
                ((ListBox) base.ControlDictionary["toolListBox"]).ClearSelected();
                ((ListBox) base.ControlDictionary["toolListBox"]).SelectedIndexChanged += new EventHandler(this.selectEvent);
                ((ListBox) base.ControlDictionary["toolListBox"]).SelectedIndex = ((ListBox) base.ControlDictionary["toolListBox"]).Items.Count - 1;
            }
            finally
            {
                ((ListBox) base.ControlDictionary["toolListBox"]).EndUpdate();
            }
        }

        private void browseEvent(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.CheckFileExists = true;
                dialog.Filter = SkyMap.Net.Core.StringParser.Parse("${res:FileFilter.ExecutableFiles}|*.exe;*.com;*.pif;*.bat;*.cmd|${res:FileFilter.AllFiles}|*.*");
                if (dialog.ShowDialog(WorkbenchSingleton.MainForm) == DialogResult.OK)
                {
                    base.ControlDictionary["commandTextBox"].Text = dialog.FileName;
                }
            }
        }

        public override void LoadPanelContents()
        {
            base.SetupFromXmlStream(base.GetType().Assembly.GetManifestResourceStream("Resources.ExternalToolOptions.xfrm"));
            ((ListBox) base.ControlDictionary["toolListBox"]).BeginUpdate();
            try
            {
                foreach (object obj2 in ToolLoader.Tool)
                {
                    ((ListBox) base.ControlDictionary["toolListBox"]).Items.Add(obj2);
                }
            }
            finally
            {
                ((ListBox) base.ControlDictionary["toolListBox"]).EndUpdate();
            }
            MenuService.CreateQuickInsertMenu((TextBox) base.ControlDictionary["argumentTextBox"], base.ControlDictionary["argumentQuickInsertButton"], argumentQuickInsertMenu);
            MenuService.CreateQuickInsertMenu((TextBox) base.ControlDictionary["workingDirTextBox"], base.ControlDictionary["workingDirQuickInsertButton"], workingDirInsertMenu);
            ((ListBox) base.ControlDictionary["toolListBox"]).SelectedIndexChanged += new EventHandler(this.selectEvent);
            base.ControlDictionary["removeButton"].Click += new EventHandler(this.removeEvent);
            base.ControlDictionary["addButton"].Click += new EventHandler(this.addEvent);
            base.ControlDictionary["moveUpButton"].Click += new EventHandler(this.moveUpEvent);
            base.ControlDictionary["moveDownButton"].Click += new EventHandler(this.moveDownEvent);
            base.ControlDictionary["browseButton"].Click += new EventHandler(this.browseEvent);
            this.selectEvent(this, EventArgs.Empty);
        }

        private void moveDownEvent(object sender, EventArgs e)
        {
            int selectedIndex = ((ListBox) base.ControlDictionary["toolListBox"]).SelectedIndex;
            if ((selectedIndex >= 0) && (selectedIndex < (((ListBox) base.ControlDictionary["toolListBox"]).Items.Count - 1)))
            {
                ((ListBox) base.ControlDictionary["toolListBox"]).SelectedIndexChanged -= new EventHandler(this.selectEvent);
                try
                {
                    object obj2 = ((ListBox) base.ControlDictionary["toolListBox"]).Items[selectedIndex + 1];
                    ((ListBox) base.ControlDictionary["toolListBox"]).Items[selectedIndex + 1] = ((ListBox) base.ControlDictionary["toolListBox"]).Items[selectedIndex];
                    ((ListBox) base.ControlDictionary["toolListBox"]).Items[selectedIndex] = obj2;
                    ((ListBox) base.ControlDictionary["toolListBox"]).SetSelected(selectedIndex, false);
                    ((ListBox) base.ControlDictionary["toolListBox"]).SetSelected(selectedIndex + 1, true);
                }
                finally
                {
                    ((ListBox) base.ControlDictionary["toolListBox"]).SelectedIndexChanged += new EventHandler(this.selectEvent);
                }
            }
        }

        private void moveUpEvent(object sender, EventArgs e)
        {
            int selectedIndex = ((ListBox) base.ControlDictionary["toolListBox"]).SelectedIndex;
            if (selectedIndex > 0)
            {
                ((ListBox) base.ControlDictionary["toolListBox"]).SelectedIndexChanged -= new EventHandler(this.selectEvent);
                try
                {
                    object obj2 = ((ListBox) base.ControlDictionary["toolListBox"]).Items[selectedIndex - 1];
                    ((ListBox) base.ControlDictionary["toolListBox"]).Items[selectedIndex - 1] = ((ListBox) base.ControlDictionary["toolListBox"]).Items[selectedIndex];
                    ((ListBox) base.ControlDictionary["toolListBox"]).Items[selectedIndex] = obj2;
                    ((ListBox) base.ControlDictionary["toolListBox"]).SetSelected(selectedIndex, false);
                    ((ListBox) base.ControlDictionary["toolListBox"]).SetSelected(selectedIndex - 1, true);
                }
                finally
                {
                    ((ListBox) base.ControlDictionary["toolListBox"]).SelectedIndexChanged += new EventHandler(this.selectEvent);
                }
            }
        }

        private void propertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            foreach (ListViewItem item in ((ListView) base.ControlDictionary["toolListView"]).Items)
            {
                if (item.Tag != null)
                {
                    item.Text = item.Tag.ToString();
                }
            }
        }

        private void removeEvent(object sender, EventArgs e)
        {
            ((ListBox) base.ControlDictionary["toolListBox"]).BeginUpdate();
            try
            {
                int selectedIndex = ((ListBox) base.ControlDictionary["toolListBox"]).SelectedIndex;
                object[] destination = new object[((ListBox) base.ControlDictionary["toolListBox"]).SelectedItems.Count];
                ((ListBox) base.ControlDictionary["toolListBox"]).SelectedItems.CopyTo(destination, 0);
                ((ListBox) base.ControlDictionary["toolListBox"]).SelectedIndexChanged -= new EventHandler(this.selectEvent);
                foreach (object obj2 in destination)
                {
                    ((ListBox) base.ControlDictionary["toolListBox"]).Items.Remove(obj2);
                }
                ((ListBox) base.ControlDictionary["toolListBox"]).SelectedIndexChanged += new EventHandler(this.selectEvent);
                if (((ListBox) base.ControlDictionary["toolListBox"]).Items.Count == 0)
                {
                    this.selectEvent(this, EventArgs.Empty);
                }
                else
                {
                    ((ListBox) base.ControlDictionary["toolListBox"]).SelectedIndex = Math.Min(selectedIndex, ((ListBox) base.ControlDictionary["toolListBox"]).Items.Count - 1);
                }
            }
            finally
            {
                ((ListBox) base.ControlDictionary["toolListBox"]).EndUpdate();
            }
        }

        private void selectEvent(object sender, EventArgs e)
        {
            base.SetEnabledStatus(((ListBox) base.ControlDictionary["toolListBox"]).SelectedItems.Count > 0, new string[] { "removeButton" });
            base.ControlDictionary["titleTextBox"].TextChanged -= new EventHandler(this.setToolValues);
            base.ControlDictionary["commandTextBox"].TextChanged -= new EventHandler(this.setToolValues);
            base.ControlDictionary["argumentTextBox"].TextChanged -= new EventHandler(this.setToolValues);
            base.ControlDictionary["workingDirTextBox"].TextChanged -= new EventHandler(this.setToolValues);
            ((CheckBox) base.ControlDictionary["promptArgsCheckBox"]).CheckedChanged -= new EventHandler(this.setToolValues);
            ((CheckBox) base.ControlDictionary["useOutputPadCheckBox"]).CheckedChanged -= new EventHandler(this.setToolValues);
            if (((ListBox) base.ControlDictionary["toolListBox"]).SelectedItems.Count == 1)
            {
                SkyMap.Net.Internal.ExternalTool.ExternalTool selectedItem = ((ListBox) base.ControlDictionary["toolListBox"]).SelectedItem as SkyMap.Net.Internal.ExternalTool.ExternalTool;
                base.SetEnabledStatus(true, dependendControlNames);
                base.ControlDictionary["titleTextBox"].Text = selectedItem.MenuCommand;
                base.ControlDictionary["commandTextBox"].Text = selectedItem.Command;
                base.ControlDictionary["argumentTextBox"].Text = selectedItem.Arguments;
                base.ControlDictionary["workingDirTextBox"].Text = selectedItem.InitialDirectory;
                ((CheckBox) base.ControlDictionary["promptArgsCheckBox"]).Checked = selectedItem.PromptForArguments;
                ((CheckBox) base.ControlDictionary["useOutputPadCheckBox"]).Checked = selectedItem.UseOutputPad;
            }
            else
            {
                base.SetEnabledStatus(false, dependendControlNames);
                base.ControlDictionary["titleTextBox"].Text = string.Empty;
                base.ControlDictionary["commandTextBox"].Text = string.Empty;
                base.ControlDictionary["argumentTextBox"].Text = string.Empty;
                base.ControlDictionary["workingDirTextBox"].Text = string.Empty;
                ((CheckBox) base.ControlDictionary["promptArgsCheckBox"]).Checked = false;
                ((CheckBox) base.ControlDictionary["useOutputPadCheckBox"]).Checked = false;
            }
            base.ControlDictionary["titleTextBox"].TextChanged += new EventHandler(this.setToolValues);
            base.ControlDictionary["commandTextBox"].TextChanged += new EventHandler(this.setToolValues);
            base.ControlDictionary["argumentTextBox"].TextChanged += new EventHandler(this.setToolValues);
            base.ControlDictionary["workingDirTextBox"].TextChanged += new EventHandler(this.setToolValues);
            ((CheckBox) base.ControlDictionary["promptArgsCheckBox"]).CheckedChanged += new EventHandler(this.setToolValues);
            ((CheckBox) base.ControlDictionary["useOutputPadCheckBox"]).CheckedChanged += new EventHandler(this.setToolValues);
        }

        private void setToolValues(object sender, EventArgs e)
        {
            SkyMap.Net.Internal.ExternalTool.ExternalTool selectedItem = ((ListBox) base.ControlDictionary["toolListBox"]).SelectedItem as SkyMap.Net.Internal.ExternalTool.ExternalTool;
            selectedItem.MenuCommand = base.ControlDictionary["titleTextBox"].Text;
            selectedItem.Command = base.ControlDictionary["commandTextBox"].Text;
            selectedItem.Arguments = base.ControlDictionary["argumentTextBox"].Text;
            selectedItem.InitialDirectory = base.ControlDictionary["workingDirTextBox"].Text;
            selectedItem.PromptForArguments = ((CheckBox) base.ControlDictionary["promptArgsCheckBox"]).Checked;
            selectedItem.UseOutputPad = ((CheckBox) base.ControlDictionary["useOutputPadCheckBox"]).Checked;
        }

        public override bool StorePanelContents()
        {
            List<SkyMap.Net.Internal.ExternalTool.ExternalTool> list = new List<SkyMap.Net.Internal.ExternalTool.ExternalTool>();
            foreach (SkyMap.Net.Internal.ExternalTool.ExternalTool tool in ((ListBox) base.ControlDictionary["toolListBox"]).Items)
            {
                if (!FileUtility.IsValidFileName(tool.Command))
                {
                    MessageService.ShowError(string.Format("The command of tool \"{0}\" is invalid.", tool.MenuCommand));
                    return false;
                }
                if (!(!(tool.InitialDirectory != "") || FileUtility.IsValidFileName(tool.InitialDirectory)))
                {
                    MessageService.ShowError(string.Format("The working directory of tool \"{0}\" is invalid.", tool.MenuCommand));
                    return false;
                }
                list.Add(tool);
            }
            ToolLoader.Tool = list;
            ToolLoader.SaveTools();
            return true;
        }
    }
}

