namespace SkyMap.Net.Workflow.Client
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class CancelExecuteException : WfClientException
    {
        public CancelExecuteException()
        {
        }

        public CancelExecuteException(string message) : base(message)
        {
        }

        protected CancelExecuteException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public CancelExecuteException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

