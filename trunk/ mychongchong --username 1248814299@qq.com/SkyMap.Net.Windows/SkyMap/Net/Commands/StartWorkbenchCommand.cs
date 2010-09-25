namespace SkyMap.Net.Commands
{
    using SkyMap.Net.Core;
    using SkyMap.Net.Gui;
    using System;
    using System.Windows.Forms;

    public class StartWorkbenchCommand
    {
        private const string workbenchMemento = "WorkbenchMemento";

        public void Run(string[] fileList)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            Form workbench = (Form) WorkbenchSingleton.Workbench;
            workbench.Show();
            Application.Idle += new EventHandler(this.ShowTipOfTheDay);
            if (true)
            {
                foreach (ICommand command in AddInTree.BuildItems("/Workspace/AutostartNothingLoaded", null, false))
                {
                    command.Run();
                }
            }
            workbench.Focus();
            Application.AddMessageFilter(new FormKeyHandler());
            Application.Run(workbench);
            try
            {
                PropertyService.Set<Properties>("WorkbenchMemento", WorkbenchSingleton.Workbench.CreateMemento());
            }
            catch (Exception exception)
            {
                MessageService.ShowError(exception, "Exception while saving workbench state.");
            }
        }

        private void ShowTipOfTheDay(object sender, EventArgs e)
        {
            Application.Idle -= new EventHandler(this.ShowTipOfTheDay);
            if (PropertyService.Get<bool>("ShowTipsAtStartup", true))
            {
                new ViewTipOfTheDay().Run();
            }
        }

        private class FormKeyHandler : IMessageFilter
        {
            private const int keyPressedMessage = 0x100;
            private string oldLayout = "Default";

            private bool PadHasFocus()
            {
                foreach (PadDescriptor descriptor in WorkbenchSingleton.Workbench.PadContentCollection)
                {
                    if (descriptor.HasFocus)
                    {
                        return true;
                    }
                }
                return false;
            }

            public bool PreFilterMessage(ref Message m)
            {
                if (m.Msg == 0x100)
                {
                    switch ((((Keys) m.WParam.ToInt32()) | Control.ModifierKeys))
                    {
                        case Keys.Escape:
                            if (!(!this.PadHasFocus() || MenuService.IsContextMenuOpen))
                            {
                                this.SelectActiveWorkbenchWindow();
                                return true;
                            }
                            return false;

                        case (Keys.Shift | Keys.Escape):
                            if (LayoutConfiguration.CurrentLayoutName == "Plain")
                            {
                                LayoutConfiguration.CurrentLayoutName = this.oldLayout;
                            }
                            else
                            {
                                WorkbenchSingleton.Workbench.WorkbenchLayout.StoreConfiguration();
                                this.oldLayout = LayoutConfiguration.CurrentLayoutName;
                                LayoutConfiguration.CurrentLayoutName = "Plain";
                            }
                            this.SelectActiveWorkbenchWindow();
                            return true;
                    }
                }
                return false;
            }

            private void SelectActiveWorkbenchWindow()
            {
                if (((WorkbenchSingleton.Workbench.ActiveWorkbenchWindow != null) && !WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.ActiveViewContent.Control.ContainsFocus) && (Form.ActiveForm == WorkbenchSingleton.MainForm))
                {
                    WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.ActiveViewContent.Control.Focus();
                }
            }
        }
    }
}

