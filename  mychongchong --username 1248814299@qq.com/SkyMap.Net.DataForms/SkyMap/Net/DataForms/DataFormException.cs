namespace SkyMap.Net.DataForms
{
    using SkyMap.Net.Core;
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class DataFormException : CoreException
    {
        public DataFormException()
        {
        }

        public DataFormException(string message) : base(message)
        {
        }

        protected DataFormException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public DataFormException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

