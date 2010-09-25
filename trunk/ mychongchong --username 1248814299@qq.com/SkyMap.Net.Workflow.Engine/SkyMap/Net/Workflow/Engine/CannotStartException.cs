namespace SkyMap.Net.Workflow.Engine
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class CannotStartException : WfException
    {
        public CannotStartException()
        {
        }

        public CannotStartException(string message) : base(message)
        {
        }

        protected CannotStartException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public CannotStartException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

