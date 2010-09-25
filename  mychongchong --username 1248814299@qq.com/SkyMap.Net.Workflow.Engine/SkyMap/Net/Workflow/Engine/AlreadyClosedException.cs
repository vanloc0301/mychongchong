namespace SkyMap.Net.Workflow.Engine
{
    using System;
    using System.Runtime.Serialization;

    public class AlreadyClosedException : WfException
    {
        public AlreadyClosedException()
        {
        }

        public AlreadyClosedException(string message) : base(message)
        {
        }

        protected AlreadyClosedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public AlreadyClosedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

