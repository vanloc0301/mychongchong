namespace SkyMap.Net.Workflow.Engine
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class CannotChangeRequesterException : WfException
    {
        public CannotChangeRequesterException()
        {
        }

        public CannotChangeRequesterException(string message) : base(message)
        {
        }

        protected CannotChangeRequesterException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public CannotChangeRequesterException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

