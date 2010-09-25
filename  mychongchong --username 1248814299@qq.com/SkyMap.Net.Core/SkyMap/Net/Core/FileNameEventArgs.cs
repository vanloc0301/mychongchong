namespace SkyMap.Net.Core
{
    using System;

    public class FileNameEventArgs : EventArgs
    {
        private string fileName;

        public FileNameEventArgs(string fileName)
        {
            this.fileName = fileName;
        }

        public string FileName
        {
            get
            {
                return this.fileName;
            }
        }
    }
}

