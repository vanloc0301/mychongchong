namespace SkyMap.Net.Core
{
    using System;

    public class FileEventArgs : EventArgs
    {
        private string fileName = null;
        private bool isDirectory;

        public FileEventArgs(string fileName, bool isDirectory)
        {
            this.fileName = fileName;
            this.isDirectory = isDirectory;
        }

        public string FileName
        {
            get
            {
                return this.fileName;
            }
        }

        public bool IsDirectory
        {
            get
            {
                return this.isDirectory;
            }
        }
    }
}

