namespace SkyMap.Net.DAO
{
    using SkyMap.Net.Core;
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class DaoSynchronizationException : CoreException
    {
        public DaoSynchronizationException()
        {
        }

        public DaoSynchronizationException(string message) : base(message)
        {
        }

        protected DaoSynchronizationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public DaoSynchronizationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

