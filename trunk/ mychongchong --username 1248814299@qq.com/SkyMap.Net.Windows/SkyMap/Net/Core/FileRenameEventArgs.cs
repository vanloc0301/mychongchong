namespace SkyMap.Net.Core
{
    using System;

    public class FileRenameEventArgs : EventArgs
    {
        private bool isDirectory;
        private string sourceFile = null;
        private string targetFile = null;

        public FileRenameEventArgs(string sourceFile, string targetFile, bool isDirectory)
        {
            this.sourceFile = sourceFile;
            this.targetFile = targetFile;
            this.isDirectory = isDirectory;
        }

        public bool IsDirectory
        {
            get
            {
                return this.isDirectory;
            }
        }

        public string SourceFile
        {
            get
            {
                return this.sourceFile;
            }
        }

        public string TargetFile
        {
            get
            {
                return this.targetFile;
            }
        }
    }
}

