namespace SkyMap.Net.Workflow.Engine
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class CannotCompleteException : WfException
    {
        public CannotCompleteException()
        {
        }

        public CannotCompleteException(string message) : base(message)
        {
        }

        protected CannotCompleteException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public CannotCompleteException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

