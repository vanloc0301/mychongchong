namespace SkyMap.Net.DataForms
{
    using SkyMap.Net.Core;
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class DataInputException : CoreException
    {
        public DataInputException()
        {
        }

        public DataInputException(string message) : base(message)
        {
        }

        protected DataInputException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public DataInputException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

