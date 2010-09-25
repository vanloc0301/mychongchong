namespace SkyMap.Net.Workflow.Engine
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class RequesterRequiredException : WfException
    {
        public RequesterRequiredException()
        {
        }

        public RequesterRequiredException(string message) : base(message)
        {
        }

        public RequesterRequiredException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public RequesterRequiredException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

