namespace SkyMap.Net.Workflow.Engine
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class CannotSendBackException : WfException
    {
        public CannotSendBackException()
        {
        }

        public CannotSendBackException(string message) : base(message)
        {
        }

        protected CannotSendBackException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public CannotSendBackException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

