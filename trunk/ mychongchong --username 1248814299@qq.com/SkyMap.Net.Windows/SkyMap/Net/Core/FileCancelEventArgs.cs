namespace SkyMap.Net.Core
{
    using System;

    public class FileCancelEventArgs : FileEventArgs
    {
        private bool cancel;
        private bool operationAlreadyDone;

        public FileCancelEventArgs(string fileName, bool isDirectory) : base(fileName, isDirectory)
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

