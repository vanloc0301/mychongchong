namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using System;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    public class FolderDialog : FolderNameEditor
    {
        private string path;

        public DialogResult DisplayDialog(string description)
        {
            using (FolderNameEditor.FolderBrowser browser = new FolderNameEditor.FolderBrowser())
            {
                browser.Description = SkyMap.Net.Core.StringParser.Parse(description);
                DialogResult result = browser.ShowDialog(WorkbenchSingleton.MainForm);
                this.path = browser.DirectoryPath;
                LoggingService.Info("FolderDialog: user has choosen path " + this.path);
                if ((this.path == null) || (this.path.Length == 0))
                {
                    return DialogResult.Cancel;
                }
                return result;
            }
        }

        public string Path
        {
            get
            {
                return this.path;
            }
        }
    }
}

