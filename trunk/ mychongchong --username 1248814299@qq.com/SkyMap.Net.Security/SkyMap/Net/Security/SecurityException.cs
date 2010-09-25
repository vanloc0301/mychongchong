namespace SkyMap.Net.Security
{
    using SkyMap.Net.Core;
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class SecurityException : CoreException
    {
        public SecurityException()
        {
        }

        public SecurityException(string message) : base(message)
        {
        }

        protected SecurityException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public SecurityException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

