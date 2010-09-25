namespace LiteLib
{
    using System;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    public class BrowseForFolderClass : FolderNameEditor
    {
        private FolderNameEditor.FolderBrowser browser = new FolderNameEditor.FolderBrowser();

        public DialogResult ShowDialog()
        {
            return this.browser.ShowDialog();
        }

        public string DirectoryPath
        {
            get
            {
                return this.browser.DirectoryPath;
            }
        }

        public string Title
        {
            set
            {
                this.browser.Description = value;
            }
        }
    }
}

