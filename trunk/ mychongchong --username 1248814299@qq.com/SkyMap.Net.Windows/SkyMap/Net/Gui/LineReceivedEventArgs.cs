namespace SkyMap.Net.Gui
{
    using System;

    public class LineReceivedEventArgs : EventArgs
    {
        private string line = string.Empty;

        public LineReceivedEventArgs(string line)
        {
            this.line = line;
        }

        public string Line
        {
            get
            {
                return this.line;
            }
        }
    }
}

