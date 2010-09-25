namespace SkyMap.Net.Workflow.Engine
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class AlreadyCompleteException : WfException
    {
        public AlreadyCompleteException()
        {
        }

        public AlreadyCompleteException(string message) : base(message)
        {
        }

        protected AlreadyCompleteException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public AlreadyCompleteException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

