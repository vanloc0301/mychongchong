namespace SkyMap.Net.Core
{
    using System;

    public class FileRenamingEventArgs : FileRenameEventArgs
    {
        private bool cancel;
        private bool operationAlreadyDone;

        public FileRenamingEventArgs(string sourceFile, string targetFile, bool isDirectory) : base(sourceFile, targetFile, isDirectory)
        {
        }

        public bool Cancel
        {
            get
            {
                return this.cancel;
            }
            set
            {
                this.cancel = value;
            }
        }

        public bool OperationAlreadyDone
        {
            get
            {
                return this.operationAlreadyDone;
            }
            set
            {
                this.operationAlreadyDone = value;
            }
        }
    }
}

