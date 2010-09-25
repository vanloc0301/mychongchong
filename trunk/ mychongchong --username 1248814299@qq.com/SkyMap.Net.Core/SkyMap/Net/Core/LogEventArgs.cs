namespace SkyMap.Net.Core
{
    using System;

    public class LogEventArgs : EventArgs
    {
        private System.Exception exception;
        private SkyMap.Net.Core.LogLevel level;
        private string member;
        private string message;
        private string source;

        public LogEventArgs()
        {
            this.level = SkyMap.Net.Core.LogLevel.Info;
            this.message = "";
            this.source = "";
            this.member = "";
            this.exception = null;
        }

        public LogEventArgs(string source, string sourceMember, string message, SkyMap.Net.Core.LogLevel level, System.Exception ex)
        {
            this.level = SkyMap.Net.Core.LogLevel.Info;
            this.message = "";
            this.source = "";
            this.member = "";
            this.exception = null;
            this.member = sourceMember;
            this.level = level;
            this.message = message;
            this.exception = ex;
            this.source = source;
        }

        public System.Exception Exception
        {
            get
            {
                return this.exception;
            }
        }

        public SkyMap.Net.Core.LogLevel Level
        {
            get
            {
                return this.level;
            }
        }

        public string Member
        {
            get
            {
                return this.member;
            }
        }

        public string Message
        {
            get
            {
                return this.message;
            }
        }

        public string Source
        {
            get
            {
                return this.source;
            }
        }
    }
}

