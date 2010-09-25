namespace SkyMap.Net.Workflow.Engine
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class CannotSuspentException : WfException
    {
        public CannotSuspentException()
        {
        }

        public CannotSuspentException(string message) : base(message)
        {
        }

        protected CannotSuspentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public CannotSuspentException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

