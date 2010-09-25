namespace SkyMap.Net.Gui
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    public class OutputReader
    {
        private string output = string.Empty;
        private StreamReader reader;
        private Thread thread;

        public event LineReceivedEventHandler LineReceived;

        public OutputReader(StreamReader reader)
        {
            this.reader = reader;
        }

        protected void OnLineReceived(string line)
        {
            if (this.LineReceived != null)
            {
                this.LineReceived(this, new LineReceivedEventArgs(line));
            }
        }

        private void ReadOutput()
        {
            this.output = string.Empty;
            StringBuilder builder = new StringBuilder();
            bool flag = false;
            while (!flag)
            {
                string str = this.reader.ReadLine();
                if (str != null)
                {
                    builder.Append(str);
                    builder.Append(Environment.NewLine);
                    this.OnLineReceived(str);
                }
                else
                {
                    flag = true;
                }
            }
            this.output = builder.ToString();
        }

        public void Start()
        {
            this.thread = new Thread(new ThreadStart(this.ReadOutput));
            this.thread.Start();
        }

        public void WaitForFinish()
        {
            if (this.thread != null)
            {
                this.thread.Join();
            }
        }

        public string Output
        {
            get
            {
                return this.output;
            }
        }
    }
}

