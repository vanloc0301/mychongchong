namespace SkyMap.Net.POP3
{
    using System;

    public class Pop3LoginException : Exception
    {
        private string m_exceptionString;

        public Pop3LoginException()
        {
            this.m_exceptionString = null;
        }

        public Pop3LoginException(string exceptionString)
        {
            this.m_exceptionString = exceptionString;
        }

        public Pop3LoginException(string exceptionString, Exception ex) : base(exceptionString, ex)
        {
        }

        public override string ToString()
        {
            return this.m_exceptionString;
        }
    }
}

