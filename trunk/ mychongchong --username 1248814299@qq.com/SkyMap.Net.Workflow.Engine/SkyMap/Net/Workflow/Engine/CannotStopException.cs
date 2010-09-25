namespace SkyMap.Net.Workflow.Engine
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class CannotStopException : WfException
    {
        public CannotStopException()
        {
        }

        public CannotStopException(string message) : base(message)
        {
        }

        protected CannotStopException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public CannotStopException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

