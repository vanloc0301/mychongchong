namespace SkyMap.Net.Workflow.Engine
{
    using System;
    using System.Runtime.Serialization;

    public class AlreadyRunningException : WfException
    {
        public AlreadyRunningException()
        {
        }

        public AlreadyRunningException(string message) : base(message)
        {
        }

        protected AlreadyRunningException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public AlreadyRunningException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

