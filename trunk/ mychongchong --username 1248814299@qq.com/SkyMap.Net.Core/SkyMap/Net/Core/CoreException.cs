namespace SkyMap.Net.Core
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class CoreException : ApplicationException
    {
        public CoreException()
        {
        }

        public CoreException(string message) : base(message)
        {
        }

        protected CoreException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public CoreException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

