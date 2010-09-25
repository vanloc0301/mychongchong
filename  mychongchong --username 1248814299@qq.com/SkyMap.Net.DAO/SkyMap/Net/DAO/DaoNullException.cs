namespace SkyMap.Net.DAO
{
    using SkyMap.Net.Core;
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class DaoNullException : CoreException
    {
        public DaoNullException()
        {
        }

        public DaoNullException(string message) : base(message)
        {
        }

        protected DaoNullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public DaoNullException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

