namespace SkyMap.Net.Workflow.Engine
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class InvalidStatusException : WfException
    {
        public InvalidStatusException()
        {
        }

        public InvalidStatusException(string message) : base(message)
        {
        }

        protected InvalidStatusException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public InvalidStatusException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

