namespace SkyMap.Net.Workflow.Engine
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class NotRunningException : WfException
    {
        public NotRunningException()
        {
        }

        public NotRunningException(string message) : base(message)
        {
        }

        protected NotRunningException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public NotRunningException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

