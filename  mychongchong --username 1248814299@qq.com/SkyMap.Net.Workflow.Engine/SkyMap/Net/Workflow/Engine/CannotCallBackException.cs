namespace SkyMap.Net.Workflow.Engine
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class CannotCallBackException : WfException
    {
        public CannotCallBackException()
        {
        }

        public CannotCallBackException(string message) : base(message)
        {
        }

        protected CannotCallBackException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public CannotCallBackException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

