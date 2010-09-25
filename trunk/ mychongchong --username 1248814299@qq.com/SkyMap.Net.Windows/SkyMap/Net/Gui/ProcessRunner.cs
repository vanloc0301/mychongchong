namespace SkyMap.Net.Gui
{
    using SkyMap.Net.Core;
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class ProcessRunner : IDisposable
    {
        private Process process;
        private OutputReader standardErrorReader;
        private string standardOutput = string.Empty;
        private OutputReader standardOutputReader;
        private string workingDirectory = string.Empty;

        public event LineReceivedEventHandler ErrorLineReceived;

        public event LineReceivedEventHandler OutputLineReceived;

        public event EventHandler ProcessExited;

        public void Dispose()
        {
        }

        [DllImport("kernel32.dll", SetLastError=true)]
        private static extern int GenerateConsoleCtrlEvent(int dwCtrlEvent, int dwProcessGroupId);
        public void Kill()
        {
            if (this.process != null)
            {
                if (!this.process.HasExited)
                {
                    this.process.Kill();
                    this.process.Close();
                    this.process.Dispose();
                    this.process = null;
                    this.standardOutputReader.WaitForFinish();
                    this.standardErrorReader.WaitForFinish();
                }
                else
                {
                    this.process = null;
                }
            }
        }

        protected void OnErrorLineReceived(object sender, LineReceivedEventArgs e)
        {
            if (this.ErrorLineReceived != null)
            {
                this.ErrorLineReceived(this, e);
            }
        }

        protected void OnOutputLineReceived(object sender, LineReceivedEventArgs e)
        {
            if (this.OutputLineReceived != null)
            {
                this.OutputLineReceived(this, e);
            }
        }

        protected void OnProcessExited(object sender, EventArgs e)
        {
            if (this.ProcessExited != null)
            {
                this.standardOutputReader.WaitForFinish();
                this.standardErrorReader.WaitForFinish();
                this.ProcessExited(this, e);
            }
        }

        public void Start(string command)
        {
            this.Start(command, string.Empty);
        }

        public void Start(string command, string arguments)
        {
            this.process = new Process();
            this.process.StartInfo.CreateNoWindow = true;
            this.process.StartInfo.FileName = command;
            this.process.StartInfo.WorkingDirectory = this.workingDirectory;
            this.process.StartInfo.RedirectStandardOutput = true;
            this.process.StartInfo.RedirectStandardError = true;
            this.process.StartInfo.UseShellExecute = false;
            this.process.StartInfo.Arguments = arguments;
            if (this.ProcessExited != null)
            {
                this.process.EnableRaisingEvents = true;
                this.process.Exited += new EventHandler(this.OnProcessExited);
            }
            this.process.Start();
            this.standardOutputReader = new OutputReader(this.process.StandardOutput);
            if (this.OutputLineReceived != null)
            {
                this.standardOutputReader.LineReceived += new LineReceivedEventHandler(this.OnOutputLineReceived);
            }
            this.standardOutputReader.Start();
            this.standardErrorReader = new OutputReader(this.process.StandardError);
            if (this.ErrorLineReceived != null)
            {
                this.standardErrorReader.LineReceived += new LineReceivedEventHandler(this.OnErrorLineReceived);
            }
            this.standardErrorReader.Start();
        }

        public void WaitForExit()
        {
            this.WaitForExit(0x7fffffff);
        }

        public bool WaitForExit(int timeout)
        {
            if (this.process == null)
            {
                throw new ProcessRunnerException(SkyMap.Net.Core.StringParser.Parse("${res:SkyMap.Net.NAntAddIn.ProcessRunner.NoProcessRunningErrorText}"));
            }
            bool flag = this.process.WaitForExit(timeout);
            if (flag)
            {
                this.standardOutputReader.WaitForFinish();
                this.standardErrorReader.WaitForFinish();
            }
            return flag;
        }

        public int ExitCode
        {
            get
            {
                int exitCode = 0;
                if (this.process != null)
                {
                    exitCode = this.process.ExitCode;
                }
                return exitCode;
            }
        }

        public bool IsRunning
        {
            get
            {
                bool flag = false;
                if (this.process != null)
                {
                    flag = !this.process.HasExited;
                }
                return flag;
            }
        }

        public string StandardError
        {
            get
            {
                string output = string.Empty;
                if (this.standardErrorReader != null)
                {
                    output = this.standardErrorReader.Output;
                }
                return output;
            }
        }

        public string StandardOutput
        {
            get
            {
                string output = string.Empty;
                if (this.standardOutputReader != null)
                {
                    output = this.standardOutputReader.Output;
                }
                return output;
            }
        }

        public string WorkingDirectory
        {
            get
            {
                return this.workingDirectory;
            }
            set
            {
                this.workingDirectory = value;
            }
        }

        private enum ConsoleEvent
        {
            ControlC,
            ControlBreak
        }
    }
}

