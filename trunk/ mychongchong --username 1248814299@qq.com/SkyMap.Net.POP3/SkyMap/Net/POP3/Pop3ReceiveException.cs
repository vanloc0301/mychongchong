namespace SkyMap.Net.POP3
{
    using System;

    public class Pop3ReceiveException : Exception
    {
        private string m_exceptionString;

        public Pop3ReceiveException()
        {
            this.m_exceptionString = null;
        }

        public Pop3ReceiveException(string exceptionString)
        {
            this.m_exceptionString = exceptionString;
        }

        public Pop3ReceiveException(string exceptionString, Exception ex) : base(exceptionString, ex)
        {
        }

        public override string ToString()
        {
            return this.m_exceptionString;
        }
    }
}

